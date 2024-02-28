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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.statusLabel = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.MemoryLabel = new System.Windows.Forms.Label();
			this.progressBar2 = new System.Windows.Forms.ProgressBar();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = global::glc_cs.Properties.Resources.icon;
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(9, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 100);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(147, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(111, 24);
			this.label1.TabIndex = 1;
			this.label1.Text = "GLauncher";
			// 
			// statusLabel
			// 
			this.statusLabel.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.statusLabel.Location = new System.Drawing.Point(112, 56);
			this.statusLabel.Name = "statusLabel";
			this.statusLabel.Size = new System.Drawing.Size(231, 16);
			this.statusLabel.TabIndex = 5;
			this.statusLabel.Text = "Please wait a moment..";
			this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(12, 115);
			this.progressBar1.Maximum = 6;
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(326, 10);
			this.progressBar1.Step = 1;
			this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar1.TabIndex = 6;
			// 
			// MemoryLabel
			// 
			this.MemoryLabel.AutoEllipsis = true;
			this.MemoryLabel.AutoSize = true;
			this.MemoryLabel.BackColor = System.Drawing.Color.Transparent;
			this.MemoryLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MemoryLabel.Location = new System.Drawing.Point(116, 95);
			this.MemoryLabel.Name = "MemoryLabel";
			this.MemoryLabel.Size = new System.Drawing.Size(102, 13);
			this.MemoryLabel.TabIndex = 7;
			this.MemoryLabel.Text = "Memory Use: xxMB";
			this.MemoryLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// progressBar2
			// 
			this.progressBar2.Location = new System.Drawing.Point(114, 76);
			this.progressBar2.Maximum = 6;
			this.progressBar2.Name = "progressBar2";
			this.progressBar2.Size = new System.Drawing.Size(223, 8);
			this.progressBar2.Step = 1;
			this.progressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.progressBar2.TabIndex = 8;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Meiryo UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(281, 28);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 14);
			this.label2.TabIndex = 9;
			this.label2.Text = "Ver.x.x.x";
			// 
			// Splash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(350, 129);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.progressBar2);
			this.Controls.Add(this.MemoryLabel);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.statusLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pictureBox1);
			this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Splash";
			this.Opacity = 0.9D;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "GLauncher C# Splash Screen";
			this.Load += new System.EventHandler(this.Form3_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label MemoryLabel;
		private System.Windows.Forms.ProgressBar progressBar2;
		private System.Windows.Forms.Label label2;
	}
}