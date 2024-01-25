using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static glc_cs.Core.Property;
using static glc_cs.General.Var;

namespace glc_cs.Core
{
	internal class Import
	{
		public static bool ImportData(string importType, string saveType, string tableName, string savePath = "", string databaseName = null, SqlConnection scn = null, MySqlConnection mcn = null)
		{
			bool result = true;

			if (importType == "INI" && !savePath.EndsWith("\\"))
			{
				savePath += "\\";
			}

			switch (saveType)
			{
				case "D":
					// MSSQL
					result = ExecMSSQLImport(importType, scn, databaseName, tableName, savePath);
					break;
				case "M":
					// MySQL
					result = ExecMySQLImport(importType, mcn, tableName, savePath);
					break;
				default:
					result = false;
					break;
			}
			return result;
		}

		private static bool ExecMSSQLImport(string importType, SqlConnection cn, string databaseName, string tableName, string importPath)
		{
			bool result = true;
			try
			{
				cn.Open();

				// DBから全ゲームデータを取り出す
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 60,
					CommandText = @"SELECT GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
								+ " FROM " + tableName
				};
				cm.Connection = cn;

				using (var reader = cm2.ExecuteReader())
				{
					// ヘッダ追加
					int i = 0;
					items[i, 0] = "GAME_NAME";
					items[i, 1] = "GAME_PATH";
					items[i, 2] = "EXECUTE_CMD";
					items[i, 3] = "IMG_PATH";
					items[i, 4] = "UPTIME";
					items[i, 5] = "RUN_COUNT";
					items[i, 6] = "DCON_TEXT";
					items[i, 7] = "AGE_FLG";
					items[i, 8] = "TEMP1";
					items[i, 9] = "LAST_RUN";
					items[i, 10] = "DCON_IMG";
					items[i, 11] = "MEMO";
					items[i, 12] = "STATUS";
					items[i, 13] = "EXTRACT_TOOL";
					items[i, 14] = "DB_VERSION";
					i++;

					while (reader.Read())
					{
						items[i, 0] = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString());
						items[i, 1] = DecodeSQLSpecialChars(reader["GAME_PATH"].ToString());
						items[i, 2] = DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString());
						items[i, 3] = DecodeSQLSpecialChars(reader["IMG_PATH"].ToString());
						items[i, 4] = reader["UPTIME"].ToString();
						items[i, 5] = reader["RUN_COUNT"].ToString();
						items[i, 6] = DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString());
						items[i, 7] = reader["AGE_FLG"].ToString();
						items[i, 8] = reader["TEMP1"].ToString();
						items[i, 9] = reader["LAST_RUN"].ToString();
						items[i, 10] = reader["DCON_IMG"].ToString();
						items[i, 11] = DecodeSQLSpecialChars(reader["MEMO"].ToString());
						items[i, 12] = reader["STATUS"].ToString();
						items[i, 13] = reader["EXTRACT_TOOL"].ToString();
						items[i, 14] = reader["DB_VERSION"].ToString();
						i++;
					}
				}

				switch (importType)
				{
					case "INI":
						IniImport(items, importPath);
						break;
					case "CSV":
						CsvImport(items, importPath);
						break;
				}
			}
			catch (Exception ex)
			{
				result = false;
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name);
			}

			return result;
		}

		private static bool ExecMySQLImport(string importType, MySqlConnection mcn, string tableName, string importPath)
		{
			bool result = true;
			int gameMaxCnt = 0;

			// 0,0で初期化
			string[,] items = new string[0, 0];

			try
			{
				mcn.Open();

				// 最大数取得
				string gameIniPath = importPath + "game.ini";
				if (!File.Exists(gameIniPath))
				{
					WriteErrorLog("ゲーム統括管理INIファイルが存在しません。", MethodBase.GetCurrentMethod().Name, gameIniPath);
					return false;
				}
				items = new string[gameMaxCnt + 1, 15];

				KeyNames[] colNames =
				{
					KeyNames.name,
					KeyNames.pass,
					KeyNames.execute_cmd,
					KeyNames.imgpass,
					KeyNames.time,
					KeyNames.start,
					KeyNames.stat,
					KeyNames.rating,
					KeyNames.temp1,
					KeyNames.lastrun,
					KeyNames.dcon_img,
					KeyNames.memo,
					KeyNames.status,
					KeyNames.extract_tool,
					KeyNames.ini_version
				};
				string[] failedVal =
				{
					"",
					"",
					"",
					"",
					"0",
					"0",
					"",
					Rate.ToString(),
					"",
					"",
					"",
					"",
					DefaultStatusValueOfNotPlaying,
					"0",
					DBVer
				};

				for (int i = 0; i < gameMaxCnt; i++)
				{
					string[] getItem = IniRead(importPath + i + ".ini", "game", colNames, failedVal);
					items[i, 0] = getItem[0];
					items[i, 1] = getItem[1];
					items[i, 2] = getItem[2];
					items[i, 3] = getItem[3];
					items[i, 4] = getItem[4];
					items[i, 5] = getItem[5];
					items[i, 6] = getItem[6];
					items[i, 7] = getItem[7];
					items[i, 8] = getItem[8];
					items[i, 9] = getItem[9];
					items[i, 10] = getItem[10];
					items[i, 11] = getItem[11];
					items[i, 12] = getItem[12];
					items[i, 13] = getItem[13];
					items[i, 14] = getItem[14];
				}

				switch (importType)
				{
					case "INI":
						IniImport(items, ConnType.MySQL);
						break;
					case "CSV":
						CsvImport(items, ConnType.MySQL);
						break;
				}
			}
			catch (Exception ex)
			{
				result = false;
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name);
			}

			return result;
		}

		private static bool CsvImport(string[,] items, ConnType connType)
		{
			bool result = true;

			// 接続情報
			SqlConnection cn = SqlCon;
			SqlCommand cm = new SqlCommand();

			try
			{
				cn.Open();

				for (int i = 0; i < items.GetLength(1); i++)
				{
					cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						// SQL文
						CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, DB_VERSION, EXTRACT_TOOL ) VALUES ( @game_name, @game_path, @execute_cmd, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @temp1, @last_run, @dcon_img, @memo, @status, @db_version, @extract_tool )"
					};
					cm.Connection = cn;

					// パラメータの設定
					cm.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(items[i, 0]));
					cm.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(items[i, 1]));
					cm.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(items[i, 2]));
					cm.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(items[i, 3]));
					cm.Parameters.AddWithValue("@uptime", items[i, 4]);
					cm.Parameters.AddWithValue("@run_count", items[i, 5]);
					cm.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(items[1, 6]));
					cm.Parameters.AddWithValue("@age_flg", items[i, 7]);
					cm.Parameters.AddWithValue("@temp1", items[i, 8]);
					cm.Parameters.AddWithValue("@lastrun", items[i, 9]);
					cm.Parameters.AddWithValue("@dcon_img", items[i, 10]);
					cm.Parameters.AddWithValue("@memo", items[i, 11]);
					cm.Parameters.AddWithValue("@status", items[i, 12]);
					cm.Parameters.AddWithValue("@extract_tool", items[i, 13]);
					cm.Parameters.AddWithValue("@db_version", items[i, 14]);

					cm.ExecuteNonQuery();
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
				result = false;
			}
			finally
			{
				if (cn.State == ConnectionState.Open)
				{
					cn.Close();
				}
			}
			return result;
		}

		private static bool IniImport(string[,] items, ConnType connType)
		{
			bool result = true;
			string importPath = string.Empty;
			try
			{
				if (items.Length == 0)
				{
					// ファイル
					FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();
					openFileDialog1.Description = "出力先を選択";
					if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
					{
						importPath = openFileDialog1.SelectedPath;
					}
					else
					{
						return false;
					}
				}

				int count = 1;
				for (int i = 1; i < items.GetLength(0); i++)
				{
					// 1件ずつローカルINIに書く
					string saveLocalIniPath = importPath + count + ".ini";
					KeyNames[] colNames =
					{
						KeyNames.name,
						KeyNames.pass,
						KeyNames.execute_cmd,
						KeyNames.imgpass,
						KeyNames.time,
						KeyNames.start,
						KeyNames.stat,
						KeyNames.rating,
						KeyNames.temp1,
						KeyNames.lastrun,
						KeyNames.dcon_img,
						KeyNames.memo,
						KeyNames.status,
						KeyNames.extract_tool,
						KeyNames.ini_version
					};
					string[] keys =
					{
						DecodeSQLSpecialChars(items[i,0].ToString()),
						DecodeSQLSpecialChars(items[i,1].ToString()),
						DecodeSQLSpecialChars(items[i,2].ToString()),
						DecodeSQLSpecialChars(items[i,3].ToString()),
						(string.IsNullOrEmpty(items[i,4].ToString()) ? "0" : items[i,4].ToString()),
						(string.IsNullOrEmpty(items[i,5].ToString()) ? "0" : items[i,5].ToString()),
						DecodeSQLSpecialChars(items[i,6].ToString()),
						(string.IsNullOrEmpty(items[i,7].ToString()) ? Rate.ToString() : items[i,7].ToString()),
						items[i,8].ToString(),
						items[i,9].ToString(),
						items[i,10].ToString(),
						DecodeSQLSpecialChars(items[i,11].ToString()),
						items[i,12].ToString(),
						items[i,13].ToString(),
						items[i,14].ToString()
					};
					IniWrite(saveLocalIniPath, "game", colNames, keys);

					count++;
				}
				IniWrite(importPath + "game.ini", "list", "game", (count - 1).ToString());
			}
			catch (Exception ex)
			{
				result = false;
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name);

				// 出力フォルダ削除
				if (Directory.Exists(importPath))
				{
					Directory.Delete(importPath, true);
				}
			}

			return result;
		}
	}
}