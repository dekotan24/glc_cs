using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using static glc_cs.Core.Property;
using static glc_cs.General.Var;

namespace glc_cs.Core
{
	internal class Export
	{
		public static bool ExportData(string exportType, string saveType, string tableName, string savePath = "", string databaseName = null, SqlConnection scn = null, MySqlConnection mcn = null)
		{
			bool result = true;

			if (exportType == "INI" && !savePath.EndsWith("\\"))
			{
				savePath += "\\";
			}

			switch (saveType)
			{
				case "D":
					// MSSQL
					result = ExecMSSQLExport(exportType, scn, databaseName, tableName, savePath);
					break;
				case "M":
					// MySQL
					result = ExecMySQLExport(exportType, mcn, tableName, savePath);
					break;
				default:
					result = false;
					break;
			}
			return result;
		}

		private static bool ExecMSSQLExport(string exportType, SqlConnection cn, string databaseName, string tableName, string exportPath)
		{
			bool result = true;

			try
			{
				cn.Open();

				// 全ゲーム数取得
				SqlCommand cm = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + tableName
				};
				cm.Connection = cn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					WriteIni("list", "game", sqlAns.ToString(), 0, exportPath);
				}
				else
				{
					// ゲームが1つもない場合
					// 作成したディレクトリを削除する
					if (Directory.Exists(exportPath))
					{
						Directory.Delete(exportPath, true);
					}
					WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] MSSQL:" + cn.ConnectionString);
					return false;
				}

				// DBから全ゲームデータを取り出す
				SqlCommand cm2 = new SqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 60,
					CommandText = @"SELECT GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
								+ " FROM " + tableName
				};
				cm2.Connection = cn;

				string[,] items = new string[sqlAns, 11];
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

				switch (exportType)
				{
					case "INI":
						IniExport(items, exportPath);
						break;
					case "CSV":
						CsvExport(items, exportPath);
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

		private static bool ExecMySQLExport(string exportType, MySqlConnection mcn, string tableName, string exportPath)
		{
			bool result = true;

			try
			{
				mcn.Open();

				// 全ゲーム数取得
				MySqlCommand cm = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 30,
					CommandText = @"SELECT count(*) FROM " + tableName
				};
				cm.Connection = mcn;

				int sqlAns = Convert.ToInt32(cm.ExecuteScalar().ToString());

				if (sqlAns > 0)
				{
					WriteIni("list", "game", sqlAns.ToString(), 0, exportPath);
				}
				else
				{
					// ゲームが1つもない場合
					// 作成したディレクトリを削除する
					if (Directory.Exists(exportPath))
					{
						Directory.Delete(exportPath, true);
					}
					WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] MySQL:" + mcn.ConnectionString);
					return false;
				}

				// DBから全ゲームデータを取り出す
				MySqlCommand cm2 = new MySqlCommand()
				{
					CommandType = CommandType.Text,
					CommandTimeout = 60,
					CommandText = @"SELECT GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
								+ " FROM " + tableName
				};
				cm2.Connection = mcn;

				string[,] items = new string[sqlAns + 1, 15];
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

					// ボディ追加
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

				switch (exportType)
				{
					case "INI":
						IniExport(items, exportPath);
						break;
					case "CSV":
						CsvExport(items, exportPath);
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

		private static bool CsvExport(string[,] items, string exportPath)
		{
			bool result = true;

			StreamWriter SW = new StreamWriter(exportPath, false, Encoding.UTF8);
			try
			{
				if (exportPath.Length == 0)
				{
					// ファイル
					OpenFileDialog openFileDialog1 = new OpenFileDialog();
					openFileDialog1.Title = "出力先を選択";
					openFileDialog1.Filter = "CSVファイル(*.csv)|*.csv";
					openFileDialog1.FileName = "";
					if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
					{
						exportPath = openFileDialog1.FileName;
					}
					else
					{
						return false;
					}
				}

				if (exportPath.Length != 0)
				{
					// 書き込み
					for (int i = 0; i < items.GetLength(0); i++)
					{
						for (int j = 0; j < items.GetLength(1); j++)
						{
							if (j != 0)
							{
								SW.Write(",");
							}

							SW.Write(items[i, j]);
						}

						// 改行
						SW.WriteLine();
					}
					SW.Close();
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, exportPath);
				result = false;

				SW.Close();

				// 出力ファイル削除
				if (File.Exists(exportPath))
				{
					File.Delete(exportPath);
				}
			}

			return result;
		}

		private static bool IniExport(string[,] items, string exportPath)
		{
			bool result = true;
			try
			{
				if (exportPath.Length == 0)
				{
					// ファイル
					FolderBrowserDialog openFileDialog1 = new FolderBrowserDialog();
					openFileDialog1.Description = "出力先を選択";
					if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
					{
						exportPath = openFileDialog1.SelectedPath;
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
					string saveLocalIniPath = exportPath + count + ".ini";
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
				IniWrite(exportPath + "game.ini", "list", "game", (count - 1).ToString());
			}
			catch (Exception ex)
			{
				result = false;
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name);

				// 出力フォルダ削除
				if (Directory.Exists(exportPath))
				{
					Directory.Delete(exportPath, true);
				}
			}

			return result;
		}
	}
}