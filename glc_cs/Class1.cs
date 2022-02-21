using System;
using System.Collections.Generic;
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
			protected static string appname = "Game Launcher C# Edition";

			/// <summary>
			/// アプリケーションバージョン
			/// </summary>
			protected static string appver = "0.96";

			/// <summary>
			/// アプリケーションビルド番号
			/// </summary>
			protected static string appbuild = "19.22.02.21";

			/// <summary>
			/// ゲームディレクトリ(作業ディレクトリ)
			/// </summary>
			protected string gamedir;

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
			/// DiscordConnectorパス
			/// </summary>
			protected string dconpath = "";

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
			protected bool dconnect;

			/// <summary>
			/// Discord Connector レーティング設定
			/// </summary>
			protected Int32 rate;




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
			/// Discord Connectorのパスを設定/返却します
			/// </summary>
			public string DconPath
			{
				get { return dconpath; }
				set { dconpath = value; }
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
			/// 棒読みちゃん接続時に、ゲーム実行／終了のフラグを設定/出力します
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
		}

		public partial class Fun : Var
		{
			public void GLConfigLoad()
			{
				// Discord設定読み込み
				Dconnect = Convert.ToBoolean(Convert.ToInt32(ReadIni("checkbox", "dconnect", "0")));

				Rate = Convert.ToInt32(ReadIni("checkbox", "rate", "-1"));

				DconPath = ReadIni("connect", "dconpath", "-1");
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

			}

			public string IniRead(String filename, String sec, String key, String failedval)
			{
				String ans = "";

				StringBuilder data = new StringBuilder(1024);
				GetPrivateProfileString(
					sec,
					key,
					failedval,
					data,
					1024,
					filename);

				ans = data.ToString();
				return ans;
			}

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
					gl.resolveError("IniWrite", ex.Message.ToString(), 0);
				}
				return;
			}

			public void WriteIni(String sec, String key, String data, int isconfig, String opt)
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
							DialogResult dr = MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。\n\n今回のみ接続しないようにしますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
							DialogResult dr = MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。\n\n今回のみ接続しないようにしますか？", General.Var.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
					}finally{
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

		}
	}

}
