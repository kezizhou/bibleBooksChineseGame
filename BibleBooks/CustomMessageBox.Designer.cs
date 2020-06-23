namespace BibleBooks {
	partial class CustomMessageBox {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnMain = new System.Windows.Forms.Button();
			this.btnRetry = new System.Windows.Forms.Button();
			this.lblMessageText = new System.Windows.Forms.Label();
			this.picIcon = new System.Windows.Forms.PictureBox();
			this.btnExit = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// btnMain
			// 
			this.btnMain.Location = new System.Drawing.Point(158, 135);
			this.btnMain.Name = "btnMain";
			this.btnMain.Size = new System.Drawing.Size(103, 40);
			this.btnMain.TabIndex = 0;
			this.btnMain.Text = "Back to Main";
			this.btnMain.UseVisualStyleBackColor = true;
			this.btnMain.Click += new System.EventHandler(this.btnMain_Click);
			// 
			// btnRetry
			// 
			this.btnRetry.Location = new System.Drawing.Point(38, 135);
			this.btnRetry.Name = "btnRetry";
			this.btnRetry.Size = new System.Drawing.Size(103, 40);
			this.btnRetry.TabIndex = 1;
			this.btnRetry.Text = "Retry";
			this.btnRetry.UseVisualStyleBackColor = true;
			this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
			// 
			// lblMessageText
			// 
			this.lblMessageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessageText.Location = new System.Drawing.Point(136, 9);
			this.lblMessageText.Name = "lblMessageText";
			this.lblMessageText.Size = new System.Drawing.Size(271, 123);
			this.lblMessageText.TabIndex = 2;
			this.lblMessageText.Text = "Message text goes here";
			this.lblMessageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// picIcon
			// 
			this.picIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.picIcon.Location = new System.Drawing.Point(12, 24);
			this.picIcon.Name = "picIcon";
			this.picIcon.Size = new System.Drawing.Size(100, 83);
			this.picIcon.TabIndex = 3;
			this.picIcon.TabStop = false;
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(278, 135);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(103, 40);
			this.btnExit.TabIndex = 4;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
			// 
			// CustomMessageBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(419, 187);
			this.Controls.Add(this.btnExit);
			this.Controls.Add(this.picIcon);
			this.Controls.Add(this.lblMessageText);
			this.Controls.Add(this.btnRetry);
			this.Controls.Add(this.btnMain);
			this.Name = "CustomMessageBox";
			this.Text = "Custom Message Caption";
			((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnMain;
		private System.Windows.Forms.Button btnRetry;
		private System.Windows.Forms.Label lblMessageText;
		private System.Windows.Forms.PictureBox picIcon;
		private System.Windows.Forms.Button btnExit;
	}
}