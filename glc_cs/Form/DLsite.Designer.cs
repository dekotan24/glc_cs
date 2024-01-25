namespace glc_cs
{
	partial class DLsite
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLsite));
			this.label1 = new System.Windows.Forms.Label();
			this.SearchTargetText = new System.Windows.Forms.TextBox();
			this.SearchButton = new System.Windows.Forms.Button();
			this.SearchResultText = new System.Windows.Forms.TextBox();
			this.SaveButton = new System.Windows.Forms.Button();
			this.CancelButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ImageText = new System.Windows.Forms.TextBox();
			this.SaveImageButton = new System.Windows.Forms.Button();
			this.ImageBox = new System.Windows.Forms.PictureBox();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.imageSavedCheck = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(57, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(145, 12);
			this.label1.TabIndex = 0;
			this.label1.Text = "DLsiteの作品IDもしくはURL：";
			// 
			// SearchTargetText
			// 
			this.SearchTargetText.Location = new System.Drawing.Point(208, 12);
			this.SearchTargetText.MaxLength = 100;
			this.SearchTargetText.Name = "SearchTargetText";
			this.SearchTargetText.Size = new System.Drawing.Size(294, 19);
			this.SearchTargetText.TabIndex = 1;
			this.toolTip1.SetToolTip(this.SearchTargetText, "ゲームの作品IDもしくはURL");
			this.SearchTargetText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.SearchButton_KeyPress);
			// 
			// SearchButton
			// 
			this.SearchButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.SearchButton.Location = new System.Drawing.Point(508, 10);
			this.SearchButton.Name = "SearchButton";
			this.SearchButton.Size = new System.Drawing.Size(62, 23);
			this.SearchButton.TabIndex = 2;
			this.SearchButton.Text = "検索";
			this.toolTip1.SetToolTip(this.SearchButton, "検索します");
			this.SearchButton.UseVisualStyleBackColor = true;
			this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
			// 
			// SearchResultText
			// 
			this.SearchResultText.Location = new System.Drawing.Point(208, 55);
			this.SearchResultText.MaxLength = 500;
			this.SearchResultText.Name = "SearchResultText";
			this.SearchResultText.Size = new System.Drawing.Size(294, 19);
			this.SearchResultText.TabIndex = 3;
			this.toolTip1.SetToolTip(this.SearchResultText, "タイトル名");
			// 
			// SaveButton
			// 
			this.SaveButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.SaveButton.Location = new System.Drawing.Point(183, 128);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(139, 37);
			this.SaveButton.TabIndex = 7;
			this.SaveButton.Text = "決定";
			this.toolTip1.SetToolTip(this.SaveButton, "適用します");
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// CancelButton
			// 
			this.CancelButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.CancelButton.Location = new System.Drawing.Point(363, 128);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(139, 37);
			this.CancelButton.TabIndex = 8;
			this.CancelButton.Text = "キャンセル";
			this.toolTip1.SetToolTip(this.CancelButton, "破棄します");
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(126, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(76, 12);
			this.label2.TabIndex = 6;
			this.label2.Text = "ゲームタイトル：";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(154, 83);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "イメージ：";
			// 
			// ImageText
			// 
			this.ImageText.Location = new System.Drawing.Point(208, 80);
			this.ImageText.MaxLength = 500;
			this.ImageText.Name = "ImageText";
			this.ImageText.Size = new System.Drawing.Size(294, 19);
			this.ImageText.TabIndex = 4;
			this.toolTip1.SetToolTip(this.ImageText, "リモートの画像URL");
			// 
			// SaveImageButton
			// 
			this.SaveImageButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.SaveImageButton.Location = new System.Drawing.Point(508, 78);
			this.SaveImageButton.Name = "SaveImageButton";
			this.SaveImageButton.Size = new System.Drawing.Size(62, 23);
			this.SaveImageButton.TabIndex = 5;
			this.SaveImageButton.Text = "保存";
			this.toolTip1.SetToolTip(this.SaveImageButton, "サムネイルの画像を使用する場合は保存します");
			this.SaveImageButton.UseVisualStyleBackColor = true;
			this.SaveImageButton.Click += new System.EventHandler(this.SaveImageButton_Click);
			// 
			// ImageBox
			// 
			this.ImageBox.Location = new System.Drawing.Point(12, 65);
			this.ImageBox.Name = "ImageBox";
			this.ImageBox.Size = new System.Drawing.Size(100, 100);
			this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.ImageBox.TabIndex = 10;
			this.ImageBox.TabStop = false;
			// 
			// imageSavedCheck
			// 
			this.imageSavedCheck.AutoSize = true;
			this.imageSavedCheck.Enabled = false;
			this.imageSavedCheck.Location = new System.Drawing.Point(508, 107);
			this.imageSavedCheck.Name = "imageSavedCheck";
			this.imageSavedCheck.Size = new System.Drawing.Size(55, 16);
			this.imageSavedCheck.TabIndex = 6;
			this.imageSavedCheck.Text = "Saved";
			this.imageSavedCheck.UseVisualStyleBackColor = true;
			// 
			// DLsite
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(580, 177);
			this.Controls.Add(this.imageSavedCheck);
			this.Controls.Add(this.ImageBox);
			this.Controls.Add(this.SaveImageButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.ImageText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.CancelButton);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.SearchResultText);
			this.Controls.Add(this.SearchButton);
			this.Controls.Add(this.SearchTargetText);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DLsite";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DLsite Information Getter - Powered by dlsite.com";
			((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox SearchTargetText;
		private System.Windows.Forms.Button SearchButton;
		private System.Windows.Forms.TextBox SearchResultText;
		private System.Windows.Forms.Button SaveButton;
		private new System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox ImageText;
		private System.Windows.Forms.Button SaveImageButton;
		private System.Windows.Forms.PictureBox ImageBox;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.CheckBox imageSavedCheck;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}