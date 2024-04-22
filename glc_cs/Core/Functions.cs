using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static glc_cs.Core.Property;

namespace glc_cs.Core
{
	class Functions
	{
		// DLL宣言
		[DllImport("KERNEL32.DLL")]
		public static extern uint
		  GetPrivateProfileString(string lpAppName,
		  string lpKeyName, string lpDefault,
		  StringBuilder lpReturnedString, uint nSize,
		  string lpFileName);

		[DllImport("KERNEL32.DLL")]
		public static extern uint
		WritePrivateProfileString(string lpAppName,
		string lpKeyName, string lpString,
		string lpFileName);

		/// <summary>
		/// システム変数をロードします
		/// </summary>
		/// <returns></returns>
		public static bool GLConfigLoad()
		{
			Crypt base64 = new Crypt();

			string currentSaveType = SaveType;

			if (File.Exists(ConfigIni))
			{
				// config.ini 存在する場合
				// ゲームデータ管理方法がオフラインINIの場合は値の更新をしない
				if (SaveType != "T")
				{
					GameDir = ReadIni("default", "directory", BaseDir) + "Data";
					GameIni = GameDir + "game.ini";
					GameDb = ReadIni("default", "database", string.Empty);
					DconPath = ReadIni("connect", "dconPath", "-1");
					DisableInitialLoadCountFlg = Convert.ToBoolean(Convert.ToInt32(ReadIni("disable", "DisableInitialLoadCount", "1")));

					SaveType = ReadIni("general", "save", "I");
					OfflineSave = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "OfflineSave", "0")));
					UseLocalDB = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "UseLocalDB", "0")));
					DbUrl = ReadIni("connect", "DBURL", string.Empty);
					DbPort = ReadIni("connect", "DBPort", string.Empty);
					DbName = ReadIni("connect", "DBName", string.Empty);
					DbTable = ReadIni("connect", "DBTable", string.Empty);
					DbUser = ReadIni("connect", "DBUser", string.Empty);
					EnablePWCrypt = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "PWCryptFlg", "0")));
				}
				try
				{
					DbPass = base64.Decode(ReadIni("connect", "DBPass", string.Empty));
				}
				catch
				{
					DbPass = string.Empty;
				}

				if (SaveType == "I" || SaveType == "T")
				{
					// ini
					GameMax = Convert.ToInt32(ReadIni("list", "game", "0", 0));
				}
				else if (SaveType == "D")
				{
					// database
					SqlConnection cn = SqlCon;
					SqlCommand cm = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + DbName + "." + DbTable
					};
					cm.Connection = cn;

					try
					{
						cn.Open();
						GameMax = Convert.ToInt32(cm.ExecuteScalar().ToString());
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						GameMax = 0;
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
					// database
					MySqlConnection cn = SqlCon2;
					MySqlCommand cm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + DbTable
					};
					cm.Connection = cn;

					try
					{
						cn.Open();
						GameMax = Convert.ToInt32(cm.ExecuteScalar().ToString());
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cm.CommandText);
						GameMax = 0;
					}
					finally
					{
						if (cn.State == ConnectionState.Open)
						{
							cn.Close();
						}
					}
				}

				// dcon.jar のチェック
				if (!File.Exists(DconPath))
				{
					if (File.Exists(BaseDir + "dcon.jar"))
					{
						DconPath = (BaseDir + "dcon.jar");
					}
					else
					{
						DconPath = string.Empty;
					}
				}

				// 棒読みちゃん設定読み込み
				ByActive = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byActive", "0")));
				ByType = Convert.ToInt32(ReadIni("connect", "byType", "0"));
				ByHost = ReadIni("connect", "byHost", "127.0.0.1");
				ByPort = Convert.ToInt32(ReadIni("connect", "byPort", "50001"));
				ByRoW = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoW", "0")));
				ByRoS = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoS", "0")));
				ByRoG = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoG", "0")));

				// 総合
				BgImg = ReadIni("imgd", "bgimg", string.Empty);
				GridEnable = !Convert.ToBoolean(Convert.ToInt32(ReadIni("disable", "grid", "0")));
				InitialUpdateCheckSkipFlg = Convert.ToBoolean(Convert.ToInt32(ReadIni("disable", "updchk", "0")));
				InitialUpdateCheckSkipVer = ReadIni("disable", "updchkVer", string.Empty);
				WindowHideControlFlg = Convert.ToBoolean(Convert.ToInt32(ReadIni("disable", "enableWindowHideControl", "0")));
				FixGridSizeFlg = Convert.ToBoolean(Convert.ToInt32(ReadIni("grid", "fixGridSizeFlg", "0")));
				FixGridSize = Convert.ToInt32(ReadIni("grid", "fixGridSize", "0"));

				// dcon設定
				Dconnect = Convert.ToBoolean(Convert.ToInt32(ReadIni("checkbox", "dconnect", "0")));
				DconAppID = ReadIni("connect", "dconappid", string.Empty);
				Rate = Convert.ToInt32(ReadIni("checkbox", "rate", "0"));

				// 抽出
				ExtractEnable = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Enabled", "0")));

				ExtractKrkrPath = ReadIni("Extract", "krkrPath", string.Empty);
				ExtractKrkrArg = ReadIni("Extract", "krkrArg", string.Empty);
				ExtractKrkrAddGameArg = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrAddGameArg", "0")));
				ExtractKrkrCurDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrCurDir", "0")));
				ExtractKrkrGameDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrGameDir", "0")));

				ExtractKrkrzPath = ReadIni("Extract", "krkrzPath", string.Empty);
				ExtractKrkrzArg = ReadIni("Extract", "krkrzArg", string.Empty);
				ExtractKrkrzAddGameArg = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrzAddGameArg", "0")));
				ExtractKrkrzCurDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrzCurDir", "0")));
				ExtractKrkrzGameDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrzGameDir", "0")));

				ExtractKrkrDumpPath = ReadIni("Extract", "krkrDumpPath", string.Empty);
				ExtractKrkrDumpArg = ReadIni("Extract", "krkrDumpArg", string.Empty);
				ExtractKrkrDumpAddGameArg = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrDumpAddGameArg", "0")));
				ExtractKrkrDumpCurDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrDumpCurDir", "0")));
				ExtractKrkrDumpGameDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "krkrDumpGameDir", "0")));

				ExtractCustom1Path = ReadIni("Extract", "Custom1Path", string.Empty);
				ExtractCustom1Arg = ReadIni("Extract", "Custom1Arg", string.Empty);
				ExtractCustom1AddGameArg = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom1AddGameArg", "0")));
				ExtractCustom1CurDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom1CurDir", "0")));
				ExtractCustom1GameDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom1GameDir", "0")));

				ExtractCustom2Path = ReadIni("Extract", "Custom2Path", string.Empty);
				ExtractCustom2Arg = ReadIni("Extract", "Custom2Arg", string.Empty);
				ExtractCustom2AddGameArg = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom2AddGameArg", "0")));
				ExtractCustom2CurDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom2CurDir", "0")));
				ExtractCustom2GameDir = Convert.ToBoolean(Convert.ToInt32(ReadIni("Extract", "Custom2GameDir", "0")));
			}
			else
			{
				// config.ini 存在しない場合
				// ゲーム保存方法をiniに設定
				SaveType = "I";

				GameDir = BaseDir + "Data";
				GameIni = GameDir + "game.ini";
				GameDb = string.Empty;

				// dcon.jar のチェック
				if (File.Exists(BaseDir + "dcon.jar"))
				{
					DconPath = (BaseDir + "dcon.jar");
				}
				else
				{
					DconPath = string.Empty;
				}

				// 棒読みちゃん設定読み込み
				ByActive = false;
				ByType = 0;
				ByHost = "127.0.0.1";
				ByPort = 50001;
				ByRoW = false;
				ByRoS = false;
				ByRoG = false;

				// 総合
				BgImg = string.Empty;

				// dcon設定
				Dconnect = false;
				Rate = 0;
			}

			// 設定ロード前の保存方法がオフラインINIモードの場合、ゲームディレクトリ等を上書きする
			if (currentSaveType == "T")
			{
				SaveType = "T";
				GameDir = LocalPath;
				GameIni = LocalIni;
			}

			return true;
		}

		public static string[] IniRead(string fileName, string sectionName, KeyNames[] keyArray, string[] failedVal)
		{
			StringBuilder ans = new StringBuilder(1024);
			string[] data = new string[keyArray.Length];

			for (int i = 0; i < keyArray.Length; i++)
			{
				try
				{
					GetPrivateProfileString(
						sectionName,
						keyArray[i].ToString(),
						failedVal[i],
						ans,
						((uint)ans.Capacity),
						fileName);
					// 逐語的文字列リテラルを使用して、replaceやエスケープを省略する
					data[i] = $@"{ans}";
				}
				catch (Exception ex)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("ファイルパス：").Append(fileName);
					sb.Append("セクション：").Append(sectionName);
					sb.Append("キー：").Append(keyArray[i]);
					sb.Append("値：").Append(data[i]);

					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
				}
			}

			return data;
		}

		public static string IniRead(string fileName, string sectionName, KeyNames keyArray, string failedVal = "")
		{
			StringBuilder ans = new StringBuilder(1024);
			string data = string.Empty;

			try
			{
				GetPrivateProfileString(
					sectionName,
					keyArray.ToString(),
					failedVal,
					ans,
					((uint)ans.Capacity),
					fileName);
				data = $@"{ans}";
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("ファイルパス：").Append(fileName);
				sb.Append("セクション：").Append(sectionName);
				sb.Append("キー：").Append(keyArray);
				sb.Append("値：").Append(data);

				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
			}

			return data;
		}

		public static string[] IniRead(string fileName, string sectionName, string[] keyArray, string[] failedVal)
		{
			StringBuilder ans = new StringBuilder(1024);
			string[] data = new string[keyArray.Length];

			for (int i = 0; i < keyArray.Length; i++)
			{
				try
				{
					GetPrivateProfileString(
						sectionName,
						keyArray[i],
						failedVal[i],
						ans,
						((uint)ans.Capacity),
						fileName);
					data[i] = $@"{ans}";
				}
				catch (Exception ex)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("ファイルパス：").Append(fileName);
					sb.Append("セクション：").Append(sectionName);
					sb.Append("キー：").Append(keyArray[i]);
					sb.Append("値：").Append(data[i]);

					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
				}
			}

			return data;
		}

		/// <summary>
		/// 指定されたINIに値を書き込みます
		/// </summary>
		/// <param name="fileName">INIパス</param>
		/// <param name="sectionName">セクション名</param>
		/// <param name="keyArray">配列のキー値</param>
		/// <param name="valueArray">配列の値</param>
		public static void IniWrite(string fileName, string sectionName, KeyNames[] keyArray, string[] valueArray)
		{
			for (int i = 0; i < keyArray.Length; i++)
			{
				try
				{
					if (!File.Exists(fileName))
					{
						if (!Directory.Exists(Path.GetDirectoryName(fileName)))
						{
							Directory.CreateDirectory(Path.GetDirectoryName(fileName));
						}
						File.Create(fileName).Close();
					}
					WritePrivateProfileString(
									sectionName,
									keyArray[i].ToString(),
									valueArray[i].ToString(),
									fileName);
				}
				catch (Exception ex)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("ファイルパス：").Append(fileName);
					sb.Append(" | セクション：").Append(sectionName);
					sb.Append(" | キー：").Append("keyArray[" + i + "]").Append(keyArray[i]);
					sb.Append(" | 値：").Append("valueArray[" + i + "]").Append(valueArray[i]);

					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
				}
			}
			return;
		}

		/// <summary>
		/// 指定されたINIに値を書き込みます
		/// </summary>
		/// <param name="fileName">INIパス</param>
		/// <param name="sectionName">セクション名</param>
		/// <param name="key">キー値</param>
		/// <param name="value">値</param>
		public static void IniWrite(string fileName, string sectionName, KeyNames key, string value)
		{
			try
			{
				if (!File.Exists(fileName))
				{
					if (!Directory.Exists(Path.GetDirectoryName(fileName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(fileName));
					}
					File.Create(fileName).Close();
				}
				WritePrivateProfileString(
								sectionName,
								key.ToString(),
								value.ToString(),
								fileName);
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("ファイルパス：").Append(fileName);
				sb.Append(" | セクション：").Append(sectionName);
				sb.Append(" | キー：").Append(key);
				sb.Append(" | 値：").Append(value);

				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
			}
			return;
		}

		/// <summary>
		/// 指定されたINIに値を書き込みます
		/// </summary>
		/// <param name="fileName">INIパス</param>
		/// <param name="sectionName">セクション名</param>
		/// <param name="key">キー値</param>
		/// <param name="value">値</param>
		public static void IniWrite(string fileName, string sectionName, string key, string value)
		{
			try
			{
				if (!File.Exists(fileName))
				{
					if (!Directory.Exists(Path.GetDirectoryName(fileName)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(fileName));
					}
					File.Create(fileName).Close();
				}
				WritePrivateProfileString(
								sectionName,
								key.ToString(),
								value.ToString(),
								fileName);
			}
			catch (Exception ex)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("ファイルパス：").Append(fileName);
				sb.Append(" | セクション：").Append(sectionName);
				sb.Append(" | キー：").Append(key);
				sb.Append(" | 値：").Append(value);

				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, sb.ToString());
			}
			return;
		}

		/// <summary>
		/// Config.iniもしくはゲームデータ管理INIに値を書き込みます。
		/// <paramref name="isconfig"/>が1の時はConfig.iniに、
		/// <paramref name="isconfig"/>が1でない時、<paramref name="opt"/>に値が指定されていた場合は<paramref name="opt"/>\game.ini、
		/// <paramref name="opt"/>に値が指定されていない場合は、設定されているゲーム統括管理INIファイル（GameIni）に書き込みます。
		/// </summary>
		/// <param name="sec">セクション名</param>
		/// <param name="key">キー値</param>
		/// <param name="data">値</param>
		/// <param name="isconfig">config.iniが対象か（既定値：1）</param>
		/// <param name="opt">ゲーム統括管理INIがあるディレクトリパス</param>
		public static void WriteIni(string sec, string key, string data, int isconfig = 1, string opt = "")
		{
			if (isconfig == 1)
			{
				if (!File.Exists(ConfigIni))
				{
					File.Create(ConfigIni).Close();
				}
				WritePrivateProfileString(
								sec,
								key,
								data,
								ConfigIni);
			}
			else if (opt.Length == 0)
			{
				if (!Directory.Exists(GameDir))
				{
					Directory.CreateDirectory(GameDir);
				}
				if (!File.Exists(GameIni))
				{
					File.Create(GameIni).Close();
				}
				WritePrivateProfileString(
								sec,
								key,
								data,
								GameIni);
			}
			else
			{
				if (!Directory.Exists(opt))
				{
					Directory.CreateDirectory(opt);
				}
				if (!File.Exists(Path.Combine(opt, "game.ini")))
				{
					File.Create(Path.Combine(opt, "game.ini")).Close();
				}
				WritePrivateProfileString(
								sec,
								key,
								data,
								Path.Combine(opt, "game.ini"));

			}
			return;
		}

		/// <summary>
		/// Config.iniもしくはゲームデータ管理INIに値を読み込みます。
		/// <paramref name="isconfig"/>が1の時はConfig.ini、
		/// <paramref name="isconfig"/>が1でない時、<paramref name="opt"/>に値が指定されていた場合は<paramref name="opt"/>\game.ini、
		/// <paramref name="opt"/>に値が指定されていない場合は、設定されているゲーム統括管理INIファイルから読み込みます。
		/// </summary>
		/// <param name="sec">セクション名</param>
		/// <param name="key">キー値</param>
		/// <param name="failedval">読み込みに失敗した場合の値</param>
		/// <returns>キーの値もしくは<paramref name="failedVal"/></returns>
		public static string ReadIni(string sec, string key, string failedVal, int isconfig = 1, string opt = "")
		{
			string ans = "";
			StringBuilder data = new StringBuilder(1024);

			if (isconfig == 1)
			{
				GetPrivateProfileString(
					sec,
					key,
					failedVal,
					data,
					1024,
					ConfigIni);
			}
			else if (opt.Length == 0)
			{
				GetPrivateProfileString(
					sec,
					key,
					failedVal,
					data,
					1024,
					GameIni);
			}
			else
			{
				GetPrivateProfileString(
					sec,
					key,
					failedVal,
					data,
					1024,
					Path.Combine(opt, "game.ini"));

			}

			ans = data.ToString();
			return ans;
		}

		public static void Bouyomiage(string text)
		{
			if (ByActive)
			{
				TcpClient tc;

				if (ByType == 1)
				{
					string url = "http://127.0.0.1:" + ByPort + "/talk";
					System.Net.WebClient wc = new System.Net.WebClient();
					//NameValueCollectionの作成
					System.Collections.Specialized.NameValueCollection ps =
						new System.Collections.Specialized.NameValueCollection();
					//送信するデータ（フィールド名と値の組み合わせ）を追加
					ps.Add("text", text);
					//データを送信し、また受信する
					try
					{
						wc.UploadValues(url, ps);
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, url);
						DialogResult dr = MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。\n\n今回のみ接続しないようにしますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						if (dr == DialogResult.Yes)
						{
							ByActive = false;
						}
						return;
					}
					wc.Dispose();
				}
				else
				{
					BysMsg = text;
					//接続テスト
					try
					{
						tc = TC;
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "");
						DialogResult dr = MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。\n\n今回のみ接続しないようにしますか？", AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
						if (dr == DialogResult.Yes)
						{
							ByActive = false;
						}
						return;
					}
					//メッセージ送信
					using (NetworkStream ns = tc.GetStream())
					{
						using (BinaryWriter bw = new BinaryWriter(ns))
						{
							bw.Write(ByCmd);    // コマンド（ 0:メッセージ読み上げ）
							bw.Write(BySpd);    // 速度    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByTone);   // 音程    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByVol);    // 音量    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByVoice);  // 声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
							bw.Write(ByCode);   // 文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
							bw.Write(ByLength); // 文字列のbyte配列の長さ
							bw.Write(BybMsg);   // 文字列のbyte配列
						}
					}
					tc.Close();
				}
			}
			return;
		}

		public static void Bouyomi_Connectchk(bool showDialog = true)
		{
			BysMsg = "ゲームランチャーとの接続テストに成功しました。";

			if (ByType == 1)
			{
				string url = "http://127.0.0.1:" + ByPort + "/talk";
				System.Net.WebClient wc = new System.Net.WebClient();
				//NameValueCollectionの作成
				System.Collections.Specialized.NameValueCollection ps =
					new System.Collections.Specialized.NameValueCollection();
				//送信するデータ（フィールド名と値の組み合わせ）を追加
				ps.Add("text", BysMsg);
				//データを送信し、また受信する
				try
				{
					wc.UploadValues(url, ps);
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, url);
					if (showDialog)
					{
						MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return;
				}
				wc.Dispose();
				if (showDialog)
				{
					MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				//接続テスト
				try
				{
					TcpClient tc = TC;
					//メッセージ送信
					using (NetworkStream ns = tc.GetStream())
					{
						using (BinaryWriter bw = new BinaryWriter(ns))
						{
							bw.Write(ByCmd);    // コマンド（ 0:メッセージ読み上げ）
							bw.Write(BySpd);    // 速度    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByTone);   // 音程    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByVol);    // 音量    （-1:棒読みちゃん画面上の設定）
							bw.Write(ByVoice);  // 声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
							bw.Write(ByCode);   // 文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
							bw.Write(ByLength); // 文字列のbyte配列の長さ
							bw.Write(BybMsg);   // 文字列のbyte配列
						}
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "");
					if (showDialog)
					{
						MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
					return;
				}
				finally
				{
					if (TC != null)
					{
						TC.Close();
					}
				}

				if (showDialog)
				{
					MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}

		/// <summary>
		/// ゲーム管理INIの文字列を置換します
		/// </summary>
		/// <param name="beforeName">置換前文字列</param>
		/// <param name="afterName">置換後文字列</param>
		/// <param name="errMsg">エラーメッセージ</param>
		/// <returns>成功すればtrue、エラー発生時はfalse</returns>
		public static bool EditAllIniFile(string beforeName, string afterName, bool editFlg1, bool editFlg2, out int sucCount, out string errMsg)
		{
			errMsg = string.Empty;
			sucCount = -1;

			// Config最新化
			if (GLConfigLoad() == false)
			{
				errMsg = "Configロード中にエラー";
				return false;
			}

			// INI管理モードでない場合は拒否する
			if (SaveType != "I" && SaveType != "T")
			{
				errMsg = "データベースモードでは使用できません\nランチャーを再起動してから再度お試し下さい。";
				return false;
			}

			// 置換文字数チェック
			if (beforeName.Length < 2 || afterName.Length < 2)
			{
				errMsg = "置換文字列の文字数が不正です";
				return false;
			}

			// 置換処理
			if (!(GameMax >= 1))
			{
				errMsg = "登録されているゲーム数が少なすぎます";
				return false;
			}

			sucCount = 0;

			// ゲーム詳細取得
			try
			{
				for (int i = 1; i <= GameMax; i++)
				{
					string readini = GameDir + i + ".ini";
					string imgpassdata = string.Empty, passdata = string.Empty;
					string imgPathData = string.Empty, exePathData = string.Empty;
					bool wasChanged = false;

					if (File.Exists(readini))
					{
						// ini読込開始
						KeyNames[] getTargetValues = { KeyNames.imgpass, KeyNames.pass };
						string[] failedValues = { "", "" };
						string[] returnValues = IniRead(readini, "game", getTargetValues, failedValues);

						imgpassdata = returnValues[0];
						passdata = returnValues[1];

						// 実行ファイル
						if (editFlg1)
						{
							exePathData = passdata.Replace(beforeName, afterName);
							if (!passdata.Equals(exePathData))
							{
								IniWrite(readini, "game", KeyNames.pass, exePathData);
								wasChanged = true;
							}
						}

						// 画像ファイル
						if (editFlg2)
						{
							imgPathData = imgpassdata.Replace(beforeName, afterName);
							if (!imgpassdata.Equals(imgPathData))
							{
								IniWrite(readini, "game", KeyNames.imgpass, imgPathData);
								wasChanged = true;
							}
						}

						if (wasChanged)
						{
							sucCount++;
						}
					}
					else
					{
						// 個別ini存在しない場合
						DialogResult dr = MessageBox.Show("iniファイル読み込み中にエラー。 [" + MethodBase.GetCurrentMethod().Name + "]\nファイルが存在しません。\n\n処理を続行しますか？\n\n次のファイルの値は更新されていない可能性があります：\n" + readini,
										AppName,
										MessageBoxButtons.YesNo,
										MessageBoxIcon.Error);
						if (dr == DialogResult.No)
						{
							errMsg = "ユーザにより中断されました";
							return false;
						}
					}

				}
			}
			catch (Exception ex)
			{
				// 予期せぬエラー
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, string.Empty);
				MessageBox.Show("予期せぬエラーが発生しました。 [" + MethodBase.GetCurrentMethod().Name + "]\nファイルの値は正常に更新されていない可能性があります。\n\n" + ex.Message,
								AppName,
								MessageBoxButtons.OK,
								MessageBoxIcon.Error);

				return false;
			}
			return true;
		}

		/// <summary>
		/// エラーログを書き込みます
		/// </summary>
		/// <param name="errorMsg">エラーメッセージ</param>
		/// <param name="moduleName">モジュール名</param>
		/// <param name="addInfo">追加情報</param>
		public static void WriteErrorLog(string errorMsg, string moduleName, string addInfo = "")
		{
			// 改行をスペースに変換
			errorMsg = errorMsg.Replace("\n", "");
			addInfo = addInfo.Replace("\n", "");

			StringBuilder sb = new StringBuilder();
			sb.Append("[ERROR] [").Append(DateTime.Now).Append("] ");
			sb.Append("(").Append(moduleName).Append(") ");
			sb.Append(errorMsg).Append(" > ");
			sb.AppendLine(addInfo);
			File.AppendAllText(BaseDir + "error.log", sb.ToString());

			return;
		}

		/// <summary>
		/// データベース内のレコードを指定されたフォルダにINIで保存します
		/// </summary>
		/// <param name="targetWorkDir">保存先フォルダ</param>
		/// <returns>False：エラー</returns>
		public static bool DownloadDbDataToLocal(string targetWorkDir)
		{
			if (ByActive && ByRoG)
			{
				Bouyomiage("最新のゲームデータのオフラインバックアップを取得しています。しばらくお待ち下さい。");
			}

			bool isSuc = false;

			try
			{
				// 退避ディレクトリがある場合、削除する
				if (Directory.Exists(BaseDir + "_temp_db_bak"))
				{
					Directory.Delete(BaseDir + "_temp_db_bak", true);
				}

				// ターゲットパスが存在しているか確認
				if (File.Exists(targetWorkDir))
				{
					// ローカルファイルが存在する場合、退避する
					Directory.Move(targetWorkDir, BaseDir + "_temp_db_bak");
				}

				// 保存用フォルダ作成
				Directory.CreateDirectory(targetWorkDir);
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "[初期退避処理] Path: " + targetWorkDir);

				// ターゲットパスが存在していない場合
				if (!File.Exists(targetWorkDir))
				{
					// 退避ファイルが存在する場合、復元する
					Directory.Move(BaseDir + "_temp_db_bak", targetWorkDir);
				}
				return false;
			}

			// Configの最新化
			// GLConfigLoad();

			// データ取得
			DataTable dt = new DataTable();

			SqlConnection cn = SqlCon;
			MySqlConnection mcn = SqlCon2;

			try
			{
				// 読み込み処理
				if (SaveType == "D")
				{
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
						WriteIni("list", "game", sqlAns.ToString(), 0, targetWorkDir);
					}
					else
					{
						// ゲームが1つもない場合
						// 作成したディレクトリを削除する
						if (Directory.Exists(targetWorkDir))
						{
							Directory.Delete(targetWorkDir, true);
						}
						WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + SaveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);
						return true;
					}

					// DBから全ゲームデータを取り出す
					SqlCommand cm2 = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 60,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
									+ " FROM " + DbName + "." + DbTable
					};
					cm2.Connection = cn;

					using (var reader = cm2.ExecuteReader())
					{
						int count = 1;
						while (reader.Read())
						{
							// 1件ずつローカルINIに書く
							if (Convert.ToInt32(reader["ID"].ToString()) > 0)
							{
								string saveLocalIniPath = targetWorkDir + count + ".ini";
								KeyNames[] colNames =
								{
										KeyNames.name,
										KeyNames.imgpass,
										KeyNames.pass,
										KeyNames.execute_cmd,
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
										DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()),
										DecodeSQLSpecialChars(reader["IMG_PATH"].ToString()),
										DecodeSQLSpecialChars(reader["GAME_PATH"].ToString()),
										DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString()),
										(string.IsNullOrEmpty(reader["UPTIME"].ToString()) ? "0" : reader["UPTIME"].ToString()),
										(string.IsNullOrEmpty(reader["RUN_COUNT"].ToString()) ? "0" : reader["RUN_COUNT"].ToString()),
										DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString()),
										(string.IsNullOrEmpty(reader["AGE_FLG"].ToString()) ? Rate.ToString() : reader["AGE_FLG"].ToString()),
										reader["TEMP1"].ToString(),
										reader["LAST_RUN"].ToString(),
										reader["DCON_IMG"].ToString(),
										DecodeSQLSpecialChars(reader["MEMO"].ToString()),
										reader["STATUS"].ToString(),
										reader["EXTRACT_TOOL"].ToString(),
										reader["DB_VERSION"].ToString()
									};
								IniWrite(saveLocalIniPath, "game", colNames, keys);
							}
							count++;
						}
						IniWrite(targetWorkDir + "game.ini", "list", "game", (count - 1).ToString());
						isSuc = true;
					}
				}
				else if (SaveType == "M")
				{
					mcn.Open();

					// 全ゲーム数取得
					MySqlCommand mcm = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 30,
						CommandText = @"SELECT count(*) FROM " + DbTable
					};
					mcm.Connection = mcn;

					int sqlAns = Convert.ToInt32(mcm.ExecuteScalar().ToString());

					if (sqlAns > 0)
					{
						WriteIni("list", "game", sqlAns.ToString(), 0, targetWorkDir);
					}
					else
					{
						// ゲームが1つもない場合
						// 作成したディレクトリを削除する
						if (Directory.Exists(targetWorkDir))
						{
							Directory.Delete(targetWorkDir, true);
						}
						WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + SaveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);
						return true;
					}

					// DBから全ゲームデータを取り出す
					MySqlCommand mcm2 = new MySqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 60,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
									+ " FROM " + DbTable
					};
					mcm2.Connection = mcn;

					using (var reader = mcm2.ExecuteReader())
					{
						int count = 1;
						while (reader.Read())
						{
							// 1件ずつローカルINIに書く
							if (Convert.ToInt32(reader["ID"].ToString()) > 0)
							{
								string saveLocalIniPath = targetWorkDir + count + ".ini";
								KeyNames[] colNames =
								{
										KeyNames.name,
										KeyNames.imgpass,
										KeyNames.pass,
										KeyNames.execute_cmd,
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
										DecodeSQLSpecialChars(reader["GAME_NAME"].ToString()),
										DecodeSQLSpecialChars(reader["IMG_PATH"].ToString()),
										DecodeSQLSpecialChars(reader["GAME_PATH"].ToString()),
										DecodeSQLSpecialChars(reader["EXECUTE_CMD"].ToString()),
										(string.IsNullOrEmpty(reader["UPTIME"].ToString()) ? "0" : reader["UPTIME"].ToString()),
										(string.IsNullOrEmpty(reader["RUN_COUNT"].ToString()) ? "0" : reader["RUN_COUNT"].ToString()),
										DecodeSQLSpecialChars(reader["DCON_TEXT"].ToString()),
										(string.IsNullOrEmpty(reader["AGE_FLG"].ToString()) ? Rate.ToString() : reader["AGE_FLG"].ToString()),
										reader["TEMP1"].ToString(),
										reader["LAST_RUN"].ToString(),
										reader["DCON_IMG"].ToString(),
										DecodeSQLSpecialChars(reader["MEMO"].ToString()),
										reader["STATUS"].ToString(),
										reader["EXTRACT_TOOL"].ToString(),
										reader["DB_VERSION"].ToString()
									};
								IniWrite(saveLocalIniPath, "game", colNames, keys);
							}
							count++;
						}
						IniWrite(targetWorkDir + "game.ini", "list", "game", (count - 1).ToString());
						isSuc = true;
					}
				}

				// 退避ディレクトリが存在する場合、削除する
				if (Directory.Exists(BaseDir + "_temp_db_bak"))
				{
					Directory.Delete(BaseDir + "_temp_db_bak", true);
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + SaveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);

				// 退避ディレクトリが存在する場合、ロールバック
				if (Directory.Exists(BaseDir + "_temp_db_bak"))
				{
					if (Directory.Exists(targetWorkDir))
					{
						// バックアップフォルダがある場合、作業フォルダを削除
						Directory.Delete(targetWorkDir, true);
					}
					// 退避ディレクトリ復元
					Directory.Move(BaseDir + "_temp_db_bak", targetWorkDir);
				}
			}
			finally
			{
				if (SaveType == "D")
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
				}
				else if (SaveType == "M")
				{
					if (mcn.State == ConnectionState.Open)
					{
						mcn.Close();
					}
				}
			}

			return isSuc;
		}

		/// <summary>
		/// INIファイルをDBにINSERTします
		/// </summary>
		/// <param name="workDir">INSERTするINIディレクトリ</param>
		/// <param name="backupDir">バックアップINIディレクトリ</param>
		/// <param name="sCount">INSERT成功件数</param>
		/// <param name="fCount">INSERT失敗件数</param>
		/// <returns>1:バックアップ作成エラー 2:INSERT失敗 3:Catchエラー 4:復元エラー</returns>
		public static int InsertIni2Db(string workDir, string backupDir, out int tmpMaxGameCount, out int sCount, out int fCount)
		{
			// 変数宣言
			SqlConnection cn1 = SqlCon;
			SqlCommand cm1 = null; // TRUNCATE用
			SqlCommand cm2 = null; // INSERT用
			SqlTransaction tran1 = null;

			MySqlConnection mcn1 = SqlCon2;
			MySqlCommand mcm1 = null; // TRUNCATE用
			MySqlCommand mcm2 = null; // INSERT用
			MySqlTransaction mtran1 = null;

			tmpMaxGameCount = 0;
			int ans = 0;
			sCount = 0;
			fCount = 0;
			bool continueAdd = true;
			string localGameIni = Path.Combine(workDir, "game.ini");

			// ini全件数取得
			if (File.Exists(localGameIni))
			{
				tmpMaxGameCount = Convert.ToInt32(ReadIni("list", "game", "0", 0, workDir));
			}

			// データベースをバックアップ
			if (!DownloadDbDataToLocal(backupDir))
			{
				continueAdd = false;
				ans = 1;
			}

			// データベースのバックアップに成功した場合
			if (continueAdd)
			{
				try
				{
					if (SaveType == "D")
					{
						// TRUNCATE文作成
						cm1 = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"TRUNCATE TABLE " + DbName + "." + DbTable
						};
						cm1.Connection = cn1;

						cm2 = new SqlCommand();

						cn1.Open();

						// TRUNCATE実行
						cm1.ExecuteNonQuery();

						// TRANSACTION開始
						tran1 = cn1.BeginTransaction();

						// INSERTデータ取得
						for (int i = 1; i <= tmpMaxGameCount; i++)
						{
							// ini情報取得
							string readini = workDir + "\\" + i + ".ini";
							KeyNames[] keyName =
							{
									KeyNames.name,			// 0
									KeyNames.pass,			// 1
									KeyNames.imgpass,		// 2
									KeyNames.time,			// 3
									KeyNames.start,			// 4
									KeyNames.stat,			// 5
									KeyNames.rating,		// 6
									KeyNames.temp1,			// 7
									KeyNames.lastrun,		// 8
									KeyNames.dcon_img,		// 9
									KeyNames.memo,			// 10
									KeyNames.status,		// 11
									KeyNames.ini_version,	// 12
									KeyNames.execute_cmd,	// 13
									KeyNames.extract_tool	// 14
								};
							string[] failedVal =
							{
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
									"未プレイ",
									DBVer,
									"",
									"0"
								};
							string[] returnVal = new string[keyName.Length];

							if (File.Exists(readini))
							{
								// データ取得
								returnVal = IniRead(readini, "game", keyName, failedVal);
								sCount++;
							}
							else
							{
								fCount++;
								WriteErrorLog("ファイルが存在しません。該当ファイルのINSERT処理をスキップします。", MethodBase.GetCurrentMethod().Name, readini);
								continue;
							}

							// INSERTコマンド作成
							cm2 = new SqlCommand()
							{
								CommandType = CommandType.Text,
								CommandTimeout = 30,
								CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, DB_VERSION, EXECUTE_CMD, EXTRACT_TOOL ) VALUES ( @game_name, @game_path, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @temp1, @last_run, @dcon_img, @memo, @status, @db_version, @execute_cmd, @extract_tool )"
							};
							cm2.Connection = cn1;
							cm2.Transaction = tran1;

							cm2.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(returnVal[0]));
							cm2.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(returnVal[1]));
							cm2.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(returnVal[2]));
							cm2.Parameters.AddWithValue("@uptime", returnVal[3]);
							cm2.Parameters.AddWithValue("@run_count", returnVal[4]);
							cm2.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(returnVal[5]));
							cm2.Parameters.AddWithValue("@age_flg", returnVal[6]);
							cm2.Parameters.AddWithValue("@temp1", returnVal[7]);
							cm2.Parameters.AddWithValue("@last_run", string.IsNullOrEmpty(returnVal[8]) ? "1900-01-01 00:00:00" : returnVal[8]);
							cm2.Parameters.AddWithValue("@dcon_img", returnVal[9]);
							cm2.Parameters.AddWithValue("@memo", EncodeSQLSpecialChars(returnVal[10]));
							cm2.Parameters.AddWithValue("@status", returnVal[11]);
							cm2.Parameters.AddWithValue("@db_version", returnVal[12]);
							cm2.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(returnVal[13]));
							cm2.Parameters.AddWithValue("@extract_tool", returnVal[14]);

							// INSERT文実行
							cm2.ExecuteNonQuery();
						}

						if (fCount > 0)
						{
							// 登録失敗のものがある場合はロールバック
							tran1.Rollback();
							WriteErrorLog("INSERT処理をスキップしたファイルがあるためロールバックしました。", MethodBase.GetCurrentMethod().Name, "スキップ：" + fCount + "件");
							ans = 2;
						}
						else
						{
							// 問題なければコミット
							tran1.Commit();
							// オフラインINIのフラグ変更
							WriteIni("list", "dbupdate", "0", 0, workDir);
							Directory.Delete(backupDir, true);
						}
					}
					else
					{
						// TRUNCATE文作成
						mcm1 = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"TRUNCATE TABLE " + DbTable
						};
						mcm1.Connection = mcn1;

						mcm2 = new MySqlCommand();

						mcn1.Open();

						// TRUNCATE実行
						mcm1.ExecuteNonQuery();

						Console.WriteLine("TRUNCATE COMPLETE!");

						// TRANSACTION開始
						mtran1 = mcn1.BeginTransaction();

						// INSERTデータ取得
						for (int i = 1; i <= tmpMaxGameCount; i++)
						{
							Console.WriteLine("GetInfo(" + i + " / " + tmpMaxGameCount + ")");
							// ini情報取得
							string readini = workDir + "\\" + i + ".ini";
							KeyNames[] keyName =
							{
									KeyNames.name,			// 0
									KeyNames.pass,			// 1
									KeyNames.imgpass,		// 2
									KeyNames.time,			// 3
									KeyNames.start,			// 4
									KeyNames.stat,			// 5
									KeyNames.rating,		// 6
									KeyNames.temp1,			// 7
									KeyNames.lastrun,		// 8
									KeyNames.dcon_img,		// 9
									KeyNames.memo,			// 10
									KeyNames.status,		// 11
									KeyNames.ini_version,	// 12
									KeyNames.execute_cmd,	// 13
									KeyNames.extract_tool	// 14
								};
							string[] failedVal =
							{
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
									"未プレイ",
									DBVer,
									"",
									"0"
								};
							string[] returnVal = new string[keyName.Length];

							if (File.Exists(readini))
							{
								// データ取得
								Console.WriteLine("file found.");
								returnVal = IniRead(readini, "game", keyName, failedVal);
								sCount++;
							}
							else
							{
								fCount++;
								Console.WriteLine("file not found.");
								WriteErrorLog("ファイルが存在しません。該当ファイルのINSERT処理をスキップします。", MethodBase.GetCurrentMethod().Name, readini);
								continue;
							}

							// INSERTコマンド作成
							mcm2 = new MySqlCommand()
							{
								CommandType = CommandType.Text,
								CommandTimeout = 30,
								CommandText = @"INSERT INTO " + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, DB_VERSION, EXECUTE_CMD, EXTRACT_TOOL ) VALUES ( @game_name, @game_path, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @temp1, @last_run, @dcon_img, @memo, @status, @db_version, @execute_cmd, @extract_tool );"
							};
							mcm2.Connection = mcn1;
							mcm2.Transaction = mtran1;

							mcm2.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(returnVal[0]));
							mcm2.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(returnVal[1]));
							mcm2.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(returnVal[2]));
							mcm2.Parameters.AddWithValue("@uptime", returnVal[3]);
							mcm2.Parameters.AddWithValue("@run_count", returnVal[4]);
							mcm2.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(returnVal[5]));
							mcm2.Parameters.AddWithValue("@age_flg", returnVal[6]);
							mcm2.Parameters.AddWithValue("@temp1", returnVal[7]);
							mcm2.Parameters.AddWithValue("@last_run", string.IsNullOrEmpty(returnVal[8]) ? "1900-01-01 00:00:00" : returnVal[8]);
							mcm2.Parameters.AddWithValue("@dcon_img", returnVal[9]);
							mcm2.Parameters.AddWithValue("@memo", EncodeSQLSpecialChars(returnVal[10]));
							mcm2.Parameters.AddWithValue("@status", returnVal[11]);
							mcm2.Parameters.AddWithValue("@db_version", returnVal[12]);
							mcm2.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(returnVal[13]));
							mcm2.Parameters.AddWithValue("@extract_tool", returnVal[14]);

							// INSERT文実行
							Console.WriteLine("INSERT(" + i + " / " + tmpMaxGameCount + ") EXECUTE!");
							mcm2.ExecuteNonQuery();
							Console.WriteLine("INSERT(" + i + " / " + tmpMaxGameCount + ") COMPLETE!");
						}

						if (fCount > 0)
						{
							// 登録失敗のものがある場合はロールバック
							mtran1.Rollback();
							WriteErrorLog("INSERT処理をスキップしたファイルがあるためロールバックしました。", MethodBase.GetCurrentMethod().Name, "スキップ：" + fCount + "件");
							ans = 2;
						}
						else
						{
							// 問題なければコミット
							mtran1.Commit();
							// オフラインINIのフラグ変更
							WriteIni("list", "dbupdate", "0", 0, workDir);
							Directory.Delete(backupDir, true);
						}
					}
				}
				catch (Exception ex)
				{
					// ロールバック
					ans = 3;
					if (SaveType == "D")
					{
						tran1.Rollback();
					}
					else
					{
						mtran1.Rollback();
					}
					if (!Ini2DbErrorRollback(backupDir))
					{
						ans = 4;
					}

					// エラー処理
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "cm1：" + (cm1 == null ? String.Empty : cm1.CommandText) + " / cm2：" + (cm2 == null ? String.Empty : cm2.CommandText) + " / mcm1：" + (mcm1 == null ? String.Empty : mcm1.CommandText) + " / mcm2：" + (mcm2 == null ? String.Empty : mcm2.CommandText));
				}
				finally
				{
					if (SaveType == "D")
					{
						if (cn1.State == ConnectionState.Open)
						{
							cn1.Close();
						}
					}
					else
					{
						if (mcn1.State == ConnectionState.Open)
						{
							mcn1.Close();
						}
					}
				}
			}
			return ans;
		}

		/// <summary>
		/// データベース操作時にエラーが発生した場合、バックアップディレクトリの値を登録し直します
		/// </summary>
		/// <param name="backupDir">バックアップINIのディレクトリ</param>
		/// <returns>正常にロールバックできた場合:true</returns>
		public static bool Ini2DbErrorRollback(string backupDir)
		{
			// 変数宣言
			SqlConnection cn1 = SqlCon;
			SqlCommand cm1 = new SqlCommand();
			SqlCommand cm2 = new SqlCommand();

			MySqlConnection mcn1 = SqlCon2;
			MySqlCommand mcm1 = new MySqlCommand();
			MySqlCommand mcm2 = new MySqlCommand();

			string localGameIni = Path.Combine(backupDir, "game.ini");
			int tmpMaxGameCount = 0;
			bool ans = true;

			// ini全件数取得
			if (File.Exists(localGameIni))
			{
				tmpMaxGameCount = Convert.ToInt32(ReadIni("list", "game", "0", 0, backupDir));
			}

			try
			{
				if (SaveType == "D")
				{
					cn1.Open();

					// INSERTデータ取得
					for (int i = 1; i <= tmpMaxGameCount; i++)
					{
						// ini情報取得
						string readini = backupDir + "\\" + i + ".ini";
						KeyNames[] keyName =
						{
									KeyNames.name,			// 0
									KeyNames.pass,			// 1
									KeyNames.imgpass,		// 2
									KeyNames.time,			// 3
									KeyNames.start,			// 4
									KeyNames.stat,			// 5
									KeyNames.rating,		// 6
									KeyNames.temp1,			// 7
									KeyNames.lastrun,		// 8
									KeyNames.dcon_img,		// 9
									KeyNames.memo,			// 10
									KeyNames.status,		// 11
									KeyNames.ini_version,	// 12
									KeyNames.execute_cmd,	// 13
									KeyNames.extract_tool	// 14
								};
						string[] failedVal =
						{
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
									"未プレイ",
									DBVer,
									"",
									"0"
								};
						string[] returnVal = new string[keyName.Length];

						if (File.Exists(readini))
						{
							// データ取得
							returnVal = IniRead(readini, "game", keyName, failedVal);
							// タイトルのエスケープ
							returnVal[0] = EncodeSQLSpecialChars(returnVal[0]);
						}
						else
						{
							ans = false;
							WriteErrorLog("ファイルが存在しません。該当ファイルのINSERT処理をスキップします。", MethodBase.GetCurrentMethod().Name, readini);
							continue;
						}

						// TRUNCATEコマンド作成
						cm1 = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"TRUNCATE TABLE " + DbName + "." + DbTable
						};
						cm1.Connection = cn1;

						// TRUNCATE文実行
						cm1.ExecuteNonQuery();

						// INSERTコマンド作成
						cm2 = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, DB_VERSION, EXECUTE_CMD, EXTRACT_TOOL ) VALUES ( @game_name, @game_path, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @temp1, @last_run, @dcon_img, @memo, @status, @db_version, @execute_cmd, @extract_tool )"
						};
						cm2.Connection = cn1;
						cm2.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(returnVal[0]));
						cm2.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(returnVal[1]));
						cm2.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(returnVal[2]));
						cm2.Parameters.AddWithValue("@uptime", EncodeSQLSpecialChars(returnVal[3]));
						cm2.Parameters.AddWithValue("@run_count", returnVal[4]);
						cm2.Parameters.AddWithValue("@dcon_text", returnVal[5]);
						cm2.Parameters.AddWithValue("@age_flg", returnVal[6]);
						cm2.Parameters.AddWithValue("@temp1", EncodeSQLSpecialChars(returnVal[7]));
						cm2.Parameters.AddWithValue("@last_run", string.IsNullOrEmpty(returnVal[8]) ? "1900-01-01 00:00:00" : returnVal[8]);
						cm2.Parameters.AddWithValue("@dcon_img", EncodeSQLSpecialChars(returnVal[9]));
						cm2.Parameters.AddWithValue("@memo", EncodeSQLSpecialChars(returnVal[10]));
						cm2.Parameters.AddWithValue("@status", EncodeSQLSpecialChars(returnVal[11]));
						cm2.Parameters.AddWithValue("@db_version", EncodeSQLSpecialChars(returnVal[12]));
						cm2.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(returnVal[13]));
						cm2.Parameters.AddWithValue("@extract_tool", returnVal[14]);

						// INSERT文実行
						cm2.ExecuteNonQuery();
					}
				}
				else
				{
					mcn1.Open();

					// INSERTデータ取得
					for (int i = 1; i <= tmpMaxGameCount; i++)
					{
						// ini情報取得
						string readini = backupDir + "\\" + i + ".ini";
						KeyNames[] keyName =
						{
									KeyNames.name,			// 0
									KeyNames.pass,			// 1
									KeyNames.imgpass,		// 2
									KeyNames.time,			// 3
									KeyNames.start,			// 4
									KeyNames.stat,			// 5
									KeyNames.rating,		// 6
									KeyNames.temp1,			// 7
									KeyNames.lastrun,		// 8
									KeyNames.dcon_img,		// 9
									KeyNames.memo,			// 10
									KeyNames.status,		// 11
									KeyNames.ini_version,	// 12
									KeyNames.execute_cmd,	// 13
									KeyNames.extract_tool	// 14
								};
						string[] failedVal =
						{
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
									"未プレイ",
									DBVer,
									"",
									"0"
								};
						string[] returnVal = new string[keyName.Length];

						if (File.Exists(readini))
						{
							// データ取得
							returnVal = IniRead(readini, "game", keyName, failedVal);
							// タイトルのエスケープ
							returnVal[0] = EncodeSQLSpecialChars(returnVal[0]);
						}
						else
						{
							ans = false;
							WriteErrorLog("ファイルが存在しません。該当ファイルのINSERT処理をスキップします。", MethodBase.GetCurrentMethod().Name, readini);
							continue;
						}

						// TRUNCATEコマンド作成
						mcm1 = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"TRUNCATE TABLE " + DbTable
						};
						mcm1.Connection = mcn1;

						// TRUNCATE文実行
						mcm1.ExecuteNonQuery();

						// INSERTコマンド作成
						mcm2 = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"INSERT INTO " + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, DB_VERSION, EXECUTE_CMD, EXTRACT_TOOL ) VALUES ( @game_name, @game_path, @img_path, @uptime, @run_count, @dcon_text, @age_flg, @temp1, @last_run, @dcon_img, @memo, @status, @db_version, @execute_cmd, @extract_tool )"
						};
						mcm2.Connection = mcn1;
						mcm2.Parameters.AddWithValue("@game_name", EncodeSQLSpecialChars(returnVal[0]));
						mcm2.Parameters.AddWithValue("@game_path", EncodeSQLSpecialChars(returnVal[1]));
						mcm2.Parameters.AddWithValue("@img_path", EncodeSQLSpecialChars(returnVal[2]));
						mcm2.Parameters.AddWithValue("@uptime", returnVal[3]);
						mcm2.Parameters.AddWithValue("@run_count", returnVal[4]);
						mcm2.Parameters.AddWithValue("@dcon_text", EncodeSQLSpecialChars(returnVal[5]));
						mcm2.Parameters.AddWithValue("@age_flg", returnVal[6]);
						mcm2.Parameters.AddWithValue("@temp1", returnVal[7]);
						mcm2.Parameters.AddWithValue("@last_run", string.IsNullOrEmpty(returnVal[8]) ? "1900-01-01 00:00:00" : returnVal[8]);
						mcm2.Parameters.AddWithValue("@dcon_img", returnVal[9]);
						mcm2.Parameters.AddWithValue("@memo", EncodeSQLSpecialChars(returnVal[10]));
						mcm2.Parameters.AddWithValue("@status", returnVal[11]);
						mcm2.Parameters.AddWithValue("@db_version", returnVal[12]);
						mcm2.Parameters.AddWithValue("@execute_cmd", EncodeSQLSpecialChars(returnVal[13]));
						mcm2.Parameters.AddWithValue("@extract_tool", returnVal[14]);

						// INSERT文実行
						mcm2.ExecuteNonQuery();
					}
				}
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "cm1：" + (cm1 != null ? cm1.CommandText : string.Empty) + "cm2：" + (cm2 != null ? cm2.CommandText : string.Empty) + "mcm1：" + (mcm1 != null ? mcm1.CommandText : string.Empty) + "mcm2：" + (mcm2 != null ? mcm2.CommandText : string.Empty));
				ans = false;
			}
			finally
			{
				if (cn1.State == ConnectionState.Open)
				{
					cn1.Close();
				}
			}
			return ans;
		}


		public static bool GenerateExtractCmd(int extractID, string gamePath, string gameArgs, out string extractAppPath, out string extractAppArgs)
		{
			StringBuilder appPath = new StringBuilder();
			StringBuilder appArgs = new StringBuilder();
			extractAppPath = string.Empty;
			extractAppArgs = string.Empty;

			// Indexが0の場合はツール未選択なので返す
			if (extractID == 0)
			{
				return false;
			}

			switch (extractID)
			{
				case 1: // krkr
					appPath.Append(ExtractKrkrPath);
					if (ExtractKrkrArg.Length != 0)
					{
						appArgs.Append(ExtractKrkrArg);
						appArgs.Append(ExtractKrkrArg).Append(" ");
					}
					if (ExtractKrkrAddGameArg)
					{
						appArgs.Append("\"");
					}
					break;
				case 2: // krkrz
					appPath.Append(ExtractKrkrzPath);
					if (ExtractKrkrzArg.Length != 0)
					{
						appArgs.Append(ExtractKrkrzArg);
						appArgs.Append(ExtractKrkrzArg).Append(" ");
					}
					if (ExtractKrkrzAddGameArg)
					{
						appArgs.Append("\"");
					}
					break;
				case 3: // krkrDump
					appPath.Append(ExtractKrkrDumpPath);
					if (ExtractKrkrDumpArg.Length != 0)
					{
						appArgs.Append(ExtractKrkrDumpArg);
						appArgs.Append(ExtractKrkrDumpArg).Append(" ");
					}
					if (ExtractKrkrDumpAddGameArg)
					{
						appArgs.Append("\"");
					}
					break;
				case 4: // Custom1
					appPath.Append(ExtractCustom1Path);
					if (ExtractCustom1Arg.Length != 0)
					{
						appArgs.Append(ExtractCustom1Arg);
						appArgs.Append(ExtractCustom1Arg).Append(" ");
					}
					if (ExtractCustom1AddGameArg)
					{
						appArgs.Append("\"");
					}
					break;
				case 5: // Custom2
					appPath.Append(ExtractCustom2Path);
					if (ExtractCustom2Arg.Length != 0)
					{
						appArgs.Append(ExtractCustom2Arg);
						appArgs.Append(ExtractCustom2Arg).Append(" ");
					}
					if (ExtractCustom2AddGameArg)
					{
						appArgs.Append("\"");
					}
					break;
			}
			appArgs.Append("\"").Append(gamePath);
			switch (extractID)
			{
				case 1: // krkr
					if (ExtractKrkrAddGameArg)
					{
						appArgs.Append(" ").Append(gameArgs);
						appArgs.Append("\"");
					}
					break;
				case 2: // krkrz
					if (ExtractKrkrzAddGameArg)
					{
						appArgs.Append(" ").Append(gameArgs);
						appArgs.Append("\"");
					}
					break;
				case 3: // krkrDump
					if (ExtractKrkrDumpAddGameArg)
					{
						appArgs.Append(" ").Append(gameArgs);
						appArgs.Append("\"");
					}
					break;
				case 4: // Custom1
					if (ExtractCustom1AddGameArg)
					{
						appArgs.Append(" ").Append(gameArgs);
						appArgs.Append("\"");
					}
					break;
				case 5: // Custom2
					if (ExtractCustom2AddGameArg)
					{
						appArgs.Append(" ").Append(gameArgs);
						appArgs.Append("\"");
					}
					break;
			}
			appArgs.Append("\"");

			extractAppPath = appPath.ToString();
			extractAppArgs = appArgs.ToString();

			return true;
		}

		public static string EncodeSQLSpecialChars(string rawText)
		{
			string result = string.Empty;

			// 引数が空の場合はリターン
			if (string.IsNullOrEmpty(rawText))
			{
				return string.Empty;
			}

			result = rawText.Replace("'", "''").Replace("\"", "\\\"").Replace("\\", "\\\\");

			return result;
		}

		public static string DecodeSQLSpecialChars(string rawText)
		{
			string result = string.Empty;

			// 引数が空の場合はリターン
			if (string.IsNullOrEmpty(rawText))
			{
				return string.Empty;
			}

			result = rawText.Replace("''", "'").Replace("\\\"", "\"").Replace("\\\\", "\\");

			return result;
		}
	}
}
