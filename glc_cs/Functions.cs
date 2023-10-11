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
			protected static readonly string appName = "GLauncher C# Edition";

			/// <summary>
			/// アプリケーションバージョン
			/// </summary>
			protected static readonly string appVer = "1.10";

			/// <summary>
			/// アプリケーションビルド番号
			/// </summary>
			protected static readonly string appBuild = "40.23.09.28";

			/// <summary>
			/// データベースバージョン
			/// </summary>
			protected static readonly string dbVer = "1.4";

			/// <summary>
			/// 初回ロードフラグ
			/// </summary>
			protected static bool isFirstLoad = true;

			/// <summary>
			/// ウィンドウ最小化ボタン表示フラグ
			/// </summary>
			protected static bool windowHideControlFlg = false;

			/// <summary>
			/// ゲームディレクトリ(作業ディレクトリ)
			/// </summary>
			protected static string gameDir = string.Empty;

			/// <summary>
			/// アプリケーションディレクトリ（ランチャー実行パス）
			/// </summary>
			protected static string baseDir = AppDomain.CurrentDomain.BaseDirectory;

			/// <summary>
			/// ゲーム情報保管iniパス
			/// </summary>
			protected static string gameIni;

			/// <summary>
			/// ゲーム情報保管dbパス
			/// </summary>
			protected static string gamedb;

			/// <summary>
			/// アプリケーション設定パス(config.ini)
			/// </summary>
			protected static string configIni = AppDomain.CurrentDomain.BaseDirectory + "config.ini";

			/// <summary>
			/// オフラインINI用ローカルディレクトリ
			/// </summary>
			protected static string localPath = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\";

			/// <summary>
			/// オフラインINI用ローカル統括管理ファイル
			/// </summary>
			protected static string localIni = BaseDir + (BaseDir.EndsWith("\\") ? "" : "\\") + "Local\\game.ini";

			/// <summary>
			/// ゲーム総数
			/// </summary>
			protected static int gameMax = 0;

			/// <summary>
			/// 選択中のゲームID（データベース接続時）
			/// </summary>
			protected static string currentGameDBVal = "-1";

			/// <summary>
			/// 背景画像パス
			/// </summary>
			protected static string bgimg = string.Empty;

			/// <summary>
			/// グリッドロード有無
			/// </summary>
			protected static bool gridEnable = true;

			/// <summary>
			/// 起動時のINI／DBアップデートチェックフラグ
			/// </summary>
			protected static bool initialUpdateCheckSkipFlg = false;

			/// <summary>
			/// 起動時のINI／DBアップデートチェック対象バージョン
			/// </summary>
			protected static string initialUpdateCheckSkipVer = string.Empty;

			/// <summary>
			/// DiscordConnectorパス
			/// </summary>
			protected static string dconPath = string.Empty;

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
			/// ローカルDB構築フラグ
			/// </summary>
			protected static bool useLocalDB = false;

			/// <summary>
			/// ローカルDB
			/// </summary>
			protected static DataTable localDB = null;

			/// <summary>
			/// データベースのURL
			/// </summary>
			protected static string dbUrl = string.Empty;

			/// <summary>
			/// データベースのポート番号
			/// </summary>
			protected static string dbPort = string.Empty;

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
			protected static string bysMsg = "GLauncherと接続しました。";

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
			/// 棒読みちゃん ランチャー起動/終了時に読み上げ
			/// </summary>
			protected static bool byRoW = false;

			/// <summary>
			/// 棒読みちゃん ゲーム起動/終了時に読み上げ
			/// </summary>
			protected static bool byRoS = false;

			/// <summary>
			/// 棒読みちゃん オフラインデータ取得時に読み上げ
			/// </summary>
			protected static bool byRoG = false;

			/// <summary>
			/// Discord Connector 有効フラグ
			/// </summary>
			protected static bool dconnectEnabled = false;

			/// <summary>
			/// Discord Connector レーティング設定
			/// </summary>
			protected static Int32 rate = 0;

			/// <summary>
			/// グリッドサイズ最大化フラグ
			/// </summary>
			protected static bool gridMax = false;

			/// <summary>
			/// 未プレイ時のデフォルトステータス
			/// </summary>
			protected static string defaultStatusValueOfNotPlaying = "未プレイ";

			/// <summary>
			/// プレイ中のデフォルトステータス
			/// </summary>
			protected static string defaultStatusValueOfPlaying = "プレイ中";

			/// <summary>
			/// グリッドのイメージサイズ固定フラグ
			/// </summary>
			protected static bool fixGridSizeFlg = false;

			/// <summary>
			/// グリッドのイメージサイズ
			/// </summary>
			protected static int fixGridSize = 32;

			/// <summary>
			/// 初期起動時ゲームロードカウンタ表示フラグ
			/// </summary>
			protected static bool disableInitialLoadCountFlg = true;

			/// <summary>
			/// 抽出機能使用可否フラグ
			/// </summary>
			protected static bool extractEnable = false;

			/// <summary>
			/// 抽出ツールのID
			/// </summary>
			protected static int currentExtractTool = 0;

			/// <summary>
			/// krkrツールパス
			/// </summary>
			protected static string extractKrkrPath = string.Empty;

			/// <summary>
			/// krkr引数
			/// </summary>
			protected static string extractKrkrArg = string.Empty;

			/// <summary>
			/// krkrゲーム引数追加フラグ
			/// </summary>
			protected static bool extractKrkrAddGameArg = false;

			/// <summary>
			/// krkrツールをカレントディレクトリにするフラグ
			/// </summary>
			protected static bool extractKrkrCurDir = false;

			/// <summary>
			/// krkrにゲームのディレクトリパスを渡すフラグ
			/// </summary>
			protected static bool extractKrkrGameDir = false;

			/// <summary>
			/// krkrzツールパス
			/// </summary>
			protected static string extractKrkrzPath = string.Empty;

			/// <summary>
			/// krkrz引数
			/// </summary>
			protected static string extractKrkrzArg = string.Empty;

			/// <summary>
			/// krkrzゲーム引数追加フラグ
			/// </summary>
			protected static bool extractKrkrzAddGameArg = false;

			/// <summary>
			/// krkrzツールをカレントディレクトリにするフラグ
			/// </summary>
			protected static bool extractKrkrzCurDir = false;

			/// <summary>
			/// krkrzにゲームのディレクトリパスを渡すフラグ
			/// </summary>
			protected static bool extractKrkrzGameDir = false;

			/// <summary>
			/// krkrDumpツールパス
			/// </summary>
			protected static string extractKrkrDumpPath = string.Empty;

			/// <summary>
			/// krkrDump引数
			/// </summary>
			protected static string extractKrkrDumpArg = string.Empty;

			/// <summary>
			/// krkrDumpゲーム引数追加フラグ
			/// </summary>
			protected static bool extractKrkrDumpAddGameArg = false;

			/// <summary>
			/// krkrDumpツールをカレントディレクトリにするフラグ
			/// </summary>
			protected static bool extractKrkrDumpCurDir = false;

			/// <summary>
			/// krkrDumpにゲームのディレクトリパスを渡すフラグ
			/// </summary>
			protected static bool extractKrkrDumpGameDir = false;

			/// <summary>
			/// カスタム1ツールパス
			/// </summary>
			protected static string extractCustom1Path = string.Empty;

			/// <summary>
			/// カスタム1引数
			/// </summary>
			protected static string extractCustom1Arg = string.Empty;

			/// <summary>
			/// カスタム1ゲーム引数追加フラグ
			/// </summary>
			protected static bool extractCustom1AddGameArg = false;

			/// <summary>
			/// カスタム1ツールをカレントディレクトリにするフラグ
			/// </summary>
			protected static bool extractCustom1CurDir = false;

			/// <summary>
			/// カスタム1にゲームのディレクトリパスを渡すフラグ
			/// </summary>
			protected static bool extractCustom1GameDir = false;

			/// <summary>
			/// カスタム2ツールパス
			/// </summary>
			protected static string extractCustom2Path = string.Empty;

			/// <summary>
			/// カスタム2引数
			/// </summary>
			protected static string extractCustom2Arg = string.Empty;

			/// <summary>
			/// カスタム2ゲーム引数追加フラグ
			/// </summary>
			protected static bool extractCustom2AddGameArg = false;

			/// <summary>
			/// カスタム2ツールをカレントディレクトリにするフラグ
			/// </summary>
			protected static bool extractCustom2CurDir = false;

			/// <summary>
			/// カスタム2にゲームのディレクトリパスを渡すフラグ
			/// </summary>
			protected static bool extractCustom2GameDir = false;


			/// <summary>
			/// INIファイルに書き込む際に使用可能なキー名一覧
			/// </summary>
			public enum KeyNames
			{
				id
				, name
				, imgpass
				, pass
				, time
				, start
				, stat
				, dcon_img
				, memo
				, status
				, ini_version
				, rating
				, lastrun
				, temp1
				, execute_cmd
				, extract_tool
			}


			/// <summary>
			/// ゲームデータ管理方法の一覧
			/// </summary>
			public enum ConnType
			{
				INI
				, MSSQL
				, MySQL
			}

			/// <summary>
			/// アプリケーション名を返却します
			/// </summary>
			public static string AppName
			{
				get { return appName; }
			}

			/// <summary>
			/// アプリケーションのバージョンを返却します
			/// </summary>
			public static string AppVer
			{
				get { return appVer; }
			}

			/// <summary>
			/// アプリケーションのビルド番号を返却します
			/// </summary>
			public static string AppBuild
			{
				get { return appBuild; }
			}

			/// <summary>
			/// データベースのバージョンを返却します
			/// </summary>
			public static string DBVer
			{
				get { return dbVer; }
			}

			/// <summary>
			/// 初回ロードフラグ
			/// </summary>
			public static bool IsFirstLoad
			{
				get { return isFirstLoad; }
				set { isFirstLoad = value; }
			}

			/// <summary>
			/// ウィンドウに最小化コントロールを表示するかのフラグです
			/// </summary>
			public static bool WindowHideControlFlg
			{
				get { return windowHideControlFlg; }
				set { windowHideControlFlg = value; }
			}

			/// <summary>
			/// 初期起動時にスクラッシュウィンドウにロード中ゲームのカウンタを表示するかのフラグです
			/// </summary>
			public static bool DisableInitialLoadCountFlg
			{
				get { return disableInitialLoadCountFlg; }
				set { disableInitialLoadCountFlg = value; }
			}

			/// <summary>
			/// ゲームの作業ディレクトリを設定/返却します
			/// </summary>
			public static string GameDir
			{
				get { return gameDir; }
				set { gameDir = (value.EndsWith("\\") ? value : value + "\\"); }
			}

			/// <summary>
			/// アプリケーションの作業ディレクトリを返却します
			/// </summary>
			public static string BaseDir
			{
				get { return baseDir; }
			}

			/// <summary>
			/// ゲーム統括管理iniのパスを設定/返却します
			/// </summary>
			public static string GameIni
			{
				get { return gameIni; }
				set { gameIni = value; }
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
				get { return configIni; }
			}

			/// <summary>
			/// オフラインINIが格納されているディレクトリパスを返却します
			/// </summary>
			public static string LocalPath
			{
				get { return localPath; }
			}

			/// <summary>
			/// オフラインINIの統括管理INIファイルパスを返却します
			/// </summary>
			public static string LocalIni
			{
				get { return localIni; }
			}

			/// <summary>
			/// ゲームの最大数を設定/返却します
			/// </summary>
			public static int GameMax
			{
				get { return gameMax; }
				set { gameMax = value; }
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
			/// グリッド使用フラグ
			/// </summary>
			public static bool GridEnable
			{
				get { return gridEnable; }
				set { gridEnable = value; }
			}

			/// <summary>
			/// 起動時アップデートチェックスキップフラグ
			/// </summary>
			public static bool InitialUpdateCheckSkipFlg
			{
				get { return initialUpdateCheckSkipFlg; }
				set { initialUpdateCheckSkipFlg = value; }
			}

			/// <summary>
			/// 起動時アップデートチェックスキップ対象バージョン
			/// </summary>
			public static string InitialUpdateCheckSkipVer
			{
				get { return initialUpdateCheckSkipVer; }
				set { initialUpdateCheckSkipVer = value; }
			}

			/// <summary>
			/// Discord Connectorのパスを設定/返却します
			/// </summary>
			public static string DconPath
			{
				get { return dconPath; }
				set { dconPath = value; }
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
			/// ゲーム保存方法(I, D, M, T)
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

			/// <summary>
			/// ローカルDB構築フラグ
			/// </summary>
			public static bool UseLocalDB
			{
				get { return useLocalDB; }
				set { useLocalDB = value; }
			}

			/// <summary>
			/// ローカルDB
			/// </summary>
			public static DataTable LocalDB
			{
				get { return localDB; }
				set { localDB = value; }
			}

			/// <summary>
			/// データベースのURL
			/// </summary>
			public static string DbUrl
			{
				get { return dbUrl; }
				set { dbUrl = value; }
			}

			/// <summary>
			/// データベースのポート番号
			/// </summary>
			public static string DbPort
			{
				get { return dbPort; }
				set { dbPort = value; }
			}

			/// <summary>
			/// データベース名
			/// </summary>
			public static string DbName
			{
				get { return dbName; }
				set { dbName = value; }
			}

			/// <summary>
			/// データベースのテーブル名
			/// </summary>
			public static string DbTable
			{
				get { return dbTable; }
				set { dbTable = value; }
			}

			/// <summary>
			/// データベースのログインユーザ名。値の設定時以外は使用しません
			/// </summary>
			public static string DbUser
			{
				get { return dbUser; }
				set { dbUser = value; }
			}

			/// <summary>
			/// データベースのログインパスワード。値の設定時以外は使用しません
			/// </summary>
			public static string DbPass
			{
				get { return dbPass; }
				set { dbPass = value; }
			}

			/// <summary>
			/// MSSQLの<see cref="SqlConnection"/>
			/// </summary>
			public static SqlConnection SqlCon
			{
				get
				{
					return new SqlConnection(
						new SqlConnectionStringBuilder()
						{
							IntegratedSecurity = false,
							//InitialCatalog = dbName,
							DataSource = DbUrl + "," + DbPort,
							UserID = DbUser,
							Password = DbPass
						}.ToString()
					);
				}
			}

			/// <summary>
			/// MySQLの<see cref="MySqlConnection"/>
			/// </summary>
			public static MySqlConnection SqlCon2
			{
				get
				{
					string server = DbUrl;
					string port = DbPort;
					string database = DbName;
					string user = DbUser;
					string pass = DbPass;
					string charset = "utf8mb4";
					string connectionString = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4};Charset={5}", server, port, database, user, pass, charset);

					MySqlConnection cn = new MySqlConnection(connectionString);
					return cn;
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
			/// 棒読みちゃん接続時に、オフラインデータ取得時の読み上げのフラグを設定/返却します
			/// </summary>
			public static bool ByRoG
			{
				get { return byRoG; }
				set { byRoG = value; }
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
				get { return dconnectEnabled; }
				set { dconnectEnabled = Convert.ToBoolean(value); }
			}

			/// <summary>
			/// デフォルトの成人向けフラグ
			/// </summary>
			public static Int32 Rate
			{
				get { return rate; }
				set { rate = value; }
			}

			/// <summary>
			/// グリッドの全画面表示フラグ
			/// </summary>
			public static bool GridMax
			{
				get { return gridMax; }
				set { gridMax = value; }
			}

			/// <summary>
			/// 未プレイ時のデフォルトのステータス
			/// </summary>
			public static string DefaultStatusValueOfNotPlaying
			{
				get { return defaultStatusValueOfNotPlaying; }
				set { defaultStatusValueOfNotPlaying = value; }
			}

			/// <summary>
			/// プレイ中のデフォルトのステータス
			/// </summary>
			public static string DefaultStatusValueOfPlaying
			{
				get { return defaultStatusValueOfPlaying; }
				set { defaultStatusValueOfPlaying = value; }
			}

			/// <summary>
			/// グリッドのイメージサイズ固定フラグ
			/// </summary>
			public static bool FixGridSizeFlg
			{
				get { return fixGridSizeFlg; }
				set { fixGridSizeFlg = value; }
			}

			/// <summary>
			/// グリッドのイメージサイズ
			/// </summary>
			public static int FixGridSize
			{
				get { return fixGridSize; }
				set { fixGridSize = (value == 8 || value == 32 || value == 64) ? value : 32; }
			}

			/// <summary>
			/// 抽出ツールID
			/// </summary>
			public static int CurrentExtractTool
			{
				get { return currentExtractTool; }
				set { currentExtractTool = value; }
			}

			/// <summary>
			/// 抽出機能使用可否フラグ
			/// </summary>
			public static bool ExtractEnable
			{
				get { return extractEnable; }
				set { extractEnable = value; }
			}

			/// <summary>
			/// krkrツールパス
			/// </summary>
			public static string ExtractKrkrPath
			{
				get { return extractKrkrPath; }
				set { extractKrkrPath = value; }
			}

			/// <summary>
			/// krkr引数
			/// </summary>
			public static string ExtractKrkrArg
			{
				get { return extractKrkrArg; }
				set { extractKrkrArg = value; }
			}

			/// <summary>
			/// krkrゲーム引数追加フラグ
			/// </summary>
			public static bool ExtractKrkrAddGameArg
			{
				get { return extractKrkrAddGameArg; }
				set { extractKrkrAddGameArg = value; }
			}

			/// <summary>
			/// krkrツールをカレントディレクトリにするフラグ
			/// </summary>
			public static bool ExtractKrkrCurDir
			{
				get { return extractKrkrCurDir; }
				set { extractKrkrCurDir = value; }
			}

			/// <summary>
			/// krkrにゲームディレクトリを渡すフラグ
			/// </summary>
			public static bool ExtractKrkrGameDir
			{
				get { return extractKrkrGameDir; }
				set { extractKrkrGameDir = value; }
			}

			/// <summary>
			/// krkrzツールパス
			/// </summary>
			public static string ExtractKrkrzPath
			{
				get { return extractKrkrzPath; }
				set { extractKrkrzPath = value; }
			}

			/// <summary>
			/// krkrz引数
			/// </summary>
			public static string ExtractKrkrzArg
			{
				get { return extractKrkrzArg; }
				set { extractKrkrzArg = value; }
			}

			/// <summary>
			/// krkrzゲーム引数追加フラグ
			/// </summary>
			public static bool ExtractKrkrzAddGameArg
			{
				get { return extractKrkrzAddGameArg; }
				set { extractKrkrzAddGameArg = value; }
			}

			/// <summary>
			/// krkrzツールをカレントディレクトリにするフラグ
			/// </summary>
			public static bool ExtractKrkrzCurDir
			{
				get { return extractKrkrzCurDir; }
				set { extractKrkrzCurDir = value; }
			}

			/// <summary>
			/// krkrzにゲームディレクトリを渡すフラグ
			/// </summary>
			public static bool ExtractKrkrzGameDir
			{
				get { return extractKrkrzGameDir; }
				set { extractKrkrzGameDir = value; }
			}

			/// <summary>
			/// krkrDumpツールパス
			/// </summary>
			public static string ExtractKrkrDumpPath
			{
				get { return extractKrkrDumpPath; }
				set { extractKrkrDumpPath = value; }
			}

			/// <summary>
			/// krkrDump引数
			/// </summary>
			public static string ExtractKrkrDumpArg
			{
				get { return extractKrkrDumpArg; }
				set { extractKrkrDumpArg = value; }
			}

			/// <summary>
			/// krkrDumpゲーム引数追加フラグ
			/// </summary>
			public static bool ExtractKrkrDumpAddGameArg
			{
				get { return extractKrkrDumpAddGameArg; }
				set { extractKrkrDumpAddGameArg = value; }
			}

			/// <summary>
			/// krkrDumpツールをカレントディレクトリにするフラグ
			/// </summary>
			public static bool ExtractKrkrDumpCurDir
			{
				get { return extractKrkrDumpCurDir; }
				set { extractKrkrDumpCurDir = value; }
			}

			/// <summary>
			/// krkrDumpにゲームディレクトリを渡すフラグ
			/// </summary>
			public static bool ExtractKrkrDumpGameDir
			{
				get { return extractKrkrDumpGameDir; }
				set { extractKrkrDumpGameDir = value; }
			}

			/// <summary>
			/// Custom1ツールパス
			/// </summary>
			public static string ExtractCustom1Path
			{
				get { return extractCustom1Path; }
				set { extractCustom1Path = value; }
			}

			/// <summary>
			/// Custom1引数
			/// </summary>
			public static string ExtractCustom1Arg
			{
				get { return extractCustom1Arg; }
				set { extractCustom1Arg = value; }
			}

			/// <summary>
			/// Custom1ゲーム引数追加フラグ
			/// </summary>
			public static bool ExtractCustom1AddGameArg
			{
				get { return extractCustom1AddGameArg; }
				set { extractCustom1AddGameArg = value; }
			}

			/// <summary>
			/// Custom1ツールをカレントディレクトリにするフラグ
			/// </summary>
			public static bool ExtractCustom1CurDir
			{
				get { return extractCustom1CurDir; }
				set { extractCustom1CurDir = value; }
			}

			/// <summary>
			/// Custom1にゲームディレクトリを渡すフラグ
			/// </summary>
			public static bool ExtractCustom1GameDir
			{
				get { return extractCustom1GameDir; }
				set { extractCustom1GameDir = value; }
			}

			/// <summary>
			/// Custom2ツールパス
			/// </summary>
			public static string ExtractCustom2Path
			{
				get { return extractCustom2Path; }
				set { extractCustom2Path = value; }
			}

			/// <summary>
			/// Custom2引数
			/// </summary>
			public static string ExtractCustom2Arg
			{
				get { return extractCustom2Arg; }
				set { extractCustom2Arg = value; }
			}

			/// <summary>
			/// Custom2ゲーム引数追加フラグ
			/// </summary>
			public static bool ExtractCustom2AddGameArg
			{
				get { return extractCustom2AddGameArg; }
				set { extractCustom2AddGameArg = value; }
			}

			/// <summary>
			/// Custom2ツールをカレントディレクトリにするフラグ
			/// </summary>
			public static bool ExtractCustom2CurDir
			{
				get { return extractCustom2CurDir; }
				set { extractCustom2CurDir = value; }
			}

			/// <summary>
			/// Custom1にゲームディレクトリを渡すフラグ
			/// </summary>
			public static bool ExtractCustom2GameDir
			{
				get { return extractCustom2GameDir; }
				set { extractCustom2GameDir = value; }
			}

			/// <summary>
			/// システム変数をロードします
			/// </summary>
			/// <returns></returns>
			public static bool GLConfigLoad()
			{
				MyBase64str base64 = new MyBase64str();

				string currentSaveType = SaveType;

				if (File.Exists(ConfigIni))
				{
					// config.ini 存在する場合
					GameDir = ReadIni("default", "directory", BaseDir) + "Data";
					GameIni = GameDir + "game.ini";
					GameDb = ReadIni("default", "database", string.Empty);
					DconPath = ReadIni("connect", "dconPath", "-1");
					disableInitialLoadCountFlg = Convert.ToBoolean(Convert.ToInt32(ReadIni("disable", "DisableInitialLoadCount", "1")));

					SaveType = ReadIni("general", "save", "I");
					OfflineSave = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "OfflineSave", "0")));
					UseLocalDB = Convert.ToBoolean(Convert.ToInt32(ReadIni("general", "UseLocalDB", "0")));
					DbUrl = ReadIni("connect", "DBURL", string.Empty);
					DbPort = ReadIni("connect", "DBPort", string.Empty);
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
						GameMax = Convert.ToInt32(ReadIni("list", "game", "0", 0));
					}
					else if (saveType == "D")
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

			public static string IniRead(string fileName, string sectionName, KeyNames keyArray, string failedVal)
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
					WritePrivateProfileString(
									sec,
									key,
									data,
									ConfigIni);
				}
				else if (opt.Length == 0)
				{
					if (File.Exists(GameIni))
					{
						WritePrivateProfileString(
										sec,
										key,
										data,
										GameIni);
					}
				}
				else
				{
					if (File.Exists((opt.EndsWith("\\") ? opt : opt + "\\") + "game.ini"))
					{
						WritePrivateProfileString(
										sec,
										key,
										data,
										(opt.EndsWith("\\") ? opt : opt + "\\") + "game.ini");
					}

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
						(opt.EndsWith("\\") ? opt : opt + "\\") + "game.ini");

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
							MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
						return;
					}
					wc.Dispose();
					if (showDialog)
					{
						MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
							MessageBox.Show("エラー：棒読みちゃんとの接続に失敗しました。\n接続できません。", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
						MessageBox.Show("棒読みちゃんとの接続テストに成功しました。", appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
						string readini = GameDir + i + ".ini";
						string imgpassdata = string.Empty, passdata = string.Empty;
						string imgPathData = string.Empty, exePathData = string.Empty;
						bool wasChanged = false;

						if (File.Exists(readini))
						{
							//ini読込開始
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
							//個別ini存在しない場合
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
			public static void WriteErrorLog(string errorMsg, string moduleName, string addInfo)
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
					if (Directory.Exists(baseDir + "_temp_db_bak"))
					{
						Directory.Delete(baseDir + "_temp_db_bak", true);
					}

					// ターゲットパスが存在しているか確認
					if (File.Exists(targetWorkDir))
					{
						// ローカルファイルが存在する場合、退避する
						Directory.Move(targetWorkDir, baseDir + "_temp_db_bak");
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
						Directory.Move(baseDir + "_temp_db_bak", targetWorkDir);
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
							CommandText = @"SELECT count(*) FROM " + dbName + "." + dbTable
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
							WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + saveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);
							return true;
						}

						// DBから全ゲームデータを取り出す
						SqlCommand cm2 = new SqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 60,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
										+ " FROM " + dbName + "." + dbTable
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
							IniWrite(targetWorkDir + "game.ini", "game", "list", count.ToString());
							isSuc = true;
						}
					}
					else
					{
						mcn.Open();

						// 全ゲーム数取得
						MySqlCommand cm = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 30,
							CommandText = @"SELECT count(*) FROM " + dbTable
						};
						cm.Connection = mcn;

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
							WriteErrorLog("データベース内にレコードが存在しません。", MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + saveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);
							return true;
						}

						// DBから全ゲームデータを取り出す
						MySqlCommand cm2 = new MySqlCommand()
						{
							CommandType = CommandType.Text,
							CommandTimeout = 60,
							CommandText = @"SELECT ID, GAME_NAME, GAME_PATH, EXECUTE_CMD, IMG_PATH, UPTIME, RUN_COUNT, DCON_TEXT, AGE_FLG, TEMP1, LAST_RUN, DCON_IMG, MEMO, STATUS, EXTRACT_TOOL, DB_VERSION "
										+ " FROM " + dbTable
						};
						cm2.Connection = mcn;

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
							IniWrite(targetWorkDir + "game.ini", "game", "list", count.ToString());
							isSuc = true;
						}

					}

					// 退避ディレクトリが存在する場合、削除する
					if (Directory.Exists(baseDir + "_temp_db_bak"))
					{
						Directory.Delete(baseDir + "_temp_db_bak", true);
					}
				}
				catch (Exception ex)
				{
					WriteErrorLog(ex.Message, MethodBase.GetCurrentMethod().Name, "[ダウンロード処理] SaveType:" + saveType + " / MSSQL:" + cn.ConnectionString + " / MySQL:" + mcn.ConnectionString);

					// 退避ディレクトリが存在する場合、ロールバック
					if (Directory.Exists(baseDir + "_temp_db_bak"))
					{
						if (Directory.Exists(targetWorkDir))
						{
							// バックアップフォルダがある場合、作業フォルダを削除
							Directory.Delete(targetWorkDir, true);
						}
						// 退避ディレクトリ復元
						Directory.Move(baseDir + "_temp_db_bak", targetWorkDir);
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
					else
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
				string localGameIni = (workDir.EndsWith("\\") ? workDir : workDir + "\\") + "game.ini";

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
						if (saveType == "D")
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
									// タイトルのエスケープ
									returnVal[0] = EncodeSQLSpecialChars(returnVal[0]);
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

							// TRANSACTION開始
							mtran1 = mcn1.BeginTransaction();

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
									// タイトルのエスケープ
									returnVal[0] = EncodeSQLSpecialChars(returnVal[0]);
									sCount++;
								}
								else
								{
									fCount++;
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
								mcm2.ExecuteNonQuery();
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
						if (saveType == "D")
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
						if (saveType == "D")
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

				string localGameIni = (backupDir.EndsWith("\\") ? backupDir : backupDir + "\\") + "game.ini";
				int tmpMaxGameCount = 0;
				bool ans = true;

				// ini全件数取得
				if (File.Exists(localGameIni))
				{
					tmpMaxGameCount = Convert.ToInt32(ReadIni("list", "game", "0", 0, backupDir));
				}

				try
				{
					if (saveType == "D")
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
