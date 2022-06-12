using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class Form2 : Form
	{
		// 変数ファイル宣言
		General.Var gv = new General.Var();

		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			if (gv.GLConfigLoad() == false)
			{
				label15.Text = "Configロード中にエラー。詳しくはエラーログを参照して下さい。";
			}

			//バージョン取得
			label10.Text = "Ver." + gv.AppVer + " Build " + gv.AppBuild;

			// 背景画像
			textBox6.Text = gv.BgImg;

			//Discord設定読み込み
			bool dconActive = gv.Dconnect;

			// Discord連携有効フラグ
			checkBox1.Checked = dconActive;
			// 機能アクティブ
			if (dconActive)
			{
				groupBox2.Enabled = true;
				groupBox6.Enabled = true;
			}
			else
			{
				groupBox2.Enabled = false;
				groupBox6.Enabled = false;
			}

			// レート設定
			if (gv.Rate == 1)
			{
				radioButton2.Checked = true;
			}
			else
			{
				radioButton1.Checked = true;
			}

			// Discord Connectorパス取得
			string dconpath = gv.DconPath;

			if (File.Exists(dconpath))
			{
				//指定パスにdcon.jar存在する場合
				textBox1.Text = dconpath;
				label11.Text = "OK";
			}
			else
			{
				textBox1.Text = string.Empty;
				label11.Text = "NG";
			}

			//棒読みちゃん設定読み込み
			bool isbyActive = gv.ByActive;
			checkBox2.Checked = isbyActive;
			if (isbyActive)
			{
				groupBox4.Enabled = true;
				groupBox5.Enabled = true;
			}

			if (gv.ByType == 1)
			{
				radioButton4.Checked = true;
			}
			else
			{
				radioButton3.Checked = true;
			}
			textBox4.Text = gv.ByHost;
			textBox5.Text = gv.ByPort.ToString();

			switch (gv.ByCErr)
			{
				case "A":
					radioButton5.Checked = true;
					break;
				case "D":
					radioButton6.Checked = true;
					break;
				case "Q":
				default:
					radioButton7.Checked = true;
					break;
			}

			checkBox4.Checked = gv.ByRoS;
			checkBox10.Checked = gv.ByRoW;

			// 保存方法
			if (gv.SaveType == "I")
			{
				radioButton8.Checked = true;
				setDirectoryControl(true);
			}
			else
			{
				radioButton9.Checked = true;
				setDirectoryControl(false);
			}

			textBox3.Text = gv.DbUrl;
			textBox11.Text = gv.DbName;
			textBox12.Text = gv.DbTable;
			textBox7.Text = gv.DbUser;
			textBox10.Text = gv.DbPass;

			checkBox8.Checked = gv.OfflineSave;

			// 作業ディレクトリ反映
			if (gv.GameDir.EndsWith("\\Data\\"))
			{
				textBox2.Text = gv.GameDir.Substring(0, gv.GameDir.Length - 5);
			}
			else if (gv.GameDir.EndsWith("\\Data"))
			{
				textBox2.Text = gv.GameDir.Substring(0, gv.GameDir.Length - 4);
			}
			else
			{
				textBox2.Text = gv.GameDir;
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			bool canExit = true;
			if (radioButton9.Checked)
			{
				if (textBox3.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (textBox11.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (textBox12.Text.Trim().Length <= 0)
				{
					canExit = false;
				}
				else if (textBox7.Text.Trim().Length <= 0)
				{
					canExit = false;
				}

				// 保存可否判定
				if (!canExit)
				{
					MessageBox.Show("データの保存方法をデータベースにした場合、\n[URL]、[DB]、[Table]、[User]は必須です。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			MyBase64str base64 = new MyBase64str();

			// 全般
			gv.WriteIni("imgd", "bgimg", textBox6.Text.Trim());

			// 保存方法
			gv.WriteIni("general", "save", radioButton9.Checked ? "D" : "I");
			gv.WriteIni("general", "OfflineSave", checkBox8.Checked ? "1" : "0");
			gv.WriteIni("connect", "DBURL", textBox3.Text.Trim());
			gv.WriteIni("connect", "DBName", textBox11.Text.Trim());
			gv.WriteIni("connect", "DBTable", textBox12.Text.Trim());
			gv.WriteIni("connect", "DBUser", textBox7.Text.Trim());
			gv.WriteIni("connect", "DBPass", base64.Encode(textBox10.Text.Trim()));

			//discord設定適用
			gv.WriteIni("checkbox", "dconnect", (Convert.ToInt32(checkBox1.Checked)).ToString());
			if (radioButton1.Checked)
			{
				gv.WriteIni("checkbox", "rate", "0");
			}
			else if (radioButton2.Checked)
			{
				gv.WriteIni("checkbox", "rate", "1");
			}
			gv.WriteIni("connect", "dconpath", textBox1.Text);

			//棒読みちゃん設定適用
			gv.WriteIni("connect", "byActive", (Convert.ToInt32(checkBox2.Checked)).ToString());
			gv.WriteIni("connect", "byType", gv.ByType.ToString());
			gv.WriteIni("connect", "byPort", textBox5.Text);
			gv.WriteIni("connect", "byHost", textBox4.Text);

			// データベースをローカルにINIで保存する
			if (checkBox8.Checked)
			{
				if (radioButton9.Checked)
				{
					string localPath = gv.BaseDir + (gv.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
					if (!gv.downloadDbDataToLocal(localPath))
					{
						// ローカル保存に失敗
						gv.WriteIni("general", "OfflineSave", "0");
						MessageBox.Show("オフライン保存に失敗しました。\nオフライン機能は無効になります。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						// オフラインINIのフラグ変更
						gv.IniWrite((localPath + "game.ini"), "list", "dbupdate", "0");
					}
				}
			}
			Hide();
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
				gv.WriteIni("default", "directory", newworkdir);

				// 作業ディレクトリに管理iniがない場合は0で初期化
				if (!File.Exists(newworkdir + "\\Data\\game.ini"))
				{
					gv.WriteIni("list", "game", "0", 0, newworkdir);
				}

				// textbox反映
				gv.GameDir = gv.ReadIni("default", "directory", gv.BaseDir);
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
			Clipboard.SetText("support_dekosoft@outlook.jp");
		}
		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel3.LinkVisited = true;
			System.Diagnostics.Process.Start("https://github.com/dekotan24/glc_cs");
		}

		private void button8_Click(object sender, EventArgs e)
		{
			textBox4.Text = "127.0.0.1";
			if (gv.ByType == 0)
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
			gv.ByHost = textBox4.Text;
			gv.ByPort = Convert.ToInt32(textBox5.Text);

			gv.Bouyomi_Connectchk(gv.ByHost, gv.ByPort, gv.ByType);
		}


		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox2.Checked)
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
			gv.ByType = 0;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			textBox4.Enabled = false;
			textBox5.Text = "50080";
			gv.ByType = 1;
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
				textBox1.Text = newpath;
				label11.Text = "OK";
			}
			return;
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (textBox3.Text.Trim().Length < 1)
			{
				return;
			}
			else if (textBox7.Text.Trim().Length < 1)
			{
				return;
			}
			else if (textBox10.Text.Trim().Length < 1)
			{
				return;
			}

			gv.DbUrl = textBox3.Text.Trim();
			gv.DbName = textBox11.Text.Trim();
			gv.DbTable = textBox12.Text.Trim();
			gv.DbUser = textBox7.Text.Trim();
			gv.DbPass = textBox10.Text.Trim();

			SqlConnection cn = gv.SqlCon;
			SqlCommand cm;
			cm = new SqlCommand()
			{
				CommandType = CommandType.Text,
				CommandTimeout = 30,
				CommandText = @"CREATE DATABASE [dekosoft_gl]"
			};
			cm.Connection = cn;

			SqlCommand cm2 = new SqlCommand()
			{
				CommandType = CommandType.Text,
				CommandTimeout = 30,
				CommandText = @"CREATE TABLE [dekosoft_gl].[dbo].[gl_item1] ( [ID] INT IDENTITY(1,1) NOT NULL, [GAME_NAME] NVARCHAR(255), [GAME_PATH] NVARCHAR(MAX), [IMG_PATH] NVARCHAR(MAX), [UPTIME] NVARCHAR(255), [RUN_COUNT] NVARCHAR(99), [DCON_TEXT] NVARCHAR(50), [AGE_FLG] NVARCHAR(1), [TEMP1] NVARCHAR(255) NULL, [LAST_RUN] DATETIME NULL )"
			};
			cm2.Connection = cn;

			// データベース作成
			try
			{
				cn.Open();
				cm.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				gv.WriteErrorLog(ex.Message, "button5_Click", cm.CommandText);
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
			try
			{
				cn.Open();
				cm2.ExecuteNonQuery();
				textBox11.Text = "dekosoft_gl";
				textBox12.Text = "dbo.gl_item1";
				gv.DbName = textBox11.Text.Trim();
				gv.DbTable = textBox12.Text.Trim();
				label15.Text = "テーブル作成が完了しました。";
			}
			catch (Exception ex)
			{
				gv.WriteErrorLog(ex.Message, "button5_Click", cm2.CommandText);
				label15.Text = "テーブル作成中にエラーが発生しました。エラーログをご覧下さい";
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
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
			if (checkBox1.Checked)
			{
				groupBox2.Enabled = true;
				groupBox6.Enabled = true;
			}
			else
			{
				groupBox2.Enabled = false;
				groupBox6.Enabled = false;
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
				dr = MessageBox.Show("現在ロードされている作業ディレクトリ\n" + gv.GameDir + "\nにある全てのINIファイルの\n" + sb.ToString() + "について、\n【" + beforeText + "】→【" + afterText + "】\nへ一括置換します。\n\n実行後は取り消せません。実行しますか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				if (dr == DialogResult.No)
				{
					return;
				}
				if (gv.EditAllFilePath(textBox8.Text.Trim(), textBox9.Text.Trim(), checkBox3.Checked, checkBox5.Checked, out sucCount, out errMsg))
				{
					MessageBox.Show("成功しました。\n\n処理件数：" + sucCount, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				else
				{
					MessageBox.Show("処理中にエラーが発生しました。\n処理を中断します。\n\n" + errMsg, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
				textBox6.Text = newpath;
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
			// databaseMode
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

			if (!radioButton9.Checked)
			{
				return;
			}
			else if (textBox3.Text.Trim().Length == 0 || textBox11.Text.Trim().Length == 0 || textBox12.Text.Trim().Length == 0 || textBox7.Text.Trim().Length == 0 || textBox10.Text.Trim().Length == 0)
			{
				return;
			}

			gv.DbUrl = textBox3.Text.Trim();
			gv.DbName = textBox11.Text.Trim();
			gv.DbTable = textBox12.Text.Trim();
			gv.DbUser = textBox7.Text.Trim();
			gv.DbPass = textBox10.Text.Trim();

			bool deleteAllRecodes = checkBox6.Checked;
			bool forceCommit = checkBox7.Checked;

			if (MessageBox.Show("現在ロードしている作業ディレクトリ\n[" + gv.GameDir + "] の全てのゲームデータをDB\n[" + gv.DbUrl + " " + gv.DbName + "." + gv.DbTable + "] \nへ取り込みます。\n\n続行しますか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}

			// ini全件数取得
			string backupPath = gv.BaseDir + (gv.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";
			int tmpMaxGameCount = 0;

			int returnVal = gv.InsertIni2Db(gv.GameDir, backupPath, out tmpMaxGameCount, out sCount, out fCount);

			if (returnVal == 0)
			{
				MessageBox.Show("変換処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				MessageBox.Show("エラーが発生しました。\n詳細はエラーログをご覧下さい。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox6.Checked)
			{
				MessageBox.Show("※※※　警告　※※※\n\nこのチェックを入れた状態で取り込みを開始すると、\n既にデータベースに取り込まれているゲームデータが\n消失します。\n\nクリーンな状態でINIファイルのデータを取り込みたい\n場合に便利な機能です。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
			textBox3.Enabled = !controlVal;
			label23.Enabled = !controlVal;
			textBox11.Enabled = !controlVal;
			label24.Enabled = !controlVal;
			textBox12.Enabled = !controlVal;
			label18.Enabled = !controlVal;
			textBox7.Enabled = !controlVal;
			label22.Enabled = !controlVal;
			textBox10.Enabled = !controlVal;
			button5.Enabled = !controlVal;
			checkBox8.Enabled = !controlVal;

			groupBox7.Enabled = controlVal;
			groupBox12.Enabled = !controlVal;
		}
	}
}