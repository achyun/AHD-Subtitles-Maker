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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AHD.Forms;
using AHD.SM.ASMP;
using AHD.SM.Formats;
using AHD.SM.Forms;
namespace AST
{
    public partial class Frm_Main : Form
    {
        SubtitlesTrack[] tracks;
        public Frm_Main()
        {
            if (Program.Settings.ShowLanguagesForm)
            {
                Frm_Language frm = new Frm_Language();
                frm.ShowDialog();
            }
            InitializeComponent();
            timeEdit_base.SetTime(25, false);
            timeEdit1.SetTime(29.79, false);
        }
        //open
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ImportSubtitlesFormat");
            op.Filter = SubtitleFormats.GetFilter();
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Frm_Import import = new Frm_Import(op.FileName, false);
                if (import.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    //prepare format
                    SubtitlesFormat format = import.SubtitlesFormat;
                    //load
                    format.Load(op.FileName, import.SelectedEncoding);
                    //get track(s)
                    tracks = format.SubtitleTracks.ToArray();
                    if (!format.IsMultiTrack)
                        tracks = new SubtitlesTrack[1] { format.SubtitleTrack };
                    for (int i = 0; i < tracks.Length; i++)
                        tracks[i].Name = Program.ResourceManager.GetString("SubtitlesTrack") + " " + (i + 1).ToString();
                    //track info
                    richTextBox1.Text = "* " + Program.ResourceManager.GetString("SubtitlesFormat") + ": " + format.Name + "\n"
                        + "* " + Program.ResourceManager.GetString("FilePath") + ": " + op.FileName;
                    //other
                    if (format.HasFrameRate)
                        timeEdit_base.SetTime(format.FrameRate, false);
                    format.Dispose();
                }
            }
        }
        //save and synchronise
        private void button2_Click(object sender, EventArgs e)
        {
            if (tracks == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoSubtitlesFormatImported"),
                  Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            if (tracks.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoSubtitlesFormatImported"),
                       Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            if (timeEdit1.GetSeconds() == 0 || timeEdit_base.GetSeconds() == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheBaseTargetFramerateMustBeLargerThan0"),
                    Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            if (timeEdit1.GetSeconds() == timeEdit_base.GetSeconds())
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheBaseFramerateMustNotEqualTheTargetFramerate"),
                    Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            //synchronise
            foreach (SubtitlesTrack track in tracks)
            {
                foreach (Subtitle sub in track.Subtitles)
                {
                    sub.StartTime = (sub.StartTime * timeEdit_base.GetSeconds()) / timeEdit1.GetSeconds();
                    sub.EndTime = (sub.EndTime * timeEdit_base.GetSeconds()) / timeEdit1.GetSeconds();
                }
            }
            Project pro = new Project("");
            pro.MediaPath = "";
            pro.SubtitleTracks.AddRange(tracks);
            Frm_Export export = new Frm_Export(pro, "");
            export.ShowDialog(this);
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpFile = ".\\" + Program.CultureInfo.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpFile))
                Help.ShowHelp(this, helpFile, HelpNavigator.KeywordIndex, "Synchronisation Tool");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", HelpNavigator.KeywordIndex, "Synchronisation Tool");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_About"),
                "About AHD Synchronisation Tool " + Application.ProductVersion);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Frm_Language frm = new Frm_Language();
            frm.ShowDialog();
        }
    }
}
