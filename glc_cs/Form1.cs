using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
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
		Form4 form4 = new Form4(null);

		public gl()
		{
			InitializeComponent();
		}

		private void gl_Load(object sender, EventArgs e)
		{
			// スプラッシュ画面表示
			form3.Show();
			Application.DoEvents();
			this.Opacity = 0;
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

			// 初期設定
			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
			// 実行ボタンカバーを表示
			pictureBox11.Visible = true;

			updateComponent();

			string item = General.Var.SaveType == "I" ? LoadItem(General.Var.GameDir) : General.Var.SaveType == "D" ? LoadItem2(General.Var.SqlCon, true) : LoadItem3(General.Var.SqlCon2, true);
			tabControl1.SelectedIndex = 1;
			tabControl1.SelectedIndex = 0;

			// 検索タブ
			searchTargetDropDown.SelectedIndex = 0;
			orderDropDown.SelectedIndex = 0;

			// 実行ボタンカバーを非表示
			pictureBox11.Visible = false;
			this.Show();
			this.Refresh();
			form3.Close();
			form3.Dispose();
			Application.DoEvents();
			this.Opacity = 1;

			if (item == "_none_game_data" || item == "0")
			{

				if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
				{
					// ini
					if (!(File.Exists(General.Var.GameIni)))
					{
						General.Var.IniWrite(General.Var.GameIni, "list", "game", "0");
					}


					if (Directory.Exists(General.Var.GameDir))
					{
						fileSystemWatcher1.Path = General.Var.GameDir;
					}
					else
					{
						try
						{
							Directory.CreateDirectory(General.Var.GameDir);
							fileSystemWatcher1.Path = General.Var.GameDir;
						}
						catch (Exception ex)
						{
							resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0);
						}
					}
				}
				// itemがnoneの場合：ゲームが登録されていない場合
				MessageBox.Show("Game Launcherをご利用頂きありがとうございます。\n\"追加\"ボタンを押して、ゲームを追加しましょう！",
								General.Var.AppName,
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
			gameList.Items.Clear();
			gameImgList.Items.Clear();
			imageList0.Images.Clear();
			imageList1.Images.Clear();
			imageList2.Images.Clear();

			// 全ゲーム数取得
			if (File.Exists(General.Var.GameIni))
			{
				General.Var.GameMax = Convert.ToInt32(General.Var.IniRead(General.Var.GameIni, "list", "game", "0"));
				reloadButton.Enabled = true;
				editButton.Enabled = true;
				randomButton.Enabled = true;
				delButton.Enabled = true;
			}
			else
			{
				// ゲームが1つもない場合
				reloadButton.Enabled = false;
				editButton.Enabled = false;
				randomButton.Enabled = false;
				delButton.Enabled = false;
				return "_none_game_data";
			}

			if (!(General.Var.GameMax >= 1)) // ゲーム登録数が1以上でない場合
			{
				reloadButton.Enabled = false;
			}


			int count;
			String readini;
			String ans = "";
			Image lvimg;
			ListViewItem lvi = new ListViewItem();

			toolStripProgressBar1.Minimum = 0;
			toolStripProgressBar1.Maximum = General.Var.GameMax;

			for (count = 1; count <= General.Var.GameMax; count++)
			{
				toolStripProgressBar1.Value = count;
				// 読込iniファイル名更新
				readini = gameDirname + "\\" + count + ".ini";

				if (File.Exists(readini))
				{
					gameList.Items.Add(General.Var.IniRead(readini, "game", "name", ""));

					try
					{
						lvimg = Image.FromFile(General.Var.IniRead(readini, "game", "imgpass", ""));
					}
					catch
					{
						lvimg = Properties.Resources.exe;
					}

					imageList0.Images.Add(count.ToString(), lvimg);
					imageList1.Images.Add(count.ToString(), lvimg);
					imageList2.Images.Add(count.ToString(), lvimg);

					lvi = new ListViewItem(General.Var.IniRead(readini, "game", "name", ""));
					lvi.ImageIndex = (count - 1);
					gameImgList.Items.Add(lvi);
				}
				else
				{
					// 個別ini存在しない場合
					resolveError(MethodBase.GetCurrentMethod().Name, "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。", 0, false);
					this.ResumeLayout();
					break;
				}
			}

			// ゲームが登録されていれば1つ目を選択した状態にする
			if (General.Var.GameMax >= 1)
			{
				gameList.SelectedIndex = 0;
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

			gameList.Items.Clear();
			searchResultList.Items.Clear();
			gameImgList.Items.Clear();
			imageList0.Images.Clear();
			imageList1.Images.Clear();
			imageList2.Images.Clear();

			searchText.Text = string.Empty;
			searchingText.Text = string.Empty;

			try
			{
				// 読み込み処理
				cn.Open();

				// 全ゲーム数取得
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + General.Var.DbName + "." + General.Var.DbTable
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					General.Var.GameMax = sqlAns;
					reloadButton.Enabled = true;
					editButton.Enabled = true;
					randomButton.Enabled = true;
					delButton.Enabled = true;
				}
				else
				{
					//ゲームが1つもない場合
					General.Var.GameMax = 0;
					reloadButton.Enabled = false;
					editButton.Enabled = false;
					randomButton.Enabled = false;
					delButton.Enabled = false;
					return "_none_game_data";
				}

				if (!(General.Var.GameMax >= 1)) // ゲーム登録数が1以上でない場合
				{
					reloadButton.Enabled = false;
				}

				Image lvimg;
				ListViewItem lvi = new ListViewItem();

				toolStripProgressBar1.Minimum = 0;
				toolStripProgressBar1.Maximum = General.Var.GameMax;

				toolStripProgressBar1.Value = 0;

				// DBからデータを取り出す
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, IMG_PATH, ROW_CNT "
								+ " FROM ( SELECT ID, GAME_NAME, IMG_PATH, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + General.Var.DbName + "." + General.Var.DbTable + ") AS T "
				};
				cm2.Connection = cn;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						gameList.Items.Add(reader["GAME_NAME"].ToString());

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
						gameImgList.Items.Add(lvi);
					}
				}

				// ゲームが登録されていれば1つ目を選択した状態にする
				if (General.Var.GameMax >= 1)
				{
					gameList.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
				errMessage = ex.Message;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			string localPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
			string localGameIni = localPath + "game.ini";
			string backupDir = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";

			if (errMessage.Length != 0)
			{
				// エラーがあった場合
				if (General.Var.OfflineSave && General.Var.SaveType == "D")
				{
					if (File.Exists(localGameIni))
					{
						// オフラインモード使用可能の場合
						tabControl1.Controls.Remove(tabPage3);
						General.Var.SaveType = "T";
						General.Var.GameDir = localPath;
						General.Var.GameIni = localGameIni;
						ans = LoadItem(localPath);
						if (firstLoad)
						{
							toolStripStatusLabel3.Visible = true;
							upButton.Visible = true;
							downButton.Visible = true;
							editButton.Visible = true;
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
					if (General.Var.IniRead(localGameIni, "list", "dbupdate", "0") == "1")
					{
						DialogResult dr = MessageBox.Show("オフラインモード実行時に変更がありました。\nデータベースへアップロードしますか？\n\n接続先：" + General.Var.DbUrl + " ▶ " + General.Var.DbName + "." + General.Var.DbTable + "\n\n※データベースのレコードを全削除し、オフラインのデータを登録します。\n\n[はい]	登録\n[いいえ]	破棄", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dr == DialogResult.Yes)
						{
							int tmpMaxGameCount, sCount, fCount;
							// データベース登録
							int returnVal = General.Var.InsertIni2Db(localPath, backupDir, out tmpMaxGameCount, out sCount, out fCount);
							if (returnVal == 0)
							{
								MessageBox.Show("処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)\n\n", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
						else
						{
							// 「いいえ」選択時、アップロードフラグを0にする
							General.Var.IniWrite(localGameIni, "list", "dbupdate", "0");
						}
					}
				}
			}
			Application.DoEvents();
			return ans;
		}

		/// <summary>
		/// データベースからゲーム一覧をロードします（MySQL）
		/// </summary>
		/// <param name="cn">MySQL Connection</param>
		/// <returns></returns>
		private string LoadItem3(MySqlConnection cn, bool firstLoad = false)
		{
			string ans = string.Empty;
			string errMessage = string.Empty;

			gameList.Items.Clear();
			searchResultList.Items.Clear();
			gameImgList.Items.Clear();
			imageList0.Images.Clear();
			imageList1.Images.Clear();
			imageList2.Images.Clear();

			searchText.Text = string.Empty;
			searchingText.Text = string.Empty;

			try
			{
				// 読み込み処理
				cn.Open();

				// 全ゲーム数取得
				MySqlCommand cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + General.Var.DbTable
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					General.Var.GameMax = sqlAns;
					reloadButton.Enabled = true;
					editButton.Enabled = true;
					randomButton.Enabled = true;
					delButton.Enabled = true;
				}
				else
				{
					//ゲームが1つもない場合
					General.Var.GameMax = 0;
					reloadButton.Enabled = false;
					editButton.Enabled = false;
					randomButton.Enabled = false;
					delButton.Enabled = false;
					return "_none_game_data";
				}

				if (!(General.Var.GameMax >= 1)) // ゲーム登録数が1以上でない場合
				{
					reloadButton.Enabled = false;
				}

				Image lvimg;
				ListViewItem lvi = new ListViewItem();

				toolStripProgressBar1.Minimum = 0;
				toolStripProgressBar1.Maximum = General.Var.GameMax;

				toolStripProgressBar1.Value = 0;

				// DBからデータを取り出す
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, IMG_PATH, ROW_CNT "
								+ " FROM ( SELECT ID, GAME_NAME, IMG_PATH, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + General.Var.DbTable + ") AS T "
				};
				cm2.Connection = cn;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						gameList.Items.Add(reader["GAME_NAME"].ToString());

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
						gameImgList.Items.Add(lvi);
					}
				}

				// ゲームが登録されていれば1つ目を選択した状態にする
				if (General.Var.GameMax >= 1)
				{
					gameList.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
				errMessage = ex.Message;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			string localPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
			string localGameIni = localPath + "game.ini";
			string backupDir = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";

			if (errMessage.Length != 0)
			{
				// エラーがあった場合
				if (General.Var.OfflineSave && General.Var.SaveType == "M")
				{
					if (File.Exists(localGameIni))
					{
						// オフラインモード使用可能の場合
						tabControl1.Controls.Remove(tabPage3);
						General.Var.SaveType = "T";
						General.Var.GameDir = localPath;
						General.Var.GameIni = localGameIni;
						ans = LoadItem(localPath);
						if (firstLoad)
						{
							toolStripStatusLabel3.Visible = true;
							upButton.Visible = true;
							downButton.Visible = true;
							editButton.Visible = true;
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
					if (General.Var.IniRead(localGameIni, "list", "dbupdate", "0") == "1")
					{
						DialogResult dr = MessageBox.Show("オフラインモード実行時に変更がありました。\nデータベースへアップロードしますか？\n\n接続先：" + General.Var.DbUrl + ":" + General.Var.DbPort + " ▶ " + General.Var.DbName + "." + General.Var.DbTable + "\n\n※データベースのレコードを全削除し、オフラインのデータを登録します。\n\n[はい]	登録\n[いいえ]	破棄", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dr == DialogResult.Yes)
						{
							int tmpMaxGameCount, sCount, fCount;
							// データベース登録
							int returnVal = General.Var.InsertIni2Db(localPath, backupDir, out tmpMaxGameCount, out sCount, out fCount);
							if (returnVal == 0)
							{
								MessageBox.Show("処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)\n\n", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
						else
						{
							// 「いいえ」選択時、アップロードフラグを0にする
							General.Var.IniWrite(localGameIni, "list", "dbupdate", "0");
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

			if (gameList.SelectedIndex == -1)
			{
				resolveError(MethodBase.GetCurrentMethod().Name, "ゲームリストが空です。", 0, false);
				return;
			}
			String selectedtext = gameList.SelectedItem.ToString();

			// 共通部分を保存
			iniedtchk(General.Var.ConfigIni, "checkbox", "track", (Convert.ToInt32(trackCheck.Checked)).ToString(), "0");
			iniedtchk(General.Var.ConfigIni, "checkbox", "winmini", (Convert.ToInt32(minCheck.Checked)).ToString(), "0");
			iniedtchk(General.Var.ConfigIni, "checkbox", "sens", (Convert.ToInt32(sensCheck.Checked)).ToString(), "0");

			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// iniの場合、ステータス状態を書き込み
				// Discordカスタムステータス、各種チェックボックス、ラジオの保存
				String path = General.Var.GameDir + "\\" + (gameList.SelectedIndex + 1) + ".ini";
				if (File.Exists(path))
				{
					iniedtchk(path, "game", "stat", (dconText.Text.Trim()), string.Empty);
					iniedtchk(path, "game", "temp1", (dconImgText.Text.Trim()), string.Empty);
					if (normalRadio.Checked)
					{
						iniedtchk(path, "game", "rating", "0", "0");
					}
					else
					{
						iniedtchk(path, "game", "rating", "1", "0");
					}

					// 更新前に選択していたゲームへ移動
					if (gameList.Items.Contains(selectedtext))
					{
						gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
					}
				}
				else
				{
					// 個別ini不存在
					resolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報保管iniが存在しません。\n" + path, 0, false);
				}
			}

			if (File.Exists(exePathText.Text))
			{
				if (!testCheck.Checked)
				{
					if (trackCheck.Checked)
					{
						if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
						{
							iniedtchk(General.Var.GameDir + "\\" + (gameList.SelectedIndex + 1).ToString() + ".ini", "game", "stat", dconText.Text, string.Empty);
							iniedtchk(General.Var.GameDir + "\\" + (gameList.SelectedIndex + 1).ToString() + ".ini", "game", "temp1", dconImgText.Text, string.Empty);
						}

						// 現在時刻取得
						string strTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime starttime = Convert.ToDateTime(strTime);

						// 実行
						System.Diagnostics.Process drunp = null;
						if (useDconCheck.Checked)
						{
							if (File.Exists(General.Var.DconPath))
							{
								// propertiesファイル書き込み
								String propertiesfile = System.IO.Path.GetDirectoryName(General.Var.DconPath) + "\\run.properties";
								Encoding enc = Encoding.GetEncoding("Shift-JIS");
								StreamWriter writer = new StreamWriter(propertiesfile, false, enc);

								if (normalRadio.Checked)
								{
									ratinginfo = 0;
								}
								else
								{
									ratinginfo = 1;
								}

								if (sensCheck.Checked)
								{
									// センシティブモード有効
									writer.WriteLine("title = " + "Unknown" + "\nrating = " + ratinginfo + "\nstat = " + dconText.Text.Trim());
								}
								else
								{
									writer.WriteLine("title = " + nameText.Text + "\nappid = " + (General.Var.DconAppID.Length != 0 ? General.Var.DconAppID : string.Empty) + "\nappicon = " + dconImgText.Text.Trim() + "\nrating = " + ratinginfo + "\nstat = " + dconText.Text.Trim());
								}

								writer.Close();

								drunp = System.Diagnostics.Process.Start(General.Var.DconPath); // dcon実行
							}
							else
							{
								resolveError(MethodBase.GetCurrentMethod().Name, "Discord Connectorが見つかりません。\n実行を中断します。", 0, false);
								return;
							}
						}

						// ウィンドウ最小化
						if (minCheck.Checked == true)
						{
							this.WindowState = FormWindowState.Minimized;
							this.notifyIcon1.Visible = true;
						}

						// 既定ディレクトリの変更
						String apppath = System.IO.Path.GetDirectoryName(exePathText.Text);
						System.Environment.CurrentDirectory = apppath;

						// 起動中gifの可視化
						pictureBox11.Visible = true;
						startButton.Enabled = false;

						// 自動更新有効時のファイルウォッチャー無効化
						fileSystemWatcher1.EnableRaisingEvents = false;

						// ゲーム実行
						System.Diagnostics.Process p =
						System.Diagnostics.Process.Start(exePathText.Text);

						// 棒読み上げ
						if (General.Var.ByRoS)
						{
							General.Var.Bouyomiage(nameText.Text + "を、トラッキングありで起動しました。");
						}

						// ゲーム終了まで待機
						p.WaitForExit();

						// ゲーム終了
						System.Environment.CurrentDirectory = General.Var.BaseDir;

						if (minCheck.Checked == true)
						{
							this.WindowState = FormWindowState.Normal;
							this.notifyIcon1.Visible = false;
						}

						// 子プロセスの終了
						if (useDconCheck.Checked)
						{
							KillChildProcess(drunp);
						}

						// 終了時刻取得
						String time = p.ExitTime.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime endtime = Convert.ToDateTime(time);

						// 起動時間計算
						String temp = (endtime - starttime).ToString();
						int anss = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

						if (General.Var.ByRoS)
						{
							General.Var.Bouyomiage("ゲームを終了しました。起動時間は、約" + anss + "秒です。");
						}

						if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
						{
							// ini
							int selecteditem = gameList.SelectedIndex + 1;
							String readini = General.Var.GameDir + "\\" + selecteditem + ".ini";

							if (File.Exists(readini))
							{
								//既存値の取得
								timedata = Convert.ToInt32(General.Var.IniRead(readini, "game", "time", "0"));
								startdata = Convert.ToInt32(General.Var.IniRead(readini, "game", "start", "0"));

								//計算
								if ((timedata + anss) >= Int32.MaxValue)
								{
									timedata = Int32.MaxValue;
								}
								else
								{
									timedata += anss;
								}
								startdata++;

								//書き換え
								General.Var.IniWrite(readini, "game", "time", timedata.ToString());
								General.Var.IniWrite(readini, "game", "start", startdata.ToString());

								// 次回DB接続時に更新するフラグを立てる
								if (General.Var.SaveType == "T")
								{
									General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
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
							if (General.Var.SaveType == "D")
							{
								// SQL文構築
								SqlConnection cn = General.Var.SqlCon;
								SqlTransaction tr = null;
								SqlCommand cm = new SqlCommand
								{
									CommandType = CommandType.Text,
									CommandTimeout = 30,
									CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET UPTIME = CAST(CAST(UPTIME AS BIGINT) + " + anss + " AS NVARCHAR), RUN_COUNT = CAST(CAST(RUN_COUNT AS INT) + 1 AS NVARCHAR), DCON_TEXT = '" + dconText.Text.Trim() + "', TEMP1 = '" + dconImgText.Text.Trim() + "', AGE_FLG = '" + (normalRadio.Checked ? "0" : "1") + "', LAST_RUN = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' "
												+ " WHERE ID = '" + General.Var.CurrentGameDbVal + "'"
								};
								cm.Connection = cn;

								// SQL実行
								try
								{
									cn.Open();
									tr = cn.BeginTransaction();

									cm.Transaction = tr;
									cm.ExecuteNonQuery();

									tr.Commit();
								}
								catch (Exception ex)
								{
									if (tr != null)
									{
										tr.Rollback();
									}
									General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
									resolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。", 0, false);
								}
								finally
								{
									if (cn.State == ConnectionState.Open)
									{
										cn.Close();
									}
								}
							}
							else if (General.Var.SaveType == "M")
							{
								// SQL文構築
								MySqlConnection cn = General.Var.SqlCon2;
								MySqlTransaction tr = null;
								MySqlCommand cm = new MySqlCommand
								{
									CommandType = CommandType.Text,
									CommandTimeout = 30,
									CommandText = @"UPDATE " + General.Var.DbTable + " SET UPTIME = CAST(CAST(UPTIME AS SIGNED) + " + anss + " AS NCHAR), RUN_COUNT = CAST(CAST(RUN_COUNT AS SIGNED) + 1 AS NCHAR), DCON_TEXT = '" + dconText.Text.Trim() + "', TEMP1 = '" + dconImgText.Text.Trim() + "', AGE_FLG = '" + (normalRadio.Checked ? "0" : "1") + "', LAST_RUN = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' "
												+ " WHERE ID = '" + General.Var.CurrentGameDbVal + "'"
								};
								cm.Connection = cn;

								// SQL実行
								try
								{
									cn.Open();
									tr = cn.BeginTransaction();

									cm.Transaction = tr;
									cm.ExecuteNonQuery();

									tr.Commit();
								}
								catch (Exception ex)
								{
									if (tr != null)
									{
										tr.Rollback();
									}
									General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
									resolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。", 0, false);
								}
								finally
								{
									if (cn.State == ConnectionState.Open)
									{
										cn.Close();
									}
								}
							}


							listBox1_SelectedIndexChanged(null, null);
							if (General.Var.SaveType == "I" && reloadCheck.Checked)
							{
								fileSystemWatcher1.EnableRaisingEvents = true;
							}
						}
						pictureBox11.Visible = false;
						startButton.Enabled = true;

						if (General.Var.SaveType == "D" || General.Var.SaveType == "M")
						{
							if (searchingText.Text.Trim().Length != 0 && searchResultList.Items.Count > 0)
							{
								// 再検索
								searchExec(true);
							}
						}
					}
					else
					{
						String apppath = System.IO.Path.GetDirectoryName(exePathText.Text);
						System.Environment.CurrentDirectory = apppath;

						if (General.Var.ByRoS)
						{
							General.Var.Bouyomiage(nameText.Text + "を、トラッキングなしで起動しました。");
						}
						System.Diagnostics.Process.Start(exePathText.Text);

						System.Environment.CurrentDirectory = General.Var.BaseDir;
					}
				}
				else
				{
					// checkBox6.Checked
					MessageBox.Show("テスト起動モードが有効です。\nこのモードでは起動時間、起動回数、DiscordRPCなどは実行されません。\n\n無効にするには、[テスト起動]チェックを外してください。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

					// 作業ディレクトリ変更
					String apppath = System.IO.Path.GetDirectoryName(exePathText.Text);
					System.Environment.CurrentDirectory = apppath;

					// 起動中gifの可視化
					pictureBox11.Visible = true;
					startButton.Enabled = false;

					// ウィンドウ最小化
					if (minCheck.Checked == true)
					{
						this.WindowState = FormWindowState.Minimized;
						this.notifyIcon1.Visible = true;
					}

					// ゲーム実行
					System.Diagnostics.Process p =
					System.Diagnostics.Process.Start(exePathText.Text);

					// 棒読み上げ
					if (General.Var.ByRoS)
					{
						General.Var.Bouyomiage(nameText.Text + "を、テストモードで起動しました。");
					}

					// ゲーム終了まで待機
					p.WaitForExit();

					// 作業ディレクトリ復元
					System.Environment.CurrentDirectory = General.Var.BaseDir;

					if (minCheck.Checked == true)
					{
						this.WindowState = FormWindowState.Normal;
						this.notifyIcon1.Visible = false;
					}

					// 起動中gifの非可視化
					pictureBox11.Visible = false;
					startButton.Enabled = true;

					// 終了検出後
					DialogResult dr = MessageBox.Show("実行終了を検出しました。\nゲームが正しく終了しましたか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (dr == DialogResult.Yes)
					{
						MessageBox.Show("正常にトラッキングできています。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show("トラッキングに失敗しています。\n\n以下をご確認ください。\n\n・ランチャーを指定していませんか？\n・GLを管理者権限で起動してみてください。\n・実行パスを英数字のみにしてみてください。\n\nそれでも解決しない場合は、トラッキングできません。ご了承ください。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
				"アプリ名を設定", General.Var.AppName, filename, -1, -1);

			String gfilepass = "";

			DialogResult rate = MessageBox.Show("成人向け(R-18)ゲームですか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
			if (rate == DialogResult.Yes)
			{
				ratedata = "1";
			}
			else
			{
				ratedata = "0";
			}

			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				if (File.Exists(General.Var.GameIni))
				{
					fileSystemWatcher1.EnableRaisingEvents = false;

					int newmaxval = General.Var.GameMax + 1;

					gfilepass = General.Var.GameDir + "\\" + newmaxval + ".ini";

					if (!(File.Exists(gfilepass)))
					{
						General.Var.IniWrite(gfilepass, "game", "name", appname_ans);
						General.Var.IniWrite(gfilepass, "game", "imgpass", imgans);
						General.Var.IniWrite(gfilepass, "game", "pass", exeans);
						General.Var.IniWrite(gfilepass, "game", "time", "0");
						General.Var.IniWrite(gfilepass, "game", "start", "0");
						General.Var.IniWrite(gfilepass, "game", "stat", string.Empty);
						General.Var.IniWrite(gfilepass, "game", "temp1", string.Empty);
						General.Var.IniWrite(gfilepass, "game", "rating", ratedata);
						General.Var.IniWrite(General.Var.GameIni, "list", "game", newmaxval.ToString());

						// 次回DB接続時に更新するフラグを立てる
						if (General.Var.SaveType == "T")
						{
							General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						String dup = General.Var.IniRead(gfilepass, "game", "name", "unknown");
						DialogResult dialogResult = MessageBox.Show("既にiniファイルが存在します！\n" + gfilepass + "\n[" + dup + "]\n上書きしますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dialogResult == DialogResult.Yes)
						{
							General.Var.IniWrite(gfilepass, "game", "name", appname_ans);
							General.Var.IniWrite(gfilepass, "game", "imgpass", imgans);
							General.Var.IniWrite(gfilepass, "game", "pass", exeans);
							General.Var.IniWrite(gfilepass, "game", "time", "0");
							General.Var.IniWrite(gfilepass, "game", "start", "0");
							General.Var.IniWrite(gfilepass, "game", "stat", string.Empty);
							General.Var.IniWrite(gfilepass, "game", "temp1", string.Empty);
							General.Var.IniWrite(gfilepass, "game", "rating", ratedata);
							General.Var.IniWrite(General.Var.GameIni, "list", "game", newmaxval.ToString());

							// 次回DB接続時に更新するフラグを立てる
							if (General.Var.SaveType == "T")
							{
								General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
							}
						}
						else if (dialogResult == DialogResult.No)
						{
							MessageBox.Show("新規のゲームを追加せずに処理を中断します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							resolveError(MethodBase.GetCurrentMethod().Name, "不明な結果です。\n実行を中断します。", 0, false);
						}
						return;
					}

					if (reloadCheck.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
				}
				else
				{
					resolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報統括管理ファイルが見つかりません！", 0, false);
					return;
				}
			}
			else
			{
				// database
				if (General.Var.SaveType == "D")
				{
					SqlConnection cn = General.Var.SqlCon;
					SqlCommand cm;
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"INSERT INTO " + General.Var.DbName + "." + General.Var.DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG ) VALUES ( '" + appname_ans + "', '" + exeans + "', '" + imgans + "', '0', '0','', '" + ratedata + "' )"
					};
					cm.Connection = cn;

					try
					{
						cn.Open();
						cm.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
					MySqlCommand cm;
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"INSERT INTO " + General.Var.DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG ) VALUES ( '" + appname_ans.Replace("'", "''").Replace("\\", "\\\\") + "', '" + exeans.Replace("'", "''").Replace("\\", "\\\\") + "', '" + imgans.Replace("'", "''").Replace("\\", "\\\\") + "', '0', '0','', '" + ratedata + "' )"
					};
					cm.Connection = cn;

					try
					{
						cn.Open();
						cm.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
					}
					finally
					{
						if (cn.State == ConnectionState.Open)
						{
							cn.Close();
						}
					}
				}
			}

			button8_Click(null, null);

		}

		private void button7_Click(object sender, EventArgs e)
		{
			DialogResult result = MessageBox.Show(General.Var.AppName + " Ver." + General.Var.AppVer + " / Build " + General.Var.AppBuild + "\n\n" + "現在の作業ディレクトリ [Mode：" + (General.Var.SaveType == "I" ? "ローカルINI" : General.Var.SaveType == "D" ? "SQL Server" : General.Var.SaveType == "M" ? "MySQL" : "オフラインINI") + "] ：" + ((General.Var.SaveType == "D" || General.Var.SaveType == "M") ? General.Var.DbUrl + ":" + General.Var.DbPort + " ▶ " + General.Var.DbName + "." + General.Var.DbTable : General.Var.GameDir) + "\n\nAuthor: Ogura Deko (dekosoft)\nMail: support_dekosoft@outlook.jp\nWeb: https://fanet.work\n\nCopyright (C) Ogura Deko and dekosoft Program rights reserved.",
								General.Var.AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(General.Var.GameMax >= 1))
			{
				return;
			}

			// グリッドと同期
			gameImgList.Items[gameList.SelectedIndex].Selected = true;
			gameImgList.EnsureVisible(gameList.SelectedIndex);

			// ゲーム詳細取得
			int selecteditem = gameList.SelectedIndex + 1;
			String readini = General.Var.GameDir + "\\" + selecteditem + ".ini";
			String id = null, namedata = null, imgpassdata = null, passdata = null, stimedata = null, startdata = null, cmtdata = null, temp1data = null, rating = null;

			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				if (File.Exists(readini))
				{
					// ini読込開始
					namedata = General.Var.IniRead(readini, "game", "name", string.Empty);
					imgpassdata = General.Var.IniRead(readini, "game", "imgpass", string.Empty);
					passdata = General.Var.IniRead(readini, "game", "pass", string.Empty);
					stimedata = General.Var.IniRead(readini, "game", "time", "0");
					startdata = General.Var.IniRead(readini, "game", "start", "0");
					cmtdata = General.Var.IniRead(readini, "game", "stat", string.Empty);
					temp1data = General.Var.IniRead(readini, "game", "temp1", string.Empty);
					rating = General.Var.IniRead(readini, "game", "rating", General.Var.Rate.ToString());
				}
				else
				{
					// 個別ini存在しない場合
					MessageBox.Show("iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。\n\nError: ini_read_error_nofile\nDebug:: gl > listBox1_SelectedIndexChanged > if > else",
									General.Var.AppName,
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);
				}
			}
			else
			{
				// database

				if (General.Var.SaveType == "D")
				{
					SqlConnection cn = General.Var.SqlCon;
					SqlCommand cm;

					if (selecteditem.ToString().Length != 0)
					{
						cm = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
											+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbName + "." + General.Var.DbTable + ") AS T "
											+ "WHERE ROWCNT = " + selecteditem
											+ " ORDER BY ID ASC"
						};
					}
					else
					{
						cm = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
											+ "FROM " + General.Var.DbName + "." + General.Var.DbTable
											+ " ORDER BY ID ASC"
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
							temp1data = reader["TEMP1"].ToString();
							rating = reader["AGE_FLG"].ToString();
						}

					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
					MySqlCommand cm;

					if (selecteditem.ToString().Length != 0)
					{
						cm = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
											+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbTable + ") AS T "
											+ "WHERE ROWCNT = " + selecteditem
											+ " ORDER BY ID ASC"
						};
					}
					else
					{
						cm = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
											+ "FROM " + General.Var.DbTable
											+ " ORDER BY ID ASC"
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
							temp1data = reader["TEMP1"].ToString();
							rating = reader["AGE_FLG"].ToString();
						}

					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
					}
					finally
					{
						if (cn.State == ConnectionState.Open)
						{
							cn.Close();
						}
					}
				}
			}

			int timedata = Convert.ToInt32(stimedata) / 60;

			label1.Text = namedata;
			nameText.Text = namedata;
			exePathText.Text = passdata;
			imgPathText.Text = imgpassdata;
			runTimeText.Text = timedata.ToString();
			startTimeText.Text = startdata;
			dconText.Text = cmtdata;
			dconImgText.Text = temp1data;
			General.Var.CurrentGameDbVal = id;
			toolStripStatusLabel1.Text = "[" + selecteditem + "/" + General.Var.GameMax + "]";
			toolStripProgressBar1.Value = gameList.SelectedIndex + 1;

			if (Convert.ToInt32(rating) == 0)
			{
				normalRadio.Checked = true;
			}
			else if (Convert.ToInt32(rating) == 1)
			{
				ratedRadio.Checked = true;
			}

			if (File.Exists(passdata))
			{
				startButton.Enabled = true;
			}
			else
			{
				startButton.Enabled = false;
			}

			if (File.Exists(imgpassdata))
			{
				try
				{
					gameIcon.ImageLocation = imgpassdata;
				}
				catch (Exception ex)
				{
					gameIcon.ImageLocation = "";
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, imgpassdata);
				}
			}
			else
			{
				gameIcon.ImageLocation = "";
			}

			if ((runTimeText.Text == "35791394") || (startTimeText.Text == Int32.MaxValue.ToString()))
			{
				// 最大の場合、実行できないようにする
				startButton.Enabled = false;
			}
			else
			{
				startButton.Enabled = true;
			}

			return;
		}

		private void button8_Click(object sender, EventArgs e)
		{
			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				if (File.Exists(General.Var.GameIni))
				{
					// ini読込開始
					General.Var.GameMax = Convert.ToInt32(General.Var.IniRead(General.Var.GameIni, "list", "game", "0"));
				}
				else
				{
					return;
				}

				LoadItem(General.Var.GameDir);
				if (gameList.Items.Count != 0)
				{
					String selectedtext = gameList.SelectedItem.ToString();
					if (gameList.Items.Contains(selectedtext))
					{
						gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
					}
				}
			}
			else
			{
				// database
				if (General.Var.SaveType == "D")
				{
					LoadItem2(General.Var.SqlCon);
				}
				else
				{
					LoadItem3(General.Var.SqlCon2);
				}

				if (gameList.Items.Count != 0)
				{
					string selectedtext = gameList.SelectedItem.ToString();
					if (gameList.Items.Contains(selectedtext))
					{
						gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
					}
				}
			}
			GC.Collect();
			return;
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			if (!(nameText.Text.Equals("")))
			{
				Clipboard.SetText(nameText.Text);
			}
		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			if (!(exePathText.Text.Equals("")))
			{
				Clipboard.SetText(exePathText.Text);
			}
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
			if (!(imgPathText.Text.Equals("")))
			{
				Clipboard.SetText(imgPathText.Text);
			}
		}

		private void pictureBox9_Click(object sender, EventArgs e)
		{
			if (!(runTimeText.Text.Equals("")))
			{
				int total = Convert.ToInt32(runTimeText.Text);
				String hour = (total / 60).ToString();
				String min = (total % 60).ToString();
				int sTotal = (Convert.ToInt32(startTimeText.Text) == 0 ? 1 : Convert.ToInt32(startTimeText.Text));
				string sMin = (total / sTotal).ToString();
				string sHour = (Convert.ToInt32(sMin) / 60).ToString();
				sMin = (Convert.ToInt32(sMin) % 60).ToString();

				DialogResult result = MessageBox.Show(nameText.Text +
										"\nおおよその起動時間：" + hour + "時間 " + min + "分" +
										"\n平均起動時間：" + sHour + "時間" + sMin + "分/回",
										General.Var.AppName,
										MessageBoxButtons.OK,
										MessageBoxIcon.Information);
			}
		}

		private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
		{
			pictureBox11.Visible = true;
			String selectedtext = gameList.SelectedItem.ToString();

			LoadItem(General.Var.GameDir);
			if (gameList.Items.Contains(selectedtext))
			{
				gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
			}
			pictureBox11.Visible = false;
			return;
		}
		private void fileSystemWatcher2_Changed(object sender, FileSystemEventArgs e)
		{
			pictureBox11.Visible = true;
			String selectedtext = gameList.SelectedItem.ToString();

			updateComponent();
			LoadItem(General.Var.GameDir);
			if (gameList.Items.Contains(selectedtext))
			{
				gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
			}
			pictureBox11.Visible = false;
			return;
		}

		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			if (reloadCheck.Checked)
			{
				if (General.Var.SaveType == "I")
				{
					// ini
					fileSystemWatcher1.EnableRaisingEvents = true;
				}
				else
				{
					// database
					MessageBox.Show("データベースを使用しているため有効にできません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					reloadCheck.Checked = false;
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
				String rawdata = General.Var.IniRead(ininame, sec, key, failedval);

				//取得値とデータが違う場合
				if (!(rawdata.Equals(data)))
				{
					if (reloadCheck.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = false;
					}

					General.Var.IniWrite(ininame, sec, key, data);

					if (General.Var.SaveType == "I")
					{
						if (reloadCheck.Checked)
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
						}
					}
				}
			}
			else
			{
				MessageBox.Show("該当するファイルがありません。\n\nError: INI_file_not_found\nDebug:: iniedtchk > if > else\n" + ininame, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			if (reloadCheck.Checked)
			{
				if (General.Var.SaveType == "I")
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
			if (General.Var.GLConfigLoad() == false)
			{
				resolveError(MethodBase.GetCurrentMethod().Name, "Configロード中にエラー。\n詳しくはエラーログを参照して下さい。", 0, false);
			}

			if (File.Exists(General.Var.ConfigIni))
			{
				int ckv0 = Convert.ToInt32(General.Var.IniRead(General.Var.ConfigIni, "checkbox", "track", "0"));
				int ckv1 = Convert.ToInt32(General.Var.IniRead(General.Var.ConfigIni, "checkbox", "winmini", "0"));

				dconImgText.Enabled = (General.Var.DconAppID.Length != 0);

				String bgimg = General.Var.BgImg;
				sensCheck.Checked = Convert.ToBoolean(Convert.ToInt32(General.Var.IniRead(General.Var.ConfigIni, "checkbox", "sens", "0")));
				useDconCheck.Checked = General.Var.Dconnect;

				if (General.Var.Rate == 1)
				{
					ratedRadio.Checked = true;
				}
				else
				{
					normalRadio.Checked = true;
				}

				if (Convert.ToInt32(General.Var.IniRead(General.Var.ConfigIni, "connect", "ByActive", "0")) == 1)
				{
					General.Var.ByActive = true;
					if (General.Var.ByRoW)
					{
						bouyomi_configload();
					}
				}
				else
				{
					General.Var.ByActive = false;
				}

				if (File.Exists(bgimg))
				{
					this.BackgroundImage = new Bitmap(bgimg);
					this.BackgroundImageLayout = ImageLayout.Stretch;
					toolStripStatusLabel2.Text = "！背景画像が適用されました。環境によっては描画に時間がかかる場合があります。！";
				}
				else
				{
					this.BackgroundImage = null;
					message();
				}

				trackCheck.Checked = Convert.ToBoolean(ckv0);
				minCheck.Checked = Convert.ToBoolean(ckv1);

				fileSystemWatcher2.Path = General.Var.BaseDir;

				// データベース使用時の部品非表示処理
				if (General.Var.SaveType == "D" || General.Var.SaveType == "M")
				{
					// databaseの場合
					upButton.Visible = false;
					downButton.Visible = false;
					reloadCheck.Checked = false;
					reloadCheck.Visible = false;
					// editButton.Visible = false;
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
				File.Create(General.Var.ConfigIni).Close();
				General.Var.GameDir = General.Var.BaseDir + "Data";
				General.Var.GameIni = General.Var.GameDir + "\\game.ini";
				trackCheck.Checked = true;
				minCheck.Checked = true;
				fileSystemWatcher2.Path = General.Var.BaseDir;
				tabControl1.TabPages.Remove(tabPage3);
				trackCheck.Checked = true;
				minCheck.Checked = true;
			}
			return;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			int rawdata = General.Var.GameMax;

			if (rawdata >= 1)
			{
				//乱数生成
				System.Random r = new System.Random();
				int ans = r.Next(1, rawdata + 1);

				gameList.SelectedIndex = ans - 1;

				//グリッドと同期
				gameImgList.Items[gameList.SelectedIndex].Selected = true;
				gameImgList.EnsureVisible(gameList.SelectedIndex);
			}
			else
			{
				MessageBox.Show("登録ゲーム数が少ないため、ランダム選択できません！", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
			ExitApp();
		}

		private void button5_Click(object sender, EventArgs e)
		{
			if (gameList.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			//選択中のゲーム保管ファイルを削除
			pictureBox11.Visible = true;
			int delval = gameList.SelectedIndex + 1;
			String delname = nameText.Text;
			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				delIniItem(delval, delname);

				// 次回DB接続時に更新するフラグを立てる
				if (General.Var.SaveType == "T")
				{
					General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
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

		private void editButton_Click(object sender, EventArgs e)
		{
			String selectedListCount = (gameList.SelectedIndex + 1).ToString();

			if (gameList.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (General.Var.SaveType == "D")
			{
				SqlConnection cn = General.Var.SqlCon;
				SqlCommand cm;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.GAME_PATH, T.IMG_PATH, T.UPTIME, T.RUN_COUNT, T.DCON_TEXT, T.TEMP1, T.AGE_FLG, T.ROW_CNT "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + General.Var.DbName + "." + General.Var.DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Form5 form5 = new Form5(General.Var.SaveType, selectedListCount, cn, cm);
				form5.StartPosition = FormStartPosition.CenterParent;
				form5.ShowDialog(this);
				listBox1_SelectedIndexChanged(null, null);
				return;
			}
			else if (General.Var.SaveType == "M")
			{
				MySqlConnection cn = General.Var.SqlCon2;
				MySqlCommand cm;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.GAME_PATH, T.IMG_PATH, T.UPTIME, T.RUN_COUNT, T.DCON_TEXT, T.TEMP1, T.AGE_FLG, T.ROW_CNT "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + General.Var.DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Form5 form5 = new Form5(General.Var.SaveType, selectedListCount, cn, cm);
				form5.StartPosition = FormStartPosition.CenterParent;
				form5.ShowDialog(this);
				listBox1_SelectedIndexChanged(null, null);
				return;
			}

			// Discordカスタムステータス、各種チェックボックス、ラジオの保存
			String path = General.Var.GameDir + "\\" + (gameList.SelectedIndex + 1) + ".ini";
			if (File.Exists(path))
			{
				Form5 form5 = new Form5(General.Var.SaveType, selectedListCount, new SqlConnection(), new SqlCommand());
				form5.StartPosition = FormStartPosition.CenterParent;
				form5.ShowDialog(this);
				listBox1_SelectedIndexChanged(null, null);
			}
			else
			{
				// 個別ini不存在
				resolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報保管iniが存在しません。\n" + path, 0, false);
			}

			return;
		}

		private void toolStripStatusLabel3_Click(object sender, EventArgs e)
		{
			if (General.Var.SaveType == "D" || General.Var.SaveType == "M")
			{
				// 現段階ではDBに登録されている情報の変更は不可（※将来対応予定）
				return;
			}

			// ini読込
			String rawdata = General.Var.GameDir + "\\" + ((gameList.SelectedIndex + 1).ToString()) + ".ini";

			if (File.Exists(rawdata))
			{
				System.Diagnostics.Process.Start(General.Var.GameDir + "\\" + (gameList.SelectedIndex + 1) + ".ini");
			}
			else
			{
				resolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報保管iniがありません！\n" + rawdata, 0, false);
			}
		}

		private void button6_Click(object sender, EventArgs e)
		{
			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				int selected = gameList.SelectedIndex;
				if (selected >= 1)
				{
					selected++;
					fileSystemWatcher1.EnableRaisingEvents = false;
					int target = selected - 1;
					String before = General.Var.GameDir + "\\" + (selected.ToString()) + ".ini";
					String temp = General.Var.GameDir + "\\" + (target.ToString()) + "_.ini";
					String after = General.Var.GameDir + "\\" + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (General.Var.SaveType == "T")
						{
							General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						resolveError(MethodBase.GetCurrentMethod().Name, "移動先もしくは移動前のファイルが見つかりません。\n\n移動前：" + before + "\n移動先：" + after, 0, false);
					}
					if (reloadCheck.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
				}
				else
				{
					MessageBox.Show("最上位です。\nこれ以上は上に移動できません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(General.Var.GameDir);
				gameList.SelectedIndex = selected - 2;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return;
		}

		private void pictureBox13_Click(object sender, EventArgs e)
		{
			if (exePathText.Text != "")
			{
				String opendir = System.IO.Path.GetDirectoryName(exePathText.Text);
				if (Directory.Exists(opendir))
				{
					System.Diagnostics.Process.Start(opendir);
				}
			}
		}

		private void pictureBox14_Click(object sender, EventArgs e)
		{
			if (exePathText.Text != "")
			{
				String opendir = System.IO.Path.GetDirectoryName(exePathText.Text);
				if (Directory.Exists(opendir))
				{
					System.Diagnostics.Process.Start(opendir);
				}
			}
		}

		private void checkBox6_CheckedChanged(object sender, EventArgs e)
		{
			if (testCheck.Checked)
			{
				trackCheck.Enabled = false;
			}
			else
			{
				trackCheck.Enabled = true;
			}
		}

		private void button10_Click(object sender, EventArgs e)
		{
			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// ini
				int selected = gameList.SelectedIndex;
				if (selected + 1 < General.Var.GameMax)
				{
					selected++;
					fileSystemWatcher1.EnableRaisingEvents = false;
					int target = selected + 1;
					String before = General.Var.GameDir + "\\" + (selected.ToString()) + ".ini";
					String temp = General.Var.GameDir + "\\" + (target.ToString()) + "_.ini";
					String after = General.Var.GameDir + "\\" + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (General.Var.SaveType == "T")
						{
							General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						resolveError(MethodBase.GetCurrentMethod().Name, "移動先もしくは移動前のファイルが見つかりません。\nファイルに影響はありません。\n\n移動前：" + before + "\n移動先：" + after, 0, false);
						if (reloadCheck.Checked)
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
						}
						return;
					}
					if (reloadCheck.Checked)
					{
						fileSystemWatcher1.EnableRaisingEvents = true;
					}
				}
				else
				{
					MessageBox.Show("最下位です。\nこれ以上は下に移動できません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(General.Var.GameDir);
				gameList.SelectedIndex = selected--;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return;
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(General.Var.GameMax >= 1))
			{
				return;
			}

			if (gameImgList.SelectedItems.Count <= 0)
			{
				return;
			}

			//リストと同期
			gameList.SelectedIndex = Convert.ToInt32(gameImgList.SelectedItems[0].Index);

			return;

		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			switch (trackBar1.Value)
			{
				case 0:

					gameImgList.LargeImageList = imageList0;
					break;
				case 1:
					gameImgList.LargeImageList = imageList1;
					break;
				case 2:
					gameImgList.LargeImageList = imageList2;
					break;
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			// 設定
			fileSystemWatcher1.EnableRaisingEvents = false;
			fileSystemWatcher2.EnableRaisingEvents = false;
			String beforeWorkDir = General.Var.BaseDir;
			String beforeSaveType = General.Var.ReadIni("general", "save", "I");
			form2.StartPosition = FormStartPosition.CenterParent;
			form2.ShowDialog(this);
			updateComponent();
			String afterWorkDir = General.Var.BaseDir;
			String aftereSaveType = General.Var.ReadIni("general", "save", "I");
			if (beforeWorkDir != afterWorkDir)
			{
				MessageBox.Show("既定の作業ディレクトリが変更されました。\nGame Launcherを再起動してください。\n\nOKを押してGame Launcherを終了します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
			}
			else if (beforeSaveType != aftereSaveType)
			{
				MessageBox.Show("データの保存方法が変更されました。\nGame Launcherを再起動してください。\n\nOKを押してGame Launcherを終了します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				Close();
			}

			if (reloadCheck.Checked)
			{
				fileSystemWatcher2.Path = General.Var.BaseDir;
				fileSystemWatcher2.EnableRaisingEvents = true;
				if (General.Var.SaveType == "I")
				{
					fileSystemWatcher1.Path = General.Var.GameDir;
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
			String delfile = (General.Var.GameDir + "\\" + delval + ".ini");
			if (File.Exists(delfile))
			{
				//削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("選択中のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delfilename + "]\n" + delfile + "\n削除しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					File.Delete(delfile);
					nextval = delval + 1;
					nextfile = (General.Var.GameDir + "\\" + nextval + ".ini");
					while (File.Exists(nextfile))
					{
						//削除ファイル以降にゲームが存在する場合に番号を下げる
						File.Move(nextfile, delfile);
						delfile = nextfile;
						nextval++;
						nextfile = (General.Var.GameDir + "\\" + nextval + ".ini");
					}
					General.Var.GameMax--;
					General.Var.IniWrite(General.Var.GameIni, "list", "game", General.Var.GameMax.ToString());
				}
				else if (dialogResult == DialogResult.No)
				{
					if (reloadCheck.Checked)
					{
						fileSystemWatcher2.EnableRaisingEvents = true;
						if (General.Var.SaveType == "I")
						{
							fileSystemWatcher1.EnableRaisingEvents = true;
						}
					}
					return;
				}
				else
				{
					MessageBox.Show("不明な結果です。\n実行を中断します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else
			{
				//削除ファイル不存在
				MessageBox.Show("該当するiniファイルが存在しません。\n" + delfile, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			LoadItem(General.Var.GameDir);
			if (General.Var.GameMax >= 2 && delfileval >= 2)
			{
				gameList.SelectedIndex = delfileval - 2;
			}
			else if (General.Var.GameMax == 1 && delfileval >= 1)
			{
				gameList.SelectedIndex = 1;
			}
			else
			{
				gameList.SelectedIndex = 0;
			}

			if (reloadCheck.Checked)
			{
				fileSystemWatcher2.EnableRaisingEvents = true;
				if (General.Var.SaveType == "I")
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

			if (General.Var.SaveType == "D")
			{
				SqlConnection cn = General.Var.SqlCon;
				SqlCommand cm;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbName + "." + General.Var.DbTable + ") AS T "
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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}

				//削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("次のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delName + "]\n" + delPath + "\n削除しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					// 削除
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DELETE " + General.Var.DbName + "." + General.Var.DbTable + " "
										+ "FROM " + General.Var.DbName + "." + General.Var.DbTable + " glt "
										+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbName + "." + General.Var.DbTable + ") tmp "
										+ "ON glt.ID = tmp.ID "
										+ "WHERE ROWCNT = " + delItemVal
					};
					cm.Connection = cn;
					try
					{
						toolStripStatusLabel2.Text = "＊＊削除実行中…";
						cn.Open();
						cm.ExecuteNonQuery();
						General.Var.GameMax--;
					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
					}
					finally
					{
						toolStripStatusLabel2.Text = "＊削除完了";
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
					MessageBox.Show("不明な結果です。\n実行を中断します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				LoadItem2(General.Var.SqlCon);
			}
			else
			{
				MySqlConnection cn = General.Var.SqlCon2;
				MySqlCommand cm;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbTable + ") AS T "
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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}

				//削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("次のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delName + "]\n" + delPath + "\n削除しますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					// 削除
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DELETE " + General.Var.DbTable + " "
										+ "FROM " + General.Var.DbTable + " glt "
										+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + General.Var.DbTable + ") tmp "
										+ "ON glt.ID = tmp.ID "
										+ "WHERE ROWCNT = " + delItemVal
					};
					cm.Connection = cn;
					try
					{
						toolStripStatusLabel2.Text = "＊＊削除実行中…";
						cn.Open();
						cm.ExecuteNonQuery();
						General.Var.GameMax--;
					}
					catch (Exception ex)
					{
						General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
					}
					finally
					{
						toolStripStatusLabel2.Text = "＊削除完了";
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
					MessageBox.Show("不明な結果です。\n実行を中断します。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				LoadItem3(General.Var.SqlCon2);
			}

			if (General.Var.GameMax >= 2 && delItemVal >= 2)
			{
				gameList.SelectedIndex = delItemVal - 2;
			}
			else if (General.Var.GameMax == 1 && delItemVal >= 1)
			{
				gameList.SelectedIndex = 1;
			}
			else
			{
				gameList.SelectedIndex = 0;
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
				MessageBox.Show("既にdcon.jarが終了しています。\n起動時間が正常に記録されない可能性があります。", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private void bouyomi_configload()
		{
			General.Var.ByHost = General.Var.IniRead(General.Var.ConfigIni, "connect", "General.Var.ByHost", "127.0.0.1");
			General.Var.ByPort = Convert.ToInt32(General.Var.IniRead(General.Var.ConfigIni, "connect", "General.Var.ByPort", "50001"));
			General.Var.Bouyomi_Connectchk(General.Var.ByHost, General.Var.ByPort, General.Var.ByType, false);
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
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case 4:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
					break;

				case 5:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, General.Var.AppName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
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
			if (General.Var.ByRoW)
			{
				General.Var.Bouyomiage("ゲームランチャーを終了しました");
			}

			if ((General.Var.SaveType == "D" || General.Var.SaveType == "M") && General.Var.OfflineSave)
			{
				string localPath = General.Var.BaseDir + (General.Var.BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
				General.Var.downloadDbDataToLocal(localPath);
			}

			Application.Exit();
		}

		private void button11_Click(object sender, EventArgs e)
		{
			long beforeMemory = Environment.WorkingSet;
			GC.Collect();
			long afterMemory = Environment.WorkingSet;
			long diffMemory = beforeMemory - afterMemory;
			MessageBox.Show("メモリを解放しました。\n" + beforeMemory + "byte -> " + afterMemory + "byte (" + diffMemory + "byte)", General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void button12_Click(object sender, EventArgs e)
		{
			if (General.Var.GridMax)
			{
				// 現在最大表示の場合、最小表示にする
				tabControl1.Width = 345;

				gameList.Width = 330;

				gameImgList.Width = 330;
				trackBar1.Width = 330;

				searchText.Width = 132;
				orderDropDown.Location = new Point(219, 9);
				searchButton.Location = new Point(274, 8);

				searchingText.Width = 197;
				searchResultList.Width = 330;
				lastOrderDrop.Location = new Point(284, 35);

				ocButton.Text = ">";
				General.Var.GridMax = false;
			}
			else
			{
				// 最小表示の場合、最大表示にする
				tabControl1.Width = 840;

				gameList.Width = 825;

				gameImgList.Width = 825;
				trackBar1.Width = 825;

				searchText.Width = 623;
				orderDropDown.Location = new Point(711, 9);
				searchButton.Location = new Point(766, 8);

				searchingText.Width = 623;
				searchResultList.Width = 825;
				lastOrderDrop.Location = new Point(711, 35);

				ocButton.Text = "<";
				General.Var.GridMax = true;
			}
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{
			if (File.Exists(gameIcon.ImageLocation))
			{
				Form4 form4 = new Form4(gameIcon.ImageLocation);
				form4.StartPosition = FormStartPosition.CenterParent;
				form4.ShowDialog(this);
			}
		}

		/// <summary>
		/// 検索ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button13_Click(object sender, EventArgs e)
		{
			searchExec(false);
		}

		/// <summary>
		/// 検索画面更新
		/// </summary>
		/// <param name="reSearch">再検索フラグ</param>
		private void searchExec(bool reSearch = false)
		{

			if (General.Var.GameMax <= 0)
			{
				return;
			}

			string searchName = reSearch ? searchingText.Text.Trim() : searchText.Text.Trim();
			string searchOption = string.Empty;

			// 検索方法設定
			switch (reSearch ? lastSearchOption.SelectedIndex : searchTargetDropDown.SelectedIndex)
			{
				case 0:
					// タイトル
					searchOption = "GAME_NAME";
					break;
				case 1:
					// 最終起動
					searchOption = "LAST_RUN";
					break;
				case 2:
					// dconメモ
					searchOption = "DCON_TEXT";
					break;
				case 3:
					// 実行パス
					searchOption = "GAME_PATH";
					break;
				case 4:
					// 画像パス
					searchOption = "IMG_PATH";
					break;
			}

			// 検索条件表示
			searchingText.Text = searchName;
			lastSearchOption.SelectedIndex = reSearch ? lastSearchOption.SelectedIndex : searchTargetDropDown.SelectedIndex;
			lastOrderDrop.SelectedIndex = reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex;

			// 検索処理
			searchResultList.Items.Clear();

			if (General.Var.SaveType == "D")
			{
				SqlConnection cn = General.Var.SqlCon;

				try
				{
					toolStripStatusLabel2.Text = "＊＊検索実行中…";
					Application.DoEvents();
					// 接続オープン
					cn.Open();

					//検索に一致するゲーム数取得
					SqlCommand cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + General.Var.DbName + "." + General.Var.DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
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
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG FROM " + General.Var.DbName + "." + General.Var.DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
										+ " ORDER BY " + searchOption + ((reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex) == 0 ? " ASC" : " DESC")
					};
					cm2.Connection = cn;

					using (var reader = cm2.ExecuteReader())
					{
						while (reader.Read() == true)
						{
							searchResultList.Items.Add(reader["GAME_NAME"].ToString());
						}
					}
				}
				catch (Exception ex)
				{
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
				}
				finally
				{
					toolStripStatusLabel2.Text = "＊検索完了";
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}
			else if (General.Var.SaveType == "M")
			{
				MySqlConnection cn = General.Var.SqlCon2;

				try
				{
					toolStripStatusLabel2.Text = "＊＊検索実行中…";
					Application.DoEvents();
					// 接続オープン
					cn.Open();

					//検索に一致するゲーム数取得
					MySqlCommand cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + General.Var.DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
					};
					cm.Connection = cn;

					int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

					if (sqlAns <= 0)
					{
						//ゲームが1つもない場合
						return;
					}

					// DBからデータを取り出す
					MySqlCommand cm2 = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, TEMP1, AGE_FLG FROM " + General.Var.DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
										+ " ORDER BY " + searchOption + ((reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex) == 0 ? " ASC" : " DESC")
					};
					cm2.Connection = cn;

					using (var reader = cm2.ExecuteReader())
					{
						while (reader.Read() == true)
						{
							searchResultList.Items.Add(reader["GAME_NAME"].ToString());
						}
					}
				}
				catch (Exception ex)
				{
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
				}
				finally
				{
					toolStripStatusLabel2.Text = "＊検索完了";
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
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
			if (!(searchResultList.Items.Count >= 1))
			{
				return;
			}

			string searchName = searchingText.Text.Trim();
			string searchOption = string.Empty;

			// 検索方法設定

			switch (lastSearchOption.SelectedIndex)
			{
				case 0:
					// タイトル
					searchOption = "GAME_NAME";
					break;
				case 1:
					// 最終起動
					searchOption = "LAST_RUN";
					break;
				case 2:
					// dconメモ
					searchOption = "DCON_TEXT";
					break;
				case 3:
					// 実行パス
					searchOption = "GAME_PATH";
					break;
				case 4:
					// 画像パス
					searchOption = "IMG_PATH";
					break;
			}

			//ゲーム詳細取得
			int selecteditem = searchResultList.SelectedIndex + 1;

			if (General.Var.SaveType == "I" || General.Var.SaveType == "T")
			{
				// iniでは検索は実行できないのでリターン
				return;
			}
			else if (General.Var.SaveType == "D")
			{
				// 選択されたアイテムの検索を行う
				SqlConnection cn = General.Var.SqlCon;
				SqlCommand cm;

				if (selecteditem.ToString().Length != 0)
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText =
						  @" SELECT"
						 + "	[ROW1] "
						 + " FROM "
						 + General.Var.DbName + "." + General.Var.DbTable + " AS MAIN "
						 + "	LEFT OUTER JOIN ( "
						 + "		SELECT "
						 + "			[T].[ID], [T].[GAME_PATH], [T].[IMG_PATH], [T].[DCON_TEXT], [T].[TEMP1], [T].[UPTIME], [T].[RUN_COUNT], [T].[AGE_FLG], [T].[LAST_RUN], ROW_NUMBER() over (ORDER BY [T].[ID]) AS [ROW1], [T2].[ROW2] "
						 + "		FROM "
						 + General.Var.DbName + "." + General.Var.DbTable + " AS [T] "
						 + "		LEFT OUTER JOIN ( "
						 + "			SELECT "
						 + "				[ID], ROW_NUMBER() over (ORDER BY " + searchOption + " " + (lastOrderDrop.SelectedIndex == 0 ? "ASC" : "DESC") + ") AS [ROW2] "
						 + "			FROM "
						 + General.Var.DbName + "." + General.Var.DbTable
						 + (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
						 + "		) AS [T2] "
						 + " ON [T].[ID] = [T2].[ID] "
						 + ") AS SUB "
						 + " ON [MAIN].[ID] = [SUB].[ID] "
						 + " WHERE [SUB].[ROW2] = " + selecteditem
						 + " ORDER BY [SUB].[ROW2] ASC "
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
						rowCount = Convert.ToInt32(reader["ROW1"]);
					}

				}
				catch (Exception ex)
				{
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
				// 選択されたアイテムの検索を行う
				MySqlConnection cn = General.Var.SqlCon2;
				MySqlCommand cm;

				if (selecteditem.ToString().Length != 0)
				{
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText =
						  @" SELECT"
						 + "	ROW1 "
						 + " FROM "
						 + General.Var.DbTable + " AS MAIN "
						 + "	LEFT OUTER JOIN ( "
						 + "		SELECT "
						 + "			T.ID, T.GAME_PATH, T.IMG_PATH, T.DCON_TEXT, T.TEMP1, T.UPTIME, T.RUN_COUNT, T.AGE_FLG, T.LAST_RUN, ROW_NUMBER() over (ORDER BY T.ID) AS ROW1, T2.ROW2 "
						 + "		FROM "
						 + General.Var.DbTable + " AS T "
						 + "		LEFT OUTER JOIN ( "
						 + "			SELECT "
						 + "				ID, ROW_NUMBER() over (ORDER BY " + searchOption + " " + (lastOrderDrop.SelectedIndex == 0 ? "ASC" : "DESC") + ") AS ROW2 "
						 + "			FROM "
						 + General.Var.DbTable
						 + (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE '%" + searchName + "%'")
						 + "		) AS T2 "
						 + " ON T.ID = T2.ID "
						 + ") AS SUB "
						 + " ON MAIN.ID = SUB.ID "
						 + " WHERE SUB.ROW2 = " + selecteditem
						 + " ORDER BY SUB.ROW2 ASC "
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
						rowCount = Convert.ToInt32(reader["ROW1"]);
					}

				}
				catch (Exception ex)
				{
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					resolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
			gameImgList.Items[rowCount].Selected = true;
			gameImgList.EnsureVisible(rowCount);

			return;
		}

		/// <summary>
		/// 検索条件ドロップダウン変更時
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchTargetDropDown_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (searchTargetDropDown.SelectedIndex)
			{
				case 0:
					// タイトル
					searchText.Enabled = true;
					break;
				case 1:
					// 最終起動
					searchText.Enabled = false;
					break;
				case 2:
					// dconメモ
					searchText.Enabled = true;
					break;
				case 3:
					// 実行パス
					searchText.Enabled = true;
					break;
				case 4:
					// 画像パス
					searchText.Enabled = true;
					break;
			}
		}
	}
}
