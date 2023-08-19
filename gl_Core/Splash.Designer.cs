namespace glc_cs
{
	partial class Splash
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = global::glc_cs.Properties.Resources.icon;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(10, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 100);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(120, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(218, 20);
			this.label1.TabIndex = 1;
			this.label1.Text = "Game Launcher C# Edition";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.linkLabel1.Location = new System.Drawing.Point(153, 93);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(194, 12);
			this.linkLabel1.TabIndex = 2;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "https://github.com/dekotan24/glc_cs";
			// 
			// label3
			// 
			this.label3.AutoEllipsis = true;
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(111, 105);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Hash:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(164, 62);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 12);
			this.label2.TabIndex = 5;
			this.label2.Text = "Please wait a moment..";
			// 
			// Splash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(350, 120);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Splash";
			this.Opacity = 0.9D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Game Launcher C# Splash Screen";
			this.Load += new System.EventHandler(this.Form3_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
	}
}