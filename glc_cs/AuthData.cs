namespace glc_cs
{
	internal class AuthData
	{
		private readonly string dmmAPI = "dmmAffiliateAPIKey";	// DMMのAPIキー
		private readonly string dmmAffID = "dmmaff-990";		// DMMのアフィリエイトID

		/// <summary>
		/// DMMのAPIキー
		/// </summary>
		protected string GetDmmAPI
		{
			get { return dmmAPI; }
		}

		/// <summary>
		/// DMMのアフィリエイトID
		/// </summary>
		protected string GetDmmAffiliateID
		{
			get { return dmmAffID; }
		}

	}
}
