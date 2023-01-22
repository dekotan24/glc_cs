using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class Form2 : Form
	{
		public Form2()
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
			if (General.Var.GLConfigLoad() == false)
			{
				label15.Text = "Configロード中にエラー。詳しくはエラーログを参照して下さい。";
			}

			//バージョン取得
			label10.Text = "Ver." + General.Var.AppVer + " Build " + General.Var.AppBuild;

			// 背景画像
			backgroundImageText.Text = General.Var.BgImg;

			// グリッド
			gridDisableCheck.Checked = !General.Var.GridEnable;

			//Discord設定読み込み
			bool dconActive = General.Var.Dconnect;
			dconAppIDText.Text = General.Var.DconAppID;

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
			if (General.Var.Rate == 1)
			{
				dconRatingRadio2.Checked = true;
			}
			else
			{
				dconRatingRadio1.Checked = true;
			}

			// Discord Connectorパス取得
			string dconpath = General.Var.DconPath;

			if (File.Exists(dconpath))
			{
				//指定パスにdcon.jar存在する場合
				dconText.Text = dconpath;
				label11.Text = "OK";
			}
			else
			{
				dconText.Text = string.Empty;
				label11.Text = "NG";
			}

			//棒読みちゃん設定読み込み
			bool isbyActive = General.Var.ByActive;
			bouyomiEnableCheck.Checked = isbyActive;
			if (isbyActive)
			{
				groupBox4.Enabled = true;
				groupBox5.Enabled = true;
			}

			if (General.Var.ByType == 1)
			{
				radioButton4.Checked = true;
			}
			else
			{
				radioButton3.Checked = true;
			}
			textBox4.Text = General.Var.ByHost;
			textBox5.Text = General.Var.ByPort.ToString();

			checkBox4.Checked = General.Var.ByRoS;
			checkBox10.Checked = General.Var.ByRoW;

			// 保存方法
			if (General.Var.SaveType == "I")
			{
				radioButton8.Checked = true;
				setDirectoryControl(true);
			}
			else if (General.Var.SaveType == "D")   // MSSQL
			{
				radioButton9.Checked = true;
				setDirectoryControl(false);
			}
			else if (General.Var.SaveType == "M")   // MySQL
			{
				radioButton5.Checked = true;
				setDirectoryControl(false);
			}
			else
			{
				switch (General.Var.ReadIni("general", "save", "I"))
				{
					case "I":
						radioButton8.Checked = true;
						break;
					case "D":
						radioButton9.Checked = true;
						break;
					case "M":
						radioButton5.Checked = true;
						break;
				}
			}

			urlText.Text = General.Var.DbUrl;
			portText.Text = General.Var.DbPort;
			dbText.Text = General.Var.DbName;
			tableText.Text = General.Var.DbTable;
			userText.Text = General.Var.DbUser;
			pwText.Text = General.Var.DbPass;

			checkBox8.Checked = General.Var.OfflineSave;

			// 作業ディレクトリ反映
			if (General.Var.SaveType != "T")
			{
				if (General.Var.GameDir.EndsWith("\\Data\\"))
				{
					textBox2.Text = General.Var.GameDir.Substring(0, General.Var.GameDir.Length - 5);
				}
				else if (General.Var.GameDir.EndsWith("\\Data"))
				{
					textBox2.Text = General.Var.GameDir.Substring(0, General.Var.GameDir.Length - 4);
				}
				else
				{
					textBox2.Text = General.Var.GameDir;
				}
			}
			else
			{
				string tmpRawGameDir = General.Var.ReadIni("default", "directory", General.Var.BaseDir.EndsWith("\\") ? General.Var.BaseDir : General.Var.BaseDir + "\\");
				if (tmpRawGameDir.EndsWith("\\Data\\"))
				{
					textBox2.Text = tmpRawGameDir.Substring(0, General.Var.GameDir.Length - 5);
				}
				else
				{
					textBox2.Text = tmpRawGameDir;
				}
			}
		}

		/// <summary>
		/// 保存ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button2_Click(object sender, EventArgs e)
		{
			string offlineSaveTypeOld = General.Var.ReadIni("general", "OfflineSave", checkBox8.Checked ? "1" : "0");
			bool canExit = true;
			if (radioButton9.Checked || radioButton5.Checked)
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
					MessageBox.Show("データの保存方法をデータベースにした場合、\n[URL]、[Port]、[DB]、[Table]、[User]は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			MyBase64str base64 = new MyBase64str();

			// 全般
			General.Var.WriteIni("imgd", "bgimg", backgroundImageText.Text.Trim());
			General.Var.WriteIni("disable", "grid", gridDisableCheck.Checked ? "1" : "0");

			// 保存方法
			General.Var.WriteIni("general", "save", radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I");
			General.Var.WriteIni("general", "OfflineSave", checkBox8.Checked ? "1" : "0");
			General.Var.WriteIni("connect", "DBURL", urlText.Text.Trim());
			General.Var.WriteIni("connect", "DBPort", portText.Text.Trim());
			General.Var.WriteIni("connect", "DbName", dbText.Text.Trim());
			General.Var.WriteIni("connect", "DbTable", tableText.Text.Trim());
			General.Var.WriteIni("connect", "DBUser", userText.Text.Trim());
			General.Var.WriteIni("connect", "DBPass", base64.Encode(pwText.Text.Trim()));

			//discord設定適用
			General.Var.WriteIni("checkbox", "dconnect", (Convert.ToInt32(dconEnableCheck.Checked)).ToString());
			if (dconRatingRadio1.Checked)
			{
				General.Var.WriteIni("checkbox", "rate", "0");
			}
			else if (dconRatingRadio2.Checked)
			{
				General.Var.WriteIni("checkbox", "rate", "1");
			}
			General.Var.WriteIni("connect", "dconpath", dconText.Text);
			General.Var.WriteIni("connect", "dconappid", dconAppIDText.Text);

			//棒読みちゃん設定適用
			General.Var.WriteIni("connect", "byActive", (Convert.ToInt32(bouyomiEnableCheck.Checked)).ToString());
			General.Var.WriteIni("connect", "byType", General.Var.ByType.ToString());
			General.Var.WriteIni("connect", "byPort", textBox5.Text);
			General.Var.WriteIni("connect", "byHost", textBox4.Text);

			// データベースをローカルにINIで保存する
			if (checkBox8.Checked)
			{
				if (radioButton9.Checked || radioButton5.Checked)
				{
					string localPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
					if (!General.Var.downloadDbDataToLocal(localPath))
					{
						// ローカル保存に失敗
						if (offlineSaveTypeOld == "0")
						{
							General.Var.WriteIni("general", "OfflineSave", "0");
							MessageBox.Show("オフライン保存に失敗しました。\nオフライン機能は無効になります。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
						else
						{
							MessageBox.Show("最新版のDB情報の取得に失敗しました。\n既にダウンロードされているものを使用します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
					else
					{
						// オフラインINIのフラグ変更
						General.Var.IniWrite((localPath + "game.ini"), "list", "dbupdate", "0");
					}
				}
			}
			Close();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			//作業ディレクトリ変更(ini)
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				String newworkdir = folderBrowserDialog1.SelectedPath;

				if (!(newworkdir.EndsWith("\\")))
				{
					newworkdir += "\\";
				}
				General.Var.WriteIni("default", "directory", newworkdir);

				// 作業ディレクトリに管理iniがない場合は0で初期化
				if (!File.Exists(newworkdir + "\\Data\\game.ini"))
				{
					General.Var.WriteIni("list", "game", "0", 0, newworkdir);
				}

				// textbox反映
				General.Var.GameDir = General.Var.ReadIni("default", "directory", General.Var.BaseDir);
				textBox2.Text = newworkdir;
			}
			return;
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel1.LinkVisited = true;
			System.Diagnostics.Process.Start("https://fanet.work");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel2.LinkVisited = true;
			Clipboard.SetText("support@fanet.work");
		}
		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel3.LinkVisited = true;
			System.Diagnostics.Process.Start("https://github.com/dekotan24/glc_cs");
		}

		private void button8_Click(object sender, EventArgs e)
		{
			textBox4.Text = "127.0.0.1";
			if (General.Var.ByType == 0)
			{
				textBox5.Text = "50001";
			}
			else
			{
				textBox5.Text = "50080";
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			General.Var.ByHost = textBox4.Text;
			General.Var.ByPort = Convert.ToInt32(textBox5.Text);

			General.Var.Bouyomi_Connectchk(General.Var.ByHost, General.Var.ByPort, General.Var.ByType);
		}


		private void checkBox2_CheckedChanged(object sender, EventArgs e)
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
			General.Var.ByType = 0;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			textBox4.Enabled = false;
			textBox5.Text = "50080";
			General.Var.ByType = 1;
		}

		/// <summary>
		/// dconパス変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			String newpath;

			//dcon.jar選択
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

		private void button5_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (radioButton5.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					return;
				}
			}

			General.Var.DbUrl = urlText.Text.Trim();
			General.Var.DbPort = portText.Text.Trim();
			General.Var.DbName = dbText.Text.Trim();
			General.Var.DbTable = tableText.Text.Trim();
			General.Var.DbUser = userText.Text.Trim();
			General.Var.DbPass = pwText.Text.Trim();

			if (General.Var.SaveType == "D")
			{
				SqlConnection cn = General.Var.SqlCon;
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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					label15.Text = "データベース作成中にエラーが発生しました。エラーログをご覧下さい";
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
					CommandText = @"CREATE TABLE [dekosoft_gl].[dbo].[gl_item1] ( [ID] INT IDENTITY(1,1) NOT NULL, [GAME_NAME] NVARCHAR(255), [GAME_PATH] NVARCHAR(MAX), [IMG_PATH] NVARCHAR(MAX), [UPTIME] NVARCHAR(255), [RUN_COUNT] NVARCHAR(99), [DCON_TEXT] NVARCHAR(50), [AGE_FLG] NVARCHAR(1), [TEMP1] NVARCHAR(255) NULL, [LAST_RUN] DATETIME NULL )"
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
					tableText.Text = "dbo.gl_item1";
					General.Var.DbName = dbText.Text.Trim();
					General.Var.DbTable = tableText.Text.Trim();

					label15.Text = "テーブル作成が完了しました。";
				}
				catch (Exception ex)
				{
					if (tr != null)
					{
						tr.Rollback();
					}
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm2.CommandText);
					label15.Text = "テーブル作成中にエラーが発生しました。エラーログをご覧下さい";
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
				MySqlConnection cn = General.Var.SqlCon2;
				MySqlCommand cm;
				MySqlTransaction tr = null;

				// テーブル作成
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"CREATE TABLE gl_item1 ( ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT, GAME_NAME NVARCHAR(255), GAME_PATH NVARCHAR(500), IMG_PATH NVARCHAR(500), UPTIME NVARCHAR(255), RUN_COUNT NVARCHAR(99), DCON_TEXT NVARCHAR(50), AGE_FLG NVARCHAR(1), TEMP1 NVARCHAR(255) NULL, LAST_RUN DATETIME NULL )"
				};
				cm2.Connection = cn;

				try
				{
					cn.Open();
					tr = cn.BeginTransaction();
					cm2.Transaction = tr;

					cm2.ExecuteNonQuery();
					tr.Commit();

					tableText.Text = "gl_item1";
					General.Var.DbName = dbText.Text.Trim();
					General.Var.DbTable = tableText.Text.Trim();

					label15.Text = "テーブル作成が完了しました。";
				}
				catch (Exception ex)
				{
					if (tr != null)
					{
						tr.Rollback();
					}
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm2.CommandText);
					label15.Text = "テーブル作成中にエラーが発生しました。エラーログをご覧下さい";
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

		private void button7_Click(object sender, EventArgs e)
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
				dr = MessageBox.Show("現在ロードされている作業ディレクトリ\n" + General.Var.GameDir + "\nにある全てのINIファイルの\n" + sb.ToString() + "について、\n【" + beforeText + "】→【" + afterText + "】\nへ一括置換します。\n\n実行後は取り消せません。実行しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dr == DialogResult.No)
				{
					return;
				}
				if (General.Var.EditAllFilePath(textBox8.Text.Trim(), textBox9.Text.Trim(), checkBox3.Checked, checkBox5.Checked, out sucCount, out errMsg))
				{
					MessageBox.Show("成功しました。\n\n処理件数：" + sucCount, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("処理中にエラーが発生しました。\n処理を中断します。\n\n" + errMsg, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}

			return;

		}

		/// <summary>
		/// 背景画像変更
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button6_Click(object sender, EventArgs e)
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
		private void button9_Click(object sender, EventArgs e)
		{
			int sCount = 0;
			int fCount = 0;

			if (radioButton8.Checked)
			{
				return;
			}
			else if (urlText.Text.Trim().Length == 0 || portText.Text.Trim().Length == 0 || dbText.Text.Trim().Length == 0 || tableText.Text.Trim().Length == 0 || userText.Text.Trim().Length == 0 || pwText.Text.Trim().Length == 0)
			{
				return;
			}

			General.Var.DbUrl = urlText.Text.Trim();
			General.Var.DbPort = portText.Text.Trim();
			General.Var.DbName = dbText.Text.Trim();
			General.Var.DbTable = tableText.Text.Trim();
			General.Var.DbUser = userText.Text.Trim();
			General.Var.DbPass = pwText.Text.Trim();

			bool deleteAllRecodes = checkBox6.Checked;
			bool forceCommit = checkBox7.Checked;

			if (MessageBox.Show("現在ロードしている作業ディレクトリ\n[" + General.Var.GameDir + "] の全てのゲームデータをDB\n[" + General.Var.DbUrl + " " + General.Var.DbName + "." + General.Var.DbTable + "] \nへ取り込みます。\n\n続行しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			// ini全件数取得
			string backupPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";
			int tmpMaxGameCount = 0;

			int returnVal = General.Var.InsertIni2Db(General.Var.GameDir, backupPath, out tmpMaxGameCount, out sCount, out fCount);

			if (returnVal == 0)
			{
				MessageBox.Show("取込処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("エラーが発生しました。\n詳細はエラーログをご覧下さい。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox6.Checked)
			{
				MessageBox.Show("※※※　警告　※※※\n\nこのチェックを入れた状態で取り込みを開始すると、\n既にデータベースに取り込まれているゲームデータが\n消失します。\n\nクリーンな状態でINIファイルのデータを取り込みたい\n場合に便利な機能です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
			textBox2.Enabled = controlVal;
			button4.Enabled = controlVal;

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
			button5.Enabled = !controlVal;
			checkBox8.Enabled = !controlVal;

			groupBox7.Enabled = controlVal;
			groupBox12.Enabled = !controlVal;
			groupBox8.Enabled = !controlVal;
		}

		private void button10_Click(object sender, EventArgs e)
		{
			// 修正
			SqlConnection cn = General.Var.SqlCon;
			SqlCommand cm;
			SqlCommand cm2;
			SqlTransaction tr = null;

			MySqlConnection mcn = General.Var.SqlCon2;
			MySqlCommand mcm;
			MySqlCommand mcm2;
			MySqlTransaction mtr = null;
			int totalRepairRows = 0;

			if (General.Var.SaveType == "D")
			{
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET UPTIME = N'" + Int32.MaxValue + "' WHERE CAST(UPTIME AS BIGINT) > " + Int32.MaxValue
				};
				cm.Connection = cn;


				cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET RUN_COUNT = N'" + Int32.MaxValue + "' WHERE CAST(RUN_COUNT AS BIGINT) > " + Int32.MaxValue
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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
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
					CommandText = @"UPDATE " + General.Var.DbTable + " SET UPTIME = N'" + Int32.MaxValue + "' WHERE CAST(UPTIME AS SIGNED) > " + Int32.MaxValue
				};
				mcm.Connection = mcn;


				mcm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + General.Var.DbTable + " SET RUN_COUNT = N'" + Int32.MaxValue + "' WHERE CAST(RUN_COUNT AS SIGNED) > " + Int32.MaxValue
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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, mcm.CommandText);
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
		private void button11_Click(object sender, EventArgs e)
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
				MessageBox.Show(text, "", MessageBoxButtons.OK, MessageBoxIcon.Information);
				if (General.Var.AppVer != text.Trim())
				{
					updchkButton.Text = "Update Available";
					updchkButton.Enabled = true;
					System.Diagnostics.Process.Start("https://github.com/dekotan24/glc_cs/releases");
				}
				else
				{
					updchkButton.Text = "Latest!";
					updchkButton.Enabled = true;
				}
			}
			catch (Exception ex)
			{
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, string.Empty);
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
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			return;
		}
	}
}