using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public String appver = "0.4";
        public String appbuild = "4.20.08.14";

        public gl()
        {
            InitializeComponent();
        }

        private void gl_Load(object sender, EventArgs e)
        {
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            pictureBox11.Visible = true;
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

                StringBuilder bgimgr = new StringBuilder(1024);
                GetPrivateProfileString(
                    "imgd",
                    "bgimg",
                    "",
                    bgimgr,
                    1024,
                    iniFileName);
                String bgimg = bgimgr.ToString();

                if (File.Exists(bgimg))
                {
                    this.BackgroundImage = new Bitmap(bgimg);
                    toolStripStatusLabel2.Text = "！背景画像が適用されました。環境によっては描画に時間がかかる場合があります。！";
                }


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
                String cfilepass = defaultdir + "\\game.ini";
                if (!File.Exists(cfilepass))
                {
                    WritePrivateProfileString(
                                    "list",
                                    "game",
                                    "0",
                                    cfilepass);
                }


                //itemがnoneの場合：ゲームが登録されていない場合
                MessageBox.Show("Game Launcherをご利用頂きありがとうございます。\n\"追加\"ボタンを押して、ゲームを追加しましょう！",
                                appname,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
            }

            if (Directory.Exists(defaultdir))
            {
                fileSystemWatcher1.Path = defaultdir;
            }
            else
            {
                Directory.CreateDirectory(defaultdir);
                fileSystemWatcher1.Path = defaultdir;
            }
            this.Activate();
            //delay();
            pictureBox11.Visible = false;
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
                button8.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                //ゲームが1つもない場合
                button8.Enabled = false;
                button2.Enabled = false;
                return "_none_game_data";
            }

            if (allgameint == 0)
            {
                button8.Enabled = false;
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

            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = allgameint;


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
                        "",
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
            if (allgameint >= 1)
            {
                listBox1.SelectedIndex = 0;
            }

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
                    iniedtchk((listBox1.SelectedIndex + 1).ToString(), "game", "stat", textBox6.Text);

                    //propertiesファイル書き込み
                    String propertiesfile = AppDomain.CurrentDomain.BaseDirectory + "run.properties";
                    Encoding enc = Encoding.GetEncoding("Shift_JIS");
                    StreamWriter writer = new StreamWriter(propertiesfile, false, enc);
                    writer.WriteLine("title = " + textBox1.Text + "\nstat = " + textBox6.Text);
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
                        this.notifyIcon1.Visible = true;
                    }

                    String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
                    System.Environment.CurrentDirectory = apppath;

                    pictureBox11.Visible = true;
                    if (checkBox3.Checked)
                    {
                        fileSystemWatcher1.EnableRaisingEvents = false;
                    }

                    System.Diagnostics.Process p =
                    System.Diagnostics.Process.Start(textBox2.Text);
                    p.WaitForExit();

                    System.Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    String dkill = AppDomain.CurrentDomain.BaseDirectory + "dkill.bat";
                    if (File.Exists(dkill))
                    {
                        System.Diagnostics.Process.Start(dkill);
                    }

                    if (checkBox2.Checked == true)
                    {
                        this.WindowState = FormWindowState.Normal;
                        this.notifyIcon1.Visible = false;
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
                            "0",
                            data,
                            1024,
                            readini);
                        timedata = Convert.ToInt32(data.ToString());

                        data = new StringBuilder(1024);
                        GetPrivateProfileString(
                            "game",
                            "start",
                            "0",
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
                    pictureBox11.Visible = false;
                    if (checkBox3.Checked)
                    {
                        fileSystemWatcher1.EnableRaisingEvents = true;
                    }
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
            String exeans = "", imgans = "";
            openFileDialog1.Title = "追加する実行ファイルを選択";
            openFileDialog1.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                exeans = openFileDialog1.FileName;
            }
            else
            {
                return;
            }

            openFileDialog1.Title = "実行ファイルの画像を選択";
            openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp)|*.png;*.jpg;*.bmp|すべてのファイル (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                imgans = openFileDialog1.FileName;
            }
            else
            {
                imgans = "";
            }

            String filename = System.IO.Path.GetFileNameWithoutExtension(exeans);
            string appname_ans = Interaction.InputBox(
                "アプリ名を設定", appname, filename, -1, -1);

            String cfilepass = defaultdir + "\\game.ini";
            String gfilepass = "";


            if (File.Exists(cfilepass))
            {

                if (checkBox3.Checked)
                {
                    fileSystemWatcher1.EnableRaisingEvents = false;
                }

                StringBuilder data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "list",
                    "game",
                    "0",
                    data,
                    1024,
                    cfilepass);
                int maxval = Convert.ToInt32(data.ToString()) + 1;

                gfilepass = defaultdir + "\\" + maxval + ".ini";

                if (!File.Exists(gfilepass))
                {
                    WritePrivateProfileString(
                                "game",
                                "name",
                                appname_ans.ToString(),
                                gfilepass);
                    WritePrivateProfileString(
                                "game",
                                "imgpass",
                                imgans.ToString(),
                                gfilepass);
                    WritePrivateProfileString(
                                "game",
                                "pass",
                                exeans.ToString(),
                                gfilepass);
                    WritePrivateProfileString(
                                "game",
                                "time",
                                "0",
                                gfilepass);
                    WritePrivateProfileString(
                                "game",
                                "start",
                                "0",
                                gfilepass);
                    WritePrivateProfileString(
                                "game",
                                "stat",
                                "",
                                gfilepass);

                    WritePrivateProfileString(
                                "list",
                                "game",
                                maxval.ToString(),
                                cfilepass);
                }

                if (checkBox3.Checked)
                {
                    fileSystemWatcher1.EnableRaisingEvents = true;
                }
            }
            else
            {
                MessageBox.Show("something error!");
            }

            button8_Click(null, null);

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

            int allgameint;
            String loadfileini = defaultdir + "\\game.ini";

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

                if (allgameint <= 0)
                {
                    return;
                }
            }
            else
            {
                return;
            }


            //ゲーム詳細取得
            int selecteditem = listBox1.SelectedIndex + 1;
            String readini = defaultdir + "\\" + selecteditem + ".ini";
            StringBuilder data;
            String namedata = null, imgpassdata = null, passdata = null, stimedata = null, startdata = null, cmtdata = null;

            if (File.Exists(defaultdir + "\\game.ini"))
            {
                //ini読込開始
                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "list",
                    "game",
                    "0",
                    data,
                    1024,
                    defaultdir + "\\game.ini");
                String max = data.ToString();

                toolStripStatusLabel1.Text = "[" + selecteditem + "/" + max + "]";
            }

            if (File.Exists(readini))
            {
                //ini読込開始
                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "name",
                    "",
                    data,
                    1024,
                    readini);
                namedata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "imgpass",
                    "",
                    data,
                    1024,
                    readini);
                imgpassdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "pass",
                    "",
                    data,
                    1024,
                    readini);
                passdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "time",
                    "0",
                    data,
                    1024,
                    readini);
                stimedata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "start",
                    "0",
                    data,
                    1024,
                    readini);
                startdata = data.ToString();

                data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "game",
                    "stat",
                    "",
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

            toolStripProgressBar1.Value = listBox1.SelectedIndex + 1;


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
            int max;
            if (File.Exists(defaultdir + "\\game.ini"))
            {
                //ini読込開始
                StringBuilder data = new StringBuilder(1024);
                GetPrivateProfileString(
                    "list",
                    "game",
                    "0",
                    data,
                    1024,
                    defaultdir + "\\game.ini");
                max = Convert.ToInt32(data.ToString());
            }
            else
            {
                return;
            }

            if (listBox1.Items.Count != 0)
            {
                String selectedtext = listBox1.SelectedItem.ToString();
                loaditem(defaultdir);
                if (listBox1.Items.Contains(selectedtext))
                {
                    listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext); ;
                }
                else
                {
                    loaditem(defaultdir);
                }
            }
            else
            {
                loaditem(defaultdir);
            }
            return;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                Clipboard.SetText(textBox1.Text);
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                Clipboard.SetText(textBox2.Text);
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                Clipboard.SetText(textBox3.Text);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
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
                MessageBox.Show("dkill.batが見つかりません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return;
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            pictureBox11.Visible = true;
            String selectedtext = listBox1.SelectedItem.ToString();

            loaditem(defaultdir);
            if (listBox1.Items.Contains(selectedtext))
            {
                listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
            }
            pictureBox11.Visible = false;
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

        private void iniedtchk(String inifilename, String sec, String key, String data)
        {
            pictureBox11.Visible = true;

            String pass = defaultdir + "\\" + inifilename + ".ini";
            if (File.Exists(pass))
            {
                //ini読込
                StringBuilder rdata = new StringBuilder(1024);
                GetPrivateProfileString(
                    sec,
                    key,
                    "0",
                    rdata,
                    1024,
                    pass);
                String rawdata = rdata.ToString();

                //取得値とデータが違う場合
                if (rawdata != data)
                {

                    if (checkBox3.Checked)
                    {
                        fileSystemWatcher1.EnableRaisingEvents = false;
                    }

                    WritePrivateProfileString(
                            sec,
                            key,
                            data.ToString(),
                            pass);

                    if (checkBox3.Checked)
                    {
                        fileSystemWatcher1.EnableRaisingEvents = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n" + pass, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            pictureBox11.Visible = false;
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String pass = defaultdir + "\\game.ini";

            if (File.Exists(pass))
            {
                //ini読込
                StringBuilder rdata = new StringBuilder(1024);
                GetPrivateProfileString(
                    "list",
                    "game",
                    "0",
                    rdata,
                    1024,
                    pass);
                int rawdata = Convert.ToInt32((rdata).ToString());

                if (rawdata >= 1)
                {
                    //乱数生成
                    System.Random r = new System.Random();
                    int ans = r.Next(1, rawdata + 1);

                    listBox1.SelectedIndex = ans - 1;
                }
                else
                {
                    MessageBox.Show("登録ゲーム数が少ないため、ランダム選択できません！", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n" + pass, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }
    }
}
