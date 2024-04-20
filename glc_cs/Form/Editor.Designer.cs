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
			this.exePathButton = new System.Windows.Forms.Button();
			this.imgPathButton = new System.Windows.Forms.Button();
			this.imgPathText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.iconImage = new System.Windows.Forms.PictureBox();
			this.gameTitleLabel = new System.Windows.Forms.Label();
			this.runTimeResetButton = new System.Windows.Forms.Button();
			this.dconText = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.startCountResetButton = new System.Windows.Forms.Button();
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
			this.label10 = new System.Windows.Forms.Label();
			this.extractToolCombo = new System.Windows.Forms.ComboBox();
			this.GetDLsiteInfoButton = new System.Windows.Forms.Button();
			this.checkBox4 = new System.Windows.Forms.CheckBox();
			this.button5 = new System.Windows.Forms.Button();
			this.savePathText = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.button6 = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.GetVNDBInfoButton = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.iconImage)).BeginInit();
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
			this.titleText.Size = new System.Drawing.Size(370, 19);
			this.titleText.TabIndex = 1;
			this.toolTip1.SetToolTip(this.titleText, "ゲームタイトル");
			// 
			// exePathText
			// 
			this.exePathText.Location = new System.Drawing.Point(97, 178);
			this.exePathText.MaxLength = 500;
			this.exePathText.Name = "exePathText";
			this.exePathText.Size = new System.Drawing.Size(344, 19);
			this.exePathText.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(43, 181);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "実行パス";
			// 
			// exePathButton
			// 
			this.exePathButton.Location = new System.Drawing.Point(447, 176);
			this.exePathButton.Name = "exePathButton";
			this.exePathButton.Size = new System.Drawing.Size(20, 23);
			this.exePathButton.TabIndex = 11;
			this.exePathButton.Text = "..";
			this.exePathButton.UseVisualStyleBackColor = true;
			this.exePathButton.Click += new System.EventHandler(this.exePathButton_Click);
			// 
			// imgPathButton
			// 
			this.imgPathButton.Location = new System.Drawing.Point(447, 226);
			this.imgPathButton.Name = "imgPathButton";
			this.imgPathButton.Size = new System.Drawing.Size(20, 23);
			this.imgPathButton.TabIndex = 31;
			this.imgPathButton.Text = "..";
			this.imgPathButton.UseVisualStyleBackColor = true;
			this.imgPathButton.Click += new System.EventHandler(this.imgPathButton_Click);
			// 
			// imgPathText
			// 
			this.imgPathText.Location = new System.Drawing.Point(97, 228);
			this.imgPathText.MaxLength = 500;
			this.imgPathText.Name = "imgPathText";
			this.imgPathText.Size = new System.Drawing.Size(344, 19);
			this.imgPathText.TabIndex = 30;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(43, 231);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "画像パス";
			// 
			// iconImage
			// 
			this.iconImage.BackColor = System.Drawing.Color.Transparent;
			this.iconImage.InitialImage = global::glc_cs.Properties.Resources.load;
			this.iconImage.Location = new System.Drawing.Point(12, 12);
			this.iconImage.Name = "iconImage";
			this.iconImage.Size = new System.Drawing.Size(100, 100);
			this.iconImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.iconImage.TabIndex = 8;
			this.iconImage.TabStop = false;
			this.iconImage.WaitOnLoad = true;
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
			// runTimeResetButton
			// 
			this.runTimeResetButton.Location = new System.Drawing.Point(213, 303);
			this.runTimeResetButton.Name = "runTimeResetButton";
			this.runTimeResetButton.Size = new System.Drawing.Size(20, 19);
			this.runTimeResetButton.TabIndex = 61;
			this.runTimeResetButton.Text = "0";
			this.runTimeResetButton.UseVisualStyleBackColor = true;
			this.runTimeResetButton.Click += new System.EventHandler(this.runTimeResetButton_Click);
			// 
			// dconText
			// 
			this.dconText.Location = new System.Drawing.Point(97, 253);
			this.dconText.MaxLength = 50;
			this.dconText.Name = "dconText";
			this.dconText.Size = new System.Drawing.Size(370, 19);
			this.dconText.TabIndex = 40;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(26, 256);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "dconテキスト";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(38, 305);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 12);
			this.label6.TabIndex = 13;
			this.label6.Text = "起動時間";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(261, 305);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 16;
			this.label7.Text = "起動回数";
			// 
			// startCountResetButton
			// 
			this.startCountResetButton.Location = new System.Drawing.Point(447, 303);
			this.startCountResetButton.Name = "startCountResetButton";
			this.startCountResetButton.Size = new System.Drawing.Size(20, 19);
			this.startCountResetButton.TabIndex = 66;
			this.startCountResetButton.Text = "0";
			this.startCountResetButton.UseVisualStyleBackColor = true;
			this.startCountResetButton.Click += new System.EventHandler(this.startCountResetButton_Click);
			// 
			// ApplyButton
			// 
			this.ApplyButton.Location = new System.Drawing.Point(98, 394);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(122, 33);
			this.ApplyButton.TabIndex = 80;
			this.ApplyButton.Text = "適用";
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(251, 394);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(122, 33);
			this.CloseButton.TabIndex = 85;
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
			this.rateCheck.Location = new System.Drawing.Point(97, 330);
			this.rateCheck.Name = "rateCheck";
			this.rateCheck.Size = new System.Drawing.Size(108, 16);
			this.rateCheck.TabIndex = 70;
			this.rateCheck.Text = "成人向け（R-18）";
			this.rateCheck.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(61, 331);
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
			this.runTimeText.Location = new System.Drawing.Point(97, 303);
			this.runTimeText.Maximum = new decimal(new int[] {
            35791394,
            0,
            0,
            0});
			this.runTimeText.Name = "runTimeText";
			this.runTimeText.Size = new System.Drawing.Size(110, 19);
			this.runTimeText.TabIndex = 60;
			// 
			// startCountText
			// 
			this.startCountText.Location = new System.Drawing.Point(320, 303);
			this.startCountText.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.startCountText.Name = "startCountText";
			this.startCountText.Size = new System.Drawing.Size(121, 19);
			this.startCountText.TabIndex = 65;
			// 
			// dconImgText
			// 
			this.dconImgText.Location = new System.Drawing.Point(97, 278);
			this.dconImgText.MaxLength = 50;
			this.dconImgText.Name = "dconImgText";
			this.dconImgText.Size = new System.Drawing.Size(370, 19);
			this.dconImgText.TabIndex = 50;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 281);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 12);
			this.label4.TabIndex = 29;
			this.label4.Text = "dconイメージID";
			// 
			// executeCmdText
			// 
			this.executeCmdText.Location = new System.Drawing.Point(97, 203);
			this.executeCmdText.MaxLength = 255;
			this.executeCmdText.Name = "executeCmdText";
			this.executeCmdText.Size = new System.Drawing.Size(370, 19);
			this.executeCmdText.TabIndex = 20;
			// 
			// executeCmdTextLabel
			// 
			this.executeCmdTextLabel.AutoSize = true;
			this.executeCmdTextLabel.Location = new System.Drawing.Point(18, 206);
			this.executeCmdTextLabel.Name = "executeCmdTextLabel";
			this.executeCmdTextLabel.Size = new System.Drawing.Size(73, 12);
			this.executeCmdTextLabel.TabIndex = 30;
			this.executeCmdTextLabel.Text = "実行パラメータ";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(332, 331);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(29, 12);
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
			this.extractToolCombo.Location = new System.Drawing.Point(367, 328);
			this.extractToolCombo.Name = "extractToolCombo";
			this.extractToolCombo.Size = new System.Drawing.Size(100, 20);
			this.extractToolCombo.TabIndex = 75;
			this.extractToolCombo.Visible = false;
			// 
			// GetDLsiteInfoButton
			// 
			this.GetDLsiteInfoButton.Location = new System.Drawing.Point(154, 149);
			this.GetDLsiteInfoButton.Name = "GetDLsiteInfoButton";
			this.GetDLsiteInfoButton.Size = new System.Drawing.Size(66, 23);
			this.GetDLsiteInfoButton.TabIndex = 5;
			this.GetDLsiteInfoButton.Text = "DLsite";
			this.GetDLsiteInfoButton.UseVisualStyleBackColor = true;
			this.GetDLsiteInfoButton.Click += new System.EventHandler(this.GetDLsiteInfoButton_Click);
			// 
			// checkBox4
			// 
			this.checkBox4.AutoSize = true;
			this.checkBox4.Location = new System.Drawing.Point(211, 330);
			this.checkBox4.Name = "checkBox4";
			this.checkBox4.Size = new System.Drawing.Size(105, 16);
			this.checkBox4.TabIndex = 86;
			this.checkBox4.Text = "セーブデータ共有";
			this.checkBox4.UseVisualStyleBackColor = true;
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(447, 354);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(20, 23);
			this.button5.TabIndex = 89;
			this.button5.Text = "D";
			this.toolTip1.SetToolTip(this.button5, "ディレクトリ参照");
			this.button5.UseVisualStyleBackColor = true;
			// 
			// savePathText
			// 
			this.savePathText.Location = new System.Drawing.Point(97, 356);
			this.savePathText.MaxLength = 500;
			this.savePathText.Name = "savePathText";
			this.savePathText.Size = new System.Drawing.Size(318, 19);
			this.savePathText.TabIndex = 88;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(29, 359);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(62, 12);
			this.label11.TabIndex = 87;
			this.label11.Text = "セーブデータ";
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(421, 354);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(20, 23);
			this.button6.TabIndex = 90;
			this.button6.Text = "F";
			this.toolTip1.SetToolTip(this.button6, "ファイル参照");
			this.button6.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(95, 154);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(53, 12);
			this.label12.TabIndex = 91;
			this.label12.Text = "自動取得";
			// 
			// GetVNDBInfoButton
			// 
			this.GetVNDBInfoButton.Location = new System.Drawing.Point(226, 149);
			this.GetVNDBInfoButton.Name = "GetVNDBInfoButton";
			this.GetVNDBInfoButton.Size = new System.Drawing.Size(66, 23);
			this.GetVNDBInfoButton.TabIndex = 6;
			this.GetVNDBInfoButton.Text = "VNDB";
			this.GetVNDBInfoButton.UseVisualStyleBackColor = true;
			this.GetVNDBInfoButton.Click += new System.EventHandler(this.GetVNDBInfoButton_Click);
			// 
			// Editor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(480, 444);
			this.Controls.Add(this.GetVNDBInfoButton);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button5);
			this.Controls.Add(this.savePathText);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.checkBox4);
			this.Controls.Add(this.GetDLsiteInfoButton);
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
			this.Controls.Add(this.startCountResetButton);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.runTimeResetButton);
			this.Controls.Add(this.dconText);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.gameTitleLabel);
			this.Controls.Add(this.iconImage);
			this.Controls.Add(this.imgPathButton);
			this.Controls.Add(this.imgPathText);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.exePathButton);
			this.Controls.Add(this.exePathText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.titleText);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Editor";
			this.Text = "登録データを編集";
			((System.ComponentModel.ISupportInitialize)(this.iconImage)).EndInit();
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
		private System.Windows.Forms.Button exePathButton;
		private System.Windows.Forms.Button imgPathButton;
		private System.Windows.Forms.TextBox imgPathText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox iconImage;
		private System.Windows.Forms.Label gameTitleLabel;
		private System.Windows.Forms.Button runTimeResetButton;
		private System.Windows.Forms.TextBox dconText;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button startCountResetButton;
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
		private System.Windows.Forms.ComboBox extractToolCombo;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button GetDLsiteInfoButton;
		private System.Windows.Forms.Button button6;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.TextBox savePathText;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox checkBox4;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button GetVNDBInfoButton;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}