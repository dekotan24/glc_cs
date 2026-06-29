using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static glc_cs.Core.Functions;
using static glc_cs.Core.Property;

namespace glc_cs
{
	public partial class Statistics : Form
	{
		public Statistics()
		{
			InitializeComponent();
		}

		private void Statistics_Load(object sender, EventArgs e)
		{
			LoadStatistics();
		}

		private void LoadStatistics()
		{
			var games = new List<GameStat>();

			try
			{
				if (SaveType == "I" || SaveType == "T")
				{
					for (int i = 1; i <= GameMax; i++)
					{
						string path = GameDir + i + ".ini";
						if (File.Exists(path))
						{
							string name = IniRead(path, "game", KeyNames.name, string.Empty);
							int time = 0;
							int.TryParse(IniRead(path, "game", KeyNames.time, "0"), out time);
							int starts = 0;
							int.TryParse(IniRead(path, "game", KeyNames.start, "0"), out starts);
							games.Add(new GameStat { Name = name, PlayTimeSeconds = time, StartCount = starts });
						}
					}
				}
				else if (SaveType == "D")
				{
					using (var cn = SqlCon)
					{
						cn.Open();
						var cm = new SqlCommand
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = "SELECT GAME_NAME, UPTIME, RUN_COUNT FROM " + SafeQualifiedTable,
							Connection = cn
						};
						using (var reader = cm.ExecuteReader())
						{
							while (reader.Read())
							{
								games.Add(new GameStat
								{
									Name = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()),
									PlayTimeSeconds = Convert.ToInt32(reader["UPTIME"]),
									StartCount = Convert.ToInt32(reader["RUN_COUNT"])
								});
							}
						}
					}
				}
				else if (SaveType == "M")
				{
					using (var cn = SqlCon2)
					{
						cn.Open();
						var cm = new MySqlCommand
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = "SELECT GAME_NAME, UPTIME, RUN_COUNT FROM " + SafeSqlIdentifier(DbTable),
							Connection = cn
						};
						using (var reader = cm.ExecuteReader())
						{
							while (reader.Read())
							{
								games.Add(new GameStat
								{
									Name = DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()),
									PlayTimeSeconds = Convert.ToInt32(reader["UPTIME"]),
									StartCount = Convert.ToInt32(reader["RUN_COUNT"])
								});
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.StackTrace);
				MessageBox.Show("統計データの取得中にエラーが発生しました。\n" + ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int totalGames = games.Count;
			long totalSeconds = games.Sum(g => (long)g.PlayTimeSeconds);
			double totalHours = totalSeconds / 3600.0;
			double avgHours = totalGames > 0 ? totalHours / totalGames : 0;
			int totalLaunches = games.Sum(g => g.StartCount);

			totalGamesLabel.Text = totalGames.ToString("N0");
			totalPlayTimeLabel.Text = totalHours.ToString("F1") + " 時間";
			avgPlayTimeLabel.Text = avgHours.ToString("F1") + " 時間";
			totalLaunchesLabel.Text = totalLaunches.ToString("N0") + " 回";

			// Top 5 by play time
			var topByTime = games.OrderByDescending(g => g.PlayTimeSeconds).Take(5).ToList();
			playTimeRankList.Items.Clear();
			for (int i = 0; i < topByTime.Count; i++)
			{
				var g = topByTime[i];
				var lvi = new ListViewItem((i + 1).ToString());
				lvi.SubItems.Add(g.Name);
				lvi.SubItems.Add((g.PlayTimeSeconds / 3600.0).ToString("F1") + " h");
				playTimeRankList.Items.Add(lvi);
			}

			// Top 5 by launch count
			var topByCount = games.OrderByDescending(g => g.StartCount).Take(5).ToList();
			launchRankList.Items.Clear();
			for (int i = 0; i < topByCount.Count; i++)
			{
				var g = topByCount[i];
				var lvi = new ListViewItem((i + 1).ToString());
				lvi.SubItems.Add(g.Name);
				lvi.SubItems.Add(g.StartCount.ToString("N0") + " 回");
				launchRankList.Items.Add(lvi);
			}
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private class GameStat
		{
			public string Name { get; set; }
			public int PlayTimeSeconds { get; set; }
			public int StartCount { get; set; }
		}
	}
}
