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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.label1 = new System.Windows.Forms.Label();
			this.titleText = new System.Windows.Forms.TextBox();
			this.exePathText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.imgPathText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.iconImage = new System.Windows.Forms.PictureBox();
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
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.label10 = new System.Windows.Forms.Label();
			this.extractToolCombo = new System.Windows.Forms.ComboBox();
			this.getInfoButton = new System.Windows.Forms.Button();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.button5 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.iconImage)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(111, 254);
			this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "タイトル";
			// 
			// titleText
			// 
			this.titleText.Location = new System.Drawing.Point(210, 248);
			this.titleText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.titleText.MaxLength = 255;
			this.titleText.Name = "titleText";
			this.titleText.Size = new System.Drawing.Size(648, 31);
			this.titleText.TabIndex = 1;
			// 
			// exePathText
			// 
			this.exePathText.Location = new System.Drawing.Point(210, 298);
			this.exePathText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.exePathText.MaxLength = 500;
			this.exePathText.Name = "exePathText";
			this.exePathText.Size = new System.Drawing.Size(715, 31);
			this.exePathText.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(93, 304);
			this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "実行パス";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(943, 294);
			this.button1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(43, 46);
			this.button1.TabIndex = 11;
			this.button1.Text = "..";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(943, 394);
			this.button2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(43, 46);
			this.button2.TabIndex = 31;
			this.button2.Text = "..";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// imgPathText
			// 
			this.imgPathText.Location = new System.Drawing.Point(210, 398);
			this.imgPathText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.imgPathText.MaxLength = 500;
			this.imgPathText.Name = "imgPathText";
			this.imgPathText.Size = new System.Drawing.Size(715, 31);
			this.imgPathText.TabIndex = 30;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(93, 404);
			this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 24);
			this.label3.TabIndex = 5;
			this.label3.Text = "画像パス";
			// 
			// iconImage
			// 
			this.iconImage.BackColor = System.Drawing.Color.Transparent;
			this.iconImage.InitialImage = global::glc_cs.Properties.Resources.load;
			this.iconImage.Location = new System.Drawing.Point(26, 24);
			this.iconImage.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.iconImage.Name = "iconImage";
			this.iconImage.Size = new System.Drawing.Size(217, 200);
			this.iconImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.iconImage.TabIndex = 8;
			this.iconImage.TabStop = false;
			this.iconImage.WaitOnLoad = true;
			// 
			// gameTitleLabel
			// 
			this.gameTitleLabel.AutoSize = true;
			this.gameTitleLabel.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.gameTitleLabel.Location = new System.Drawing.Point(256, 170);
			this.gameTitleLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.gameTitleLabel.Name = "gameTitleLabel";
			this.gameTitleLabel.Size = new System.Drawing.Size(194, 26);
			this.gameTitleLabel.TabIndex = 9;
			this.gameTitleLabel.Text = "GameTitleLabel";
			this.gameTitleLabel.UseMnemonic = false;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(462, 548);
			this.button3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(43, 38);
			this.button3.TabIndex = 61;
			this.button3.Text = "0";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// dconText
			// 
			this.dconText.Location = new System.Drawing.Point(210, 448);
			this.dconText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.dconText.MaxLength = 50;
			this.dconText.Name = "dconText";
			this.dconText.Size = new System.Drawing.Size(771, 31);
			this.dconText.TabIndex = 40;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(56, 454);
			this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(131, 24);
			this.label5.TabIndex = 10;
			this.label5.Text = "dconテキスト";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(82, 554);
			this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(106, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "起動時間";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(540, 554);
			this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(106, 24);
			this.label7.TabIndex = 16;
			this.label7.Text = "起動回数";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(943, 548);
			this.button4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(43, 38);
			this.button4.TabIndex = 66;
			this.button4.Text = "0";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// ApplyButton
			// 
			this.ApplyButton.Location = new System.Drawing.Point(210, 706);
			this.ApplyButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(264, 66);
			this.ApplyButton.TabIndex = 80;
			this.ApplyButton.Text = "適用";
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(544, 706);
			this.CloseButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(264, 66);
			this.CloseButton.TabIndex = 85;
			this.CloseButton.Text = "キャンセル";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Enabled = false;
			this.checkBox1.Location = new System.Drawing.Point(407, 24);
			this.checkBox1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(143, 28);
			this.checkBox1.TabIndex = 22;
			this.checkBox1.Text = "ローカルINI";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// checkBox2
			// 
			this.checkBox2.AutoSize = true;
			this.checkBox2.Enabled = false;
			this.checkBox2.Location = new System.Drawing.Point(585, 24);
			this.checkBox2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(161, 28);
			this.checkBox2.TabIndex = 23;
			this.checkBox2.Text = "オンラインDB";
			this.checkBox2.UseVisualStyleBackColor = true;
			// 
			// checkBox3
			// 
			this.checkBox3.AutoSize = true;
			this.checkBox3.Enabled = false;
			this.checkBox3.Location = new System.Drawing.Point(780, 24);
			this.checkBox3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new System.Drawing.Size(183, 28);
			this.checkBox3.TabIndex = 24;
			this.checkBox3.Text = "オフラインモード";
			this.checkBox3.UseVisualStyleBackColor = true;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label9.Location = new System.Drawing.Point(256, 30);
			this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(90, 26);
			this.label9.TabIndex = 25;
			this.label9.Text = "GameID";
			// 
			// rateCheck
			// 
			this.rateCheck.AutoSize = true;
			this.rateCheck.Location = new System.Drawing.Point(210, 598);
			this.rateCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.rateCheck.Name = "rateCheck";
			this.rateCheck.Size = new System.Drawing.Size(209, 28);
			this.rateCheck.TabIndex = 70;
			this.rateCheck.Text = "成人向け（R-18）";
			this.rateCheck.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(132, 600);
			this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(61, 24);
			this.label8.TabIndex = 27;
			this.label8.Text = "フラグ";
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			// 
			// runTimeText
			// 
			this.runTimeText.Location = new System.Drawing.Point(210, 548);
			this.runTimeText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.runTimeText.Maximum = new decimal(new int[] {
            35791394,
            0,
            0,
            0});
			this.runTimeText.Name = "runTimeText";
			this.runTimeText.Size = new System.Drawing.Size(238, 31);
			this.runTimeText.TabIndex = 60;
			// 
			// startCountText
			// 
			this.startCountText.Location = new System.Drawing.Point(667, 548);
			this.startCountText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.startCountText.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.startCountText.Name = "startCountText";
			this.startCountText.Size = new System.Drawing.Size(262, 31);
			this.startCountText.TabIndex = 65;
			// 
			// dconImgText
			// 
			this.dconImgText.Location = new System.Drawing.Point(210, 498);
			this.dconImgText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.dconImgText.MaxLength = 50;
			this.dconImgText.Name = "dconImgText";
			this.dconImgText.Size = new System.Drawing.Size(771, 31);
			this.dconImgText.TabIndex = 50;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(30, 504);
			this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 24);
			this.label4.TabIndex = 29;
			this.label4.Text = "dconイメージID";
			// 
			// executeCmdText
			// 
			this.executeCmdText.Location = new System.Drawing.Point(210, 348);
			this.executeCmdText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.executeCmdText.MaxLength = 255;
			this.executeCmdText.Name = "executeCmdText";
			this.executeCmdText.Size = new System.Drawing.Size(771, 31);
			this.executeCmdText.TabIndex = 20;
			// 
			// executeCmdTextLabel
			// 
			this.executeCmdTextLabel.AutoSize = true;
			this.executeCmdTextLabel.Location = new System.Drawing.Point(39, 354);
			this.executeCmdTextLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.executeCmdTextLabel.Name = "executeCmdTextLabel";
			this.executeCmdTextLabel.Size = new System.Drawing.Size(149, 24);
			this.executeCmdTextLabel.TabIndex = 30;
			this.executeCmdTextLabel.Text = "実行パラメータ";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(658, 600);
			this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(58, 24);
			this.label10.TabIndex = 32;
			this.label10.Text = "抽出";
			this.label10.Visible = false;
			// 
			// extractToolCombo
			// 
			this.extractToolCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.extractToolCombo.FormattingEnabled = true;
			this.extractToolCombo.Items.AddRange(new object[] {
            "",
            "krkr",
            "krkrz",
            "krkrDump",
            "カスタム1",
            "カスタム2"});
			this.extractToolCombo.Location = new System.Drawing.Point(728, 597);
			this.extractToolCombo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.extractToolCombo.Name = "extractToolCombo";
			this.extractToolCombo.Size = new System.Drawing.Size(258, 32);
			this.extractToolCombo.TabIndex = 75;
			this.extractToolCombo.Visible = false;
			// 
			// getInfoButton
			// 
			this.getInfoButton.Location = new System.Drawing.Point(875, 244);
			this.getInfoButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.getInfoButton.Name = "getInfoButton";
			this.getInfoButton.Size = new System.Drawing.Size(111, 46);
			this.getInfoButton.TabIndex = 5;
			this.getInfoButton.Text = "DLsite";
			this.getInfoButton.UseVisualStyleBackColor = true;
			this.getInfoButton.Click += new System.EventHandler(this.getInfoButton_Click);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Enabled = false;
			this.checkBox4.Location = new System.Drawing.Point(431, 598);
			this.checkBox4.Margin = new System.Windows.Forms.Padding(6);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(205, 28);
			this.checkBox4.TabIndex = 86;
			this.checkBox4.Text = "セーブデータ共有";
			this.checkBox4.UseVisualStyleBackColor = true;
			this.checkBox4.Visible = false;
			// 
			// button5
			// 
			this.button5.Enabled = false;
			this.button5.Location = new System.Drawing.Point(943, 637);
			this.button5.Margin = new System.Windows.Forms.Padding(6);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(43, 46);
			this.button5.TabIndex = 89;
			this.button5.Text = "...";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Visible = false;
			// 
			// textBox1
			// 
			this.textBox1.Enabled = false;
			this.textBox1.Location = new System.Drawing.Point(214, 645);
			this.textBox1.Margin = new System.Windows.Forms.Padding(6);
			this.textBox1.MaxLength = 500;
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(662, 31);
			this.textBox1.TabIndex = 88;
			this.textBox1.Visible = false;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Enabled = false;
			this.label11.Location = new System.Drawing.Point(67, 648);
			this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(125, 24);
			this.label11.TabIndex = 87;
			this.label11.Text = "セーブデータ";
			this.label11.Visible = false;
			// 
			// button6
			// 
			this.button6.Enabled = false;
			this.button6.Location = new System.Drawing.Point(888, 637);
			this.button6.Margin = new System.Windows.Forms.Padding(6);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(43, 46);
			this.button6.TabIndex = 90;
			this.button6.Text = "..";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Visible = false;
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1012, 802);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.getInfoButton);
			this.Controls.Add(this.extractToolCombo);
			this.Controls.Add(this.label10);
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
			this.Controls.Add(this.iconImage);
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
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Editor";
			this.Text = "登録データを編集";
			((System.ComponentModel.ISupportInitialize)(this.iconImage)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
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
		private System.Windows.Forms.PictureBox iconImage;
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
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.ComboBox extractToolCombo;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button getInfoButton;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox checkBox4;
	}
}