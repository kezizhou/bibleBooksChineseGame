namespace BibleBooks {
	partial class MainMenu {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenu));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.hebrewScripturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.greekScripturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hebrewScripturesToolStripMenuItem,
            this.greekScripturesToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(402, 28);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// hebrewScripturesToolStripMenuItem
			// 
			this.hebrewScripturesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hebrewScripturesToolStripMenuItem.Name = "hebrewScripturesToolStripMenuItem";
			this.hebrewScripturesToolStripMenuItem.Size = new System.Drawing.Size(142, 24);
			this.hebrewScripturesToolStripMenuItem.Text = "Hebrew Scriptures";
			this.hebrewScripturesToolStripMenuItem.Click += new System.EventHandler(this.hebrewScripturesToolStripMenuItem_Click);
			// 
			// greekScripturesToolStripMenuItem
			// 
			this.greekScripturesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.greekScripturesToolStripMenuItem.Name = "greekScripturesToolStripMenuItem";
			this.greekScripturesToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
			this.greekScripturesToolStripMenuItem.Text = "Greek Scriptures";
			this.greekScripturesToolStripMenuItem.Click += new System.EventHandler(this.greekScripturesToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.panel1.Controls.Add(this.pictureBox1);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(2, 31);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(400, 186);
			this.panel1.TabIndex = 6;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
			this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBox1.Location = new System.Drawing.Point(3, 10);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(394, 129);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(15, 152);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(371, 25);
			this.label1.TabIndex = 6;
			this.label1.Text = "Use the menu strip at the top to begin";
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
			this.settingsToolStripMenuItem.Text = "Settings";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// MainMenu
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(402, 217);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainMenu";
			this.Text = "Main Menu";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem hebrewScripturesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem greekScripturesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
	}
}

