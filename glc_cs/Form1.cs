using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace glc_cs
{
    public partial class gl : Form
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

        public String defaultdir;
        public String appname = "Game Launcher C# Edition";
        public String appver = "0.2";
        public String appbuild = "2.20.08.07"; 

        public gl()
        {
            InitializeComponent();
        }

        private void gl_Load(object sender, EventArgs e)
        {
            //String defaultdir;
            //iniファイル読み込み
            string iniFileName = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

            /*
            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(
                "checkbox",      // セクション名
                "track",          // キー名    
                "0",   // 値が取得できなかった場合に返される初期値
                sb,             // 格納先
                Convert.ToUInt32(sb.Capacity), // 格納先のキャパ
                iniFileName);   // iniファイル名
                */

            //ファイル存在チェック及び読み込み
            if (File.Exists(iniFileName))
            {

                //set checkbox
                StringBuilder ckv0 = new StringBuilder(1024);
                GetPrivateProfileString(
                    "checkbox",
                    "track",
                    "0",
                    ckv0,
                    Convert.ToUInt32(ckv0.Capacity),
                    iniFileName);

                StringBuilder ckv1 = new StringBuilder(1024);
                GetPrivateProfileString(
                    "checkbox",
                    "winmini",
                    "0",
                    ckv1,
                    Convert.ToUInt32(ckv1.Capacity),
                    iniFileName);

                StringBuilder defaultdirr = new StringBuilder(1024);
                GetPrivateProfileString(
                    "default",
                    "directory",
                    AppDomain.CurrentDomain.BaseDirectory,
                    defaultdirr,
                    1024,
                    iniFileName);
                defaultdir = defaultdirr.ToString();
                defaultdir += "Data";


                if (int.Parse(ckv0.ToString()) == 1)
                {
                    checkBox1.Checked = true;
                }

                if (int.Parse(ckv1.ToString()) == 1)
                {
                    checkBox2.Checked = true;
                }

            }
            else
            {
                //ini存在しない場合
                defaultdir = AppDomain.CurrentDomain.BaseDirectory + "Data";

                checkBox1.Checked = true;
                checkBox2.Checked = true;
            }


            String item = loaditem(defaultdir.ToString());

            if (item == "_none_game_data" || item == "0")
            {
                //itemがnoneの場合：ゲームが登録されていない場合
                MessageBox.Show("登録済みゲーム：0\n\n通常版のGame Launcherを起動してください。\nアプリケーションを終了します。\n\nError: loaditem_returned_none_or_0\nDebug:: gl > gl_Load > if > true",
                                appname,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Close();
            }


            fileSystemWatcher1.Path = defaultdir;


        }

        public string loaditem(String defaultdir)
        {
            //String[][] list;
            //int allgameint;
            int allgameint;
            String test = defaultdir.ToString();
            String loadfileini = test + "\\game.ini";
            listBox1.Items.Clear();


            //全ゲーム数取得
            if (File.Exists(loadfileini))
            {
                StringBuilder allgameintr = new StringBuilder(1024);
                GetPrivateProfileString(
                    "list",
                    "game",
                    "0",
                    allgameintr,
                    Convert.ToUInt32(allgameintr.Capacity),
                    loadfileini);
                allgameint = Convert.ToInt32(allgameintr.ToString());
            }
            else
            {
                //ゲームが1つもない場合
                return "_none_game_data";
            }

            if (allgameint <= 0)
            {
                return "_none_game_data";
            }
            /* allgameint = ゲーム数カウンタ
               defaultdir = iniファイルディレクトリ
               loadfileini = 全ゲーム数記録ini
            */

            int count;
            String readini;
            String ans = "";
            String temp;
            StringBuilder data;

            for (count = 1; count <= allgameint; count++)
            {
                //読込iniファイル名更新
                readini = defaultdir + "\\" + count + ".ini";

                if (File.Exists(readini))
                {
                    //ini読込開始
                    data = new StringBuilder(1024);
                    GetPrivateProfileString(
                        "game",
                        "name",
                        AppDomain.CurrentDomain.BaseDirectory,
                        data,
                        1024,
                        readini);
                    temp = data.ToString();

                    listBox1.Items.Add(temp);
                }
                else
                {
                    //個別ini存在しない場合
                    MessageBox.Show("iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。\n\nError: ini_read_error_nofile\nDebug:: gl > loaditem > for > if > else",
                                    appname,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    break;
                }
            }
            //変数値の返却
            listBox1.SelectedIndex = 0;

            //listBox1_SelectedIndexChanged(null,null);

            return ans;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder data;
            int startdata, timedata;

            if (File.Exists(textBox2.Text))
            {
                if (checkBox1.Checked == true)
                {
                    //propertiesファイル書き込み
                    String propertiesfile = AppDomain.CurrentDomain.BaseDirectory + "run.properties";
                    Encoding enc = Encoding.GetEncoding("Shift_JIS");
                    StreamWriter writer = new StreamWriter(propertiesfile, false, enc);
                    writer.WriteLine("title = " + textBox1.Text);
                    writer.Close();


                    //現在時刻取得
                    string strTime = DateTime.Now.ToString();
                    string format = "yyyy/MM/dd HH:mm:ss";
                    DateTime starttime = DateTime.ParseExact(strTime, format, null);


                    //トラッキング対象の指定、実行
                    String drun = AppDomain.CurrentDomain.BaseDirectory + "dcon.jar";
                    if (File.Exists(drun))
                    {
                        System.Diagnostics.Process.Start(drun);
                    }
                    else
                    {
                        MessageBox.Show("drun.batが見つかりません。\n実行を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (checkBox2.Checked == true)
                    {
                        this.WindowState = FormWindowState.Minimized;
                        ShowInTaskbar = false;
                        notifyIcon1.Visible = true;
                    }

                    String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
                    System.Environment.CurrentDirectory = apppath;

                    System.Diagnostics.Process p =
                    System.Diagnostics.Process.Start(textBox2.Text);
                    p.WaitForExit();

                    System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    button16_Click(null, null);

                    if (checkBox2.Checked == true)
                    {
                        this.WindowState = FormWindowState.Normal;
                        ShowInTaskbar = true;
                        notifyIcon1.Visible = false;
                    }

                    //終了時刻取得
                    String time = p.ExitTime.ToString();
                    DateTime endtime = DateTime.ParseExact(time, format, null);

                    //起動時間計算
                    String temp = (endtime - starttime).ToString();
                    int anss = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

                    //ini上書き
                    int selecteditem = listBox1.SelectedIndex + 1;
                    String readini = defaultdir + "\\" + selecteditem + ".ini";

                    if (File.Exists(readini))
                    {
                        //既存値の取得
                        data = new StringBuilder(1024);
                        GetPrivateProfileString(
                            "game",
                            "time",
                            AppDomain.CurrentDomain.BaseDirectory,
                            data,
                            1024,
                            readini);
                        timedata = Convert.ToInt32(data.ToString());

                        data = new StringBuilder(1024);
                        GetPrivateProfileString(
                            "game",
                            "start",
                            AppDomain.CurrentDomain.BaseDirectory,
                            data,
                            1024,
                            readini);
                        startdata = Convert.ToInt32(data.ToString());

                        //計算
                        timedata += anss;
                        startdata += 1;

                        //書き換え
                        WritePrivateProfileString(
                            "game",
                            "time",
                            timedata.ToString(),
                            readini);

                        WritePrivateProfileString(
                            "game",
                            "start",
                            startdata.ToString(),
                            readini);
                    }
                    else
                    {
                        //個別ini存在しない場合
                        MessageBox.Show("iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。\n\nError: ini_read_error_nofile\nDebug:: gl > button1_Click > if > if > else",
                                        appname,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                    listBox1_SelectedIndexChanged(null, null);
                }
                else
                {
                    String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
                    System.Environment.CurrentDirectory = apppath;

                    System.Diagnostics.Process.Start(textBox2.Text);

                    System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /*
            openFileDialog1.Title = "追加する実行ファイルを選択";
            openFileDialog1.Filter = "実行ファイル|*.exe|すべてのファイル|*.*";
            openFileDialog1.ShowDialog().ToString();
            String exeans = openFileDialog1.FileName;
            MessageBox.Show(exeans);
            if(exeans == null || exeans == "")
            {
                return;
            }

            openFileDialog1.Title = "実行ファイルの画像を選択";
            openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.ico)|*.png;*.jpg;*.bmp;*.ico";
            openFileDialog1.ShowDialog().ToString();
            String imgans = openFileDialog1.FileName;
            */

        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(appname + " Ver." + appver + " / Build " + appbuild + " Beta\n\n" + "現在の作業ディレクトリ：" + defaultdir + "\n\nAuthor: Ogura Deko (dekosoft)\nMail: support_dekosoft@outlook.jp\nWeb: https://sunkun.nippombashi.net\n\nCopyright (C) 2020 Ogura Deko and dekosoft All rights reserved.",
                                appname,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selecteditem = listBox1.SelectedIndex + 1;
            String readini = defaultdir + "\\" + selecteditem + ".ini";
            StringBuilder data;
            String namedata = null, imgpassdata = null, passdata = null, stimedata = null, startdata = null, cmtdata = null;

            if (File.Exists(readini))
            {
                //ini読込開始
                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "name",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                namedata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "imgpass",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                imgpassdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "pass",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                passdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "time",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                stimedata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "start",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                startdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "comment",
                    AppDomain.CurrentDomain.BaseDirectory,
                    data,
                    1024,
                    readini);
                cmtdata = data.ToString();
            }
            else
            {
                //個別ini存在しない場合
                MessageBox.Show("iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。\n\nError: ini_read_error_nofile\nDebug:: gl > listBox1_SelectedIndexChanged > if > else",
                                appname,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }

            int timedata = Convert.ToInt32(stimedata) / 60;

            label1.Text = namedata;
            textBox1.Text = namedata;
            textBox2.Text = passdata;
            textBox3.Text = imgpassdata;
            textBox4.Text = timedata.ToString();
            textBox5.Text = startdata;
            textBox6.Text = cmtdata;


            if (File.Exists(passdata))
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }

            if (File.Exists(imgpassdata))
            {
                pictureBox1.ImageLocation = imgpassdata;
            }
            else
            {
                pictureBox1.ImageLocation = "";
            }

            return;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            String selectedtext = listBox1.SelectedItem.ToString();
            loaditem(defaultdir);
            if (listBox1.Items.Contains(selectedtext))
            {
                listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext); ;
            }
            return;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox2.Text);
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox3.Text);
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            int total = Convert.ToInt32(textBox4.Text);
            String hour = (total / 60).ToString();
            String min = (total % 60).ToString();

            DialogResult result = MessageBox.Show(textBox1.Text +
                                    "\nおおよその起動時間：" + hour + "時間 " + min + "分",
                                    appname,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            String dkill = AppDomain.CurrentDomain.BaseDirectory + "dkill.bat";
            if (File.Exists(dkill))
            {
                System.Diagnostics.Process.Start(dkill);
            }
            else
            {
                MessageBox.Show("dkill.batが見つかりません。", appname, MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            return;
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            String selectedtext = listBox1.SelectedItem.ToString();

            loaditem(defaultdir);

            if (listBox1.Items.Contains(selectedtext))
            {
                listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
            }
            return;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                fileSystemWatcher1.EnableRaisingEvents = true;
            }
            else
            {
                fileSystemWatcher1.EnableRaisingEvents = false;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
