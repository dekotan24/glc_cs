﻿using glc_cs.Properties;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static glc_cs.Core.DataBind;
using static glc_cs.Core.Property;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class gl : Form
	{
		// 設定フォーム宣言
		Config ConfigForm = new Config();
		Splash SplashForm = new Splash();
		Splash2 Splash2Form = new Splash2();

		private bool enabledExSplash = false;

		public gl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 初期起動時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gl_Load(object sender, EventArgs e)
		{
			// スプラッシュ画面表示
			enabledExSplash = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "exSplash", "0", 1)));
			if (enabledExSplash)
			{
				Splash2Form.Show();
			}
			else
			{
				SplashForm.Show();
			}

			// ステータス変更
			UpdateSplashInfo(1, "準備中…", enabledExSplash);

			Application.DoEvents();

			// スタイル設定
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			DateTime appStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

			// 終了処理設定
			Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

			// 実行ボタンカバーを表示
			runningPicture.Visible = true;

			// ステータス変更
			UpdateSplashInfo(2, "設定を読み込み中…", enabledExSplash);

			// 設定ファイル読込
			UpdateComponent();

			// ステータス変更
			UpdateSplashInfo(3, "アップデートのチェック中…", enabledExSplash);
			// アップデートチェック
			if (!(InitialUpdateCheckSkipFlg && InitialUpdateCheckSkipVer.Equals(AppVer)))
			{
				CheckItemUpdate();
			}

			// ステータス変更
			UpdateSplashInfo(4, "ゲームリストのロード中…", enabledExSplash);

			// アイテム読込
			string item = SaveType == "I" ? LoadItem(GameDir) : SaveType == "D" ? LoadItem2(SqlCon, IsFirstLoad) : LoadItem3(SqlCon2, IsFirstLoad);

			// ステータス変更
			UpdateSplashInfo(5, "UIの読み込み中…", enabledExSplash);

			if (WindowHideControlFlg)
			{
				this.MinimizeBox = true;
			}

			tabControl1.SelectedIndex = 1;
			tabControl1.SelectedIndex = 0;

			// グリッド削除処理
			if (!GridEnable)
			{
				tabControl1.Controls.Remove(tabPage2);
			}

			// コントロールセット
			// ステータスドロップダウン
			foreach (string statusName in StatusDropDown())
			{
				statusCombo.Items.Add(statusName);
			}

			// 検索タブ
			searchTargetDropDown.SelectedIndex = 0;
			orderDropDown.SelectedIndex = 0;

			// 実行ボタン読込中画像を非表示
			runningPicture.Visible = false;

			// ステータス変更
			UpdateSplashInfo(6, "最終処理中…", enabledExSplash);

			// アイテム詳細の再表示
			GameList_SelectedIndexChanged(sender, e);

			// メインフォーム表示
			this.Show();
			this.Activate();
			this.Refresh();
			SplashForm.Close();
			SplashForm.Dispose();
			Splash2Form.Close();
			Splash2Form.Dispose();
			Application.DoEvents();

			// 準備所要時間計算
			DateTime appReadyTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
			string temp = (appReadyTime - appStartTime).ToString();
			int wakeUpTimes = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

			Message(AppName + "へようこそ。ランチャーの起動時間は、" + wakeUpTimes + "秒でした。");

			if (ByActive && ByRoW)
			{
				Bouyomiage("ゲームランチャーを起動しました");
			}

			if (item == "_none_game_data" || item == "0")
			{
				if (SaveType == "I" || SaveType == "T")
				{
					// ini
					if (!(File.Exists(GameIni)))
					{
						WriteIni("list", "game", "0", 0);
					}


					if (!(Directory.Exists(GameDir)))
					{
						try
						{
							Directory.CreateDirectory(GameDir);
						}
						catch (Exception ex)
						{
							ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
						}
					}
				}
				// itemがnoneの場合：ゲームが登録されていない場合
				MessageBox.Show("GLauncherをご利用頂きありがとうございます。\n\"追加\"ボタンを押して、ゲームを追加しましょう！",
								AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
			}

			IsFirstLoad = false;
			GC.Collect();
		}

		/// <summary>
		/// スプラッシュスクリーンのステータス変更
		/// </summary>
		/// <param name="value">進捗率</param>
		/// <param name="message">メッセージ</param>
		private void UpdateSplashInfo(int value, string message, bool exSplash, int value2 = 0, int value3 = 0)
		{
			if (exSplash)
			{
				Splash2Form.SetProgress(value, message, value2, value3);
			}
			else
			{
				SplashForm.SetProgress(value, message, value2, value3);
			}
		}

		/// <summary>
		/// ゲームリストの読み込み
		/// </summary>
		/// <param name="gameDirname">iniファイルが格納されているフォルダ</param>
		/// <returns>異常なら"_none_game_data"を返します。正常に処理した場合は空欄が返されます。</returns>
		public string LoadItem(string gameDirname)
		{
			gameList.Items.Clear();
			gameImgList.Items.Clear();
			imageList8.Images.Clear();
			imageList32.Images.Clear();
			imageList64.Images.Clear();

			// 全ゲーム数取得
			if (File.Exists(GameIni))
			{
				GameMax = Convert.ToInt32(ReadIni("list", "game", "0", 0));
				reloadButton.Enabled = true;
				editButton.Enabled = true;
				randomButton.Enabled = true;
				delButton.Enabled = true;
				memoButton.Enabled = true;
				statusCombo.Enabled = true;
			}
			else
			{
				// ゲーム統括管理INIがない場合
				reloadButton.Enabled = false;
				editButton.Enabled = false;
				randomButton.Enabled = false;
				delButton.Enabled = false;
				memoButton.Enabled = false;
				statusCombo.Enabled = false;
				return "_none_game_data";
			}

			if (!(GameMax >= 1)) // ゲーム登録数が1以上でない場合
			{
				reloadButton.Enabled = false;
			}


			int count;
			string readini;
			string ans = "";
			Image lvimg;
			ListViewItem lvi = new ListViewItem();

			toolStripProgressBar1.Minimum = 0;
			toolStripProgressBar1.Maximum = GameMax;

			if (IsFirstLoad)
			{
				UpdateSplashInfo(-1, "ゲームリストのロード中… [" + GameMax + "件]", enabledExSplash);
			}

			for (count = 1; count <= GameMax; count++)
			{
				if (IsFirstLoad && !DisableInitialLoadCountFlg)
				{
					UpdateSplashInfo(-1, "ゲームリストのロード中", enabledExSplash, count, GameMax);
				}

				toolStripProgressBar1.Value = count;
				// 読込iniファイル名更新
				readini = gameDirname + "\\" + count + ".ini";

				if (File.Exists(readini))
				{
					gameList.Items.Add(IniRead(readini, "game", KeyNames.name, string.Empty));

					if (GridEnable)
					{
						try
						{
							lvimg = Image.FromFile(IniRead(readini, "game", KeyNames.imgpass, string.Empty));
						}
						catch
						{
							lvimg = Properties.Resources.exe;
						}

						imageList8.Images.Add(count.ToString(), lvimg);
						imageList32.Images.Add(count.ToString(), lvimg);
						imageList64.Images.Add(count.ToString(), lvimg);

						lvi = new ListViewItem(IniRead(readini, "game", KeyNames.name, string.Empty));
						lvi.ImageIndex = (count - 1);
						gameImgList.Items.Add(lvi);
					}
				}
				else
				{
					// 個別ini存在しない場合
					ResolveError(MethodBase.GetCurrentMethod().Name, "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n[設定]→[ツール]→[【INI】連番修正]を実行してください。", 0, false, readini);
					this.ResumeLayout();
					break;
				}
			}

			// ゲームが登録されていれば1つ目を選択した状態にする
			if (GameMax >= 1)
			{
				try
				{
					gameList.SelectedIndex = 0;
				}
				catch (Exception ex)
				{
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false, "GameMax:" + GameMax.ToString());
					gameList.SelectedIndex = -1;
				}
			}

			if (IsFirstLoad)
			{
				// グリッド画像最大値の設定
				SetGridImgSizeChangeBar();
			}

			GC.Collect();
			Application.DoEvents();
			return ans;
		}

		/// <summary>
		/// データベースからゲーム一覧をロードします（MSSQL）
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
			imageList8.Images.Clear();
			imageList32.Images.Clear();
			imageList64.Images.Clear();

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
					CommandText = @"SELECT count(*) FROM " + DbName + "." + DbTable
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					GameMax = sqlAns;
					reloadButton.Enabled = true;
					editButton.Enabled = true;
					randomButton.Enabled = true;
					delButton.Enabled = true;
					memoButton.Enabled = true;
					statusCombo.Enabled = true;
				}
				else
				{
					// ゲームが1つもない場合
					GameMax = 0;
					reloadButton.Enabled = false;
					editButton.Enabled = false;
					randomButton.Enabled = false;
					delButton.Enabled = false;
					memoButton.Enabled = false;
					statusCombo.Enabled = false;
					return "_none_game_data";
				}

				if (IsFirstLoad)
				{
					UpdateSplashInfo(-1, "ゲームリストのロード中… [" + GameMax + "件]", enabledExSplash);
				}

				if (!(GameMax >= 1)) // ゲーム登録数が1以上でない場合
				{
					reloadButton.Enabled = false;
				}

				Image lvimg;
				ListViewItem lvi = new ListViewItem();

				toolStripProgressBar1.Minimum = 0;
				toolStripProgressBar1.Maximum = GameMax;

				toolStripProgressBar1.Value = 0;

				// DBからデータを取り出す
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, IMG_PATH, ROW_CNT "
								+ " FROM ( SELECT ID, GAME_NAME, IMG_PATH, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbName + "." + DbTable + ") AS T "
				};
				cm2.Connection = cn;

				int counter = 0;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						counter++;
						if (IsFirstLoad && !DisableInitialLoadCountFlg)
						{
							UpdateSplashInfo(-1, "ゲームリストのロード中", enabledExSplash, counter, GameMax);
						}

						gameList.Items.Add(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));

						if (GridEnable)
						{
							try
							{
								lvimg = Image.FromFile(DecodeSQLSpecialChars(reader["IMG_PATH"].ToString()));
							}
							catch
							{
								lvimg = Properties.Resources.exe;
							}

							imageList8.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
							imageList32.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
							imageList64.Images.Add(reader["ROW_CNT"].ToString(), lvimg);

							lvi = new ListViewItem(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));
							lvi.ImageIndex = (Convert.ToInt32(reader["ROW_CNT"]) - 1);
							gameImgList.Items.Add(lvi);
						}
					}
				}

				// ゲームが登録されていれば1つ目を選択した状態にする
				if (GameMax >= 1)
				{
					gameList.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
				errMessage = ex.Message;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			string backupDir = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";

			if (errMessage.Length != 0)
			{
				// エラーがあった場合
				if (OfflineSave && SaveType == "D")
				{
					if (File.Exists(LocalIni))
					{
						// オフラインモード使用可能の場合
						tabControl1.Controls.Remove(tabPage3);
						SaveType = "T";
						GameDir = LocalPath;
						GameIni = LocalIni;
						ans = LoadItem(LocalPath);
						if (firstLoad)
						{
							editINIStatusLabel.Visible = true;
							upButton.Visible = true;
							downButton.Visible = true;
							editButton.Visible = true;
							ResolveError(MethodBase.GetCurrentMethod().Name, "データベースに接続できませんでした。オフラインモードで起動します。\nこの問題が一時的なものであると考えられる場合、再起動で解決する場合があります。", 0, false, errMessage);
						}
					}
					else
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false, errMessage);
					}
				}
				else
				{
					ResolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false, errMessage);
				}
			}
			else
			{
				// エラーがなかったとき、初回ロード時のみオフラインモードのチェックを行う
				if (firstLoad)
				{
					// オフラインモードで変更がなかったかチェック
					if (ReadIni("list", "dbupdate", "0", 0, LocalPath) == "1")
					{
						DialogResult dr = MessageBox.Show("オフラインモード実行時に変更がありました。\nデータベースへアップロードしますか？\n\n接続先：" + DbUrl + " ▶ " + DbName + "." + DbTable + "\n\n※データベースのレコードを全削除し、オフラインのデータを登録します。\n\n[はい]	登録\n[いいえ]	破棄", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dr == DialogResult.Yes)
						{
							int tmpMaxGameCount, sCount, fCount;
							// データベース登録
							int returnVal = InsertIni2Db(LocalPath, backupDir, out tmpMaxGameCount, out sCount, out fCount);
							if (returnVal == 0)
							{
								MessageBox.Show("処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)\n\n", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							else
							{
								switch (returnVal)
								{
									case 1:
										// バックアップ作成エラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "バックアップの作成中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 2:
										// insertエラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "Insert中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 3:
									// Catchエラー
									case 4:
									// 復元エラー
									default:
										// 不明なエラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "致命的なエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
								}
							}
						}
						else
						{
							// 「いいえ」選択時、アップロードフラグを0にする
							WriteIni("list", "dbupdate", "0", 0, LocalPath);
						}
					}
				}
			}

			if (IsFirstLoad)
			{
				// グリッド画像最大値の設定
				SetGridImgSizeChangeBar();
			}

			GC.Collect();
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
			imageList8.Images.Clear();
			imageList32.Images.Clear();
			imageList64.Images.Clear();

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
					CommandText = @"SELECT count(*) FROM " + DbTable
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					GameMax = sqlAns;
					reloadButton.Enabled = true;
					editButton.Enabled = true;
					randomButton.Enabled = true;
					delButton.Enabled = true;
					memoButton.Enabled = true;
					statusCombo.Enabled = true;
				}
				else
				{
					// ゲームが1つもない場合
					GameMax = 0;
					reloadButton.Enabled = false;
					editButton.Enabled = false;
					randomButton.Enabled = false;
					delButton.Enabled = false;
					memoButton.Enabled = false;
					statusCombo.Enabled = false;
					return "_none_game_data";
				}

				if (IsFirstLoad)
				{
					UpdateSplashInfo(-1, "ゲームリストのロード中… [" + GameMax + "件]", enabledExSplash);
				}

				if (!(GameMax >= 1)) // ゲーム登録数が1以上でない場合
				{
					reloadButton.Enabled = false;
				}

				Image lvimg;
				ListViewItem lvi = new ListViewItem();

				toolStripProgressBar1.Minimum = 0;
				toolStripProgressBar1.Maximum = GameMax;

				toolStripProgressBar1.Value = 0;

				// DBからデータを取り出す
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, IMG_PATH, ROW_CNT "
								+ " FROM ( SELECT ID, GAME_NAME, IMG_PATH, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbTable + ") AS T "
				};
				cm2.Connection = cn;

				int counter = 0;

				using (var reader = cm2.ExecuteReader())
				{
					while (reader.Read() == true)
					{
						counter++;
						if (IsFirstLoad && !DisableInitialLoadCountFlg)
						{
							UpdateSplashInfo(-1, "ゲームリストのロード中", enabledExSplash, counter, GameMax);
						}

						gameList.Items.Add(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));

						if (GridEnable)
						{
							try
							{
								lvimg = Image.FromFile(DecodeSQLSpecialChars(reader["IMG_PATH"].ToString()));
							}
							catch
							{
								lvimg = Properties.Resources.exe;
							}

							imageList8.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
							imageList32.Images.Add(reader["ROW_CNT"].ToString(), lvimg);
							imageList64.Images.Add(reader["ROW_CNT"].ToString(), lvimg);

							lvi = new ListViewItem(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));
							lvi.ImageIndex = (Convert.ToInt32(reader["ROW_CNT"]) - 1);
							gameImgList.Items.Add(lvi);
						}
					}
				}

				// ゲームが登録されていれば1つ目を選択した状態にする
				if (GameMax >= 1)
				{
					gameList.SelectedIndex = 0;
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
				errMessage = ex.Message;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}

			string backupDir = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "DbBackup\\";

			if (errMessage.Length != 0)
			{
				// エラーがあった場合
				if (OfflineSave && SaveType == "M")
				{
					if (File.Exists(LocalIni))
					{
						// オフラインモード使用可能の場合
						tabControl1.Controls.Remove(tabPage3);
						SaveType = "T";
						GameDir = LocalPath;
						GameIni = LocalIni;
						ans = LoadItem(LocalPath);
						if (firstLoad)
						{
							editINIStatusLabel.Visible = true;
							upButton.Visible = true;
							downButton.Visible = true;
							editButton.Visible = true;
							ResolveError(MethodBase.GetCurrentMethod().Name, "データベースに接続できませんでした。オフラインモードで起動します。\nこの問題が一時的なものであると考えられる場合、再起動で解決する場合があります。", 0, false, errMessage);
						}
					}
					else
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false, errMessage);
					}
				}
				else
				{
					ResolveError(MethodBase.GetCurrentMethod().Name, errMessage, 0, false, errMessage);
				}
			}
			else
			{
				// エラーがなかったとき、初回ロード時のみオフラインモードのチェックを行う
				if (firstLoad)
				{
					// オフラインモードで変更がなかったかチェック
					if (ReadIni("list", "dbupdate", "0", 0, LocalPath) == "1")
					{
						DialogResult dr = MessageBox.Show("オフラインモード実行時に変更がありました。\nデータベースへアップロードしますか？\n\n接続先：" + DbUrl + ":" + DbPort + " ▶ " + DbName + "." + DbTable + "\n\n※データベースのレコードを全削除し、オフラインのデータを登録します。\n\n[はい]	登録\n[いいえ]	変更を破棄", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
						if (dr == DialogResult.Yes)
						{
							int tmpMaxGameCount, sCount, fCount;
							// データベース登録
							int returnVal = InsertIni2Db(LocalPath, backupDir, out tmpMaxGameCount, out sCount, out fCount);
							if (returnVal == 0)
							{
								MessageBox.Show("処理が完了しました。(全: " + tmpMaxGameCount + "件 / 成功: " + sCount + "件 / 失敗: " + fCount + "件)\n\n", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							else
							{
								switch (returnVal)
								{
									case 1:
										// バックアップ作成エラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "バックアップの作成中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 2:
										// insertエラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "Insert中にエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
									case 3:
									// Catchエラー
									case 4:
									// 復元エラー
									default:
										// 不明なエラー
										ResolveError(MethodBase.GetCurrentMethod().Name, "致命的なエラーが発生しました。\n詳細はエラーログをご覧下さい。", 0, false);
										break;
								}
							}
						}
						else
						{
							// 「いいえ」選択時、アップロードフラグを0にする
							WriteIni("list", "dbupdate", "0", 0, LocalPath);
						}
					}
				}
			}

			if (IsFirstLoad)
			{
				// グリッド画像最大値の設定
				SetGridImgSizeChangeBar();
			}

			GC.Collect();
			Application.DoEvents();
			return ans;
		}

		/// <summary>
		/// グリッドの画像サイズ変更バーの最大値を設定します。
		/// </summary>
		private void SetGridImgSizeChangeBar()
		{
			if (GridEnable && FixGridSizeFlg)
			{
				trackBar1.Visible = false;
				gameImgList.Height = 420;

				switch (FixGridSize)
				{
					case 8:
						gameImgList.LargeImageList = imageList8;
						imageList32.Images.Clear();
						imageList64.Images.Clear();
						imageList32.Dispose();
						imageList64.Dispose();
						break;
					case 32:
						gameImgList.LargeImageList = imageList32;
						imageList8.Images.Clear();
						imageList64.Images.Clear();
						imageList8.Dispose();
						imageList64.Dispose();
						break;
					case 64:
						gameImgList.LargeImageList = imageList64;
						imageList8.Images.Clear();
						imageList32.Images.Clear();
						imageList8.Dispose();
						imageList32.Dispose();
						break;
				}
			}
		}

		/// <summary>
		/// 実行ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StartButton_Click(object sender, EventArgs e)
		{
			int startdata, timedata, ratinginfo;
			bool sucExit = true;
			string executeAppPath = exePathText.Text.Trim();
			string executeAppArg = executeCmdText.Text.Trim();

			if (gameList.SelectedIndex == -1)
			{
				ResolveError(MethodBase.GetCurrentMethod().Name, "ゲームリストが空です。", 0, false);
				return;
			}
			string selectedtext = gameList.SelectedItem.ToString();

			// 共通部分を保存
			string[] columnNames = { "track", "sens" };
			string[] data = { (Convert.ToInt32(trackCheck.Checked)).ToString(), (Convert.ToInt32(sensCheck.Checked)).ToString() };
			string[] failedVal = { "0", "0" };

			IniEditCheck(ConfigIni, "checkbox", columnNames, data, failedVal);

			if (SaveType == "I" || SaveType == "T")
			{
				// iniの場合、ステータス状態を書き込み
				// Discordカスタムステータス、各種チェックボックス、ラジオの保存
				string path = GameDir + (gameList.SelectedIndex + 1) + ".ini";
				if (File.Exists(path))
				{
					KeyNames[] keyNames = { KeyNames.stat, KeyNames.dcon_img, KeyNames.rating };
					data = new string[] { (dconText.Text.Trim()), (dconImgText.Text.Trim()), (normalRadio.Checked ? "0" : "1") };
					failedVal = new string[] { string.Empty, string.Empty, "0" };

					IniEditCheck(path, "game", keyNames, data, failedVal);

					// 更新前に選択していたゲームへ移動
					if (gameList.Items.Contains(selectedtext))
					{
						gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
					}
				}
				else
				{
					// 個別ini不存在
					ResolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報管理iniが存在しません。\n" + path, 0, false);
				}
			}

			if (File.Exists(exePathText.Text))
			{
				// トラッキングあり
				if (trackCheck.Checked)
				{
					// 通常トラッキング実行
					if (!testCheck.Checked)
					{
						// 現在時刻取得
						string strTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
						DateTime starttime = Convert.ToDateTime(strTime);

						// 実行
						Process drunp = null;
						if (useDconCheck.Checked)
						{
							if (File.Exists(DconPath))
							{
								// propertiesファイル書き込み
								string propertiesfile = Path.GetDirectoryName(DconPath) + "\\run.properties";
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
									writer.WriteLine("title = " + nameText.Text + "\nappid = " + (DconAppID.Length != 0 ? DconAppID : string.Empty) + "\nappicon = " + dconImgText.Text.Trim() + "\nrating = " + ratinginfo + "\nstat = " + dconText.Text.Trim());
								}

								writer.Close();

								drunp = Process.Start(DconPath); // dcon実行
								if (drunp == null)
								{
									string dconArgs = "-jar " + DconPath;
									var startInfo = new ProcessStartInfo("javaw.exe", dconArgs);
									drunp = Process.Start(startInfo);
								}
							}
							else
							{
								ResolveError(MethodBase.GetCurrentMethod().Name, "Discord Connectorが見つかりません。\n実行を中断します。", 0, false);
								return;
							}
						}

						// 既定ディレクトリの取得
						string originAppPath = Path.GetDirectoryName(executeAppPath);

						// 抽出パス構成
						if (extractCheck.Checked)
						{
							if (!GenerateExtractCmd(CurrentExtractTool, executeAppPath, executeAppArg, out executeAppPath, out executeAppArg))
							{
								MessageBox.Show("抽出ツールが選択されていません！", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}

						// 既定ディレクトリの変更
						string newAppPath = Path.GetDirectoryName(executeAppPath);
						Environment.CurrentDirectory = JudgeCurrentDir(CurrentExtractTool, originAppPath, newAppPath);

						// 起動中gifの可視化
						runningPicture.Visible = true;
						startButton.Enabled = false;

						// ウィンドウ最小化
						this.WindowState = FormWindowState.Minimized;

						Application.DoEvents();

						// ゲーム実行
						Process p = Process.Start(executeAppPath, executeAppArg);

						// 棒読み上げ
						if (ByActive && ByRoS)
						{
							Bouyomiage(nameText.Text + "を、トラッキングありで起動しました。");
						}

						// ゲーム終了まで待機
						p.WaitForExit();

						// 終了時刻取得
						string time = (sucExit ? p.ExitTime : DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
						DateTime endtime = Convert.ToDateTime(time);

						// 作業ディレクトリ復元
						Environment.CurrentDirectory = BaseDir;

						// ウィンドウ通常表示化
						this.WindowState = FormWindowState.Normal;

						// 子プロセスの終了
						if (useDconCheck.Checked)
						{
							sucExit = KillChildProcess(drunp);
						}

						// 起動時間計算
						string temp = (endtime - starttime).ToString();
						int anss = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);

						if (ByActive && ByRoS)
						{
							Bouyomiage("ゲームを終了しました。起動時間は、約" + anss + "秒です。");
						}

						if (anss < 15)
						{
							// 15秒以内に終了した場合、確認を行う
							DialogResult dr = MessageBox.Show("起動時間が短いようです。\n今回：" + anss + "秒 ｜ しきい値：15秒\n\n今回の記録を破棄しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
							if (dr == DialogResult.Yes)
							{
								// 時間更新を行わずに終了
								runningPicture.Visible = false;
								startButton.Enabled = true;
								return;
							}
						}

						if (SaveType == "I" || SaveType == "T")
						{
							// ini
							int selecteditem = gameList.SelectedIndex + 1;
							string readini = GameDir + selecteditem + ".ini";

							if (File.Exists(readini))
							{
								// 既存値の取得
								KeyNames[] keyNames = { KeyNames.time, KeyNames.start };
								string[] failedVals = { "0", "0" };
								string[] returnVals = IniRead(readini, "game", keyNames, failedVals);

								timedata = Convert.ToInt32(returnVals[0]);
								startdata = Convert.ToInt32(returnVals[1]);

								// 起動時間オーバーフローチェック
								if ((timedata + anss) >= Int32.MaxValue)
								{
									timedata = Int32.MaxValue;
								}
								else
								{
									// 起動時間加算
									timedata += anss;
								}
								// 起動回数オーバーフローチェック
								if (!((startdata + 1) >= Int32.MaxValue))
								{
									// 起動回数加算
									startdata++;
								}

								// 値書き換え
								string[] insertData = { timedata.ToString(), startdata.ToString() };
								IniWrite(readini, "game", keyNames, insertData);

								// 次回DB接続時に更新するフラグを立てる
								if (SaveType == "T")
								{
									WriteIni("list", "dbupdate", "1", 0);
								}
							}
							else
							{
								// 個別ini存在しない場合
								ResolveError(MethodBase.GetCurrentMethod().Name, "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。", 0, false);
							}
						}
						else
						{
							// database
							if (SaveType == "D")
							{
								// SQL文構築
								SqlConnection cn = SqlCon;
								SqlTransaction tr = null;
								SqlCommand cm = new SqlCommand
								{
									CommandType = CommandType.Text,
									CommandTimeout = 30,
									CommandText = @"UPDATE " + DbName + "." + DbTable + " SET UPTIME = CAST(CAST(UPTIME AS BIGINT) + @uptime AS NVARCHAR), RUN_COUNT = CAST(CAST(RUN_COUNT AS INT) + 1 AS NVARCHAR), DCON_TEXT = @dcon_text, DCON_IMG = @dcon_img, AGE_FLG = @age_flg, LAST_RUN = @last_run, STATUS = (CASE STATUS WHEN N'" + DefaultStatusValueOfNotPlaying + "' THEN N'" + DefaultStatusValueOfPlaying + "' ELSE STATUS END) "
																			+ " WHERE ID = @current_game_db_val"
								};
								cm.Connection = cn;
								cm.Parameters.AddWithValue("@uptime", anss);
								cm.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(dconText.Text.Trim()));
								cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
								cm.Parameters.AddWithValue("@age_flg", normalRadio.Checked ? "0" : "1");
								cm.Parameters.AddWithValue("@last_run", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
								cm.Parameters.AddWithValue("@current_game_db_val", CurrentGameDbVal);
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
									WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
									ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。\nSQLエディタを所持していない場合、[設定] > [ツール] > [SQLエディタ] からSQLを実行できます。", 0, false);
								}
								finally
								{
									if (cn.State == ConnectionState.Open)
									{
										cn.Close();
									}
								}
							}
							else if (SaveType == "M")
							{
								// SQL文構築
								MySqlConnection cn = SqlCon2;
								MySqlTransaction tr = null;
								MySqlCommand cm = new MySqlCommand
								{
									CommandType = CommandType.Text,
									CommandTimeout = 30,
									CommandText = @"UPDATE " + DbTable + " SET UPTIME = CAST(CAST(UPTIME AS SIGNED) + @uptime AS NCHAR), RUN_COUNT = CAST(CAST(RUN_COUNT AS SIGNED) + 1 AS NCHAR), DCON_TEXT = @dcon_text, DCON_IMG = @dcon_img, AGE_FLG = @age_flg, LAST_RUN = @last_run, STATUS = (CASE STATUS WHEN N'" + DefaultStatusValueOfNotPlaying + "' THEN N'" + DefaultStatusValueOfPlaying + "' ELSE STATUS END) "
																			+ " WHERE ID = @current_game_db_val;"
								};
								cm.Connection = cn;
								cm.Parameters.AddWithValue("@uptime", anss);
								cm.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(dconText.Text.Trim()));
								cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
								cm.Parameters.AddWithValue("@age_flg", normalRadio.Checked ? "0" : "1");
								cm.Parameters.AddWithValue("@last_run", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
								cm.Parameters.AddWithValue("@current_game_db_val", CurrentGameDbVal);

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
									WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
									ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。\nSQLエディタを所持していない場合、[設定] > [ツール] > [SQLエディタ] からSQLを実行できます。", 0, false);
								}
								finally
								{
									if (cn.State == ConnectionState.Open)
									{
										cn.Close();
									}
								}
							}

							GameList_SelectedIndexChanged(sender, e);
						}
						runningPicture.Visible = false;
						startButton.Enabled = true;

						if (SaveType == "D" || SaveType == "M")
						{
							if (searchResultList.Items.Count > 0)
							{
								// 再検索
								SearchExec(true);
							}
						}
					}
					// テスト起動モード
					else
					{
						// checkBox6.Checked
						MessageBox.Show("テスト起動モードが有効です。\nこのモードでは起動時間、起動回数、DiscordRPCなどは実行されません。\n\n無効にするには、[テスト起動]チェックを外してください。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);

						// 既定ディレクトリの取得
						string originAppPath = Path.GetDirectoryName(executeAppPath);

						// 抽出パス構成
						if (extractCheck.Checked)
						{
							if (!GenerateExtractCmd(CurrentExtractTool, executeAppPath, executeAppArg, out executeAppPath, out executeAppArg))
							{
								MessageBox.Show("抽出ツールが選択されていません！", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
								return;
							}
						}

						// 既定ディレクトリの変更
						string newAppPath = Path.GetDirectoryName(executeAppPath);
						Environment.CurrentDirectory = JudgeCurrentDir(CurrentExtractTool, originAppPath, newAppPath);

						// 起動開始日時を取得
						DateTime startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));

						// 起動中gifの可視化
						runningPicture.Visible = true;
						startButton.Enabled = false;

						// ウィンドウ最小化
						this.WindowState = FormWindowState.Minimized;

						// ゲーム実行
						Process p = Process.Start(executeAppPath, executeAppArg);

						// 棒読み上げ
						if (ByActive && ByRoS)
						{
							Bouyomiage(nameText.Text + "を、テストモードで起動しました。");
						}

						// ゲーム終了まで待機
						p.WaitForExit();

						// 起動終了日時を取得
						DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
						string temp = (endTime - startTime).ToString();
						int wakeUpTimes = Convert.ToInt32(TimeSpan.Parse(temp).TotalSeconds);


						// 作業ディレクトリ復元
						Environment.CurrentDirectory = BaseDir;

						// 起動中gifの非可視化
						runningPicture.Visible = false;
						startButton.Enabled = true;

						// ウィンドウ通常表示化
						this.WindowState = FormWindowState.Normal;

						// 終了検出後
						DialogResult dr = MessageBox.Show("実行終了を検出しました。\n開始日時：" + startTime.ToString("yyyy/MM/dd HH:mm:ss") + "\n終了日時：" + endTime.ToString("yyyy/MM/dd HH:mm:ss") + "\n実行時間：" + wakeUpTimes + "\n\n正しくトラッキングできていますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
						if (dr == DialogResult.Yes)
						{
							MessageBox.Show("正常にトラッキングできています。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							MessageBox.Show("トラッキングに失敗しています。\n以下をご確認ください。\n\n・ランチャーを指定していませんか？\n・ランチャーを管理者権限で起動してみてください。\n・実行パスを英数字のみにしてみてください。\n\nそれでも解決しない場合は、GitHubでIssueを開いてファイルパスやゲームエンジン等を教えてください。可能な限り対応します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}

					}
				}
				// トラッキングなしで実行
				else
				{
					// 既定ディレクトリの取得
					string originAppPath = Path.GetDirectoryName(executeAppPath);

					// 抽出パス構成
					if (extractCheck.Checked)
					{
						if (!GenerateExtractCmd(CurrentExtractTool, executeAppPath, executeAppArg, out executeAppPath, out executeAppArg))
						{
							MessageBox.Show("抽出ツールが選択されていません！", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					// 既定ディレクトリの変更
					string newAppPath = Path.GetDirectoryName(executeAppPath);
					Environment.CurrentDirectory = JudgeCurrentDir(CurrentExtractTool, originAppPath, newAppPath);

					if (ByActive && ByRoS)
					{
						Bouyomiage(nameText.Text + "を、トラッキングなしで起動しました。");
					}
					Process.Start(executeAppPath, executeAppArg);

					Environment.CurrentDirectory = BaseDir;
				}
			}
			GC.Collect();
		}

		/// <summary>
		/// ゲーム起動時のカレントディレクトリを選択・返却します。
		/// </summary>
		/// <param name="CurrentExtractTool">抽出ドロップダウンの値</param>
		/// <param name="originAppPath">ゲームのディレクトリパス</param>
		/// <param name="newAppPath">抽出ツールのディレクトリパス</param>
		/// <returns></returns>
		private string JudgeCurrentDir(int CurrentExtractTool, string originAppPath, string newAppPath)
		{
			string resultCurDirPath = originAppPath;
			if (newAppPath.Length == 0)
			{
				return resultCurDirPath;
			}

			switch (CurrentExtractTool)
			{
				case 1: // krkr
					if (ExtractKrkrCurDir)
					{
						resultCurDirPath = newAppPath;
					}
					break;
				case 2: // krkrz
					if (ExtractKrkrzCurDir)
					{
						resultCurDirPath = newAppPath;
					}
					break;
				case 3: // krkrDump
					if (ExtractKrkrDumpCurDir)
					{
						resultCurDirPath = newAppPath;
					}
					break;
				case 4: // Custom1
					if (ExtractCustom1CurDir)
					{
						resultCurDirPath = newAppPath;
					}
					break;
				case 5: // Custom2
					if (ExtractCustom2CurDir)
					{
						resultCurDirPath = newAppPath;
					}
					break;
			}
			return resultCurDirPath;
		}

		/// <summary>
		/// 追加ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddButton_Click(object sender, EventArgs e)
		{
			if (SaveType == "D")
			{
				SqlConnection cn = SqlCon;

				AddItem addItem = new AddItem(SaveType, cn);
				addItem.StartPosition = FormStartPosition.CenterParent;
				addItem.ShowDialog(this);
			}
			else if (SaveType == "M")
			{
				MySqlConnection cn = SqlCon2;

				AddItem addItem = new AddItem(SaveType, cn);
				addItem.StartPosition = FormStartPosition.CenterParent;
				addItem.ShowDialog(this);
			}
			else
			{
				AddItem addItem = new AddItem(SaveType);
				addItem.StartPosition = FormStartPosition.CenterParent;
				addItem.ShowDialog(this);
			}

			ReloadItems();
			GC.Collect();
		}

		/// <summary>
		/// 情報ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void InfoButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show(AppName + " Ver." + AppVer + " / Build " + AppBuild + "\n\n" + "現在の作業ディレクトリ [" + (SaveType == "I" ? "ローカルINI" : SaveType == "D" ? "SQL Server" : SaveType == "M" ? "MySQL" : "オフラインINI") + "]：\n" + ((SaveType == "D" || SaveType == "M") ? DbUrl + ":" + DbPort + " ▶ " + DbName + "." + DbTable : GameDir),
								AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Information);
		}

		/// <summary>
		/// ゲームリスト変更時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(GameMax >= 1) || !(gameList.SelectedIndex >= 0))
			{
				return;
			}

			if (GridEnable)
			{
				// グリッドと同期
				gameImgList.Items[gameList.SelectedIndex].Selected = true;
				gameImgList.EnsureVisible(gameList.SelectedIndex);
			}

			// ゲーム詳細取得
			int selecteditem = gameList.SelectedIndex + 1;
			string readini = GameDir + selecteditem + ".ini";
			string id = null, namedata = null, imgpassdata = null, passdata = null, execute_cmd = null, stimedata = null, startdata = null, cmtdata = null, dcon_imgdata = null, rating = null, status = null;

			if (SaveType == "I" || SaveType == "T")
			{
				// ini
				if (File.Exists(readini))
				{
					// ini読込開始
					KeyNames[] keyNames = { KeyNames.name, KeyNames.pass, KeyNames.imgpass, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.rating, KeyNames.status, KeyNames.execute_cmd, KeyNames.extract_tool };
					string[] failedVal = { string.Empty, string.Empty, string.Empty, "0", "0", string.Empty, string.Empty, Rate.ToString(), string.Empty, string.Empty, "0" };
					string[] returnVals = IniRead(readini, "game", keyNames, failedVal);

					namedata = returnVals[0].ToString();
					passdata = returnVals[1].ToString();
					imgpassdata = returnVals[2].ToString();
					stimedata = returnVals[3].ToString();
					startdata = returnVals[4].ToString();
					cmtdata = returnVals[5].ToString();
					dcon_imgdata = returnVals[6].ToString();
					rating = returnVals[7].ToString();
					status = returnVals[8].ToString();
					execute_cmd = returnVals[9].ToString();
					CurrentExtractTool = Convert.ToInt32(returnVals[10].ToString());
				}
				else
				{
					// 個別ini存在しない場合
					ResolveError(MethodBase.GetCurrentMethod().Name, "iniファイル読み込み中にエラー。\nファイルが存在しません。\n\n処理を中断します。\n予期せぬ不具合の発生につながる場合があります。\n直ちに終了することをお勧めしますが、このまま実行することもできます。", 4, false, readini);
				}
			}
			else
			{
				// database
				// MSSQL
				if (SaveType == "D")
				{
					SqlConnection cn = SqlCon;
					SqlCommand cm;

					if (selecteditem.ToString().Length != 0)
					{
						cm = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, STATUS, EXECUTE_CMD, EXTRACT_TOOL "
											+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbName + "." + DbTable + ") AS T "
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
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, STATUS, EXECUTE_CMD, EXTRACT_TOOL "
											+ "FROM " + DbName + "." + DbTable
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
							namedata = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
							imgpassdata = DecodeSQLSpecialChars(reader["IMG_PATH"].ToString());
							passdata = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
							execute_cmd = DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString());
							stimedata = reader["UPTIME"].ToString();
							startdata = reader["RUN_COUNT"].ToString();
							cmtdata = DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString());
							dcon_imgdata = reader["DCON_IMG"].ToString();
							rating = reader["AGE_FLG"].ToString();
							status = reader["STATUS"].ToString();
							CurrentExtractTool = Convert.ToInt32(reader["EXTRACT_TOOL"].ToString());
						}

					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
					MySqlConnection cn = SqlCon2;
					MySqlCommand cm;

					if (selecteditem.ToString().Length != 0)
					{
						cm = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, STATUS, EXECUTE_CMD, EXTRACT_TOOL "
											+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbTable + ") AS T "
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
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, STATUS, EXECUTE_CMD, EXTRACT_TOOL "
											+ "FROM " + DbTable
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
							namedata = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
							imgpassdata = DecodeSQLSpecialChars(reader["IMG_PATH"].ToString());
							execute_cmd = DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString());
							passdata = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
							stimedata = reader["UPTIME"].ToString();
							startdata = reader["RUN_COUNT"].ToString();
							cmtdata = DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString());
							dcon_imgdata = reader["DCON_IMG"].ToString();
							rating = reader["AGE_FLG"].ToString();
							status = reader["STATUS"].ToString();
							CurrentExtractTool = Convert.ToInt32(reader["EXTRACT_TOOL"].ToString());
						}

					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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

			titleLabel.Text = namedata;
			nameText.Text = namedata;
			exePathText.Text = passdata;
			imgPathText.Text = imgpassdata;
			executeCmdText.Text = execute_cmd;
			runTimeText.Text = timedata.ToString();
			startCountText.Text = startdata;
			dconText.Text = cmtdata;
			dconImgText.Text = dcon_imgdata;
			statusCombo.SelectedItem = status;
			CurrentGameDbVal = id;
			toolStripStatusLabel1.Text = "[" + selecteditem + "/" + GameMax + "]";
			toolStripProgressBar1.Value = gameList.SelectedIndex + 1;

			bool startButtonEnable = true;

			if (string.IsNullOrEmpty(execute_cmd))
			{
				// 引数テキストボックス非表示
				executeCmdText.Visible = false;
				exePathText.Width = 381;
			}
			else
			{
				// 表示
				executeCmdText.Visible = true;
				exePathText.Width = 299;
			}

			if (Convert.ToInt32(rating) == 0)
			{
				normalRadio.Checked = true;
			}
			else if (Convert.ToInt32(rating) == 1)
			{
				ratedRadio.Checked = true;
			}

			if (!File.Exists(passdata))
			{
				// アプリケーションが存在しない場合、実行できないようにする
				startButtonEnable = false;
				startButton.Text = "ゲームパスが存在しません";
			}

			if (File.Exists(imgpassdata))
			{
				try
				{
					gameIcon.Image = null;
					gameIcon.ImageLocation = imgpassdata;
				}
				catch (Exception ex)
				{
					gameIcon.ImageLocation = "";
					gameIcon.Image = Resources.exe;
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, imgpassdata);
				}
			}
			else
			{
				gameIcon.ImageLocation = "";
				gameIcon.Image = Resources.exe;
			}

			if ((Convert.ToInt32(runTimeText.Text) >= 35791394) || (Convert.ToInt32(startCountText.Text) >= Int32.MaxValue))
			{
				// 最大の場合、実行できないようにする
				startButtonEnable = false;
				startButton.Text = "記録上限のため起動不可";
			}

			startButton.Enabled = startButtonEnable;

			if (startButtonEnable)
			{
				startButton.Text = "実行";
			}

			return;
		}

		/// <summary>
		/// ゲームタイトルをコピーします
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CopySelectedGameTitle(object sender, EventArgs e)
		{
			if (titleLabel.Text.Length > 0)
			{
				try
				{
					Clipboard.SetText(titleLabel.Text);
					System.Media.SystemSounds.Asterisk.Play();
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, titleLabel.Text);
					System.Media.SystemSounds.Exclamation.Play();
				}
			}
		}

		/// <summary>
		/// 再読込ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ReloadButton_Click(object sender, EventArgs e)
		{
			ReloadItems();
			return;
		}

		/// <summary>
		/// ゲームタイトルをコピーします
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameTitleCopyPictureBox_Click(object sender, EventArgs e)
		{
			if (!(nameText.Text.Equals("")))
			{
				Clipboard.SetText(nameText.Text);
			}
		}

		/// <summary>
		/// ゲームパスをコピーします
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GamePathCopyPictureBox_Click(object sender, EventArgs e)
		{
			if (!(exePathText.Text.Equals("")))
			{
				Clipboard.SetText(exePathText.Text);
			}
		}

		/// <summary>
		/// ゲーム画像パスをコピーします
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameImgCopyPictureBox_Click(object sender, EventArgs e)
		{
			if (!(imgPathText.Text.Equals("")))
			{
				Clipboard.SetText(imgPathText.Text);
			}
		}

		/// <summary>
		/// ゲーム起動時間を表示します
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameRunningTimePictureBox_Click(object sender, EventArgs e)
		{
			if (!(runTimeText.Text.Equals("")))
			{
				int total = Convert.ToInt32(runTimeText.Text);
				string hour = (total / 60).ToString();
				string min = (total % 60).ToString();
				int sTotal = (Convert.ToInt32(startCountText.Text) == 0 ? 1 : Convert.ToInt32(startCountText.Text));
				string sMin = (total / sTotal).ToString();
				string sHour = (Convert.ToInt32(sMin) / 60).ToString();
				sMin = (Convert.ToInt32(sMin) % 60).ToString();

				DialogResult result = MessageBox.Show(nameText.Text +
										"\n統計起動時間：" + hour + "時間" + min + "分" +
										"\n平均起動時間：" + sHour + "時間" + sMin + "分/回",
										AppName,
										MessageBoxButtons.OK,
										MessageBoxIcon.Information);
			}
		}

		/// <summary>
		/// 引数とINIファイルのデータ一致チェック（相違があればINIファイルを上書き）
		/// </summary>
		/// <param name="ininame">調べるINIファイルパス</param>
		/// <param name="sec">セクション</param>
		/// <param name="key">キー</param>
		/// <param name="data">データ</param>
		/// <param name="failedval">失敗時の値</param>
		private void IniEditCheck(string ininame, string sec, string[] keyNames, string[] data, string[] failedVal)
		{
			runningPicture.Visible = true;

			if (File.Exists(ininame))
			{
				string[] resultData = IniRead(ininame, sec, keyNames, failedVal);

				for (int i = 0; i < keyNames.Length; i++)
				{
					// 取得値とデータが違う場合
					if (!(resultData[i].Equals(data[i])))
					{
						IniWrite(ininame, sec, keyNames[i], data[i].ToString());
					}
				}
			}
			else
			{
				ResolveError(MethodBase.GetCurrentMethod().Name, "該当するファイルがありません。\n\n[Error]\n" + ininame, 0, false, ininame);
			}

			runningPicture.Visible = false;
			return;
		}

		/// <summary>
		/// 引数とINIファイルのデータ一致チェック（相違があればINIファイルを上書き）
		/// </summary>
		/// <param name="ininame">調べるINIファイルパス</param>
		/// <param name="sec">セクション</param>
		/// <param name="key">キー</param>
		/// <param name="data">データ</param>
		/// <param name="failedval">失敗時の値</param>
		private void IniEditCheck(string ininame, string sec, KeyNames[] keyNames, string[] data, string[] failedVal)
		{
			runningPicture.Visible = true;

			if (File.Exists(ininame))
			{
				string[] resultData = IniRead(ininame, sec, keyNames, failedVal);

				for (int i = 0; i < keyNames.Length; i++)
				{
					// 取得値とデータが違う場合
					if (!(resultData[i].Equals(data[i])))
					{
						IniWrite(ininame, sec, keyNames[i], data[i].ToString());
					}
				}
			}
			else
			{
				ResolveError(MethodBase.GetCurrentMethod().Name, "該当するファイルがありません。\n\n[Error]\n" + ininame, 0, false, ininame);
			}

			runningPicture.Visible = false;
			return;
		}

		/// <summary>
		/// コンフィグファイルのロードと画面反映を行います
		/// </summary>
		private void UpdateComponent()
		{
			if (GLConfigLoad() == false)
			{
				ResolveError(MethodBase.GetCurrentMethod().Name, "Configロード中にエラー。\n詳しくはエラーログを参照して下さい。", 0, false);
			}

			if (File.Exists(ConfigIni))
			{
				int ckv0 = Convert.ToInt32(ReadIni("checkbox", "track", "0"));

				string bgimg = BgImg;
				sensCheck.Checked = Convert.ToBoolean(Convert.ToInt32(ReadIni("checkbox", "sens", "0")));
				useDconCheck.Checked = Dconnect;
				if (useDconCheck.Checked)
				{
					sensCheck.Visible = true;
				}
				else
				{
					sensCheck.Visible = false;
				}

				if (ExtractEnable)
				{
					extractCheck.Visible = true;
				}
				else
				{
					extractCheck.Visible = false;
				}

				if (Rate == 1)
				{
					ratedRadio.Checked = true;
				}
				else
				{
					normalRadio.Checked = true;
				}

				if (ByActive)
				{
					if (ByRoW)
					{
						Bouyomi_Connectchk();
					}
				}

				if (File.Exists(bgimg))
				{
					this.BackgroundImage = new Bitmap(bgimg);
					this.BackgroundImageLayout = ImageLayout.Stretch;
				}
				else
				{
					this.BackgroundImage = null;
					Message();
				}

				trackCheck.Checked = Convert.ToBoolean(ckv0);

				if (trackCheck.Checked)
				{
					dconTextPictureBox.Visible = true;
					dconText.Visible = true;
					dconImgPictureBox.Visible = true;
					dconImgText.Visible = true;

					dconConnectGroupBox.Visible = true;

					testCheck.Visible = true;
					if (useDconCheck.Checked)
					{
						sensCheck.Visible = true;
					}
				}
				else
				{
					dconTextPictureBox.Visible = false;
					dconText.Visible = false;
					dconImgPictureBox.Visible = false;
					dconImgText.Visible = false;

					dconConnectGroupBox.Visible = false;

					testCheck.Visible = false;
					sensCheck.Visible = false;
				}

				// データベース使用時の部品非表示処理
				if (SaveType == "D" || SaveType == "M")
				{
					// databaseの場合
					upButton.Visible = false;
					downButton.Visible = false;
					editINIStatusLabel.Visible = false;
				}
				else
				{
					// iniの場合
					tabControl1.Controls.Remove(tabPage3);
				}
			}
			else
			{
				// ini存在しない場合
				File.Create(ConfigIni).Close();
				GameDir = BaseDir + "Data";
				GameIni = GameDir + "game.ini";
				trackCheck.Checked = true;
				tabControl1.TabPages.Remove(tabPage3);
			}
			return;
		}

		/// <summary>
		/// 起動時のアップデートチェック
		/// </summary>
		private void CheckItemUpdate()
		{
			DialogResult dr = new DialogResult();
			if (SaveType == "D")
			{
				// MSSQL
				DBUpdate DBUpdateForm = new DBUpdate(SaveType, SqlCon);
				DBUpdateForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
				dr = DBUpdateForm.ShowDialog(this);
			}
			else if (SaveType == "M")
			{
				// MySQL
				DBUpdate DBUpdateForm = new DBUpdate(SaveType, SqlCon2);
				DBUpdateForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
				dr = DBUpdateForm.ShowDialog(this);
			}
			else
			{
				// INI
				DBUpdate DBUpdateForm = new DBUpdate(SaveType);
				DBUpdateForm.StartPosition = FormStartPosition.WindowsDefaultLocation;
				dr = DBUpdateForm.ShowDialog(this);
			}

			// チェック結果別処理
			// Cancel:必須アップデートしない（終了）、No:任意アップデートしない、OK:アップデート完了、Ignore:アップデートなし
			if (dr == DialogResult.Cancel)
			{
				ExitApp(false);
			}
		}

		/// <summary>
		/// ゲームリストの再読込
		/// </summary>
		private void ReloadItems()
		{
			UpdateComponent();

			if (SaveType == "I" || SaveType == "T")
			{
				// ini
				if (File.Exists(GameIni))
				{
					// ini読込開始
					GameMax = Convert.ToInt32(ReadIni("list", "game", "0", 0));
				}
				else
				{
					return;
				}

				LoadItem(GameDir);
				if (gameList.Items.Count != 0)
				{
					string selectedtext = gameList.SelectedItem.ToString();
					if (gameList.Items.Contains(selectedtext))
					{
						gameList.SelectedIndex = gameList.Items.IndexOf(selectedtext);
					}
				}
			}
			else
			{
				// database
				if (SaveType == "D")
				{
					LoadItem2(SqlCon);
				}
				else
				{
					LoadItem3(SqlCon2);
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

		/// <summary>
		/// ランダム選択ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RandomButton_Click(object sender, EventArgs e)
		{
			int rawdata = GameMax;

			if (rawdata >= 1)
			{
				// 乱数生成
				Random r = new Random();
				int ans = r.Next(1, rawdata + 1);

				gameList.SelectedIndex = ans - 1;

				if (GridEnable)
				{
					// グリッドと同期
					gameImgList.Items[gameList.SelectedIndex].Selected = true;
					gameImgList.EnsureVisible(gameList.SelectedIndex);
				}
			}
			else
			{
				MessageBox.Show("登録ゲーム数が少ないため、ランダム選択できません！", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			GC.Collect();
		}

		/// <summary>
		/// フォーム終了処理
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Application_ApplicationExit(object sender, EventArgs e)
		{
			ExitApp();
		}

		/// <summary>
		/// 削除ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DelButton_Click(object sender, EventArgs e)
		{
			if (gameList.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			// 選択中のゲーム保管ファイルを削除
			runningPicture.Visible = true;
			int delval = gameList.SelectedIndex + 1;
			string delname = nameText.Text;
			if (SaveType == "I" || SaveType == "T")
			{
				// ini
				DelIniItem(delval, delname);

				// 次回DB接続時に更新するフラグを立てる
				if (SaveType == "T")
				{
					IniWrite(GameIni, "list", "dbupdate", "1");
				}
			}
			else
			{
				// database
				DelDbItem(delval);
			}

			runningPicture.Visible = false;
			return;
		}

		/// <summary>
		/// 編集ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditButton_Click(object sender, EventArgs e)
		{
			string selectedListCount = (gameList.SelectedIndex + 1).ToString();

			if (!(gameList.SelectedIndex >= 0))
			{
				MessageBox.Show("ゲームリストが空です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (SaveType == "D")
			{
				// MSSQLの場合
				SqlConnection cn = SqlCon;
				SqlCommand cm;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.GAME_PATH, T.EXECUTE_CMD, T.IMG_PATH, T.UPTIME, T.RUN_COUNT, T.DCON_TEXT, T.DCON_IMG, T.AGE_FLG, T.ROW_CNT, T.EXTRACT_TOOL, T.SAVEDATA_PATH "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbName + "." + DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Editor form5 = new Editor(SaveType, selectedListCount, cn, cm);
				form5.StartPosition = FormStartPosition.CenterParent;
				form5.ShowDialog(this);
				if (!string.IsNullOrEmpty(form5.newGameName))
				{
					gameList.Items[gameList.SelectedIndex] = form5.newGameName;
					if (GridEnable)
					{
						gameImgList.Items[gameList.SelectedIndex].Text = form5.newGameName;
					}
				}
			}
			else if (SaveType == "M")
			{
				// MySQLの場合
				MySqlConnection cn = SqlCon2;
				MySqlCommand cm;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.GAME_PATH, T.EXECUTE_CMD, T.IMG_PATH, T.UPTIME, T.RUN_COUNT, T.DCON_TEXT, T.DCON_IMG, T.AGE_FLG, T.ROW_CNT, T.EXTRACT_TOOL, T.SAVEDATA_PATH "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Editor form5 = new Editor(SaveType, selectedListCount, cn, cm);
				form5.StartPosition = FormStartPosition.CenterParent;
				form5.ShowDialog(this);
				if (!string.IsNullOrEmpty(form5.newGameName))
				{
					gameList.Items[gameList.SelectedIndex] = form5.newGameName;
					if (GridEnable)
					{
						gameImgList.Items[gameList.SelectedIndex].Text = form5.newGameName;
					}
				}
			}
			else
			{
				// iniの場合
				string path = GameDir + (gameList.SelectedIndex + 1) + ".ini";
				if (File.Exists(path))
				{
					Editor form5 = new Editor(SaveType, selectedListCount, new SqlConnection(), new SqlCommand());
					form5.StartPosition = FormStartPosition.CenterParent;
					form5.ShowDialog(this);
					if (!string.IsNullOrEmpty(form5.newGameName))
					{
						gameList.Items[gameList.SelectedIndex] = form5.newGameName;
						if (GridEnable)
						{
							gameImgList.Items[gameList.SelectedIndex].Text = form5.newGameName;
						}
					}
				}
				else
				{
					// 個別ini不存在
					ResolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報管理iniが存在しません。\n" + path, 0, false);
				}
			}

			GameList_SelectedIndexChanged(sender, e);
			if ((SaveType == "D" || SaveType == "M") && searchResultList.Items.Count > 0)
			{
				SearchExec(true);
			}
			GC.Collect();
			return;
		}

		/// <summary>
		/// メモボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MemoButton_Click(object sender, EventArgs e)
		{
			string selectedListCount = (gameList.SelectedIndex + 1).ToString();

			if (gameList.SelectedIndex == -1)
			{
				MessageBox.Show("ゲームリストが空です。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (SaveType == "D")
			{
				SqlConnection cn = SqlCon;
				SqlCommand cm;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.IMG_PATH, T.MEMO, T.AGE_FLG, T.ROW_CNT "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbName + "." + DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Memo memoForm = new Memo(SaveType, selectedListCount, cn, cm);
				memoForm.StartPosition = FormStartPosition.CenterParent;
				memoForm.ShowDialog(this);
				GameList_SelectedIndexChanged(sender, e);
				return;
			}
			else if (SaveType == "M")
			{
				MySqlConnection cn = SqlCon2;
				MySqlCommand cm;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT T.ID, T.GAME_NAME, T.IMG_PATH, T.MEMO, T.AGE_FLG, T.ROW_CNT "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROW_CNT FROM " + DbTable + ") AS T "
									+ "WHERE T.ROW_CNT = " + selectedListCount
				};
				cm.Connection = cn;

				Memo memoForm = new Memo(SaveType, selectedListCount, cn, cm);
				memoForm.StartPosition = FormStartPosition.CenterParent;
				memoForm.ShowDialog(this);
				GameList_SelectedIndexChanged(sender, e);
				return;
			}

			// INIの場合
			string path = GameDir + (gameList.SelectedIndex + 1) + ".ini";
			if (File.Exists(path))
			{
				Memo memoForm = new Memo(SaveType, selectedListCount, new SqlConnection(), new SqlCommand());
				memoForm.StartPosition = FormStartPosition.CenterParent;
				memoForm.ShowDialog(this);
				GameList_SelectedIndexChanged(sender, e);
			}
			else
			{
				// 個別ini不存在
				ResolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報管理iniが存在しません。\n" + path, 0, false);
			}

			return;
		}

		/// <summary>
		/// ゲーム管理データ編集ステータスラベルイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EditINIStatusLabel_Click(object sender, EventArgs e)
		{
			if (SaveType == "D" || SaveType == "M")
			{
				// 現段階ではDBに登録されている情報の変更は不可（※将来対応予定）
				return;
			}

			// ini読込
			string rawdata = GameDir + ((gameList.SelectedIndex + 1).ToString()) + ".ini";

			if (File.Exists(rawdata))
			{
				Process.Start(GameDir + (gameList.SelectedIndex + 1) + ".ini");
			}
			else
			{
				ResolveError(MethodBase.GetCurrentMethod().Name, "ゲーム情報管理iniがありません！\n" + rawdata, 0, false);
			}
		}

		/// <summary>
		/// 上に移動ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UpButton_Click(object sender, EventArgs e)
		{
			if (SaveType == "I" || SaveType == "T")
			{
				// ini
				int selected = gameList.SelectedIndex;
				if (selected >= 1)
				{
					selected++;
					int target = selected - 1;
					string before = GameDir + (selected.ToString()) + ".ini";
					string temp = GameDir + (target.ToString()) + "_.ini";
					string after = GameDir + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (SaveType == "T")
						{
							IniWrite(GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, "移動先もしくは移動前のファイルが見つかりません。\n\n移動前：" + before + "\n移動先：" + after, 0, false);
					}
				}
				else
				{
					MessageBox.Show("最上位です。\nこれ以上は上に移動できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(GameDir);
				gameList.SelectedIndex = selected - 2;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return;
		}

		/// <summary>
		/// 下に移動ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DownButton_Click(object sender, EventArgs e)
		{
			if (SaveType == "I" || SaveType == "T")
			{
				// ini
				int selected = gameList.SelectedIndex;
				if (selected + 1 < GameMax)
				{
					selected++;
					int target = selected + 1;
					string before = GameDir + (selected.ToString()) + ".ini";
					string temp = GameDir + (target.ToString()) + "_.ini";
					string after = GameDir + (target.ToString()) + ".ini";
					if (File.Exists(before) && File.Exists(after))
					{
						File.Move(after, temp);
						File.Move(before, after);
						File.Move(temp, before);

						// 次回DB接続時に更新するフラグを立てる
						if (SaveType == "T")
						{
							IniWrite(GameIni, "list", "dbupdate", "1");
						}
					}
					else
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, "移動先もしくは移動前のファイルが見つかりません。\nファイルに影響はありません。\n\n移動前：" + before + "\n移動先：" + after, 0, false);
						return;
					}
				}
				else
				{
					MessageBox.Show("最下位です。\nこれ以上は下に移動できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				LoadItem(GameDir);
				gameList.SelectedIndex = selected--;
			}
			else
			{
				// database
				MessageBox.Show("データベースを使用しているため移動できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			return;
		}

		/// <summary>
		/// ゲームパスをエクスプローラーで起動します
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GamePathOpenPictureBox_Click(object sender, EventArgs e)
		{
			if (exePathText.Text != "")
			{
				string opendir = Path.GetDirectoryName(exePathText.Text);
				if (Directory.Exists(opendir))
				{
					Process.Start(opendir);
				}
			}
		}

		/// <summary>
		/// ゲーム画像パスをエクスプローラーで起動します
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameImgOpenPictureBox_Click(object sender, EventArgs e)
		{
			if (exePathText.Text != "")
			{
				string opendir = Path.GetDirectoryName(exePathText.Text);
				if (Directory.Exists(opendir))
				{
					Process.Start(opendir);
				}
			}
		}

		/// <summary>
		/// イメージグリッド変更時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameImgList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!(GameMax >= 1))
			{
				return;
			}

			if (gameImgList.SelectedItems.Count <= 0)
			{
				return;
			}

			// リストと同期
			gameList.SelectedIndex = Convert.ToInt32(gameImgList.SelectedItems[0].Index);

			return;

		}

		/// <summary>
		/// グリッドの画像サイズ変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TrackBar1_Scroll(object sender, EventArgs e)
		{
			// ImageSizeを変更する
			switch (trackBar1.Value)
			{
				case 0:
					gameImgList.LargeImageList = imageList8;
					break;
				case 1:
					gameImgList.LargeImageList = imageList32;
					break;
				case 2:
					gameImgList.LargeImageList = imageList64;
					break;
			}

			GC.Collect();
		}

		/// <summary>
		/// 設定ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ConfigButton_Click(object sender, EventArgs e)
		{
			// 設定
			string beforeWorkDir = BaseDir;
			string beforeSaveType = SaveType;
			bool beforeGridEnabled = GridEnable;
			bool beforeWindowMinimunEnabled = WindowHideControlFlg;

			ConfigForm.StartPosition = FormStartPosition.CenterParent;
			ConfigForm.ShowDialog(this);
			UpdateComponent();

			string afterWorkDir = BaseDir;
			string afterSaveType = SaveType;
			bool afterGridEnabled = GridEnable;
			bool afterWindowMinimunEnabled = WindowHideControlFlg;

			if (beforeWorkDir != afterWorkDir)
			{
				MessageBox.Show("既定の作業ディレクトリが変更されました。\nGLauncherを再起動してください。\n\nOKを押してGLauncherを終了します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				ExitApp(!OfflineSave);
			}
			else if (beforeSaveType != afterSaveType)
			{
				// WriteIni("disable", "updchkVer", "0.0");
				MessageBox.Show("データの保存方法が変更されました。\nGLauncherを再起動してください。\n\nOKを押してGLauncherを終了します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				ExitApp(!OfflineSave);
			}
			else if ((beforeGridEnabled != afterGridEnabled) || (beforeWindowMinimunEnabled != afterWindowMinimunEnabled))
			{
				MessageBox.Show("UIに関する設定が変更されました。\nGLauncherを再起動してください。\n\nOKを押してGLauncherを終了します。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				ExitApp(!OfflineSave);
			}

			GC.Collect();
			return;
		}

		/// <summary>
		/// ステータスバーに表示されるメッセージを選択
		/// </summary>
		private void Message(string message = null)
		{
			string ans = "";
			int tmp;
			Random r = new Random();
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

			if (IsFirstLoad && !string.IsNullOrEmpty(message))
			{
				ans = message;
			}
			else if (IsFirstLoad && toolStripStatusLabel2.Text.Trim().Length != 0)
			{
				// 初回起動時はメッセージを更新しない
				return;
			}

			toolStripStatusLabel2.Text = ans;
			return;
		}

		/// <summary>
		/// [INI] 選択データ削除
		/// </summary>
		/// <param name="delfileval"></param>
		/// <param name="delfilename"></param>
		private void DelIniItem(int delfileval, string delfilename)
		{
			int nextval;
			string nextfile;
			int delval = delfileval;
			string delfile = (GameDir + delval + ".ini");

			if (File.Exists(delfile))
			{
				// 削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("選択中のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n[" + delfilename + "]\n" + delfile + "\n削除しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					File.Delete(delfile);
					nextval = delval + 1;
					nextfile = (GameDir + nextval + ".ini");
					while (File.Exists(nextfile))
					{
						// 削除ファイル以降にゲームが存在する場合に番号を下げる
						File.Move(nextfile, delfile);
						delfile = nextfile;
						nextval++;
						nextfile = (GameDir + nextval + ".ini");
					}
					GameMax--;
					IniWrite(GameIni, "list", "game", GameMax.ToString());
				}
			}
			else
			{
				// 削除ファイル不存在
				ResolveError(MethodBase.GetCurrentMethod().Name, "削除対象のiniファイルが存在しません。\n" + delfile, 0, false, delfile);
			}
			LoadItem(GameDir);
			if (GameMax >= 2 && delfileval >= 2)
			{
				gameList.SelectedIndex = delfileval - 2;
			}
			else if (GameMax == 1 && delfileval >= 1)
			{
				gameList.SelectedIndex = 1;
			}
			else
			{
				gameList.SelectedIndex = 0;
			}
			return;
		}

		/// <summary>
		/// [DB] 選択データ削除
		/// </summary>
		/// <param name="delItemVal"></param>
		private void DelDbItem(int delItemVal)
		{
			string delName = string.Empty;
			string delPath = string.Empty;

			if (SaveType == "D")
			{
				SqlConnection cn = SqlCon;
				SqlCommand cm;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, GAME_PATH "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbName + "." + DbTable + ") AS T "
									+ "WHERE ROWCNT = " + delItemVal
				};
				cm.Connection = cn;

				try
				{
					cn.Open();
					var reader = cm.ExecuteReader();

					if (reader.Read())
					{
						delName = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
						delPath = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}

				// 削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("次のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n\n[" + delName + "]\n" + delPath + "\n\n削除しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					// 削除
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DELETE " + "glt "
										+ "FROM " + DbName + "." + DbTable + " glt "
										+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbName + "." + DbTable + ") tmp "
										+ "ON glt.ID = tmp.ID "
										+ "WHERE ROWCNT = " + delItemVal
					};
					cm.Connection = cn;
					try
					{
						toolStripStatusLabel2.Text = "＊＊削除実行中…";
						cn.Open();
						cm.ExecuteNonQuery();
						GameMax--;
					}
					catch (Exception ex)
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false, cm.CommandText);
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

				LoadItem2(SqlCon);
			}
			else
			{
				MySqlConnection cn = SqlCon2;
				MySqlCommand cm;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT ID, GAME_NAME, GAME_PATH "
									+ "FROM ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbTable + ") AS T "
									+ "WHERE ROWCNT = " + delItemVal
				};
				cm.Connection = cn;

				try
				{
					cn.Open();
					var reader = cm.ExecuteReader();

					if (reader.Read())
					{
						delName = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
						delPath = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
					}
				}
				catch (Exception ex)
				{
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false, cm.CommandText);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}

				// 削除ファイル存在
				DialogResult dialogResult = MessageBox.Show("次のゲームをランチャーの一覧から削除します。\n※この操作は元に戻せません。\n\n[" + delName + "]\n" + delPath + "\n\n削除しますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (dialogResult == DialogResult.Yes)
				{
					// 削除
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"DELETE " + "glt "
										+ "FROM " + DbTable + " glt "
										+ "INNER JOIN ( SELECT *, ROW_NUMBER() OVER (ORDER BY ID) AS ROWCNT FROM " + DbTable + ") tmp "
										+ "ON glt.ID = tmp.ID "
										+ "WHERE ROWCNT = " + delItemVal + ";"
					};
					cm.Connection = cn;
					try
					{
						toolStripStatusLabel2.Text = "＊＊削除実行中…";
						cn.Open();
						cm.ExecuteNonQuery();
						GameMax--;
					}
					catch (Exception ex)
					{
						ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false, cm.CommandText);
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

				LoadItem3(SqlCon2);
			}

			if (GameMax >= 2 && delItemVal >= 2)
			{
				gameList.SelectedIndex = delItemVal - 2;
			}
			else if (GameMax == 1 && delItemVal >= 1)
			{
				gameList.SelectedIndex = 1;
			}
			else
			{
				gameList.SelectedIndex = 0;
			}
			return;
		}

		/// <summary>
		/// 子プロセスを終了します
		/// </summary>
		/// <param name="process">プロセス</param>
		bool KillChildProcess(Process process)
		{
			try
			{
				if (process != null)
				{
					process.Kill();
					return true;
				}
				else
				{
					throw new Exception();
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "processName:" + process.ProcessName + " (" + process.HasExited + ") / exitTime:" + process.ExitTime);
				MessageBox.Show("既にdcon.jarが終了しています。\nDiscord RPCが正しく動作しなかった可能性があります。\n\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
		}

		/// <summary>
		/// エラーダイアログを表示します。
		/// </summary>
		/// <param name="methodName">関数名</param>
		/// <param name="errorMsg">エラーメッセージ</param>
		/// <param name="dialogType">0：OK、4：Yes/No、5：再試行、キャンセル</param>
		/// <param name="forceClose">true：ダイアログ処理後に終了する、false：終了しないで戻る</param>
		/// <param name="addInfo">追加情報</param>
		/// <returns><paramref name="dialogType">dialogType</paramref>に対応するDialogResult</returns>
		public DialogResult ResolveError(string methodName, string errorMsg, int dialogType, bool forceClose = true, string addInfo = "")
		{
			DialogResult dr = new DialogResult();

			// エラー内容記述
			WriteErrorLog(errorMsg, methodName, addInfo);

			if (addInfo.Length > 0)
			{
				errorMsg += "\n\n[追加情報]\n" + addInfo;
			}

			// エラーダイアログ表示
			switch (dialogType)
			{
				case 0:
				default:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;

				case 4:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Error);
					break;

				case 5:
					dr = MessageBox.Show("エラーが発生しました。 [" + methodName + "]\n\n" + errorMsg, AppName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
					break;
			}

			if (forceClose)
			{
				ExitApp();
			}

			return dr;
		}

		/// <summary>
		/// アプリケーションを安全に終了する
		/// </summary>
		public void ExitApp(bool downloadDbEnabled = true)
		{
			Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);

			if ((SaveType == "D" || SaveType == "M") && OfflineSave && downloadDbEnabled)
			{
				notifyIcon1.BalloonTipText = "オフラインデータの取得中です。しばらくお待ち下さい。";
				notifyIcon1.ShowBalloonTip(10);
				string localPath = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";
				DownloadDbDataToLocal(localPath);
			}

			if (ByActive && ByRoW)
			{
				Bouyomiage("ゲームランチャーを終了しました。");
			}

			GC.Collect();
			this.Dispose();
			Application.Exit();
		}

		/// <summary>
		/// メモリ解放ボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CleanButton_Click(object sender, EventArgs e)
		{
			long beforeMemory = Environment.WorkingSet;
			GC.Collect();
			long afterMemory = Environment.WorkingSet;
			long diffMemory = beforeMemory - afterMemory;
			MessageBox.Show("メモリを解放しました。\n" + beforeMemory + "byte -> " + afterMemory + "byte (" + diffMemory + "byte)", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		/// <summary>
		/// アコーディオンボタン
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OcButton_Click(object sender, EventArgs e)
		{
			if (GridMax)
			{
				// 現在最大表示の場合、最小表示にする
				tabControl1.Width = 345;

				gameList.Width = 337;

				gameImgList.Width = 337;
				trackBar1.Width = 337;

				searchText.Width = 142;
				orderDropDown.Location = new Point(224, 1);
				searchButton.Location = new Point(277, 0);

				searchingText.Width = 197;
				searchResultList.Width = 337;
				lastOrderDrop.Location = new Point(284, 35);

				ocButton.Text = ">>";
				GridMax = false;
			}
			else
			{
				// 最小表示の場合、最大表示にする
				tabControl1.Width = 840;

				gameList.Width = 832;

				gameImgList.Width = 832;
				trackBar1.Width = 832;

				searchText.Width = 637;
				orderDropDown.Location = new Point(720, 1);
				searchButton.Location = new Point(773, 0);

				searchingText.Width = 623;
				searchResultList.Width = 832;
				lastOrderDrop.Location = new Point(711, 35);

				ocButton.Text = "<<";
				GridMax = true;
			}

			if (GridEnable)
			{
				// グリッドと同期
				gameImgList.EnsureVisible(gameList.SelectedIndex);
			}
		}

		/// <summary>
		/// ゲーム画像 クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GameIcon_Click(object sender, EventArgs e)
		{
			if (File.Exists(gameIcon.ImageLocation))
			{
				IconView IconViewForm = new IconView(gameIcon.ImageLocation);
				IconViewForm.StartPosition = FormStartPosition.CenterParent;
				IconViewForm.ShowDialog(this);
			}
		}

		/// <summary>
		/// 検索ボタン クリックイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchButton_Click(object sender, EventArgs e)
		{
			SearchExec(false);
		}

		/// <summary>
		/// 検索テキスト KeyPressイベント（検索）
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchText_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Enter押下で検索を実行
			if (e.KeyChar == (char)Keys.Enter)
			{
				SearchButton_Click(sender, e);
			}
		}

		/// <summary>
		/// 検索画面更新
		/// </summary>
		/// <param name="reSearch">再検索フラグ</param>
		private void SearchExec(bool reSearch = false)
		{

			if (GameMax <= 0)
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
				case 5:
					// メモ
					searchOption = "MEMO";
					break;
			}

			// 検索条件表示
			searchingText.Text = searchName;
			lastSearchOption.SelectedIndex = reSearch ? lastSearchOption.SelectedIndex : searchTargetDropDown.SelectedIndex;
			lastOrderDrop.SelectedIndex = reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex;

			// 検索処理
			searchResultList.Items.Clear();

			if (SaveType == "D")
			{
				SqlConnection cn = SqlCon;

				try
				{
					toolStripStatusLabel2.Text = "＊＊検索実行中…";
					Application.DoEvents();
					// 接続オープン
					cn.Open();

					// 検索に一致するゲーム数取得
					SqlCommand cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + DbName + "." + DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
					};
					cm.Connection = cn;
					cm.Parameters.AddWithValue("@search_name", "%" + searchName + "%");

					int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

					if (sqlAns <= 0)
					{
						// ゲームが1つもない場合
						return;
					}

					// DBからデータを取り出す
					SqlCommand cm2 = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, MEMO FROM " + DbName + "." + DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
										+ " ORDER BY " + searchOption + ((reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex) == 0 ? " ASC" : " DESC")
					};
					cm2.Connection = cn;
					cm2.Parameters.AddWithValue("@search_name", "%" + searchName + "%");

					using (var reader = cm2.ExecuteReader())
					{
						while (reader.Read() == true)
						{
							searchResultList.Items.Add(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));
						}
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
			else if (SaveType == "M")
			{
				MySqlConnection cn = SqlCon2;

				try
				{
					toolStripStatusLabel2.Text = "＊＊検索実行中…";
					Application.DoEvents();
					// 接続オープン
					cn.Open();

					// 検索に一致するゲーム数取得
					MySqlCommand cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
					};
					cm.Connection = cn;
					cm.Parameters.AddWithValue("@search_name", "%" + searchName + "%");

					int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

					if (sqlAns <= 0)
					{
						// ゲームが1つもない場合
						return;
					}

					// DBからデータを取り出す
					MySqlCommand cm2 = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, DCON_IMG, AGE_FLG, MEMO FROM " + DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
										+ " ORDER BY " + searchOption + ((reSearch ? lastOrderDrop.SelectedIndex : orderDropDown.SelectedIndex) == 0 ? " ASC" : " DESC")
					};
					cm2.Connection = cn;
					cm2.Parameters.AddWithValue("@search_name", "%" + searchName + "%");

					using (var reader = cm2.ExecuteReader())
					{
						while (reader.Read() == true)
						{
							searchResultList.Items.Add(DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()));
						}
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
			GC.Collect();
			return;
		}

		/// <summary>
		/// 検索リストボックス 選択アイテム変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchResultList_SelectedIndexChanged(object sender, EventArgs e)
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
				case 5:
					// メモ
					searchOption = "MEMO";
					break;
			}

			// ゲーム詳細取得
			int selecteditem = searchResultList.SelectedIndex + 1;

			if (SaveType == "I" || SaveType == "T")
			{
				// iniでは検索は実行できないのでリターン
				return;
			}
			else if (SaveType == "D")
			{
				// 選択されたアイテムの検索を行う
				SqlConnection cn = SqlCon;
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
						 + DbName + "." + DbTable + " AS MAIN "
						 + "	LEFT OUTER JOIN ( "
						 + "		SELECT "
						 + "			[T].[ID], [T].[GAME_PATH], [T].[IMG_PATH], [T].[DCON_TEXT], [T].[DCON_IMG], [T].[UPTIME], [T].[RUN_COUNT], [T].[AGE_FLG], [T].[LAST_RUN], [T].[MEMO], [T].[STATUS], ROW_NUMBER() over (ORDER BY [T].[ID]) AS [ROW1], [T2].[ROW2] "
						 + "		FROM "
						 + DbName + "." + DbTable + " AS [T] "
						 + "		LEFT OUTER JOIN ( "
						 + "			SELECT "
						 + "				[ID], ROW_NUMBER() over (ORDER BY " + searchOption + " " + (lastOrderDrop.SelectedIndex == 0 ? "ASC" : "DESC") + ") AS [ROW2] "
						 + "			FROM "
						 + DbName + "." + DbTable
						 + (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
						 + "		) AS [T2] "
						 + " ON [T].[ID] = [T2].[ID] "
						 + ") AS SUB "
						 + " ON [MAIN].[ID] = [SUB].[ID] "
						 + " WHERE [SUB].[ROW2] = @selected_item"
						 + " ORDER BY [SUB].[ROW2] ASC "
					};
					cm.Connection = cn;
					cm.Parameters.AddWithValue("@search_name", "%" + searchName + "%");
					cm.Parameters.AddWithValue("@selected_item", selecteditem);
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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
				MySqlConnection cn = SqlCon2;
				MySqlCommand cm;

				if (selecteditem.ToString().Length != 0)
				{
					cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @" SELECT "
										+ " main.ID"
										+ ",GAME_NAME"
										+ ",GAME_PATH"
										+ ",IMG_PATH"
										+ ",UPTIME"
										+ ",RUN_COUNT"
										+ ",DCON_TEXT"
										+ ",DCON_IMG"
										+ ",AGE_FLG"
										+ ",MEMO"
										+ ",sub.ROW1"
										+ ",sub2.ROW2"
										+ " FROM "
										+ DbTable + " main"
										+ " LEFT OUTER JOIN "
										+ "("
										+ "		SELECT "
										+ "			 ID"
										+ "			,ROW_NUMBER() over (ORDER BY ID ASC) AS ROW1"
										+ "		FROM "
										+ DbTable
										+ ") AS sub"
										+ " ON "
										+ "		main.ID = sub.ID "
										+ " LEFT OUTER JOIN "
										+ "("
										+ "		SELECT "
										+ "			 ID"
										+ "			,ROW_NUMBER() over (ORDER BY " + searchOption + " " + (lastOrderDrop.SelectedIndex == 0 ? " ASC" : " DESC") + ") AS ROW2"
										+ "		FROM "
										+ DbTable
										+ (searchOption == "LAST_RUN" ? "" : " WHERE " + searchOption + " LIKE @search_name")
										+ ") AS sub2"
										+ " ON "
										+ "		main.ID = sub2.ID "
										+ " WHERE "
										+ (searchOption == "LAST_RUN" ? "" : searchOption + " LIKE @search_name AND")
										+ " ROW2 = @selected_item"
										+ " ORDER BY "
										+ searchOption + (lastOrderDrop.SelectedIndex == 0 ? " ASC" : " DESC")
					};
					cm.Connection = cn;
					cm.Parameters.AddWithValue("@search_name", "%" + searchName + "%");
					cm.Parameters.AddWithValue("@selected_item", searchResultList.SelectedIndex + 1);
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message, 0, false);
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

			if (GridEnable)
			{
				// 他のグリッドと同期
				gameImgList.Items[rowCount].Selected = true;
				gameImgList.EnsureVisible(rowCount);
			}
			else
			{
				// リストと同期
				gameList.SelectedIndex = rowCount;
			}

			return;
		}

		/// <summary>
		/// 検索条件ドロップダウン 変更イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SearchTargetDropDown_SelectedIndexChanged(object sender, EventArgs e)
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
					orderDropDown.SelectedIndex = 1;
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
				case 5:
					// メモ
					searchText.Enabled = true;
					break;
			}
		}

		/// <summary>
		/// ステータスコンボボックス チェンジイベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StatusCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			// アイテムがない場合はリターン
			if (gameList.Items.Count == 0 || !statusCombo.Focused)
			{
				return;
			}

			// database
			// MSSQL
			if (SaveType == "D")
			{
				// SQL文構築
				SqlConnection cn = SqlCon;
				SqlTransaction tr = null;
				SqlCommand cm = new SqlCommand
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET STATUS = '" + statusCombo.SelectedItem.ToString() + "'"
								+ " WHERE ID = '" + CurrentGameDbVal + "'"
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。", 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}
			// MySQL
			else if (SaveType == "M")
			{
				// SQL文構築
				MySqlConnection cn = SqlCon2;
				MySqlTransaction tr = null;
				MySqlCommand cm = new MySqlCommand
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbTable + " SET STATUS = '" + statusCombo.SelectedItem.ToString() + "'"
								+ " WHERE ID = '" + CurrentGameDbVal + "'"
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					ResolveError(MethodBase.GetCurrentMethod().Name, ex.Message + "\n\nエラーログに記載されているSQL文を手動で実行すると更新できます。", 0, false);
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
			}
			// INI
			else
			{
				string readini = GameDir + (gameList.SelectedIndex + 1) + ".ini";
				IniWrite(readini, "game", "status", statusCombo.SelectedItem.ToString());
			}
		}

		/// <summary>
		/// トラッキングチェックボックス変更時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TrackCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (trackCheck.Focused)
			{
				try
				{
					WriteIni("checkbox", "track", (Convert.ToInt32(trackCheck.Checked)).ToString());
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, Convert.ToInt32(trackCheck.Checked).ToString());
				}

				if (trackCheck.Checked)
				{
					dconTextPictureBox.Visible = true;
					dconText.Visible = true;
					dconImgPictureBox.Visible = true;
					dconImgText.Visible = true;

					dconConnectGroupBox.Visible = true;

					testCheck.Visible = true;
					if (useDconCheck.Checked)
					{
						sensCheck.Visible = true;
					}
				}
				else
				{
					dconTextPictureBox.Visible = false;
					dconText.Visible = false;
					dconImgPictureBox.Visible = false;
					dconImgText.Visible = false;

					dconConnectGroupBox.Visible = false;

					testCheck.Visible = false;
					sensCheck.Visible = false;
				}
			}
		}

		/// <summary>
		/// DiscordConnectorチェックボックス変更時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UseDconCheck_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				WriteIni("checkbox", "dconnect", (Convert.ToInt32(useDconCheck.Checked)).ToString());
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, Convert.ToInt32(useDconCheck.Checked).ToString());
				System.Media.SystemSounds.Hand.Play();
			}

			if (useDconCheck.Checked)
			{
				sensCheck.Visible = true;
			}
			else
			{
				sensCheck.Visible = false;
			}
		}

		/// <summary>
		/// センシティブモードチェックボックス変更時イベント
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SensCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (sensCheck.Focused)
			{
				try
				{
					WriteIni("checkbox", "sens", (Convert.ToInt32(sensCheck.Checked)).ToString());
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, Convert.ToInt32(sensCheck.Checked).ToString());
					System.Media.SystemSounds.Hand.Play();
				}
			}
		}

		/// <summary>
		/// 抽出モード有効チェック
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExtractCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (extractCheck.Checked)
			{
				MessageBox.Show("いくつかのツールは起動直後に自己タスクを終了するため、\n正常にトラッキングできない場合があります。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}
	}
}
