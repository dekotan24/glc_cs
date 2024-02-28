using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace glc_cs.Core
{
	internal class Property
	{
		// 共通
		/// <summary>
		/// アプリケーション名
		/// </summary>
		protected static readonly string appName = "GLauncher";

		/// <summary>
		/// アプリケーションバージョン
		/// </summary>
		protected static readonly string appVer = "1.11";

		/// <summary>
		/// アプリケーションビルド番号
		/// </summary>
		protected static readonly string appBuild = "41.23.--.--";

		/// <summary>
		/// データベースバージョン
		/// </summary>
		protected static readonly string dbVer = "1.5";

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
		/// ゲーム情報管理iniパス
		/// </summary>
		protected static string gameIni;

		/// <summary>
		/// ゲーム情報管理dbパス
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
		/// 7z.dllへのパス
		/// </summary>
		protected static string sevenZipDllPath = string.Empty;

		/// <summary>
		/// DBパスワード暗号化フラグ
		/// </summary>
		protected static bool enablePWCrypt = false;


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
			, savedata_path
		}

		/// <summary>
		/// ゲームデータ管理方法の一覧
		/// </summary>
		public enum ConnType
		{
			INI
			, MSSQL
			, MySQL
			, Temp
		}

		/// <summary>
		/// ステータス
		/// </summary>
		public enum StatusType
		{
			未プレイ,
			プレイ中,
			プレイ済,
			未攻略,
			攻略中,
			攻略済
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
		/// 7z.dllへのパス
		/// </summary>
		public static string SevenZipDllPath
		{
			get { return sevenZipDllPath; }
			set { sevenZipDllPath = (File.Exists(value) ? value : string.Empty); }
		}

		/// <summary>
		/// DBパスワード暗号化フラグ
		/// </summary>
		public static bool EnablePWCrypt
		{
			get { return enablePWCrypt; }
			set { enablePWCrypt = value; }
		}

	}
}
