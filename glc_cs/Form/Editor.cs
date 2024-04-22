using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static glc_cs.Core.Functions;
using static glc_cs.Core.Property;

namespace glc_cs
{
	public partial class Editor : Form
	{
		DLsite dlSearchForm = new DLsite();
		vndb vndbSearchForm = new vndb();

		SqlConnection con = new SqlConnection();
		MySqlConnection con2 = new MySqlConnection();
		string genSaveType = string.Empty;
		string iniPath = string.Empty;
		public string newGameName = string.Empty;

		/// <summary>
		/// MSSQL用
		/// </summary>
		/// <param name="saveType">ゲームデータ保存方法</param>
		/// <param name="selectedListCount">選択中のリストアイテムID</param>
		/// <param name="cn">SQLコネクション</param>
		/// <param name="cm">SQLコマンド</param>
		public Editor(string saveType, string selectedListCount, SqlConnection cn, SqlCommand cm)
		{
			InitializeComponent();
			// 数値ボックス最大値設定
			runTimeText.Maximum = Int32.MaxValue;
			startCountText.Maximum = Int32.MaxValue;
			if (ExtractEnable)
			{
				label10.Visible = true;
				extractToolCombo.Visible = true;
				extractToolCombo.SelectedIndex = 0;
			}

			// モードチェックボックス反映
			switch (saveType)
			{
				case "I":
					checkBox1.Checked = true;
					break;
				case "D":
					checkBox2.Checked = true;
					break;
				case "T":
					checkBox3.Checked = true;
					break;
			}

			con = cn;
			genSaveType = saveType;

			// データ検索と画面反映
			if (saveType == "D")
			{
				searchExec(selectedListCount, cn, cm);
			}
			else
			{
				getExec(selectedListCount);
			}
		}

		/// <summary>
		/// MySQL用
		/// </summary>
		/// <param name="saveType">ゲームデータ保存方法</param>
		/// <param name="selectedListCount">選択中のリストアイテムID</param>
		/// <param name="cn">SQLコネクション</param>
		/// <param name="cm">SQLコマンド</param>
		public Editor(string saveType, string selectedListCount, MySqlConnection cn, MySqlCommand cm)
		{
			InitializeComponent();
			// 数値ボックス最大値設定
			runTimeText.Maximum = Int32.MaxValue;
			startCountText.Maximum = Int32.MaxValue;
			if (ExtractEnable)
			{
				label10.Visible = true;
				extractToolCombo.Visible = true;
				extractToolCombo.SelectedIndex = 0;
			}

			// モードチェックボックス反映
			switch (saveType)
			{
				case "I":
					checkBox1.Checked = true;
					break;
				case "M":
					checkBox2.Checked = true;
					break;
				case "T":
					checkBox3.Checked = true;
					break;
			}

			con2 = cn;
			genSaveType = saveType;

			// データ検索と画面反映
			if (saveType == "M")
			{
				searchExec(selectedListCount, cn, cm);
			}
			else
			{
				getExec(selectedListCount);
			}
		}


