using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BibleBooksWPF
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public NewUser()
        {
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

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
				winNewUser.Title = "新用户";
				txbUsernameDesc.Text = "用户名";
				txbSelectProfile.Text = "选择个人图片";
				btnCancel.Content = "取消";
			}
		}
	}

	public class UsernameRule : ValidationRule {
		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {

			try {
				if (value == null) {
					return new ValidationResult(false, "Username cannot be empty");
				} else {
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return ValidationResult.ValidResult;
		}
	}
}
