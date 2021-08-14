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
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;
using AHD.SM.Formats;

namespace AHD.SM.Forms
{
    public partial class Frm_NewProject : Form
    {
        public Frm_NewProject()
        {
            InitializeComponent();
        }
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
          Assembly.GetExecutingAssembly());
        public string ProjectName
        { get { return textBox_name.Text; } }
        public string ProjectMediaFile
        { get { return textBox_media.Text; } }
        public string ProjectImport
        { get { return textBox_import.Text; } }
        public string ProjectPreparedText
        {
            get { return textBox_preparedText.Text; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        //change media
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = resources.GetString("Title_ChangeMedia");
            op.Filter = Filters.Media;
            op.FileName = textBox_media.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_media.Text = op.FileName;
                if (textBox_name.Text.Length == 0)
                    textBox_name.Text = System.IO.Path.GetFileNameWithoutExtension(op.FileName);
            }
        }
        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox_name.Text.Length > 0;
        }
        //import subtitle format
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = resources.GetString("Title_ImportSubtitlesFormat");
            op.Filter = SubtitleFormats.GetFilter();
            op.FileName = textBox_import.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_import.Text = op.FileName;
            }
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = resources.GetString("Title_OpenPreparedTextFile");
            op.Filter = "Text (*.txt)|*.txt";
            op.FileName = textBox_preparedText.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_preparedText.Text = op.FileName;
            }
        }
    }
}
