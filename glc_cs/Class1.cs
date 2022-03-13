using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
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
			protected static readonly string appver = "0.97";

			/// <summary>
			/// アプリケーションビルド番号
			/// </summary>
			protected static readonly string appbuild = "22.22.03.13";

			/// <summary>
			/// ゲームディレクトリ(作業ディレクトリ)
			/// </summary>
			protected string gamedir = string.Empty;

			/// <summary>
			/// アプリケーションディレクトリ(ランチャー実行パス)
			/// </summary>
			protected string basedir = AppDomain.CurrentDomain.BaseDirectory;

			/// <summary>
			/// ゲーム情報保管iniパス
			/// </summary>
			protected string gameini;

			/// <summary>
			/// ゲーム情報保管dbパス
			/// </summary>
			protected string gamedb;

			/// <summary>
			/// アプリケーション設定パス(config.ini)
			/// </summary>
			protected string configini = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

			/// <summary>
			/// ゲーム総数
			/// </summary>
			protected int gamemax = 0;

			/// <summary>
			/// 選択中のゲームID（データベース接続時）
			/// </summary>
			protected string currentGameDBVal = "-1";

			/// <summary>
			/// 背景画像パス
			/// </summary>
			protected string bgimg = null;

			/// <summary>
			/// DiscordConnectorパス
			/// </summary>
			protected string dconpath = string.Empty;

			// データ保存方法関係
			/// <summary>
			/// データ保存方法
			/// </summary>
			protected string saveType = string.Empty;

			/// <summary>
			/// データベースのURL
			/// </summary>
			protected string dbUrl = string.Empty;

			/// <summary>
			/// データベース名
			/// </summary>
			protected string dbName = string.Empty;

			/// <summary>
			/// テーブル名
			/// </summary>
			protected string dbTable = string.Empty;

			/// <summary>
			/// データベース接続ユーザ名
			/// </summary>
			protected string dbUser = string.Empty;

			/// <summary>
			/// データベース接続パスワード
			/// </summary>
			protected string dbPass = string.Empty;

			//棒読みちゃん関係
			/// <summary>
			/// 棒読みちゃん 有効フラグ
			/// </summary>
			protected bool byActive = false;

			/// <summary>
			/// 棒読みちゃん 読み上げメッセージ
			/// </summary>
			protected string bysMsg = "Game Launcherと接続しました。";

			/// <summary>
			/// 棒読みちゃん タイプ
			/// </summary>
			protected int byType = 0;

			/// <summary>
			/// 棒読みちゃん 文字エンコーディング
			/// </summary>
			protected byte byCode = 0; //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)

			/// <summary>
			/// 棒読みちゃん 詳細設定群
			/// </summary>
			protected Int16 byVoice = 0, byVol = -1, bySpd = -1, byTone = -1, byCmd = 0x0001;

			/// <summary>
			/// 棒読みちゃん 読み上げ文字数
			/// </summary>
			protected Int32 byLength = 0;

			/// <summary>
			/// 棒読みちゃん 読み上げメッセージのbyte配列
			/// </summary>
			protected byte[] bybMsg;

			/// <summary>
			/// 棒読みちゃん 接続ホスト名
			/// </summary>
			protected string byHost = "127.0.0.1";

			/// <summary>
			/// 棒読みちゃん 接続ポート番号
			/// </summary>
			protected int byPort = 50001;

			/// <summary>
			/// 棒読みちゃん TCP接続用
			/// </summary>
			protected TcpClient tc = null;

			/// <summary>
			/// 棒読みちゃん 接続できない場合
			/// A:常に有効 D:一時的に無効 Q:問い合わせる
			/// </summary>
			protected string byCErr = "Q";

			/// <summary>
			/// 棒読みちゃん ランチャー起動時に読み上げ
			/// </summary>
			protected bool byRoW = false;

			/// <summary>
			/// 棒読みちゃん ゲーム起動時に読み上げ
			/// </summary>
			protected bool byRoS = false;

			/// <summary>
			/// Discord Connector 有効フラグ
			/// </summary>
			protected bool dconnect = false;

			/// <summary>
			/// Discord Connector レーティング設定
			/// </summary>
			protected Int32 rate = 0;

			/// <summary>
			/// グリッドサイズ最大化フラグ
			/// </summary>
			protected bool gridMax = false;

			protected DataTable dt;



			/// <summary>
			/// アプリケーション名を返却します
			/// </summary>
			public string AppName
			{
				get { return appname; }
			}

			/// <summary>
			/// アプリケーションのバージョンを返却します
			/// </summary>
			public string AppVer
			{
				get { return appver; }
			}

			/// <summary>
			/// アプリケーションのビルド番号を返却します
			/// </summary>
			public string AppBuild
			{
				get { return appbuild; }
			}

			/// <summary>
			/// ゲームの作業ディレクトリを設定/返却します
			/// </summary>
			public string GameDir
			{
				get { return gamedir; }
				set { gamedir = value; }
			}

			/// <summary>
			/// アプリケーションの作業ディレクトリを返却します
			/// </summary>
			public string BaseDir
			{
				get { return basedir; }
			}

			/// <summary>
			/// ゲーム情報が格納されているiniのディレクトリを設定/返却します
			/// </summary>
			public string GameIni
			{
				get { return gameini; }
				set { gameini = value; }
			}

			/// <summary>
			/// ゲーム情報が格納されているDBのパスを設定/返却します
			/// </summary>
			public string GameDb
			{
				get { return gamedb; }
				set { gamedb = value; }
			}

			/// <summary>
			/// アプリケーション設定が格納されているiniファイルのパスを返却します
			/// </summary>
			public string ConfigIni
			{
				get { return configini; }
			}

			/// <summary>
			/// ゲームの最大数を設定/返却します
			/// </summary>
			public int GameMax
			{
				get { return gamemax; }
				set { gamemax = value; }
			}

			/// <summary>
			/// 現在選択しているゲームのIDを設定/返却します。データベース接続時のみ使用します。
			/// </summary>
			public string CurrentGameDbVal
			{
				get { return currentGameDBVal; }
				set { currentGameDBVal = value; }
			}

			/// <summary>
			/// ゲームの最大数を設定/返却します
			/// </summary>
			public string BgImg
			{
				get { return bgimg; }
				set { bgimg = value; }
			}

			/// <summary>
			/// Discord Connectorのパスを設定/返却します
			/// </summary>
			public string DconPath
			{
				get { return dconpath; }
				set { dconpath = value; }
			}

			/// <summary>
			/// ゲーム保存方法
			/// </summary>
			public string SaveType
			{
				get { return saveType; }
				set { saveType = value; }
			}

			public string DbUrl
			{
				get { return dbUrl; }
				set { dbUrl = value; }
			}

			public string DbName
			{
				get { return dbName; }
				set { dbName = value; }
			}

			public string DbTable
			{
				get { return dbTable; }
				set { dbTable = value; }
			}

			public string DbUser
			{
				get { return dbUser; }
				set { dbUser = value; }
			}

			public string DbPass
			{
				get { return dbPass; }
				set { dbPass = value; }
			}

			public SqlConnection SqlCon
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
			public bool ByActive
			{
				get { return byActive; }
				set { byActive = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージを設定/返却します
			/// </summary>
			public string BysMsg
			{
				get { return bysMsg; }
				set { bysMsg = value; }
			}

			/// <summary>
			/// 棒読みちゃんとの接続方法（TCP/HTTP）
			/// </summary>
			public int ByType
			{
				get { return byType; }
				set { byType = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージの文字エンコーディング値を返却します
			/// </summary>
			public byte ByCode
			{
				get { return byCode; }
			}

			/// <summary>
			/// 棒読みちゃんのボイス設定を返却します
			/// </summary>
			public Int16 ByVoice
			{
				get { return byVoice; }
			}

			/// <summary>
			/// 棒読みちゃんの音量設定を返却します
			/// </summary>
			public Int16 ByVol
			{
				get { return byVol; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げ速度を返却します
			/// </summary>
			public Int16 BySpd
			{
				get { return bySpd; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げトーンを返却します
			/// </summary>
			public Int16 ByTone
			{
				get { return byTone; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げコマンドを返却します
			/// </summary>
			public Int16 ByCmd
			{
				get { return byCmd; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージの長さを返却します
			/// </summary>
			public Int32 ByLength
			{
				get { return byLength = BybMsg.Length; }
				set { byLength = value; }
			}

			/// <summary>
			/// 棒読みちゃんの読み上げメッセージを読み上げ可能なbyte配列に変換して返却します
			/// </summary>
			public byte[] BybMsg
			{
				get { return bybMsg = Encoding.UTF8.GetBytes(BysMsg); }
			}

			/// <summary>
			/// 棒読みちゃんのホスト名を設定/返却します
			/// </summary>
			public string ByHost
			{
				get { return byHost; }
				set { byHost = value; }
			}

			/// <summary>
			/// 棒読みちゃんのポート番号を設定/返却します
			/// </summary>
			public int ByPort
			{
				get { return byPort; }
				set { byPort = value; }
			}

			/// <summary>
			/// 棒読みちゃんの接続に失敗した場合にダイアログを表示するかを設定/返却します
			/// </summary>
			public string ByCErr
			{
				get { return byCErr; }
				set { byCErr = value; }
			}

			/// <summary>
			/// 棒読みちゃん接続時に、ランチャー起動時の読み上げフラグを設定/返却します
			/// </summary>
			public bool ByRoW
			{
				get { return byRoW; }
				set { byRoW = value; }
			}

			/// <summary>
			/// 棒読みちゃん接続時に、ゲーム実行／終了時の読み上げのフラグを設定/返却します
			/// </summary>
			public bool ByRoS
			{
				get { return byRoS; }
				set { byRoS = value; }
			}

			/// <summary>
			/// 棒読みちゃんとのTCP接続を構築し、その値を返却します。
			/// 受け手側でTcpClientに代入して使用します。
			/// </summary>
			public TcpClient TC
			{
				get { return tc = new TcpClient(ByHost, ByPort); }
			}

			/// <summary>
			/// Discord Connectorの有効フラグです。
			/// </summary>
			public bool Dconnect
			{
				get { return dconnect; }
				set { dconnect = Convert.ToBoolean(value); }
			}

			public Int32 Rate
			{
				get { return rate; }
				set { rate = value; }
			}
			public bool GridMax
			{
				get { return gridMax; }
				set { gridMax = value; }
			}

			public DataTable Dt
			{
				get { return dt; }
				set { dt = value; }
			}


			public bool GLConfigLoad()
			{
				if (File.Exists(ConfigIni))
				{
					// config.ini 存在する場合
					GameDir = IniRead(ConfigIni, "default", "directory", BaseDir) + "Data";
					GameIni = GameDir + "\\game.ini";
					GameDb = IniRead(ConfigIni, "default", "database", string.Empty);
					DconPath = ReadIni("connect", "dconpath", "-1");

					SaveType = ReadIni("general", "save", "I");
					DbUrl = ReadIni("connect", "DBURL", string.Empty);
					DbName = ReadIni("connect", "DBName", string.Empty);
					DbTable = ReadIni("connect", "DBTable", string.Empty);
					DbUser = ReadIni("connect", "DBUser", string.Empty);
					DbPass = ReadIni("connect", "DBPass", string.Empty);

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
					ByCErr = ReadIni("connect", "byCErr", "Q");
					ByRoW = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoW", "0")));
					ByRoS = Convert.ToBoolean(Convert.ToInt32(ReadIni("connect", "byRoS", "0")));

					// 総合
					BgImg = ReadIni("imgd", "bgimg", string.Empty);

					// dcon設定
					Dconnect = Convert.ToBoolean(Convert.ToInt32(ReadIni("checkbox", "dconnect", "0")));
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
					ByCErr = "Q";
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

			public string IniRead(String filename, String sec, String key, String failedval)
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
			public void IniWrite(String filename, String sec, String key, String data)
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
			/// Config.ini及び、ゲームデータ管理INIに値を書き込みます
			/// </summary>
			/// <param name="sec">セクション名</param>
			/// <param name="key">キー値</param>
			/// <param name="data">値</param>
			/// <param name="isconfig">config.iniが対象か（既定値：1）</param>
			/// <param name="opt">ゲームデータ管理INIのパス</param>
			public void WriteIni(String sec, String key, String data, int isconfig = 1, String opt = "")
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
			public string ReadIni(String sec, String key, String failedval)
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

			public void Bouyomiage(String text)
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
								bw.Write(ByCmd); //コマンド（ 0:メッセージ読み上げ）
								bw.Write(BySpd);   //速度    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByTone);    //音程    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVol);  //音量    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVoice);   //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
								bw.Write(ByCode);    //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
								bw.Write(ByLength);  //文字列のbyte配列の長さ
								bw.Write(BybMsg); //文字列のbyte配列
							}
						}
						tc.Close();
					}
				}
				return;
			}

			public void Bouyomi_Connectchk(string byHost, int byPort, int byType, bool showDialog = true)
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
								bw.Write(ByCmd); //コマンド（ 0:メッセージ読み上げ）
								bw.Write(BySpd);   //速度    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByTone);    //音程    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVol);  //音量    （-1:棒読みちゃん画面上の設定）
								bw.Write(ByVoice);   //声質    （ 0:棒読みちゃん画面上の設定、1:女性1、2:女性2、3:男性1、4:男性2、5:中性、6:ロボット、7:機械1、8:機械2、10001～:SAPI5）
								bw.Write(ByCode);    //文字列のbyte配列の文字コード(0:UTF-8, 1:Unicode, 2:Shift-JIS)
								bw.Write(ByLength);  //文字列のbyte配列の長さ
								bw.Write(BybMsg); //文字列のbyte配列
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
			public bool EditAllFilePath(string beforeName, string afterName, bool editFlg1, bool editFlg2, out int sucCount, out string errMsg)
			{
				errMsg = string.Empty;
				sucCount = -1;

				// Config最新化
				if (GLConfigLoad() == false)
				{
					errMsg = "Configロード中にエラー";
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
			public void WriteErrorLog(string errorMsg, string moduleName, string addInfo)
			{
				StringBuilder sb = new StringBuilder();
				sb.Append("[ERROR] [").Append(DateTime.Now).Append("] ");
				sb.Append("(").Append(moduleName).Append(") ");
				sb.Append(errorMsg).Append(" > ");
				sb.AppendLine(addInfo);
				File.AppendAllText(BaseDir + "error.log", sb.ToString());
				return;
			}
		}
	}

}
