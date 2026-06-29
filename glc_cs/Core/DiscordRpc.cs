using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using static glc_cs.Core.Property;

namespace glc_cs.Core
{
	internal class DiscordRpc : IDisposable
	{
		private const string DefaultAppId = "617680312549376003";

#if DEBUG
		private const bool IsDebug = true;
#else
		private const bool IsDebug = false;
#endif

		private static string AppLabel => "GLauncher v" + AppVer + (IsDebug ? " <dev>" : "");

		private NamedPipeClientStream _pipe;
		private Thread _callbackThread;
		private volatile bool _running;
		private readonly object _writeLock = new object();

		public bool IsConnected => _pipe != null && _pipe.IsConnected;

		public void SetPresence(string title, string state, string appId, string appIcon, bool isSensitive)
		{
			if (string.IsNullOrEmpty(title)) return;

			string effectiveAppId = (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appIcon))
				? appId : DefaultAppId;

			string largeImageKey;
			string largeImageText;
			string smallImageKey = null;

			if (!string.IsNullOrEmpty(appId) && !string.IsNullOrEmpty(appIcon))
			{
				largeImageKey = appIcon;
				largeImageText = title;
				smallImageKey = "fav";
			}
			else if (isSensitive)
			{
				largeImageKey = "hidden";
				largeImageText = title;
				smallImageKey = "fav";
			}
			else
			{
				var match = ResolveIcon(title);
				if (match != null)
				{
					largeImageKey = match.IconKey;
					largeImageText = title;
					smallImageKey = "fav";
				}
				else
				{
					largeImageKey = "fav";
					largeImageText = title;
					smallImageKey = null;
				}
			}

			Connect(effectiveAppId);
			if (!IsConnected) return;

			long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

			var activity = new StringBuilder();
			activity.Append("{\"cmd\":\"SET_ACTIVITY\",\"args\":{\"pid\":");
			activity.Append(Process.GetCurrentProcess().Id);
			activity.Append(",\"activity\":{");
			activity.Append("\"details\":").Append(JsonEscape(title));
			if (!string.IsNullOrEmpty(state))
				activity.Append(",\"state\":").Append(JsonEscape(state));
			activity.Append(",\"timestamps\":{\"start\":").Append(timestamp).Append("}");
			activity.Append(",\"assets\":{");
			activity.Append("\"large_image\":").Append(JsonEscape(largeImageKey));
			activity.Append(",\"large_text\":").Append(JsonEscape(largeImageText));
			if (smallImageKey != null)
			{
				activity.Append(",\"small_image\":").Append(JsonEscape(smallImageKey));
				activity.Append(",\"small_text\":").Append(JsonEscape(AppLabel));
			}
			activity.Append("}");
			activity.Append("}},\"nonce\":\"").Append(Guid.NewGuid().ToString()).Append("\"}");

			SendFrame(1, activity.ToString());

			_running = true;
			_callbackThread = new Thread(CallbackLoop) { Name = "DiscordRPC-Callback", IsBackground = true };
			_callbackThread.Start();
		}

		private void Connect(string appId)
		{
			for (int i = 0; i < 10; i++)
			{
				try
				{
					var pipe = new NamedPipeClientStream(".", "discord-ipc-" + i, PipeDirection.InOut, PipeOptions.Asynchronous);
					pipe.Connect(1000);
					_pipe = pipe;

					string handshake = "{\"v\":1,\"client_id\":\"" + appId + "\"}";
					SendFrame(0, handshake);

					ReadFrame();
					return;
				}
				catch
				{
					_pipe?.Dispose();
					_pipe = null;
				}
			}
		}

		private void SendFrame(int opcode, string payload)
		{
			if (_pipe == null || !_pipe.IsConnected) return;
			byte[] data = Encoding.UTF8.GetBytes(payload);
			byte[] header = new byte[8];
			BitConverter.GetBytes(opcode).CopyTo(header, 0);
			BitConverter.GetBytes(data.Length).CopyTo(header, 4);
			lock (_writeLock)
			{
				_pipe.Write(header, 0, 8);
				_pipe.Write(data, 0, data.Length);
				_pipe.Flush();
			}
		}

