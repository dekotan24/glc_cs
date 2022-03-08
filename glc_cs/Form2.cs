using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Collections;

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
			gv.GLConfigLoad();

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

			textBox3.Text = gv.GameDb;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			// 全般
			gv.WriteIni("imgd", "bgimg", textBox6.Text.Trim());
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
			System.Diagnostics.Process.Start("https://angel.nippombashi.net");
		}

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.linkLabel2.LinkVisited = true;
			System.Diagnostics.Process.Start("mailto:support_dekosoft@outlook.jp");
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
			}
			else
			{
				groupBox4.Enabled = false;
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
			String newpath;

			//dcon.jar選択
			openFileDialog1.Title = "データベース(applist.accdb)を選択";
			openFileDialog1.Filter = "GL AppList for MSDB|applist.accdb";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				newpath = openFileDialog1.FileName;
				textBox3.Text = newpath;
				gv.GameDb = newpath;
			}
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
	}
}