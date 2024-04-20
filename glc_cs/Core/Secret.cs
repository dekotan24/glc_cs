namespace glc_cs.Core
{
	internal class Secret
	{
		internal class DMM
		{
			private readonly static string dmmAPI = "dmmAffiliateAPIKey";  // DMMのAPIキー
			private readonly static string dmmAffID = "dmmaff-990";        // DMMのアフィリエイトID

			/// <summary>
			/// DMMのAPIキー
			/// </summary>
			internal static string GetDmmAPI
			{
				get { return dmmAPI; }
			}

			/// <summary>
			/// DMMのアフィリエイトID
			/// </summary>
			internal static string GetDmmAffiliateID
			{
				get { return dmmAffID; }
			}
		}

		internal class Crypt
		{
			private readonly static string cryptKey = "e3UW9cGHS6j0o4UxN5aZ8dB7p1QfM3dD";  // 32桁の暗号化用キー
			private readonly static string cryptVector = "f2TV8bFGR5i9n3Tx";   // 16桁の暗号化用初期ベクトル

			internal static string CryptKey
			{
				get { return cryptKey; }
			}

			internal static string CryptVector
			{
				get { return cryptVector; }
			}
		}

	}
}
