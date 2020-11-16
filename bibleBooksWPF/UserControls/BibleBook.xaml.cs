using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using BibleBooksWPF.Helpers;

namespace BibleBooksWPF.UserControls {
	/// <summary>
	/// Interaction logic for BibleBook.xaml
	/// </summary>
	public partial class BibleBook : UserControl {
		private bool blnDragging = false;
		private Point clickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		public BibleBook() {
			InitializeComponent();
		}

		public static readonly DependencyProperty SetTextProperty = DependencyProperty.Register("SetText", typeof(string), typeof(BibleBook),
								new PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));

		public string SetText {
			get { return (string)GetValue(SetTextProperty); }
			set { SetValue(SetTextProperty, value); }
		}

		private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			BibleBook bibleBook = d as BibleBook;
			bibleBook.OnSetTextChanged(e);
		}

		private void OnSetTextChanged(DependencyPropertyChangedEventArgs e) {
			lblBook.Content = e.NewValue.ToString();
		}

		public static readonly DependencyProperty SetBackgroundProperty = DependencyProperty.Register("SetBackground", typeof(SolidColorBrush), typeof(BibleBook),
						new PropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#9BC1FF"), new PropertyChangedCallback(OnSetBackgroundChanged)));

		public SolidColorBrush SetBackground {
			get { 
				return (SolidColorBrush)GetValue(SetBackgroundProperty); 
			}
			set { 
				SetValue(SetBackgroundProperty, value); 
			}
		}

		private static void OnSetBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			BibleBook bibleBook = d as BibleBook;
			bibleBook.OnSetBackgroundChanged(e);
		}

		private void OnSetBackgroundChanged(DependencyPropertyChangedEventArgs e) {
			lblBook.Background = (SolidColorBrush)e.NewValue;
		}

		private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
			try {
				blnDragging = true;
				Label lblActiveElement = sender as Label;
				clickPosition = e.GetPosition(this.Parent as UIElement);
				Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

				lblActiveElement.CaptureMouse();

				lblActiveElement.BringToFront();
				Cursor = Cursors.Hand;

				// Send move label message
				MoveLabelMessage.Move(mouseOnElement, new Point(lblActiveElement.ActualWidth, lblActiveElement.ActualHeight));

				// Check audio setting
				// If on, play audio
				if (Properties.Settings.Default.blnAudio == true) {
					playAudio(sender);
				}
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void Label_MouseMove(object sender, MouseEventArgs e) {
			try {
				Label lblActiveElement = sender as Label;

				if (blnDragging && lblActiveElement != null) {
					Point currentPosition = e.GetPosition(this.Parent as UIElement);
					Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

					TranslateTransform transform = lblActiveElement.RenderTransform as TranslateTransform;
					if (transform == null || dctTransform.ContainsKey(lblActiveElement.Name) == false) {
						transform = new TranslateTransform();
						lblActiveElement.RenderTransform = transform;
					}

					// Transform the distance from the current position to the position it was last in when mouse clicked
					transform.X = currentPosition.X - clickPosition.X;
					transform.Y = currentPosition.Y - clickPosition.Y;

					// Label has been moved before
					if (dctTransform.ContainsKey(lblActiveElement.Name)) {
						if (dctTransform.ContainsKey(lblActiveElement.Name)) {
							// Add on the previous transform
							transform.X += dctTransform[lblActiveElement.Name].X;
							transform.Y += dctTransform[lblActiveElement.Name].Y;
						}
					}
				}
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
			try {
				blnDragging = false;
				Label lblActiveElement = sender as Label;
				TranslateTransform transform = lblActiveElement.RenderTransform as TranslateTransform;

				// Check if it has been matched to a book
				CheckLabelsTouchingMessage.Check(lblActiveElement);

				lblActiveElement.ReleaseMouseCapture();
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				Cursor = Cursors.Arrow;
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void playAudio(object sender) {
			try {
				Label lblBook = sender as Label;
				string strRead = lblBook.Content.ToString();

				SpeechSynthesizer synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();
				PromptBuilder pBuilder = new PromptBuilder();

				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("en-US");

					if (strRead.Contains("1")) {
						strRead = strRead.Replace("1", "First");
						pBuilder.AppendText(strRead);
					} else if (strRead.Contains("2")) {
						strRead = strRead.Replace("2", "Second");
						pBuilder.AppendText(strRead);
					} else if (strRead.Contains("3")) {
						strRead = strRead.Replace("3", "Third");
						pBuilder.AppendText(strRead);
					} else if (strRead == "Philemon") {
						pBuilder.AppendTextWithPronunciation("Philemon", "faɪlimən");
					} else {
						pBuilder.AppendText(strRead);
					}
				} else if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("zh-CN");
					pBuilder.AppendText(strRead);
				}

				synthesizer.SpeakAsync(pBuilder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}

	public class MoveLabelMessage {
		public Point pntMouseOnElement { get; set; }
		public Point pntActiveElement { get; set; }

		public static void Move(Point _pntMouseOnElement, Point _pntActiveElement) {
			var moveMessage = new MoveLabelMessage() { pntMouseOnElement = _pntMouseOnElement, pntActiveElement = _pntActiveElement };
			Messenger.Default.Send<MoveLabelMessage>(moveMessage);
		}
	}

	public class CheckLabelsTouchingMessage {
		public Label sender { get; set; }

		public static void Check(Label _sender) {
			var checkTouchingMessage = new CheckLabelsTouchingMessage() { sender = _sender };
			Messenger.Default.Send<CheckLabelsTouchingMessage>(checkTouchingMessage);
		}
	}
}
