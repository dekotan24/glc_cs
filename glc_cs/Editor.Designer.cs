namespace glc_cs
{
	partial class Editor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.label1 = new System.Windows.Forms.Label();
			this.titleText = new System.Windows.Forms.TextBox();
			this.exePathText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.imgPathText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.gameTitleLabel = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.dconText = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.button4 = new System.Windows.Forms.Button();
			this.ApplyButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.checkBox3 = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.rateCheck = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.runTimeText = new System.Windows.Forms.NumericUpDown();
			this.startCountText = new System.Windows.Forms.NumericUpDown();
			this.dconImgText = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.executeCmdText = new System.Windows.Forms.TextBox();
			this.executeCmdTextLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(51, 127);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "タイトル";
			// 
			// titleText
			// 
			this.titleText.Location = new System.Drawing.Point(97, 124);
			this.titleText.MaxLength = 255;
			this.titleText.Name = "titleText";
			this.titleText.Size = new System.Drawing.Size(358, 19);
			this.titleText.TabIndex = 1;
			// 
			// exePathText
			// 
			this.exePathText.Location = new System.Drawing.Point(97, 149);
			this.exePathText.MaxLength = 500;
			this.exePathText.Name = "exePathText";
			this.exePathText.Size = new System.Drawing.Size(332, 19);
			this.exePathText.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "実行パス";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(435, 147);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(20, 23);
			this.button1.TabIndex = 3;
			this.button1.Text = "..";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(435, 197);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(20, 23);
			this.button2.TabIndex = 6;
			this.button2.Text = "..";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// imgPathText
			// 
			this.imgPathText.Location = new System.Drawing.Point(97, 199);
			this.imgPathText.MaxLength = 500;
			this.imgPathText.Name = "imgPathText";
			this.imgPathText.Size = new System.Drawing.Size(332, 19);
			this.imgPathText.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(43, 202);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "画像パス";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.InitialImage = global::glc_cs.Properties.Resources.load;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(100, 100);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.WaitOnLoad = true;
			// 
			// gameTitleLabel
			// 
			this.gameTitleLabel.AutoSize = true;
			this.gameTitleLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.gameTitleLabel.Location = new System.Drawing.Point(118, 85);
			this.gameTitleLabel.Name = "gameTitleLabel";
			this.gameTitleLabel.Size = new System.Drawing.Size(105, 13);
			this.gameTitleLabel.TabIndex = 9;
			this.gameTitleLabel.Text = "GameTitleLabel";
			this.gameTitleLabel.UseMnemonic = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(213, 274);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(20, 19);
			this.button3.TabIndex = 10;
			this.button3.Text = "0";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// dconText
			// 
			this.dconText.Location = new System.Drawing.Point(97, 224);
			this.dconText.MaxLength = 50;
			this.dconText.Name = "dconText";
			this.dconText.Size = new System.Drawing.Size(358, 19);
			this.dconText.TabIndex = 7;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(26, 227);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "dconテキスト";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(38, 277);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 12);
			this.label6.TabIndex = 13;
			this.label6.Text = "起動時間";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(249, 277);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 16;
			this.label7.Text = "起動回数";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(435, 274);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(20, 19);
			this.button4.TabIndex = 12;
			this.button4.Text = "0";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// ApplyButton
			// 
			this.ApplyButton.Location = new System.Drawing.Point(97, 325);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(122, 33);
			this.ApplyButton.TabIndex = 14;
			this.ApplyButton.Text = "適用";
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(251, 325);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(122, 33);
			this.CloseButton.TabIndex = 15;
			this.CloseButton.Text = "キャンセル";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Location = new System.Drawing.Point(188, 12);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(76, 16);
			this.checkBox1.TabIndex = 22;
			this.checkBox1.Text = "ローカルINI";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Enabled = false;
			this.checkBox2.Location = new System.Drawing.Point(270, 12);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(84, 16);
			this.checkBox2.TabIndex = 23;
			this.checkBox2.Text = "オンラインDB";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Enabled = false;
			this.checkBox3.Location = new System.Drawing.Point(360, 12);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(95, 16);
			this.checkBox3.TabIndex = 24;
			this.checkBox3.Text = "オフラインモード";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label9.Location = new System.Drawing.Point(118, 15);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(49, 13);
			this.label9.TabIndex = 25;
			this.label9.Text = "GameID";
			// 
			// rateCheck
			// 
			this.rateCheck.AutoSize = true;
			this.rateCheck.Location = new System.Drawing.Point(97, 299);
			this.rateCheck.Name = "rateCheck";
			this.rateCheck.Size = new System.Drawing.Size(108, 16);
			this.rateCheck.TabIndex = 13;
			this.rateCheck.Text = "成人向け（R-18）";
			this.rateCheck.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(61, 300);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(30, 12);
			this.label8.TabIndex = 27;
			this.label8.Text = "フラグ";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// runTimeText
			// 
			this.runTimeText.Location = new System.Drawing.Point(97, 274);
			this.runTimeText.Maximum = new decimal(new int[] {
            35791394,
            0,
            0,
            0});
			this.runTimeText.Name = "runTimeText";
			this.runTimeText.Size = new System.Drawing.Size(110, 19);
			this.runTimeText.TabIndex = 9;
			// 
			// startCountText
			// 
			this.startCountText.Location = new System.Drawing.Point(308, 274);
			this.startCountText.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.startCountText.Name = "startCountText";
			this.startCountText.Size = new System.Drawing.Size(121, 19);
			this.startCountText.TabIndex = 11;
			// 
			// dconImgText
			// 
			this.dconImgText.Location = new System.Drawing.Point(97, 249);
			this.dconImgText.MaxLength = 50;
			this.dconImgText.Name = "dconImgText";
			this.dconImgText.Size = new System.Drawing.Size(358, 19);
			this.dconImgText.TabIndex = 8;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 252);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 12);
			this.label4.TabIndex = 29;
			this.label4.Text = "dconイメージID";
			// 
			// executeCmdText
			// 
			this.executeCmdText.Location = new System.Drawing.Point(97, 174);
			this.executeCmdText.MaxLength = 255;
			this.executeCmdText.Name = "executeCmdText";
			this.executeCmdText.Size = new System.Drawing.Size(358, 19);
			this.executeCmdText.TabIndex = 4;
			// 
			// executeCmdTextLabel
			// 
			this.executeCmdTextLabel.AutoSize = true;
			this.executeCmdTextLabel.Location = new System.Drawing.Point(18, 177);
			this.executeCmdTextLabel.Name = "executeCmdTextLabel";
			this.executeCmdTextLabel.Size = new System.Drawing.Size(73, 12);
			this.executeCmdTextLabel.TabIndex = 30;
			this.executeCmdTextLabel.Text = "実行パラメータ";
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(467, 374);
			this.Controls.Add(this.executeCmdText);
			this.Controls.Add(this.executeCmdTextLabel);
			this.Controls.Add(this.dconImgText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.startCountText);
			this.Controls.Add(this.runTimeText);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.rateCheck);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.checkBox3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.ApplyButton);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.dconText);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gameTitleLabel);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.imgPathText);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.exePathText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.titleText);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Editor";
			this.Text = "Editor";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox titleText;
		private System.Windows.Forms.TextBox exePathText;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TextBox imgPathText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label gameTitleLabel;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox dconText;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button ApplyButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.CheckBox rateCheck;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.NumericUpDown runTimeText;
		private System.Windows.Forms.NumericUpDown startCountText;
		private System.Windows.Forms.TextBox dconImgText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox executeCmdText;
		private System.Windows.Forms.Label executeCmdTextLabel;
	}
}