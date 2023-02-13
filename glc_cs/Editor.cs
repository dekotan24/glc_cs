using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

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
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
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
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
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
				iniPath = General.Var.GameDir + "\\" + selectedListCount + ".ini";
				if (File.Exists(iniPath))
				{
					label9.Text = selectedListCount;
					titleText.Text = General.Var.IniRead(iniPath, "game", "name", string.Empty);
					gameTitleLabel.Text = titleText.Text;
					imgPathText.Text = General.Var.IniRead(iniPath, "game", "imgpass", string.Empty);
					exePathText.Text = General.Var.IniRead(iniPath, "game", "pass", string.Empty);
					runTimeText.Value = Convert.ToInt32(General.Var.IniRead(iniPath, "game", "time", "0"));
					startCountText.Value = Convert.ToInt32(General.Var.IniRead(iniPath, "game", "start", string.Empty));
					dconText.Text = General.Var.IniRead(iniPath, "game", "stat", string.Empty);
					dconImgText.Text = General.Var.IniRead(iniPath, "game", "dcon_img", string.Empty);
					rateCheck.Checked = Convert.ToBoolean(Convert.ToInt32(General.Var.IniRead(iniPath, "game", "rating", General.Var.Rate.ToString())));
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
				General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, string.Empty);
			}
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void ApplyButton_Click(object sender, EventArgs e)
		{
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
					CommandText = @"SELECT GAME_NAME FROM " + General.Var.DbName + "." + General.Var.DbTable
									+ " WHERE ID = " + label9.Text.Trim()
				};

				cm2.Connection = con;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + General.Var.DbName + "." + General.Var.DbTable + " SET GAME_NAME = N'" + titleText.Text.Trim() + "', GAME_PATH = N'" + exePathText.Text.Trim() + "', IMG_PATH = N'" + imgPathText.Text.Trim() + "', UPTIME = N'" + runTimeText.Value + "', RUN_COUNT = N'" + startCountText.Value + "', DCON_TEXT = N'" + dconText.Text.Trim() + "', AGE_FLG = N'" + (rateCheck.Checked ? "1" : "0") + "', DCON_IMG = N'" + dconImgText.Text.Trim() + "' "
									+ "WHERE ID = " + label9.Text.Trim()
				};

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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					exMsg = ex.Message;
				}
				finally
				{
					if (con.State == ConnectionState.Open)
					{
						con.Close();
					}
					if (exMsg.Length != 0)
					{
						MessageBox.Show(exMsg, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
					CommandText = @"SELECT GAME_NAME FROM " + General.Var.DbTable
									+ " WHERE ID = " + label9.Text.Trim()
				};

				cm2.Connection = con2;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + General.Var.DbTable + " SET GAME_NAME = N'" + titleText.Text.Trim().Replace("'", "''").Replace("\\", "\\\\") + "', GAME_PATH = N'" + exePathText.Text.Trim().Replace("'", "''").Replace("\\", "\\\\") + "', IMG_PATH = N'" + imgPathText.Text.Trim().Replace("'", "''").Replace("\\", "\\\\") + "', UPTIME = N'" + runTimeText.Value + "', RUN_COUNT = N'" + startCountText.Value + "', DCON_TEXT = N'" + dconText.Text.Trim().Replace("'", "''").Replace("\\", "\\\\") + "', AGE_FLG = N'" + (rateCheck.Checked ? "1" : "0") + "', DCON_IMG = N'" + dconImgText.Text.Trim() + "' "
									+ "WHERE ID = " + label9.Text.Trim()
				};

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
					General.Var.WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
					exMsg = ex.Message;
				}
				finally
				{
					if (con2.State == ConnectionState.Open)
					{
						con2.Close();
					}
					if (exMsg.Length != 0)
					{
						MessageBox.Show(exMsg, General.Var.AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			else
			{
				// 次回DB接続時に更新するフラグを立てる
				if (genSaveType == "T")
				{
					General.Var.IniWrite(General.Var.GameIni, "list", "dbupdate", "1");
				}

				decimal runTimeTmp = runTimeText.Value;
				decimal startTimeTmp = startCountText.Value;

				General.Var.IniWrite(iniPath, "game", "name", titleText.Text.Trim());
				General.Var.IniWrite(iniPath, "game", "imgpass", imgPathText.Text.Trim());
				General.Var.IniWrite(iniPath, "game", "pass", exePathText.Text.Trim());
				General.Var.IniWrite(iniPath, "game", "time", runTimeTmp.ToString());
				General.Var.IniWrite(iniPath, "game", "start", startTimeTmp.ToString());
				General.Var.IniWrite(iniPath, "game", "stat", dconText.Text.Trim());
				General.Var.IniWrite(iniPath, "game", "dcon_img", dconImgText.Text.Trim());
				General.Var.IniWrite(iniPath, "game", "rating", (rateCheck.Checked ? "1" : "0"));
			}

			Close();
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