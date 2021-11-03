/*
 * 
 * アプリケーション名：Game Launcher C# Edition
 * ファイル名：Form1.cs
 * 
 * 作者：Ogura Deko
 * メール：support_dekosoft@outlook.jp
 * WEB：https://sunkun.nippombashi.net
 * 
 * バージョン履歴
 * 管理番号A001 2021/05/31 テスト起動機能追加
 * 管理番号A002 2021/06/01 テスト起動の修正
 */
 
using Microsoft.VisualBasic;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

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

		public String gamedir, basedir = AppDomain.CurrentDomain.BaseDirectory + "\\", gameini, configini = AppDomain.CurrentDomain.BaseDirectory + "\\config.ini";
		public static String appname = "Game Launcher C# Edition";
		public static String appver = "0.95";
		public static String appbuild = "17.21.11.03";
		public int gamemax = 0, pfmax = 0;
		private string dconpath = "";

		//棒読みちゃん関係
		public bool byActive = false;
		public String bysMsg = null;
		public byte byCode = 2; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
		public Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;
		public byte[] byMsg;
		public Int32 byLength;
		public string byHost = "127.0.0.1";
		public int byPort = 50001;
		public TcpClient tc = null;

		//profile
		//config.ini, game.ini自動更新無効有効

		Form2 form2 = new Form2();

		public gl()
		{
			InitializeComponent();
		}

		private void gl_Load(object sender, EventArgs e)
		{
			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			pictureBox11.Visible = true;

			updateconfig();

			String item = loaditem(gamedir.ToString());

			if (item == "_none_game_data" || item == "0")
			{

				if (!(File.Exists(gameini)))
				{
					iniwrite(gameini, "list", "game", "0");
				}


				//itemがnoneの場合：ゲームが登録されていない場合
				MessageBox.Show("Game Launcherをご利用頂きありがとうございます。\n\"追加\"ボタンを押して、ゲームを追加しましょう！",
								appname,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
			}

			if (Directory.Exists(gamedir))
			{
				fileSystemWatcher1.Path = gamedir;
			}
			else
			{
				Directory.CreateDirectory(gamedir);
				fileSystemWatcher1.Path = gamedir;
			}
			this.Activate();
			pictureBox11.Visible = false;
		}

		/// <summary>
		/// ゲームリストの読み込み
		/// </summary>
		/// <param name="gamedirname">iniファイルが格納されているフォルダ</param>
		/// <returns>異常なら"_none_game_data"を返します。正常に処理した場合は空欄が返されます。</returns>
		public string loaditem(String gamedirname)
		{
			listBox1.Items.Clear();
			listView1.Items.Clear();

			//全ゲーム数取得
			if (File.Exists(gameini))
			{
				gamemax = Convert.ToInt32(iniread(gameini, "list", "game", "0"));
				button8.Enabled = true;
				button9.Enabled = true;
				button2.Enabled = true;
				button5.Enabled = true;
			}
			else
			{
				//ゲームが1つもない場合
				button8.Enabled = false;
				button9.Enabled = false;
				button2.Enabled = false;
				button5.Enabled = false;
				return "_none_game_data";
			}

			if (!(gamemax >= 1)) //ゲーム登録数が1以上でない場合
			{
				button8.Enabled = false;
			}

			int count;
			String readini;
			String ans = "";
			Image lvimg;
			ListViewItem lvi = new ListViewItem();

			toolStripProgressBar1.Minimum = 0;
			toolStripProgressBar1.Maximum = gamemax;

			for (count = 1; count <= gamemax; count++)
			{
				//読込iniファイル名更新
				readini = gamedirname + "\\" + count + ".ini";

				if (File.Exists(readini))
				{
					listBox1.Items.Add(iniread(readini, "game", "name", ""));

					try
					{
						lvimg = System.Drawing.Image.FromFile(@iniread(readini, "game", "imgpass", ""));
					}
					catch
					{
						lvimg = glc_cs.Properties.Resources.exe;
					}

					imageList1.Images.Add(count.ToString(), lvimg);

					lvi = new ListViewItem(iniread(readini, "game", "name", ""));
					lvi.ImageIndex = (count - 1);
					listView1.Items.Add(lvi);
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

			//ゲームが登録されていれば1つ目を選択した状態にする
			if (gamemax >= 1)
			{
				listBox1.SelectedIndex = 0;
			}

			return ans;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			int startdata, timedata, ratinginfo;
			button9_Click(null, null);
			if (File.Exists(textBox2.Text))
			{
				if (!checkBox6.Checked)
				{
					if (checkBox1.Checked == true)
					{
						iniedtchk(gamedir + "\\" + (listBox1.SelectedIndex + 1).ToString() + ".ini", "game", "stat", textBox6.Text, "");

						//propertiesファイル書き込み
// 管理番号A001 From
						dconCheck();
						if (!File.Exists(dconpath))
						{
							MessageBox.Show("Discord Connectorが見つかりません。\n実行を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
// 管理番号A001 To
						String propertiesfile = System.IO.Path.GetDirectoryName(dconpath) + "\\run.properties";
						Encoding enc = Encoding.GetEncoding("Shift-JIS");
						StreamWriter writer = new StreamWriter(propertiesfile, false, enc);

						if (radioButton1.Checked)
						{
							ratinginfo = 0;
						}
						else
						{
							ratinginfo = 1;
						}

						if (checkBox4.Checked)
						{
							//センシティブモード有効
							writer.WriteLine("title = " + "Unknown" + "\nrating = " + ratinginfo + "\nstat = " + textBox6.Text);
						}
						else
						{
							writer.WriteLine("title = " + textBox1.Text + "\nrating = " + ratinginfo + "\nstat = " + textBox6.Text);
						}

						writer.Close();


						//現在時刻取得
						string strTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime starttime = Convert.ToDateTime(strTime);


						//dconの確認／実行
// 管理番号A001 From
						//string drun = AppDomain.CurrentDomain.BaseDirectory + "dcon.jar";
						dconCheck();
// 管理番号A001 To
						System.Diagnostics.Process drunp = null;
						if (checkBox5.Checked)
						{
// 管理番号A001 From
							//if(File.Exists(drun))
							if (File.Exists(dconpath))
// 管理番号A001 To
							{
								drunp = System.Diagnostics.Process.Start(dconpath); //dcon実行
							}
							else
							{
								MessageBox.Show("Discord Connectorが見つかりません。\n実行を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}

						//ウィンドウ最小化
						if (checkBox2.Checked == true)
						{
							this.WindowState = FormWindowState.Minimized;
							this.notifyIcon1.Visible = true;
						}

						//既定ディレクトリの変更
						String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
						System.Environment.CurrentDirectory = apppath;

						//起動中gifの可視化
						pictureBox11.Visible = true;

						//自動更新有効時のファイルウォッチャー無効化
						if (checkBox3.Checked)
						{
							fileSystemWatcher1.EnableRaisingEvents = false;
						}

						//ゲーム実行
						System.Diagnostics.Process p =
						System.Diagnostics.Process.Start(textBox2.Text);

						//棒読み上げ
						bouyomiage(textBox1.Text + "を、トラッキングありで起動しました。");

						//ゲーム終了まで待機
						p.WaitForExit();

						System.Environment.CurrentDirectory = basedir;


						if (checkBox2.Checked == true)
						{
							this.WindowState = FormWindowState.Normal;
							this.notifyIcon1.Visible = false;
						}

						//子プロセスの終了
						if (checkBox5.Checked)
						{
							KillChildProcess(drunp);
						}

						//終了時刻取得
						String time = p.ExitTime.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime endtime = Convert.ToDateTime(time);
						//DateTime endtime = DateTime.ParseExact(time, format, null);

						//起動時間計算
						String temp = (endtime - starttime).ToString();
						int anss = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

						bouyomiage("ゲームを終了しました。起動時間は、約" + anss + "秒です。");
						//ini上書き
						int selecteditem = listBox1.SelectedIndex + 1;
						String readini = gamedir + "\\" + selecteditem + ".ini";

						if (File.Exists(readini))
						{
							//既存値の取得
							timedata = Convert.ToInt32(iniread(readini, "game", "time", "0"));
							startdata = Convert.ToInt32(iniread(readini, "game", "start", "0"));

							//計算
							timedata += anss;
							startdata++;

							//書き換え
							iniwrite(readini, "game", "time", timedata.ToString());
							iniwrite(readini, "game", "start", startdata.ToString());
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

						bouyomiage(textBox1.Text + "を、トラッキングなしで起動しました。");
						System.Diagnostics.Process.Start(textBox2.Text);

						System.Environment.CurrentDirectory = basedir;
					}
				}
				else
				{
					//checkBox6.Checked
					MessageBox.Show("テスト起動モードが有効です。\nこのモードでは起動時間、起動回数、DiscordRPCなどは実行されません。\n\n無効にするには、[テスト起動]チェックを外してください。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);

// 管理番号A002 From
					//作業ディレクトリ変更
					String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
					System.Environment.CurrentDirectory = apppath;
// 管理番号A002 To

					//起動中gifの可視化
					pictureBox11.Visible = true;

					//ゲーム実行
					System.Diagnostics.Process p =
					System.Diagnostics.Process.Start(textBox2.Text);

					//棒読み上げ
					bouyomiage(textBox1.Text + "を、テストモードで起動しました。");

					//ゲーム終了まで待機
					p.WaitForExit();

// 管理番号A002 From
					//作業ディレクトリ復元
					System.Environment.CurrentDirectory = basedir;
// 管理番号A002 To

					//起動中gifの非可視化
					pictureBox11.Visible = false;

					//終了検出後
					DialogResult dr = MessageBox.Show("実行終了を検出しました。\nゲームが正しく終了しましたか？", appname, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dr == DialogResult.Yes)
					{
						MessageBox.Show("正常にトラッキングできています。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show("トラッキングに失敗しています。\n\n以下をご確認ください。\n\n・ランチャーを指定していませんか？\n・GLを管理者権限で起動してみてください。\n・実行パスを英数字のみにしてみてください。\n\nそれでも解決しない場合は、トラッキングできません。ご了承ください。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}

				}
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			String exeans = "", imgans = "", ratedata = "";
			openFileDialog1.Title = "追加する実行ファイルを選択";
			openFileDialog1.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				exeans = openFileDialog1.FileName;
			}
			else
			{
				return;
			}

			openFileDialog1.Title = "実行ファイルの画像を選択";
			openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif;*.ico)|*.png;*.jpg;*.bmp;*.gif;*.ico|すべてのファイル (*.*)|*.*";
			openFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(exeans).ToString() + ".png";
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

			String gfilepass = "";

			DialogResult rate = MessageBox.Show("成人向け(R-18)ゲームですか？", appname, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (rate == DialogResult.Yes)
			{
				ratedata = "1";
			}
			else
			{
				ratedata = "0";
			}

			if (File.Exists(gameini))
			{

				if (checkBox3.Checked)
				{
					fileSystemWatcher1.EnableRaisingEvents = false;
				}

				int newmaxval = gamemax + 1;

				gfilepass = gamedir + "\\" + newmaxval + ".ini";

				if (!(File.Exists(gfilepass)))
				{
					iniwrite(gfilepass, "game", "name", appname_ans);
					iniwrite(gfilepass, "game", "imgpass", imgans);
					iniwrite(gfilepass, "game", "pass", exeans);
					iniwrite(gfilepass, "game", "time", "0");
					iniwrite(gfilepass, "game", "start", "0");
					iniwrite(gfilepass, "game", "stat", "");
					iniwrite(gfilepass, "game", "rating", ratedata);
					iniwrite(gameini, "list", "game", newmaxval.ToString());
				}
				else
				{
					String dup = iniread(gfilepass, "game", "name", "unknown");
					DialogResult dialogResult = MessageBox.Show("既にiniファイルが存在します！\n" + gfilepass + "\n[" + dup + "]\n上書きしますか？", appname, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
					if (dialogResult == DialogResult.Yes)
					{
						iniwrite(gfilepass, "game", "name", appname_ans);
						iniwrite(gfilepass, "game", "imgpass", imgans);
						iniwrite(gfilepass, "game", "pass", exeans);
						iniwrite(gfilepass, "game", "time", "0");
						iniwrite(gfilepass, "game", "start", "0");
						iniwrite(gfilepass, "game", "stat", "");
						iniwrite(gfilepass, "game", "rating", ratedata);
						iniwrite(gameini, "list", "game", newmaxval.ToString());
					}
					else if (dialogResult == DialogResult.No)
					{
						MessageBox.Show("新規のゲームを追加せずに処理を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show("不明な結果です。\n実行を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					return;
				}

				if (checkBox3.Checked)
				{
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
			}
			else
			{
				MessageBox.Show("ゲーム情報統括管理ファイルが見つかりません！", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			button8_Click(null, null);

		}

		private void button7_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(appname + " Ver." + appver + " / Build " + appbuild + " Beta\n\n" + "現在の作業ディレクトリ：" + gamedir + "\n\nAuthor: Ogura Deko (dekosoft)\nMail: support_dekosoft@outlook.jp\nWeb: https://sunkun.nippombashi.net\n\nCopyright (C) Ogura Deko and dekosoft All rights reserved.",
								appname,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(gamemax >= 1))
			{
				return;
			}

			//グリッドと同期
			listView1.Items[listBox1.SelectedIndex].Selected = true;

			//ゲーム詳細取得
			int selecteditem = listBox1.SelectedIndex + 1;
			String readini = gamedir + "\\" + selecteditem + ".ini";
			String namedata = null, imgpassdata = null, passdata = null, stimedata = null, startdata = null, cmtdata = null, rating = null;


			if (File.Exists(readini))
			{
				//ini読込開始
				namedata = iniread(readini, "game", "name", "");
				imgpassdata = iniread(readini, "game", "imgpass", "");
				passdata = iniread(readini, "game", "pass", "");
				stimedata = iniread(readini, "game", "time", "0");
				startdata = iniread(readini, "game", "start", "0");
				cmtdata = iniread(readini, "game", "stat", "");
				rating = iniread(readini, "game", "rating", "-1");
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
			toolStripStatusLabel1.Text = "[" + selecteditem + "/" + gamemax + "]";
			toolStripProgressBar1.Value = listBox1.SelectedIndex + 1;

			if (Convert.ToInt32(rating) == 0)
			{
				radioButton1.Checked = true;
			}
			else if (Convert.ToInt32(rating) == 1)
			{
				radioButton2.Checked = true;
			}

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
			if (File.Exists(gameini))
			{
				//ini読込開始
				gamemax = Convert.ToInt32(iniread(gameini, "list", "game", "0"));
			}
			else
			{
				return;
			}

			if (listBox1.Items.Count != 0)
			{
				String selectedtext = listBox1.SelectedItem.ToString();
				loaditem(gamedir);
				if (listBox1.Items.Contains(selectedtext))
				{
					listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext); ;
				}
				else
				{
					loaditem(gamedir);
				}
			}
			else
			{
				loaditem(gamedir);
			}
			return;
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			if (!(textBox1.Text.Equals("")))
			{
				Clipboard.SetText(textBox1.Text);
			}
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			if (!(textBox2.Text.Equals("")))
			{
				Clipboard.SetText(textBox2.Text);
			}
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
			if (!(textBox3.Text.Equals("")))
			{
				Clipboard.SetText(textBox3.Text);
			}
		}

		private void pictureBox9_Click(object sender, EventArgs e)
		{
			if (!(textBox4.Text.Equals("")))
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

		private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
		{
			pictureBox11.Visible = true;
			String selectedtext = listBox1.SelectedItem.ToString();

			loaditem(gamedir);
			if (listBox1.Items.Contains(selectedtext))
			{
				listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
			}
			pictureBox11.Visible = false;
			return;
		}
		private void fileSystemWatcher2_Changed(object sender, FileSystemEventArgs e)
		{
			pictureBox11.Visible = true;
			String selectedtext = listBox1.SelectedItem.ToString();

			updateconfig();
			loaditem(gamedir);
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
				fileSystemWatcher2.EnableRaisingEvents = true;
			}
			else
			{
				fileSystemWatcher1.EnableRaisingEvents = false;
				fileSystemWatcher2.EnableRaisingEvents = false;
			}
		}

		private void iniedtchk(String ininame, String sec, String key, String data, String failedval)
		{
			pictureBox11.Visible = true;
			fileSystemWatcher1.EnableRaisingEvents = false;
			fileSystemWatcher2.EnableRaisingEvents = false;

			if (File.Exists(ininame))
			{
				String rawdata = iniread(ininame, sec, key, failedval);

				//取得値とデータが違う場合
				if (!(rawdata.Equals(data)))
				{
					if (checkBox3.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = false;
					}

					iniwrite(ininame, sec, key, data);

					if (checkBox3.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
				}
			}
			else
			{
				MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n" + ininame, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (checkBox3.Checked)
			{
				fileSystemWatcher1.EnableRaisingEvents = true;
				fileSystemWatcher2.EnableRaisingEvents = true;
			}
			pictureBox11.Visible = false;
			return;
		}

		private string iniread(String filename, String sec, String key, String failedval)
		{
			String ans = "";

			StringBuilder data = new StringBuilder(1024);
			GetPrivateProfileString(
				sec,
				key,
				failedval,
				data,
				1024,
				filename);

			ans = data.ToString();
			return ans;
		}

		private void iniwrite(String filename, String sec, String key, String data)
		{
			if (!File.Exists(filename))
			{
				if (!File.Exists(System.IO.Path.GetDirectoryName(filename)))
				{
					Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filename));
				}
				File.Create(filename).Close();
			}
			WritePrivateProfileString(
							sec,
							key,
							data.ToString(),
							filename);

			return;
		}

		private void updateconfig()
		{
			if (File.Exists(configini))
			{
				int ckv0 = Convert.ToInt32(iniread(configini, "checkbox", "track", "0"));
				int ckv1 = Convert.ToInt32(iniread(configini, "checkbox", "winmini", "0"));
				gamedir = iniread(configini, "default", "directory", basedir) + "Data";
				gameini = gamedir + "\\game.ini";
				String bgimg = iniread(configini, "imgd", "bgimg", "");
				checkBox4.Checked = Convert.ToBoolean(Convert.ToInt32(iniread(configini, "checkbox", "sens", "0")));
				checkBox5.Checked = Convert.ToBoolean(Convert.ToInt32(iniread(configini, "checkbox", "dconnect", "1")));
				if (Convert.ToInt32(iniread(configini, "checkbox", "rate", "-1")) == 1)
				{
					radioButton2.Checked = true;
				}
				else
				{
					radioButton1.Checked = true;
				}

				if (Convert.ToInt32(iniread(configini, "connect", "byActive", "0")) == 1)
				{
					byActive = true;
					bouyomi_configload();
				}
				else
				{
					byActive = false;
				}

				if (File.Exists(bgimg))
				{
					this.BackgroundImage = new Bitmap(bgimg);
					toolStripStatusLabel2.Text = "！背景画像が適用されました。環境によっては描画に時間がかかる場合があります。！";
				}
				else
				{
					this.BackgroundImage = null;
					message();
				}

				checkBox1.Checked = Convert.ToBoolean(ckv0);
				checkBox2.Checked = Convert.ToBoolean(ckv1);

				fileSystemWatcher2.Path = basedir;
			}
			else
			{
				//ini存在しない場合
				File.Create(configini).Close();
				gamedir = basedir + "Data";
				gameini = gamedir + "\\game.ini";
				checkBox1.Checked = true;
				checkBox2.Checked = true;
				fileSystemWatcher2.Path = basedir;
			}
			return;
		}

		private void button2_Click(object sender, EventArgs e)
		{

			if (File.Exists(gameini))
			{

				int rawdata = Convert.ToInt32(iniread(gameini, "list", "game", "0"));

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
				MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
			bouyomiage("ゲームランチャーを終了しました");
			this.ShowInTaskbar = false;
			this.Dispose();
			Close();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//選択中のゲーム保管ファイルを削除
			pictureBox11.Visible = true;
			int delval = listBox1.SelectedIndex + 1;
			String delname = textBox1.Text;
			delini(delval, delname);

			pictureBox11.Visible = false;
			return;
		}

		private void button9_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			String selectedtext = listBox1.SelectedItem.ToString();

			//Discordカスタムステータス、各種チェックボックス、ラジオの保存
			String pass = gamedir + "\\" + (listBox1.SelectedIndex + 1) + ".ini";
			if (File.Exists(pass))
			{
				iniedtchk(pass, "game", "stat", (textBox6.Text), "");
				iniedtchk(configini, "checkbox", "track", (Convert.ToInt32(checkBox1.Checked)).ToString(), "0");
				iniedtchk(configini, "checkbox", "winmini", (Convert.ToInt32(checkBox2.Checked)).ToString(), "0");
				iniedtchk(configini, "checkbox", "sens", (Convert.ToInt32(checkBox4.Checked)).ToString(), "0");
				if (radioButton1.Checked)
				{
					iniedtchk(configini, "checkbox", "rate", "0", "-1");
				}
				else
				{
					iniedtchk(configini, "checkbox", "rate", "1", "-1");
				}
				loaditem(gamedir);

				//更新前に選択していたゲームへ移動
				if (listBox1.Items.Contains(selectedtext))
				{
					listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
				}
			}
			else
			{
				//個別ini不存在
				MessageBox.Show("ゲーム情報保管iniが存在しません。\n" + pass, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			return;
		}

		private void toolStripStatusLabel3_Click(object sender, EventArgs e)
		{
			//ini読込
			String rawdata = gamedir + "\\" + ((listBox1.SelectedIndex + 1).ToString()) + ".ini";

			if (File.Exists(rawdata))
			{
				System.Diagnostics.Process.Start(@gamedir + "\\" + (listBox1.SelectedIndex + 1) + ".ini");
			}
			else
			{
				MessageBox.Show("ゲーム情報保管iniがありません！\n" + rawdata, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			int selected = listBox1.SelectedIndex;
			if (selected >= 1)
			{
				selected++;
				fileSystemWatcher1.EnableRaisingEvents = false;
				int target = selected - 1;
				String before = gamedir + "\\" + (selected.ToString()) + ".ini";
				String temp = gamedir + "\\" + (target.ToString()) + "_.ini";
				String after = gamedir + "\\" + (target.ToString()) + ".ini";
				if (File.Exists(before) && File.Exists(after))
				{
					File.Move(after, temp);
					File.Move(before, after);
					File.Move(temp, before);
				}
				else
				{
					MessageBox.Show("移動先もしくは移動前のファイルが見つかりません。\n\n移動前：" + before + "\n移動先：" + after, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				fileSystemWatcher1.EnableRaisingEvents = true;
			}
			else
			{
				MessageBox.Show("最上位です。\nこれ以上は上に移動できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			loaditem(gamedir);
			listBox1.SelectedIndex = selected - 2;
			return;
		}

		private void pictureBox13_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "")
			{
				String opendir = System.IO.Path.GetDirectoryName(textBox2.Text);
				if (Directory.Exists(opendir))
				{
					System.Diagnostics.Process.Start(opendir);
				}
			}
		}

		private void pictureBox14_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "")
			{
				String opendir = System.IO.Path.GetDirectoryName(textBox2.Text);
				if (Directory.Exists(opendir))
				{
					System.Diagnostics.Process.Start(opendir);
				}
			}
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox6.Checked)
			{
				checkBox1.Enabled = false;
			}
			else
			{
				checkBox1.Enabled = true;
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			int selected = listBox1.SelectedIndex;
			if (selected + 1 < gamemax)
			{
				selected++;
				fileSystemWatcher1.EnableRaisingEvents = false;
				int target = selected + 1;
				String before = gamedir + "\\" + (selected.ToString()) + ".ini";
				String temp = gamedir + "\\" + (target.ToString()) + "_.ini";
				String after = gamedir + "\\" + (target.ToString()) + ".ini";
				if (File.Exists(before) && File.Exists(after))
				{
					File.Move(after, temp);
					File.Move(before, after);
					File.Move(temp, before);
				}
				else
				{
					MessageBox.Show("移動先もしくは移動前のファイルが見つかりません。\nファイルに影響はありません。\n\n移動前：" + before + "\n移動先：" + after, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
					fileSystemWatcher1.EnableRaisingEvents = true;
					return;
				}
				fileSystemWatcher1.EnableRaisingEvents = true;
			}
			else
			{
				MessageBox.Show("最下位です。\nこれ以上は下に移動できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			loaditem(gamedir);
			listBox1.SelectedIndex = selected--;
			return;
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(gamemax >= 1))
			{
				return;
			}

			if (listView1.SelectedItems.Count <= 0)
			{
				return;
			}

			//リストと同期
			listBox1.SelectedIndex = Convert.ToInt32(listView1.SelectedItems[0].Index);

			return;

		}

		private void button4_Click(object sender, EventArgs e)
		{
			//設定
			fileSystemWatcher1.EnableRaisingEvents = false;
			fileSystemWatcher2.EnableRaisingEvents = false;
			String before = iniread(configini, "default", "directory", "0");
			form2.ShowDialog();
			String after = iniread(configini, "default", "directory", "0");
			if (before != after)
			{
				MessageBox.Show("既定の作業ディレクトリが変更されました。\nGame Launcherを再度起動してください。\n\nOKを押してGame Launcherを終了します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
			}
			fileSystemWatcher1.EnableRaisingEvents = true;
			fileSystemWatcher2.EnableRaisingEvents = true;
			updateconfig();
			return;
		}

		private void message()
		{
			String ans = "";
			int tmp;
			System.Random r = new System.Random();
			tmp = r.Next(1, 3);

			switch (tmp)
			{
				case 1:
					ans = "＊背景画像を設定すると、画面の描画に時間がかかる場合があります。";
					break;

				case 2:
					ans = "＊変更を保存するには「保存」か「起動」を押します。";
					break;

				case 3:
					ans = "＊本ソフトウェアはベータ版です。エラーが発生した場合はご連絡ください。";
					break;
			}
			toolStripStatusLabel2.Text = ans;
			return;
		}

		private void delini(int delfileval, String delfilename)
		{
			fileSystemWatcher2.EnableRaisingEvents = false;
			fileSystemWatcher1.EnableRaisingEvents = false;

			int nextval;
			String nextfile;
			int delval = delfileval;
			String delfile = (gamedir + "\\" + delval + ".ini");
			if (File.Exists(delfile))
			{
				//削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("選択中のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delfilename + "]\n" + delfile + "\n削除しますか？", appname, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					File.Delete(delfile);
					nextval = delval + 1;
					nextfile = (gamedir + "\\" + nextval + ".ini");
					while (File.Exists(nextfile))
					{
						//削除ファイル以降にゲームが存在する場合に番号を下げる
						File.Move(nextfile, delfile);
						delfile = nextfile;
						nextval++;
						nextfile = (gamedir + "\\" + nextval + ".ini");
					}
					gamemax--;
					iniwrite(gameini, "list", "game", gamemax.ToString());
				}
				else if (dialogResult == DialogResult.No)
				{
					fileSystemWatcher2.EnableRaisingEvents = true;
					if (checkBox3.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
					return;
				}
				else
				{
					MessageBox.Show("不明な結果です。\n実行を中断します。", appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				//削除ファイル不存在
				MessageBox.Show("該当するiniファイルが存在しません。\n" + delfile, appname, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			loaditem(gamedir);
			if (gamemax >= 2 && delfileval >= 2)
			{
				listBox1.SelectedIndex = delfileval - 2;
			}
			else if (gamemax == 1 && delfileval >= 1)
			{
				listBox1.SelectedIndex = 1;
			}
			else
			{
				listBox1.SelectedIndex = 0;
			}

			fileSystemWatcher2.EnableRaisingEvents = true;
			if (checkBox3.Checked)
			{
				fileSystemWatcher1.EnableRaisingEvents = true;
			}
			return;
		}

		void KillChildProcess(System.Diagnostics.Process process)
		{
			try
			{
				process.Kill();
			}
			catch (Exception)
			{
				MessageBox.Show("既にdcon.jarが終了しています。\n起動時間が正常に記録されない可能性があります。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void bouyomi_configload()
		{
			byHost = iniread(configini, "connect", "byHost", "127.0.0.1");
			byPort = Convert.ToInt32(iniread(configini, "connect", "byPort", "50001"));
			bouyomi_connectchk();
			return;
		}

		private void bouyomi_connectchk()
		{
			String bysMsg = "ゲームランチャーと接続しました";
			byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
			Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;
			TcpClient tc = null;

			byte[] bybMsg = Encoding.UTF8.GetBytes(bysMsg);
			Int32 byLength = bybMsg.Length;

			if (Convert.ToInt32(iniread(configini, "connect", "byType", "0")) == 1)
			{
				string url = "http://localhost:" + iniread(configini, "connect", "byPort", "50080") + "/talk";
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
				catch (Exception)
				{
					MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					return;
				}
				wc.Dispose();
			}
			else
			{
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
			}
			return;
		}

		private void bouyomiage(String text)
		{
			if (byActive)
			{
				byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
				Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;
				TcpClient tc;

				byte[] bybMsg = Encoding.UTF8.GetBytes(text);
				Int32 byLength = bybMsg.Length;

				if (Convert.ToInt32(iniread(configini, "connect", "byType", "0")) == 1)
				{
					string url = "http://localhost:" + iniread(configini, "connect", "byPort", "50080") + "/talk";
					System.Net.WebClient wc = new System.Net.WebClient();
					//NameValueCollectionの作成
					System.Collections.Specialized.NameValueCollection ps =
						new System.Collections.Specialized.NameValueCollection();
					//送信するデータ（フィールド名と値の組み合わせ）を追加
					ps.Add("text", text);
					//データを送信し、また受信する
					try
					{
						wc.UploadValues(url, ps);
					}
					catch (Exception)
					{
						MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
					wc.Dispose();
				}
				else
				{
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
				}
			}
			return;
		}

		private void dconCheck()
		{
			dconpath = iniread(configini, "connect", "dconpath", "-1");
			if (!File.Exists(dconpath))
			{
				if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "dcon.jar"))
				{
					//アプリケーションルートに存在する場合
					dconpath = AppDomain.CurrentDomain.BaseDirectory + "dcon.jar";
				}else{
					dconpath = "-1";
				}
			}
			return;
		}
	}
}
