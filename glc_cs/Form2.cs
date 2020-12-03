using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;

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

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Convert.ToBoolean(Convert.ToInt32(readini("checkbox", "dconnect", "1")));
            if(Convert.ToInt32(readini("checkbox", "rate", "-1")) == 0){
                radioButton1.Checked = true;
            }else if(Convert.ToInt32(readini("checkbox", "rate", "-1")) == 1){
                radioButton2.Checked = true;
            }

            //checkBox3.Checked = Convert.ToBoolean(Convert.ToInt32(readini("checkbox", "attbat", "1")));
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //適用して閉じる
            writeini("checkbox", "dconnect", (Convert.ToInt32(checkBox1.Checked)).ToString());
            if (radioButton1.Checked)
            {
                writeini("checkbox", "rate", "0");
            }
            else if (radioButton2.Checked)
            {
                writeini("checkbox", "rate", "1");
            }
            //writeini("checkbox", "attbat", (Convert.ToInt32(checkBox3.Checked)).ToString());
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //作業ディレクトリ変更
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                String newworkdir = folderBrowserDialog1.SelectedPath;
                writeini("default", "directory", newworkdir);
            }
            return;
        }

        private void writeini(String sec, String key, String data)
        {
            WritePrivateProfileString(
                            sec,
                            key,
                            data,
                            configini);
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
            textBox5.Text = "50001";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //作業ディレクトリ変更
            if (folderBrowserDialog2.ShowDialog() == DialogResult.OK)
            {
                String newbydir = folderBrowserDialog2.SelectedPath;
                writeini("connect", "byAppdir", newbydir);
            }
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bouyomi_connectchk();
        }
        private void bouyomi_connectchk()
        {

            //棒読みちゃん関係
            String bysMsg = "連携しました。";
            byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
            Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;
            byte[] byMsg;
            TcpClient tc = null;
            String byHost = readini("connect", "byHost", "127.0.0.1");
            int byPort = Convert.ToInt32(readini("connect", "byPort", "50001"));

            byte[] bybMsg = Encoding.UTF8.GetBytes(bysMsg);
            Int32 byLength = bybMsg.Length;

            //接続テスト
            try
            {
                tc = new TcpClient(byHost, byPort);
                MessageBox.Show("棒読みちゃんと連携しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("エラー：棒読みちゃんの連携に失敗しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            //切断
            tc.Close();
            return;
        }
    }
}
