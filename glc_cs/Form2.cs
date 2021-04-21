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
        [DllImport("KERNEL32.DLL")]
        public static extern uint
          GetPrivateProfileString(string lpAppName,
          string lpKeyName, string lpDefault,
          StringBuilder lpReturnedString, uint nSize,
          string lpFileName);

        [DllImport("KERNEL32.DLL")]
        public static extern uint
        WritePrivateProfileString(string lpAppName,
        string lpKeyName, string lpString,
        string lpFileName);

        String configini = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
        string appname = "GLconfig";
        int byType;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //Discord設定読み込み
            checkBox1.Checked = Convert.ToBoolean(Convert.ToInt32(readini("checkbox", "dconnect", "0")));
            if (Convert.ToInt32(readini("checkbox", "rate", "-1")) == 0)
            {
                radioButton1.Checked = true;
            }
            else if (Convert.ToInt32(readini("checkbox", "rate", "-1")) == 1)
            {
                radioButton2.Checked = true;
            }
            else
            {
                radioButton1.Checked = true;
            }

            //棒読みちゃん設定読み込み
            int isbyActive = Convert.ToInt32(readini("connect", "byActive", "0"));
            checkBox2.Checked = Convert.ToBoolean(isbyActive);
            if (isbyActive == 1)
            {
                groupBox4.Enabled = true;
            }

            byType = Convert.ToInt32(readini("connect", "byType", "0"));
            if (byType == 1)
            {
                radioButton4.Checked = true;
            }
            else
            {
                radioButton3.Checked = true;
            }
            textBox4.Text = readini("connect", "byHost", "127.0.0.1");
            textBox5.Text = readini("connect", "byPort", "50001");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //discord設定適用
            writeini("checkbox", "dconnect", (Convert.ToInt32(checkBox1.Checked)).ToString(), 1, "");
            if (radioButton1.Checked)
            {
                writeini("checkbox", "rate", "0", 1, "");
            }
            else if (radioButton2.Checked)
            {
                writeini("checkbox", "rate", "1", 1, "");
            }

            //棒読みちゃん設定適用
            writeini("connect", "byActive", (Convert.ToInt32(checkBox2.Checked)).ToString(), 1, "");
            writeini("connect", "byType", byType.ToString(), 1, "");
            writeini("connect", "byHost", textBox4.Text, 1, "");
            writeini("connect", "byPort", textBox5.Text, 1, "");
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //作業ディレクトリ変更
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                String newworkdir = folderBrowserDialog1.SelectedPath;
                if (!(newworkdir.EndsWith("\\")))
                {
                    newworkdir += "\\";
                }
                writeini("default", "directory", newworkdir, 1, "");
                writeini("list", "game", "0", 0, newworkdir);
            }
            return;
        }

        private void writeini(String sec, String key, String data, int isconfig, String opt)
        {
            if (isconfig == 1)
            {
                WritePrivateProfileString(
                                sec,
                                key,
                                data,
                                configini);
            }
            else
            {
                if (!File.Exists(opt + "\\Data\\game.ini"))
                {
                    WritePrivateProfileString(
                                    sec,
                                    key,
                                    data,
                                    opt);
                }
            }
            return;
        }

        private string readini(String sec, String key, String failedval)
        {
            String ans = "";

            StringBuilder data = new StringBuilder(1024);
            GetPrivateProfileString(
                sec,
                key,
                failedval,
                data,
                1024,
                configini);

            ans = data.ToString();
            return ans;
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
            if (byType == 0)
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
            String byHost = textBox4.Text;
            int byPort = Convert.ToInt32(textBox5.Text);

            bouyomi_connectchk(byHost, byPort, byType);
        }

        private void bouyomi_connectchk(string byHost, int byPort, int byType)
        {
            String bysMsg = "ゲームランチャーとの接続テストに成功しました。";

            if (byType == 1)
            {
                string url = "http://localhost:" + textBox5.Text + "/talk";
                System.Net.WebClient wc = new System.Net.WebClient();
                //NameValueCollectionの作成
                System.Collections.Specialized.NameValueCollection ps =
                    new System.Collections.Specialized.NameValueCollection();
                //送信するデータ（フィールド名と値の組み合わせ）を追加
                ps.Add("text", bysMsg);
                //データを送信し、また受信する
                try
                {
                    wc.UploadValues(url, ps);
                }
                catch(Exception)
                {
                    MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                wc.Dispose();
                MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
                Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;
                TcpClient tc = null;

                byte[] bybMsg = Encoding.UTF8.GetBytes(bysMsg);
                Int32 byLength = bybMsg.Length;

                //接続テスト
                try
                {
                    tc = new TcpClient(byHost, byPort);
                }
                catch (Exception)
                {
                    MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //メッセージ送信
                using (NetworkStream ns = tc.GetStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ns))
                    {
                        bw.Write(byCmd); //コマンド（ 0:メッセージ読み上げ）
                        bw.Write(bySpd);   //速度    （-1:棒読みちゃん画面上の設定）
                        bw.Write(byTone);    //音程    （-1:棒読みちゃん画面上の設定）
                        bw.Write(byVol);  //音量    （-1:棒読みちゃん画面上の設定）
                        bw.Write(byVoice);   //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
                        bw.Write(byCode);    //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
                        bw.Write(byLength);  //文字列のbyte配列の長さ
                        bw.Write(bybMsg); //文字列のbyte配列
                    }
                }
                tc.Close();
                MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
            byType = 0;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox4.Enabled = false;
            textBox5.Text = "50080";
            byType = 1;
        }
    }
}
