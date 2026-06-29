namespace glc_cs
{
	partial class Statistics
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
			this.summaryGroup = new System.Windows.Forms.GroupBox();
			this.totalGamesDescLabel = new System.Windows.Forms.Label();
			this.totalGamesLabel = new System.Windows.Forms.Label();
			this.totalPlayTimeDescLabel = new System.Windows.Forms.Label();
			this.totalPlayTimeLabel = new System.Windows.Forms.Label();
			this.avgPlayTimeDescLabel = new System.Windows.Forms.Label();
			this.avgPlayTimeLabel = new System.Windows.Forms.Label();
			this.totalLaunchesDescLabel = new System.Windows.Forms.Label();
			this.totalLaunchesLabel = new System.Windows.Forms.Label();
			this.playTimeRankGroup = new System.Windows.Forms.GroupBox();
			this.playTimeRankList = new System.Windows.Forms.ListView();
			this.playTimeRankCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.playTimeTitleCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.playTimeTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.launchRankGroup = new System.Windows.Forms.GroupBox();
			this.launchRankList = new System.Windows.Forms.ListView();
			this.launchRankCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.launchTitleCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.launchCountCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.closeButton = new System.Windows.Forms.Button();
			this.summaryGroup.SuspendLayout();
			this.playTimeRankGroup.SuspendLayout();
			this.launchRankGroup.SuspendLayout();
			this.SuspendLayout();
			//
			// summaryGroup
			//
			this.summaryGroup.Controls.Add(this.totalGamesDescLabel);
			this.summaryGroup.Controls.Add(this.totalGamesLabel);
			this.summaryGroup.Controls.Add(this.totalPlayTimeDescLabel);
			this.summaryGroup.Controls.Add(this.totalPlayTimeLabel);
			this.summaryGroup.Controls.Add(this.avgPlayTimeDescLabel);
			this.summaryGroup.Controls.Add(this.avgPlayTimeLabel);
			this.summaryGroup.Controls.Add(this.totalLaunchesDescLabel);
			this.summaryGroup.Controls.Add(this.totalLaunchesLabel);
			this.summaryGroup.Location = new System.Drawing.Point(12, 12);
			this.summaryGroup.Name = "summaryGroup";
			this.summaryGroup.Size = new System.Drawing.Size(480, 110);
			this.summaryGroup.TabIndex = 0;
			this.summaryGroup.TabStop = false;
			this.summaryGroup.Text = "概要";
			//
			// totalGamesDescLabel
			//
			this.totalGamesDescLabel.AutoSize = true;
			this.totalGamesDescLabel.Location = new System.Drawing.Point(15, 25);
			this.totalGamesDescLabel.Name = "totalGamesDescLabel";
			this.totalGamesDescLabel.Size = new System.Drawing.Size(67, 12);
			this.totalGamesDescLabel.TabIndex = 0;
			this.totalGamesDescLabel.Text = "ゲーム数:";
			//
			// totalGamesLabel
			//
			this.totalGamesLabel.AutoSize = true;
			this.totalGamesLabel.Location = new System.Drawing.Point(150, 25);
			this.totalGamesLabel.Name = "totalGamesLabel";
			this.totalGamesLabel.Size = new System.Drawing.Size(11, 12);
			this.totalGamesLabel.TabIndex = 1;
			this.totalGamesLabel.Text = "-";
			//
			// totalPlayTimeDescLabel
			//
			this.totalPlayTimeDescLabel.AutoSize = true;
			this.totalPlayTimeDescLabel.Location = new System.Drawing.Point(15, 45);
			this.totalPlayTimeDescLabel.Name = "totalPlayTimeDescLabel";
			this.totalPlayTimeDescLabel.Size = new System.Drawing.Size(89, 12);
			this.totalPlayTimeDescLabel.TabIndex = 2;
			this.totalPlayTimeDescLabel.Text = "総プレイ時間:";
			//
			// totalPlayTimeLabel
			//
			this.totalPlayTimeLabel.AutoSize = true;
			this.totalPlayTimeLabel.Location = new System.Drawing.Point(150, 45);
			this.totalPlayTimeLabel.Name = "totalPlayTimeLabel";
			this.totalPlayTimeLabel.Size = new System.Drawing.Size(11, 12);
			this.totalPlayTimeLabel.TabIndex = 3;
			this.totalPlayTimeLabel.Text = "-";
			//
			// avgPlayTimeDescLabel
			//
			this.avgPlayTimeDescLabel.AutoSize = true;
			this.avgPlayTimeDescLabel.Location = new System.Drawing.Point(15, 65);
			this.avgPlayTimeDescLabel.Name = "avgPlayTimeDescLabel";
			this.avgPlayTimeDescLabel.Size = new System.Drawing.Size(101, 12);
			this.avgPlayTimeDescLabel.TabIndex = 4;
			this.avgPlayTimeDescLabel.Text = "平均プレイ時間:";
			//
			// avgPlayTimeLabel
			//
			this.avgPlayTimeLabel.AutoSize = true;
			this.avgPlayTimeLabel.Location = new System.Drawing.Point(150, 65);
			this.avgPlayTimeLabel.Name = "avgPlayTimeLabel";
			this.avgPlayTimeLabel.Size = new System.Drawing.Size(11, 12);
			this.avgPlayTimeLabel.TabIndex = 5;
			this.avgPlayTimeLabel.Text = "-";
			//
			// totalLaunchesDescLabel
			//
			this.totalLaunchesDescLabel.AutoSize = true;
			this.totalLaunchesDescLabel.Location = new System.Drawing.Point(15, 85);
			this.totalLaunchesDescLabel.Name = "totalLaunchesDescLabel";
			this.totalLaunchesDescLabel.Size = new System.Drawing.Size(77, 12);
			this.totalLaunchesDescLabel.TabIndex = 6;
			this.totalLaunchesDescLabel.Text = "総起動回数:";
			//
			// totalLaunchesLabel
			//
			this.totalLaunchesLabel.AutoSize = true;
			this.totalLaunchesLabel.Location = new System.Drawing.Point(150, 85);
			this.totalLaunchesLabel.Name = "totalLaunchesLabel";
			this.totalLaunchesLabel.Size = new System.Drawing.Size(11, 12);
			this.totalLaunchesLabel.TabIndex = 7;
			this.totalLaunchesLabel.Text = "-";
			//
			// playTimeRankGroup
			//
			this.playTimeRankGroup.Controls.Add(this.playTimeRankList);
			this.playTimeRankGroup.Location = new System.Drawing.Point(12, 128);
			this.playTimeRankGroup.Name = "playTimeRankGroup";
			this.playTimeRankGroup.Size = new System.Drawing.Size(480, 130);
			this.playTimeRankGroup.TabIndex = 1;
			this.playTimeRankGroup.TabStop = false;
			this.playTimeRankGroup.Text = "プレイ時間ランキング";
			//
			// playTimeRankList
			//
			this.playTimeRankList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.playTimeRankCol,
            this.playTimeTitleCol,
            this.playTimeTimeCol});
			this.playTimeRankList.FullRowSelect = true;
			this.playTimeRankList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.playTimeRankList.HideSelection = false;
			this.playTimeRankList.Location = new System.Drawing.Point(10, 18);
			this.playTimeRankList.Name = "playTimeRankList";
			this.playTimeRankList.Size = new System.Drawing.Size(460, 102);
			this.playTimeRankList.TabIndex = 0;
			this.playTimeRankList.UseCompatibleStateImageBehavior = false;
			this.playTimeRankList.View = System.Windows.Forms.View.Details;
			//
			// playTimeRankCol
			//
			this.playTimeRankCol.Text = "#";
			this.playTimeRankCol.Width = 30;
			//
			// playTimeTitleCol
			//
			this.playTimeTitleCol.Text = "タイトル";
			this.playTimeTitleCol.Width = 280;
			//
			// playTimeTimeCol
			//
			this.playTimeTimeCol.Text = "時間";
			this.playTimeTimeCol.Width = 80;
			//
			// launchRankGroup
			//
			this.launchRankGroup.Controls.Add(this.launchRankList);
			this.launchRankGroup.Location = new System.Drawing.Point(12, 264);
			this.launchRankGroup.Name = "launchRankGroup";
			this.launchRankGroup.Size = new System.Drawing.Size(480, 130);
			this.launchRankGroup.TabIndex = 2;
			this.launchRankGroup.TabStop = false;
			this.launchRankGroup.Text = "起動回数ランキング";
			//
			// launchRankList
			//
			this.launchRankList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.launchRankCol,
            this.launchTitleCol,
            this.launchCountCol});
			this.launchRankList.FullRowSelect = true;
			this.launchRankList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.launchRankList.HideSelection = false;
			this.launchRankList.Location = new System.Drawing.Point(10, 18);
			this.launchRankList.Name = "launchRankList";
			this.launchRankList.Size = new System.Drawing.Size(460, 102);
			this.launchRankList.TabIndex = 0;
			this.launchRankList.UseCompatibleStateImageBehavior = false;
			this.launchRankList.View = System.Windows.Forms.View.Details;
			//
			// launchRankCol
			//
			this.launchRankCol.Text = "#";
			this.launchRankCol.Width = 30;
			//
			// launchTitleCol
			//
			this.launchTitleCol.Text = "タイトル";
			this.launchTitleCol.Width = 280;
			//
			// launchCountCol
			//
			this.launchCountCol.Text = "回数";
			this.launchCountCol.Width = 80;
			//
			// closeButton
			//
			this.closeButton.Location = new System.Drawing.Point(407, 405);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(85, 28);
			this.closeButton.TabIndex = 3;
			this.closeButton.Text = "閉じる";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			//
			// Statistics
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(504, 441);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.launchRankGroup);
			this.Controls.Add(this.playTimeRankGroup);
			this.Controls.Add(this.summaryGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Statistics";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "統計 - GLauncher";
			this.Load += new System.EventHandler(this.Statistics_Load);
			this.summaryGroup.ResumeLayout(false);
			this.summaryGroup.PerformLayout();
			this.playTimeRankGroup.ResumeLayout(false);
			this.launchRankGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox summaryGroup;
		private System.Windows.Forms.Label totalGamesDescLabel;
		private System.Windows.Forms.Label totalGamesLabel;
		private System.Windows.Forms.Label totalPlayTimeDescLabel;
		private System.Windows.Forms.Label totalPlayTimeLabel;
		private System.Windows.Forms.Label avgPlayTimeDescLabel;
		private System.Windows.Forms.Label avgPlayTimeLabel;
		private System.Windows.Forms.Label totalLaunchesDescLabel;
		private System.Windows.Forms.Label totalLaunchesLabel;
		private System.Windows.Forms.GroupBox playTimeRankGroup;
		private System.Windows.Forms.ListView playTimeRankList;
		private System.Windows.Forms.ColumnHeader playTimeRankCol;
		private System.Windows.Forms.ColumnHeader playTimeTitleCol;
		private System.Windows.Forms.ColumnHeader playTimeTimeCol;
		private System.Windows.Forms.GroupBox launchRankGroup;
		private System.Windows.Forms.ListView launchRankList;
		private System.Windows.Forms.ColumnHeader launchRankCol;
		private System.Windows.Forms.ColumnHeader launchTitleCol;
		private System.Windows.Forms.ColumnHeader launchCountCol;
		private System.Windows.Forms.Button closeButton;
	}
}
