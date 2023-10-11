namespace glc_cs
{
	internal class AuthData
	{
		private readonly string dmmAPI = "hHzqSTpm5ZNmZwn6Fbew";    // DMMのAPIキー
		private readonly string dmmAffID = "deko-990";              // DMMのアフィリエイトID

		/// <summary>
		/// DMMのAPIキー
		/// </summary>
		protected string GetDmmAPI
		{
			get { return dmmAPI; }
		}

	}
}
