namespace glc_cs
{
	partial class AddItem
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddItem));
			this.label1 = new System.Windows.Forms.Label();
			this.titleText = new System.Windows.Forms.TextBox();
			this.exePathText = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.exePathButton = new System.Windows.Forms.Button();
			this.imgPathButton = new System.Windows.Forms.Button();
			this.imgPathText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.imgPictureBox = new System.Windows.Forms.PictureBox();
			this.runTimeResetButton = new System.Windows.Forms.Button();
			this.dconText = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.startCountResetButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.localIniCheck = new System.Windows.Forms.CheckBox();
			this.onlineCheck = new System.Windows.Forms.CheckBox();
			this.offlineCheck = new System.Windows.Forms.CheckBox();
			this.rateCheck = new System.Windows.Forms.CheckBox();
			this.label8 = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.runTimeText = new System.Windows.Forms.NumericUpDown();
			this.startCountText = new System.Windows.Forms.NumericUpDown();
			this.dconImgText = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.disCloseCheck = new System.Windows.Forms.CheckBox();
			this.bioText = new System.Windows.Forms.Label();
			this.executeCmdText = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.getInfoButton = new System.Windows.Forms.Button();
			this.extractToolCombo = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(362, 170);
			this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(81, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "タイトル";
			// 
			// titleText
			// 
			this.titleText.Location = new System.Drawing.Point(462, 164);
			this.titleText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.titleText.MaxLength = 255;
			this.titleText.Name = "titleText";
			this.titleText.Size = new System.Drawing.Size(515, 31);
			this.titleText.TabIndex = 1;
			// 
			// exePathText
			// 
			this.exePathText.Location = new System.Drawing.Point(462, 220);
			this.exePathText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.exePathText.MaxLength = 500;
			this.exePathText.Name = "exePathText";
			this.exePathText.Size = new System.Drawing.Size(582, 31);
			this.exePathText.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(344, 226);
			this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "実行パス";
			// 
			// exePathButton
			// 
			this.exePathButton.Location = new System.Drawing.Point(1062, 216);
			this.exePathButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.exePathButton.Name = "exePathButton";
			this.exePathButton.Size = new System.Drawing.Size(43, 46);
			this.exePathButton.TabIndex = 6;
			this.exePathButton.Text = "..";
			this.exePathButton.UseVisualStyleBackColor = true;
			this.exePathButton.Click += new System.EventHandler(this.exePathButton_Click);
			// 
			// imgPathButton
			// 
			this.imgPathButton.Location = new System.Drawing.Point(1062, 316);
			this.imgPathButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.imgPathButton.Name = "imgPathButton";
			this.imgPathButton.Size = new System.Drawing.Size(43, 46);
			this.imgPathButton.TabIndex = 16;
			this.imgPathButton.Text = "..";
			this.imgPathButton.UseVisualStyleBackColor = true;
			this.imgPathButton.Click += new System.EventHandler(this.imgPathButton_Click);
			// 
			// imgPathText
			// 
			this.imgPathText.Location = new System.Drawing.Point(462, 320);
			this.imgPathText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.imgPathText.MaxLength = 500;
			this.imgPathText.Name = "imgPathText";
			this.imgPathText.Size = new System.Drawing.Size(582, 31);
			this.imgPathText.TabIndex = 15;
			this.imgPathText.TextChanged += new System.EventHandler(this.applyPictureBox);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(344, 326);
			this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 24);
			this.label3.TabIndex = 5;
			this.label3.Text = "画像パス";
			// 
			// imgPictureBox
			// 
			this.imgPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.imgPictureBox.InitialImage = global::glc_cs.Properties.Resources.load;
			this.imgPictureBox.Location = new System.Drawing.Point(26, 24);
			this.imgPictureBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.imgPictureBox.Name = "imgPictureBox";
			this.imgPictureBox.Size = new System.Drawing.Size(217, 200);
			this.imgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgPictureBox.TabIndex = 8;
			this.imgPictureBox.TabStop = false;
			this.imgPictureBox.WaitOnLoad = true;
			// 
			// runTimeResetButton
			// 
			this.runTimeResetButton.Location = new System.Drawing.Point(689, 482);
			this.runTimeResetButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.runTimeResetButton.Name = "runTimeResetButton";
			this.runTimeResetButton.Size = new System.Drawing.Size(43, 38);
			this.runTimeResetButton.TabIndex = 31;
			this.runTimeResetButton.Text = "0";
			this.runTimeResetButton.UseVisualStyleBackColor = true;
			this.runTimeResetButton.Click += new System.EventHandler(this.runTimeResetButton_Click);
			// 
			// dconText
			// 
			this.dconText.Location = new System.Drawing.Point(462, 376);
			this.dconText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.dconText.MaxLength = 50;
			this.dconText.Name = "dconText";
			this.dconText.Size = new System.Drawing.Size(639, 31);
			this.dconText.TabIndex = 20;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(308, 382);
			this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(131, 24);
			this.label5.TabIndex = 10;
			this.label5.Text = "dconテキスト";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(334, 488);
			this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(106, 24);
			this.label6.TabIndex = 13;
			this.label6.Text = "起動時間";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(763, 486);
			this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(106, 24);
			this.label7.TabIndex = 16;
			this.label7.Text = "起動回数";
			// 
			// startCountResetButton
			// 
			this.startCountResetButton.Location = new System.Drawing.Point(1062, 482);
			this.startCountResetButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.startCountResetButton.Name = "startCountResetButton";
			this.startCountResetButton.Size = new System.Drawing.Size(43, 38);
			this.startCountResetButton.TabIndex = 36;
			this.startCountResetButton.Text = "0";
			this.startCountResetButton.UseVisualStyleBackColor = true;
			this.startCountResetButton.Click += new System.EventHandler(this.startCountResetButton_Click);
			// 
			// AddButton
			// 
			this.AddButton.Location = new System.Drawing.Point(245, 616);
			this.AddButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(264, 66);
			this.AddButton.TabIndex = 60;
			this.AddButton.Text = "追加";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(630, 616);
			this.CloseButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(264, 66);
			this.CloseButton.TabIndex = 70;
			this.CloseButton.Text = "キャンセル";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// localIniCheck
			// 
			this.localIniCheck.AutoSize = true;
			this.localIniCheck.Enabled = false;
			this.localIniCheck.Location = new System.Drawing.Point(407, 24);
			this.localIniCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.localIniCheck.Name = "localIniCheck";
			this.localIniCheck.Size = new System.Drawing.Size(143, 28);
			this.localIniCheck.TabIndex = 22;
			this.localIniCheck.Text = "ローカルINI";
			this.localIniCheck.UseVisualStyleBackColor = true;
			// 
			// onlineCheck
			// 
			this.onlineCheck.AutoSize = true;
			this.onlineCheck.Enabled = false;
			this.onlineCheck.Location = new System.Drawing.Point(585, 24);
			this.onlineCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.onlineCheck.Name = "onlineCheck";
			this.onlineCheck.Size = new System.Drawing.Size(161, 28);
			this.onlineCheck.TabIndex = 23;
			this.onlineCheck.Text = "オンラインDB";
			this.onlineCheck.UseVisualStyleBackColor = true;
			// 
			// offlineCheck
			// 
			this.offlineCheck.AutoSize = true;
			this.offlineCheck.Enabled = false;
			this.offlineCheck.Location = new System.Drawing.Point(780, 24);
			this.offlineCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.offlineCheck.Name = "offlineCheck";
			this.offlineCheck.Size = new System.Drawing.Size(183, 28);
			this.offlineCheck.TabIndex = 24;
			this.offlineCheck.Text = "オフラインモード";
			this.offlineCheck.UseVisualStyleBackColor = true;
			// 
			// rateCheck
			// 
			this.rateCheck.AutoSize = true;
			this.rateCheck.Location = new System.Drawing.Point(462, 532);
			this.rateCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.rateCheck.Name = "rateCheck";
			this.rateCheck.Size = new System.Drawing.Size(209, 28);
			this.rateCheck.TabIndex = 40;
			this.rateCheck.Text = "成人向け（R-18）";
			this.rateCheck.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(384, 534);
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
			this.runTimeText.Location = new System.Drawing.Point(462, 482);
			this.runTimeText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.runTimeText.Maximum = new decimal(new int[] {
            35791394,
            0,
            0,
            0});
			this.runTimeText.Name = "runTimeText";
			this.runTimeText.Size = new System.Drawing.Size(215, 31);
			this.runTimeText.TabIndex = 30;
			// 
			// startCountText
			// 
			this.startCountText.Location = new System.Drawing.Point(891, 482);
			this.startCountText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.startCountText.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.startCountText.Name = "startCountText";
			this.startCountText.Size = new System.Drawing.Size(158, 31);
			this.startCountText.TabIndex = 35;
			// 
			// dconImgText
			// 
			this.dconImgText.Location = new System.Drawing.Point(462, 432);
			this.dconImgText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.dconImgText.MaxLength = 50;
			this.dconImgText.Name = "dconImgText";
			this.dconImgText.Size = new System.Drawing.Size(639, 31);
			this.dconImgText.TabIndex = 25;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(282, 438);
			this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(154, 24);
			this.label4.TabIndex = 29;
			this.label4.Text = "dconイメージID";
			// 
			// disCloseCheck
			// 
			this.disCloseCheck.AutoSize = true;
			this.disCloseCheck.Location = new System.Drawing.Point(767, 706);
			this.disCloseCheck.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.disCloseCheck.Name = "disCloseCheck";
			this.disCloseCheck.Size = new System.Drawing.Size(318, 28);
			this.disCloseCheck.TabIndex = 55;
			this.disCloseCheck.Text = "追加後にウィンドウを閉じない";
			this.disCloseCheck.UseVisualStyleBackColor = true;
			// 
			// bioText
			// 
			this.bioText.AutoSize = true;
			this.bioText.Location = new System.Drawing.Point(297, 94);
			this.bioText.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.bioText.Name = "bioText";
			this.bioText.Size = new System.Drawing.Size(562, 24);
			this.bioText.TabIndex = 31;
			this.bioText.Text = "ランチャーにアイテムを追加します。D&&Dで自動補填します。";
			// 
			// executeCmdText
			// 
			this.executeCmdText.Location = new System.Drawing.Point(462, 270);
			this.executeCmdText.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.executeCmdText.MaxLength = 255;
			this.executeCmdText.Name = "executeCmdText";
			this.executeCmdText.Size = new System.Drawing.Size(639, 31);
			this.executeCmdText.TabIndex = 10;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(290, 276);
			this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(149, 24);
			this.label9.TabIndex = 32;
			this.label9.Text = "実行パラメータ";
			// 
			// getInfoButton
			// 
			this.getInfoButton.Location = new System.Drawing.Point(995, 160);
			this.getInfoButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.getInfoButton.Name = "getInfoButton";
			this.getInfoButton.Size = new System.Drawing.Size(111, 46);
			this.getInfoButton.TabIndex = 2;
			this.getInfoButton.Text = "DLsite";
			this.getInfoButton.UseVisualStyleBackColor = true;
			this.getInfoButton.Click += new System.EventHandler(this.getInfoButton_Click);
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
			this.extractToolCombo.Location = new System.Drawing.Point(843, 528);
			this.extractToolCombo.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.extractToolCombo.Name = "extractToolCombo";
			this.extractToolCombo.Size = new System.Drawing.Size(258, 32);
			this.extractToolCombo.TabIndex = 50;
			this.extractToolCombo.Visible = false;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(767, 534);
			this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(58, 24);
			this.label10.TabIndex = 58;
			this.label10.Text = "抽出";
			this.label10.Visible = false;
			// 
			// AddItem
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1124, 746);
			this.Controls.Add(this.extractToolCombo);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.getInfoButton);
			this.Controls.Add(this.executeCmdText);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.bioText);
			this.Controls.Add(this.disCloseCheck);
			this.Controls.Add(this.dconImgText);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.startCountText);
			this.Controls.Add(this.runTimeText);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.rateCheck);
			this.Controls.Add(this.offlineCheck);
			this.Controls.Add(this.onlineCheck);
			this.Controls.Add(this.localIniCheck);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.startCountResetButton);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.runTimeResetButton);
			this.Controls.Add(this.dconText);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.imgPictureBox);
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
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddItem";
			this.Text = "ゲームを追加";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AddItem_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AddItem_DragEnter);
			((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).EndInit();
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
		private System.Windows.Forms.PictureBox imgPictureBox;
		private System.Windows.Forms.Button runTimeResetButton;
		private System.Windows.Forms.TextBox dconText;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button startCountResetButton;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.CheckBox localIniCheck;
		private System.Windows.Forms.CheckBox onlineCheck;
		private System.Windows.Forms.CheckBox offlineCheck;
		private System.Windows.Forms.CheckBox rateCheck;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.NumericUpDown runTimeText;
		private System.Windows.Forms.NumericUpDown startCountText;
		private System.Windows.Forms.TextBox dconImgText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox disCloseCheck;
		private System.Windows.Forms.Label bioText;
		private System.Windows.Forms.TextBox executeCmdText;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button getInfoButton;
		private System.Windows.Forms.ComboBox extractToolCombo;
		private System.Windows.Forms.Label label10;
	}
}