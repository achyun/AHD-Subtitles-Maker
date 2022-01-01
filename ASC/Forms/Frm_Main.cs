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

namespace ASC
{
    public partial class Frm_Main : Form
    {
        SubtitlesTrack[] tracks;
        public Frm_Main(string[] args)
        {
            if (Program.Settings.ShowLanguagesForm)
            {
                Frm_Language frm = new Frm_Language();
                frm.ShowDialog();
            }
            InitializeComponent();

            if (args != null)
            {
                if (args.Length == 0) return;
                // Open format
                Frm_Import import = new Frm_Import(args[0], false);
                if (import.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    //prepare format
                    SubtitlesFormat format = import.SubtitlesFormat;
                    format.LoadStarted += new EventHandler(format_LoadStarted);
                    format.LoadFinished += new EventHandler(format_LoadFinished);
                    format.Progress += new EventHandler<ProgressArgs>(format_Progress);
                    //load
                    format.Load(args[0], import.SelectedEncoding);
                    //get track(s)
                    tracks = format.SubtitleTracks.ToArray();
                    if (!format.IsMultiTrack)
                        tracks = new SubtitlesTrack[1] { format.SubtitleTrack };
                    for (int i = 0; i < tracks.Length; i++)
                        tracks[i].Name = Program.ResourceManager.GetString("SubtitlesTrack") + " " + (i + 1).ToString();
                    //track info
                    richTextBox1.Text = "* " + Program.ResourceManager.GetString("SubtitlesFormat") + ": " + format.Name + "\n"
                        + "* " + Program.ResourceManager.GetString("FilePath") + ": " + args[0];
                    format.Dispose();
                }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker/wiki/AHD-Subtitles-Convertor");
            }
            catch { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_About"), "About AHD Subtitles Converter " + Application.ProductVersion);
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
                    format.LoadStarted += new EventHandler(format_LoadStarted);
                    format.LoadFinished += new EventHandler(format_LoadFinished);
                    format.Progress += new EventHandler<ProgressArgs>(format_Progress);
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
                    format.Dispose();
                }
            }
        }

        void format_Progress(object sender, ProgressArgs e)
        {
            label_status.Text = e.Status;
            progressBar1.Value = e.PrecentageCompleted;
        }
        void format_LoadFinished(object sender, EventArgs e)
        {
            label_status.Visible = progressBar1.Visible = false;
        }
        void format_LoadStarted(object sender, EventArgs e)
        {
            label_status.Visible = progressBar1.Visible = true;
        }
        //export
        private void button2_Click(object sender, EventArgs e)
        {
            if (tracks == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoSubtitlesFormatToConvert"),
                    Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            if (tracks.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoSubtitlesFormatToConvert"),
                    Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            Project pro = new Project("");
            pro.MediaPath = "";
            pro.SubtitleTracks.AddRange(tracks);
            Frm_Export export = new Frm_Export(pro, "");
            export.ShowDialog(this);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Frm_Language frm = new Frm_Language();
            frm.ShowDialog();
        }

        // Drag and drop
        private void Frm_Main_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }
        }
        private void Frm_Main_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
                {
                    // Do it !
                    string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                    if (files == null)
                    {
                        MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_OnlyOneFileCanBeDragged"),
                            Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                        return;
                    }
                    if (files.Length != 1)
                    {
                        MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_OnlyOneFileCanBeDragged"),
                            Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                        return;
                    }
                    // Open format
                    Frm_Import import = new Frm_Import(files[0], false);
                    if (import.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        //prepare format
                        SubtitlesFormat format = import.SubtitlesFormat;
                        format.LoadStarted += new EventHandler(format_LoadStarted);
                        format.LoadFinished += new EventHandler(format_LoadFinished);
                        format.Progress += new EventHandler<ProgressArgs>(format_Progress);
                        //load
                        format.Load(files[0], import.SelectedEncoding);
                        //get track(s)
                        tracks = format.SubtitleTracks.ToArray();
                        if (!format.IsMultiTrack)
                            tracks = new SubtitlesTrack[1] { format.SubtitleTrack };
                        for (int i = 0; i < tracks.Length; i++)
                            tracks[i].Name = Program.ResourceManager.GetString("SubtitlesTrack") + " " + (i + 1).ToString();
                        //track info
                        richTextBox1.Text = "* " + Program.ResourceManager.GetString("SubtitlesFormat") + ": " + format.Name + "\n"
                            + "* " + Program.ResourceManager.GetString("FilePath") + ": " + files[0];
                        format.Dispose();
                    }
                }
            }
        }
    }
}
