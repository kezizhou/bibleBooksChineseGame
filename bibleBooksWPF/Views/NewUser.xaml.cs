﻿using Newtonsoft.Json;
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
	}
}