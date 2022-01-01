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
using System.IO;

namespace AHD.SM.Forms
{
    public partial class Frm_QuickStart : Form
    {
        public Frm_QuickStart(System.Collections.Specialized.StringCollection RecentProjects, bool ShowAtStartUp)
        {
            InitializeComponent();
            _RecentProjects = RecentProjects;

            //Add recents
            foreach (string rec in RecentProjects)
                comboBox1.Items.Add(Path.GetFileName(rec));
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            //Show at start up
            checkBox1.Checked = ShowAtStartUp;
        }
        QuickStartResult _result = QuickStartResult.Close;
        System.Collections.Specialized.StringCollection _RecentProjects;
        public QuickStartResult QuickStartResult
        { get { return _result; } }
        public string OpenRecentPath
        {
            get
            {
                if (comboBox1.SelectedIndex >= 0)
                    return _RecentProjects[comboBox1.SelectedIndex];
                else
                    return "";
            }
        }
        public bool ShowAtStartUp
        { get { return checkBox1.Checked; } }

        private void button1_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.NewProject;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.OpenProject;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex >= 0)
            {
                _result = QuickStartResult.OpenRecentProject;
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.GettingStarted;
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.Close;
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.Import;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.SearchOS;
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            _result = QuickStartResult.RipMKV;
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try { System.Diagnostics.Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker"); }
            catch { }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public enum QuickStartResult
    { Close, NewProject, OpenProject, Import, OpenRecentProject, GettingStarted, SearchOS, RipMKV }
}
