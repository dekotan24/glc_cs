using System;
using System.Net;
using System.Windows.Forms;
using static glc_cs.General.Var;

namespace glc_cs
{
	public partial class DLsite : Form
	{
		public string resultText = string.Empty;
		public string resultImagePath = string.Empty;

		public DLsite()
		{
			InitializeComponent();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			resultText = string.Empty;
			resultImagePath = string.Empty;
			this.Hide();
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(searchResultText.Text.Trim()))
			{
				resultText = searchResultText.Text.Trim();
				this.Hide();
			}
			else
			{
				searchTargetText.Focus();
			}
		}

		private void searchButton_Click(object sender, EventArgs e)
		{
			searchResultText.Text = string.Empty;
			resultText = string.Empty;
			resultImagePath = string.Empty;

			var main = new DLsiteInfoGetter.Main();

			if (!string.IsNullOrEmpty(searchTargetText.Text.Trim()))
			{
				bool result = main.GetInfo(searchTargetText.Text.Trim(), out string prodID, out string searchResult, out string circle, out string prodType, out string imageUrl, out string errMsg);

				if (!string.IsNullOrEmpty(errMsg))
				{
					MessageBox.Show(errMsg, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					searchButton.Focus();
					return;
				}
				// 結果がtrueの場合、結果を格納する。
				if (result)
				{
					resultText = searchResult;
					searchResultText.Text = searchResult;
					imageText.Text = imageUrl;
					try
					{
						imageBox.ImageLocation = imageUrl;
					}
					catch (Exception ex)
					{
						imageBox.ImageLocation = null;
						MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					saveButton.Focus();
				}
			}
			else
			{
				searchTargetText.Focus();
			}
		}

		private void saveImageButton_Click(object sender, EventArgs e)
		{
			saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.FileName = "thumb.jpg";
			saveFileDialog1.Title = "画像を保存";
			saveFileDialog1.Filter = "画像ファイル (*.jpg)|*.jpg";
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					WebClient wc = new WebClient();
					wc.DownloadFile(imageText.Text, saveFileDialog1.FileName);
					resultImagePath = saveFileDialog1.FileName;
					wc.Dispose();
					System.Media.SystemSounds.Beep.Play();
				}
				catch (Exception ex)
				{
					resultImagePath = string.Empty;
					MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}
