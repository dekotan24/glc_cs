﻿namespace glc_cs
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
			this.components = new System.ComponentModel.Container();
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
			this.getDLsiteInfoButton = new System.Windows.Forms.Button();
			this.extractToolCombo = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.getVNDBInfoButton = new System.Windows.Forms.Button();
			this.label11 = new System.Windows.Forms.Label();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.imgPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.runTimeText)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.startCountText)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(167, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "タイトル";
			// 
			// titleText
			// 
			this.titleText.Location = new System.Drawing.Point(213, 82);
			this.titleText.MaxLength = 255;
			this.titleText.Name = "titleText";
			this.titleText.Size = new System.Drawing.Size(297, 19);
			this.titleText.TabIndex = 1;
			this.toolTip1.SetToolTip(this.titleText, "ゲームタイトル");
			// 
			// exePathText
			// 
			this.exePathText.Location = new System.Drawing.Point(213, 136);
			this.exePathText.MaxLength = 500;
			this.exePathText.Name = "exePathText";
			this.exePathText.Size = new System.Drawing.Size(271, 19);
			this.exePathText.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(159, 139);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "実行パス";
			// 
			// exePathButton
			// 
			this.exePathButton.Location = new System.Drawing.Point(490, 134);
			this.exePathButton.Name = "exePathButton";
			this.exePathButton.Size = new System.Drawing.Size(20, 23);
			this.exePathButton.TabIndex = 11;
			this.exePathButton.Text = "..";
			this.exePathButton.UseVisualStyleBackColor = true;
			this.exePathButton.Click += new System.EventHandler(this.ExePathButton_Click);
			// 
			// imgPathButton
			// 
			this.imgPathButton.Location = new System.Drawing.Point(490, 184);
			this.imgPathButton.Name = "imgPathButton";
			this.imgPathButton.Size = new System.Drawing.Size(20, 23);
			this.imgPathButton.TabIndex = 16;
			this.imgPathButton.Text = "..";
			this.imgPathButton.UseVisualStyleBackColor = true;
			this.imgPathButton.Click += new System.EventHandler(this.ImgPathButton_Click);
			// 
			// imgPathText
			// 
			this.imgPathText.Location = new System.Drawing.Point(213, 186);
			this.imgPathText.MaxLength = 500;
			this.imgPathText.Name = "imgPathText";
			this.imgPathText.Size = new System.Drawing.Size(271, 19);
			this.imgPathText.TabIndex = 15;
			this.imgPathText.TextChanged += new System.EventHandler(this.ApplyPictureBox);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(159, 189);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 12);
			this.label3.TabIndex = 5;
			this.label3.Text = "画像パス";
			// 
			// imgPictureBox
			// 
			this.imgPictureBox.BackColor = System.Drawing.Color.Transparent;
			this.imgPictureBox.InitialImage = global::glc_cs.Properties.Resources.load;
			this.imgPictureBox.Location = new System.Drawing.Point(12, 12);
			this.imgPictureBox.Name = "imgPictureBox";
			this.imgPictureBox.Size = new System.Drawing.Size(100, 100);
			this.imgPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgPictureBox.TabIndex = 8;
			this.imgPictureBox.TabStop = false;
			this.imgPictureBox.WaitOnLoad = true;
			// 
			// runTimeResetButton
			// 
			this.runTimeResetButton.Location = new System.Drawing.Point(318, 267);
			this.runTimeResetButton.Name = "runTimeResetButton";
			this.runTimeResetButton.Size = new System.Drawing.Size(20, 19);
			this.runTimeResetButton.TabIndex = 31;
			this.runTimeResetButton.Text = "0";
			this.runTimeResetButton.UseVisualStyleBackColor = true;
			this.runTimeResetButton.Click += new System.EventHandler(this.RunTimeResetButton_Click);
			// 
			// dconText
			// 
			this.dconText.Location = new System.Drawing.Point(213, 214);
			this.dconText.MaxLength = 50;
			this.dconText.Name = "dconText";
			this.dconText.Size = new System.Drawing.Size(297, 19);
			this.dconText.TabIndex = 20;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(142, 217);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(65, 12);
			this.label5.TabIndex = 10;
			this.label5.Text = "dconテキスト";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(154, 270);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 12);
			this.label6.TabIndex = 13;
			this.label6.Text = "起動時間";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(352, 269);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(53, 12);
			this.label7.TabIndex = 16;
			this.label7.Text = "起動回数";
			// 
			// startCountResetButton
			// 
			this.startCountResetButton.Location = new System.Drawing.Point(490, 267);
			this.startCountResetButton.Name = "startCountResetButton";
			this.startCountResetButton.Size = new System.Drawing.Size(20, 19);
			this.startCountResetButton.TabIndex = 36;
			this.startCountResetButton.Text = "0";
			this.startCountResetButton.UseVisualStyleBackColor = true;
			this.startCountResetButton.Click += new System.EventHandler(this.StartCountResetButton_Click);
			// 
			// AddButton
			// 
			this.AddButton.Location = new System.Drawing.Point(113, 334);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(122, 33);
			this.AddButton.TabIndex = 55;
			this.AddButton.Text = "追加";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.ApplyButton_Click);
			// 
			// CloseButton
			// 
			this.CloseButton.Location = new System.Drawing.Point(291, 334);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(122, 33);
			this.CloseButton.TabIndex = 60;
			this.CloseButton.Text = "キャンセル";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// localIniCheck
			// 
			this.localIniCheck.AutoSize = true;
			this.localIniCheck.Enabled = false;
			this.localIniCheck.Location = new System.Drawing.Point(188, 12);
			this.localIniCheck.Name = "localIniCheck";
			this.localIniCheck.Size = new System.Drawing.Size(76, 16);
			this.localIniCheck.TabIndex = 22;
			this.localIniCheck.Text = "ローカルINI";
			this.localIniCheck.UseVisualStyleBackColor = true;
			// 
			// onlineCheck
			// 
			this.onlineCheck.AutoSize = true;
			this.onlineCheck.Enabled = false;
			this.onlineCheck.Location = new System.Drawing.Point(270, 12);
			this.onlineCheck.Name = "onlineCheck";
			this.onlineCheck.Size = new System.Drawing.Size(84, 16);
			this.onlineCheck.TabIndex = 23;
			this.onlineCheck.Text = "オンラインDB";
			this.onlineCheck.UseVisualStyleBackColor = true;
			// 
			// offlineCheck
			// 
			this.offlineCheck.AutoSize = true;
			this.offlineCheck.Enabled = false;
			this.offlineCheck.Location = new System.Drawing.Point(360, 12);
			this.offlineCheck.Name = "offlineCheck";
			this.offlineCheck.Size = new System.Drawing.Size(95, 16);
			this.offlineCheck.TabIndex = 24;
			this.offlineCheck.Text = "オフラインモード";
			this.offlineCheck.UseVisualStyleBackColor = true;
			// 
			// rateCheck
			// 
			this.rateCheck.AutoSize = true;
			this.rateCheck.Location = new System.Drawing.Point(213, 292);
			this.rateCheck.Name = "rateCheck";
			this.rateCheck.Size = new System.Drawing.Size(108, 16);
			this.rateCheck.TabIndex = 40;
			this.rateCheck.Text = "成人向け（R-18）";
			this.rateCheck.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(177, 293);
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
			this.runTimeText.Location = new System.Drawing.Point(213, 267);
			this.runTimeText.Maximum = new decimal(new int[] {
            35791394,
            0,
            0,
            0});
			this.runTimeText.Name = "runTimeText";
			this.runTimeText.Size = new System.Drawing.Size(99, 19);
			this.runTimeText.TabIndex = 30;
			// 
			// startCountText
			// 
			this.startCountText.Location = new System.Drawing.Point(411, 267);
			this.startCountText.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this.startCountText.Name = "startCountText";
			this.startCountText.Size = new System.Drawing.Size(73, 19);
			this.startCountText.TabIndex = 35;
			// 
			// dconImgText
			// 
			this.dconImgText.Location = new System.Drawing.Point(213, 242);
			this.dconImgText.MaxLength = 50;
			this.dconImgText.Name = "dconImgText";
			this.dconImgText.Size = new System.Drawing.Size(297, 19);
			this.dconImgText.TabIndex = 25;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(130, 245);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 12);
			this.label4.TabIndex = 29;
			this.label4.Text = "dconイメージID";
			// 
			// disCloseCheck
			// 
			this.disCloseCheck.AutoSize = true;
			this.disCloseCheck.Location = new System.Drawing.Point(354, 379);
			this.disCloseCheck.Name = "disCloseCheck";
			this.disCloseCheck.Size = new System.Drawing.Size(162, 16);
			this.disCloseCheck.TabIndex = 50;
			this.disCloseCheck.Text = "追加後にウィンドウを閉じない";
			this.disCloseCheck.UseVisualStyleBackColor = true;
			// 
			// bioText
			// 
			this.bioText.AutoSize = true;
			this.bioText.Location = new System.Drawing.Point(137, 47);
			this.bioText.Name = "bioText";
			this.bioText.Size = new System.Drawing.Size(281, 12);
			this.bioText.TabIndex = 31;
			this.bioText.Text = "ランチャーにアイテムを追加します。D&&Dで自動補填します。";
			// 
			// executeCmdText
			// 
			this.executeCmdText.Location = new System.Drawing.Point(213, 161);
			this.executeCmdText.MaxLength = 255;
			this.executeCmdText.Name = "executeCmdText";
			this.executeCmdText.Size = new System.Drawing.Size(297, 19);
			this.executeCmdText.TabIndex = 15;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(134, 164);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(73, 12);
			this.label9.TabIndex = 32;
			this.label9.Text = "実行パラメータ";
			// 
			// getDLsiteInfoButton
			// 
			this.getDLsiteInfoButton.Location = new System.Drawing.Point(270, 107);
			this.getDLsiteInfoButton.Name = "getDLsiteInfoButton";
			this.getDLsiteInfoButton.Size = new System.Drawing.Size(66, 23);
			this.getDLsiteInfoButton.TabIndex = 5;
			this.getDLsiteInfoButton.Text = "DLsite";
			this.getDLsiteInfoButton.UseVisualStyleBackColor = true;
			this.getDLsiteInfoButton.Click += new System.EventHandler(this.GetDLsiteInfoButton_Click);
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
			this.extractToolCombo.Location = new System.Drawing.Point(389, 290);
			this.extractToolCombo.Name = "extractToolCombo";
			this.extractToolCombo.Size = new System.Drawing.Size(121, 20);
			this.extractToolCombo.TabIndex = 45;
			this.extractToolCombo.Visible = false;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(354, 293);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(29, 12);
			this.label10.TabIndex = 58;
			this.label10.Text = "抽出";
			this.label10.Visible = false;
			// 
			// getVNDBInfoButton
			// 
			this.getVNDBInfoButton.Location = new System.Drawing.Point(342, 107);
			this.getVNDBInfoButton.Name = "getVNDBInfoButton";
			this.getVNDBInfoButton.Size = new System.Drawing.Size(66, 23);
			this.getVNDBInfoButton.TabIndex = 6;
			this.getVNDBInfoButton.Text = "VNDB";
			this.getVNDBInfoButton.UseVisualStyleBackColor = true;
			this.getVNDBInfoButton.Click += new System.EventHandler(this.GetVNDBInfoButton_Click);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(211, 112);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 12);
			this.label11.TabIndex = 72;
			this.label11.Text = "自動取得";
			// 
			// AddItem
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 407);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.getVNDBInfoButton);
			this.Controls.Add(this.extractToolCombo);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.getDLsiteInfoButton);
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
		private System.Windows.Forms.Button getDLsiteInfoButton;
		private System.Windows.Forms.ComboBox extractToolCombo;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Button getVNDBInfoButton;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}