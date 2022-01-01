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
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using AHD.SM.ASMP;
using AHD.SM.ASMP.SpellCheck;
using AHD.Forms;

namespace AHD.SM.Forms
{
    public partial class Frm_SpellCheck : Form
    {
        /// <summary>
        /// The spell check window
        /// </summary>
        /// <param name="track">The subtitles track to check subtitles for</param>
        /// <param name="selectedSubtitleIndex">The selected subtitle to select on initialize, default is 0</param>
        public Frm_SpellCheck(SubtitlesTrack track, int selectedSubtitleIndex, bool checkInstantly)
        {
            InitializeComponent();
            this.track = track;

            temp = track.Clone();//To allow cancel via keeping the original track untouched until user ok.
            //load subtitles
            if (temp.Subtitles.Count == 0)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_TheresNoSubtitleToLoad"),
                    resources.GetString("MessageCaption_SpellCheck"));
                Close();
                return;
            }
            int i = 1;
            foreach (Subtitle sub in temp.Subtitles)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                item.ImageIndex = 0;
                item.SubItems.Add(sub.Text.ToString());
                if (i - 1 == selectedSubtitleIndex)
                    item.Selected = true;

                listView1.Items.Add(item);
                i++;
            }
            checker = new SpellChecker(".\\Dictionaries\\");
            RefreshDictionaries();
            //no dictionary
            if (comboBox_language.Items.Count == 0)
            {
                MessageDialogResult result = MessageDialog.ShowQuestionMessage(resources.GetString("Message_NoDictionaryToUse"),
                    resources.GetString("MessageCaption_NoDictionaryToUse"), resources.GetString("Button_Yes"),
                    resources.GetString("Button_No"));
                if (result == MessageDialogResult.Ok)
                {
                    button4_Click(this, null);
                }
                else
                {
                    Close();
                }
            }
            if (checkInstantly)
            {
                button5_Click(this, null);
            }
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource", Assembly.GetExecutingAssembly());
        private SubtitlesTrack track;
        private SubtitlesTrack temp;
        private SpellChecker checker;

        private void RefreshDictionaries()
        {
            comboBox_language.Items.Clear();
            foreach (SpellCheckerDictionary dic in checker.Dictionaries)
            {
                comboBox_language.Items.Add(dic);
            }
            if (comboBox_language.Items.Count > 0)
                comboBox_language.SelectedIndex = 0;
        }
        //download dictionaries
        private void button4_Click(object sender, EventArgs e)
        {
            Frm_DictionariesDownload frm = new Frm_DictionariesDownload();
            frm.ShowDialog(this);

            checker.RefreshDictionaries(".\\Dictionaries\\");
            RefreshDictionaries();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        //OK
        private void button1_Click(object sender, EventArgs e)
        {
            // Save subtitles !!
            track.Subtitles = new List<Subtitle>(temp.Subtitles);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        //check selected subtitle
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectASubtitleFirst"),
                    resources.GetString("MessageCaption_SpellCheck"));
                return;
            }
            if (comboBox_language.SelectedIndex < 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectLanguageFirst"),
                   resources.GetString("MessageCaption_SpellCheck"));
                return;
            }
            //show the window checker for the current subtitle
            Frm_SpellCheckSubtitle frm = new Frm_SpellCheckSubtitle(temp.Subtitles[listView1.SelectedItems[0].Index],
                (SpellCheckerDictionary)comboBox_language.SelectedItem);
            try
            {
                frm.ShowDialog(this);
            }
            catch
            {
            }
            Frm_SpellCheckSubtitle.CheckResult result = frm.ResultOfCheck;
            if ((result & Frm_SpellCheckSubtitle.CheckResult.Ok) == Frm_SpellCheckSubtitle.CheckResult.Ok)
            {
                listView1.SelectedItems[0].SubItems[1].Text = temp.Subtitles[listView1.SelectedItems[0].Index].Text.ToString();

                if ((result & Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords) ==
                    Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords)
                    listView1.SelectedItems[0].ImageIndex = 3;//warning image
                else
                    listView1.SelectedItems[0].ImageIndex = 1;//ok image
            }
            else if ((result & Frm_SpellCheckSubtitle.CheckResult.Abort) == Frm_SpellCheckSubtitle.CheckResult.Abort)
            {
                //retrive the original status of the subtitle
                temp.Subtitles[listView1.SelectedItems[0].Index].Text =
                    SubtitleTextWrapper.Clone(track.Subtitles[listView1.SelectedItems[0].Index].Text);
                listView1.SelectedItems[0].SubItems[1].Text = temp.Subtitles[listView1.SelectedItems[0].Index].Text.ToString();
                listView1.SelectedItems[0].ImageIndex = 2;//abort image
            }
            ((SpellCheckerDictionary)comboBox_language.SelectedItem).Save();
        }
        //Check all
        private void button6_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectASubtitleFirst"),
                    resources.GetString("MessageCaption_SpellCheck"));
                return;
            }
            if (comboBox_language.SelectedIndex < 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectLanguageFirst"),
                   resources.GetString("MessageCaption_SpellCheck"));
                return;
            }
            int i = 0;
            foreach (Subtitle sub in temp.Subtitles)
            {
                //show the window checker for the current subtitle
                Frm_SpellCheckSubtitle frm = new Frm_SpellCheckSubtitle(sub, (SpellCheckerDictionary)comboBox_language.SelectedItem);
                try
                {
                    frm.ShowDialog(this);
                }
                catch
                {
                }
                Frm_SpellCheckSubtitle.CheckResult result = frm.ResultOfCheck;
                if ((result & Frm_SpellCheckSubtitle.CheckResult.Ok) == Frm_SpellCheckSubtitle.CheckResult.Ok)
                {
                    listView1.Items[i].SubItems[1].Text = temp.Subtitles[i].Text.ToString();
                    if ((result & Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords) ==
                        Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords)
                        listView1.Items[i].ImageIndex = 3;//warning image
                    else
                        listView1.Items[i].ImageIndex = 1;//ok image
                }
                else if ((result & Frm_SpellCheckSubtitle.CheckResult.Abort) == Frm_SpellCheckSubtitle.CheckResult.Abort)
                {
                    //retrive the original status of the subtitle
                    temp.Subtitles[i].Text = SubtitleTextWrapper.Clone(track.Subtitles[i].Text);
                    listView1.Items[i].SubItems[1].Text = temp.Subtitles[i].Text.ToString();
                    listView1.Items[i].ImageIndex = 2;//abort image
                    break;
                }
                listView1.Items[i].EnsureVisible();
                listView1.Refresh();
                i++;
            }
            ((SpellCheckerDictionary)comboBox_language.SelectedItem).Save();
        }
        private void Frm_SpellCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (SpellCheckerDictionary dic in checker.Dictionaries)
                dic.Save();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string helpPath = ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpPath))
                Help.ShowHelp(this, helpPath, HelpNavigator.KeywordIndex, "Spell check");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", HelpNavigator.KeywordIndex, "Spell check");
        }
    }
}