		private string ReadFrame()
		{
			if (_pipe == null || !_pipe.IsConnected) return null;
			byte[] header = new byte[8];
			int read = 0;
			while (read < 8)
			{
				int n = _pipe.Read(header, read, 8 - read);
				if (n == 0) return null;
				read += n;
			}
			int length = BitConverter.ToInt32(header, 4);
			if (length <= 0 || length > 65536) return null;
			byte[] payload = new byte[length];
			read = 0;
			while (read < length)
			{
				int n = _pipe.Read(payload, read, length - read);
				if (n == 0) return null;
				read += n;
			}
			return Encoding.UTF8.GetString(payload);
		}

		private void CallbackLoop()
		{
			while (_running && IsConnected)
			{
				try
				{
					Thread.Sleep(2000);
					ReadFrame();
				}
				catch { break; }
			}
		}

		private static string JsonEscape(string s)
		{
			if (s == null) return "null";
			var sb = new StringBuilder(s.Length + 2);
			sb.Append('"');
			foreach (char c in s)
			{
				switch (c)
				{
					case '"': sb.Append("\\\""); break;
					case '\\': sb.Append("\\\\"); break;
					case '\n': sb.Append("\\n"); break;
					case '\r': sb.Append("\\r"); break;
					case '\t': sb.Append("\\t"); break;
					default: sb.Append(c); break;
				}
			}
			sb.Append('"');
			return sb.ToString();
		}

		public void Dispose()
		{
			_running = false;
			try
			{
				if (_pipe != null && _pipe.IsConnected)
				{
					SendFrame(2, "{}");
				}
			}
			catch { }
			try { _pipe?.Dispose(); } catch { }
			_pipe = null;
			try { _callbackThread?.Join(3000); } catch { }
			_callbackThread = null;
		}

		#region Icon Mapping Table

		private class IconMatch
		{
			public string IconKey;
			public string Brand;
		}

		private IconMatch ResolveIcon(string title)
		{
			if (string.IsNullOrEmpty(title)) return null;

			// hidden/Unknown → hidden icon
			if (title.Contains("hidden") || title.Contains("Unknown"))
				return new IconMatch { IconKey = "hidden", Brand = null };

			foreach (var entry in _iconTable)
			{
				if (entry.Title2 != null)
				{
					if (title.Contains(entry.Title) && title.Contains(entry.Title2))
						return new IconMatch { IconKey = entry.IconKey, Brand = entry.Brand };
				}
				else
				{
					if (title.Contains(entry.Title))
						return new IconMatch { IconKey = entry.IconKey, Brand = entry.Brand };
				}
			}

			return null;
		}

		private class TableEntry
		{
			public string Title;
			public string Title2;
			public string IconKey;
			public string Brand;

			public TableEntry(string title, string iconKey, string brand, string title2 = null)
			{
				Title = title;
				Title2 = title2;
				IconKey = iconKey;
				Brand = brand;
			}
		}

