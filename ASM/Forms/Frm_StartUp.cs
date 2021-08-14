// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
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
using System.Windows.Forms;
using AHD.SM.ASMP;
using AHD.Forms;
using System.Diagnostics;

namespace AHD.SM
{
    public partial class Frm_StartUp : Form
    {
        string[] Args;
        public Frm_StartUp(string[] Args)
        {
            InitializeComponent();
            this.Args = Args;
            label_version.Text = Program.ResourceManager.GetString("Version") + " " + Program.Version;
        }
        private void Frm_StartUp_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            //first run
            if (!Program.Settings.FirstRun)
            {
                Frm_FirstRun frm = new Frm_FirstRun();
                frm.ShowDialog(this);
            }
            // Check for updates
            if (Program.Settings.CheckForUpdatesAutoamtically)
            {
                label_progress.Text = Program.ResourceManager.GetString("Status_CheckingForUpdates") + " ....";
                label_progress.Refresh();
                UpdateCheckResult res = UpdateChecker.CheckForUpdate(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                if (!res.IsUpToDate)
                {
                    // Show the updater !!
                    MessageDialogResult result = MessageDialog.ShowMessage(this,
                           string.Format("{0}\n{1}: {2}\n{3}: {4}\n\n{5}",
                         Program.ResourceManager.GetString("Message_ANewerVersionAvailable"),
                           Program.ResourceManager.GetString("Text_CurrentVersion"),
                           System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                           Program.ResourceManager.GetString("Text_AvailableVersion"),
                           res.LatestUpdateFound.Version,
                                Program.ResourceManager.GetString("Message_WouldYouLikeTodownloadTheVersion")),
                            Program.ResourceManager.GetString("MessageCaption_AutoUpdater"),
                            MessageDialogButtons.OkNoCancel,
                            MessageDialogIcon.Info, true,
                            Program.ResourceManager.GetString("Button_DownloadNow"),
                            Program.ResourceManager.GetString("Button_ViewDetails"),
                            Program.ResourceManager.GetString("Button_Cancel"),
                            Program.ResourceManager.GetString("CheckBox_CheckForUpdatesAutomatically"));
                    // Save the setting
                    Program.Settings.CheckForUpdatesAutoamtically =
                        (result & MessageDialogResult.Checked) == MessageDialogResult.Checked;
                    if ((result & MessageDialogResult.Ok) == MessageDialogResult.Ok)
                    {
                        // Download now !!
                        // Go to the downloads page
                        try
                        {
                            Process.Start(res.LatestUpdateFound.Link);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.ShowErrorMessage(string.Format("{0}\n\n{1}\n{2}",
                                Program.ResourceManager.GetString("Message_UnapleToDownloadTheUpdate"), ex.Message, ex.ToString()),
                                Program.ResourceManager.GetString("MessageCaption_AutoUpdater"));
                        }
                    }
                    else if ((result & MessageDialogResult.No) == MessageDialogResult.No)
                    {
                        // View details !!
                        try
                        {
                            ProcessStartInfo str = new ProcessStartInfo();
                            str.Arguments = "/checknow /lang " + "\"" + Program.Settings.Language + "\"";
                            str.FileName = Program.StartUpPath + "\\Updater.exe";
                            Process.Start(str);
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.ShowErrorMessage(string.Format("{0}\n\n{1}\n{2}",
                                "Unaple to download the update !!", ex.Message, ex.ToString()), "Auto updater");
                        }
                    }
                    else
                    {
                        // Do nothing !
                    }
                }
            }
            //Main form
            label_progress.Text = Program.ResourceManager.GetString("Status_LoadingMainWindow") + " ....";
            label_progress.Refresh();
            Program.MainForm = new Frm_Main(Args);
            Program.MainForm.Show();
            label_progress.Text = Program.ResourceManager.GetString("Status_Done");
            label_progress.Refresh();
            //Quick Start
            if (Program.Settings.ShowQuickStart)
                Program.MainForm.ShowQuickStart();
            //Tip of the day
            if (Program.Settings.ShowTipOfDay)
                Program.MainForm.ShowTipOfTheDay();

            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void Frm_StartUp_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
