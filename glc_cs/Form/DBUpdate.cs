﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static glc_cs.Core.Property;
using static glc_cs.General.Var;

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
		/// <summary>
		/// セーブデータ管理方法（I：INI、D：MSSQL、M：MySQL、T：Temp）。
		/// </summary>
		private string saveType = "I";  // INIファイルをデフォルトとしてセット

		/// <summary>
		/// MSSQLの接続用<see cref="SqlConnection"/>。
		/// </summary>
		private SqlConnection con = new SqlConnection();

		/// <summary>
		/// MySQLの接続用<see cref="MySqlConnection"/>。
		/// </summary>
		private MySqlConnection con2 = new MySqlConnection();

		/// <summary>
		/// エラーメッセージ保持用<see cref="StringBuilder"/>。
		/// </summary>
		private StringBuilder errMsg = new StringBuilder();

		/// <summary>
		/// 更新履歴表示用<see cref="StringBuilder"/>。適用対象の更新履歴を追加します。
		/// </summary>
		private StringBuilder updateLog = new StringBuilder();

		/// <summary>
		/// 強制アップデートフラグ。
		/// </summary>
		private bool updateRequired = false;

		/// <summary>
		/// アップデート有無フラグ。
		/// </summary>
		private bool hasUpdate = false;

		/// <summary>
		/// アップデートのステータスフラグの管理。
		/// </summary>
		private int updateStatus = 0;

		/// <summary>
		/// 現在のDBバージョン。
		/// </summary>
		private string currentVersion = "1.0";

		/// <summary>
		/// 最新のDBバージョン。<see cref="DBVer"/> を保持。
		/// </summary>
		private string latestVersion = DBVer;

		/// <summary>
		/// 必要なアップデート数カウンタ。全体進捗度の管理に使用。
		/// </summary>
		private int updatePhaseCount = 0;


		/// <summary>
		/// [必須] Ver.1.1(update to GL 1.03)
		/// </summary>
		private readonly StringBuilder v1_1 = new StringBuilder("[必須] Ver.1.1(Update to GL 1.03 or above)\n")
		.AppendLine("\t* [ALTER TABLE] [※MySQLのみ] 既存テーブルの文字コード変換(utf8mb4 / utf8mb4_general_ci)")
		.AppendLine("\t* [カラム追加] [DCON_IMG]:DiscordConnector カスタム画像用、[TEMP1]の値で更新")
		.AppendLine("\t* [カラム追加] [MEMO]:メモ用")
		.AppendLine("\t* [カラム追加] [STATUS]:ステータス用")
		.AppendLine("\t* [カラム追加] [DB_VERSION]:DBバージョン管理用")
		.AppendLine("\t* [UPDATE] [TEMP1]:NULL");

		/// <summary>
		/// [必須] Ver.1.2(update to GL 1.07)
		/// </summary>
		private readonly StringBuilder v1_2 = new StringBuilder("[必須] Ver.1.2(Update to GL 1.07 or above)\n")
		.AppendLine("\t* [UPDATE] [STATUS]:【未着手/着手中/完了】が【未プレイ/プレイ中/プレイ済】のどれかになるように更新");

		/// <summary>
		/// [必須] Ver.1.3(update to GL 1.09)
		/// </summary>
		private readonly StringBuilder v1_3 = new StringBuilder("[必須] Ver.1.3(Update to GL 1.09 or above)\n")
		.AppendLine("\t* [カラム追加] [EXECUTE_CMD]:実行時パラメータ保存用");

		/// <summary>
		/// [必須] Ver.1.4(update to GL 1.10)
		/// </summary>
		private readonly StringBuilder v1_4 = new StringBuilder("[必須] Ver.1.4(Update to GL 1.10 or above)\n")
		.AppendLine("\t* [カラム追加] [EXTRACT_TOOL]:抽出ツール保存用");

		/// <summary>
		/// [必須] Ver.1.4(update to GL 1.10)
		/// </summary>
		private readonly StringBuilder v1_5 = new StringBuilder("[必須] Ver.1.5(Update to GL 1.11 or above)\n")
		.AppendLine("\t* [カラム追加] [SAVEDATA_PATH]:セーブデータのパス用");


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
			updateTargetLabel.Text = "INI Version";
		}

		private void DBUpdate_Load(object sender, EventArgs e)
		{
			glVersionLabel.Text = AppVer;

			if (saveType == "D")    // MSSQLの場合
			{
				// MSSQL

				/* ↓ [必須] Ver.1.1(update to GL 1.03) ↓ */
				// [DCON_IMG] 存在チェック
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(0, "DCON_IMG")
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
						CommandText = createIsExistsTableSQL(0, "MEMO")
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
						CommandText = createIsExistsTableSQL(0, "STATUS")
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
						CommandText = createIsExistsTableSQL(0, "DB_VERSION")
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
					string errorMsg = "[v1.0 -> v1.1 | sys.columns] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
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
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.1";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.1(update to GL 1.03) ↑ */
				/* ↓ [必須] Ver.1.2(update to GL 1.07) ↓ */
				// [STATUS]が削除対象の存在チェック
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createAndGetDataSQL(0, "*", "WHERE STATUS IN(N'',N'-------',N'未攻略',N'攻略中',N'攻略済',N'未着手',N'着手',N'完了',NULL)")
				};
				cm.Connection = con;

				try
				{
					con.Open();
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在する場合、アップデートを行う
						if (reader.Read())
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.1 -> v1.2 | STATUS] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_2.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.2";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.2(update to GL 1.07) ↑ */
				/* ↓ [必須] Ver.1.3(update to GL 1.09) ↓ */
				// [EXECUTE_CMD] 存在チェック
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(0, "EXECUTE_CMD")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.2 -> v1.3 | EXECUTE_CMD] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_3.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.3";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.3(update to GL 1.09) ↑ */
				/* ↓ [必須] Ver.1.4(update to GL 1.10) ↓ */
				// [EXTRACT_TOOL] 存在チェック
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(0, "EXTRACT_TOOL")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.3 -> v1.4 | EXTRACT_TOOL] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_4.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.4";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.4(update to GL 1.10) ↑ */
				/* ↓ [必須] Ver.1.5(update to GL 1.11) ↓ */
				// [EXTRACT_TOOL] 存在チェック
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(0, "SAVEDATA_PATH")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.4 -> v1.5 | SAVEDATA_PATH] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_5.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.5";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.4(update to GL 1.10) ↑ */


			}
			else if (saveType == "M")   // MySQLの場合
			{
				// MySQL
				/* ↓ [必須] Ver.1.1(update to GL 1.03) ↓ */
				MySqlCommand cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(1, "DCON_IMG")
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
						CommandText = createIsExistsTableSQL(1, "MEMO")
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
						CommandText = createIsExistsTableSQL(1, "STATUS")
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
						CommandText = createIsExistsTableSQL(1, "DB_VERSION")
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.1";
					}
					updatePhaseCount++;
				}

				/* ↑ [必須] Ver.1.1(update to GL 1.03) ↑ */
				/* ↓ [必須] Ver.1.2(update to GL 1.07) ↓ */
				// [STATUS]が削除対象の存在チェック
				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createAndGetDataSQL(1, "*", "WHERE STATUS IN(N'',N'-------',N'未着手',N'着手',N'完了',NULL)")
				};
				cm.Connection = con2;

				try
				{
					con2.Open();
					using (var reader = cm.ExecuteReader())
					{
						// カラムが存在する場合、アップデートを行う
						if (reader.Read())
						{
							// DataReaderクローズ（クローズしないとエラーになる）
							reader.Close();

							hasUpdate = true;
						}
					}
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.1 -> v1.2 | STATUS] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine(errorMsg);
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}

					if (hasUpdate)
					{
						updateLog.AppendLine(v1_2.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.2";
					}
					updatePhaseCount++;
				}
				/* ↑ [必須] Ver.1.2(update to GL 1.07) ↑ */
				/* ↓ [必須] Ver.1.3(update to GL 1.09) ↓ */
				// [EXECUTE_CMD] 存在チェック
				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(1, "EXECUTE_CMD")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.2 -> v1.3 | EXECUTE_CMD] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
						updateLog.AppendLine(v1_3.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.3";
					}
					updatePhaseCount++;
				}

				/* ↑ [必須] Ver.1.3(update to GL 1.09) ↑ */
				/* ↓ [必須] Ver.1.4(update to GL 1.10) ↓ */
				// [EXTRACT_TOOL] 存在チェック
				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(1, "EXTRACT_TOOL")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.3 -> v1.4 | EXTRACT_TOOL] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
						updateLog.AppendLine(v1_4.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.4";
					}
					updatePhaseCount++;
				}

				/* ↑ [必須] Ver.1.4(update to GL 1.10) ↑ */
				/* ↓ [必須] Ver.1.5(update to GL 1.11) ↓ */
				// [EXTRACT_TOOL] 存在チェック
				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = createIsExistsTableSQL(1, "SAVEDATA_PATH")
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
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.4 -> v1.5 | SAVEDATA_PATH] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText + " / hasUpdate:" + hasUpdate;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
						updateLog.AppendLine(v1_5.ToString());
						updateRequired = true;
						hasUpdate = false;
					}
					else
					{
						currentVersion = "1.5";
					}
					updatePhaseCount++;
				}

				/* ↑ [必須] Ver.1.5(update to GL 1.11) ↑ */
			}
			else
			{
				// INI
				int fileCount = 0;
				string readini = string.Empty;
				string gameDirName = GameDir;
				string dcon_img = string.Empty;
				string memo = string.Empty;
				string status = string.Empty;
				string ini_version = string.Empty;
				string execute_cmd = string.Empty;
				string extract_tool = string.Empty;
				string savedata_path = string.Empty;

				// 全ゲーム数取得
				if (File.Exists(GameIni))
				{
					fileCount = Convert.ToInt32(ReadIni("list", "game", "0", 0));
				}

				if (fileCount >= 1) // ゲーム登録数が1以上の場合
				{
					for (int curCount = 1; curCount <= fileCount; curCount++)
					{
						// 読込iniファイル名更新
						readini = gameDirName + curCount + ".ini";

						if (File.Exists(readini))
						{
							try
							{
								/* ↓ [必須] Ver.1.1(update to GL 1.03) ↓ */
								// アップデート対象のセクションを取得する
								KeyNames[] keyNames = { KeyNames.dcon_img, KeyNames.memo, KeyNames.status, KeyNames.ini_version, KeyNames.execute_cmd, KeyNames.extract_tool, KeyNames.savedata_path };
								string[] failedVal = { "!Err", "!Err", "!Rrr", "!Err", "!Err", "!Err", "!Err" };

								string[] resultValues = IniRead(readini, "game", keyNames, failedVal);
								dcon_img = resultValues[0];
								memo = resultValues[1];
								status = resultValues[2];
								ini_version = resultValues[3];
								execute_cmd = resultValues[4];
								extract_tool = resultValues[5];
								savedata_path = resultValues[6];


								// 取得できなかった場合、アップデートフラグを立てる
								if (dcon_img.Equals("!Err") || memo.Equals("!Err") || status.Equals("!Err") || ini_version.Equals("!Err"))
								{
									hasUpdate = true;
									currentVersion = "1.0";
									break;  // 1個でもアップデート対象があった場合、全てアップデート対象となるためループから抜ける
								}
								/* ↑ [必須] Ver.1.1(update to GL 1.03) ↑ */

								/* ↓ [必須] Ver.1.2(update to GL 1.07) ↓ */
								// 廃止したステータスの場合、アップデートフラグを立てる
								if (status == "-------" || status == "未着手" || status == "着手" || status == "完了" || status == "")
								{
									if (!hasUpdate)
									{
										currentVersion = "1.1";
									}
									hasUpdate = true;
									break;  // 1個でもアップデート対象があった場合、全てアップデート対象となるためループから抜ける
								}
								/* ↑ [必須] Ver.1.2(update to GL 1.07) ↑ */

								/* ↓ [必須] Ver.1.3(update to GL 1.09) ↓ */
								if (execute_cmd.Equals("!Err"))
								{
									hasUpdate = true;
									currentVersion = "1.2";
									break;  // 1個でもアップデート対象があった場合、全てアップデート対象となるためループから抜ける
								}
								/* ↑ [必須] Ver.1.3(update to GL 1.09) ↑ */

								/* ↓ [必須] Ver.1.4(update to GL 1.10) ↓ */
								if (extract_tool.Equals("!Err"))
								{
									hasUpdate = true;
									currentVersion = "1.3";
									break;  // 1個でもアップデート対象があった場合、全てアップデート対象となるためループから抜ける
								}
								/* ↑ [必須] Ver.1.4(update to GL 1.10) ↑ */

								/* ↓ [必須] Ver.1.5(update to GL 1.11) ↓ */
								if (savedata_path.Equals("!Err"))
								{
									hasUpdate = true;
									currentVersion = "1.4";
									break;  // 1個でもアップデート対象があった場合、全てアップデート対象となるためループから抜ける
								}
								/* ↑ [必須] Ver.1.5(update to GL 1.11) ↑ */
							}
							catch
							{
								errMsg.AppendLine("[ERROR] ファイルの読込中にエラー。(" + readini + ")");
							}
						}
						else
						{
							// 個別ini存在しない場合
							errMsg.AppendLine("[ERROR] ファイルが存在しません。(" + readini + ")");
						}
					}

					if (hasUpdate)
					{
						switch (currentVersion)
						{
							case "1.0":
								updateLog.AppendLine(v1_1.ToString());
								updateRequired = true;
								goto case "1.1";

							case "1.1":
								updateLog.AppendLine(v1_2.ToString());
								updateRequired = true;
								goto case "1.2";

							case "1.2":
								updateLog.AppendLine(v1_3.ToString());
								updateRequired = true;
								goto case "1.3";

							case "1.3":
								updateLog.AppendLine(v1_4.ToString());
								updateRequired = true;
								goto case "1.4";

							case "1.4":
								updateLog.AppendLine(v1_5.ToString());
								updateRequired = true;
								break;
						}
						hasUpdate = false;
						updatePhaseCount++;
					}
					updatePhaseCount++;
				}
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
				WriteErrorLog("アップデートチェック中にエラーが発生しました。", MethodBase.GetCurrentMethod().Name, errMsg.ToString());
				MessageBox.Show("アップデートチェック中にエラーが発生しました。\n\n" + errMsg.ToString(), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					/*
					 * MSSQLでは文字コード変換を行わない
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbName + " COLLATE Japanese_CI_AS;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();
					*/

					// カラム追加（DCON_IMG, MEMO, STATUS, DB_VERSION）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " ADD DCON_IMG NVARCHAR(50) NULL, MEMO NVARCHAR(500) NULL, STATUS NVARCHAR(10) NULL DEFAULT N'未プレイ', DB_VERSION NVARCHAR(5) NOT NULL DEFAULT N'1.1';"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム更新（TEMP1→DCON_IMG）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbName + "." + DbTable + " SET DCON_IMG = TEMP1 WHERE TEMP1 IS NOT NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム更新（STATUS）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbName + "." + DbTable + " SET STATUS = (CASE WHEN RUN_COUNT = '0' THEN N'未プレイ' ELSE N'プレイ中' END) WHERE STATUS IN(N'未着手',N'着手中',N'完了');"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム更新（NULL→TEMP1）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbName + "." + DbTable + " SET TEMP1 = NULL WHERE TEMP1 IS NOT NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// カラム文字コード一括変換（ALTER TABLE）
					/*
					 * MSSQLでは文字コード変換を行わない
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();
					*/

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | ALTER TABLE(ADD), UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.2(update to GL 1.07)
				try
				{
					con.Open();

					// レコード更新
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbName + "." + DbTable + " SET STATUS = (CASE WHEN STATUS = N'完了' THEN N'プレイ済' WHEN STATUS = N'着手中' THEN N'プレイ中' ELSE N'未プレイ' END) WHERE STATUS IN(N'未着手',N'着手中',N'完了',N'-------',N'',NULL);"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.1 -> v1.2 | UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.3(update to GL 1.09)
				try
				{
					con.Open();
					// カラム追加（EXECUTE_CMD）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " ADD EXECUTE_CMD NVARCHAR(500) NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.2 -> v1.3 | ALTER TABLE(ADD), EXECUTE_CMD] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.4(update to GL 1.10)
				try
				{
					con.Open();
					// カラム追加（EXTRACT_TOOL）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " ADD EXTRACT_TOOL NVARCHAR(10) NULL DEFAULT N'0';"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.3 -> v1.4 | ALTER TABLE(ADD), EXTRACT_TOOL] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.5(update to GL 1.11)
				try
				{
					con.Open();
					// カラム追加（EXTRACT_TOOL）
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " ADD SAVEDATA_PATH NVARCHAR(MAX) NULL;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.4 -> v1.5 | ALTER TABLE(ADD), SAVEDATA_PATH] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// ↓ 最終共通処理
				try
				{
					con.Open();
					// DBバージョン更新
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbName + "." + DbTable + " SET DB_VERSION = N'" + latestVersion + "';"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					// DBバージョンデフォルト値更新
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbName + "." + DbTable + " ADD DEFAULT N'" + latestVersion + "' FOR DB_VERSION;"
					};
					cm.Connection = con;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[any -> any | UPDATE, DB_VERSION] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
				// ↑ 最終共通処理
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
						CommandText = @"ALTER TABLE " + DbTable + " CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム追加（DCON_IMG, MEMO, STATUS, DB_VERSION）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " ADD COLUMN DCON_IMG NVARCHAR(50) NULL, ADD COLUMN MEMO NVARCHAR(500) NULL, ADD COLUMN STATUS NVARCHAR(10) NULL DEFAULT '未プレイ', ADD COLUMN DB_VERSION NVARCHAR(5) NOT NULL DEFAULT '1.1';"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム更新（TEMP1→DCON_IMG）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbTable + " SET DCON_IMG = TEMP1 WHERE TEMP1 IS NOT NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム更新（STATUS）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbTable + " SET STATUS = (CASE WHEN RUN_COUNT = '0' THEN N'未プレイ' ELSE N'プレイ中' END);"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム更新（NULL→TEMP1）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbTable + " SET TEMP1 = NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// カラム文字コード一括変換（ALTER TABLE）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " CONVERT TO CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.0 -> v1.1 | ALTER TABLE(ADD), UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.2(update to GL 1.07)
				try
				{
					con2.Open();

					// カラム更新（STATUS）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbTable + " SET STATUS = (CASE WHEN STATUS = N'完了' THEN N'プレイ済' WHEN STATUS = N'着手中' THEN N'プレイ中' ELSE N'未プレイ' END) WHERE STATUS IN(N'未着手',N'着手中',N'完了',N'-------',N'',NULL);"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.1 -> v1.2 | UPDATE] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.3(update to GL 1.09)
				try
				{
					con2.Open();
					// カラム追加（EXECUTE_CMD）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " ADD COLUMN EXECUTE_CMD NVARCHAR(500) NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.2 -> v1.3 | ALTER TABLE(ADD), EXECUTE_CMD] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.4(update to GL 1.10)
				try
				{
					con2.Open();
					// カラム追加（EXTRACT_TOOL）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " ADD COLUMN EXTRACT_TOOL NVARCHAR(10) NULL DEFAULT '0';"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.3 -> v1.4 | ALTER TABLE(ADD), EXTRACT_TOOL] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// [必須] Ver.1.5(update to GL 1.11)
				try
				{
					con2.Open();
					// カラム追加（SAVEDATA_PATH）
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " ADD COLUMN SAVEDATA_PATH NVARCHAR(500) NULL;"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[v1.4 -> v1.5 | ALTER TABLE(ADD), SAVEDATA_PATH] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
					errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
					updateStatus = -1;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					// アップデート処理後のステータス更新
					updateProgress.Value++;
				}

				// ↓ 最終共通処理
				try
				{
					con2.Open();
					// DBバージョン更新
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"UPDATE " + DbTable + " SET DB_VERSION = N'" + latestVersion + "';"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					// DBバージョンデフォルト値更新
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"ALTER TABLE " + DbTable + " ALTER DB_VERSION SET DEFAULT N'" + latestVersion + "';"
					};
					cm.Connection = con2;
					cm.ExecuteNonQuery();

					updateStatus = 1;
				}
				catch (Exception ex)
				{
					string errorMsg = "[any -> any | UPDATE, DB_VERSION] " + ex.Message + " / SaveType:" + saveType + " / SQLCon:" + con2.ConnectionString + " / SQLCommand:" + cm.CommandText;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
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
				// ↑ 最終共通処理
			}
			else
			{
				// INI
				int fileCount = 0;
				string readini = string.Empty;
				string gameDirName = GameDir;
				string dcon_img = string.Empty;
				string memo = string.Empty;
				string status = string.Empty;
				string ini_version = "1.1";
				string execute_cmd = string.Empty;
				string extract_tool = "0";
				string savedata_path = string.Empty;

				// アップデート処理後のステータス更新
				updateStatus = 1;

				// 全ゲーム数取得
				if (File.Exists(GameIni))
				{
					fileCount = Convert.ToInt32(ReadIni("list", "game", "0", 0));
				}

				if (fileCount >= 1) // ゲーム登録数が1以上の場合
				{
					for (int curCount = 1; curCount <= fileCount; curCount++)
					{
						// 読込iniファイル名更新
						readini = gameDirName + "\\" + curCount + ".ini";

						if (File.Exists(readini))
						{
							try
							{
								// アップデート対象のセクションを取得する
								KeyNames[] keyNames = { KeyNames.temp1, KeyNames.memo, KeyNames.status, KeyNames.ini_version, KeyNames.execute_cmd, KeyNames.extract_tool, KeyNames.savedata_path };
								string[] failedVal = { "!Err", "!Err", "!Rrr", "!Err", "!Err", "!Err", "!Err" };

								string[] resultValues = IniRead(readini, "game", keyNames, failedVal);
								dcon_img = resultValues[0];
								memo = resultValues[1];
								status = resultValues[2];
								ini_version = resultValues[3];
								execute_cmd = resultValues[4];
								extract_tool = resultValues[5];
								savedata_path = resultValues[6];

								/* ↓ [必須] Ver.1.1(update to GL 1.03) ↓ */
								// 取得できなかった場合、アップデートを行う
								if (dcon_img.Equals("!Err") || memo.Equals("!Err") || status.Equals("!Err") || ini_version.Equals("!Err"))
								{
									KeyNames[] keyColumns = { KeyNames.dcon_img, KeyNames.memo, KeyNames.status, KeyNames.ini_version, KeyNames.temp1 };
									string[] writeValues = { (dcon_img.Equals("!Err") ? string.Empty : dcon_img), string.Empty, (IniRead(readini, "game", KeyNames.start, "0").Equals("0") ? "未プレイ" : "プレイ中"), "1.1", string.Empty };
									IniWrite(readini, "game", keyColumns, writeValues);
								}
								/* ↑ [必須] Ver.1.1(update to GL 1.03) ↑ */

								/* ↓ [必須] Ver.1.2(update to GL 1.07) ↓ */
								// 廃止したステータスの場合、アップデートフラグを立てる
								if (status.Contains("-------") || status.Contains("未着手") || status.Contains(""))
								{
									IniWrite(readini, "game", KeyNames.status, "未プレイ");
								}
								else if (status.Contains("着手"))
								{
									IniWrite(readini, "game", KeyNames.status, "プレイ中");
								}
								else if (status.Contains("完了"))
								{
									IniWrite(readini, "game", KeyNames.status, "プレイ済");
								}
								/* ↑ [必須] Ver.1.2(update to GL 1.07) ↑ */
								/* ↓ [必須] Ver.1.3(update to GL 1.09) ↓ */
								if (execute_cmd.Equals("!Err"))
								{
									IniWrite(readini, "game", KeyNames.execute_cmd, string.Empty);
								}
								/* ↑ [必須] Ver.1.3(update to GL 1.09) ↑ */
								/* ↓ [必須] Ver.1.4(update to GL 1.10) ↓ */
								if (extract_tool.Equals("!Err"))
								{
									IniWrite(readini, "game", KeyNames.extract_tool, "0");
								}
								/* ↑ [必須] Ver.1.4(update to GL 1.10) ↑ */
								/* ↓ [必須] Ver.1.5(update to GL 1.11) ↓ */
								if (savedata_path.Equals("!Err"))
								{
									IniWrite(readini, "game", KeyNames.savedata_path, "0");
								}
								/* ↑ [必須] Ver.1.5(update to GL 1.11) ↑ */

								/* ↓ 最終共通処理 */
								// INIバージョン設定
								IniWrite(readini, "game", KeyNames.ini_version, latestVersion);
								/* ↑ 最終共通処理 */
							}
							catch (Exception ex)
							{
								string errorMsg = "[v" + currentVersion + " -> v" + latestVersion + " | R/W] " + ex.Message + " / SaveType:" + saveType + " / INI:" + readini + " / dcon_img:" + dcon_img + " / memo:" + memo + " / status:" + status;
								WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, errorMsg);
								errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
								updateStatus = -1;
							}
						}
						else
						{
							// 個別ini存在しない場合
							string errorMsg = "[v" + currentVersion + " -> v" + latestVersion + " | Read File] ファイルが存在しません。 / SaveType:" + saveType + " / INI:" + readini;
							WriteErrorLog("ファイルの読込に失敗しました。ファイルが存在しません。", MethodBase.GetCurrentMethod().Name, errorMsg);
							errMsg.AppendLine("[ERROR] [" + DateTime.Now + "] " + errorMsg);
							updateStatus = -1;
						}
					}
					updateProgress.Value++;
				}
			}

			// アップデート完了処理
			if (updateStatus == 1)
			{
				// アップデート完了
				if (InitialUpdateCheckSkipFlg)
				{
					WriteIni("disable", "updchkVer", AppVer);
				}

				MessageBox.Show("アップデートが完了しました！（" + latestVersion + "）" + (errMsg.Length != 0 ? "\n\nアップデート中に幾つかのエラーに遭遇しました。\nカラム重複エラー以外のエラーが存在する場合は正しくアップデートできなかった可能性があります。\nエラー内容はエラーログをご確認ください。" : ""), AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				this.DialogResult = DialogResult.OK;
			}
			else
			{
				// アップデート失敗
				MessageBox.Show("アップデートに失敗しました。\n一部のファイルは更新されなかった可能性があります。\n\n" + errMsg.ToString(), AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				DialogResult dr = MessageBox.Show("ランチャーの動作に必須のアップデートがあります。\nアップデートしないと、ランチャーを起動できません。\n\nランチャーを終了しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
					DialogResult dr = MessageBox.Show("ランチャーをアップデートせずに続行しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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

		/// <summary>
		/// カラム存在チェックを行うSQL文を作成します。
		/// </summary>
		/// <param name="type"><value>0</value>：<see cref="System.Data.SqlClient"/>、<value>1</value>:<see cref="MySql.Data.MySqlClient"/></param>
		/// <param name="columnName">カラム名</param>
		/// <returns><paramref name="type"/>に応じた<paramref name="columnName"/>が存在するかをチェックするSQL</returns>
		private string createIsExistsTableSQL(int type, string columnName)
		{
			string ans = string.Empty;

			switch (type)
			{
				case 0:
					// MSSQL
					ans = @"USE " + DbName + "; SELECT * FROM sys.columns WHERE Name = '" + columnName + "' AND Object_ID = OBJECT_ID(N'" + DbTable + "');";
					break;

				default:
				case 1:
					// MySQL / default
					ans = @"DESCRIBE " + DbTable + " " + columnName + ";";
					break;
			}

			return ans;
		}

		/// <summary>
		/// レコード存在チェックを行うSQL文を作成します。以下のSQLと等価です。<code>SELECT <paramref name="getTargetColumn"/> FROM (<see cref="DbName"/>).(<see cref="DbTable"/>) <paramref name="wherePhraseFullText"/></code>
		/// </summary>
		/// <param name="type"><value>0</value>：<see cref="System.Data.SqlClient"/>、<value>1</value>:<see cref="MySql.Data.MySqlClient"/></param>
		/// <param name="getTargetColumn">検索対象のカラム名</param>
		/// <param name="wherePhraseFullText">フルテキストのWHERE条件</param>
		/// <returns><paramref name="type"/>に応じたSELECT文</returns>
		private string createAndGetDataSQL(int type, string getTargetColumn, string wherePhraseFullText)
		{
			string ans = string.Empty;

			switch (type)
			{
				case 0:
					// MSSQL
					ans = @"USE " + DbName + "; SELECT " + getTargetColumn + " FROM " + DbTable + " " + wherePhraseFullText + ";";
					break;

				default:
				case 1:
					// MySQL / default
					ans = @"SELECT " + getTargetColumn + " FROM " + DbName + "." + DbTable + " " + wherePhraseFullText + ";";
					break;
			}

			return ans;
		}
	}
}
