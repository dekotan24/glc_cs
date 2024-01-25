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
			// label3.Text = "MD5: " + ComputeFileHash(Application.ExecutablePath);
		}

		private static string ComputeFileHash(string filePath)
		{
			var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
			var bs = hashProvider.ComputeHash(fs);
			fs.Close();
			return BitConverter.ToString(bs).ToLower().Replace("-", "");
		}

		public void SetProgress(int value, string message, int value2 = 0, int value3 = 0)
		{
			string appendMsg = " [" + value2 + " / " + value3 + "]";
			if (value == -1 && value3 != 0)
			{
				if (!progressBar2.Visible)
				{
					progressBar2.Visible = true;
					progressBar2.Maximum = value3;
				}
				value = progressBar1.Value;
				progressBar2.Value = value2;
				message = message + appendMsg;
			}
			else
			{
				progressBar2.Visible = false;
			}

			if (value == -1)
			{
				value = progressBar1.Value;
			}

			progressBar1.Value = value;
			statusLabel.Text = message + " (" + value.ToString() + " / " + progressBar1.Maximum + ")";
			MemoryLabel.Text = "Memory Use: " + (Environment.WorkingSet / 1024 / 1024) + "MB";
			Application.DoEvents();
		}
	}
}
