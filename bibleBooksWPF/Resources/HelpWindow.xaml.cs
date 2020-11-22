using System;
using System.Windows;
using System.Windows.Controls;

namespace BibleBooksWPF.Resources {
	/// <summary>
	/// Interaction logic for HelpWindow.xaml
	/// </summary>
	public partial class HelpWindow : Window {
		int intPage = 0;
		string[] astrVideos = new string[] { "MenuBar.mp4", "MainMenu.mp4", "SelectUser.mp4", "Matching.mp4", "Reorder.mp4", "Statistics.mp4", "Settings.mp4" };
		string[] astrTitles = new string[] { "Menu Bar", "Main Menu", "Select User", "Matching Games", "Reorder Games", "Statistics", "Settings" };
		string[] astrDescriptions = new string[] { "Click on the menu items to navigate to games and other pages. \n You can exit the game at any time by clicking the exit tab.",
												   "Click the Change User button to switch users. You can access this help page again by clicking the blue info icon.",
												   "Click the button in the top right to switch between English and Chinese. \n Here, you can select or delete an existing user, or create a new user. You can store up to 3 users.",
												   "Click and hold a Bible book in gray, and drag it to a Bible book in blue. Release the left mouse button to match. If the match is correct, the book will stay in the position. If it is incorrect, it will turn red and return to the previous position. You can pause the game at any time by clicking the pause button in the bottom right.",
												   "Click and hold a Bible book in blue, and drag it to a cell in the grid corresponding to its order in the Bible books. Release the left mouse button to match. If the match is correct, the book will stay in the position. If it is incorrect, it will turn red and return to the previous position. You can pause the game at any time by clicking the pause button in the bottom left.",
												   "On this page, you can find your record and average scores for each game type. You will also see total points and badges earned. Hover over the badge to read its description. You can also enter codes here to earn more badges.",
												   "Turn on/off audio and change language and user settings on this page. For user settings, enter the new username or select the new profile picture and click the save button."};

		string[] astrChTitles = new string[] { "菜单", "主菜单", "选择用户", "配对游戏", "排序游戏", "成绩", "设置" };
		string[] astrChDescriptions = new string[] { "点击菜单选项就可以玩游戏，或者使用其他功能。\n 按'退出'就可以退出游戏。",
													 "想要更换用户，点击'改变用户'。 点击蓝色的信息图标获得帮助。",
													 "点选'设置'来设定游戏的语言：中文或英文。 \n 你也可以建立一个新的用户，删除一个用户，或选用一个已建立的用户。最多可以存3个用户。",
													 "点击并拖拽灰色的圣经书名，把它移到对应的蓝色圣经书名， 释放鼠标按钮。如果你的选择正确，拖动会有效；否则，灰色的书名会变红色，并回到原位。如果你需要暂停游戏，请按右下角的暂停按钮。",
													 "点击并拖拽兰色的圣经书名，把它移到右边，找到它在圣经书卷中的位置， 释放鼠标按钮。如果你的选择正确，拖动会有效；否则，兰色的书名会变红色，并回到原位。如果你需要暂停游戏，请按左下角的暂停按钮。",
													 "这个页面有你的最好成绩，以及每种游戏的平均分。你也会看到总分和你得到的奖章。把鼠标停留在奖章上，就可以知道怎么做可以得到那个奖章。",
													 "开启、关闭语音提示，改变语言设置，用户设置。要改变用户设置，输入一个新的用户名或选择一个用户的头像图片，然后按'保存'。"};

		public HelpWindow() {
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			video.LoadedBehavior = MediaState.Play;
			LoadPage();
		}

		private void btnBack_Click(object sender, RoutedEventArgs e) {
			intPage -= 1;
			LoadPage();
		}

		private void btnNext_Click(object sender, RoutedEventArgs e) {
			intPage += 1;
			LoadPage();
		}

		private void LoadPage() {
			video.Source = new Uri(@"Resources\HelpVideos\" + astrVideos[intPage], UriKind.Relative);

			if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
				txbTitle.Text = astrChTitles[intPage];
				txbDescription.Text = astrChDescriptions[intPage];
			} else if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
				txbTitle.Text = astrTitles[intPage];
				txbDescription.Text = astrDescriptions[intPage];
			}
			
			if (intPage == 0) {
				// First page
				btnBack.Visibility = Visibility.Hidden;
			} else if (intPage == astrVideos.Length - 1) {
				// Last page
				btnNext.Visibility = Visibility.Hidden;
			} else {
				btnBack.Visibility = Visibility.Visible;
				btnNext.Visibility = Visibility.Visible;
			}

			video.Play();
		}

		private void video_MediaEnded(object sender, RoutedEventArgs e) {
			video.Position = TimeSpan.Zero;
			video.Play();
		}
	}
}
