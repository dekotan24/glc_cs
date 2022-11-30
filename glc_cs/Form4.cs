using System;
using System.Windows.Forms;

namespace glc_cs
{
	public partial class Form4 : Form
	{
		public Form4(string imgPath)
		{
			InitializeComponent();
			pictureBox1.ImageLocation = imgPath;
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			if (trackBar1.Value == 0)
			{
				pictureBox1.Width = 100;
				pictureBox1.Height = 100;
			}
			else if (trackBar1.Value == 1)
			{
				pictureBox1.Width = 200;
				pictureBox1.Height = 200;
			}
			else
			{
				pictureBox1.Width = 300;
				pictureBox1.Height = 300;
			}
		}
	}
}
