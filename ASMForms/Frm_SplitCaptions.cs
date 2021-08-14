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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AHD.SM.ASMP;
using System.Reflection;
using System.Resources;

namespace AHD.SM.Forms
{
    public partial class Frm_SplitCaptions : Form
    {
        public Frm_SplitCaptions(SubtitlesTrack track)
        {
            InitializeComponent();
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
Assembly.GetExecutingAssembly());
        public bool IsOneTrack
        {
            get { return radioButton_split_into_one_track.Checked; }
        }
        public bool OneTrackCaptionsOnly
        {
            get { return radioButton_one_track_captions_only.Checked; }
        }
        public bool IsCaptionSurrounds
        {
            get { return radioButton_caption_is_surround.Checked; }
        }
        public string CaptionsSurroundingFirstChar { get { return textBox_first_char.Text; } }
        public string CaptionsSurroundingLastChar { get { return textBox_last_char.Text; } }

        private void radioButton_split_into_one_track_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_split_one_track_options.Enabled = radioButton_split_into_one_track.Checked;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "(";
            textBox_last_char.Text = ")";
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "[";
            textBox_last_char.Text = "]";
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "{";
            textBox_last_char.Text = "}";
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "#";
            textBox_last_char.Text = "#";
        }
        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "$";
            textBox_last_char.Text = "$";
        }
        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox_first_char.Text = "§";
            textBox_last_char.Text = "§";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_first_char.Text.Length != 1)
            {
                MessageBox.Show(resources.GetString("Message_PleaseEnterSurroundingChar"));
            }
            if (textBox_last_char.Text.Length != 1)
            {
                MessageBox.Show(resources.GetString("Message_PleaseEnterSurroundingChar"));
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
