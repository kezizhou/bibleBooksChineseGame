using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibleBooks {

	public partial class GreekReorder : Form {

		private Point locationBeforeReorder;
		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		private static int intAnswered = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		public delegate void invokeDevelgate();
		List<Point> lpntLabels = new List<Point>();
		private Cursor bitmapCursor = null;

		String[] astrGreek = new String[] { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
											"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
											"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblRevelation" };


		public GreekReorder() {
			InitializeComponent();
			DoubleBuffered = true;
			WindowState = FormWindowState.Maximized;
		}

		private void GreekReorder_Load(object sender, EventArgs e) {
			Random r = new Random();

			foreach (String strLbl in astrGreek) {
				// Add mouse down method for each label
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;
				lbl.MouseDown += new MouseEventHandler(lbl_MouseDown);

				// Add each label's location to a list of points
				lpntLabels.Add(lbl.Location);
			}

			// Randomly shuffle all book label locations
			foreach (String strLbl in astrGreek) {
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;
				lbl.Location = lpntLabels[r.Next(0, lpntLabels.Count)];
				lpntLabels.Remove(lbl.Location);
			}

			// Center main panel if window size is greater than form
			if (this.Size.Width > 1369 && this.Size.Height > 704) {
				pnlWindowResize.Left = (this.Size.Width - pnlWindowResize.Width) / 2;
				pnlWindowResize.Top = (this.Size.Height - pnlWindowResize.Height) / 2;
			}
		}

		private void lbl_MouseDown(object sender, MouseEventArgs e) {
			Label lblBook = sender as Label;

			// Check audio setting
			// If on, play audio
			if (Program.blnAudio) {
				// Use a task so mouse move event will not wait on audio
				Task.Run(() => playAudio(sender));
			}

			// Copy the label in a bitmap
			Bitmap bmp = new Bitmap(lblBook.Width, lblBook.Height);
			lblBook.DrawToBitmap(bmp, new Rectangle(Point.Empty, bmp.Size));
			// Save the cursor with the image of label
			bitmapCursor = new Cursor(bmp.GetHicon());

			// Save location before control is moved in drag drop
			locationBeforeReorder = lblBook.Location;

			// Hide the label so it appears to follow the bitmap cursor
			lblBook.Hide();

			tlpOrderedBooks.DoDragDrop(lblBook, DragDropEffects.Move);

			lblBook.Show();
			
			// Reset cursor
			Cursor = Cursors.Default;

		}

		private void playAudio(object sender) {
			Label lblChineseBook = sender as Label;
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("en"));
			
			// 1, 2, 3 books pronunciation
			if (lblChineseBook.Text.Contains('1')) {
				synthesizer.Speak(lblChineseBook.Text.Replace("1", "First"));
			} else if (lblChineseBook.Text.Contains('2')) {
				synthesizer.Speak(lblChineseBook.Text.Replace("2", "Second"));
			} else if (lblChineseBook.Text.Contains('3')) {
				synthesizer.Speak(lblChineseBook.Text.Replace("3", "Third"));
			} else {
				// Normal pronunciation
				synthesizer.Speak(lblChineseBook.Text);
			}
		}

		private bool checkLabelOrder(object sender) {
			Label lblBook = sender as Label;

			int intOrderAdded = tlpOrderedBooks.GetPositionFromControl(lblBook).Row;
			int intCorrectOrder = Array.IndexOf(astrGreek, lblBook.Name);

			intAnswered += 1;

			// Incorrect
			if ( intOrderAdded != intCorrectOrder) {
				// Point penalty
				intCurrentPoints -= 1;
				Program.intTotalPoints -= 1;
				
				// Change color
				lblBook.BackColor = Color.Salmon;
				incorrectFlash(lblBook);

				return false;
			// Correct
			} else {
				// Do not allow control to be moved again
				lblBook.MouseDown -= new MouseEventHandler(lbl_MouseDown);

				intCurrentPoints += 1;
				intNumberCorrect += 1;
				Program.intTotalPoints += 1;
				return true;
			}
		}

		private void refreshPoints() {
			lblCurrentPoints.Text = intCurrentPoints.ToString();
			lblTotalPoints.Text = Program.intTotalPoints.ToString();
			lblPercentageCorrect.Text = ((double)intNumberCorrect / intAnswered * 100).ToString();
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tsSecondsElapsed += TimeSpan.FromSeconds(1);
			lblTimeElapsed.Text = tsSecondsElapsed.ToString();
		}

		private void tlpOrderedBooks_DragDrop(object sender, DragEventArgs e) {
			Boolean blnCorrect = false;
			Label lblBook = (Label)e.Data.GetData(typeof(Label));

			// Show the label so it can be added to table layout panel
			lblBook.Show();
			tlpOrderedBooks.Controls.Add(lblBook);

			// Check if it has been matched in the right order
			blnCorrect = checkLabelOrder(lblBook);
			refreshPoints();

			if (blnCorrect == false) {
				tlpOrderedBooks.Controls.Remove(lblBook);

				// Re-add control to background panel at previous position
				pnlWindowResize.Controls.Add(lblBook);
				lblBook.Location = locationBeforeReorder;
			}

			if (tlpOrderedBooks.Controls.Count == 26) {
				completedMatching();
			}

		}

		private void completedMatching() {
			timer1.Enabled = false;

			using (var frmMsgBox = new CustomMessageBox()) {
				DialogResult dlgResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
											"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intAnswered) + "\n" +
											"Time Elapsed: " + lblTimeElapsed.Text, "Congratulations!", "congrats", frmMsgBox);

				switch (dlgResponse) {
					case DialogResult.Retry:
						Form frmNewGreekReorder = new GreekReorder();
						frmNewGreekReorder.Show();
						this.Close();
						break;
					case DialogResult.OK:
						Form frmMainMenu = new MainMenu();
						frmMainMenu.Show();
						this.Close();
						break;
					case DialogResult.Abort:
						Application.Exit();
						break;
					default:
						break;
				}
			}	
		}

		private void tlpOrderedBooks_DragEnter(object sender, DragEventArgs e) {
			e.Effect = DragDropEffects.Move;
		}


		private void tlpOrderedBooks_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
			e.UseDefaultCursors = false;
			if (e.Effect == DragDropEffects.None || e.Effect == DragDropEffects.Move) {
				// Show the label being dragged as the cursor
				Cursor = bitmapCursor;
			}
		}

		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
			MainMenu frmMainMenu = new MainMenu();
			frmMainMenu.ShowDialog();
		}

		private void matchChineseToEnglishToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
			GreekScriptures frmGreekMatch = new GreekScriptures();
			frmGreekMatch.ShowDialog();
		}

		private void hebrewScripturesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
			HebrewScriptures frmHebrewMatch = new HebrewScriptures();
			frmHebrewMatch.ShowDialog();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Close();
			Settings frmSettings = new Settings();
			frmSettings.ShowDialog();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private async void incorrectFlash(Label lblIncorrectBook) {
			await Task.Delay(900);
			lblIncorrectBook.BackColor = Color.White;
		}

		private void matchChineseToEnglishHebrewToolStripMenuItem1_Click(object sender, EventArgs e) {
			this.Hide();
			HebrewScriptures frmHebrew = new HebrewScriptures();
			frmHebrew.ShowDialog();
			this.Close();
		}

		private void matchChineseToEnglishGreekToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.ShowDialog();
			this.Close();
		}
	}

}
