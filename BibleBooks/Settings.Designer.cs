namespace BibleBooks {
	partial class Settings {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.mainMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hebrewScripturesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.grbAudioPronunciation = new System.Windows.Forms.GroupBox();
			this.radAudioOff = new System.Windows.Forms.RadioButton();
			this.radAudioOn = new System.Windows.Forms.RadioButton();
			this.menuStrip1.SuspendLayout();
			this.grbAudioPronunciation.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainMenuToolStripMenuItem,
            this.hebrewScripturesToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(472, 28);
			this.menuStrip1.TabIndex = 89;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// mainMenuToolStripMenuItem
			// 
			this.mainMenuToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.mainMenuToolStripMenuItem.Name = "mainMenuToolStripMenuItem";
			this.mainMenuToolStripMenuItem.Size = new System.Drawing.Size(95, 24);
			this.mainMenuToolStripMenuItem.Text = "Main Menu";
			this.mainMenuToolStripMenuItem.Click += new System.EventHandler(this.mainMenuToolStripMenuItem_Click);
			// 
			// hebrewScripturesToolStripMenuItem
			// 
			this.hebrewScripturesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.hebrewScripturesToolStripMenuItem.Name = "hebrewScripturesToolStripMenuItem";
			this.hebrewScripturesToolStripMenuItem.Size = new System.Drawing.Size(142, 24);
			this.hebrewScripturesToolStripMenuItem.Text = "Hebrew Scriptures";
			this.hebrewScripturesToolStripMenuItem.Click += new System.EventHandler(this.hebrewScripturesToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
			this.settingsToolStripMenuItem.Text = "Greek Scriptures";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(45, 24);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// grbAudioPronunciation
			// 
			this.grbAudioPronunciation.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.grbAudioPronunciation.Controls.Add(this.radAudioOff);
			this.grbAudioPronunciation.Controls.Add(this.radAudioOn);
			this.grbAudioPronunciation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.grbAudioPronunciation.Location = new System.Drawing.Point(12, 41);
			this.grbAudioPronunciation.Name = "grbAudioPronunciation";
			this.grbAudioPronunciation.Size = new System.Drawing.Size(212, 90);
			this.grbAudioPronunciation.TabIndex = 0;
			this.grbAudioPronunciation.TabStop = false;
			this.grbAudioPronunciation.Text = "Audio Pronunciation";
			// 
			// radAudioOff
			// 
			this.radAudioOff.AutoSize = true;
			this.radAudioOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radAudioOff.Location = new System.Drawing.Point(6, 60);
			this.radAudioOff.Name = "radAudioOff";
			this.radAudioOff.Size = new System.Drawing.Size(49, 24);
			this.radAudioOff.TabIndex = 1;
			this.radAudioOff.Text = "Off";
			this.radAudioOff.UseVisualStyleBackColor = true;
			// 
			// radAudioOn
			// 
			this.radAudioOn.AutoSize = true;
			this.radAudioOn.Checked = true;
			this.radAudioOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.radAudioOn.Location = new System.Drawing.Point(6, 30);
			this.radAudioOn.Name = "radAudioOn";
			this.radAudioOn.Size = new System.Drawing.Size(48, 24);
			this.radAudioOn.TabIndex = 0;
			this.radAudioOn.TabStop = true;
			this.radAudioOn.Text = "On";
			this.radAudioOn.UseVisualStyleBackColor = true;
			// 
			// Settings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLight;
			this.ClientSize = new System.Drawing.Size(472, 375);
			this.Controls.Add(this.grbAudioPronunciation);
			this.Controls.Add(this.menuStrip1);
			this.Name = "Settings";
			this.Text = "Settings";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.grbAudioPronunciation.ResumeLayout(false);
			this.grbAudioPronunciation.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem mainMenuToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hebrewScripturesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.GroupBox grbAudioPronunciation;
		private System.Windows.Forms.RadioButton radAudioOff;
		private System.Windows.Forms.RadioButton radAudioOn;
	}
}