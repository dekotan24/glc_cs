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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DLsite));
			this.label1 = new System.Windows.Forms.Label();
			this.searchTargetText = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.searchResultText = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.imageText = new System.Windows.Forms.TextBox();
			this.saveImageButton = new System.Windows.Forms.Button();
			this.imageBox = new System.Windows.Forms.PictureBox();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).BeginInit();
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
			// searchTargetText
			// 
			this.searchTargetText.Location = new System.Drawing.Point(208, 12);
			this.searchTargetText.MaxLength = 100;
			this.searchTargetText.Name = "searchTargetText";
			this.searchTargetText.Size = new System.Drawing.Size(294, 19);
			this.searchTargetText.TabIndex = 1;
			// 
			// searchButton
			// 
			this.searchButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.searchButton.Location = new System.Drawing.Point(508, 10);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(62, 23);
			this.searchButton.TabIndex = 2;
			this.searchButton.Text = "検索";
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// searchResultText
			// 
			this.searchResultText.Location = new System.Drawing.Point(208, 55);
			this.searchResultText.MaxLength = 500;
			this.searchResultText.Name = "searchResultText";
			this.searchResultText.Size = new System.Drawing.Size(294, 19);
			this.searchResultText.TabIndex = 3;
			// 
			// saveButton
			// 
			this.saveButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.saveButton.Location = new System.Drawing.Point(183, 128);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(139, 37);
			this.saveButton.TabIndex = 4;
			this.saveButton.Text = "決定";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.cancelButton.Location = new System.Drawing.Point(363, 128);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(139, 37);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "キャンセル";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
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
			// imageText
			// 
			this.imageText.Location = new System.Drawing.Point(208, 80);
			this.imageText.MaxLength = 500;
			this.imageText.Name = "imageText";
			this.imageText.Size = new System.Drawing.Size(294, 19);
			this.imageText.TabIndex = 7;
			// 
			// saveImageButton
			// 
			this.saveImageButton.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.saveImageButton.Location = new System.Drawing.Point(508, 78);
			this.saveImageButton.Name = "saveImageButton";
			this.saveImageButton.Size = new System.Drawing.Size(62, 23);
			this.saveImageButton.TabIndex = 9;
			this.saveImageButton.Text = "保存";
			this.saveImageButton.UseVisualStyleBackColor = true;
			this.saveImageButton.Click += new System.EventHandler(this.saveImageButton_Click);
			// 
			// imageBox
			// 
			this.imageBox.Location = new System.Drawing.Point(12, 65);
			this.imageBox.Name = "imageBox";
			this.imageBox.Size = new System.Drawing.Size(100, 100);
			this.imageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imageBox.TabIndex = 10;
			this.imageBox.TabStop = false;
			// 
			// DLsite
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(580, 177);
			this.Controls.Add(this.imageBox);
			this.Controls.Add(this.saveImageButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.imageText);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.searchResultText);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.searchTargetText);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DLsite";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DLsite Information Getter";
			((System.ComponentModel.ISupportInitialize)(this.imageBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox searchTargetText;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.TextBox searchResultText;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox imageText;
		private System.Windows.Forms.Button saveImageButton;
		private System.Windows.Forms.PictureBox imageBox;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}