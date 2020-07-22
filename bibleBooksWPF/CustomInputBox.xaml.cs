using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for CustomInputBox.xaml
	/// </summary>
	public partial class CustomInputBox : Window {
		public CustomInputBox() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void BtnOK_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "";
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Cancel";
			this.Close();
		}
	}

	public class UsernameRule : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {

			if (value == null) {
				return new ValidationResult(false, "Username cannot be empty");
			}
			else {
				// Get the users from the file if not empty
				if (new FileInfo("users.json").Length != 0) {
					RootUser lstUsers = new RootUser();

					using (StreamReader file = File.OpenText("users.json")) {
						JsonSerializer serializer = new JsonSerializer();
						lstUsers = serializer.Deserialize(file, typeof(RootUser)) as RootUser;
					}

					// Loop through users and check if it is a duplicate
					foreach (User user in lstUsers.Users) {
						if (value.ToString().Equals(user.username)) {
							return new ValidationResult(false, "Please enter a unique username");
						}
					}
				}
			}
			return ValidationResult.ValidResult;
		}
	}
}
