namespace glc_cs
{
	partial class DBUpdate
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBUpdate));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.glVersionLabel = new System.Windows.Forms.Label();
			this.currentDBVersionLabel = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.updateProgress = new System.Windows.Forms.ProgressBar();
			this.label6 = new System.Windows.Forms.Label();
			this.latestDBVersionLabel = new System.Windows.Forms.Label();
			this.updateLogText = new System.Windows.Forms.RichTextBox();
			this.updateButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.updateRequiredLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("ＭＳ 明朝", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(64, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(485, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "データベースのアップデートが必要です。";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("ＭＳ 明朝", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(451, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(135, 15);
			this.label2.TabIndex = 1;
			this.label2.Text = "Launcher Version";
			// 
			// glVersionLabel
			// 
			this.glVersionLabel.AutoSize = true;
			this.glVersionLabel.Font = new System.Drawing.Font("ＭＳ 明朝", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.glVersionLabel.Location = new System.Drawing.Point(592, 49);
			this.glVersionLabel.Name = "glVersionLabel";
			this.glVersionLabel.Size = new System.Drawing.Size(47, 15);
			this.glVersionLabel.TabIndex = 2;
			this.glVersionLabel.Text = "x.x.x";
			// 
			// currentDBVersionLabel
			// 
			this.currentDBVersionLabel.AutoSize = true;
			this.currentDBVersionLabel.Font = new System.Drawing.Font("ＭＳ 明朝", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.currentDBVersionLabel.Location = new System.Drawing.Point(343, 92);
			this.currentDBVersionLabel.Name = "currentDBVersionLabel";
			this.currentDBVersionLabel.Size = new System.Drawing.Size(46, 24);
			this.currentDBVersionLabel.TabIndex = 4;
			this.currentDBVersionLabel.Text = "old";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("ＭＳ 明朝", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label5.Location = new System.Drawing.Point(96, 92);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(202, 24);
			this.label5.TabIndex = 3;
			this.label5.Text = "Database Version";
			// 
			// updateProgress
			// 
			this.updateProgress.Location = new System.Drawing.Point(12, 240);
			this.updateProgress.Name = "updateProgress";
			this.updateProgress.Size = new System.Drawing.Size(653, 23);
			this.updateProgress.TabIndex = 5;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label6.Location = new System.Drawing.Point(411, 99);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(24, 16);
			this.label6.TabIndex = 6;
			this.label6.Text = "→";
			// 
			// latestDBVersionLabel
			// 
			this.latestDBVersionLabel.AutoSize = true;
			this.latestDBVersionLabel.Font = new System.Drawing.Font("ＭＳ 明朝", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.latestDBVersionLabel.Location = new System.Drawing.Point(459, 92);
			this.latestDBVersionLabel.Name = "latestDBVersionLabel";
			this.latestDBVersionLabel.Size = new System.Drawing.Size(46, 24);
			this.latestDBVersionLabel.TabIndex = 7;
			this.latestDBVersionLabel.Text = "new";
			// 
			// updateLogText
			// 
			this.updateLogText.Location = new System.Drawing.Point(13, 131);
			this.updateLogText.Name = "updateLogText";
			this.updateLogText.ReadOnly = true;
			this.updateLogText.Size = new System.Drawing.Size(652, 103);
			this.updateLogText.TabIndex = 8;
			this.updateLogText.Text = "";
			// 
			// updateButton
			// 
			this.updateButton.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.updateButton.Location = new System.Drawing.Point(13, 270);
			this.updateButton.Name = "updateButton";
			this.updateButton.Size = new System.Drawing.Size(492, 40);
			this.updateButton.TabIndex = 9;
			this.updateButton.Text = "アップデート";
			this.updateButton.UseVisualStyleBackColor = true;
			this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Font = new System.Drawing.Font("ＭＳ ゴシック", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.cancelButton.Location = new System.Drawing.Point(511, 270);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(154, 40);
			this.cancelButton.TabIndex = 10;
			this.cancelButton.Text = "終了";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// updateRequiredLabel
			// 
			this.updateRequiredLabel.AutoSize = true;
			this.updateRequiredLabel.Font = new System.Drawing.Font("ＭＳ 明朝", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.updateRequiredLabel.ForeColor = System.Drawing.Color.Red;
			this.updateRequiredLabel.Location = new System.Drawing.Point(555, 9);
			this.updateRequiredLabel.Name = "updateRequiredLabel";
			this.updateRequiredLabel.Size = new System.Drawing.Size(110, 24);
			this.updateRequiredLabel.TabIndex = 11;
			this.updateRequiredLabel.Text = "（必須）";
			this.updateRequiredLabel.Visible = false;
			// 
			// DBUpdate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(677, 316);
			this.Controls.Add(this.updateRequiredLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.updateButton);
			this.Controls.Add(this.updateLogText);
			this.Controls.Add(this.latestDBVersionLabel);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.updateProgress);
			this.Controls.Add(this.currentDBVersionLabel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.glVersionLabel);
			this.Controls.Add(this.label2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DBUpdate";
			this.Text = "Database Updator";
			this.Load += new System.EventHandler(this.DBUpdate_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label glVersionLabel;
		private System.Windows.Forms.Label currentDBVersionLabel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ProgressBar updateProgress;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label latestDBVersionLabel;
		private System.Windows.Forms.RichTextBox updateLogText;
		private System.Windows.Forms.Button updateButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label updateRequiredLabel;
	}
}