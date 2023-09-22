namespace glc_cs
{
	partial class Memo
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Memo));
			this.gameIDLabel = new System.Windows.Forms.Label();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.CloseButton = new System.Windows.Forms.Button();
			this.ApplyButton = new System.Windows.Forms.Button();
			this.gameTitleLabel = new System.Windows.Forms.Label();
			this.gameImage = new System.Windows.Forms.PictureBox();
			this.memoLabel = new System.Windows.Forms.Label();
			this.memoTextBox = new System.Windows.Forms.RichTextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.gameImage)).BeginInit();
			this.SuspendLayout();
			// 
			// gameIDLabel
			// 
			this.gameIDLabel.AutoSize = true;
			this.gameIDLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.gameIDLabel.Location = new System.Drawing.Point(256, 30);
			this.gameIDLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.gameIDLabel.Name = "gameIDLabel";
			this.gameIDLabel.Size = new System.Drawing.Size(90, 26);
			this.gameIDLabel.TabIndex = 34;
			this.gameIDLabel.Text = "GameID";
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Enabled = false;
			this.checkBox3.Location = new System.Drawing.Point(780, 24);
			this.checkBox3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(183, 28);
			this.checkBox3.TabIndex = 33;
			this.checkBox3.Text = "オフラインモード";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Enabled = false;
			this.checkBox2.Location = new System.Drawing.Point(585, 24);
			this.checkBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(161, 28);
			this.checkBox2.TabIndex = 32;
			this.checkBox2.Text = "オンラインDB";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Location = new System.Drawing.Point(407, 24);
			this.checkBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(143, 28);
			this.checkBox1.TabIndex = 31;
			this.checkBox1.Text = "ローカルINI";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(544, 600);
			this.CloseButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(264, 66);
			this.CloseButton.TabIndex = 30;
			this.CloseButton.Text = "キャンセル";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// ApplyButton
			// 
			this.ApplyButton.Location = new System.Drawing.Point(210, 600);
			this.ApplyButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(264, 66);
			this.ApplyButton.TabIndex = 29;
			this.ApplyButton.Text = "適用";
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// gameTitleLabel
			// 
			this.gameTitleLabel.AutoSize = true;
			this.gameTitleLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.gameTitleLabel.Location = new System.Drawing.Point(256, 170);
			this.gameTitleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.gameTitleLabel.Name = "gameTitleLabel";
			this.gameTitleLabel.Size = new System.Drawing.Size(194, 26);
			this.gameTitleLabel.TabIndex = 28;
			this.gameTitleLabel.Text = "GameTitleLabel";
			this.gameTitleLabel.UseMnemonic = false;
			// 
			// gameImage
			// 
			this.gameImage.BackColor = System.Drawing.Color.Transparent;
			this.gameImage.InitialImage = global::glc_cs.Properties.Resources.load;
			this.gameImage.Location = new System.Drawing.Point(26, 24);
			this.gameImage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.gameImage.Name = "gameImage";
			this.gameImage.Size = new System.Drawing.Size(217, 200);
			this.gameImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.gameImage.TabIndex = 27;
			this.gameImage.TabStop = false;
			this.gameImage.WaitOnLoad = true;
			// 
			// memoLabel
			// 
			this.memoLabel.AutoSize = true;
			this.memoLabel.Location = new System.Drawing.Point(26, 250);
			this.memoLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.memoLabel.Name = "memoLabel";
			this.memoLabel.Size = new System.Drawing.Size(45, 24);
			this.memoLabel.TabIndex = 26;
			this.memoLabel.Text = "メモ";
			// 
			// memoTextBox
			// 
			this.memoTextBox.Location = new System.Drawing.Point(26, 280);
			this.memoTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.memoTextBox.MaxLength = 500;
			this.memoTextBox.Multiline = false;
			this.memoTextBox.Name = "memoTextBox";
			this.memoTextBox.Size = new System.Drawing.Size(949, 304);
			this.memoTextBox.TabIndex = 35;
			this.memoTextBox.Text = "";
			// 
			// Memo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1005, 700);
			this.Controls.Add(this.memoTextBox);
			this.Controls.Add(this.gameIDLabel);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.ApplyButton);
			this.Controls.Add(this.gameTitleLabel);
			this.Controls.Add(this.gameImage);
			this.Controls.Add(this.memoLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Memo";
			this.Text = "メモを編集";
			((System.ComponentModel.ISupportInitialize)(this.gameImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label gameIDLabel;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Button ApplyButton;
		private System.Windows.Forms.Label gameTitleLabel;
		private System.Windows.Forms.PictureBox gameImage;
		private System.Windows.Forms.Label memoLabel;
		private System.Windows.Forms.RichTextBox memoTextBox;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}