		private void searchExec(String selectedListCount, SqlConnection cn, SqlCommand cm)
		{
			// 検索実行と画面反映
			try
			{
				cn.Open();
				var reader = cm.ExecuteReader();

				if (reader.Read())
				{
					label9.Text = reader["ID"].ToString();
					titleText.Text = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
					gameTitleLabel.Text = titleText.Text;
					executeCmdText.Text = DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString());
					imgPathText.Text = DecodeSQLSpecialChars(reader["IMG_PATH"].ToString());
					exePathText.Text = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
					runTimeText.Value = Convert.ToInt32(reader["UPTIME"]);
					startCountText.Value = Convert.ToInt32(reader["RUN_COUNT"]);
					dconText.Text = DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString());
					dconImgText.Text = reader["DCON_IMG"].ToString();
					rateCheck.Checked = reader["AGE_FLG"].ToString() == "1" ? true : false;
					extractToolCombo.SelectedIndex = Convert.ToInt32(DecodeSQLSpecialChars(reader["EXTRACT_TOOL"].ToString()));
					savePathText.Text = DecodeSQLSpecialChars(reader["SAVEDATA_PATH"].ToString());
					if (File.Exists(imgPathText.Text))
					{
						iconImage.ImageLocation = imgPathText.Text;
					}
					else
					{
						iconImage.ImageLocation = string.Empty;
					}
				}

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}
		}

		private void searchExec(String selectedListCount, MySqlConnection cn, MySqlCommand cm)
		{
			// 検索実行と画面反映
			try
			{
				cn.Open();
				var reader = cm.ExecuteReader();

				if (reader.Read())
				{
					label9.Text = reader["ID"].ToString();
					titleText.Text = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
					gameTitleLabel.Text = titleText.Text;
					executeCmdText.Text = DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString());
					imgPathText.Text = DecodeSQLSpecialChars(reader["IMG_PATH"].ToString());
					exePathText.Text = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
					runTimeText.Value = Convert.ToInt32(reader["UPTIME"]);
					startCountText.Value = Convert.ToInt32(reader["RUN_COUNT"]);
					dconText.Text = DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString());
					dconImgText.Text = reader["DCON_IMG"].ToString();
					rateCheck.Checked = reader["AGE_FLG"].ToString() == "1" ? true : false;
					extractToolCombo.SelectedIndex = Convert.ToInt32(DecodeSQLSpecialChars(reader["EXTRACT_TOOL"].ToString()));
					savePathText.Text = DecodeSQLSpecialChars(reader["SAVEDATA_PATH"].ToString());

					if (File.Exists(imgPathText.Text))
					{
						iconImage.ImageLocation = imgPathText.Text;
					}
					else
					{
						iconImage.ImageLocation = string.Empty;
					}
				}

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}
		}

		private void getExec(String selectedListCount)
		{
			// ini取得と画面反映
			try
			{
				iniPath = GameDir + selectedListCount + ".ini";
				if (File.Exists(iniPath))
				{
					KeyNames[] keyNames = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.rating, KeyNames.execute_cmd, KeyNames.extract_tool, KeyNames.savedata_path };
					string[] failedVal = { string.Empty, string.Empty, string.Empty, "0", string.Empty, string.Empty, string.Empty, Rate.ToString(), String.Empty, "0", string.Empty };

					string[] resultValues = IniRead(iniPath, "game", keyNames, failedVal);

					label9.Text = selectedListCount;
					titleText.Text = resultValues[0];
					gameTitleLabel.Text = titleText.Text;
					executeCmdText.Text = resultValues[8];
					imgPathText.Text = resultValues[1];
					exePathText.Text = resultValues[2];
					runTimeText.Value = Convert.ToInt32(resultValues[3]);
					startCountText.Value = Convert.ToInt32(resultValues[4]);
					dconText.Text = resultValues[5];
					dconImgText.Text = resultValues[6];
					rateCheck.Checked = Convert.ToBoolean(Convert.ToInt32(resultValues[7]));
					extractToolCombo.SelectedIndex = Convert.ToInt32(resultValues[9]);
					savePathText.Text = resultValues[10];
					if (File.Exists(imgPathText.Text))
					{
						iconImage.ImageLocation = imgPathText.Text;
					}
					else
					{
						iconImage.ImageLocation = string.Empty;
					}
				}

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, string.Empty);
			}
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			newGameName = string.Empty;
			Close();
		}

		private void ApplyButton_Click(object sender, EventArgs e)
		{
			bool hasError = false;
			if (genSaveType == "D")
			{
				string exMsg = string.Empty;
				SqlCommand cm;
				SqlCommand cm2;
				SqlDataReader dr;

				cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT GAME_NAME FROM " + DbName + "." + DbTable
									+ " WHERE ID = " + label9.Text.Trim()
				};
				cm2.Connection = con;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					// SQL文
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET GAME_NAME = @game_name, GAME_PATH = @game_path, IMG_PATH = @img_path, UPTIME = @uptime, RUN_COUNT = @run_count, DCON_TEXT = @dcon_text, AGE_FLG = @age_flg, DCON_IMG = @dcon_img, EXECUTE_CMD = @execute_cmd, EXTRACT_TOOL = @extract_tool, SAVEDATA_PATH = @savePath "
								+ "WHERE ID = @id"
				};
				// パラメータの設定
				cm.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(titleText.Text.Trim()));
				cm.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(exePathText.Text.Trim()));
				cm.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(imgPathText.Text.Trim()));
				cm.Parameters.AddWithValue("@uptime", runTimeText.Value);
				cm.Parameters.AddWithValue("@run_count", startCountText.Value);
				cm.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(dconText.Text.Trim()));
				cm.Parameters.AddWithValue("@age_flg", (rateCheck.Checked ? "1" : "0"));
				cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
				cm.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(executeCmdText.Text.Trim()));
				cm.Parameters.AddWithValue("@extract_tool", extractToolCombo.SelectedIndex);
				cm.Parameters.AddWithValue("@savePath", EncodeSQLSpecialChars(savePathText.Text.Trim()));
				cm.Parameters.AddWithValue("@id", label9.Text.Trim());
				cm.Connection = con;

				try
				{
					con.Open();

					dr = cm2.ExecuteReader();

					if (dr.Read())
					{
						if (DecodeSQLSpecialChars(dr["GAME_NAME"].ToString()) != gameTitleLabel.Text)
						{
							throw new Exception("ゲーム名が一致しません。リストが変更された可能性があります。\n検証で取得されたタイトル：[" + DecodeSQLSpecialChars(dr["GAME_NAME"].ToString()) + "]\n編集中のタイトル：[" + gameTitleLabel.Text + "]");
						}
					}
					dr.Close();

					cm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					exMsg = ex.Message;
					hasError = true;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					if (exMsg.Length != 0)
					{
						MessageBox.Show(exMsg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			else if (genSaveType == "M")
			{
				string exMsg = string.Empty;
				MySqlCommand cm;
				MySqlCommand cm2;
				MySqlDataReader dr;

				cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT GAME_NAME FROM " + DbTable
									+ " WHERE ID = " + label9.Text.Trim()
				};

				cm2.Connection = con2;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					// SQL文
					CommandText = @"UPDATE " + DbTable + " SET GAME_NAME = @game_name, GAME_PATH = @game_path, IMG_PATH = @img_path, UPTIME = @uptime, RUN_COUNT = @run_count, DCON_TEXT = @dcon_text, AGE_FLG = @age_flg, DCON_IMG = @dcon_img, EXECUTE_CMD = @execute_cmd, EXTRACT_TOOL = @extract_tool, SAVEDATA_PATH = @savePath "
												+ "WHERE ID = @id"
				};
				// パラメータの設定
				cm.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(titleText.Text.Trim()));
				cm.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(exePathText.Text.Trim()));
				cm.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(imgPathText.Text.Trim()));
				cm.Parameters.AddWithValue("@uptime", runTimeText.Value);
				cm.Parameters.AddWithValue("@run_count", startCountText.Value);
				cm.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(dconText.Text.Trim()));
				cm.Parameters.AddWithValue("@age_flg", (rateCheck.Checked ? "1" : "0"));
				cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
				cm.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(executeCmdText.Text.Trim()));
				cm.Parameters.AddWithValue("@extract_tool", extractToolCombo.SelectedIndex);
				cm.Parameters.AddWithValue("@savePath", EncodeSQLSpecialChars(savePathText.Text.Trim()));
				cm.Parameters.AddWithValue("@id", label9.Text.Trim());

				cm.Connection = con2;

				try
				{
					con2.Open();

					dr = cm2.ExecuteReader();

					if (dr.Read())
					{
						if (DecodeSQLSpecialChars(dr["GAME_NAME"].ToString()) != gameTitleLabel.Text)
						{
							throw new Exception("ゲーム名が一致しません。リストが変更された可能性があります。\n検証で取得されたタイトル：[" + DecodeSQLSpecialChars(dr["GAME_NAME"].ToString()) + "]\n編集中のタイトル：[" + gameTitleLabel.Text + "]");
						}
					}
					dr.Close();

					cm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					exMsg = ex.Message;
					hasError = true;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					if (exMsg.Length != 0)
					{
						MessageBox.Show(exMsg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			else
			{
				// 次回DB接続時に更新するフラグを立てる
				if (genSaveType == "T")
				{
					IniWrite(GameIni, "list", "dbupdate", "1");
				}

				decimal runTimeTmp = runTimeText.Value;
				decimal startTimeTmp = startCountText.Value;

				KeyNames[] keyColumns = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.rating, KeyNames.execute_cmd, KeyNames.extract_tool, KeyNames.savedata_path };
				string[] writeValues = { titleText.Text.Trim(), imgPathText.Text.Trim(), exePathText.Text.Trim(), runTimeTmp.ToString(), startTimeTmp.ToString(), dconText.Text.Trim(), dconImgText.Text.Trim(), (rateCheck.Checked ? "1" : "0"), executeCmdText.Text.Trim(), extractToolCombo.SelectedIndex.ToString(), savePathText.Text.Trim() };
				IniWrite(iniPath, "game", keyColumns, writeValues);
			}

			if (!hasError)
			{
				newGameName = titleText.Text.Trim();
				Close();
			}
		}

		private void runTimeResetButton_Click(object sender, EventArgs e)
		{
			runTimeText.Value = 0;
		}

		private void startCountResetButton_Click(object sender, EventArgs e)
		{
			startCountText.Value = 0;
		}

		private void exePathButton_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "追加する実行ファイルを選択";
			openFileDialog1.Filter = "実行ファイル (*.exe)|*.exe|すべてのファイル (*.*)|*.*";
			openFileDialog1.FileName = "";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				exePathText.Text = openFileDialog1.FileName;
			}
			else
			{
				return;
			}
		}

		private void imgPathButton_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "実行ファイルの画像を選択";
			openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
			openFileDialog1.FileName = Path.GetFileNameWithoutExtension(exePathText.Text.Trim()).ToString() + ".png";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				imgPathText.Text = openFileDialog1.FileName;
			}
			else
			{
				return;
			}
		}

		private void GetDLsiteInfoButton_Click(object sender, EventArgs e)
		{
			// dlsiteからデータを取得します。
			dlSearchForm.StartPosition = FormStartPosition.CenterParent;
			dlSearchForm.ShowDialog();

			// 反映
			if (!string.IsNullOrEmpty(dlSearchForm.resultText))
			{
				titleText.Text = dlSearchForm.resultText;
				if (dlSearchForm.resultImageSaved && !string.IsNullOrEmpty(dlSearchForm.resultImagePath) && File.Exists(dlSearchForm.resultImagePath))
				{
					imgPathText.Text = dlSearchForm.resultImagePath;
					iconImage.ImageLocation = dlSearchForm.resultImagePath;
				}

				// フォーカス移動
				exePathText.Focus();
			}
			else
			{
				titleText.Focus();
			}
		}

		private void GetVNDBInfoButton_Click(object sender, EventArgs e)
		{
			// vndbフォーム初期化
			vndbSearchForm.StartPosition = FormStartPosition.CenterParent;

			vndbSearchForm.Title = titleText.Text.Trim();
			vndbSearchForm.ImageUrl = string.Empty;
			vndbSearchForm.SaveImage = (imgPathText.Text.Trim().Length == 0) ? true : false;
			vndbSearchForm.RequireApply = false;

			vndbSearchForm.ShowDialog();

			// 反映
			if (vndbSearchForm.RequireApply)
			{
				titleText.Text = vndbSearchForm.Title;
				if (!string.IsNullOrEmpty(vndbSearchForm.ImageUrl) && File.Exists(vndbSearchForm.ImageUrl))
				{
					imgPathText.Text = vndbSearchForm.ImageUrl;
					iconImage.ImageLocation = vndbSearchForm.ImageUrl;
				}

				// フォーカス移動
				if (string.IsNullOrEmpty(exePathText.Text))
				{
					exePathText.Focus();
				}
				else if (string.IsNullOrEmpty(imgPathText.Text))
				{
					imgPathText.Focus();
				}
				else
				{
					ApplyButton.Focus();
				}
			}
			else
			{
				titleText.Focus();
			}
		}

		private void Editor_DragDrop(object sender, DragEventArgs e)
		{
			// DataFormats.FileDropを与えて、GetDataPresent()メソッドを呼び出す。
			var dropTarget = (string[])e.Data.GetData(DataFormats.FileDrop, false);

			// GetDataにより取得したString型の配列から要素を取り出す。
			var targetFile = dropTarget[0];

			AutoComplete(targetFile);
		}

		private void Editor_DragEnter(object sender, DragEventArgs e)
		{
			// マウスポインター形状変更
			//
			// DragDropEffects
			//  Copy  :データがドロップ先にコピーされようとしている状態
			//  Move  :データがドロップ先に移動されようとしている状態
			//  Scroll:データによってドロップ先でスクロールが開始されようとしている状態、あるいは現在スクロール中である状態
			//  All   :上の3つを組み合わせたもの
			//  Link  :データのリンクがドロップ先に作成されようとしている状態
			//  None  :いかなるデータもドロップ先が受け付けようとしない状態

			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		/// <summary>
		/// <see cref="imgPathText"/> に入力されているパスで <see cref="imgPictureBox"/> に画像を反映
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ApplyPictureBox(object sender, EventArgs e)
		{
			// 画面反映
			if (File.Exists(imgPathText.Text))
			{
				try
				{
					iconImage.ImageLocation = imgPathText.Text;
				}
				catch (Exception ex)
				{
					iconImage.ImageLocation = "";
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, imgPathText.Text);
				}
			}
			else
			{
				iconImage.ImageLocation = "";
			}
		}

		/// <summary>
		/// 実行アプリケーションが指定された場合に、自動的にアプリケーション情報を取得します。
		/// </summary>
		/// <param name="targetFile"></param>
		/// <param name="targetType"></param>
		private void AutoComplete(string targetFile, string targetType = "")
		{
			// ファイルタイプが指定されていない場合に自動判別
			if (targetType == string.Empty)
			{
				string extension = Path.GetExtension(targetFile);
				if (extension == ".jpg" || extension == ".png" || extension == ".bmp" || extension == ".gif")
				{
					targetType = "img";
				}
				else
				{
					targetType = "exe";
				}
			}

			// ファイルごとによる処理
			if (targetType == "exe")
			{
				// タイトル自動補填（ファイル名）
				if (string.IsNullOrEmpty(titleText.Text.Trim()))
				{
					titleText.Text = Path.GetFileNameWithoutExtension(targetFile);
				}

				// 実行ファイルパス自動補填（ファイルフルパス）
				exePathText.Text = targetFile;

				// 画像ファイルパス自動補填（[ファイル名(.png|.jpg)]の検出）
				string targetPath = Path.GetDirectoryName(targetFile).EndsWith("\\") ? Path.GetDirectoryName(targetFile) : Path.GetDirectoryName(targetFile) + "\\";
				string targetName = Path.GetFileNameWithoutExtension(targetFile);
				string[] targetImageTemp = { targetPath + targetName + ".png", targetPath + targetName + ".jpg" };

				if (File.Exists(targetImageTemp[0]))
				{
					imgPathText.Text = targetImageTemp[0];
				}
				else if (File.Exists(targetImageTemp[1]))
				{
					imgPathText.Text = targetImageTemp[1];
				}
				else
				{
					imgPathText.Text = string.Empty;
				}

				// 抽出ツールの自動選択
				if (ExtractEnable)
				{
					string extractToolResult = GetExtractTool(targetFile);
					if (extractToolResult.Equals("krkr") || extractToolResult.Equals("krkrz"))
					{
						extractToolCombo.SelectedItem = extractToolResult;
					}
				}
			}
			else
			{
				imgPathText.Text = targetFile;
			}

			ApplyPictureBox(null, null);

			// 起動回数、実行時間の初期化
			startCountText.Value = 0;
			runTimeText.Value = 0;

			// フラグ初期化
			rateCheck.Checked = Rate != 0;
		}

		private string GetExtractTool(string executePath)
		{
			string result = string.Empty;
			if (File.Exists(executePath))
			{
				// アプリケーション情報の取得
				System.Diagnostics.FileVersionInfo vi = System.Diagnostics.FileVersionInfo.GetVersionInfo(executePath);
				string productName = vi.ProductName;
				if (ProductName.Contains("(KIRIKIRI) Z"))
				{
					result = "krkrz";
				}
				else if (ProductName.Contains("(KIRIKIRI)"))
				{
					result = "krkr";
				}
			}
			return result;
		}
	}
}