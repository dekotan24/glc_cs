using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class Form3 : Form
	{
		static readonly HashAlgorithm hashProvider = new MD5CryptoServiceProvider();

		public Form3()
		{
			InitializeComponent();
		}

		private void Form3_Load(object sender, EventArgs e)
		{
			label3.Text = "MD5: " + ComputeFileHash(Application.ExecutablePath);
		}

		public static string ComputeFileHash(string filePath)
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			var bs = hashProvider.ComputeHash(fs);
			return BitConverter.ToString(bs).ToLower().Replace("-", "");
		}
	}
}
