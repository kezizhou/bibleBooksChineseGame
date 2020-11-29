using System;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
						new PropertyMetadata("AudioOff", new PropertyChangedCallback(OnSetAudioChanged)));
		public string SetAudio {
			get { 
				return (string)GetValue(SetAudioProperty); 
			}
			set { 
				SetValue(SetAudioProperty, value); 
			}
		}
		private static void OnSetAudioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			BibleBook bibleBook = d as BibleBook;
			bibleBook.OnSetAudioChanged(e);
		}
		private void OnSetAudioChanged(DependencyPropertyChangedEventArgs e) {
			lblBook.Tag = e.NewValue.ToString();
		}

		private string GetAudioLanguage(string strGame) {
			if (strGame == "Match" && Properties.Settings.Default.strLanguage == "zh-CN") {
				return "en-US";
			} else if (strGame == "Reorder" && Properties.Settings.Default.strLanguage == "zh-CN") {
				return "zh-CN";
			} else if (strGame == "Match" && Properties.Settings.Default.strLanguage == "en-US") {
				return "zh-CN";
			} else if (strGame == "Reorder" && Properties.Settings.Default.strLanguage == "en-US") {
				return "en-US";
			}

			return null;
		}

		private void PlayAudio(object sender, string strLanguage) {
			string strRead = SetText;

			SpeechSynthesizer synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			PromptBuilder pBuilder = new PromptBuilder();

			if (strLanguage == "en-US") {
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
			} else if (strLanguage == "zh-CN") {
				pBuilder.Culture = CultureInfo.GetCultureInfo("zh-CN");
				pBuilder.AppendText(strRead);
			}

			synthesizer.SpeakAsync(pBuilder);
		}

		private void lblBook_MouseDown(object sender, MouseButtonEventArgs e) {
			try {
				if (Properties.Settings.Default.blnAudio == true && SetAudio != "AudioOff") {
					PlayAudio(sender, GetAudioLanguage(lblBook.Tag.ToString()));
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
