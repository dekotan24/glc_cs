using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class AddItem : Form
	{
		SqlConnection con = new SqlConnection();
		MySqlConnection con2 = new MySqlConnection();
		string genSaveType = string.Empty;
		string iniPath = string.Empty;

		/// <summary>
		/// MSSQL用
		/// </summary>
		/// <param name="saveType">ゲームデータ保存方法</param>
		/// <param name="cn">SQLコネクション</param>
		/// <param name="cm">SQLコマンド</param>
		public AddItem(string saveType, SqlConnection cn)
		{
			InitializeComponent();
			// 数値ボックス最大値設定
			runTimeText.Maximum = Int32.MaxValue;
			startCountText.Maximum = Int32.MaxValue;

			// モードチェックボックス反映
			switch (saveType)
			{
				case "I":
					localIniCheck.Checked = true;
					break;
				case "D":
					onlineCheck.Checked = true;
					break;
				case "T":
					offlineCheck.Checked = true;
					break;
			}

			con = cn;
			genSaveType = saveType;
		}

		/// <summary>
		/// MySQL用
		/// </summary>
		/// <param name="saveType">ゲームデータ保存方法</param>
		/// <param name="cn">SQLコネクション</param>
		/// <param name="cm">SQLコマンド</param>
		public AddItem(string saveType, MySqlConnection cn)
		{
			InitializeComponent();
			// 数値ボックス最大値設定
			runTimeText.Maximum = Int32.MaxValue;
			startCountText.Maximum = Int32.MaxValue;

			// モードチェックボックス反映
			switch (saveType)
			{
				case "I":
					localIniCheck.Checked = true;
					break;
				case "M":
					onlineCheck.Checked = true;
					break;
				case "T":
					offlineCheck.Checked = true;
					break;
			}

			con2 = cn;
			genSaveType = saveType;
		}

		/// <summary>
		/// INI / オフラインモード用
		/// </summary>
		/// <param name="saveType">ゲームデータ保存方法</param>
		public AddItem(string saveType)
		{
			InitializeComponent();
			// 数値ボックス最大値設定
			runTimeText.Maximum = Int32.MaxValue;
			startCountText.Maximum = Int32.MaxValue;

			// モードチェックボックス反映
			switch (saveType)
			{
				case "I":
					localIniCheck.Checked = true;
					break;
				case "T":
					offlineCheck.Checked = true;
					break;
			}
			genSaveType = saveType;
		}

		/// <summary>
		/// フォームに対するドラッグアンドドロップ処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddItem_DragDrop(object sender, DragEventArgs e)
		{
			// DataFormats.FileDropを与えて、GetDataPresent()メソッドを呼び出す。
			var dropTarget = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			// GetDataにより取得したString型の配列から要素を取り出す。
			var targetFile = dropTarget[0];

			autoComplete(targetFile);
		}

		private void AddItem_DragEnter(object sender, DragEventArgs e)
		{
			// マウスポインター形状変更
			//
			// DragDropEffects
			//  Copy  :データがドロップ先にコピーされようとしている状態
			//  Move  :データがドロップ先に移動されようとしている状態
			//  Scroll:データによってドロップ先でスクロールが開始されようとしている状態、あるいは現在スクロール中である状態
			//  All   :上の3つを組み合わせたもの
			//  Link  :データのリンクがドロップ先に作成されようとしている状態
			//  None  :いかなるデータもドロップ先が受け付けようとしない状態

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		/// <summary>
		/// キャンセルボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		/// <summary>
		/// 追加ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ApplyButton_Click(object sender, EventArgs e)
		{
			string game_text = titleText.Text.Trim();
			string gamePath = exePathText.Text.Trim();
			string executeCmd = executeCmdText.Text.Trim();
			string imgPath = imgPathText.Text.Trim();
			string dcon_text = dconText.Text.Trim();
			string dcon_img = dconImgText.Text.Trim();
			string runTime = runTimeText.Value.ToString();
			string startCount = startCountText.Value.ToString();
			string rate = rateCheck.Checked ? "1" : "0";

			if (game_text.Length == 0 || gamePath.Length == 0)
			{
				return;
			}

			if (genSaveType == "I" || genSaveType == "T")
			{
				// ini
				if (File.Exists(GameIni))
				{
					int newmaxval = GameMax + 1;

					string targetFilePath = GameDir + newmaxval + ".ini";

					if (!(File.Exists(targetFilePath)))
					{
						KeyNames[] writeKeys = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.execute_cmd, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.memo, KeyNames.status, KeyNames.ini_version, KeyNames.rating };
						string[] writeValues = { game_text, imgPath, gamePath, executeCmd, runTime, startCount, dcon_text, dcon_img, string.Empty, "未プレイ", DBVer, rate };

						IniWrite(targetFilePath, "game", writeKeys, writeValues);
						WriteIni("list", "game", newmaxval.ToString(), 0);

						// 次回DB接続時に更新するフラグを立てる
						if (SaveType == "T")
						{
							WriteIni("list", "dbupdate", "1", 0);
						}
					}
					else
					{
						String dup = IniRead(targetFilePath, "game", KeyNames.name, "unknown");
						DialogResult dialogResult = MessageBox.Show("既にiniファイルが存在します！！\nあり得ません。手動で管理INIファイルを追加したか、内部処理で何らかのミスが発生した可能性があります。\n最も安全な対処法は、一度このフォームを閉じて、[再読込]することです。\n\n" + targetFilePath + "\n[" + dup + "]\n上書きしますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dialogResult == DialogResult.Yes)
						{
							KeyNames[] writeKeys = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.execute_cmd, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.memo, KeyNames.status, KeyNames.ini_version, KeyNames.rating };
							string[] writeValues = { game_text, imgPath, gamePath, executeCmd, runTime, startCount, dcon_text, dcon_img, string.Empty, "未プレイ", DBVer, rate };

							IniWrite(targetFilePath, "game", writeKeys, writeValues);
							WriteIni("list", "game", newmaxval.ToString(), 0);

							// 次回DB接続時に更新するフラグを立てる
							if (SaveType == "T")
							{
								WriteIni("list", "dbupdate", "1", 0);
							}
						}
						else if (dialogResult == DialogResult.No)
						{
							MessageBox.Show("新規のゲームを追加せずに処理を中断します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							WriteErrorLog(MethodBase.GetCurrentMethod().Name, "不明な結果です。\n実行を中断します。", string.Empty);
						}
					}
				}
				else
				{
					WriteErrorLog("ゲーム情報統括管理ファイルが見つかりません！", MethodBase.GetCurrentMethod().Name, GameIni);
					MessageBox.Show("ゲーム情報統括管理ファイルが見つかりません！\n" + GameIni, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			else
			{
				// database
				if (SaveType == "D")
				{
					// 接続情報
					SqlConnection cn = SqlCon;
					SqlCommand cm;
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						// SQL文
						CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, DCON_IMG, MEMO, STATUS, DB_VERSION ) VALUES ( @game_name, @game_path, @execute_cmd, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @dcon_img, '', '未プレイ', @db_version )"
					};
					cm.Connection = cn;
					// パラメータの設定
					cm.Parameters.AddWithValue("@game_name", game_text);
					cm.Parameters.AddWithValue("@game_path", gamePath);
					cm.Parameters.AddWithValue("@execute_cmd", executeCmd);
					cm.Parameters.AddWithValue("@img_path", imgPath);
					cm.Parameters.AddWithValue("@uptime", runTime);
					cm.Parameters.AddWithValue("@run_count", startCount);
					cm.Parameters.AddWithValue("@dcon_text", dcon_text);
					cm.Parameters.AddWithValue("@age_flg", rate);
					cm.Parameters.AddWithValue("@dcon_img", dcon_img);
					cm.Parameters.AddWithValue("@db_version", DBVer);

					try
					{
						cn.Open();
						cm.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					}
					finally
					{
						if (cn.State == ConnectionState.Open)
						{
							cn.Close();
						}
					}
				}
				else
				{
					// 接続情報
					MySqlConnection cn = SqlCon2;
					MySqlCommand cm;
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						// SQL文
						CommandText = @"INSERT INTO " + DbTable + " ( GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, DCON_IMG, MEMO, STATUS, DB_VERSION ) VALUES ( @game_name, @game_path, @execute_cmd, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @dcon_img, '', N'未プレイ', @db_version );"
					};
					cm.Connection = cn;
					// パラメータの設定
					cm.Parameters.AddWithValue("@game_name", game_text);
					cm.Parameters.AddWithValue("@game_path", gamePath);
					cm.Parameters.AddWithValue("@execute_cmd", executeCmd);
					cm.Parameters.AddWithValue("@img_path", imgPath);
					cm.Parameters.AddWithValue("@uptime", runTime);
					cm.Parameters.AddWithValue("@run_count", startCount);
					cm.Parameters.AddWithValue("@dcon_text", dcon_text);
					cm.Parameters.AddWithValue("@age_flg", rate);
					cm.Parameters.AddWithValue("@dcon_img", dcon_img);
					cm.Parameters.AddWithValue("@db_version", DBVer);

					try
					{
						cn.Open();
						cm.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					}
					finally
					{
						if (cn.State == ConnectionState.Open)
						{
							cn.Close();
						}
					}
				}
			}

			if (!disCloseCheck.Checked)
			{
				Close();
			}
			else
			{
				titleText.Text = string.Empty;
				exePathText.Text = string.Empty;
				executeCmdText.Text = string.Empty;
				imgPathText.Text = string.Empty;
				dconText.Text = string.Empty;
				dconImgText.Text = string.Empty;
				runTimeText.Value = 0;
				startCountText.Value = 0;
				imgPictureBox.ImageLocation = "";

				reloadIni();
			}
		}

		/// <summary>
		/// 起動回数リセットボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void runTimeResetButton_Click(object sender, EventArgs e)
		{
			runTimeText.Value = 0;
		}

		/// <summary>
		/// 起動時間リセットボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void startCountResetButton_Click(object sender, EventArgs e)
		{
			startCountText.Value = 0;
		}

		private void exePathButton_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "追加する実行ファイルを選択";
			openFileDialog1.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				autoComplete(openFileDialog1.FileName, "exe");
			}
			else
			{
				return;
			}
		}

		private void imgPathButton_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "実行ファイルの画像を選択";
			openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif|すべてのファイル (*.*)|*.*";
			openFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(exePathText.Text.Trim()).ToString() + ".png";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				autoComplete(openFileDialog1.FileName, "img");
			}
			else
			{
				return;
			}
		}

		private void autoComplete(string targetFile, string targetType = "")
		{
			// ファイルタイプが指定されていない場合に自動判別
			if (targetType == string.Empty)
			{
				string extension = Path.GetExtension(targetFile);
				if (extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".gif")
				{
					targetType = "img";
				}
				else
				{
					targetType = "exe";
				}
			}

			// ファイルごとによる処理
			if (targetType == "exe")
			{
				// タイトル自動補填（ファイル名）
				titleText.Text = Path.GetFileNameWithoutExtension(targetFile);

				// 実行ファイルパス自動補填（ファイルフルパス）
				exePathText.Text = targetFile;

				// 画像ファイルパス自動補填（[ファイル名(.png|.jpg)]の検出）
				string targetPath = Path.GetDirectoryName(targetFile).EndsWith("\\") ? Path.GetDirectoryName(targetFile) : Path.GetDirectoryName(targetFile) + "\\";
				string targetName = Path.GetFileNameWithoutExtension(targetFile);
				string[] targetImageTemp = { targetPath + targetName + ".png", targetPath + targetName + ".jpg" };

				if (File.Exists(targetImageTemp[0]))
				{
					imgPathText.Text = targetImageTemp[0];
				}
				else if (File.Exists(targetImageTemp[1]))
				{
					imgPathText.Text = targetImageTemp[1];
				}
				else
				{
					imgPathText.Text = string.Empty;
				}
			}
			else
			{
				imgPathText.Text = targetFile;
			}

			applyPictureBox(null, null);

			// 起動回数、実行時間の初期化
			startCountText.Value = 0;
			runTimeText.Value = 0;

			// フラグ初期化
			rateCheck.Checked = Rate != 0;
		}

		/// <summary>
		/// <see cref="imgPathText"/> に入力されているパスで <see cref="imgPictureBox"/> に画像を反映
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void applyPictureBox(object sender, EventArgs e)
		{
			// 画面反映
			if (File.Exists(imgPathText.Text))
			{
				try
				{
					imgPictureBox.ImageLocation = imgPathText.Text;
				}
				catch (Exception ex)
				{
					imgPictureBox.ImageLocation = "";
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, imgPathText.Text);
				}
			}
			else
			{
				imgPictureBox.ImageLocation = "";
			}
		}

		private void reloadIni()
		{
			GLConfigLoad();
		}
	}
}