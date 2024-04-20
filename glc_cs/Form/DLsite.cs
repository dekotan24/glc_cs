using DLsiteInfoGetter;
using System;
using System.Net;
using System.Windows.Forms;
using static glc_cs.Core.Property;

namespace glc_cs
{
	public partial class DLsite : Form
	{
		public string resultText = string.Empty;
		public string resultImagePath = string.Empty;
		public bool resultImageSaved = false;
		private string productID = string.Empty;

		public DLsite()
		{
			InitializeComponent();
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			resultText = string.Empty;
			resultImagePath = string.Empty;
			this.Hide();
		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(SearchResultText.Text.Trim()))
			{
				resultText = SearchResultText.Text.Trim();
				resultImageSaved = imageSavedCheck.Checked;
				this.Hide();
			}
			else
			{
				SearchTargetText.Focus();
			}
		}

		private void SearchButton_Click(object sender, EventArgs e)
		{
			SearchResultText.Text = string.Empty;
			resultText = string.Empty;
			resultImagePath = string.Empty;

			if (!string.IsNullOrEmpty(SearchTargetText.Text.Trim()))
			{
				try
				{
					DLsiteInfo result = DLsiteInfo.GetInfo(SearchTargetText.Text.Trim());

					SearchResultText.Text = result.Title;
					ImageText.Text = result.ImageUrl;
					ImageBox.ImageLocation = result.ImageUrl;
					productID = result.ProductId;
				}
				catch (Exception ex)
				{
					SearchResultText.Text = string.Empty;
					ImageText.Text = string.Empty;
					ImageBox.ImageLocation = null;
					productID = string.Empty;
					imageSavedCheck.Enabled = false;
					imageSavedCheck.Checked = false;
					MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				SaveButton.Focus();
			}
			else
			{
				SearchTargetText.Focus();
			}
		}

		private void SaveImageButton_Click(object sender, EventArgs e)
		{
			saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.FileName = productID + "_thumb.jpg";
			saveFileDialog1.Title = "画像を保存";
			saveFileDialog1.Filter = "画像ファイル (*.jpg)|*.jpg";
			if (saveFileDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					WebClient wc = new WebClient();
					wc.DownloadFile(ImageText.Text, saveFileDialog1.FileName);
					resultImagePath = saveFileDialog1.FileName;
					wc.Dispose();
					imageSavedCheck.Checked = true;
					imageSavedCheck.Enabled = true;
					System.Media.SystemSounds.Beep.Play();
				}
				catch (Exception ex)
				{
					resultImagePath = string.Empty;
					imageSavedCheck.Checked = false;
					imageSavedCheck.Enabled = false;
					MessageBox.Show(ex.Message, AppName, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void SearchButton_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Enter)
			{
				SearchButton_Click(sender, e);
			}
		}
	}
}
