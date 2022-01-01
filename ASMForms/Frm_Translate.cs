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
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;
using AHD.Forms;

namespace AHD.SM.Forms
{
    public partial class Frm_Translate : Form
    {
        public Frm_Translate(SubtitlesTrack subtitlesTrack)
        {
            InitializeComponent();
            this.subtitlesTrack = subtitlesTrack;
            //Clone
            foreach (Subtitle sub in subtitlesTrack.Subtitles)
            {
                Subtitle newSubtitle = new Subtitle();

                newSubtitle.EndTime = sub.EndTime;
                newSubtitle.StartTime = sub.StartTime;
                //text
                SubtitleText newText = new SubtitleText();
                newText.CustomPosition = sub.Text.CustomPosition;
                newText.IsCustomPosition = sub.Text.IsCustomPosition;
                newText.Position = sub.Text.Position;
                foreach (SubtitleLine line in sub.Text.TextLines)
                {
                    SubtitleLine newline = new SubtitleLine();
                    newline.Alignement = line.Alignement;
                    foreach (SubtitleChar chr in line.Chars)
                    {
                        newline.Chars.Add(new SubtitleChar(chr.TheChar, chr.Font, chr.Color));
                    }
                    newText.TextLines.Add(newline);
                }
                newSubtitle.Text.RighttoLeft = sub.Text.RighttoLeft;
                newSubtitle.Text = newText;
                TsubtitlesTrack.Subtitles.Add(newSubtitle);
            }
            //Refresh Track
            SubtitleTimingMode timingMode = SubtitleTimingMode.Timespan_Milli;
            listView1.Items.Clear();
            listView2.Items.Clear();
            int i = 1;
            foreach (Subtitle sub in subtitlesTrack.Subtitles)
            {
                //add to original
                ListViewItem_Subtitle item = new ListViewItem_Subtitle();
                item.Number = i;
                item.Subtitle = sub;
                item.ChangeTimingView(timingMode);
                listView1.Items.Add(item);
                //add to translation
                ListViewItem_Subtitle Titem = new ListViewItem_Subtitle();
                Titem.Number = i;
                Titem.Subtitle = TsubtitlesTrack.Subtitles[i - 1];
                Titem.RefreshText(true);
                listView2.Items.Add(Titem);
                i++;
            }
        }
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
          Assembly.GetExecutingAssembly());
        SubtitlesTrack subtitlesTrack;
        SubtitlesTrack TsubtitlesTrack = new SubtitlesTrack();//translation subtitles track
        bool createNew = false;
        bool isSelecting1 = false;
        bool isSelecting2 = false;
        Thread mainThread;
        string targetLanguage = "English";
        string sourceLanguage = "English";
        delegate void SetIntValue(int value);
        delegate void EnableDisable();
        delegate string GetText(int index);
        delegate void TranslateItem(int index, string text);
        delegate void ShowErrorMessageDelegate(Exception ex);
        public bool ISGOOGLETRANSLATE = false;
        //properties
        public SubtitlesTrack TranslationSubtitlesTrack
        {
            get { return TsubtitlesTrack; }
        }
        public bool IsCreateNew
        { get { return createNew; } }

