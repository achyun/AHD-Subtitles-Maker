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
using AHD.SM.ASMP;
using AHD.SM.Formats;
using AHD.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Resources;

namespace AHD.SM.Forms
{
    public partial class Frm_Export : Form
    {
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        Project project;
        string exportedFilePath = "";
        public string ExportedFilePath
        { get { return exportedFilePath; } }
        private bool AutoMakeExportPath = true;
        private bool allowExit;
        public Frm_Export(Project project, string favorateFormat, string ExportPath)
        {
            InitializeComponent();
            allowExit = true;
            this.textBox1.Text = ExportPath;
            AutoMakeExportPath = false;
            this.project = project;
            //load subtitle tracks
            foreach (SubtitlesTrack track in project.SubtitleTracks)
                listBox_subtitlesTrack.Items.Add(track);
            listBox_subtitlesTrack.SelectedIndex = 0;
            //load formats
            foreach (SubtitlesFormat format in SubtitleFormats.EnabledFormats)
            {
                listBox_subtitlesFormat.Items.Add(format);
                if (format.Name == favorateFormat)
                    listBox_subtitlesFormat.SelectedItem = format;
            }
            if (listBox_subtitlesFormat.SelectedIndex < 0)
                listBox_subtitlesFormat.SelectedIndex = 0;
            else
                listBox_subtitlesFormat.AutoScrollOffset = new Point(0, listBox_subtitlesFormat.SelectedIndex);
        }
        public Frm_Export(Project project, string favorateFormat)
        {
            InitializeComponent();
            allowExit = true;
            AutoMakeExportPath = true;
            this.project = project;
            //load subtitle tracks
            foreach (SubtitlesTrack track in project.SubtitleTracks)
                listBox_subtitlesTrack.Items.Add(track);
            listBox_subtitlesTrack.SelectedIndex = 0;
            //load formats
            foreach (SubtitlesFormat format in SubtitleFormats.EnabledFormats)
            {
                listBox_subtitlesFormat.Items.Add(format);
                if (format.Name == favorateFormat)
                    listBox_subtitlesFormat.SelectedItem = format;
            }
            if (listBox_subtitlesFormat.SelectedIndex < 0)
                listBox_subtitlesFormat.SelectedIndex = 0;
            else
                listBox_subtitlesFormat.AutoScrollOffset = new Point(0, listBox_subtitlesFormat.SelectedIndex);
        }
        public string FavoriteFormat
        {
            get
            {
                if (listBox_subtitlesFormat.SelectedIndex > 0)
                {
                    return ((SubtitlesFormat)(listBox_subtitlesFormat.SelectedItem)).Name;
                }
                return "";
            }
        }
        private void listBox_subtitlesTrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            int i = 1;
            foreach (Subtitle sub in ((SubtitlesTrack)listBox_subtitlesTrack.SelectedItem).Subtitles)
            {
                ListViewItem_Subtitle item = new ListViewItem_Subtitle();
                item.Number = i;
                item.Subtitle = sub;
                item.ChangeTimingView(SubtitleTimingMode.Timespan_Milli);
                listView1.Items.Add(item);
                i++;
            }
        }
        private void listBox_subtitlesFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).Description;
            panel_formatOptions.Controls.Clear();
            if (((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).HasOptions)
            {
                UserControl control = ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).OptionsControl;
                control.Location = new Point(0, 0);
                panel_formatOptions.Controls.Add(control);
            }
            // try to create a path to make it easier for the user
            try
            {
                if (AutoMakeExportPath)
                {
                    if (File.Exists(project.MediaPath))
                        textBox1.Text = Path.GetDirectoryName(project.MediaPath) + @"\" + Path.GetFileNameWithoutExtension(project.MediaPath) +
                            ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).Extensions[0];
                    else
                        textBox1.Text = project.Name + "." + ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).Extensions[0];
                }
            }
            catch { }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_back.Enabled = tabControl1.SelectedIndex > 0;
            button_next.Enabled = tabControl1.SelectedIndex < tabControl1.TabCount - 1;
        }
        private void button_next_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex++;
        }
        private void button_back_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex--;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            allowExit = true;
            this.Close();
        }
        private void Frm_Export_FormClosed(object sender, FormClosedEventArgs e)
        {
            encodingsTool1.SaveSettings();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog op = new SaveFileDialog();
            op.Title = resources.GetString("Title_ExportTo")
                + " " + ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).Name + " " +
                resources.GetString("Title_SubtitlesFormat");
            op.Filter = ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem).GetFilter();
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                textBox1.Text = op.FileName;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (encodingsTool1.SelectedEncoding == null)
            {
                MessageBox.Show(resources.GetString("Message_PleaseSelectEncodingFirst")); return;
            }
            Encoding EN = encodingsTool1.SelectedEncoding;
            //Set subtitle tracks
            SubtitlesFormat selectedFormat = ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem);
            selectedFormat.SubtitleTracks = project.SubtitleTracks;
            selectedFormat.SubtitleTrack = ((SubtitlesTrack)listBox_subtitlesTrack.SelectedItem);
            //Save !!
            selectedFormat.Save(Path.GetTempPath() + "\\test.tst", EN);
            //preview
            richTextBox2.Lines = File.ReadAllLines(Path.GetTempPath() + "\\test.tst", EN);
        }

        private void button_export_Click(object sender, EventArgs e)
        {
            allowExit = true;
            if (encodingsTool1.SelectedEncoding == null)
            {
                MessageBox.Show(resources.GetString("Message_PleaseSelectEncodingFirst")); return;
            }
            if (((SubtitlesTrack)listBox_subtitlesTrack.SelectedItem).Subtitles.Count == 0)
            {
                MessageBox.Show(resources.GetString("Message_NoSubtitleDetectedToExport")); return;
            }
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show(resources.GetString("Message_PleaseBrowseWhereToSave")); return;
            }
            // Encoding
            Encoding EN = encodingsTool1.SelectedEncoding;
            //Set subtitle tracks
            SubtitlesFormat selectedFormat = ((SubtitlesFormat)listBox_subtitlesFormat.SelectedItem);
            selectedFormat.SubtitleTracks = project.SubtitleTracks;
            selectedFormat.SubtitleTrack = ((SubtitlesTrack)listBox_subtitlesTrack.SelectedItem);
            selectedFormat.SaveStarted += new EventHandler(selectedFormat_SaveStarted);
            selectedFormat.SaveFinished += new EventHandler(selectedFormat_SaveFinished);
            selectedFormat.Progress += new EventHandler<ProgressArgs>(selectedFormat_Progress);
            //save !!
            selectedFormat.Save(textBox1.Text, EN);
            selectedFormat.Dispose();
            exportedFilePath = textBox1.Text;
            allowExit = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            MessageDialogResult result =
                MessageDialog.ShowMessage(this, resources.GetString("Message_Done"),
                resources.GetString("MessageCaption_SubtitlesFormatExport"), (MessageDialogButtons.OkNo | MessageDialogButtons.Checked),
                MessageDialogIcon.Info, true, resources.GetString("Button_Browse"), resources.GetString("Button_Close"), "",
              resources.GetString("CloseThisWizard"));
            if ((result & MessageDialogResult.Ok) == MessageDialogResult.Ok)
            {
                try
                {
                    Process.Start("explorer.exe", @"/select, " + textBox1.Text);
                }
                catch { }
            }
            if ((result & MessageDialogResult.Checked) == MessageDialogResult.Checked)
            {
                allowExit = true;
                this.Close();
            }
        }
        void selectedFormat_Progress(object sender, ProgressArgs e)
        {
            label_status.Text = e.Status;
            label_status.Refresh();
            progressBar1.Value = e.PrecentageCompleted;
            progressBar1.Refresh();
        }
        void selectedFormat_SaveFinished(object sender, EventArgs e)
        {
            label_status.Visible = progressBar1.Visible = false;
        }
        void selectedFormat_SaveStarted(object sender, EventArgs e)
        {
            label_status.Visible = progressBar1.Visible = true;
        }

        private void Frm_Export_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allowExit)
                e.Cancel = true;
        }
    }
}
