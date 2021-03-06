﻿using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;

namespace BibleBooksWPF {
    public static class LanguageResources {
        /// <summary>  
        /// Get application name from an element  
        /// </summary>  
        /// <param name="element"></param>  
        /// <returns></returns>  
        private static string getAppName(FrameworkElement element) {
            var elType = element.GetType().ToString();
            var elNames = elType.Split('.');
            return elNames[0];
        }

        /// <summary>  
        /// Generate a name from an element base on its class name  
        /// </summary>  
        /// <param name="element"></param>  
        /// <returns></returns>  
        private static string getElementName(FrameworkElement element) {
            var elType = element.GetType().ToString();
            var elNames = elType.Split('.');
            var elName = "";
            if (elNames.Length >= 2) elName = elNames[elNames.Length - 1];
            return elName;
        }

        /// <summary>  
        /// Get current culture info name base on previously saved setting if any,  
        /// otherwise get from OS language  
        /// </summary>  
        /// <param name="element"></param>  
        /// <returns></returns>  
        public static string GetCurrentCultureName(FrameworkElement element) {
            // If on old language setting, before version 1.2.0
            if (Properties.Settings.Default.strLanguage.ToString() == "Chinese") {
                Properties.Settings.Default.strLanguage = "zh-CN";
            } else if (Properties.Settings.Default.strLanguage.ToString() == "English") {
                Properties.Settings.Default.strLanguage = "en-US";
            }

            string cultureName = Properties.Settings.Default.strLanguage.ToString();

            return cultureName;
        }

        /// <summary>  
        /// Set language based on previously save language setting,  
        /// otherwise set to OS lanaguage  
        /// </summary>  
        /// <param name="element"></param>  
        public static void SetDefaultLanguage(FrameworkElement element) {
            SetLanguageResourceDictionary(element, GetLocXAMLFilePath(getElementName(element), GetCurrentCultureName(element)));
        }

        /// <summary>  
        /// Dynamically load a Localization ResourceDictionary from a file  
        /// </summary>  
        public static void SwitchLanguage(FrameworkElement element, string inFiveCharLang) {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(inFiveCharLang);
            SetLanguageResourceDictionary(element, GetLocXAMLFilePath(getElementName(element), inFiveCharLang));

            // Save new culture info
            Properties.Settings.Default.strLanguage = inFiveCharLang;
        }

        /// <summary>  
        /// Returns the path to the ResourceDictionary file based on the language character string.  
        /// </summary>  
        /// <param name="inFiveCharLang"></param>  
        /// <returns></returns>  
        public static string GetLocXAMLFilePath(string element, string inFiveCharLang) {
            string locXamlFile = "";
            if (element.Contains("Match") || element.Contains("Reorder")) {
                locXamlFile = "MatchReorderGames" + "." + inFiveCharLang + ".xaml";
            } else if (element.Contains("DiceBooks")) {
                locXamlFile = "DiceBooks" + "." + inFiveCharLang + ".xaml";
            } else if (element.Contains("Message")) { 
                locXamlFile = "MessageBox" + "." + inFiveCharLang + ".xaml";
            } else {
                locXamlFile = element + "." + inFiveCharLang + ".xaml";
            }
           
            string directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return Path.Combine(directory, "Languages", locXamlFile);
        }

        /// <summary>  
        /// Sets or replaces the ResourceDictionary by dynamically loading  
        /// a Localization ResourceDictionary from the file path passed in.  
        /// </summary>  
        /// <param name="inFile"></param>  
        private static void SetLanguageResourceDictionary(FrameworkElement element, String inFile) {
            if (File.Exists(inFile)) {
                // Read in ResourceDictionary File  
                var languageDictionary = new ResourceDictionary();
                languageDictionary.Source = new Uri(inFile);
                // Remove any previous Localization dictionaries loaded  
                int langDictId = -1;
                for (int i = 0; i < element.Resources.MergedDictionaries.Count; i++) {
                    var md = element.Resources.MergedDictionaries[i];
                    // Make sure your Localization ResourceDictionarys have the ResourceDictionaryName  
                    // key and that it is set to a value starting with "Loc-".  
                    if (md.Contains("ResourceDictionaryName")) {
                        if (md["ResourceDictionaryName"].ToString().StartsWith("Loc-")) {
                            langDictId = i;
                            break;
                        }
                    }
                }
                if (langDictId == -1) {
                    // Add in newly loaded Resource Dictionary  
                    element.Resources.MergedDictionaries.Add(languageDictionary);
                } else {
                    // Replace the current langage dictionary with the new one  
                    element.Resources.MergedDictionaries[langDictId] = languageDictionary;
                }
            } else {
                MessageBox.Show("'" + inFile + "' not found.");
            }
        }
    }
}
