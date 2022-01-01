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
using System.Windows.Forms;

namespace AHD.Forms
{
    /// <summary>
    /// Find and replace form
    /// </summary>
    public partial class Frm_FindAndReplace : Form
    {
        /// <summary>
        /// Find and replace form
        /// </summary>
        /// <param name="findAndReplace">Is find and replace ? if so, select the find and replace tab otherwise select the find only tab</param>
        /// <param name="wordToFind">The word to search for, can't be null</param>
        public Frm_FindAndReplace(bool findAndReplace, string wordToFind)
        {
            InitializeComponent();
            tabControl1.SelectedIndex = findAndReplace ? 1 : 0;
            textBox_findandReplace_findWhat.Text = textBox_Find_FindWhat.Text = wordToFind;
        }
        /// <summary>
        /// Get or set the find word (word to search for)
        /// </summary>
        public string FindWord
        { get { return textBox_Find_FindWhat.Text; } set { textBox_findandReplace_findWhat.Text = textBox_Find_FindWhat.Text = value; } }
        /// <summary>
        /// Get or set if the find and replace tab is active
        /// </summary>
        public bool IsFindAndReplace
        { get { return tabControl1.SelectedIndex == 1; } set { tabControl1.SelectedIndex = value ? 1 : 0; } }
        /// <summary>
        /// Rise the find next event
        /// </summary>
        public void FindNext()
        {
            if (tabControl1.SelectedIndex == 0)
                button_find_find_Click(this, null);
            else
                button_findAndreplace_find_Click(this, null);
        }
        /// <summary>
        /// Rised up when the user asks to find or replace, the FindAndReplaceArgs will determine the options
        /// </summary>
        public event EventHandler<FindAndReplaceArgs> FindNextRequest;

        private void button_find_find_Click(object sender, EventArgs e)
        {
            if (textBox_Find_FindWhat.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_Find_FindWhat.Text, "", checkBox1.Checked, checkBox2.Checked, false));
        }
        private void button_findAndreplace_find_Click(object sender, EventArgs e)
        {
            if (textBox_findandReplace_findWhat.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_findandReplace_findWhat.Text, "", checkBox4.Checked, checkBox3.Checked, false));
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox_findandReplace_findWhat.Text.Length == 0)
                return;
            if (textBox3.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_findandReplace_findWhat.Text, textBox3.Text, checkBox4.Checked, checkBox3.Checked, false));
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox_findandReplace_findWhat.Text.Length == 0)
                return;
            if (textBox3.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_findandReplace_findWhat.Text, textBox3.Text, checkBox4.Checked, checkBox3.Checked, true));
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_Find_FindWhat.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_Find_FindWhat.Text, "", checkBox1.Checked, checkBox2.Checked, true));
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox_findandReplace_findWhat.Text.Length == 0)
                return;
            if (FindNextRequest != null)
                FindNextRequest(this, new FindAndReplaceArgs(textBox_findandReplace_findWhat.Text, "", checkBox4.Checked, checkBox3.Checked, true));
        }
        private void Frm_FindAndReplace_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3 || e.KeyCode == Keys.Return)
                FindNext();
        }
    }
}
