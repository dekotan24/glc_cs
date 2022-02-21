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
		string appname = "GLconfig";

		// 変数ファイル宣言
		General.Var gv = new General.Var();
		General.Fun gf = new General.Fun();

		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			gf.GLConfigLoad();

			//バージョン取得
			label10.Text = "Ver." + gv.AppVer + " Build " + gv.AppBuild;

			//Discord設定読み込み
			checkBox1.Checked = Convert.ToBoolean(Convert.ToInt32(gf.ReadIni("checkbox", "dconnect", "0")));
			if (Convert.ToInt32(gf.ReadIni("checkbox", "rate", "-1")) == 0)
			{
				radioButton1.Checked = true;
			}
			else if (Convert.ToInt32(gf.ReadIni("checkbox", "rate", "-1")) == 1)
			{
				radioButton2.Checked = true;
			}
			else
			{
				radioButton1.Checked = true;
			}
			string dconpath = gf.ReadIni("connect", "dconpath", "-1");
			if (File.Exists(dconpath))
			{
				//指定パスにdcon.jar存在する場合
				textBox1.Text = dconpath;
				label11.Text = "OK";
			}
			else if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "dcon.jar"))
			{
				//アプリケーションルートに存在する場合
				textBox1.Text = AppDomain.CurrentDomain.BaseDirectory + "dcon.jar";
				label11.Text = "OK";
			}
			else
			{
				label11.Text = "NG";
			}

			//棒読みちゃん設定読み込み
			int isbyActive = Convert.ToInt32(gf.ReadIni("connect", "byActive", "0"));
			checkBox2.Checked = Convert.ToBoolean(isbyActive);
			if (isbyActive == 1)
			{
				groupBox4.Enabled = true;
			}

			gv.ByType = Convert.ToInt32(gf.ReadIni("connect", "byType", "0"));
			if (gv.ByType == 1)
			{
				radioButton4.Checked = true;
			}
			else
			{
				radioButton3.Checked = true;
			}
			textBox4.Text = gf.ReadIni("connect", "byHost", "127.0.0.1");
			textBox5.Text = gf.ReadIni("connect", "byPort", "50001");

		}

		private void button2_Click(object sender, EventArgs e)
		{
			//discord設定適用
			gf.WriteIni("checkbox", "dconnect", (Convert.ToInt32(checkBox1.Checked)).ToString(), 1, "");
			if (radioButton1.Checked)
			{
				gf.WriteIni("checkbox", "rate", "0", 1, "");
			}
			else if (radioButton2.Checked)
			{
				gf.WriteIni("checkbox", "rate", "1", 1, "");
			}
			gf.WriteIni("connect", "dconpath", textBox1.Text, 1, "");

			//棒読みちゃん設定適用
			gf.WriteIni("connect", "byActive", (Convert.ToInt32(checkBox2.Checked)).ToString(), 1, "");
			gf.WriteIni("connect", "byType", gv.ByType.ToString(), 1, "");
			gf.WriteIni("connect", "byHost", textBox4.Text, 1, "");
			gf.WriteIni("connect", "byPort", textBox5.Text, 1, "");
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
				gf.WriteIni("default", "directory", newworkdir, 1, "");
				gf.WriteIni("list", "game", "0", 0, newworkdir);
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

			gf.Bouyomi_Connectchk(gv.ByHost, gv.ByPort, gv.ByType);
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
	}
}