		private static readonly TableEntry[] _iconTable = new TableEntry[]
		{
			// CLEARRAVE / PALETTE-QUALIA
			new TableEntry("オトメ＊ドメイン", "tomedome", "CLEARRAVE / PALETTE-QUALIA"),
			// VISUAL ARTS / Key
			new TableEntry("Summer Pockets", "sp", "VISUAL ARTS / Key"),
			// HARUKAZE
			new TableEntry("ノラと皇女と野良猫ハート２", "noratoto2", "HARUKAZE"),
			new TableEntry("ノラと皇女と野良猫ハート", "noratoto", "HARUKAZE"),
			// Lump of Sugar
			new TableEntry("若葉色のカルテット", "wq", "Lump of Sugar"),
			// Hearts/AMUSE CRAFT (ナツイロココロログ + Happy Summer)
			new TableEntry("ナツイロココロログ", "kokorog_hs", "Hearts/AMUSE CRAFT", "Happy Summer"),
			new TableEntry("ナツイロココロログ", "kokorog", "Hearts/AMUSE CRAFT"),
			// Lump of Sugar
			new TableEntry("縁りて此の葉は紅に", "yorikure", "Lump of Sugar"),
			// PIXEL MINT
			new TableEntry("ぱらだいすお～しゃん", "po", "PIXEL MINT"),
			// Qruppo
			new TableEntry("抜きゲーみたいな島に住んでる貧乳はどうすりゃいいですか？２", "nukitasi2", "Qruppo"),
			new TableEntry("抜きゲーみたいな島に住んでる貧乳はどうすりゃいいですか", "nukitashi", "Qruppo"),
			// SAGA PLANETS
			new TableEntry("花咲ワークスプリング", "hanasaki", "SAGA PLANETS"),
			// sprite/fairys
			new TableEntry("蒼の彼方のフォーリズム", "aokana", "sprite/fairys"),
			// しろくまだんご
			new TableEntry("癒しの女神の実験台", "im", "しろくまだんご"),
			// CLEARRAVE / PALETTE
			new TableEntry("ここのつここのかここのいろ", "kokoiro", "CLEARRAVE / PALETTE"),
			new TableEntry("そらいろそらうたそらのおと", "sorairo", "CLEARRAVE / PALETTE"),
			new TableEntry("はるいろはるこいはるのかぜ", "haruiro", "CLEARRAVE / PALETTE"),
			new TableEntry("ゆきいろゆきはなゆきのあと", "yukiiro", "CLEARRAVE / PALETTE"),
			new TableEntry("9-nine-", "nineallage", "CLEARRAVE / PALETTE"),
			// MARMALADE
			new TableEntry("お家に帰るまでがましまろです", "mashimaro", "MARMALADE"),
			// まどそふと
			new TableEntry("ラズベリーキューブ", "raspberrycube", "まどそふと"),
			new TableEntry("ヤキモチストリーム", "yakisuto", "まどそふと"),
			new TableEntry("ワガママハイスペックOC", "wagahigh2", "まどそふと"),
			new TableEntry("ワガママハイスペック", "wagahigh", "まどそふと"),
			new TableEntry("ハミダシクリエイティブ", "hamidashi", "まどそふと"),
			// CRYSTALiA/AMUSE CRAFT
			new TableEntry("絆きらめく恋いろは", "mekuiro", "CRYSTALiA/AMUSE CRAFT"),
			// SMEE
			new TableEntry("Making＊Lovers", "ml", "SMEE"),
			new TableEntry("Sugar＊Style", "ss", "SMEE"),
			// onepoint
			new TableEntry("こいのす☆イチャコライズ", "koicha", "onepoint"),
			// Hearts/AMUSE CRAFT
			new TableEntry("恋するココロと魔法のコトバ", "koikoro", "Hearts/AMUSE CRAFT"),
			// SAGA PLANETS (金色ラブリッチェ)
			new TableEntry("金色ラブリッチェ-GoldenTime-", "kinkoi2", "SAGA PLANETS"),
			new TableEntry("金色ラブリッチェ", "kinkoi", "SAGA PLANETS"),
			// YUZUSOFT / JUNOS inc.
			new TableEntry("RIDDLE JOKER", "yuzusoft", "YUZUSOFT / JUNOS inc."),
			new TableEntry("サノバウィッチ", "yuzusoft", "YUZUSOFT / JUNOS inc."),
			new TableEntry("千恋＊万花", "yuzusoft", "YUZUSOFT / JUNOS inc."),
			new TableEntry("喫茶ステラと死神の蝶", "yuzusoft", "YUZUSOFT / JUNOS inc."),
			// スミレ
			new TableEntry("僕と恋するポンコツアクマ。すっごいえっち！", "koikuma2", "スミレ"),
			new TableEntry("僕と恋するポンコツアクマ。", "koikuma", "スミレ"),
			// SILKYS PLUS
			new TableEntry("きまぐれテンプテーション", "kimaten", "SILKYS PLUS"),
			// mirai (宿星のガールフレンド)
			new TableEntry("宿星のガールフレンド 芙慈子編", "syukugar4", "mirai"),
			new TableEntry("宿星のガールフレンド３", "syukugar3", "mirai"),
			new TableEntry("宿星のガールフレンド２", "syukugar2", "mirai"),
			new TableEntry("宿星のガールフレンド", "syukugar1", "mirai"),
			// AZARASHI SOFTWARE (アイカギ + アフターデイズ)
			new TableEntry("アイカギ", "aikagiad", "AZARASHI SOFTWARE", "アフターデイズ"),
			new TableEntry("アイカギ", "aikagi", "AZARASHI SOFTWARE"),
			// eufonie
			new TableEntry("はにデビ", "hanidevi", "eufonie"),
			// Campus
			new TableEntry("恋音セ・ピアーチェ", "sepiarche", "Campus"),
			// りびどーそふと
			new TableEntry("捻くれモノの学園青春物語", "hinekure", "りびどーそふと"),
			// みるくふぁくとりー
			new TableEntry("もっと！孕ませ！炎のおっぱい異世界エロ魔法学園！", "mhh", "みるくふぁくとりー"),
			// feng
			new TableEntry("夢と色でできている", "yumeiro", "feng"),
			// WillPlus/PULLTOP
			new TableEntry("さくらいろ、舞うころに", "sakurairo", "WillPlus/PULLTOP"),
			// hachimitsu_soft
			new TableEntry("アイシング-love coating-", "icing", "hachimitsu_soft"),
			// プレカノ
			new TableEntry("おにあま", "oniama", "プレカノ"),
			// ZION
			new TableEntry("聖光天使ノエル", "noer", "ZION"),
			// Mint CUBE
			new TableEntry("勇者と魔王と、魔女のカフェ", "majocafe", "Mint CUBE"),
			// AKABEi SOFT3
			new TableEntry("働くオトナの恋愛事情２", "renaijijo2", "AKABEi SOFT3"),
			new TableEntry("働くオトナの恋愛事情", "renaijijo", "AKABEi SOFT3"),
			// illusion
			new TableEntry("AI＊少女", "ais", "illusion"),
			new TableEntry("ハニーセレクト2", "hs2rbd", "illusion"),
			new TableEntry("コイカツ サンシャイン", "koikatuss", "illusion"),
			new TableEntry("コイカツ", "koikatu", "illusion"),
			new TableEntry("Koikatu", "koikatu", "illusion"),
			// Barista Lab
			new TableEntry("アナベル・メイドガーデン", "amg", "Barista Lab"),
			// hibiki works
			new TableEntry("ＰＲＥＴＴＹ×Ｃ∧ＴＩＯＮ２", "pc2", "hibiki works"),
			new TableEntry("ＰＲＥＴＴＹ×Ｃ∧ＴＩＯＮ", "pc", "hibiki works"),
			// CourregesA
			new TableEntry("えんとも", "entomo", "CourregesA"),
			// Purple software
			new TableEntry("青春フラジャイル", "fragile", "Purple software"),
			// PENCIL
			new TableEntry("あまあまシェアリング", "amasharing", "PENCIL"),
			// 裸足少女
			new TableEntry("プリンセスハートリンク", "phl", "裸足少女"),
			// SAGA PLANETS
			new TableEntry("かけぬけ★青春スパーキング！", "kakenuke", "SAGA PLANETS"),
			// Whirlpool
			new TableEntry("初情スプリンクル", "hatsujo", "Whirlpool"),
			new TableEntry("竜姫ぐーたらいふ", "drapri", "Whirlpool"),
			// Lump of Sugar
			new TableEntry("ねこツク、さくら。", "nekotsuku", "Lump of Sugar"),
			// onomatope＊raspberry
			new TableEntry("シス△キャン", "siscan", "onomatope＊raspberry"),
			// Whirlpool (pieces compound checks)
			new TableEntry("pieces", "somnium", "Whirlpool", "渡り鳥のソムニウム"),
			new TableEntry("pieces", "canary", "Whirlpool", "揺り籠のカナリア"),
			// ALICESOFT
			new TableEntry("ドーナドーナ いっしょにわるいことをしよう", "dohnadohna", "ALICESOFT"),
		};

		#endregion
	}
}
