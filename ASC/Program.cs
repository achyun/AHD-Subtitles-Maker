// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using System.IO;

namespace ASC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            DetectSupportedLanguages();


            AHD.SM.Formats.SubtitleFormats.DetectSupportedFormats(true, true);

            //Load settings
            settings.Reload();
            //set the language before loading resources
            Language = settings.Language;

            resources = new ResourceManager("ASC.LanguageResources.Resource",
              Assembly.GetExecutingAssembly());

            Application.Run(new Frm_Main(args));
        }
        static Properties.Settings settings = new Properties.Settings();
        static ResourceManager resources;
        static string[,] supportedLanguages = { { "English (United States)", "en-US", "English" } };

        public static string[,] SupportedLanguages
        { get { return supportedLanguages; } }
        public static string Language
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.NativeName;
            }
            set
            {
                for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
                {
                    if (Program.SupportedLanguages[i, 0] == value)
                    {
                        Thread.CurrentThread.CurrentUICulture = new CultureInfo(Program.SupportedLanguages[i, 1]);
                        break;
                    }
                }
            }
        }
        public static System.Globalization.CultureInfo CultureInfo
        { get { return Thread.CurrentThread.CurrentUICulture; } }
        /// <summary>
        /// Get or set the resources manager
        /// </summary>
        public static ResourceManager ResourceManager
        { get { return resources; } set { resources = value; } }

        /// <summary>
        /// Get the settings class
        /// </summary>
        public static Properties.Settings Settings
        { get { return settings; } }

        private static void DetectSupportedLanguages()
        {
            string[] langsFolders = Directory.GetDirectories(Application.StartupPath);
            List<string> ids = new List<string>();
            List<string> englishNames = new List<string>();
            List<string> NativeNames = new List<string>();
            foreach (string folder in langsFolders)
            {
                try
                {
                    CultureInfo inf = new CultureInfo(Path.GetFileName(folder));
                    // no errors lol add the id
                    ids.Add(Path.GetFileName(folder));
                    englishNames.Add(inf.EnglishName);
                    NativeNames.Add(inf.NativeName);
                }
                catch { }
            }
            if (ids.Count > 0)
            {
                supportedLanguages = new string[ids.Count, 3];
                for (int i = 0; i < ids.Count; i++)
                {
                    supportedLanguages[i, 0] = englishNames[i];
                    supportedLanguages[i, 1] = ids[i];
                    supportedLanguages[i, 2] = NativeNames[i];
                }
            }
        }
    }
}
