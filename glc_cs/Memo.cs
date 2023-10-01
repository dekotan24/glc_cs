﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class Memo : Form
	{
		SqlConnection con = new SqlConnection();
		MySqlConnection con2 = new MySqlConnection();
		string genSaveType = string.Empty;
		string iniPath = string.Empty;

		public Memo(string saveType, string selectedListCount, SqlConnection cn, SqlCommand cm)
		{
			InitializeComponent();

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

			memoTextBox.Focus();
		}

		public Memo(string saveType, string selectedListCount, MySqlConnection cn, MySqlCommand cm)
		{
			InitializeComponent();

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

			memoTextBox.Focus();
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
					gameIDLabel.Text = reader["ID"].ToString();
					gameTitleLabel.Text = reader["GAME_NAME"].ToString();
					toolTip1.SetToolTip(gameTitleLabel, gameTitleLabel.Text);
					memoTextBox.Text = reader["MEMO"].ToString();
					if (File.Exists(reader["IMG_PATH"].ToString()))
					{
						gameImage.ImageLocation = reader["IMG_PATH"].ToString();
					}
					else
					{
						gameImage.ImageLocation = string.Empty;
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
					gameIDLabel.Text = reader["ID"].ToString();
					gameTitleLabel.Text = reader["GAME_NAME"].ToString();
					toolTip1.SetToolTip(gameTitleLabel, gameTitleLabel.Text);
					memoTextBox.Text = reader["MEMO"].ToString();
					if (File.Exists(reader["IMG_PATH"].ToString()))
					{
						gameImage.ImageLocation = reader["IMG_PATH"].ToString();
					}
					else
					{
						gameImage.ImageLocation = string.Empty;
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
					KeyNames[] keyNames = { KeyNames.name, KeyNames.memo, KeyNames.imgpass };
					string[] failedVal = { string.Empty, string.Empty, string.Empty };

					string[] resultValues = IniRead(iniPath, "game", keyNames, failedVal);

					gameIDLabel.Text = selectedListCount;
					gameTitleLabel.Text = resultValues[0];
					toolTip1.SetToolTip(gameTitleLabel, gameTitleLabel.Text);
					memoTextBox.Text = resultValues[1];
					if (File.Exists(resultValues[2]))
					{
						gameImage.ImageLocation = resultValues[2];
					}
					else
					{
						gameImage.ImageLocation = string.Empty;
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
									+ " WHERE ID = " + gameIDLabel.Text.Trim()
				};

				cm2.Connection = con;

				cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET MEMO = N'" + memoTextBox.Text.Trim() + "' "
									+ "WHERE ID = " + gameIDLabel.Text.Trim()
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
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
									+ " WHERE ID = " + gameIDLabel.Text.Trim()
				};

				cm2.Connection = con2;

				cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"UPDATE " + DbName + "." + DbTable + " SET MEMO = N'" + memoTextBox.Text.Trim() + "' "
									+ "WHERE ID = " + gameIDLabel.Text.Trim()
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
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
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

				IniWrite(iniPath, "game", KeyNames.memo, memoTextBox.Text.Trim());
			}

			Close();
		}
	}
}