namespace glc_cs
{
	partial class Fanza
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(vndb));
			this.searchText = new System.Windows.Forms.TextBox();
			this.searchButton = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.SaveImageCheck = new System.Windows.Forms.CheckBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// searchText
			// 
			this.searchText.Location = new System.Drawing.Point(13, 13);
			this.searchText.Name = "searchText";
			this.searchText.Size = new System.Drawing.Size(661, 19);
			this.searchText.TabIndex = 0;
			this.toolTip1.SetToolTip(this.searchText, "検索キーワードを入力してください\r\n一般的にはゲーム名です");
			this.searchText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchText_KeyPress);
			// 
			// searchButton
			// 
			this.searchButton.Location = new System.Drawing.Point(680, 11);
			this.searchButton.Name = "searchButton";
			this.searchButton.Size = new System.Drawing.Size(81, 23);
			this.searchButton.TabIndex = 1;
			this.searchButton.Text = "検索";
			this.toolTip1.SetToolTip(this.searchButton, "検索します");
			this.searchButton.UseVisualStyleBackColor = true;
			this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(13, 39);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(823, 399);
			this.flowLayoutPanel1.TabIndex = 2;
			// 
			// SaveImageCheck
			// 
			this.SaveImageCheck.AutoSize = true;
			this.SaveImageCheck.Location = new System.Drawing.Point(767, 15);
			this.SaveImageCheck.Name = "SaveImageCheck";
			this.SaveImageCheck.Size = new System.Drawing.Size(72, 16);
			this.SaveImageCheck.TabIndex = 3;
			this.SaveImageCheck.Text = "画像取得";
			this.toolTip1.SetToolTip(this.SaveImageCheck, "画像も取得する場合はチェック");
			this.SaveImageCheck.UseVisualStyleBackColor = true;
			// 
			// vndb
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(851, 452);
			this.Controls.Add(this.SaveImageCheck);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.searchButton);
			this.Controls.Add(this.searchText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "vndb";
			this.Text = "VNDB Information Getter - Powered by VNDB.org";
			this.Load += new System.EventHandler(this.vndb_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox searchText;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.CheckBox SaveImageCheck;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}