        /*void TranslateProgress()
        {
            ISGOOGLETRANSLATE = true;
            //disable
            Disablecontrols();

            Translator translator = new Translator();
            translator.TargetLanguage = targetLanguage;
            translator.SourceLanguage = sourceLanguage;

            for (int i = 0; i < subtitlesTrack.Subtitles.Count; i++)
            {
                string translation = "";
                bool translated = true;
                foreach (SubtitleLine line in subtitlesTrack.Subtitles[i].Text.TextLines)
                {
                    translator.SourceText = line.ToString();
                    try
                    {
                        translator.Translate();
                    }
                    catch { translated = false; }
                    translation += translator.Translation + "\n";
                }
                if (translated && translation.Length > 0)
                {
                    translation = translation.Substring(0, translation.Length - 1);
                    TranslateSubItem(i, translation);
                }
                int x = (i * 100) / listView1.Items.Count;
                SetProgress(x);
            }

            //finish
            Enablecontrols();
            ISGOOGLETRANSLATE = false;
        }*/
        void Disablecontrols()
        {
            if (!this.InvokeRequired)
                Disablecontrols1();
            else
                Invoke(new EnableDisable(Disablecontrols1));
        }
        void Disablecontrols1()
        {
            subtitleTextEditor_original.Enabled = subtitleTextEditor_translate.Enabled =
                button1.Enabled = button2.Enabled = toolStrip1.Enabled = false;
            progressBar1.Visible = true;
            button3.Text = resources.GetString("Button_Stop");
            progressBar1.Value = 0;
        }
        void Enablecontrols()
        {
            if (!this.InvokeRequired)
                Enablecontrols1();
            else
                Invoke(new EnableDisable(Enablecontrols1));
        }
        void Enablecontrols1()
        {
            subtitleTextEditor_original.Enabled = subtitleTextEditor_translate.Enabled =
                 button1.Enabled = button2.Enabled = toolStrip1.Enabled = true;
            progressBar1.Visible = false;
            button3.Text = resources.GetString("Button_Cancel");
        }
        void SetProgress(int value)
        {
            if (!this.InvokeRequired)
                SetProgress1(value);
            else
                Invoke(new SetIntValue(SetProgress1), new object[] { value });
        }
        void SetProgress1(int value)
        {
            progressBar1.Value = value;
        }
        void TranslateSubItem(int index, string text)
        {
            if (!this.InvokeRequired)
                TranslateSubItem1(index, text);
            else
                Invoke(new TranslateItem(TranslateSubItem1), new object[] { index, text });
        }
        void TranslateSubItem1(int index, string text)
        {
            ((ListViewItem_Subtitle)listView2.Items[index]).Subtitle.Text = SubtitleText.FromString(text, new Font("Tahoma", 8, FontStyle.Regular), Color.White);
            ((ListViewItem_Subtitle)listView2.Items[index]).RefreshText(true);
            ((ListViewItem_Subtitle)listView2.Items[index]).EnsureVisible();
        }
        void ShowErrorMessage(Exception ex)
        {
            if (!this.InvokeRequired)
                ShowErrorMessage1(ex);
            else
                Invoke(new ShowErrorMessageDelegate(ShowErrorMessage1), new object[] { ex });
        }
        void ShowErrorMessage1(Exception ex)
        {
            MessageDialog.ShowErrorMessage(ex.Message, "!!");
        }
        void ApplyTranslatedSubtitle()
        {
            if (listView2.SelectedItems.Count != 1)
                return;
            if (subtitleTextEditor_translate.Save)
            {
                subtitleTextEditor_translate.SaveTextToSubtitle();
                ((ListViewItem_Subtitle)listView2.SelectedItems[0]).RefreshText(true);
                ApplyToTheSameText();
            }
        }
        void ApplyToTheSameText()
        {
            if (toolStripButton_apply_to_same_text.Checked)
            {
                if (listView1.SelectedItems.Count != 1)
                    return;
                string text = ((ListViewItem_Subtitle)listView1.SelectedItems[0]).Subtitle.Text.ToString();
                int index = listView1.SelectedItems[0].Index;

                for (int i = 0; i < listView2.Items.Count; i++)
                {
                    if (i == index)
                        continue;
                    if (((ListViewItem_Subtitle)listView2.Items[i]).Subtitle.Text.ToString() == text)
                    {
                        ((ListViewItem_Subtitle)listView2.Items[i]).Subtitle.Text = subtitleTextEditor_translate.SubtitleText.Clone();
                        ((ListViewItem_Subtitle)listView2.Items[i]).RefreshText(true);
                    }
                }

            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSelecting1)
                return;
            if (listView1.SelectedItems.Count != 1)
                return;
            ApplyTranslatedSubtitle();
            int index = listView1.SelectedItems[0].Index;
            //update selection
            if (index >= 0 && index < listView2.Items.Count)
            {
                isSelecting2 = true;
                listView2.Items[index].Selected = true;
                listView2.Items[index].EnsureVisible();
                subtitleTextEditor_translate.SubtitleText = ((ListViewItem_Subtitle)listView2.SelectedItems[0]).Subtitle.Text;
                isSelecting2 = false;
            }

            //text
            subtitleTextEditor_original.SubtitleText = ((ListViewItem_Subtitle)listView1.SelectedItems[0]).Subtitle.Text;
            ApplyToTheSameText();
        }
        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSelecting2)
                return;
            if (listView2.SelectedItems.Count != 1)
                return;
            ApplyTranslatedSubtitle();
            if (listView2.SelectedItems[0].Index >= 0 && listView2.SelectedItems[0].Index < listView1.Items.Count)
            {
                isSelecting1 = true;
                //update selection
                listView1.Items[listView2.SelectedItems[0].Index].Selected = true;
                listView1.Items[listView2.SelectedItems[0].Index].EnsureVisible();
                subtitleTextEditor_original.SubtitleText = ((ListViewItem_Subtitle)listView1.SelectedItems[0]).Subtitle.Text;
                isSelecting1 = false;
            }

            //text
            subtitleTextEditor_translate.SubtitleText = ((ListViewItem_Subtitle)listView2.SelectedItems[0]).Subtitle.Text;
        }
        //Create New
        private void button1_Click(object sender, EventArgs e)
        {
            createNew = true;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        //Replace
        private void button2_Click(object sender, EventArgs e)
        {
            createNew = false;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (mainThread != null)
            {
                if (mainThread.IsAlive)
                {
                    if (MessageDialog.ShowMessage(resources.GetString("Message_AreYouSureYouWantToStopCurrentProgress"),
                        resources.GetString("MessageCaption_StopTranslationProgress"), MessageDialogButtons.OkNo, MessageDialogIcon.Question) == MessageDialogResult.Ok)
                    {
                        mainThread.Abort();
                        mainThread = null;
                        Enablecontrols();
                    }
                    return;
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        //Show Help
        private void button4_Click(object sender, EventArgs e)
        {
            string helpPath = ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpPath))
                Help.ShowHelp(this, ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm",
                    HelpNavigator.KeywordIndex, "How to, Translate subtitles track");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm",
                    HelpNavigator.KeywordIndex, "How to, Translate subtitles track");
        }

        private void subtitleTextEditor_translate_SubtitleTextChanged(object sender, EventArgs e)
        {
            if (listView2.SelectedItems.Count != 1)
                return;

            ((ListViewItem_Subtitle)listView2.SelectedItems[0]).RefreshText(true);
            ApplyToTheSameText();
        }
        // Previous
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ApplyTranslatedSubtitle();
            int index = 0;
            if (listView1.SelectedItems.Count > 0)
            {

                index = listView1.SelectedItems[0].Index;
                listView1.SelectedItems[0].Selected = false;
            }
            index--;
            if (listView1.Items.Count > 0 && index >= 0)
                listView1.Items[index].Selected = true;
        }
        // Next
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ApplyTranslatedSubtitle();
            int index = -1;
            if (listView1.SelectedItems.Count > 0)
            {
                index = listView1.SelectedItems[0].Index;
                listView1.SelectedItems[0].Selected = false;
            }
            index++;
            if (listView1.Items.Count > 0 && index < listView1.Items.Count && index >= 0)
                listView1.Items[index].Selected = true;
        }

        private void Frm_Translate_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    {
                        toolStripButton2_Click(null, null);
                        break;
                    }
                case Keys.F2:
                    {
                        toolStripButton1_Click(null, null);
                        break;
                    }
            }
        }
    }
}
