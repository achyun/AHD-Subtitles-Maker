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
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using AHD.SM.Formats;
using AHD.SM.ASMP;
using AHD.ID3;
using AHD.SM.Controls;
namespace AHD.SM
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="Args">The commandlines list</param>
        [STAThread]
        static void Main(string[] Args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            TempPath = Path.Combine(Path.GetTempPath(), "AHD Subtitles Maker");
            Directory.CreateDirectory(TempPath);
            TempPath = Path.Combine(TempPath, "Temp");
            Directory.CreateDirectory(TempPath);

            //load languages
            DetectSupportedLanguages();
            //load formats
            SubtitleFormats.DetectSupportedFormats(true, true);
            //install id3 tag frames
            FramesManager.InstallFrames();
            //Load settings
            bool settingsReset = false;
            for (int i = 0; i < Args.Length; i++)
            {
                switch (Args[i])
                {
                    case "/rst": settings.Reset(); settingsReset = true; MessageBox.Show("Settings reset done !"); break;
                }
            }
            if (!settingsReset)
                settings.Reload();
            ControlsBase.Settings.Reload();
            LoadID3V2Settings();
            //set the language before loading resources
            Language = settings.Language;
            resources = new ResourceManager("AHD.SM.LanguageResources.Resource",
              Assembly.GetExecutingAssembly());
            //load media player
            MediaPlayerManager.LoadMediaPlayer(settings.MediaPlayerCurrent);
            //show splash and load things
            Frm_StartUp startUp = new Frm_StartUp(Args);
            startUp.Show();
            //run !!
            Application.Run();
        }
        private static Properties.Settings settings = new Properties.Settings();
        private static Frm_Main frmMain;
        private static ProjectManager projectManager = new ProjectManager();
        private static ResourceManager resources;
        // This should filled at startup
        private static string[,] supportedLanguages = { { "English (United States)", "en-US", "English (United States)" } };

        public static string TempPath { get; private set; }
        /// <summary>
        /// Get the settings class
        /// </summary>
        public static Properties.Settings Settings
        { get { return settings; } }
        /// <summary>
        /// Get or set the main form
        /// </summary>
        public static Frm_Main MainForm
        { get { return frmMain; } set { frmMain = value; } }
        /// <summary>
        /// Get or set the project manager which hold the project
        /// </summary>
        public static ProjectManager ProjectManager
        { get { return projectManager; } set { projectManager = value; } }
        /// <summary>
        /// Get the version of ASM
        /// </summary>
        public static string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
        public static string StartUpPath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }
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
        public static CultureInfo CultureInfo
        { get { return Thread.CurrentThread.CurrentUICulture; } }
        /// <summary>
        /// Get or set the resources manager
        /// </summary>
        public static ResourceManager ResourceManager
        { get { return resources; } set { resources = value; } }
        public static void LoadID3V2Settings()
        {
            ID3TagSettings.DropExtendedHeader = settings.ID3V2_DropExtendedHeader;
            ID3TagSettings.ID3V2Version = settings.DefaultID3V2Version;
            ID3TagSettings.KeepPadding = settings.ID3V2_KeepPadding;
            ID3TagSettings.UseUnsynchronisation = settings.ID3V2_UseUnsynchronisation;
            ID3TagSettings.WriteFooter = settings.ID3V2_WriteFooter;
        }
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