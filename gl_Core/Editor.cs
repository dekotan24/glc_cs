using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class Editor : Form
	{
		SqlConnection con = new SqlConnection();
		MySqlConnection con2 = new MySqlConnection();
		string genSaveType = string.Empty;
		string iniPath = string.Empty;

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
					titleText.Text = reader["GAME_NAME"].ToString();
					gameTitleLabel.Text = titleText.Text;
					executeCmdText.Text = reader["EXECUTE_CMD"].ToString();
					imgPathText.Text = reader["IMG_PATH"].ToString();
					exePathText.Text = reader["GAME_PATH"].ToString();
					runTimeText.Value = Convert.ToInt32(reader["UPTIME"]);
					startCountText.Value = Convert.ToInt32(reader["RUN_COUNT"]);
					dconText.Text = reader["DCON_TEXT"].ToString();
					dconImgText.Text = reader["DCON_IMG"].ToString();
					rateCheck.Checked = reader["AGE_FLG"].ToString() == "1" ? true : false;
					if (File.Exists(imgPathText.Text))
					{
						pictureBox1.ImageLocation = imgPathText.Text;
					}
					else
					{
						pictureBox1.ImageLocation = string.Empty;
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
					titleText.Text = reader["GAME_NAME"].ToString();
					gameTitleLabel.Text = titleText.Text;
					executeCmdText.Text = reader["EXECUTE_CMD"].ToString();
					imgPathText.Text = reader["IMG_PATH"].ToString();
					exePathText.Text = reader["GAME_PATH"].ToString();
					runTimeText.Value = Convert.ToInt32(reader["UPTIME"]);
					startCountText.Value = Convert.ToInt32(reader["RUN_COUNT"]);
					dconText.Text = reader["DCON_TEXT"].ToString();
					dconImgText.Text = reader["DCON_IMG"].ToString();
					rateCheck.Checked = reader["AGE_FLG"].ToString() == "1" ? true : false;
					if (File.Exists(imgPathText.Text))
					{
						pictureBox1.ImageLocation = imgPathText.Text;
					}
					else
					{
						pictureBox1.ImageLocation = string.Empty;
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
					KeyNames[] keyNames = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.rating, KeyNames.execute_cmd };
					string[] failedVal = { string.Empty, string.Empty, string.Empty, "0", string.Empty, string.Empty, string.Empty, Rate.ToString(), String.Empty };

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
					if (File.Exists(imgPathText.Text))
					{
						pictureBox1.ImageLocation = imgPathText.Text;
					}
					else
					{
						pictureBox1.ImageLocation = string.Empty;
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
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET GAME_NAME = @game_name, GAME_PATH = @game_path, IMG_PATH = @img_path, UPTIME = @uptime, RUN_COUNT = @run_count, DCON_TEXT = @dcon_text, AGE_FLG = @age_flg, DCON_IMG = @dcon_img, EXECUTE_CMD = @execute_cmd "
								+ "WHERE ID = @id"
				};
				// パラメータの設定
				cm.Parameters.AddWithValue("@game_name", titleText.Text.Trim());
				cm.Parameters.AddWithValue("@game_path", exePathText.Text.Trim());
				cm.Parameters.AddWithValue("@img_path", imgPathText.Text.Trim());
				cm.Parameters.AddWithValue("@uptime", runTimeText.Value);
				cm.Parameters.AddWithValue("@run_count", startCountText.Value);
				cm.Parameters.AddWithValue("@dcon_text", dconText.Text.Trim());
				cm.Parameters.AddWithValue("@age_flg", (rateCheck.Checked ? "1" : "0"));
				cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
				cm.Parameters.AddWithValue("@execute_cmd", executeCmdText.Text.Trim());
				cm.Parameters.AddWithValue("@id", label9.Text.Trim());
				cm.Connection = con;

				try
				{
					con.Open();

					dr = cm2.ExecuteReader();

					if (dr.Read())
					{
						if (dr["GAME_NAME"].ToString() != gameTitleLabel.Text)
						{
							throw new Exception("変更前の値に変更が行われた可能性があります。[" + dr["GAME_NAME"].ToString() + "] : [" + gameTitleLabel.Text + "]");
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
					CommandText = @"UPDATE " + DbTable + " SET GAME_NAME = @game_name, GAME_PATH = @game_path, IMG_PATH = @img_path, UPTIME = @uptime, RUN_COUNT = @run_count, DCON_TEXT = @dcon_text, AGE_FLG = @age_flg, DCON_IMG = @dcon_img, EXECUTE_CMD = @execute_cmd "
												+ "WHERE ID = @id"
				};
				// パラメータの設定
				cm.Parameters.AddWithValue("@game_name", titleText.Text.Trim());
				cm.Parameters.AddWithValue("@game_path", exePathText.Text.Trim());
				cm.Parameters.AddWithValue("@img_path", imgPathText.Text.Trim());
				cm.Parameters.AddWithValue("@uptime", runTimeText.Value);
				cm.Parameters.AddWithValue("@run_count", startCountText.Value);
				cm.Parameters.AddWithValue("@dcon_text", dconText.Text.Trim());
				cm.Parameters.AddWithValue("@age_flg", (rateCheck.Checked ? "1" : "0"));
				cm.Parameters.AddWithValue("@dcon_img", dconImgText.Text.Trim());
				cm.Parameters.AddWithValue("@execute_cmd", executeCmdText.Text.Trim());
				cm.Parameters.AddWithValue("@id", label9.Text.Trim());

				cm.Connection = con2;

				try
				{
					con2.Open();

					dr = cm2.ExecuteReader();

					if (dr.Read())
					{
						if (dr["GAME_NAME"].ToString() != gameTitleLabel.Text)
						{
							throw new Exception("変更前の値に変更が行われた可能性があります。[" + dr["GAME_NAME"].ToString() + "] : [" + gameTitleLabel.Text + "]");
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

				KeyNames[] keyColumns = { KeyNames.name, KeyNames.imgpass, KeyNames.pass, KeyNames.time, KeyNames.start, KeyNames.stat, KeyNames.dcon_img, KeyNames.rating, KeyNames.execute_cmd };
				string[] writeValues = { titleText.Text.Trim(), imgPathText.Text.Trim(), exePathText.Text.Trim(), runTimeTmp.ToString(), startTimeTmp.ToString(), dconText.Text.Trim(), dconImgText.Text.Trim(), (rateCheck.Checked ? "1" : "0"), executeCmdText.Text.Trim() };
				IniWrite(iniPath, "game", keyColumns, writeValues);
			}

			if (!hasError)
			{
				Close();
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			runTimeText.Value = 0;
		}

		private void button4_Click(object sender, EventArgs e)
		{
			startCountText.Value = 0;
		}

		private void button1_Click(object sender, EventArgs e)
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

		private void button2_Click(object sender, EventArgs e)
		{
			openFileDialog1.Title = "実行ファイルの画像を選択";
			openFileDialog1.Filter = "画像ファイル (*.png;*.jpg;*.bmp;*.gif)|*.png;*.jpg;*.bmp;*.gif";
			openFileDialog1.FileName = System.IO.Path.GetFileNameWithoutExtension(exePathText.Text.Trim()).ToString() + ".png";
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				imgPathText.Text = openFileDialog1.FileName;
			}
			else
			{
				return;
			}
		}
	}
}