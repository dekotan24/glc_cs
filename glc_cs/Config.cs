using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class Config : Form
	{
		public Config()
		{
			InitializeComponent();
		}

		/// <summary>
		/// コンフィグ画面ロード
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form2_Load(object sender, EventArgs e)
		{
			if (GLConfigLoad() == false)
			{
				label15.Text = "Configロード中にエラー。詳しくはエラーログを参照して下さい。";
			}

			// バージョン取得
			verLabel.Text = "Ver." + AppVer + " Build " + AppBuild;

			// [全般]タブ
			// 背景画像
			backgroundImageText.Text = BgImg;
			// グリッド無効化
			gridDisableCheck.Checked = !GridEnable;
			// アップデートフラグ
			updateCheckDisableCheck.Checked = InitialUpdateCheckSkipFlg;
			// 最小化コントロール表示フラグ
			enableWindowHideControlCheck.Checked = WindowHideControlFlg;
			// グリッドサイズ固定フラグ
			fixGridSizeCheck.Checked = FixGridSizeFlg;
			// グリッドサイズ値
			switch (FixGridSize)
			{
				case 8:
					fixGridSize8.Checked = true;
					break;
				case 64:
					fixGridSize64.Checked = true;
					break;
				case 32:
				default:
					fixGridSize32.Checked = true;
					break;
			}
			DisableInitialLoadCountCheck.Checked = DisableInitialLoadCountFlg;

			// Discord設定読み込み
			bool dconActive = Dconnect;
			dconAppIDText.Text = DconAppID;

			// Discord連携有効フラグ
			dconEnableCheck.Checked = dconActive;
			// 機能アクティブ
			if (dconActive)
			{
				groupBox2.Enabled = true;
				groupBox6.Enabled = true;
				groupBox13.Enabled = true;
			}
			else
			{
				groupBox2.Enabled = false;
				groupBox6.Enabled = false;
				groupBox13.Enabled = false;
			}

			// レート設定
			if (Rate == 1)
			{
				dconRatingRadio2.Checked = true;
			}
			else
			{
				dconRatingRadio1.Checked = true;
			}

			// Discord Connectorパス取得
			string dconpath = DconPath;

			if (File.Exists(dconpath))
			{
				// 指定パスにdcon.jar存在する場合
				dconText.Text = dconpath;
				label11.Text = "OK";
			}
			else
			{
				dconText.Text = string.Empty;
				label11.Text = "NG";
			}

			// 棒読みちゃん設定読み込み
			bool isbyActive = ByActive;
			bouyomiEnableCheck.Checked = isbyActive;
			if (isbyActive)
			{
				groupBox4.Enabled = true;
				groupBox5.Enabled = true;
			}

			if (ByType == 1)
			{
				radioButton4.Checked = true;
			}
			else
			{
				radioButton3.Checked = true;
			}
			textBox4.Text = ByHost;
			textBox5.Text = ByPort.ToString();

			RoWCheck.Checked = ByRoW;
			RoSCheck.Checked = ByRoS;
			RoGCheck.Checked = ByRoG;

			// [保存方法]
			if (SaveType == "I")
			{
				iniRadio.Checked = true;
				setDirectoryControl(true);
			}
			else if (SaveType == "D")   // MSSQL
			{
				mssqlRadio.Checked = true;
				setDirectoryControl(false);
			}
			else if (SaveType == "M")   // MySQL
			{
				mysqlRadio.Checked = true;
				setDirectoryControl(false);
			}
			else
			{
				// 一時モードの場合はiniの値を取得する
				switch (ReadIni("general", "save", "I"))
				{
					case "I":
						iniRadio.Checked = true;
						break;
					case "D":
						mssqlRadio.Checked = true;
						break;
					case "M":
						mysqlRadio.Checked = true;
						break;
				}
			}

			urlText.Text = DbUrl;
			portText.Text = DbPort;
			dbText.Text = DbName;
			tableText.Text = DbTable;
			userText.Text = DbUser;
			pwText.Text = DbPass;

			offlineSaveEnableCheck.Checked = OfflineSave;
			useLocalDBCheck.Checked = UseLocalDB;


			// スーパーモード
			// ドロップダウン既定値設定
			insertColumnsDropDown.SelectedIndex = 0;

			// 作業ディレクトリ反映
			if (SaveType != "T")
			{
				if (GameDir.EndsWith("\\Data\\"))
				{
					iniText.Text = GameDir.Substring(0, GameDir.Length - 5);
				}
				else
				{
					iniText.Text = GameDir;
				}
			}
			else
			{
				string tmpRawGameDir = ReadIni("default", "directory", BaseDir.EndsWith("\\") ? BaseDir : BaseDir + "\\");
				if (tmpRawGameDir.EndsWith("\\Data\\"))
				{
					iniText.Text = tmpRawGameDir.Substring(0, GameDir.Length - 5);
				}
				else
				{
					iniText.Text = tmpRawGameDir;
				}
			}

			// [ツール]タブ
			enableExtractCheck.Checked = ExtractEnable;
			extractToolsGroup.Enabled = ExtractEnable;
			extractToolSelectCombo.SelectedIndex = 0;
			extractToolPathText.Text = string.Empty;
			extractToolArgText.Text = string.Empty;
			addGameArgCheck.Checked = false;
			extractCurrentDirCheck.Checked = false;
			addGameDirCheck.Checked = false;

			saveWithDownloadCheck.Visible = OfflineSave;
		}

		/// <summary>
		/// 保存ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveButton_Click(object sender, EventArgs e)
		{
			string offlineSaveTypeOld = ReadIni("general", "OfflineSave", offlineSaveEnableCheck.Checked ? "1" : "0");
			bool canExit = true;
			if (mssqlRadio.Checked || mysqlRadio.Checked)
			{
				if (urlText.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (portText.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (dbText.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (tableText.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (userText.Text.Trim().Length <= 0)
				{
					canExit = false;
				}

				// 保存可否判定
				if (!canExit)
				{
					MessageBox.Show("データの保存方法をデータベースにした場合、\n[URL]、[Port]、[DB]、[Table]、[User]は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			MyBase64str base64 = new MyBase64str();

			// 全般
			WriteIni("imgd", "bgimg", backgroundImageText.Text.Trim());
			WriteIni("disable", "grid", gridDisableCheck.Checked ? "1" : "0");
			WriteIni("disable", "updchk", updateCheckDisableCheck.Checked ? "1" : "0");
			if (updateCheckDisableCheck.Checked)
			{
				if (!InitialUpdateCheckSkipVer.Equals(AppVer))
				{
					WriteIni("disable", "updchkVer", AppVer);
				}
			}
			else
			{
				WriteIni("disable", "updchkVer", "0.0");
			}
			WriteIni("disable", "enableWindowHideControl", enableWindowHideControlCheck.Checked ? "1" : "0");
			WriteIni("disable", "DisableInitialLoadCount", DisableInitialLoadCountCheck.Checked ? "1" : "0");
			WriteIni("grid", "fixGridSizeFlg", fixGridSizeCheck.Checked ? "1" : "0");
			WriteIni("grid", "fixGridSize", (fixGridSize8.Checked ? "8" : (fixGridSize64.Checked ? "64" : "32")));

			// 保存方法
			WriteIni("general", "save", mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I");
			WriteIni("general", "OfflineSave", offlineSaveEnableCheck.Checked ? "1" : "0");
			WriteIni("general", "UseLocalDB", useLocalDBCheck.Checked ? "1" : "0");
			WriteIni("connect", "DBURL", urlText.Text.Trim());
			WriteIni("connect", "DBPort", portText.Text.Trim());
			WriteIni("connect", "DbName", dbText.Text.Trim());
			WriteIni("connect", "DbTable", tableText.Text.Trim());
			WriteIni("connect", "DBUser", userText.Text.Trim());
			WriteIni("connect", "DBPass", base64.Encode(pwText.Text.Trim()));

			// discord設定適用
			WriteIni("checkbox", "dconnect", (Convert.ToInt32(dconEnableCheck.Checked)).ToString());
			if (dconRatingRadio1.Checked)
			{
				WriteIni("checkbox", "rate", "0");
			}
			else if (dconRatingRadio2.Checked)
			{
				WriteIni("checkbox", "rate", "1");
			}
			WriteIni("connect", "dconpath", dconText.Text);
			WriteIni("connect", "dconappid", dconAppIDText.Text);

			// 棒読みちゃん設定適用
			WriteIni("connect", "byActive", (Convert.ToInt32(bouyomiEnableCheck.Checked)).ToString());
			WriteIni("connect", "byType", ByType.ToString());
			WriteIni("connect", "byPort", textBox5.Text);
			WriteIni("connect", "byHost", textBox4.Text);
			WriteIni("connect", "byRoW", RoWCheck.Checked ? "1" : "0");
			WriteIni("connect", "byRoS", RoSCheck.Checked ? "1" : "0");
			WriteIni("connect", "byRoG", RoGCheck.Checked ? "1" : "0");

			// 抽出
			WriteIni("Extract", "Enabled", (Convert.ToInt32(enableExtractCheck.Checked)).ToString());

			// データベースをローカルにINIで保存する
			if (offlineSaveEnableCheck.Checked && saveWithDownloadCheck.Checked)
			{
				if (mssqlRadio.Checked || mysqlRadio.Checked)
				{
					string oldButtonText = saveButton.Text;
					saveButton.Enabled = false;
					saveButton.Text = "データ取得中…";
					Application.DoEvents();

					if (!downloadDbDataToLocal(LocalPath))
					{
						// ローカル保存に失敗
						if (offlineSaveTypeOld == "0")
						{
							WriteIni("general", "OfflineSave", "0");
							MessageBox.Show("オフライン保存に失敗しました。\nオフライン機能は無効になります。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							MessageBox.Show("最新版のDB情報の取得に失敗しました。\n既にダウンロードされているものを使用します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
					else
					{
						// オフラインINIのフラグ変更
						WriteIni("list", "dbupdate", "0", 0, LocalPath);
					}
					saveButton.Enabled = true;
					saveButton.Text = oldButtonText;
				}
			}
			Close();
		}

		private void iniFolderSelectButton_Click(object sender, EventArgs e)
		{
			// 作業ディレクトリ変更(ini)
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				String newworkdir = folderBrowserDialog1.SelectedPath;

				if (!(newworkdir.EndsWith("\\")))
				{
					newworkdir += "\\";
				}
				WriteIni("default", "directory", newworkdir);

				// 作業ディレクトリに管理iniがない場合は0で初期化
				if (!File.Exists(newworkdir + "\\Data\\game.ini"))
				{
					WriteIni("list", "game", "0", 0, newworkdir);
				}

				// textbox反映
				GameDir = ReadIni("default", "directory", BaseDir);
				iniText.Text = newworkdir;
			}
			return;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.webLinkLabel.LinkVisited = true;
			System.Diagnostics.Process.Start("https://fanet.work");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.mailLinkLabel.LinkVisited = true;
			Clipboard.SetText("support@fanet.work");
		}
		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.githubLinkLabel.LinkVisited = true;
			System.Diagnostics.Process.Start("https://github.com/dekotan24/glc_cs");
		}

		private void byResetButton_Click(object sender, EventArgs e)
		{
			textBox4.Text = "127.0.0.1";
			if (ByType == 0)
			{
				textBox5.Text = "50001";
			}
			else
			{
				textBox5.Text = "50080";
			}
		}

		private void byConnectionTestButton_Click(object sender, EventArgs e)
		{
			ByHost = textBox4.Text;
			ByPort = Convert.ToInt32(textBox5.Text);

			Bouyomi_Connectchk();
		}


		private void bouyomiEnableCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (bouyomiEnableCheck.Checked)
			{
				groupBox4.Enabled = true;
				groupBox5.Enabled = true;
			}
			else
			{
				groupBox4.Enabled = false;
				groupBox5.Enabled = false;
			}
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			textBox4.Enabled = true;
			textBox5.Text = "50001";
			ByType = 0;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			textBox4.Enabled = false;
			textBox5.Text = "50080";
			ByType = 1;
		}

		/// <summary>
		/// dconパス変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dconSearchButton_Click(object sender, EventArgs e)
		{
			String newpath;

			// dcon.jar選択
			openFileDialog1.Title = "\"dcon.jar\"を選択";
			openFileDialog1.Filter = "Discord Connector|dcon.jar";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				newpath = openFileDialog1.FileName;
				dconText.Text = newpath;
				label11.Text = "OK";
			}
			return;
		}

		/// <summary>
		/// DB自動作成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void createTableButton_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				MessageBox.Show("URLは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (mysqlRadio.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			DbUrl = urlText.Text.Trim();
			DbPort = portText.Text.Trim();
			DbName = dbText.Text.Trim();
			DbTable = tableText.Text.Trim();
			DbUser = userText.Text.Trim();
			DbPass = pwText.Text.Trim();
			SaveType = mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I";

			if (string.IsNullOrEmpty(DbTable))
			{
				DbTable = "gl_item1";
			}

			if (SaveType == "D")
			{
				SqlConnection cn = SqlCon;
				SqlCommand cm;
				SqlTransaction tr = null;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"CREATE DATABASE [dekosoft_gl]"
				};
				cm.Connection = cn;

				// データベース作成
				try
				{
					cn.Open();
					cm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					cn.Close();
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					label15.Text = "データベース作成中にエラーが発生しました。エラーログをご覧下さい";
					MessageBox.Show("データベース作成中にエラーが発生しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}


				// テーブル作成
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"CREATE TABLE [dekosoft_gl].[dbo].[" + DbTable + "] ( [ID] INT IDENTITY(1,1) NOT NULL, [GAME_NAME] NVARCHAR(255), [GAME_PATH] NVARCHAR(MAX), [IMG_PATH] NVARCHAR(MAX), [UPTIME] NVARCHAR(255), [RUN_COUNT] NVARCHAR(99), [DCON_TEXT] NVARCHAR(50), [AGE_FLG] NVARCHAR(1), [TEMP1] NVARCHAR(255) NULL, [LAST_RUN] DATETIME NOT NULL DEFAULT N'" + Convert.ToDateTime("1900-01-01 00:00:00") + "', [DCON_IMG] NVARCHAR(50) NULL, [MEMO] NVARCHAR(500) NULL, [STATUS] NVARCHAR(10) NULL DEFAULT N'未プレイ', [DB_VERSION] NVARCHAR(5) NOT NULL DEFAULT N'" + DBVer + "' )"
				};
				cm2.Connection = cn;

				try
				{
					cn.Open();
					tr = cn.BeginTransaction();
					cm2.Transaction = tr;

					cm2.ExecuteNonQuery();
					tr.Commit();

					dbText.Text = "dekosoft_gl";
					tableText.Text = "dbo." + DbTable;
					DbName = dbText.Text.Trim();
					DbTable = tableText.Text.Trim();

					label15.Text = "テーブル作成が完了しました。";
				}
				catch (Exception ex)
				{
					if (tr != null)
					{
						tr.Rollback();
					}
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm2.CommandText);
					label15.Text = "テーブル作成中にエラーが発生しました。エラーログをご覧下さい";
					MessageBox.Show("テーブル作成中にエラーが発生しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MySqlConnection cn = SqlCon2;
				MySqlTransaction tr = null;

				// テーブル作成
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"CREATE TABLE " + DbTable + " ( ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT, GAME_NAME NVARCHAR(255), GAME_PATH NVARCHAR(500), IMG_PATH NVARCHAR(500), UPTIME NVARCHAR(255), RUN_COUNT NVARCHAR(99), DCON_TEXT NVARCHAR(50), AGE_FLG NVARCHAR(1), TEMP1 NVARCHAR(255) NULL, LAST_RUN DATETIME NOT NULL DEFAULT N'" + Convert.ToDateTime("1900-01-01 00:00:00") + "', DCON_IMG NVARCHAR(50) NULL, MEMO NVARCHAR(500) NULL, STATUS NVARCHAR(10) NULL DEFAULT N'未プレイ', DB_VERSION NVARCHAR(5) NOT NULL DEFAULT N'" + DBVer + "' )"
				};
				cm2.Connection = cn;

				try
				{
					cn.Open();
					tr = cn.BeginTransaction();
					cm2.Transaction = tr;

					cm2.ExecuteNonQuery();
					tr.Commit();

					tableText.Text = DbTable;
					DbName = dbText.Text.Trim();

					label15.Text = "テーブル作成が完了しました。";
				}
				catch (Exception ex)
				{
					if (tr != null)
					{
						tr.Rollback();
					}
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm2.CommandText);
					label15.Text = "テーブル作成中にエラーが発生しました。エラーログをご覧下さい";
					MessageBox.Show("テーブル作成中にエラーが発生しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}

			System.Media.SystemSounds.Beep.Play();
			return;
		}

		/// <summary>
		/// Discord連携有効チェック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (dconEnableCheck.Checked)
			{
				groupBox2.Enabled = true;
				groupBox6.Enabled = true;
				groupBox13.Enabled = true;
			}
			else
			{
				groupBox2.Enabled = false;
				groupBox6.Enabled = false;
				groupBox13.Enabled = false;
			}
		}

		private void iniAllEditButton_Click(object sender, EventArgs e)
		{
			StringBuilder sb = new StringBuilder();
			DialogResult dr = new DialogResult();
			string errMsg = string.Empty;
			int sucCount = -1;

			if (checkBox3.Checked)
			{
				sb.Append("[ゲームの実行ファイルパス]\n");
			}
			if (checkBox5.Checked)
			{
				sb.Append("[ゲームの画像ファイルパス]\n");
			}

			if (sb.ToString().Length == 0)
			{
				return;
			}

			if (textBox8.Text.Trim().Length > 1 && textBox9.Text.Trim().Length > 1)
			{
				string beforeText = textBox8.Text.Trim();
				string afterText = textBox9.Text.Trim();
				dr = MessageBox.Show("現在ロードされている作業ディレクトリ\n" + GameDir + "\nにある全てのINIファイルの\n" + sb.ToString() + "について、\n【" + beforeText + "】→【" + afterText + "】\nへ一括置換します。\n\n実行後は取り消せません。実行しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dr == DialogResult.No)
				{
					return;
				}
				if (EditAllIniFile(textBox8.Text.Trim(), textBox9.Text.Trim(), checkBox3.Checked, checkBox5.Checked, out sucCount, out errMsg))
				{
					MessageBox.Show("成功しました。\n\n処理件数：" + sucCount, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("処理中にエラーが発生しました。\n処理を中断します。\n\n" + errMsg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			return;

		}

		/// <summary>
		/// 背景画像変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void backgroundImageSelectButton_Click(object sender, EventArgs e)
		{
			String newpath;

			// 画像選択
			openFileDialog1.Title = "背景画像を選択";
			openFileDialog1.Filter = "画像ファイル|*.png;*.jpg";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				newpath = openFileDialog1.FileName;
				backgroundImageText.Text = newpath;
			}
			return;
		}

		private void radioButton8_CheckedChanged(object sender, EventArgs e)
		{
			// iniMode
			setDirectoryControl(true);
		}

		private void radioButton9_CheckedChanged(object sender, EventArgs e)
		{
			// databaseMode(MSSQL)
			setDirectoryControl(false);
		}

		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			// databaseMode(MySQL)
			setDirectoryControl(false);
		}

		/// <summary>
		/// INI→DB変換
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importIniToDbButton_Click(object sender, EventArgs e)
		{
			int sCount = 0;
			int fCount = 0;

			if (iniRadio.Checked)
			{
				return;
			}
			else if (urlText.Text.Trim().Length == 0 || portText.Text.Trim().Length == 0 || dbText.Text.Trim().Length == 0 || tableText.Text.Trim().Length == 0 || userText.Text.Trim().Length == 0 || pwText.Text.Trim().Length == 0)
			{
				return;
			}

			DbUrl = urlText.Text.Trim();
			DbPort = portText.Text.Trim();
			DbName = dbText.Text.Trim();
			DbTable = tableText.Text.Trim();
			DbUser = userText.Text.Trim();
			DbPass = pwText.Text.Trim();
			SaveType = mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I";

			bool deleteAllRecodes = checkBox6.Checked;
			bool forceCommit = checkBox7.Checked;

			if (File.Exists(GameIni) == false)
			{
				MessageBox.Show("ゲームファイルが存在しません。\n[ディレクトリ関連]タブで、INIファイルの作業ディレクトリを更新してください。\n\n" + GameIni, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			string iniCount = ReadIni("list", "game", "取得できませんでした。", 0);

			if (MessageBox.Show("作業ディレクトリの全データを" + (SaveType == "D" ? "MSSQL" : "MySQL") + "に取り込みます。\n\n作業ディレクトリ\t\t：[" + GameDir + "]（全" + iniCount + "件）\nデータベース接続先\t：[" + DbUrl + ":" + DbPort + "]\nデータベース名\t\t：[" + DbName + "]\nデータベーステーブル名\t：[" + DbTable + "]\n\n続行しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			// ini全件数取得
			string backupPath = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";
			int tmpMaxGameCount = 0;

			// ボタンの状態を変更
			importIniToDbButton.Enabled = false;
			importIniToDbButton.Text = "取込中…";
			Application.DoEvents();

			int returnVal = InsertIni2Db(GameDir, backupPath, out tmpMaxGameCount, out sCount, out fCount);

			// ボタンの状態を変更
			importIniToDbButton.Enabled = true;
			importIniToDbButton.Text = "INIファイルのデータをDBに取込";

			if (returnVal == 0)
			{
				MessageBox.Show("取込処理が完了しました。\n全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("エラーが発生しました。\n詳細はエラーログをご覧下さい。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox6.Checked)
			{
				MessageBox.Show("※※※　警告　※※※\n\nこのチェックを入れた状態で取り込みを開始すると、\n既にデータベースに取り込まれているゲームデータが\n消失します。\n\nクリーンな状態でINIファイルのデータを取り込みたい\n場合に便利な機能です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		/// <summary>
		/// ディレクトリ関連タブのコントロールをセットします。
		/// </summary>
		/// <param name="controlVal">iniモード</param>
		private void setDirectoryControl(bool controlVal)
		{
			// ini controlVal
			label9.Enabled = controlVal;
			iniText.Enabled = controlVal;
			iniFolderSelectButton.Enabled = controlVal;
			iniAutoNumberingFixButton.Enabled = controlVal;

			// database controlVal
			label16.Enabled = !controlVal;
			urlText.Enabled = !controlVal;
			label29.Enabled = !controlVal;
			portText.Enabled = !controlVal;
			label23.Enabled = !controlVal;
			dbText.Enabled = !controlVal;
			label24.Enabled = !controlVal;
			tableText.Enabled = !controlVal;
			label18.Enabled = !controlVal;
			userText.Enabled = !controlVal;
			label22.Enabled = !controlVal;
			pwText.Enabled = !controlVal;
			createTableButton.Enabled = !controlVal;
			offlineSaveEnableCheck.Enabled = !controlVal;
			useLocalDBCheck.Enabled = !controlVal;
			saveWithDownloadCheck.Enabled = !controlVal;
			dbBackupButton.Enabled = !controlVal;

			groupBox7.Enabled = controlVal;
			groupBox12.Enabled = !controlVal;
			dbOverflowFixButton.Enabled = !controlVal;
			saveWithDownloadCheck.Enabled = !controlVal;
		}

		private void iniAutoNumberingFixButton_Click(object sender, EventArgs e)
		{
			string readini = string.Empty;
			string gameDirName = GameDir;
			int fileCount = 0;
			int hasErrorIni = 0;
			int tempFileCount = 0;
			int baseFileCount = 0;
			bool overflowIni = false;

			// 全ゲーム数取得
			if (File.Exists(GameIni))
			{
				fileCount = Convert.ToInt32(ReadIni("list", "game", "0", 0));
			}

			if (fileCount >= 1) // ゲーム登録数が1以上の場合
			{
				// 通常ファイル存在チェック
				for (int curCount = 1; curCount <= fileCount; curCount++)
				{
					readini = gameDirName + curCount + ".ini";

					if (!File.Exists(readini))
					{
						hasErrorIni++;
					}
				}

				// 最大値より大きいiniファイル存在チェック
				tempFileCount = fileCount + 1;
				baseFileCount = (int.MaxValue - 1001 > tempFileCount) ? tempFileCount + 1000 : int.MaxValue - 1000;
				readini = gameDirName + tempFileCount.ToString() + ".ini";
				// 既に登録されているゲーム数+1000（もしくはIntの最大値）上限突破しているファイルがあるか調べる
				while (tempFileCount < baseFileCount)
				{
					if (File.Exists(readini))
					{
						overflowIni = true;
						break;
					}
					tempFileCount++;
					readini = gameDirName + tempFileCount.ToString() + ".ini";
				}

				// ファイルエラー修正確認
				if (hasErrorIni > 0 || overflowIni)
				{
					DialogResult dr = MessageBox.Show("欠番しているINIファイルが " + hasErrorIni + "件" + (overflowIni ? "、\n範囲外に存在するINIファイルが" : "") + "あります。\n連番となるように修正しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dr == DialogResult.Yes)
					{
						// 自動整番処理
						int result = iniReNumbering((overflowIni ? baseFileCount : 0));
						string newFileCount = ReadIni("list", "game", "0", 0);
						MessageBox.Show("整番が完了しました！\n\n欠番処理件数：" + result + "件\n登録済みゲーム総数：" + fileCount + "→" + newFileCount + "件", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					MessageBox.Show("欠番データは見つかりませんでした。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				MessageBox.Show("登録済みデータはありません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void dbOverflowFixButton_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				MessageBox.Show("URLは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (mysqlRadio.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			DbUrl = urlText.Text.Trim();
			DbPort = portText.Text.Trim();
			DbName = dbText.Text.Trim();
			DbTable = tableText.Text.Trim();
			DbUser = userText.Text.Trim();
			DbPass = pwText.Text.Trim();
			SaveType = mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I";

			// 修正
			SqlConnection cn = SqlCon;
			SqlCommand cm;
			SqlCommand cm2;
			SqlTransaction tr = null;

			MySqlConnection mcn = SqlCon2;
			MySqlCommand mcm;
			MySqlCommand mcm2;
			MySqlTransaction mtr = null;
			int totalRepairRows = 0;

			if (SaveType == "D")
			{
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET UPTIME = N'" + Int32.MaxValue + "' WHERE CAST(UPTIME AS BIGINT) > " + Int32.MaxValue
				};
				cm.Connection = cn;


				cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET RUN_COUNT = N'" + Int32.MaxValue + "' WHERE CAST(RUN_COUNT AS BIGINT) > " + Int32.MaxValue
				};
				cm2.Connection = cn;

				try
				{
					cn.Open();
					tr = cn.BeginTransaction();
					cm.Transaction = tr;
					cm2.Transaction = tr;

					totalRepairRows = cm.ExecuteNonQuery();
					totalRepairRows += cm2.ExecuteNonQuery();
					tr.Commit();
					label15.Text = "テーブルの修復が完了しました（" + totalRepairRows + "件）";
				}
				catch (Exception ex)
				{
					if (tr != null)
					{
						tr.Rollback();
					}
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					label15.Text = "テーブル修復中にエラーが発生しました。エラーログをご覧下さい";
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
				mcm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbTable + " SET UPTIME = N'" + Int32.MaxValue + "' WHERE CAST(UPTIME AS SIGNED) > " + Int32.MaxValue
				};
				mcm.Connection = mcn;


				mcm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbTable + " SET RUN_COUNT = N'" + Int32.MaxValue + "' WHERE CAST(RUN_COUNT AS SIGNED) > " + Int32.MaxValue
				};
				mcm2.Connection = mcn;

				try
				{
					mcn.Open();
					mtr = mcn.BeginTransaction();
					mcm.Transaction = mtr;
					mcm2.Transaction = mtr;

					totalRepairRows = mcm.ExecuteNonQuery();
					totalRepairRows += mcm2.ExecuteNonQuery();
					mtr.Commit();
					label15.Text = "テーブルの修復が完了しました（" + totalRepairRows + "件）";
				}
				catch (Exception ex)
				{
					if (mtr != null)
					{
						mtr.Rollback();
					}
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, mcm.CommandText);
					label15.Text = "テーブル修復中にエラーが発生しました。エラーログをご覧下さい";
				}
				finally
				{
					if (mcn.State == ConnectionState.Open)
					{
						mcn.Close();
					}
				}
			}

			return;
		}

		/// <summary>
		/// Discord RPC Application IDテキスト初期化
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dconAppIDClearButton_Click(object sender, EventArgs e)
		{
			dconAppIDText.Text = string.Empty;
		}

		/// <summary>
		/// アップデートチェックボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void updchkButton_Click(object sender, EventArgs e)
		{
			updchkButton.Text = "Checking..";
			updchkButton.Enabled = false;
			WebClient wc = new WebClient();
			try
			{
				wc.Encoding = Encoding.UTF8;
				string text = wc.DownloadString("https://raw.githubusercontent.com/dekotan24/glc_cs/master/version");
				if (AppVer != text.Trim())
				{
					updchkButton.Text = "Update Available";
					updchkButton.Enabled = true;
					MessageBox.Show("アップデートがあります。\n（" + AppVer + " -> " + text.Trim() + "）", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					System.Diagnostics.Process.Start("https://github.com/dekotan24/glc_cs/releases");
				}
				else
				{
					updchkButton.Text = "Latest!";
					updchkButton.Enabled = true;
					MessageBox.Show("最新版です。（" + text.Trim() + "）", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, string.Empty);
				label15.Text = "アップデートチェック中にエラーが発生しました。エラーログをご覧下さい";
				updchkButton.Text = "Check Update";
				updchkButton.Enabled = true;
			}
		}

		/// <summary>
		/// dconダウンロードボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void getDconButton_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("https://mega.nz/folder/slACCBiB#RYGUVYRgC3AIg84drWjR9w");
		}

		/// <summary>
		/// アプリケーションロゴクリック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void logoPictureBox_Click(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// バックアップボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dbBackupButton_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				MessageBox.Show("URLは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (mysqlRadio.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			// ボタンテキストを反映
			string oldButtonText = dbBackupButton.Text;
			dbBackupButton.Text = "取得中…";
			dbBackupButton.Enabled = false;
			Application.DoEvents();

			DbUrl = urlText.Text.Trim();
			DbPort = portText.Text.Trim();
			DbName = dbText.Text.Trim();
			DbTable = tableText.Text.Trim();
			DbUser = userText.Text.Trim();
			DbPass = pwText.Text.Trim();
			SaveType = mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I";

			string backupPath = (BaseDir.EndsWith("\\") ? BaseDir : BaseDir + "\\") + "database_backup(" + (DateTime.Now).ToString().Replace("/", "_").Replace(":", "_").Replace(" ", "_") + ")\\";
			bool returnVal = downloadDbDataToLocal(backupPath);

			if (returnVal)
			{
				MessageBox.Show("バックアップを取得しました。\n\n" + backupPath, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("エラーが発生しました。\n詳細はエラーログをご覧下さい。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			dbBackupButton.Text = oldButtonText;
			dbBackupButton.Enabled = true;
		}

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			if (offlineSaveEnableCheck.Checked)
			{
				saveWithDownloadCheck.Visible = true;
				saveWithDownloadCheck.Checked = true;
			}
			else
			{
				saveWithDownloadCheck.Visible = false;
			}

			if (offlineSaveEnableCheck.Checked && offlineSaveEnableCheck.Focused)
			{
				// オフライン保存有効時にダイアログを表示
				MessageBox.Show("[" + offlineSaveEnableCheck.Text + "]を有効にすると、以下のタイミングで自動的にDBのバックアップが取得されます。\n\n・設定画面の[" + saveButton.Text + "]を押した時\n・アプリケーションを終了する時\n\nバックアップの保存先：" + BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\" + "\n\nまた、バックアップの取得に時間がかかる場合があります。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// INIファイルの再自動採番
		/// </summary>
		/// <param name="hasOverflowIni"></param>
		/// <returns>処理件数</returns>
		private int iniReNumbering(int overflowIniMaxCount)
		{
			bool needMoveIni = false;
			bool hasMovedIni = false;
			int currentSavedCount = 1;
			int baseFileCount = 0;
			int movedCount = 0;
			int curCount = 0;
			string readini = string.Empty;
			string newReadIni = string.Empty;
			string gameDirName = GameDir;

			// ゲーム統括管理iniの最大値を取得する
			if (File.Exists(GameIni))
			{
				baseFileCount = Convert.ToInt32(ReadIni("list", "game", "0", 0));
			}

			try
			{
				// ループで回しながら処理する
				for (curCount = 1; curCount <= baseFileCount; curCount++)
				{
					readini = gameDirName + curCount + ".ini";

					if (File.Exists(readini))
					{
						if (needMoveIni || hasMovedIni)
						{
							// 移動フラグが立っている場合、ファイルを移動する
							needMoveIni = false;
							newReadIni = gameDirName + currentSavedCount + ".ini";
							File.Move(readini, newReadIni);
							movedCount++;
							hasMovedIni = true;
						}
						currentSavedCount++;
					}
					else
					{
						// 移動フラグを立て、次にファイルが存在した時に移動するようにする
						needMoveIni = true;
					}
				}

				// ゲーム統括管理iniの範囲外にiniファイルが存在する場合に処理を行う
				if (overflowIniMaxCount > 0)
				{
					while (curCount < overflowIniMaxCount)
					{
						readini = gameDirName + curCount + ".ini";
						if (File.Exists(readini))
						{
							if (needMoveIni || hasMovedIni)
							{
								// 移動フラグが立っている場合、ファイルを移動する
								needMoveIni = false;
								newReadIni = gameDirName + currentSavedCount + ".ini";
								File.Move(readini, newReadIni);
								movedCount++;
								hasMovedIni = true;
							}
							currentSavedCount++;
						}
						else
						{
							// 移動フラグを立て、次にファイルが存在した時に移動するようにする
							needMoveIni = true;
						}

						curCount++;
					}
				}

				// 最終処理
				currentSavedCount--;
				if (currentSavedCount > -1)
				{
					WriteIni("list", "game", currentSavedCount.ToString(), 0);
					if (SaveType == "T")
					{
						WriteIni("list", "dbupdate", "1", 0);
					}
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, readini);
			}

			return movedCount;
		}

		/// <summary>
		/// クエリ実行ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queryExecuteButton_Click(object sender, EventArgs e)
		{
			if (queryText.Text.Trim().Length != 0 && MessageBox.Show("入力したSQLクエリを実行しますか？\n\nクエリはトランザクションで実行され、\nエラーが発生した場合はロールバックされます。\n\n※元に戻すことはできません！\n\nHost：" + urlText.Text.Trim() + " / Port：" + portText.Text.Trim() + "\nDatabase：" + dbText.Text.Trim() + " / Table：" + tableText.Text.Trim() + "\nUser：" + userText.Text.Trim() + " / Pass：*** Your Password ***", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				// Execute

				if (urlText.Text.Trim().Length < 1)
				{
					MessageBox.Show("URLは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tabControl1.SelectedIndex = 3;
					urlText.Focus();
					return;
				}
				else if (portText.Text.Trim().Length < 1)
				{
					MessageBox.Show("ポート番号は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tabControl1.SelectedIndex = 3;
					portText.Focus();
					return;
				}
				else if (userText.Text.Trim().Length < 1)
				{
					MessageBox.Show("ユーザ名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tabControl1.SelectedIndex = 3;
					userText.Focus();
					return;
				}
				else if (pwText.Text.Trim().Length < 1)
				{
					MessageBox.Show("パスワードは必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tabControl1.SelectedIndex = 3;
					pwText.Focus();
					return;
				}
				else if (tableText.Text.Trim().Length < 1)
				{
					MessageBox.Show("テーブル名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tabControl1.SelectedIndex = 3;
					tableText.Focus();
					return;
				}

				// MySQLだけDatabaseも補填していないとエラーとする
				if (mysqlRadio.Checked)
				{
					if (dbText.Text.Trim().Length < 1)
					{
						MessageBox.Show("データベース名は必須です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						tabControl1.SelectedIndex = 3;
						dbText.Focus();
						return;
					}
				}

				DbUrl = urlText.Text.Trim();
				DbPort = portText.Text.Trim();
				DbName = dbText.Text.Trim();
				DbTable = tableText.Text.Trim();
				DbUser = userText.Text.Trim();
				DbPass = pwText.Text.Trim();
				SaveType = mssqlRadio.Checked ? "D" : mysqlRadio.Checked ? "M" : "I";

				// 修正
				SqlConnection cn = SqlCon;
				SqlCommand cm;
				SqlTransaction tr = null;

				MySqlConnection mcn = SqlCon2;
				MySqlCommand mcm;
				MySqlTransaction mtr = null;

				string queryResult = string.Empty;

				if (SaveType == "D")
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @queryText.Text
					};
					cm.Connection = cn;

					try
					{
						cn.Open();
						tr = cn.BeginTransaction();
						cm.Transaction = tr;

						if (executeNonQueryRadio.Checked)
						{
							queryResult = "影響行数：" + cm.ExecuteNonQuery().ToString();
						}
						else
						{
							queryResult = cm.ExecuteScalar().ToString();
						}

						tr.Commit();
						if (queryResult.Length != 0)
						{
							MessageBox.Show(@queryResult, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						if (tr != null)
						{
							tr.Rollback();
						}
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						MessageBox.Show("実行に失敗しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					mcm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @queryText.Text
					};
					mcm.Connection = mcn;

					try
					{
						mcn.Open();
						mtr = mcn.BeginTransaction();
						mcm.Transaction = mtr;

						if (executeNonQueryRadio.Checked)
						{
							queryResult = "影響行数：" + mcm.ExecuteNonQuery().ToString();
						}
						else
						{
							queryResult = mcm.ExecuteScalar().ToString();
						}

						mtr.Commit();
						if (queryResult.Length != 0)
						{
							MessageBox.Show(@queryResult, AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						if (mtr != null)
						{
							mtr.Rollback();
						}
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, mcm.CommandText);
						MessageBox.Show("実行に失敗しました。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					finally
					{
						if (mcn.State == ConnectionState.Open)
						{
							mcn.Close();
						}
					}
				}
			}
			return;
		}

		/// <summary>
		/// クエリクリアボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void queryClearButton_Click(object sender, EventArgs e)
		{
			queryText.Clear();
			return;
		}

		private void insertTableNameButton_Click(object sender, EventArgs e)
		{
			queryText.AppendText(tableText.Text.Trim());
			return;
		}

		private void insertDatabaseNameButton_Click(object sender, EventArgs e)
		{
			queryText.AppendText(dbText.Text.Trim());
			return;
		}

		private void insertColumnButton_Click(object sender, EventArgs e)
		{
			string appendText = string.Empty;
			switch (insertColumnsDropDown.SelectedIndex)
			{
				case 0:
					appendText = "ID";
					break;
				case 1:
					appendText = "GAME_NAME";
					break;
				case 2:
					appendText = "GAME_PATH";
					break;
				case 3:
					appendText = "IMG_PATH";
					break;
				case 4:
					appendText = "UPTIME";
					break;
				case 5:
					appendText = "RUN_COUNT";
					break;
				case 6:
					appendText = "DCON_TEXT";
					break;
				case 7:
					appendText = "AGE_FLG";
					break;
				case 8:
					appendText = "LAST_RUN";
					break;
				case 9:
					appendText = "DCON_IMG";
					break;
				case 10:
					appendText = "MEMO";
					break;
				case 11:
					appendText = "STATUS";
					break;
				case 12:
					appendText = "DB_VERSION";
					break;
			}
			queryText.AppendText(appendText + " ");
			queryText.Focus();
			return;
		}

		private void updateCheckDisableCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (updateCheckDisableCheck.Checked && updateCheckDisableCheck.Focused)
			{
				MessageBox.Show("これは毎起動時に行われるアップデートチェックによる負荷軽減のための機能です。\n\n※※※警告※※※\n将来バージョンのアップデートをiniファイルの値を直接書き換えて回避する等、本機能を不正に使用した場合に発生する いかなる損害・損失は一切責任を負いません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void fixGridSize_CheckedChanged(object sender, EventArgs e)
		{
			if (fixGridSizeCheck.Checked)
			{
				fixGridSize8.Enabled = fixGridSize32.Enabled = fixGridSize64.Enabled = true;
			}
			else
			{
				fixGridSize8.Enabled = fixGridSize32.Enabled = fixGridSize64.Enabled = false;
			}
		}

		private void gridDisableCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (gridDisableCheck.Checked)
			{
				fixGridSizeCheck.Enabled = false;
				if (fixGridSizeCheck.Checked)
				{
					fixGridSize8.Enabled = fixGridSize32.Enabled = fixGridSize64.Enabled = false;
				}
			}
			else
			{
				fixGridSizeCheck.Enabled = true;
				if (fixGridSizeCheck.Checked)
				{
					fixGridSize8.Enabled = fixGridSize32.Enabled = fixGridSize64.Enabled = true;
				}
			}
		}

		private void calcExecPlanButton_Click(object sender, EventArgs e)
		{
			if (GenerateExtractCmd(extractToolSelectCombo.SelectedIndex, "[ゲーム実行パス]", "[ゲーム実行引数]", out string extractAppPath, out string extractAppArgs))
			{
				extractExecPlanText.Text = extractAppPath + " " + extractAppArgs;
			}
			else
			{
				extractExecPlanText.Text = string.Empty;
			}

			return;
		}

		private void extractToolSelectButton_Click(object sender, EventArgs e)
		{
			openFileDialog2.Title = "抽出ツールを選択";
			openFileDialog2.Filter = "実行ファイル(*.exe)|*.exe|すべてのファイル(*.*)|*.*";
			openFileDialog2.FileName = "";
			if (openFileDialog2.ShowDialog() == DialogResult.OK)
			{
				extractToolPathText.Text = openFileDialog2.FileName;
			}

			return;
		}

		private void extractToolSelectCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			extractExecPlanText.Text = string.Empty;
			switch (extractToolSelectCombo.SelectedIndex)
			{
				case 1: // krkr
					extractToolPathText.Text = ExtractKrkrPath;
					extractToolArgText.Text = ExtractKrkrArg;
					addGameArgCheck.Checked = ExtractKrkrAddGameArg;
					extractCurrentDirCheck.Checked = ExtractKrkrCurDir;
					addGameDirCheck.Checked = ExtractKrkrGameDir;
					break;
				case 2: // krkrz
					extractToolPathText.Text = ExtractKrkrzPath;
					extractToolArgText.Text = ExtractKrkrzArg;
					addGameArgCheck.Checked = ExtractKrkrzAddGameArg;
					extractCurrentDirCheck.Checked = ExtractKrkrzCurDir;
					addGameDirCheck.Checked = ExtractKrkrzGameDir;
					break;
				case 3: // krkrDump
					extractToolPathText.Text = ExtractKrkrDumpPath;
					extractToolArgText.Text = ExtractKrkrDumpArg;
					addGameArgCheck.Checked = ExtractKrkrDumpAddGameArg;
					extractCurrentDirCheck.Checked = ExtractKrkrDumpCurDir;
					addGameDirCheck.Checked = ExtractKrkrDumpGameDir;
					break;
				case 4: // カスタム1
					extractToolPathText.Text = ExtractCustom1Path;
					extractToolArgText.Text = ExtractCustom1Arg;
					addGameArgCheck.Checked = ExtractCustom1AddGameArg;
					extractCurrentDirCheck.Checked = ExtractCustom1CurDir;
					addGameDirCheck.Checked = ExtractCustom1GameDir;
					break;
				case 5: // カスタム2
					extractToolPathText.Text = ExtractCustom2Path;
					extractToolArgText.Text = ExtractCustom2Arg;
					addGameArgCheck.Checked = ExtractCustom2AddGameArg;
					extractCurrentDirCheck.Checked = ExtractCustom2CurDir;
					addGameDirCheck.Checked = ExtractCustom2GameDir;
					break;
				default:
					extractToolPathText.Text = string.Empty;
					extractToolArgText.Text = string.Empty;
					addGameArgCheck.Checked = false;
					extractCurrentDirCheck.Checked = false;
					addGameDirCheck.Checked = false;
					break;
			}
		}

		private void extractSaveButton_Click(object sender, EventArgs e)
		{
			try
			{
				switch (extractToolSelectCombo.SelectedIndex)
				{
					case 1: // krkr
						WriteIni("Extract", "krkrPath", extractToolPathText.Text.Trim());
						WriteIni("Extract", "krkrArg", extractToolArgText.Text.Trim());
						WriteIni("Extract", "krkrAddGameArg", addGameArgCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrCurDir", extractCurrentDirCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrGameDir", addGameDirCheck.Checked ? "1" : "0");
						ExtractKrkrPath = extractToolPathText.Text.Trim();
						ExtractKrkrArg = extractToolArgText.Text.Trim();
						ExtractKrkrAddGameArg = Convert.ToBoolean(Convert.ToInt32(addGameArgCheck.Checked ? "1" : "0"));
						ExtractKrkrCurDir = Convert.ToBoolean(Convert.ToInt32(extractCurrentDirCheck.Checked ? "1" : "0"));
						ExtractKrkrGameDir = Convert.ToBoolean(Convert.ToInt32(addGameDirCheck.Checked ? "1" : "0"));
						break;
					case 2: // krkrz
						WriteIni("Extract", "krkrzPath", extractToolPathText.Text.Trim());
						WriteIni("Extract", "krkrzArg", extractToolArgText.Text.Trim());
						WriteIni("Extract", "krkrzAddGameArg", addGameArgCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrzCurDir", extractCurrentDirCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrzGameDir", addGameDirCheck.Checked ? "1" : "0");
						ExtractKrkrzPath = extractToolPathText.Text.Trim();
						ExtractKrkrzArg = extractToolArgText.Text.Trim();
						ExtractKrkrzAddGameArg = Convert.ToBoolean(Convert.ToInt32(addGameArgCheck.Checked ? "1" : "0"));
						ExtractKrkrzCurDir = Convert.ToBoolean(Convert.ToInt32(extractCurrentDirCheck.Checked ? "1" : "0"));
						ExtractKrkrzGameDir = Convert.ToBoolean(Convert.ToInt32(addGameDirCheck.Checked ? "1" : "0"));
						break;
					case 3: // krkrDump
						WriteIni("Extract", "krkrDumpPath", extractToolPathText.Text.Trim());
						WriteIni("Extract", "krkrDumpArg", extractToolArgText.Text.Trim());
						WriteIni("Extract", "krkrDumpAddGameArg", addGameArgCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrDumpCurDir", extractCurrentDirCheck.Checked ? "1" : "0");
						WriteIni("Extract", "krkrDumpGameDir", addGameDirCheck.Checked ? "1" : "0");
						ExtractKrkrDumpPath = extractToolPathText.Text.Trim();
						ExtractKrkrDumpArg = extractToolArgText.Text.Trim();
						ExtractKrkrDumpAddGameArg = Convert.ToBoolean(Convert.ToInt32(addGameArgCheck.Checked ? "1" : "0"));
						ExtractKrkrDumpCurDir = Convert.ToBoolean(Convert.ToInt32(extractCurrentDirCheck.Checked ? "1" : "0"));
						ExtractKrkrDumpGameDir = Convert.ToBoolean(Convert.ToInt32(addGameDirCheck.Checked ? "1" : "0"));
						break;
					case 4: // カスタム1
						WriteIni("Extract", "Custom1Path", extractToolPathText.Text.Trim());
						WriteIni("Extract", "Custom1Arg", extractToolArgText.Text.Trim());
						WriteIni("Extract", "Custom1AddGameArg", addGameArgCheck.Checked ? "1" : "0");
						WriteIni("Extract", "Custom1CurDir", extractCurrentDirCheck.Checked ? "1" : "0");
						WriteIni("Extract", "Custom1GameDir", addGameDirCheck.Checked ? "1" : "0");
						ExtractCustom1Path = extractToolPathText.Text.Trim();
						ExtractCustom1Arg = extractToolArgText.Text.Trim();
						ExtractCustom1AddGameArg = Convert.ToBoolean(Convert.ToInt32(addGameArgCheck.Checked ? "1" : "0"));
						ExtractCustom1CurDir = Convert.ToBoolean(Convert.ToInt32(extractCurrentDirCheck.Checked ? "1" : "0"));
						ExtractCustom1GameDir = Convert.ToBoolean(Convert.ToInt32(addGameDirCheck.Checked ? "1" : "0"));
						break;
					case 5: // カスタム2
						WriteIni("Extract", "Custom2Path", extractToolPathText.Text.Trim());
						WriteIni("Extract", "Custom2Arg", extractToolArgText.Text.Trim());
						WriteIni("Extract", "Custom2AddGameArg", addGameArgCheck.Checked ? "1" : "0");
						WriteIni("Extract", "Custom2CurDir", extractCurrentDirCheck.Checked ? "1" : "0");
						WriteIni("Extract", "Custom2GameDir", addGameDirCheck.Checked ? "1" : "0");
						ExtractCustom2Path = extractToolPathText.Text.Trim();
						ExtractCustom2Arg = extractToolArgText.Text.Trim();
						ExtractCustom2AddGameArg = Convert.ToBoolean(Convert.ToInt32(addGameArgCheck.Checked ? "1" : "0"));
						ExtractCustom2CurDir = Convert.ToBoolean(Convert.ToInt32(extractCurrentDirCheck.Checked ? "1" : "0"));
						ExtractCustom2GameDir = Convert.ToBoolean(Convert.ToInt32(addGameDirCheck.Checked ? "1" : "0"));
						break;
				}
				System.Media.SystemSounds.Beep.Play();
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("SelectedIndex:").Append(extractToolSelectCombo.SelectedIndex).Append(" / ");
				sb.Append("PathText:").Append(extractToolPathText.Text.Trim()).Append(" / ");
				sb.Append("ArgText:").Append(extractToolArgText.Text.Trim()).Append(" / ");
				sb.Append("AddGameArg:").Append(addGameArgCheck.Checked ? "1" : "0");

				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());

				MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return;
		}

		private void enableExtractCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (enableExtractCheck.Checked)
			{
				extractToolsGroup.Enabled = true;
			}
			else
			{
				extractToolsGroup.Enabled = false;
			}
		}
	}
}