using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace glc_cs
{
	class General
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
		/// 変数管理
		/// </summary>
		public partial class Var
		{
			// 共通
			/// <summary>
			/// アプリケーション名
			/// </summary>
			protected static readonly string appname = "Game Launcher C# Edition";

			/// <summary>
			/// アプリケーションバージョン
			/// </summary>
			protected static readonly string appver = "1.00";

			/// <summary>
			/// アプリケーションビルド番号
			/// </summary>
			protected static readonly string appbuild = "25.22.09.17";

			/// <summary>
			/// ゲームディレクトリ(作業ディレクトリ)
			/// </summary>
			protected static string gamedir = string.Empty;

			/// <summary>
			/// アプリケーションディレクトリ(ランチャー実行パス)
			/// </summary>
			protected static string basedir = AppDomain.CurrentDomain.BaseDirectory;

			/// <summary>
			/// ゲーム情報保管iniパス
			/// </summary>
			protected static string gameini;

			/// <summary>
			/// ゲーム情報保管dbパス
			/// </summary>
			protected static string gamedb;

			/// <summary>
			/// アプリケーション設定パス(config.ini)
			/// </summary>
			protected static string configini = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

			/// <summary>
			/// ゲーム総数
			/// </summary>
			protected static int gamemax = 0;

			/// <summary>
			/// 選択中のゲームID（データベース接続時）
			/// </summary>
			protected static string currentGameDBVal = "-1";

			/// <summary>
			/// 背景画像パス
			/// </summary>
			protected static string bgimg = null;

			/// <summary>
			/// DiscordConnectorパス
			/// </summary>
			protected static string dconpath = string.Empty;

			/// <summary>
			/// Discord connector Application ID
			/// </summary>
			protected static string dconAppID = string.Empty;


			// データ保存方法関係
			/// <summary>
			/// データ保存方法
			/// </summary>
			protected static string saveType = string.Empty;

			/// <summary>
			/// オフラインセーブフラグ
			/// </summary>
			protected static bool offlineSave = false;

			/// <summary>
			/// データベースのURL
			/// </summary>
			protected static string dbUrl = string.Empty;

			/// <summary>
			/// データベース名
			/// </summary>
			protected static string dbName = string.Empty;

			/// <summary>
			/// テーブル名
			/// </summary>
			protected static string dbTable = string.Empty;

			/// <summary>
			/// データベース接続ユーザ名
			/// </summary>
			protected static string dbUser = string.Empty;

			/// <summary>
			/// データベース接続パスワード
			/// </summary>
			protected static string dbPass = string.Empty;

			//棒読みちゃん関係
			/// <summary>
			/// 棒読みちゃん 有効フラグ
			/// </summary>
			protected static bool byActive = false;

			/// <summary>
			/// 棒読みちゃん 読み上げメッセージ
			/// </summary>
			protected static string bysMsg = "Game Launcherと接続しました。";

			/// <summary>
			/// 棒読みちゃん タイプ
			/// </summary>
			protected static int byType = 0;

			/// <summary>
			/// 棒読みちゃん 文字エンコーディング
			/// </summary>
			protected static byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)

			/// <summary>
			/// 棒読みちゃん 詳細設定群
			/// </summary>
			protected static Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;

			/// <summary>
			/// 棒読みちゃん 読み上げ文字数
			/// </summary>
			protected static Int32 byLength = 0;

			/// <summary>
			/// 棒読みちゃん 読み上げメッセージのbyte配列
			/// </summary>
			protected static byte[] bybMsg;

			/// <summary>
			/// 棒読みちゃん 接続ホスト名
			/// </summary>
			protected static string byHost = "127.0.0.1";

			/// <summary>
			/// 棒読みちゃん 接続ポート番号
			/// </summary>
			protected static int byPort = 50001;

			/// <summary>
			/// 棒読みちゃん TCP接続用
			/// </summary>
			protected static TcpClient tc = null;

			/// <summary>
			/// 棒読みちゃん ランチャー起動時に読み上げ
			/// </summary>
			protected static bool byRoW = false;

			/// <summary>
			/// 棒読みちゃん ゲーム起動時に読み上げ
			/// </summary>
			protected static bool byRoS = false;

			/// <summary>
			/// Discord Connector 有効フラグ
			/// </summary>
			protected static bool dconnect = false;

			/// <summary>
			/// Discord Connector レーティング設定
			/// </summary>
			protected static Int32 rate = 0;

			/// <summary>
			/// グリッドサイズ最大化フラグ
			/// </summary>
			protected static bool gridMax = false;



			/// <summary>
			/// アプリケーション名を返却します
			/// </summary>
			public static string AppName
			{
				get { return appname; }
			}

			/// <summary>
			/// アプリケーションのバージョンを返却します
			/// </summary>
			public static string AppVer
			{
				get { return appver; }
			}

			/// <summary>
			/// アプリケーションのビルド番号を返却します
			/// </summary>
			public static string AppBuild
			{
				get { return appbuild; }
			}

			/// <summary>
			/// ゲームの作業ディレクトリを設定/返却します
			/// </summary>
			public static string GameDir
			{
				get { return gamedir; }
				set { gamedir = value; }
			}

			/// <summary>
			/// アプリケーションの作業ディレクトリを返却します
			/// </summary>
			public static string BaseDir
			{
				get { return basedir; }
			}

			/// <summary>
			/// ゲーム統括管理iniのパスを設定/返却します
			/// </summary>
			public static string GameIni
			{
				get { return gameini; }
				set { gameini = value; }
			}

			/// <summary>
			/// ゲーム情報が格納されているDBのパスを設定/返却します
			/// </summary>
			public static string GameDb
			{
				get { return gamedb; }
				set { gamedb = value; }
			}

			/// <summary>
			/// アプリケーション設定が格納されているiniファイルのパスを返却します
			/// </summary>
			public static string ConfigIni
			{
				get { return configini; }
			}

			/// <summary>
			/// ゲームの最大数を設定/返却します
			/// </summary>
			public static int GameMax
			{
				get { return gamemax; }
				set { gamemax = value; }
			}

			/// <summary>
			/// 現在選択しているゲームのIDを設定/返却します。データベース接続時のみ使用します。
			/// </summary>
			public static string CurrentGameDbVal
			{
				get { return currentGameDBVal; }
				set { currentGameDBVal = value; }
			}

			/// <summary>
			/// ゲームの最大数を設定/返却します
			/// </summary>
			public static string BgImg
			{
				get { return bgimg; }
				set { bgimg = value; }
			}

			/// <summary>
			/// Discord Connectorのパスを設定/返却します
			/// </summary>
			public static string DconPath
			{
				get { return dconpath; }
				set { dconpath = value; }
			}

			/// <summary>
			/// Discord ConnectorのアプリケーションIDを設定/返却します
			/// </summary>
			public static string DconAppID
			{
				get { return dconAppID; }
				set { dconAppID = value; }
			}

			/// <summary>
			/// ゲーム保存方法
			/// </summary>
			public static string SaveType
			{
				get { return saveType; }
				set { saveType = value; }
			}

			/// <summary>
			/// オフラインセーブフラグ
			/// </summary>
			public static bool OfflineSave
			{
				get { return offlineSave; }
				set { offlineSave = value; }
			}

			public static string DbUrl
			{
				get { return dbUrl; }
				set { dbUrl = value; }
			}

			public static string DbName
			{
				get { return dbName; }
				set { dbName = value; }
			}

			public static string DbTable
			{
				get { return dbTable; }
				set { dbTable = value; }
			}

			public static string DbUser
			{
				get { return dbUser; }
				set { dbUser = value; }
			}

			public static string DbPass
			{
				get { return dbPass; }
				set { dbPass = value; }
			}

			public static SqlConnection SqlCon
			{
				get
				{
					return new SqlConnection(
						new SqlConnectionStringBuilder()
						{
							IntegratedSecurity = false,
							//InitialCatalog = dbName,
							DataSource = DbUrl,
							UserID = DbUser,
							Password = DbPass
						}.ToString()
					);
				}
			}

			/// <summary>
			/// 棒読みちゃんの有効フラグを設定/返却します
			/// </summary>
			public static bool ByActive
			{
				get { return byActive; }
				set { byActive = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージを設定/返却します
			/// </summary>
			public static string BysMsg
			{
				get { return bysMsg; }
				set { bysMsg = value; }
			}

			/// <summary>
			/// 棒読みちゃんとの接続方法（TCP/HTTP）
			/// </summary>
			public static int ByType
			{
				get { return byType; }
				set { byType = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージの文字エンコーディング値を返却します
			/// </summary>
			public static byte ByCode
			{
				get { return byCode; }
			}

			/// <summary>
			/// 棒読みちゃんのボイス設定を返却します
			/// </summary>
			public static Int16 ByVoice
			{
				get { return byVoice; }
			}

			/// <summary>
			/// 棒読みちゃんの音量設定を返却します
			/// </summary>
			public static Int16 ByVol
			{
				get { return byVol; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げ速度を返却します
			/// </summary>
			public static Int16 BySpd
			{
				get { return bySpd; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げトーンを返却します
			/// </summary>
			public static Int16 ByTone
			{
				get { return byTone; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げコマンドを返却します
			/// </summary>
			public static Int16 ByCmd
			{
				get { return byCmd; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージの長さを返却します
			/// </summary>
			public static Int32 ByLength
			{
				get { return byLength = BybMsg.Length; }
				set { byLength = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージを読み上げ可能なbyte配列に変換して返却します
			/// </summary>
			public static byte[] BybMsg
			{
				get { return bybMsg = Encoding.UTF8.GetBytes(BysMsg); }
			}

			/// <summary>
			/// 棒読みちゃんのホスト名を設定/返却します
			/// </summary>
			public static string ByHost
			{
				get { return byHost; }
				set { byHost = value; }
			}

			/// <summary>
			/// 棒読みちゃんのポート番号を設定/返却します
			/// </summary>
			public static int ByPort
			{
				get { return byPort; }
				set { byPort = value; }
			}

			/// <summary>
			/// 棒読みちゃん接続時に、ランチャー起動時の読み上げフラグを設定/返却します
			/// </summary>
			public static bool ByRoW
			{
				get { return byRoW; }
				set { byRoW = value; }
			}

			/// <summary>
			/// 棒読みちゃん接続時に、ゲーム実行／終了時の読み上げのフラグを設定/返却します
			/// </summary>
			public static bool ByRoS
			{
				get { return byRoS; }
				set { byRoS = value; }
			}

			/// <summary>
			/// 棒読みちゃんとのTCP接続を構築し、その値を返却します。
			/// 受け手側でTcpClientに代入して使用します。
			/// </summary>
			public static TcpClient TC
			{
				get { return tc = new TcpClient(ByHost, ByPort); }
			}

			/// <summary>
			/// Discord Connectorの有効フラグです。
			/// </summary>
			public static bool Dconnect
			{
				get { return dconnect; }
				set { dconnect = Convert.ToBoolean(value); }
			}

			public static Int32 Rate
			{
				get { return rate; }
				set { rate = value; }
			}
			public static bool GridMax
			{
				get { return gridMax; }
				set { gridMax = value; }
			}


			public static bool GLConfigLoad()
			{
				MyBase64str base64 = new MyBase64str();

				if (SaveType == "T")
				{
					return true;
				}

				if (File.Exists(ConfigIni))
				{
					// config.ini 存在する場合
					GameDir = IniRead(ConfigIni, "default", "directory", BaseDir) + "Data";
					GameIni = GameDir + "\\game.ini";
					GameDb = IniRead(ConfigIni, "default", "database", string.Empty);
					DconPath = ReadIni("connect", "dconpath", "-1");

					SaveType = ReadIni("general", "save", "I");
					OfflineSave = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "OfflineSave", "0")));
					DbUrl = ReadIni("connect", "DBURL", string.Empty);
					DbName = ReadIni("connect", "DBName", string.Empty);
					DbTable = ReadIni("connect", "DBTable", string.Empty);
					DbUser = ReadIni("connect", "DBUser", string.Empty);
					try
					{
						DbPass = base64.Decode(ReadIni("connect", "DBPass", string.Empty));
					}
					catch
					{
						DbPass = string.Empty;
					}

					if (SaveType == "I")
					{
						// ini
						GameMax = Convert.ToInt32(IniRead(GameIni, "list", "game", "0"));
					}
					else
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
							WriteErrorLog(ex.Message, "GLConfigLoad", cm.CommandText);
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

					//棒読みちゃん設定読み込み
					ByActive = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byActive", "0")));
					ByType = Convert.ToInt32(ReadIni("connect", "byType", "0"));
					ByHost = ReadIni("connect", "byHost", "127.0.0.1");
					ByPort = Convert.ToInt32(ReadIni("connect", "byPort", "50001"));
					ByRoW = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoW", "0")));
					ByRoS = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoS", "0")));

					// 総合
					BgImg = ReadIni("imgd", "bgimg", string.Empty);

					// dcon設定
					Dconnect = Convert.ToBoolean(Convert.ToInt32(ReadIni("checkbox", "dconnect", "0")));
					DconAppID = ReadIni("connect", "dconappid", string.Empty);
					Rate = Convert.ToInt32(ReadIni("checkbox", "rate", "0"));
				}
				else
				{
					// ゲーム保存方法をiniに設定
					SaveType = "I";

					// config.ini 存在しない場合
					GameDir = BaseDir + "Data";
					GameIni = GameDir + "\\game.ini";
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

					//棒読みちゃん設定読み込み
					ByActive = false;
					ByType = 0;
					ByHost = "127.0.0.1";
					ByPort = 50001;
					ByRoW = false;
					ByRoS = false;

					// 総合
					BgImg = null;

					// dcon設定
					Dconnect = false;
					Rate = 0;
				}

				return true;
			}

			public static string IniRead(String filename, String sec, String key, String failedval)
			{
				String ans = "";
				StringBuilder data = new StringBuilder(1024);
				try
				{
					GetPrivateProfileString(
						sec,
						key,
						failedval,
						data,
						1024,
						filename);
				}
				catch (Exception ex)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("ファイルパス：").Append(filename);
					sb.Append("セクション：").Append(sec);
					sb.Append("キー：").Append(key);
					sb.Append("値：").Append(data);

					WriteErrorLog(ex.Message, "IniRead", sb.ToString());
				}
				ans = data.ToString();
				return ans;
			}

			/// <summary>
			/// 指定されたINIに値を書き込みます
			/// </summary>
			/// <param name="filename">INIパス</param>
			/// <param name="sec">セクション名</param>
			/// <param name="key">キー値</param>
			/// <param name="data">値</param>
			public static void IniWrite(String filename, String sec, String key, String data)
			{
				try
				{
					if (!File.Exists(filename))
					{
						if (!File.Exists(System.IO.Path.GetDirectoryName(filename)))
						{
							Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filename));
						}
						File.Create(filename).Close();
					}
					WritePrivateProfileString(
									sec,
									key,
									data.ToString(),
									filename);
				}
				catch (Exception ex)
				{
					StringBuilder sb = new StringBuilder();
					sb.Append("ファイルパス：").Append(filename);
					sb.Append("セクション：").Append(sec);
					sb.Append("キー：").Append(key);
					sb.Append("値：").Append(data);

					WriteErrorLog(ex.Message, "IniWrite", sb.ToString());
				}
				return;
			}

			/// <summary>
			/// Config.ini及び、opt+ゲームデータ管理INIに値を書き込みます。opt+データ管理INIへは、存在しない場合に書き込みます。
			/// </summary>
			/// <param name="sec">セクション名</param>
			/// <param name="key">キー値</param>
			/// <param name="data">値</param>
			/// <param name="isconfig">config.iniが対象か（既定値：1）</param>
			/// <param name="opt">ゲームデータ管理INIのパス</param>
			public static void WriteIni(String sec, String key, String data, int isconfig = 1, String opt = "")
			{
				if (isconfig == 1)
				{
					WritePrivateProfileString(
									sec,
									key,
									data,
									ConfigIni);
				}
				else
				{
					if (!File.Exists(opt + "\\Data\\game.ini"))
					{
						WritePrivateProfileString(
										sec,
										key,
										data,
										opt);
					}
				}
				return;
			}

			/// <summary>
			/// Configファイル内の値を返却します
			/// </summary>
			/// <param name="sec">セクション</param>
			/// <param name="key">キー</param>
			/// <param name="failedval">読み込みに失敗した場合の値</param>
			/// <returns>キーの値もしくは失敗時の値</returns>
			public static string ReadIni(String sec, String key, String failedval)
			{
				String ans = "";

				StringBuilder data = new StringBuilder(1024);
				GetPrivateProfileString(
					sec,
					key,
					failedval,
					data,
					1024,
					ConfigIni);

				ans = data.ToString();
				return ans;
			}

			public static void Bouyomiage(String text)
			{
				if (ByActive)
				{
					TcpClient tc;

					if (Convert.ToInt32(IniRead(ConfigIni, "connect", "byType", "0")) == 1)
					{
						string url = "http://localhost:" + IniRead(ConfigIni, "connect", "byPort", "50080") + "/talk";
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
						catch (Exception)
						{
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
						catch (Exception)
						{
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
								bw.Write(ByCmd);    //コマンド（ 0:メッセージ読み上げ）
								bw.Write(BySpd);    //速度    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByTone);   //音程    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVol);    //音量    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVoice);  //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
								bw.Write(ByCode);   //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
								bw.Write(ByLength); //文字列のbyte配列の長さ
								bw.Write(BybMsg);   //文字列のbyte配列
							}
						}
						tc.Close();
					}
				}
				return;
			}

			public static void Bouyomi_Connectchk(string byHost, int byPort, int byType, bool showDialog = true)
			{
				BysMsg = "ゲームランチャーとの接続テストに成功しました。";

				if (byType == 1)
				{
					string url = "http://localhost:" + byPort + "/talk";
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
					catch (Exception)
					{
						if (showDialog)
						{
							MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
						return;
					}
					wc.Dispose();
					if (showDialog)
					{
						MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
								bw.Write(ByCmd);    //コマンド（ 0:メッセージ読み上げ）
								bw.Write(BySpd);    //速度    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByTone);   //音程    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVol);    //音量    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVoice);  //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
								bw.Write(ByCode);   //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
								bw.Write(ByLength); //文字列のbyte配列の長さ
								bw.Write(BybMsg);   //文字列のbyte配列
							}
						}
					}
					catch (Exception)
					{
						if (showDialog)
						{
							MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
						return;
					}
					finally
					{
						if (tc != null)
						{
							tc.Close();
						}
					}

					if (showDialog)
					{
						MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appname, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
			public static bool EditAllFilePath(string beforeName, string afterName, bool editFlg1, bool editFlg2, out int sucCount, out string errMsg)
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
				if (saveType != "I")
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

				//ゲーム詳細取得
				try
				{
					for (int i = 1; i <= GameMax; i++)
					{
						String readini = GameDir + "\\" + i + ".ini";
						String imgpassdata = null, passdata = null;
						String imgPathData = null, exePathData = null;
						bool wasChanged = false;

						if (File.Exists(readini))
						{
							//ini読込開始
							imgpassdata = IniRead(readini, "game", "imgpass", "");
							passdata = IniRead(readini, "game", "pass", "");

							// 実行ファイル
							if (editFlg1)
							{
								exePathData = passdata.Replace(beforeName, afterName);
								if (!passdata.Equals(exePathData))
								{
									IniWrite(readini, "game", "pass", exePathData);
									wasChanged = true;
								}
							}

							// 画像ファイル
							if (editFlg2)
							{
								imgPathData = imgpassdata.Replace(beforeName, afterName);
								if (!imgpassdata.Equals(imgPathData))
								{
									IniWrite(readini, "game", "imgpass", imgPathData);
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
							//個別ini存在しない場合
							DialogResult dr = MessageBox.Show("iniファイル読み込み中にエラー。 [button7_Click]\nファイルが存在しません。\n\n処理を続行しますか？\n\n次のファイルの値は更新されていない可能性があります：\n" + readini,
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
					MessageBox.Show("予期せぬエラーが発生しました。 [button7_Click]\nファイルの値は正常に更新されていない可能性があります。\n\n" + ex.Message,
									AppName,
									MessageBoxButtons.OK,
									MessageBoxIcon.Error);

					WriteErrorLog(ex.Message, "IniRead", string.Empty);
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
			public static void WriteErrorLog(string errorMsg, string moduleName, string addInfo)
			{
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
			public static bool downloadDbDataToLocal(string targetWorkDir)
			{
				string localGameIni = targetWorkDir + "game.ini";
				bool isSuc = false;

				try
				{
					// 退避ディレクトリがある場合、削除する
					if (Directory.Exists(basedir + "_temp_db_bak"))
					{
						Directory.Delete(basedir + "_temp_db_bak", true);
					}

					// ローカルに保存されているか確認
					if (File.Exists(localGameIni))
					{
						// ローカルファイルが存在する場合、退避する
						Directory.Move(targetWorkDir, basedir + "_temp_db_bak");
					}

					// ローカルDBフォルダがまだ残っている場合、退避する
					if (Directory.Exists(targetWorkDir))
					{
						if (Directory.Exists(basedir + "_temp_db_bak"))
						{
							// 退避ディレクトリがある場合、削除する
							Directory.Delete(basedir + "_temp_db_bak", true);
						}
						Directory.Move(targetWorkDir, basedir + "_temp_db_bak");
					}

					// 保存用フォルダ作成
					Directory.CreateDirectory(targetWorkDir);
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "Path: " + targetWorkDir + " / INI: " + localGameIni);
					return false;
				}
				// Configの最新化
				GLConfigLoad();

				// データ取得
				DataTable dt = new DataTable();

				SqlConnection cn = SqlCon;

				try
				{
					// 読み込み処理
					cn.Open();

					//全ゲーム数取得
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
						IniWrite(localGameIni, "list", "game", sqlAns.ToString());
					}
					else
					{
						//ゲームが1つもない場合
						return false;
					}

					// DBから全ゲームデータを取り出す
					SqlCommand cm2 = new SqlCommand()
					{
						CommandType = CommandType.Text,
						CommandTimeout = 60,
						CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN "
									+ " FROM " + DbName + "." + DbTable
					};
					cm2.Connection = cn;

					using (var reader = cm2.ExecuteReader())
					{
						while (reader.Read())
						{
							// 1件ずつローカルINIに書く
							if (Convert.ToInt32(reader["ID"].ToString()) > 0)
							{
								string saveLocalIniPath = targetWorkDir + reader["ID"].ToString() + ".ini";
								IniWrite(saveLocalIniPath, "game", "name", reader["GAME_NAME"].ToString());
								IniWrite(saveLocalIniPath, "game", "imgpass", reader["IMG_PATH"].ToString());
								IniWrite(saveLocalIniPath, "game", "pass", reader["GAME_PATH"].ToString());
								IniWrite(saveLocalIniPath, "game", "time", (string.IsNullOrEmpty(reader["UPTIME"].ToString()) ? "0" : reader["UPTIME"].ToString()));
								IniWrite(saveLocalIniPath, "game", "start", (string.IsNullOrEmpty(reader["RUN_COUNT"].ToString()) ? "0" : reader["RUN_COUNT"].ToString()));
								IniWrite(saveLocalIniPath, "game", "stat", reader["DCON_TEXT"].ToString());
								IniWrite(saveLocalIniPath, "game", "rating", (string.IsNullOrEmpty(reader["AGE_FLG"].ToString()) ? Rate.ToString() : reader["AGE_FLG"].ToString()));
								IniWrite(saveLocalIniPath, "game", "temp1", reader["TEMP1"].ToString());
								IniWrite(saveLocalIniPath, "game", "lastrun", reader["LAST_RUN"].ToString());
							}
						}
						isSuc = true;
					}
					// 最後に、ローカルデータを削除する
					Directory.Delete(basedir + "_temp_db_bak", true);
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, cn.ConnectionString);
					if (Directory.Exists(basedir + "_temp_db_bak"))
					{
						if (Directory.Exists(targetWorkDir))
						{
							// バックアップフォルダがある場合、作業フォルダを削除しバックアップを復元
							Directory.Delete(targetWorkDir, true);
							Directory.Move(basedir + "_temp_db_bak", targetWorkDir);
						}
					}
				}
				finally
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
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
				SqlCommand cm1; // TRUNCATE用
				SqlCommand cm2; // INSERT用
				SqlTransaction tran1 = null;

				string localGameIni = workDir + "game.ini";
				tmpMaxGameCount = 0;
				int ans = 0;
				sCount = 0;
				fCount = 0;
				bool continueAdd = true;
				string gameName = string.Empty, gamePath = string.Empty, imgPath = string.Empty, runTime = "0", runCount = "0", dconText = string.Empty, rateFlg = "0", temp1 = string.Empty, lastRun = string.Empty;

				// ini全件数取得
				if (File.Exists(localGameIni))
				{
					tmpMaxGameCount = Convert.ToInt32(IniRead(localGameIni, "list", "game", "0"));
				}

				// データベースをバックアップ
				if (!downloadDbDataToLocal(backupDir))
				{
					continueAdd = false;
					ans = 1;
				}

				// データベースのバックアップに成功した場合
				if (continueAdd)
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

					try
					{
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

							if (File.Exists(readini))
							{
								gameName = IniRead(readini, "game", "name", "").Replace("'", "''");
								imgPath = IniRead(readini, "game", "imgpass", "").Replace("'", "''");
								gamePath = IniRead(readini, "game", "pass", "").Replace("'", "''");
								runTime = IniRead(readini, "game", "time", "0");
								runCount = IniRead(readini, "game", "start", "0");
								dconText = IniRead(readini, "game", "stat", "");
								rateFlg = IniRead(readini, "game", "rating", Rate.ToString());
								temp1 = IniRead(readini, "game", "temp1", "");
								lastRun = IniRead(readini, "game", "lastrun", "");
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
								CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN ) VALUES ( '" + gameName + "', '" + gamePath + "', '" + imgPath + "', '" + runTime + "', '" + runCount + "','" + dconText + "', '" + rateFlg + "', '" + temp1 + "', '" + lastRun + "' )"
							};
							cm2.Connection = cn1;
							cm2.Transaction = tran1;

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
							IniWrite(localGameIni, "list", "dbupdate", "0");
							Directory.Delete(backupDir, true);
						}
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "cm1：" + cm1 != null ? cm1.CommandText : string.Empty + "cm2：" + cm2 != null ? cm2.CommandText : string.Empty);
						ans = 3;
						tran1.Rollback();
						if (!Ini2DbErrorRollback(backupDir))
						{
							ans = 4;
						}
					}
					finally
					{
						if (cn1.State == ConnectionState.Open)
						{
							cn1.Close();
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

				string localGameIni = backupDir + "game.ini";
				int tmpMaxGameCount = 0;
				bool ans = true;
				string gameName = string.Empty, gamePath = string.Empty, imgPath = string.Empty, runTime = "0", runCount = "0", dconText = string.Empty, rateFlg = "0", temp1 = string.Empty, lastRun = string.Empty;

				// ini全件数取得
				if (File.Exists(localGameIni))
				{
					tmpMaxGameCount = Convert.ToInt32(IniRead(localGameIni, "list", "game", "0"));
				}

				try
				{
					cn1.Open();

					// INSERTデータ取得
					for (int i = 1; i <= tmpMaxGameCount; i++)
					{
						// ini情報取得
						string readini = backupDir + "\\" + i + ".ini";

						if (File.Exists(readini))
						{
							gameName = IniRead(readini, "game", "name", "").Replace("'", "''");
							imgPath = IniRead(readini, "game", "imgpass", "").Replace("'", "''");
							gamePath = IniRead(readini, "game", "pass", "").Replace("'", "''");
							runTime = IniRead(readini, "game", "time", "0");
							runCount = IniRead(readini, "game", "start", "0");
							dconText = IniRead(readini, "game", "stat", "");
							rateFlg = IniRead(readini, "game", "rating", Rate.ToString());
							temp1 = IniRead(readini, "game", "temp1", "");
							lastRun = IniRead(readini, "game", "lastrun", "");
						}
						else
						{
							ans = false;
							WriteErrorLog("ファイルが存在しません。該当ファイルのINSERT処理をスキップします。", MethodBase.GetCurrentMethod().Name, readini);
							continue;
						}

						// INSERTコマンド作成
						cm1 = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"INSERT INTO " + DbName + "." + DbTable + " ( GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN ) VALUES ( '" + gameName + "', '" + gamePath + "', '" + imgPath + "', '" + runTime + "', '" + runCount + "','" + dconText + "', '" + rateFlg + "', '" + temp1 + "', '" + lastRun + "' )"
						};
						cm1.Connection = cn1;

						// INSERT文実行
						cm1.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "cm1：" + cm1 != null ? cm1.CommandText : string.Empty);
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

		}
	}

	public class MyBase64str
	{
		private Encoding enc = Encoding.UTF8;

		public string Encode(string str)
		{
			return Convert.ToBase64String(enc.GetBytes(str));
		}

		public string Decode(string str)
		{
			return enc.GetString(Convert.FromBase64String(str));
		}
	}

}
