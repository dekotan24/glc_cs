﻿using MySql.Data.MySqlClient;
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

			checkBox4.Checked = General.Var.ByRoW;
			checkBox10.Checked = General.Var.ByRoS;

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

			// スーパーモード
			// ドロップダウン既定値設定
			insertColumnsDropDown.SelectedIndex = 0;

			// 作業ディレクトリ反映
			if (General.Var.SaveType != "T")
			{
				if (General.Var.GameDir.EndsWith("\\Data\\"))
				{
					iniText.Text = General.Var.GameDir.Substring(0, General.Var.GameDir.Length - 5);
				}
				else
				{
					iniText.Text = General.Var.GameDir;
				}
			}
			else
			{
				string tmpRawGameDir = General.Var.ReadIni("default", "directory", General.Var.BaseDir.EndsWith("\\") ? General.Var.BaseDir : General.Var.BaseDir + "\\");
				if (tmpRawGameDir.EndsWith("\\Data\\"))
				{
					iniText.Text = tmpRawGameDir.Substring(0, General.Var.GameDir.Length - 5);
				}
				else
				{
					iniText.Text = tmpRawGameDir;
				}
			}
		}

		/// <summary>
		/// 保存ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveButton_Click(object sender, EventArgs e)
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
			General.Var.WriteIni("connect", "byRoW", checkBox4.Checked ? "1" : "0");
			General.Var.WriteIni("connect", "byRoS", checkBox10.Checked ? "1" : "0");

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

		private void iniFolderSelectButton_Click(object sender, EventArgs e)
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
				iniText.Text = newworkdir;
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

		private void byResetButton_Click(object sender, EventArgs e)
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

		private void byConnectionTestButton_Click(object sender, EventArgs e)
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
		private void dconSearchButton_Click(object sender, EventArgs e)
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

		/// <summary>
		/// DB自動作成
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void createTableButton_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				MessageBox.Show("URLは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (radioButton5.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			General.Var.DbUrl = urlText.Text.Trim();
			General.Var.DbPort = portText.Text.Trim();
			General.Var.DbName = dbText.Text.Trim();
			General.Var.DbTable = tableText.Text.Trim();
			General.Var.DbUser = userText.Text.Trim();
			General.Var.DbPass = pwText.Text.Trim();
			General.Var.SaveType = radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I";

			if (string.IsNullOrEmpty(General.Var.DbTable))
			{
				General.Var.DbTable = "gl_item1";
			}

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
					MessageBox.Show("データベース作成中にエラーが発生しました。\n\n" + ex.Message, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					CommandText = @"CREATE TABLE [dekosoft_gl].[dbo].[" + General.Var.DbTable + "] ( [ID] INT IDENTITY(1,1) NOT NULL, [GAME_NAME] NVARCHAR(255), [GAME_PATH] NVARCHAR(MAX), [IMG_PATH] NVARCHAR(MAX), [UPTIME] NVARCHAR(255), [RUN_COUNT] NVARCHAR(99), [DCON_TEXT] NVARCHAR(50), [AGE_FLG] NVARCHAR(1), [TEMP1] NVARCHAR(255) NULL, [LAST_RUN] DATETIME NULL, [DCON_IMG] NVARCHAR(50) NULL, [MEMO] NVARCHAR(500) NULL, [STATUS] NVARCHAR(10) NULL DEFAULT N'未プレイ', [DB_VERSION] NVARCHAR(5) NOT NULL DEFAULT N'" + General.Var.DBVer + "' )"
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
					tableText.Text = "dbo." + General.Var.DbTable;
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
					MessageBox.Show("テーブル作成中にエラーが発生しました。\n\n" + ex.Message, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				MySqlTransaction tr = null;

				// テーブル作成
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"CREATE TABLE " + General.Var.DbTable + " ( ID INT NOT NULL PRIMARY KEY AUTO_INCREMENT, GAME_NAME NVARCHAR(255), GAME_PATH NVARCHAR(500), IMG_PATH NVARCHAR(500), UPTIME NVARCHAR(255), RUN_COUNT NVARCHAR(99), DCON_TEXT NVARCHAR(50), AGE_FLG NVARCHAR(1), TEMP1 NVARCHAR(255) NULL, LAST_RUN DATETIME NULL, DCON_IMG NVARCHAR(50) NULL, MEMO NVARCHAR(500) NULL, STATUS NVARCHAR(10) NULL DEFAULT N'未プレイ', DB_VERSION NVARCHAR(5) NOT NULL DEFAULT N'" + General.Var.DBVer + "' )"
				};
				cm2.Connection = cn;

				try
				{
					cn.Open();
					tr = cn.BeginTransaction();
					cm2.Transaction = tr;

					cm2.ExecuteNonQuery();
					tr.Commit();

					tableText.Text = General.Var.DbTable;
					General.Var.DbName = dbText.Text.Trim();

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
					MessageBox.Show("テーブル作成中にエラーが発生しました。\n\n" + ex.Message, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				dr = MessageBox.Show("現在ロードされている作業ディレクトリ\n" + General.Var.GameDir + "\nにある全てのINIファイルの\n" + sb.ToString() + "について、\n【" + beforeText + "】→【" + afterText + "】\nへ一括置換します。\n\n実行後は取り消せません。実行しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dr == DialogResult.No)
				{
					return;
				}
				if (General.Var.EditAllIniFile(textBox8.Text.Trim(), textBox9.Text.Trim(), checkBox3.Checked, checkBox5.Checked, out sucCount, out errMsg))
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
			General.Var.SaveType = radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I";

			bool deleteAllRecodes = checkBox6.Checked;
			bool forceCommit = checkBox7.Checked;

			string gameIni = General.Var.GameDir + "game.ini";
			if (File.Exists(gameIni) == false)
			{
				MessageBox.Show("ゲームファイルが存在しません。\n[ディレクトリ関連]タブで、INIファイルの作業ディレクトリを更新してください。\n\n" + gameIni, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			string iniCount = General.Var.IniRead(gameIni, "list", "game", "取得できませんでした。");

			if (MessageBox.Show("作業ディレクトリの全データを" + (General.Var.SaveType == "D" ? "MSSQL" : "MySQL") + "に取り込みます。\n\n作業ディレクトリ\t\t：[" + General.Var.GameDir + "]（全" + iniCount + "件）\nデータベース接続先\t：[" + General.Var.DbUrl + ":" + General.Var.DbPort + "]\nデータベース名\t\t：[" + General.Var.DbName + "]\nデータベーステーブル名\t：[" + General.Var.DbTable + "]\n\n続行しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			// ini全件数取得
			string backupPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";
			int tmpMaxGameCount = 0;

			int returnVal = General.Var.InsertIni2Db(General.Var.GameDir, backupPath, out tmpMaxGameCount, out sCount, out fCount);

			if (returnVal == 0)
			{
				MessageBox.Show("取込処理が完了しました。\n全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			checkBox8.Enabled = !controlVal;
			dbBackupButton.Enabled = !controlVal;

			groupBox7.Enabled = controlVal;
			groupBox12.Enabled = !controlVal;
			dbOverflowFixButton.Enabled = !controlVal;
		}

		private void iniAutoNumberingFixButton_Click(object sender, EventArgs e)
		{
			string readini = string.Empty;
			string gameDirName = General.Var.GameDir;
			int fileCount = 0;
			int hasErrorIni = 0;
			int tempFileCount = 0;
			bool overflowIni = false;

			// 全ゲーム数取得
			if (File.Exists(General.Var.GameIni))
			{
				fileCount = Convert.ToInt32(General.Var.IniRead(General.Var.GameIni, "list", "game", "0"));
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
				readini = gameDirName + tempFileCount.ToString() + ".ini";
				while (File.Exists(readini) && tempFileCount < 1000)
				{
					overflowIni = true;
					tempFileCount++;
					readini = gameDirName + tempFileCount.ToString() + ".ini";
				}

				// ファイルエラー修正確認
				if (hasErrorIni > 0 || overflowIni)
				{
					DialogResult dr = MessageBox.Show("欠番しているINIファイルが " + hasErrorIni + "件" + (overflowIni ? "、\n範囲外に存在するINIファイルが" : "") + "あります。\n連番となるように修正しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dr == DialogResult.Yes)
					{
						// 自動整番処理
						int result = iniReNumbering(overflowIni);
						string newFileCount = General.Var.IniRead(General.Var.GameIni, "list", "game", "0");
						MessageBox.Show("整番が完了しました！\n\n欠番処理件数：" + result + "件\n登録済みゲーム総数：" + fileCount + "→" + newFileCount + "件", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
				else
				{
					MessageBox.Show("欠番データは見つかりませんでした。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				MessageBox.Show("登録済みデータはありません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void dbOverflowFixButton_Click(object sender, EventArgs e)
		{
			if (urlText.Text.Trim().Length < 1)
			{
				MessageBox.Show("URLは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (radioButton5.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			General.Var.DbUrl = urlText.Text.Trim();
			General.Var.DbPort = portText.Text.Trim();
			General.Var.DbName = dbText.Text.Trim();
			General.Var.DbTable = tableText.Text.Trim();
			General.Var.DbUser = userText.Text.Trim();
			General.Var.DbPass = pwText.Text.Trim();
			General.Var.SaveType = radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I";

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
				MessageBox.Show("最新バージョン:" + text.Replace("\n", "") + "\n現在のバージョン:" + General.Var.AppVer, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
		private async void logoPictureBox_Click(object sender, EventArgs e)
		{
			Random r1 = new System.Random();
			int rand = r1.Next(0, 5);
			switch (rand)
			{
				case 0:
					System.Media.SystemSounds.Beep.Play();
					break;
				case 1:
					System.Media.SystemSounds.Asterisk.Play();
					break;
				case 2:
					System.Media.SystemSounds.Exclamation.Play();
					break;
				case 3:
					System.Media.SystemSounds.Hand.Play();
					break;
				case 4:
					System.Media.SystemSounds.Question.Play();
					break;
			}
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
				MessageBox.Show("URLは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				urlText.Focus();
				return;
			}
			else if (portText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ポート番号は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				portText.Focus();
				return;
			}
			else if (userText.Text.Trim().Length < 1)
			{
				MessageBox.Show("ユーザ名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				userText.Focus();
				return;
			}
			else if (pwText.Text.Trim().Length < 1)
			{
				MessageBox.Show("パスワードは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				pwText.Focus();
				return;
			}

			// MySQLだけDatabaseも補填していないとエラーとする
			if (radioButton5.Checked)
			{
				if (dbText.Text.Trim().Length < 1)
				{
					MessageBox.Show("データベース名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					dbText.Focus();
					return;
				}
			}

			General.Var.DbUrl = urlText.Text.Trim();
			General.Var.DbPort = portText.Text.Trim();
			General.Var.DbName = dbText.Text.Trim();
			General.Var.DbTable = tableText.Text.Trim();
			General.Var.DbUser = userText.Text.Trim();
			General.Var.DbPass = pwText.Text.Trim();
			General.Var.SaveType = radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I";

			string backupPath = (General.Var.BaseDir.EndsWith("\\") ? General.Var.BaseDir : General.Var.BaseDir + "\\") + "database_backup(" + (DateTime.Now).ToString().Replace("/", "_").Replace(":", "_") + ")\\";
			bool returnVal = General.Var.downloadDbDataToLocal(backupPath);

			if (returnVal)
			{
				MessageBox.Show("バックアップを取得しました。\n\n" + backupPath, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("エラーが発生しました。\n詳細はエラーログをご覧下さい。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBox8_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox8.Checked && checkBox8.Focused)
			{
				// オフライン保存有効時にダイアログを表示
				MessageBox.Show("[オフラインに保存]を有効にすると、以下のタイミングで自動的にDBのバックアップが取得されます。\n\n・設定画面の[適用して閉じる]を押した時\n・アプリケーションを終了する時\n\nバックアップの保存先：" + General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\" + "\n\nまた、バックアップの取得に時間がかかる場合があります。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// INIファイルの再自動採番
		/// </summary>
		/// <param name="hasOverflowIni"></param>
		/// <returns>処理件数</returns>
		private int iniReNumbering(bool hasOverflowIni)
		{
			bool needMoveIni = false;
			int currentSavedCount = 1;
			int baseFileCount = 0;
			int movedCount = 0;
			int curCount = 0;
			string readini = string.Empty;
			string newReadIni = string.Empty;
			string gameDirName = General.Var.GameDir;

			// ゲーム統括管理iniの最大値を取得する
			if (File.Exists(General.Var.GameIni))
			{
				baseFileCount = Convert.ToInt32(General.Var.IniRead(General.Var.GameIni, "list", "game", "0"));
			}

			try
			{
				// ループで回しながら処理する
				for (curCount = 1; curCount <= baseFileCount; curCount++)
				{
					readini = gameDirName + curCount + ".ini";

					if (File.Exists(readini))
					{
						if (needMoveIni)
						{
							// 移動フラグが立っている場合、ファイルを移動する
							needMoveIni = false;
							newReadIni = gameDirName + currentSavedCount + ".ini";
							File.Move(readini, newReadIni);
							movedCount++;
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
				if (hasOverflowIni)
				{
					hasOverflowIni = false;
					readini = gameDirName + curCount + ".ini";
					while (File.Exists(readini) && curCount < 1000)
					{
						readini = gameDirName + curCount + ".ini";

						if (File.Exists(readini))
						{
							if (needMoveIni)
							{
								// 移動フラグが立っている場合、ファイルを移動する
								needMoveIni = false;
								newReadIni = gameDirName + currentSavedCount + ".ini";
								File.Move(readini, newReadIni);
								movedCount++;
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
					General.Var.IniWrite(General.Var.GameIni, "list", "game", currentSavedCount.ToString());
					if (General.Var.SaveType == "T")
					{
						General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
					}
				}
			}
			catch (Exception ex)
			{
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, readini);
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
			if (queryText.Text.Trim().Length != 0 && MessageBox.Show("入力したSQLクエリを実行しますか？\n\nクエリはトランザクションで実行され、\nエラーが発生した場合はロールバックされます。\n\n※元に戻すことはできません！\n\nHost：" + urlText.Text.Trim() + " / Port：" + portText.Text.Trim() + "\nDatabase：" + dbText.Text.Trim() + " / Table：" + tableText.Text.Trim() + "\nUser：" + userText.Text.Trim() + " / Pass：*** Your Password ***", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
			{
				// Execute

				if (urlText.Text.Trim().Length < 1)
				{
					MessageBox.Show("URLは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					urlText.Focus();
					return;
				}
				else if (portText.Text.Trim().Length < 1)
				{
					MessageBox.Show("ポート番号は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					portText.Focus();
					return;
				}
				else if (userText.Text.Trim().Length < 1)
				{
					MessageBox.Show("ユーザ名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					userText.Focus();
					return;
				}
				else if (pwText.Text.Trim().Length < 1)
				{
					MessageBox.Show("パスワードは必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					pwText.Focus();
					return;
				}
				else if (tableText.Text.Trim().Length < 1)
				{
					MessageBox.Show("テーブル名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					tableText.Focus();
					return;
				}

				// MySQLだけDatabaseも補填していないとエラーとする
				if (radioButton5.Checked)
				{
					if (dbText.Text.Trim().Length < 1)
					{
						MessageBox.Show("データベース名は必須です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						dbText.Focus();
						return;
					}
				}

				General.Var.DbUrl = urlText.Text.Trim();
				General.Var.DbPort = portText.Text.Trim();
				General.Var.DbName = dbText.Text.Trim();
				General.Var.DbTable = tableText.Text.Trim();
				General.Var.DbUser = userText.Text.Trim();
				General.Var.DbPass = pwText.Text.Trim();
				General.Var.SaveType = radioButton9.Checked ? "D" : radioButton5.Checked ? "M" : "I";

				// 修正
				SqlConnection cn = General.Var.SqlCon;
				SqlCommand cm;
				SqlCommand cm2;
				SqlTransaction tr = null;

				MySqlConnection mcn = General.Var.SqlCon2;
				MySqlCommand mcm;
				MySqlCommand mcm2;
				MySqlTransaction mtr = null;

				string queryResult = string.Empty;

				if (General.Var.SaveType == "D")
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
							MessageBox.Show(@queryResult, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						if (tr != null)
						{
							tr.Rollback();
						}
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						MessageBox.Show("実行に失敗しました。\n\n" + ex.Message, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
							MessageBox.Show(@queryResult, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
					}
					catch (Exception ex)
					{
						if (mtr != null)
						{
							mtr.Rollback();
						}
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, mcm.CommandText);
						MessageBox.Show("実行に失敗しました。\n\n" + ex.Message, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
	}
}