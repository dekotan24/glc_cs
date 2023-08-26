using System;
using System.IO;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class Splash : Form
	{
		static readonly HashAlgorithm hashProvider = new MD5CryptoServiceProvider();

		public Splash()
		{
			InitializeComponent();
		}

		private void Form3_Load(object sender, EventArgs e)
		{
			label3.Text = "MD5: " + ComputeFileHash(Application.ExecutablePath);
		}

		private static string ComputeFileHash(string filePath)
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var bs = hashProvider.ComputeHash(fs);
			fs.Close();
			return BitConverter.ToString(bs).ToLower().Replace("-", "");
		}

		public void SetProgress(int value, string message)
		{
			progressBar1.Value = value;
			statusLabel.Text = message + " (" + value.ToString() + " / " + progressBar1.Maximum + ")";
			Application.DoEvents();
		}
	}
}
