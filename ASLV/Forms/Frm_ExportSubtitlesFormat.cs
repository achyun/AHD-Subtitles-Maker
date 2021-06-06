// This file is part of AHD Subtitles Maker
// A program that can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This library is free software; you can redistribute it and/or modify 
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 3 of the License, 
// or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public 
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, 
// Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AHD.SM.Formats;
using AHD.SM.ASMP;
using AHD.ID3.Frames;

namespace AHD.ID3.Editor.GUI
{
    public partial class Frm_ExportSubtitlesFormat : Form
    {
        public Frm_ExportSubtitlesFormat(SynchronisedLyricsFrame frame)
        {
            this.frame = frame;
            InitializeComponent();
            // install supported formats
            SubtitleFormats.DetectSupportedFormats(true, true);
            foreach (SubtitlesFormat format in SubtitleFormats.Formats)
            {
                listBox1.Items.Add(format);
            }
            listBox1.SelectedIndex = 0;
            // setup encodings
            infos = Encoding.GetEncodings();
            foreach (EncodingInfo inf in infos)
            {
                comboBox1.Items.Add(inf.DisplayName + " (" + inf.Name + ") [" + inf.CodePage + "]");
            }
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }
        private SynchronisedLyricsFrame frame;
        private EncodingInfo[] infos;
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            panel1.Controls.Clear();
            if (listBox1.SelectedIndex < 0)
                return;
            SubtitlesFormat format = (SubtitlesFormat)listBox1.SelectedItem;
            richTextBox1.Text = format.Description;
            if (format.HasOptions)
            {
                format.OptionsControl.Location = new Point(0, 0);
                panel1.Controls.Add(format.OptionsControl);
            }
        }
        // save
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show(AHD.ID3.Viewer.Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesFormatFirst"));
                return;
            }
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show(AHD.ID3.Viewer.Program.ResourceManager.GetString("Message_PleaseSelectWriteEncodingFirst"));
                return;
            }
            SubtitlesFormat format = (SubtitlesFormat)listBox1.SelectedItem;
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = AHD.ID3.Viewer.Program.ResourceManager.GetString("Title_ExportTo") + " " + format.Name + " " +
                AHD.ID3.Viewer.Program.ResourceManager.GetString("Title_Format");
            sav.Filter = format.GetFilter();
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Encoding enc = Encoding.GetEncoding(infos[comboBox1.SelectedIndex].CodePage);
                // create a track with subtitles
                SubtitlesTrack track = new SubtitlesTrack("");
                for (int i = 0; i < frame.Items.Count; i++)
                {
                    Subtitle sub = new Subtitle();
                    sub.StartTime = (double)frame.Items[i].Time / 1000;
                    if (i != frame.Items.Count - 1)
                    {
                        sub.EndTime = ((double)frame.Items[i + 1].Time / 1000) - 0.001;
                    }
                    else
                        sub.EndTime = sub.StartTime + 2;
                    sub.Text = SubtitleText.FromString(frame.Items[i].Text);
                    track.Subtitles.Add(sub);
                }
                format.SubtitleTrack = track;
                format.Save(sav.FileName, enc);
                MessageBox.Show(AHD.ID3.Viewer.Program.ResourceManager.GetString("Message_Done"));
                this.Close();
            }
        }
    }
}
