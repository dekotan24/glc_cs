using Microsoft.VisualBasic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class gl : Form
	{
		// 設定フォーム宣言
		Form2 form2 = new Form2();
		Form3 form3 = new Form3();
		About about = new About();

		// 外部ソースを宣言
		General.Var gv = new General.Var();


		public gl()
		{
			InitializeComponent();
		}

		private void gl_Load(object sender, EventArgs e)
		{
			form3.Show();
			Application.DoEvents();
			this.Opacity = 0;
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			pictureBox11.Visible = true;

			updateComponent();

			String item = gv.SaveType == "I" ? LoadItem(gv.GameDir) : LoadItem2(gv.SqlCon, true);
			tabControl1.SelectedIndex = 1;
			tabControl1.SelectedIndex = 0;

			pictureBox11.Visible = false;
			this.Refresh();
			this.Show();
			form3.Close();
			form3.Dispose();
			Application.DoEvents();
			this.Opacity = 1;

			if (item == "_none_game_data" || item == "0")
			{

				if (gv.SaveType == "I" || gv.SaveType == "T")
				{
					// ini
					if (!(File.Exists(gv.GameIni)))
					{
						gv.IniWrite(gv.GameIni, "list", "game", "0");
					}


					if (Directory.Exists(gv.GameDir))
					{
						fileSystemWatcher1.Path = gv.GameDir;
					}
					else
					{
						try
						{
							Directory.CreateDirectory(gv.GameDir);
							fileSystemWatcher1.Path = gv.GameDir;
						}
						catch (Exception ex)
						{
							resolveError("gl_Load", ex.Message, 0);
						}
					}
				}
				//itemがnoneの場合：ゲームが登録されていない場合
				MessageBox.Show("Game Launcherをご利用頂きありがとうございます。\n\"追加\"ボタンを押して、ゲームを追加しましょう！",
								gv.AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
			}
		}
		/// <summary>
		/// ゲームリストの読み込み
		/// </summary>
		/// <param name="gameDirname">iniファイルが格納されているフォルダ</param>
		/// <returns>異常なら"_none_game_data"を返します。正常に処理した場合は空欄が返されます。</returns>
		public string LoadItem(String gameDirname)
		{
			listBox1.Items.Clear();
			listView1.Items.Clear();
			imageList0.Images.Clear();
			imageList1.Images.Clear();
			imageList2.Images.Clear();

			//全ゲーム数取得
			if (File.Exists(gv.GameIni))
			{
				gv.GameMax = Convert.ToInt32(gv.IniRead(gv.GameIni, "list", "game", "0"));
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

			if (!(gv.GameMax >= 1)) //ゲーム登録数が1以上でない場合
			{
				button8.Enabled = false;
			}


			int count;
			String readini;
			String ans = "";
			Image lvimg;
			ListViewItem lvi = new ListViewItem();

			toolStripProgressBar1.Minimum = 0;
			toolStripProgressBar1.Maximum = gv.GameMax;

			for (count = 1; count <= gv.GameMax; count++)
			{
				toolStripProgressBar1.Value = count;
				//読込iniファイル名更新
				readini = gameDirname + "\\" + count + ".ini";

				if (File.Exists(readini))
				{
					listBox1.Items.Add(gv.IniRead(readini, "game", "name", ""));

					try
					{
						lvimg = Image.FromFile(gv.IniRead(readini, "game", "imgpass", ""));
					}
					catch
					{
						lvimg = Properties.Resources.exe;
					}

					imageList0.Images.Add(count.ToString(), lvimg);
					imageList1.Images.Add(count.ToString(), lvimg);
					imageList2.Images.Add(count.ToString(), lvimg);

					lvi = new ListViewItem(gv.IniRead(readini, "game", "name", ""));
					lvi.ImageIndex = (count - 1);
					listView1.Items.Add(lvi);
				}
				else
				{
					//個別ini存在しない場合
					resolveError("LoadItem", "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。", 0, false);
					this.ResumeLayout();
					break;
				}
			}

			//ゲームが登録されていれば1つ目を選択した状態にする
			if (gv.GameMax >= 1)
			{
				listBox1.SelectedIndex = 0;
			}

			Application.DoEvents();
			return ans;
		}

		/// <summary>
		/// データベースからゲーム一覧をロードします
		/// </summary>
		/// <param name="cn">SQL Connection</param>
		/// <returns></returns>
		private string LoadItem2(SqlConnection cn, bool firstLoad = false)
		{
			string ans = string.Empty;
			string errMessage = string.Empty;

			listBox1.Items.Clear();
			listBox2.Items.Clear();
			listView1.Items.Clear();
			imageList0.Images.Clear();
			imageList1.Images.Clear();
			imageList2.Images.Clear();

			textBox7.Text = string.Empty;
			textBox8.Text = string.Empty;

			try
			{
				// 読み込み処理
				cn.Open();

				//全ゲーム数取得
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + gv.DbName + "." + gv.DbTable
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					gv.GameMax = sqlAns;
					button8.Enabled = true;
					button9.Enabled = true;
					button2.Enabled = true;
					button5.Enabled = true;
				}
				else
				{
					//ゲームが1つもない場合
					gv.GameMax = 0;
					button8.Enabled = false;
					button9.Enabled = false;
					button2.Enabled = false;
					button5.Enabled = false;
					return "_none_game_data";
				}

				if (!(gv.GameMax >= 1)) //ゲーム登録数が1以上でない場合
				{
					button8.Enabled = false;
				}

				Image lvimg;
				ListViewItem lvi = new ListViewItem();

				toolStripProgressBar1.Minimum = 0;
				toolStripProgressBar1.Maximum = gv.GameMax;

				toolStripProgressBar1.Value = 0;

				// DBからデータを取り出す
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, IMG_PATH, ROW_CNT "
								+ " FROM ( SELECT ID, GAME_NAME, IMG_PATH, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + gv.DbName + "." + gv.DbTable + ") AS T "
				};
				cm2.Connection = cn;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						listBox1.Items.Add(reader["GAME_NAME"].ToString());

						try
						{
							lvimg = Image.FromFile(reader["IMG_PATH"].ToString());
						}
						catch
						{
							lvimg = Properties.Resources.exe;
						}

						imageList0.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
						imageList1.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
						imageList2.Images.Add(reader["ROW_CNT"].ToString(), lvimg);

						lvi = new ListViewItem(reader["GAME_NAME"].ToString());
						lvi.ImageIndex = (Convert.ToInt32(reader["ROW_CNT"]) - 1);
						listView1.Items.Add(lvi);
					}
				}

				//ゲームが登録されていれば1つ目を選択した状態にする
				if (gv.GameMax >= 1)
				{
					listBox1.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				gv.WriteErrorLog(ex.Message, "LoadItem2", cn.ConnectionString);
				errMessage = ex.Message;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			string localPath = gv.BaseDir + (gv.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
			string localGameIni = localPath + "game.ini";
			string backupDir = gv.BaseDir + (gv.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";

			if (errMessage.Length != 0)
			{
				// エラーがあった場合
				if (gv.OfflineSave && gv.SaveType == "D")
				{
					if (File.Exists(localGameIni))
					{
						// オフラインモード使用可能の場合
						tabControl1.Controls.Remove(tabPage3);
						gv.SaveType = "T";
						gv.GameDir = localPath;
						gv.GameIni = localGameIni;
						ans = LoadItem(localPath);
						if (firstLoad)
						{
							toolStripStatusLabel3.Visible = true;
							button6.Visible = true;
							button10.Visible = true;
							button9.Visible = true;
							resolveError(MethodBase.GetCurrentMethod().Name, "データベースに接続できなかった為、オフラインモードで起動します。", 0, false);
						}
					}
					else
					{
						resolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false);
					}
				}
				else
				{
					resolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false);
				}
			}
			else
			{
				// エラーがなかったとき、初回ロード時のみオフラインモードのチェックを行う
				if (firstLoad)
				{
					// オフラインモードで変更がなかったかチェック
					if (gv.IniRead(localGameIni, "list", "dbupdate", "0") == "1")
					{
						DialogResult dr = MessageBox.Show("オフラインモード実行時に変更がありました。\nデータベースへアップロードしますか？\n\n※データベースのテーブルを全削除し、オフラインのデータを登録します。\n\n[はい]	登録\n[いいえ]	破棄", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dr == DialogResult.Yes)
						{
							int tmpMaxGameCount, sCount, fCount;
							// データベース登録
							int returnVal = gv.InsertIni2Db(localPath, backupDir, out tmpMaxGameCount, out sCount, out fCount);
							if (returnVal == 0)
							{
								gv.IniWrite(localGameIni, "list", "dbupdate", "0");
								MessageBox.Show("処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)\n\n", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							else
							{
								switch (returnVal)
								{
									case 1:
										// バックアップ作成エラー
										resolveError(MethodBase.GetCurrentMethod().Name, "バックアップの作成中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 2:
										// insertエラー
										resolveError(MethodBase.GetCurrentMethod().Name, "Insert中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 3:
										// Catchエラー
									case 4:
										// 復元エラー
									default:
										// 不明なエラー
										resolveError(MethodBase.GetCurrentMethod().Name, "致命的なエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
								}
							}
						}
					}
				}
			}
			Application.DoEvents();
			return ans;
		}

		/// <summary>
		/// Start button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			int startdata, timedata, ratinginfo;

			if (listBox1.SelectedIndex == -1)
			{
				resolveError(MethodBase.GetCurrentMethod().Name, "ゲームリストが空です。", 0, false);
				return;
			}
			String selectedtext = listBox1.SelectedItem.ToString();

			// 共通部分を保存
			iniedtchk(gv.ConfigIni, "checkbox", "track", (Convert.ToInt32(checkBox1.Checked)).ToString(), "0");
			iniedtchk(gv.ConfigIni, "checkbox", "winmini", (Convert.ToInt32(checkBox2.Checked)).ToString(), "0");
			iniedtchk(gv.ConfigIni, "checkbox", "sens", (Convert.ToInt32(checkBox4.Checked)).ToString(), "0");

			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// iniの場合、ステータス状態を書き込み
				// Discordカスタムステータス、各種チェックボックス、ラジオの保存
				String path = gv.GameDir + "\\" + (listBox1.SelectedIndex + 1) + ".ini";
				if (File.Exists(path))
				{
					iniedtchk(path, "game", "stat", (textBox6.Text), "");
					if (radioButton1.Checked)
					{
						iniedtchk(path, "game", "rating", "0", "0");
					}
					else
					{
						iniedtchk(path, "game", "rating", "1", "0");
					}

					// 更新前に選択していたゲームへ移動
					if (listBox1.Items.Contains(selectedtext))
					{
						listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
					}
				}
				else
				{
					// 個別ini不存在
					resolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報保管iniが存在しません。\n" + path, 0, false);
				}
			}

			if (File.Exists(textBox2.Text))
			{
				if (!checkBox6.Checked)
				{
					if (checkBox1.Checked)
					{
						if (gv.SaveType == "I" || gv.SaveType == "T")
						{
							iniedtchk(gv.GameDir + "\\" + (listBox1.SelectedIndex + 1).ToString() + ".ini", "game", "stat", textBox6.Text, "");
						}

						// 現在時刻取得
						string strTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime starttime = Convert.ToDateTime(strTime);

						// 実行
						System.Diagnostics.Process drunp = null;
						if (checkBox5.Checked)
						{
							if (File.Exists(gv.DconPath))
							{
								// propertiesファイル書き込み
								String propertiesfile = System.IO.Path.GetDirectoryName(gv.DconPath) + "\\run.properties";
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
									// センシティブモード有効
									writer.WriteLine("title = " + "Unknown" + "\nrating = " + ratinginfo + "\nstat = " + textBox6.Text);
								}
								else
								{
									writer.WriteLine("title = " + textBox1.Text + "\nrating = " + ratinginfo + "\nstat = " + textBox6.Text);
								}

								writer.Close();

								drunp = System.Diagnostics.Process.Start(gv.DconPath); //dcon実行
							}
							else
							{
								resolveError(MethodBase.GetCurrentMethod().Name, "Discord Connectorが見つかりません。\n実行を中断します。", 0, false);
								return;
							}
						}

						// ウィンドウ最小化
						if (checkBox2.Checked == true)
						{
							this.WindowState = FormWindowState.Minimized;
							this.notifyIcon1.Visible = true;
							//notifyIcon1.ShowBalloonTip(50, gv.AppName, textBox1.Text + "を実行中", ToolTipIcon.Info);
						}

						// 既定ディレクトリの変更
						String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
						System.Environment.CurrentDirectory = apppath;

						// 起動中gifの可視化
						pictureBox11.Visible = true;
						button1.Enabled = false;

						// 自動更新有効時のファイルウォッチャー無効化
						fileSystemWatcher1.EnableRaisingEvents = false;

						// ゲーム実行
						System.Diagnostics.Process p =
						System.Diagnostics.Process.Start(textBox2.Text);

						// 棒読み上げ
						if (gv.ByRoS)
						{
							gv.Bouyomiage(textBox1.Text + "を、トラッキングありで起動しました。");
						}

						// ゲーム終了まで待機
						p.WaitForExit();

						// ゲーム終了
						System.Environment.CurrentDirectory = gv.BaseDir;

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

						//起動時間計算
						String temp = (endtime - starttime).ToString();
						int anss = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

						if (gv.ByRoS)
						{
							gv.Bouyomiage("ゲームを終了しました。起動時間は、約" + anss + "秒です。");
						}

						if (gv.SaveType == "I" || gv.SaveType == "T")
						{
							// ini
							int selecteditem = listBox1.SelectedIndex + 1;
							String readini = gv.GameDir + "\\" + selecteditem + ".ini";

							if (File.Exists(readini))
							{
								//既存値の取得
								timedata = Convert.ToInt32(gv.IniRead(readini, "game", "time", "0"));
								startdata = Convert.ToInt32(gv.IniRead(readini, "game", "start", "0"));

								//計算
								timedata += anss;
								startdata++;

								//書き換え
								gv.IniWrite(readini, "game", "time", timedata.ToString());
								gv.IniWrite(readini, "game", "start", startdata.ToString());

								// 次回DB接続時に更新するフラグを立てる
								if (gv.SaveType == "T")
								{
									gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
								}
							}
							else
							{
								//個別ini存在しない場合
								resolveError(MethodBase.GetCurrentMethod().Name, "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。", 0, false);
							}
						}
						else
						{
							// database
							// SQL文構築
							SqlConnection cn = gv.SqlCon;
							SqlCommand cm = new SqlCommand
							{
								CommandType = CommandType.Text,
								CommandTimeout = 30,
								CommandText = @"UPDATE " + gv.DbName + "." + gv.DbTable + " SET UPTIME = CAST(CAST(UPTIME AS INT) + " + anss + " AS NVARCHAR), RUN_COUNT = CAST(CAST(RUN_COUNT AS INT) + 1 AS NVARCHAR), DCON_TEXT = '" + textBox6.Text.Trim() + "', AGE_FLG = '" + (radioButton1.Checked ? "0" : "1") + "', LAST_RUN = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' "
											+ " WHERE ID = '" + gv.CurrentGameDbVal + "'"
							};
							cm.Connection = cn;

							// SQL実行
							try
							{
								cn.Open();
								cm.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								gv.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
								resolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。", 0, false);
							}
							finally
							{
								if (cn.State == ConnectionState.Open)
								{
									cn.Close();
								}
							}

							listBox1_SelectedIndexChanged(null, null);
							if (gv.SaveType == "I" && checkBox3.Checked)
							{
								fileSystemWatcher1.EnableRaisingEvents = true;
							}
						}
						pictureBox11.Visible = false;
						button1.Enabled = true;
					}
					else
					{
						String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
						System.Environment.CurrentDirectory = apppath;

						if (gv.ByRoS)
						{
							gv.Bouyomiage(textBox1.Text + "を、トラッキングなしで起動しました。");
						}
						System.Diagnostics.Process.Start(textBox2.Text);

						System.Environment.CurrentDirectory = gv.BaseDir;
					}
				}
				else
				{
					//checkBox6.Checked
					MessageBox.Show("テスト起動モードが有効です。\nこのモードでは起動時間、起動回数、DiscordRPCなどは実行されません。\n\n無効にするには、[テスト起動]チェックを外してください。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

					//作業ディレクトリ変更
					String apppath = System.IO.Path.GetDirectoryName(textBox2.Text);
					System.Environment.CurrentDirectory = apppath;

					//起動中gifの可視化
					pictureBox11.Visible = true;
					button1.Enabled = false;

					//ウィンドウ最小化
					if (checkBox2.Checked == true)
					{
						this.WindowState = FormWindowState.Minimized;
						this.notifyIcon1.Visible = true;
						//notifyIcon1.ShowBalloonTip(50, gv.AppName, textBox1.Text + "を実行中", ToolTipIcon.Info);
					}

					//ゲーム実行
					System.Diagnostics.Process p =
					System.Diagnostics.Process.Start(textBox2.Text);

					//棒読み上げ
					if (gv.ByRoS)
					{
						gv.Bouyomiage(textBox1.Text + "を、テストモードで起動しました。");
					}

					//ゲーム終了まで待機
					p.WaitForExit();

					//作業ディレクトリ復元
					System.Environment.CurrentDirectory = gv.BaseDir;

					if (checkBox2.Checked == true)
					{
						this.WindowState = FormWindowState.Normal;
						this.notifyIcon1.Visible = false;
					}

					//起動中gifの非可視化
					pictureBox11.Visible = false;
					button1.Enabled = true;

					//終了検出後
					DialogResult dr = MessageBox.Show("実行終了を検出しました。\nゲームが正しく終了しましたか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dr == DialogResult.Yes)
					{
						MessageBox.Show("正常にトラッキングできています。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show("トラッキングに失敗しています。\n\n以下をご確認ください。\n\n・ランチャーを指定していませんか？\n・GLを管理者権限で起動してみてください。\n・実行パスを英数字のみにしてみてください。\n\nそれでも解決しない場合は、トラッキングできません。ご了承ください。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}

				}
			}
		}

		/// <summary>
		/// 追加ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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
			openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
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
				"アプリ名を設定", gv.AppName, filename, -1, -1);

			String gfilepass = "";

			DialogResult rate = MessageBox.Show("成人向け(R-18)ゲームですか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (rate == DialogResult.Yes)
			{
				ratedata = "1";
			}
			else
			{
				ratedata = "0";
			}

			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				if (File.Exists(gv.GameIni))
				{
					fileSystemWatcher1.EnableRaisingEvents = false;

					int newmaxval = gv.GameMax + 1;

					gfilepass = gv.GameDir + "\\" + newmaxval + ".ini";

					if (!(File.Exists(gfilepass)))
					{
						gv.IniWrite(gfilepass, "game", "name", appname_ans);
						gv.IniWrite(gfilepass, "game", "imgpass", imgans);
						gv.IniWrite(gfilepass, "game", "pass", exeans);
						gv.IniWrite(gfilepass, "game", "time", "0");
						gv.IniWrite(gfilepass, "game", "start", "0");
						gv.IniWrite(gfilepass, "game", "stat", "");
						gv.IniWrite(gfilepass, "game", "rating", ratedata);
						gv.IniWrite(gv.GameIni, "list", "game", newmaxval.ToString());

						// 次回DB接続時に更新するフラグを立てる
						if (gv.SaveType == "T")
						{
							gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						String dup = gv.IniRead(gfilepass, "game", "name", "unknown");
						DialogResult dialogResult = MessageBox.Show("既にiniファイルが存在します！\n" + gfilepass + "\n[" + dup + "]\n上書きしますか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dialogResult == DialogResult.Yes)
						{
							gv.IniWrite(gfilepass, "game", "name", appname_ans);
							gv.IniWrite(gfilepass, "game", "imgpass", imgans);
							gv.IniWrite(gfilepass, "game", "pass", exeans);
							gv.IniWrite(gfilepass, "game", "time", "0");
							gv.IniWrite(gfilepass, "game", "start", "0");
							gv.IniWrite(gfilepass, "game", "stat", "");
							gv.IniWrite(gfilepass, "game", "rating", ratedata);
							gv.IniWrite(gv.GameIni, "list", "game", newmaxval.ToString());

							// 次回DB接続時に更新するフラグを立てる
							if (gv.SaveType == "T")
							{
								gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
							}
						}
						else if (dialogResult == DialogResult.No)
						{
							MessageBox.Show("新規のゲームを追加せずに処理を中断します。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							resolveError("button3_Click", "不明な結果です。\n実行を中断します。", 0, false);
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
					resolveError("button3_Click", "ゲーム情報統括管理ファイルが見つかりません！", 0, false);
					return;
				}
			}
			else
			{
				// database

				SqlConnection cn = gv.SqlCon;
				SqlCommand cm;
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"INSERT INTO " + gv.DbName + "." + gv.DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG ) VALUES ( '" + appname_ans + "', '" + exeans + "', '" + imgans + "', '0', '0','', '" + ratedata + "' )"
				};
				cm.Connection = cn;

				try
				{
					cn.Open();
					cm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					gv.WriteErrorLog(ex.Message, "button3_Click", cm.CommandText);
					resolveError("button3_Click", ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}

			button8_Click(null, null);

		}

		private void button7_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(gv.AppName + " Ver." + gv.AppVer + " / Build " + gv.AppBuild + " Beta\n\n" + "現在の作業ディレクトリ：" + gv.GameDir + "\n\nAuthor: Ogura Deko (dekosoft)\nMail: support_dekosoft@outlook.jp\nWeb: https://sunkun.nippombashi.net\n\nCopyright (C) Ogura Deko and dekosoft All rights reserved.",
								gv.AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
			//about.ShowDialog(this);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(gv.GameMax >= 1))
			{
				return;
			}

			//グリッドと同期
			listView1.Items[listBox1.SelectedIndex].Selected = true;
			listView1.EnsureVisible(listBox1.SelectedIndex);

			//ゲーム詳細取得
			int selecteditem = listBox1.SelectedIndex + 1;
			String readini = gv.GameDir + "\\" + selecteditem + ".ini";
			String id = null, namedata = null, imgpassdata = null, passdata = null, stimedata = null, startdata = null, cmtdata = null, rating = null;

			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				if (File.Exists(readini))
				{
					//ini読込開始
					namedata = gv.IniRead(readini, "game", "name", "");
					imgpassdata = gv.IniRead(readini, "game", "imgpass", "");
					passdata = gv.IniRead(readini, "game", "pass", "");
					stimedata = gv.IniRead(readini, "game", "time", "0");
					startdata = gv.IniRead(readini, "game", "start", "0");
					cmtdata = gv.IniRead(readini, "game", "stat", "");
					rating = gv.IniRead(readini, "game", "rating", gv.Rate.ToString());
				}
				else
				{
					//個別ini存在しない場合
					MessageBox.Show("iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。\n\nError: ini_read_error_nofile\nDebug:: gl > listBox1_SelectedIndexChanged > if > else",
									gv.AppName,
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
			}
			else
			{
				// database

				SqlConnection cn = gv.SqlCon;
				SqlCommand cm;

				if (selecteditem.ToString().Length != 0)
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG "
										+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + gv.DbName + "." + gv.DbTable + ") AS T "
										+ "WHERE ROWCNT = " + selecteditem
					};
				}
				else
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG "
										+ "FROM " + gv.DbName + "." + gv.DbTable
					};
				}
				cm.Connection = cn;

				try
				{
					cn.Open();
					var reader = cm.ExecuteReader();

					if (reader.Read())
					{
						id = reader["ID"].ToString();
						namedata = reader["GAME_NAME"].ToString();
						imgpassdata = reader["IMG_PATH"].ToString();
						passdata = reader["GAME_PATH"].ToString();
						stimedata = reader["UPTIME"].ToString();
						startdata = reader["RUN_COUNT"].ToString();
						cmtdata = reader["DCON_TEXT"].ToString();
						rating = reader["AGE_FLG"].ToString();
					}

				}
				catch (Exception ex)
				{
					gv.WriteErrorLog(ex.Message, "listBox1_SelectedIndexChanged", cm.CommandText);
					resolveError("listBox1_SelectedIndexChanged", ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}

			int timedata = Convert.ToInt32(stimedata) / 60;

			label1.Text = namedata;
			textBox1.Text = namedata;
			textBox2.Text = passdata;
			textBox3.Text = imgpassdata;
			textBox4.Text = timedata.ToString();
			textBox5.Text = startdata;
			textBox6.Text = cmtdata;
			gv.CurrentGameDbVal = id;
			toolStripStatusLabel1.Text = "[" + selecteditem + "/" + gv.GameMax + "]";
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
			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				if (File.Exists(gv.GameIni))
				{
					//ini読込開始
					gv.GameMax = Convert.ToInt32(gv.IniRead(gv.GameIni, "list", "game", "0"));
				}
				else
				{
					return;
				}

				LoadItem(gv.GameDir);
				if (listBox1.Items.Count != 0)
				{
					String selectedtext = listBox1.SelectedItem.ToString();
					if (listBox1.Items.Contains(selectedtext))
					{
						listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
					}
				}
			}
			else
			{
				// database
				LoadItem2(gv.SqlCon);
				if (listBox1.Items.Count != 0)
				{
					string selectedtext = listBox1.SelectedItem.ToString();
					if (listBox1.Items.Contains(selectedtext))
					{
						listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
					}
				}
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
										gv.AppName,
										MessageBoxButtons.OK,
										MessageBoxIcon.Information);
			}
		}

		private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
		{
			pictureBox11.Visible = true;
			String selectedtext = listBox1.SelectedItem.ToString();

			LoadItem(gv.GameDir);
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

			updateComponent();
			LoadItem(gv.GameDir);
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
				if (gv.SaveType == "I")
				{
					// ini
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
				else
				{
					// database
					MessageBox.Show("データベースを使用しているため有効にできません。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					checkBox3.Checked = false;
					return;
				}
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
				String rawdata = gv.IniRead(ininame, sec, key, failedval);

				//取得値とデータが違う場合
				if (!(rawdata.Equals(data)))
				{
					if (checkBox3.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = false;
					}

					gv.IniWrite(ininame, sec, key, data);

					if (gv.SaveType == "I")
					{
						if (checkBox3.Checked)
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
						}
					}
				}
			}
			else
			{
				MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n" + ininame, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (checkBox3.Checked)
			{
				if (gv.SaveType == "I")
				{
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
				fileSystemWatcher2.EnableRaisingEvents = true;
			}
			pictureBox11.Visible = false;
			return;
		}

		/// <summary>
		/// コンフィグファイルのロードと画面反映を行います
		/// </summary>
		private void updateComponent()
		{
			if (gv.GLConfigLoad() == false)
			{
				resolveError("updateComponent", "Configロード中にエラー。\n詳しくはエラーログを参照して下さい。", 0, false);
			}

			if (File.Exists(gv.ConfigIni))
			{
				int ckv0 = Convert.ToInt32(gv.IniRead(gv.ConfigIni, "checkbox", "track", "0"));
				int ckv1 = Convert.ToInt32(gv.IniRead(gv.ConfigIni, "checkbox", "winmini", "0"));

				String bgimg = gv.BgImg;
				checkBox4.Checked = Convert.ToBoolean(Convert.ToInt32(gv.IniRead(gv.ConfigIni, "checkbox", "sens", "0")));
				checkBox5.Checked = gv.Dconnect;

				if (gv.Rate == 1)
				{
					radioButton2.Checked = true;
				}
				else
				{
					radioButton1.Checked = true;
				}

				if (Convert.ToInt32(gv.IniRead(gv.ConfigIni, "connect", "ByActive", "0")) == 1)
				{
					gv.ByActive = true;
					if (gv.ByRoW)
					{
						bouyomi_configload();
					}
				}
				else
				{
					gv.ByActive = false;
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

				fileSystemWatcher2.Path = gv.BaseDir;

				// データベース使用時の部品非表示処理
				if (gv.SaveType == "D")
				{
					// databaseの場合
					button6.Visible = false;
					button10.Visible = false;
					checkBox3.Checked = false;
					checkBox3.Visible = false;
					button9.Visible = false;
					toolStripStatusLabel3.Visible = false;
				}
				else
				{
					// iniの場合
					tabControl1.Controls.Remove(tabPage3);
				}
			}
			else
			{
				//ini存在しない場合
				File.Create(gv.ConfigIni).Close();
				gv.GameDir = gv.BaseDir + "Data";
				gv.GameIni = gv.GameDir + "\\game.ini";
				checkBox1.Checked = true;
				checkBox2.Checked = true;
				fileSystemWatcher2.Path = gv.BaseDir;
				tabControl1.TabPages.Remove(tabPage3);
				checkBox1.Checked = true;
				checkBox2.Checked = true;
			}
			return;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int rawdata = gv.GameMax;

			if (rawdata >= 1)
			{
				//乱数生成
				System.Random r = new System.Random();
				int ans = r.Next(1, rawdata + 1);

				listBox1.SelectedIndex = ans - 1;

				//グリッドと同期
				listView1.Items[listBox1.SelectedIndex].Selected = true;
				listView1.EnsureVisible(listBox1.SelectedIndex);
			}
			else
			{
				MessageBox.Show("登録ゲーム数が少ないため、ランダム選択できません！", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
			ExitApp();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//選択中のゲーム保管ファイルを削除
			pictureBox11.Visible = true;
			int delval = listBox1.SelectedIndex + 1;
			String delname = textBox1.Text;
			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				delIniItem(delval, delname);

				// 次回DB接続時に更新するフラグを立てる
				if (gv.SaveType == "T")
				{
					gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
				}
			}
			else
			{
				// database
				delDbItem(delval);
			}

			pictureBox11.Visible = false;
			return;
		}

		private void button9_Click(object sender, EventArgs e)
		{
			if (listBox1.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (gv.SaveType == "D")
			{
				// データベース使用中は現段階では変更不可（※将来対応予定）
				return;
			}
			String selectedtext = listBox1.SelectedItem.ToString();

			// Discordカスタムステータス、各種チェックボックス、ラジオの保存
			String pass = gv.GameDir + "\\" + (listBox1.SelectedIndex + 1) + ".ini";
			if (File.Exists(pass))
			{
				iniedtchk(pass, "game", "stat", (textBox6.Text), "");
				iniedtchk(gv.ConfigIni, "checkbox", "track", (Convert.ToInt32(checkBox1.Checked)).ToString(), "0");
				iniedtchk(gv.ConfigIni, "checkbox", "winmini", (Convert.ToInt32(checkBox2.Checked)).ToString(), "0");
				iniedtchk(gv.ConfigIni, "checkbox", "sens", (Convert.ToInt32(checkBox4.Checked)).ToString(), "0");
				if (radioButton1.Checked)
				{
					iniedtchk(gv.ConfigIni, "checkbox", "rate", "0", "-1");
				}
				else
				{
					iniedtchk(gv.ConfigIni, "checkbox", "rate", "1", "-1");
				}

				LoadItem(gv.GameDir);

				// 更新前に選択していたゲームへ移動
				if (listBox1.Items.Contains(selectedtext))
				{
					listBox1.SelectedIndex = listBox1.Items.IndexOf(selectedtext);
				}
			}
			else
			{
				// 個別ini不存在
				MessageBox.Show("ゲーム情報保管iniが存在しません。\n" + pass, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return;
		}

		private void toolStripStatusLabel3_Click(object sender, EventArgs e)
		{
			if (gv.SaveType == "D")
			{
				// 現段階ではDBに登録されている情報の変更は不可（※将来対応予定）
				return;
			}

			// ini読込
			String rawdata = gv.GameDir + "\\" + ((listBox1.SelectedIndex + 1).ToString()) + ".ini";

			if (File.Exists(rawdata))
			{
				System.Diagnostics.Process.Start(gv.GameDir + "\\" + (listBox1.SelectedIndex + 1) + ".ini");
			}
			else
			{
				MessageBox.Show("ゲーム情報保管iniがありません！\n" + rawdata, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				int selected = listBox1.SelectedIndex;
				if (selected >= 1)
				{
					selected++;
					fileSystemWatcher1.EnableRaisingEvents = false;
					int target = selected - 1;
					String before = gv.GameDir + "\\" + (selected.ToString()) + ".ini";
					String temp = gv.GameDir + "\\" + (target.ToString()) + "_.ini";
					String after = gv.GameDir + "\\" + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (gv.SaveType == "T")
						{
							gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						MessageBox.Show("移動先もしくは移動前のファイルが見つかりません。\n\n移動前：" + before + "\n移動先：" + after, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					if (checkBox3.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
				}
				else
				{
					MessageBox.Show("最上位です。\nこれ以上は上に移動できません。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(gv.GameDir);
				listBox1.SelectedIndex = selected - 2;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
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
			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// ini
				int selected = listBox1.SelectedIndex;
				if (selected + 1 < gv.GameMax)
				{
					selected++;
					fileSystemWatcher1.EnableRaisingEvents = false;
					int target = selected + 1;
					String before = gv.GameDir + "\\" + (selected.ToString()) + ".ini";
					String temp = gv.GameDir + "\\" + (target.ToString()) + "_.ini";
					String after = gv.GameDir + "\\" + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (gv.SaveType == "T")
						{
							gv.IniWrite(gv.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						MessageBox.Show("移動先もしくは移動前のファイルが見つかりません。\nファイルに影響はありません。\n\n移動前：" + before + "\n移動先：" + after, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
						if (checkBox3.Checked)
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
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
					MessageBox.Show("最下位です。\nこれ以上は下に移動できません。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(gv.GameDir);
				listBox1.SelectedIndex = selected--;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return;
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(gv.GameMax >= 1))
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

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			switch (trackBar1.Value)
			{
				case 0:

					listView1.LargeImageList = imageList0;
					break;
				case 1:
					listView1.LargeImageList = imageList1;
					break;
				case 2:
					listView1.LargeImageList = imageList2;
					break;
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			//設定
			fileSystemWatcher1.EnableRaisingEvents = false;
			fileSystemWatcher2.EnableRaisingEvents = false;
			String beforeWorkDir = gv.BaseDir;
			String beforeSaveType = gv.SaveType;
			form2.ShowDialog();
			updateComponent();
			String afterWorkDir = gv.BaseDir;
			String aftereSaveType = gv.SaveType;
			if (beforeWorkDir != afterWorkDir)
			{
				MessageBox.Show("既定の作業ディレクトリが変更されました。\nGame Launcherを再起動してください。\n\nOKを押してGame Launcherを終了します。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
			}
			else if (beforeSaveType != aftereSaveType)
			{
				MessageBox.Show("データの保存方法が変更されました。\nGame Launcherを再起動してください。\n\nOKを押してGame Launcherを終了します。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
			}

			if (checkBox3.Checked)
			{
				fileSystemWatcher2.Path = gv.BaseDir;
				fileSystemWatcher2.EnableRaisingEvents = true;
				if (gv.SaveType == "I")
				{
					fileSystemWatcher1.Path = gv.GameDir;
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
			}

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

		private void delIniItem(int delfileval, String delfilename)
		{
			fileSystemWatcher2.EnableRaisingEvents = false;
			fileSystemWatcher1.EnableRaisingEvents = false;

			int nextval;
			String nextfile;
			int delval = delfileval;
			String delfile = (gv.GameDir + "\\" + delval + ".ini");
			if (File.Exists(delfile))
			{
				//削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("選択中のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delfilename + "]\n" + delfile + "\n削除しますか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					File.Delete(delfile);
					nextval = delval + 1;
					nextfile = (gv.GameDir + "\\" + nextval + ".ini");
					while (File.Exists(nextfile))
					{
						//削除ファイル以降にゲームが存在する場合に番号を下げる
						File.Move(nextfile, delfile);
						delfile = nextfile;
						nextval++;
						nextfile = (gv.GameDir + "\\" + nextval + ".ini");
					}
					gv.GameMax--;
					gv.IniWrite(gv.GameIni, "list", "game", gv.GameMax.ToString());
				}
				else if (dialogResult == DialogResult.No)
				{
					if (checkBox3.Checked)
					{
						fileSystemWatcher2.EnableRaisingEvents = true;
						if (gv.SaveType == "I")
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
						}
					}
					return;
				}
				else
				{
					MessageBox.Show("不明な結果です。\n実行を中断します。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				//削除ファイル不存在
				MessageBox.Show("該当するiniファイルが存在しません。\n" + delfile, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			LoadItem(gv.GameDir);
			if (gv.GameMax >= 2 && delfileval >= 2)
			{
				listBox1.SelectedIndex = delfileval - 2;
			}
			else if (gv.GameMax == 1 && delfileval >= 1)
			{
				listBox1.SelectedIndex = 1;
			}
			else
			{
				listBox1.SelectedIndex = 0;
			}

			if (checkBox3.Checked)
			{
				fileSystemWatcher2.EnableRaisingEvents = true;
				if (gv.SaveType == "I")
				{
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
			}
			return;
		}

		private void delDbItem(int delItemVal)
		{

			fileSystemWatcher2.EnableRaisingEvents = false;
			fileSystemWatcher1.EnableRaisingEvents = false;

			string delName = string.Empty;
			string delPath = string.Empty;

			SqlConnection cn = gv.SqlCon;
			SqlCommand cm;

			cm = new SqlCommand()
			{
				CommandType = CommandType.Text,
				CommandTimeout = 30,
				CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG "
								+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + gv.DbName + "." + gv.DbTable + ") AS T "
								+ "WHERE ROWCNT = " + delItemVal
			};
			cm.Connection = cn;

			try
			{
				cn.Open();
				var reader = cm.ExecuteReader();

				if (reader.Read())
				{
					delName = reader["GAME_NAME"].ToString();
					delPath = reader["GAME_PATH"].ToString();
				}
			}
			catch (Exception ex)
			{
				gv.WriteErrorLog(ex.Message, "delDbItem", cm.CommandText);
				resolveError("delDbItem", ex.Message, 0, false);
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			//削除ファイル存在
			DialogResult dialogResult = MessageBox.Show("次のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delName + "]\n" + delPath + "\n削除しますか？", gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			if (dialogResult == DialogResult.Yes)
			{
				// 削除
				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"DELETE " + gv.DbName + "." + gv.DbTable + " "
									+ "FROM " + gv.DbName + "." + gv.DbTable + " glt "
									+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + gv.DbName + "." + gv.DbTable + ") tmp "
									+ "ON glt.ID = tmp.ID "
									+ "WHERE ROWCNT = " + delItemVal
				};
				cm.Connection = cn;
				try
				{
					cn.Open();
					cm.ExecuteNonQuery();
					gv.GameMax--;
				}
				catch (Exception ex)
				{
					gv.WriteErrorLog(ex.Message, "delDbItem", cm.CommandText);
					resolveError("delDbItem", ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}
			else if (dialogResult == DialogResult.No)
			{
				return;
			}
			else
			{
				MessageBox.Show("不明な結果です。\n実行を中断します。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			LoadItem2(gv.SqlCon);

			if (gv.GameMax >= 2 && delItemVal >= 2)
			{
				listBox1.SelectedIndex = delItemVal - 2;
			}
			else if (gv.GameMax == 1 && delItemVal >= 1)
			{
				listBox1.SelectedIndex = 1;
			}
			else
			{
				listBox1.SelectedIndex = 0;
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
				MessageBox.Show("既にdcon.jarが終了しています。\n起動時間が正常に記録されない可能性があります。", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void bouyomi_configload()
		{
			gv.ByHost = gv.IniRead(gv.ConfigIni, "connect", "gv.ByHost", "127.0.0.1");
			gv.ByPort = Convert.ToInt32(gv.IniRead(gv.ConfigIni, "connect", "gv.ByPort", "50001"));
			gv.Bouyomi_Connectchk(gv.ByHost, gv.ByPort, gv.ByType, false);
			return;
		}


		/// <summary>
		/// エラーダイアログを表示します。
		/// </summary>
		/// <param name="methodName">関数名</param>
		/// <param name="errorMsg">エラーメッセージ</param>
		/// <param name="dialogType">0：OK、4：Yes/No、5：再試行、キャンセル</param>
		public DialogResult resolveError(string methodName, string errorMsg, int dialogType, bool forceClose = true)
		{
			DialogResult dr = new DialogResult();
			switch (dialogType)
			{
				case 0:
				default:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case 4:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, gv.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
					break;

				case 5:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, gv.AppName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
					break;
			}

			if (forceClose)
			{
				ExitApp();
			}

			return dr;
		}

		public void ExitApp()
		{
			if (gv.SaveType == "D" && gv.OfflineSave)
			{
				string localPath = gv.BaseDir + (gv.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
				gv.downloadDbDataToLocal(localPath);
			}

			if (gv.ByRoW)
			{
				gv.Bouyomiage("ゲームランチャーを終了しました");
			}

			Application.Exit();
		}

		private void button11_Click(object sender, EventArgs e)
		{
			long beforeMemory = Environment.WorkingSet;
			GC.Collect();
			long afterMemory = Environment.WorkingSet;
			long diffMemory = beforeMemory - afterMemory;
			MessageBox.Show("メモリを解放しました。\n" + beforeMemory + "byte -> " + afterMemory + "byte (" + diffMemory + "byte)", gv.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void button12_Click(object sender, EventArgs e)
		{
			if (gv.GridMax)
			{
				// 現在最大表示の場合、最小表示にする
				tabControl1.Width = 345;

				listBox1.Width = 330;

				listView1.Width = 330;
				trackBar1.Width = 330;

				textBox7.Width = 265;
				button13.Location = new Point(274, 8);
				textBox8.Width = 330;
				listBox2.Width = 330;

				button12.Text = ">";
				gv.GridMax = false;
			}
			else
			{
				// 最小表示の場合、最大表示にする
				tabControl1.Width = 840;

				listBox1.Width = 825;

				listView1.Width = 825;
				trackBar1.Width = 825;

				textBox7.Width = 760;
				button13.Location = new Point(766, 8);
				textBox8.Width = 825;
				listBox2.Width = 825;

				button12.Text = "<";
				gv.GridMax = true;
			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			if (File.Exists(pictureBox1.ImageLocation))
			{
				Form4 form4 = new Form4(pictureBox1.ImageLocation);
				form4.ShowDialog();
			}
		}

		/// <summary>
		/// 検索ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button13_Click(object sender, EventArgs e)
		{
			if (gv.GameMax <= 0)
			{
				return;
			}

			string searchName = textBox7.Text.Trim();

			// 検索条件表示
			textBox8.Text = searchName;

			// 検索処理
			listBox2.Items.Clear();

			SqlConnection cn = gv.SqlCon;

			try
			{
				// 接続オープン
				cn.Open();

				//検索に一致するゲーム数取得
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + gv.DbName + "." + gv.DbTable
									+ " WHERE GAME_NAME LIKE '%" + searchName + "%'"
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns <= 0)
				{
					//ゲームが1つもない場合
					return;
				}

				// DBからデータを取り出す
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG FROM " + gv.DbName + "." + gv.DbTable
									+ " WHERE GAME_NAME LIKE '%" + searchName + "%'"
				};
				cm2.Connection = cn;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						listBox2.Items.Add(reader["GAME_NAME"].ToString());
					}
				}
			}
			catch (Exception ex)
			{
				gv.WriteErrorLog(ex.Message, "button13_Click", cn.ConnectionString);
				resolveError("button13_Click", ex.Message, 0, false);
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			Application.DoEvents();
			return;
		}

		/// <summary>
		/// 検索リストボックス
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
		{
			int rowCount = -1;

			// リストボックスにアイテムがなければリターン
			if (!(listBox2.Items.Count >= 1))
			{
				return;
			}

			//ゲーム詳細取得
			int selecteditem = listBox2.SelectedIndex + 1;

			if (gv.SaveType == "I" || gv.SaveType == "T")
			{
				// iniでは検索は実行できないのでリターン
				return;
			}
			else
			{
				// 選択されたアイテムの検索を行う
				SqlConnection cn = gv.SqlCon;
				SqlCommand cm;

				if (selecteditem.ToString().Length != 0)
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT T.ID, T.GAME_NAME, T.GAME_PATH, T.IMG_PATH, T.UPTIME, T.RUN_COUNT, T.DCON_TEXT, T.AGE_FLG, T.ROW_CNT "
										+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + gv.DbName + "." + gv.DbTable + ") AS T "
										+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + gv.DbName + "." + gv.DbTable + " WHERE GAME_NAME LIKE '%" + textBox8.Text + "%') AS T2 "
										+ " ON T.ID = T2.ID "
										+ "WHERE T2.ROW_CNT = " + selecteditem
					};
				}
				else
				{
					// アイテムが選択されていなければ検索を行う必要はないのでリターン
					return;
				}
				cm.Connection = cn;

				// 検索と反映
				try
				{
					cn.Open();
					var reader = cm.ExecuteReader();

					if (reader.Read())
					{
						// 各項目にデータ反映
						rowCount = Convert.ToInt32(reader["ROW_CNT"]);
					}

				}
				catch (Exception ex)
				{
					gv.WriteErrorLog(ex.Message, "listBox2_SelectedIndexChanged", cm.CommandText);
					resolveError("listBox2_SelectedIndexChanged", ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}

			if (rowCount == -1)
			{
				// rowCountが-1の場合は値の取得に失敗しているのでリターン
				return;
			}

			// rowCountをリスト値に変換
			rowCount--;

			// 他のグリッドと同期
			listView1.Items[rowCount].Selected = true;
			listView1.EnsureVisible(rowCount);

			return;
		}
	}
}
