using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class DBUpdate : Form
	{
		// DLL宣言
		[DllImport("user32.dll")]
		static extern Int32 FlashWindowEx(ref FLASHWINFO pwfi);

		[StructLayout(LayoutKind.Sequential)]
		public struct FLASHWINFO
		{
			public UInt32 cbSize;    // FLASHWINFO構造体のサイズ
			public IntPtr hwnd;      // 点滅対象のウィンドウ・ハンドル
			public UInt32 dwFlags;   // 以下の「FLASHW_XXX」のいずれか
			public UInt32 uCount;    // 点滅する回数
			public UInt32 dwTimeout; // 点滅する間隔（ミリ秒単位）
		}
		// 点滅を止める
		public const UInt32 FLASHW_STOP = 0;
		// タイトルバーを点滅させる
		public const UInt32 FLASHW_CAPTION = 1;
		// タスクバー・ボタンを点滅させる
		public const UInt32 FLASHW_TRAY = 2;
		// タスクバー・ボタンとタイトルバーを点滅させる
		public const UInt32 FLASHW_ALL = 3;
		// FLASHW_STOPが指定されるまでずっと点滅させる
		public const UInt32 FLASHW_TIMER = 4;
		// ウィンドウが最前面に来るまでずっと点滅させる
		public const UInt32 FLASHW_TIMERNOFG = 12;

		// 変数宣言
		private string saveType = "I";  // INIファイルをデフォルトとしてセット
		private string configIni = string.Empty;
		private SqlConnection con = new SqlConnection();
		private MySqlConnection con2 = new MySqlConnection();
		private StringBuilder errMsg = new StringBuilder();
		private StringBuilder updateLog = new StringBuilder();
		private bool updateRequired = false;
		private bool hasUpdate = false;
		private int updateStatus = 0;
		private string currentVersion = "1.0";
		private string latestVersion = "new";
		private int updatePhaseCount = 0;


		/// <summary>
		/// [必須] Ver.1.1(update to GL 1.03)
		/// </summary>
		private readonly StringBuilder v1_1 = new StringBuilder("[必須] Ver.1.1(Update to GL 1.03)")
		.AppendLine("")
		.AppendLine("\t* [ALTER TABLE] テーブルの文字コード変更／既存のカラムの文字コード変換(utf8mb4 / utf8mb4_general_ci)")
		.AppendLine("\t* [カラム追加] [DCON_IMG]:DiscordConnector カスタム画像用、[TEMP1]の値で更新")
		.AppendLine("\t* [カラム追加] [MEMO]:メモ用")
		.AppendLine("\t* [カラム追加] [STATUS]:ステータス用")
		.AppendLine("\t* [カラム追加] [DB_VERSION]:DBバージョン管理用")
		.AppendLine("\t* [UPDATE] [TEMP1]:NULL");


		/// <summary>
		/// MSSQL
		/// </summary>
		/// <param name="orgSaveType">ゲーム保存方法</param>
		/// <param name="cn">SQLコネクション</param>
		public DBUpdate(string orgSaveType, SqlConnection cn)
		{
			InitializeComponent();
			saveType = orgSaveType;
			con = cn;
		}

		/// <summary>
		/// MySQL
		/// </summary>
		/// <param name="orgSaveType">ゲーム保存方法</param>
		/// <param name="cn">SQLコネクション</param>
		public DBUpdate(string orgSaveType, MySqlConnection cn)
		{
			InitializeComponent();
			saveType = orgSaveType;
			con2 = cn;
		}

		/// <summary>
		/// INI
		/// </summary>
		/// <param name="orgSaveType">ゲーム保存方法</param>
		public DBUpdate(string orgSaveType)
		{
			InitializeComponent();
			saveType = orgSaveType;
		}

		private void DBUpdate_Load(object sender, EventArgs e)
		{
			glVersionLabel.Text = General.Var.AppVer;

			if (saveType == "D")
			{
				// MSSQL

				// [必須] Ver.1.1(update to GL 1.03)
				// [DCON_IMG] 存在チェック
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " DCON_IMG;"
				};
				cm.Connection = con;

				try
				{
					con.Open();
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [MEMO] 存在チェック
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " MEMO;"
					};
					cm.Connection = con;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [STATUS] 存在チェック
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " STATUS;"
					};
					cm.Connection = con;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [DB_VERSION] 存在チェック
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " DB_VERSION;"
					};
					cm.Connection = con;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | DESCRIBE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_1.ToString());
						latestVersion = "1.1";
						updateRequired = true;
						hasUpdate = false;
						updatePhaseCount++;
					}
					else
					{
						currentVersion = "1.0";
					}
				}

			}
			else if (saveType == "M")
			{
				// MySQL
				// [必須] Ver.1.1(update to GL 1.03)
				MySqlCommand cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"DESCRIBE " + General.Var.DbTable + " DCON_IMG;"
				};
				cm.Connection = con2;

				try
				{
					con2.Open();
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [MEMO] 存在チェック
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " MEMO;"
					};
					cm.Connection = con2;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [STATUS] 存在チェック
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " STATUS;"
					};
					cm.Connection = con2;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}

					// [DB_VERSION] 存在チェック
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DESCRIBE " + General.Var.DbName + "." + General.Var.DbTable + " DB_VERSION;"
					};
					cm.Connection = con2;
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在しない場合、アップデートを行う
						if (reader.Read() == false)
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | DESCRIBE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_1.ToString());
						latestVersion = "1.1";
						updateRequired = true;
						hasUpdate = false;
						updatePhaseCount++;
					}
					else
					{
						currentVersion = "1.0";
					}
				}
			}
			else
			{
				// INI
				// [必須] Ver.1.1(update to GL 1.03)
			}

			// アップデートチェック
			if (updateLog.ToString().Length > 0)
			{
				if (updateRequired)
				{
					// 必須アップデートあり
					updateRequiredLabel.Visible = true;
				}
				updateLogText.Text = updateLog.ToString();
				currentDBVersionLabel.Text = currentVersion;
				latestDBVersionLabel.Text = latestVersion;
				this.Show();
				this.Focus();
				System.Media.SystemSounds.Exclamation.Play();

				// ウィンドウ点滅
				FLASHWINFO fInfo = new FLASHWINFO();
				fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
				fInfo.hwnd = this.Handle;
				fInfo.dwFlags = FLASHW_TIMERNOFG;
				fInfo.uCount = 5;         // 点滅する回数
				fInfo.dwTimeout = 0;

				FlashWindowEx(ref fInfo);
			}

			// エラー表示
			if (errMsg.ToString().Length > 0)
			{
				MessageBox.Show("アップデートチェック中にエラーが発生しました。\n\n" + errMsg.ToString(), General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			// アップデートなし時に自動で閉じる
			if (updateLog.ToString().Length == 0)
			{
				this.DialogResult = DialogResult.Ignore;
			}
		}

		/// <summary>
		/// アップデートボタン押下イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void updateButton_Click(object sender, EventArgs e)
		{
			// 初期化
			updateStatus = 0;
			updateProgress.Minimum = 0;
			updateProgress.Value = 0;
			updateProgress.Maximum = updatePhaseCount;

			// アップデート処理
			if (saveType == "D")
			{
				// MSSQL
				SqlCommand cm = new SqlCommand();

				// [必須] Ver.1.1(update to GL 1.03)
				try
				{
					con.Open();
					// 文字エンコード変更（ALTER TABLE）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbName + "." + General.Var.DbName + " CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム追加（DCON_IMG, MEMO, STATUS, DB_VERSION）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbName + "." + General.Var.DbTable + " ADD (DCON_IMG NVARCHAR(50) NULL, MEMO NVARCHAR(500) NULL, STATUS NVARCHAR(10) NULL, DB_VERSION NVARCHAR(5) NOT NULL DEFAULT '" + latestVersion + "');"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム更新（TEMP1→DCON_IMG）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET DCON_IMG = TEMP1 WHERE DCON_IMG IS NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム更新（NULL→TEMP1）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET TEMP1 = NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム文字コード一括変換（ALTER TABLE）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbName + "." + General.Var.DbTable + " CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
					updateProgress.Value++;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | ALTER TABLE(ADD), UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
				}

			}
			else if (saveType == "M")
			{
				// MySQL
				MySqlCommand cm = new MySqlCommand();

				// [必須] Ver.1.1(update to GL 1.03)
				try
				{
					con2.Open();
					// テーブル文字コード変更（ALTER TABLE）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbTable + " CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム追加（DCON_IMG, MEMO, STATUS, DB_VERSION）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbTable + " ADD COLUMN DCON_IMG VARCHAR(50) NULL, ADD COLUMN MEMO VARCHAR(500) NULL, ADD COLUMN STATUS VARCHAR(10) NULL, ADD COLUMN DB_VERSION VARCHAR(5) NOT NULL DEFAULT '" + latestVersion + "';"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム更新（TEMP1→DCON_IMG）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + General.Var.DbTable + " SET DCON_IMG = TEMP1 WHERE DCON_IMG IS NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム更新（NULL→TEMP1）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + General.Var.DbTable + " SET TEMP1 = NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム文字コード一括変換（ALTER TABLE）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + General.Var.DbTable + " CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
					updateProgress.Value++;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | ALTER TABLE(ADD), UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
				}
			}
			else
			{
				// INI
				// [必須] Ver.1.1(update to GL 1.03)
			}

			// アップデート完了処理
			if (updateStatus == 1)
			{
				// アップデート完了
				MessageBox.Show("アップデートが完了しました！", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				// アップデート失敗
				MessageBox.Show("アップデートに失敗しました。\n\n" + errMsg.ToString(), General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// キャンセルボタン押下イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (updateRequired)
			{
				DialogResult dr = MessageBox.Show("ランチャーの動作に必須のアップデートがあります。\nアップデートしないと、ランチャーを起動できません。\n\nランチャーを終了しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
				if (dr == DialogResult.Yes)
				{
					this.DialogResult = DialogResult.Cancel;
				}
				else
				{
					return;
				}
			}
			else
			{
				if (hasUpdate)
				{
					DialogResult dr = MessageBox.Show("ランチャーをアップデートせずに続行しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
					if (dr == DialogResult.Yes)
					{
						this.DialogResult = DialogResult.No;
					}
					else
					{
						return;
					}
				}
			}
		}
	}
}
