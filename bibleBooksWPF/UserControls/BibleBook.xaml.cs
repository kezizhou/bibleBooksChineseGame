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
		public BibleBook() {
			InitializeComponent();
		}


		// Text Content
		public static readonly DependencyProperty SetTextProperty = DependencyProperty.Register("SetText", typeof(string), typeof(BibleBook),
								new PropertyMetadata("", new PropertyChangedCallback(OnSetTextChanged)));
		public string SetText {
			get { 
				return (string)GetValue(SetTextProperty); 
			}
			set { 
				SetValue(SetTextProperty, value); 
			}
		}
		private static void OnSetTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			BibleBook bibleBook = d as BibleBook;
			bibleBook.OnSetTextChanged(e);
		}
		private void OnSetTextChanged(DependencyPropertyChangedEventArgs e) {
			lblBook.Content = e.NewValue.ToString();
		}


		// Background
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


		// Audio
		public static readonly DependencyProperty SetAudioProperty = DependencyProperty.Register("SetAudio", typeof(string), typeof(BibleBook),
						new PropertyMetadata("", new PropertyChangedCallback(OnSetAudioChanged)));
		public string SetAudio {
			get { 
				return (string)GetValue(SetTextProperty); 
			}
			set { 
				SetValue(SetTextProperty, value); 
			}
		}
		private static void OnSetAudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			BibleBook bibleBook = d as BibleBook;
			bibleBook.OnSetAudioChanged(e);
		}
		private void OnSetAudioChanged(DependencyPropertyChangedEventArgs e) {
			lblBook.Tag = e.NewValue.ToString();
		}

		private void playAudio(object sender) {
			try {
				string strRead = SetText;

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

		private void lblBook_MouseDown(object sender, MouseButtonEventArgs e) {
			if (Properties.Settings.Default.blnAudio == true && lblBook.Tag.ToString() == "AudioOn") {
				playAudio(sender);
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
