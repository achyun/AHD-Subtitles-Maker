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
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;
using AHD.Forms;

namespace AHD.SM.Controls
{
    public partial class SubtitleEditor : UserControl
    {
        public SubtitleEditor()
        {
            InitializeComponent();
        }
        ListViewItem_Subtitle[] selectedItems;
        SubtitlesTrack subtitleTrack;
        Subtitle currentSubtitle;
        int timer = 1;
        bool directApply = true;
        bool setSave = false;
        bool askWhenChangingRTL;
        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource", Assembly.GetExecutingAssembly());

        public void Clear()
        {
            currentSubtitle = null;
            timeEdit_duration.SetTime(0, false);
            timeSpaner_start.SetTime(0, false);
            timeSpaner_end.SetTime(0, false);
            subtitleTextEditor1.SubtitleText = null;
            SetStrings();
            groupBox1.Visible = false;
        }
        void SetStrings()
        {
            label_start.Text = timeSpaner_start.GetSeconds().ToString("F3") + " " + resources.GetString("Unit_Second");
            label_end.Text = timeSpaner_end.GetSeconds().ToString("F3") + " " + resources.GetString("Unit_Second");
            label_duration.Text = timeEdit_duration.GetSeconds().ToString("F3") + " " + resources.GetString("Unit_Second");
        }
        void SetSaveRequest()
        {
            timer = 5;
            timer1.Start();
        }
        public void Save()
        {
            selectedItems[0].Subtitle.StartTime = timeSpaner_start.GetSeconds();
            selectedItems[0].Subtitle.EndTime = timeSpaner_end.GetSeconds();
            subtitleTextEditor1.SaveTextToSubtitle();
            selectedItems[0].Subtitle.Text = subtitleTextEditor1.SubtitleText;
            selectedItems[0].ChangeTimingView();//and refresh text
        }
        public void DeleteSelected()
        {
            subtitleTextEditor1.Delete();
        }

        /// <summary>
        /// Get or set selected items
        /// </summary>
        public ListViewItem_Subtitle[] SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                RefreshText();
                //refresh compobox items
                setSave = false;
                comboBox1.Items.Clear();
                /*if (subtitleTrack != null)
                {
                    foreach (Subtitle sub in subtitleTrack.Subtitles)
                    {
                        bool found = false;
                        foreach (SubtitleText text in comboBox1.Items)
                        {
                            if (text.ToString() == sub.Text.ToString())
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            comboBox1.Items.Add(sub.Text);
                    }
                }*/
                //set text ...
                if (selectedItems != null)
                {
                    if (selectedItems.Length == 1)
                        comboBox1.Text = selectedItems[0].Subtitle.Text.ToString();
                    else
                    {
                        if (subtitleTextEditor1.SubtitleText != null)
                            if (subtitleTextEditor1.SubtitleText.ToString() != "<SUBTITLE TEXTS>")
                                comboBox1.Text = selectedItems[0].Subtitle.Text.ToString();
                    }
                }
                setSave = true;
            }
        }
        public void RefreshText()
        {
            setSave = false;
            timer1.Stop();
            if (selectedItems == null)
            {
                currentSubtitle = null;
                Clear();
                this.Enabled = false;
                return;
            }
            if (selectedItems.Length == 0)
            {
                currentSubtitle = null;
                Clear();
                this.Enabled = false;
                return;
            }
            if (selectedItems.Length == 1)
            {
                this.Enabled = true;
                timeEdit_duration.Enabled = timeSpaner_start.Enabled = timeSpaner_end.Enabled = true;
                currentSubtitle = selectedItems[0].Subtitle;
                timeEdit_duration.SetTime(selectedItems[0].Subtitle.Duration, false);
                timeSpaner_start.SetTime(selectedItems[0].Subtitle.StartTime, false);
                timeSpaner_end.SetTime(selectedItems[0].Subtitle.EndTime, false);
                subtitleTextEditor1.SubtitleText = selectedItems[0].Subtitle.Text;
                SetStrings();
                CheckForErrors();
            }
            if (selectedItems.Length > 1)
            {
                this.Enabled = true;
                currentSubtitle = null;
                Clear();
                timeEdit_duration.Enabled = timeSpaner_start.Enabled = timeSpaner_end.Enabled = false;
                bool same = true;
                string startText = selectedItems[0].Subtitle.Text.ToString();
                foreach (ListViewItem_Subtitle sub in selectedItems)
                {
                    if (sub.Subtitle.Text.ToString() != startText)
                    {
                        same = false; break;
                    }
                }
                if (same)
                    subtitleTextEditor1.SubtitleText = selectedItems[0].Subtitle.Text;
                else
                    subtitleTextEditor1.SubtitleText = SubtitleText.FromString(resources.GetString("SUBTITLETEXTS"),
                        new Font("Tahoma", 8, FontStyle.Regular), Color.White);
            }
        }
        public void RefreshText(string rtf)
        {
            subtitleTextEditor1.SetTextRTF(rtf);
        }
        public SubtitlesTrack SubtitlesTrack
        {
            get { return subtitleTrack; }
            set
            {
                subtitleTrack = value;
                if (value == null)
                    Clear();
            }
        }
        public bool AskWhenChangingRTLToApplyToAll
        { get { return askWhenChangingRTL; } set { askWhenChangingRTL = value; } }
        public void RefreshStyles(ASMPFontStyle[] styles)
        {
            subtitleTextEditor1.RefreshStyles(styles);
        }

        public void CheckForErrors()
        {
            if (currentSubtitle == null)
                return;
            listBox1.Items.Clear();
            double Start = timeSpaner_start.GetSeconds();
            double End = timeSpaner_end.GetSeconds();
            //Set duration text in the time spanner
            //Check text
            if (subtitleTextEditor1.TextLength == 0)
            {
                listBox1.Items.Add(resources.GetString("Error_YouMustWriteAText"));
            }
            //Check start time
            if (Start < 0)
            {
                listBox1.Items.Add(resources.GetString("Error_StartTimeIsLessThan0"));
            }
            if (Start > End)
            {
                listBox1.Items.Add(resources.GetString("Error_StartTimeIsLargerThanTheEndTime"));
            }
            if (Start == End)
            {
                listBox1.Items.Add(resources.GetString("Error_StartTimeIsEqualTheEndTime"));
            }
            foreach (Subtitle Su in subtitleTrack.Subtitles)
            {
                if (Su == currentSubtitle)
                { continue; }

                if (Start >= Su.StartTime & Start <= Su.EndTime)
                {
                    string T = TimeSpan.FromSeconds(Su.StartTime).ToString();
                    if (T.Length > 12) { T = T.Substring(0, 12); }
                    listBox1.Items.Add(resources.GetString("Error_StartTimeIsOverAnotherSubtitleAt") + " " + T);
                }
            }
            //Check end time
            if (End < 0)
            {
                listBox1.Items.Add(resources.GetString("Error_EndTimeIsLessThan0"));
            }
            foreach (Subtitle Su in subtitleTrack.Subtitles)
            {
                if (Su == currentSubtitle)
                { continue; }
                if (End >= Su.StartTime & End <= Su.EndTime)
                {
                    string T = TimeSpan.FromSeconds(Su.StartTime).ToString();
                    if (T.Length > 12) { T = T.Substring(0, 12); }
                    listBox1.Items.Add(resources.GetString("Error_EndTimeIsoverAnotherSubtitleAt") + " " + T);
                }
            }
            //set button enablation
            groupBox1.Visible = listBox1.Items.Count > 0;
        }
        /// <summary>
        /// Get a value indecate wether errors appears to user
        /// </summary>
        public bool HasErrors
        { get { return groupBox1.Visible; } }
        /// <summary>
        /// Get or set a value indecate wether the conrol should apply changed values to the items directly
        /// </summary>
        public bool DirectApplyToItem
        { get { return directApply; } set { directApply = value; } }

        //events
        /// <summary>
        /// Rised up when the errors list visible changed
        /// </summary>
        public event EventHandler ErrorVisibleChanged;
        /// <summary>
        /// Rised up when this editor done an edit and require a project save
        /// </summary>
        public event EventHandler SaveRequest;
        public event EventHandler EditStylesRequest;
        public Color EditorBackColor
        {
            get { return subtitleTextEditor1.BackColor; }
            set { subtitleTextEditor1.BackColor = value; }
        }
        public bool ShowStatusStrip
        { get { return subtitleTextEditor1.ShowStatusStrip; } set { subtitleTextEditor1.ShowStatusStrip = value; } }
        public void SelectAll()
        {
            subtitleTextEditor1.SelectAll();
        }
        private void timeSpaner_start_TimeChanged(object sender, EventArgs e)
        {
            timeEdit_duration.SetTime(timeSpaner_end.GetSeconds() - timeSpaner_start.GetSeconds(), false);
            SetStrings();
            CheckForErrors();
            if (directApply)
            {
                //do we have errors ? if not, update the subtitle
                if (!groupBox1.Visible)
                {
                    selectedItems[0].Subtitle.StartTime = timeSpaner_start.GetSeconds();
                    selectedItems[0].RefreshText();
                    selectedItems[0].ChangeTimingView();
                    SetSaveRequest();
                }
            } 
        }
        private void timeSpaner_end_TimeChanged(object sender, EventArgs e)
        {
            timeEdit_duration.SetTime(timeSpaner_end.GetSeconds() - timeSpaner_start.GetSeconds(), false);
            SetStrings();
            CheckForErrors();
            if (directApply)
            {
                //do we have errors ? if not, update the subtitle
                if (!groupBox1.Visible)
                {
                    selectedItems[0].Subtitle.EndTime = timeSpaner_end.GetSeconds();
                    selectedItems[0].RefreshText();
                    selectedItems[0].ChangeTimingView();
                    SetSaveRequest();
                }
            }

        }
        private void timeEdit_duration_TimeChanged(object sender, EventArgs e)
        {
            double time = timeSpaner_start.GetSeconds() + timeEdit_duration.GetSeconds();
            timeSpaner_end.SetTime(time, false);
            SetStrings();
            CheckForErrors();
            if (directApply)
            {
                //do we have errors ? if not, update the subtitle
                if (!groupBox1.Visible)
                {
                    selectedItems[0].Subtitle.EndTime = timeSpaner_end.GetSeconds();
                    selectedItems[0].RefreshText();
                    selectedItems[0].ChangeTimingView();
                    SetSaveRequest();
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer > 0)
                timer--;
            else
            {
                timer1.Stop();
                if (SaveRequest != null)
                    SaveRequest(this, new EventArgs());
            }
        }
        private void subtitleTextEditor1_SubtitleTextChanged(object sender, EventArgs e)
        {
            if (currentSubtitle != null)
                CheckForErrors();
            else
            {
                listBox1.Items.Clear();
                if (subtitleTextEditor1.TextLength == 0)
                {
                    listBox1.Items.Add(resources.GetString("Error_YouMustWriteAText"));
                }
                //set button enablation
                groupBox1.Visible = listBox1.Items.Count > 0;
            }
            if (directApply)
            {
                if (!groupBox1.Visible)
                {
                    if (currentSubtitle != null)
                    {
                        //selectedItems[0].Subtitle.Text = timeSpaner_end.GetSeconds();
                        selectedItems[0].Subtitle.Text = SubtitleTextWrapper.Clone(subtitleTextEditor1.SubtitleText);
                        selectedItems[0].RefreshText();
                        selectedItems[0].ChangeTimingView();
                        //save request
                        if (SaveRequest != null)
                            SaveRequest(this, new EventArgs());
                    }
                    else
                    {
                        //multi subtitles text "Same text must have"
                        foreach (ListViewItem_Subtitle sub in selectedItems)
                        {
                            sub.Subtitle.Text = SubtitleTextWrapper.Clone(subtitleTextEditor1.SubtitleText);
                            sub.RefreshText();
                            sub.ChangeTimingView();
                        }
                        //save request
                        if (SaveRequest != null)
                            SaveRequest(this, new EventArgs());
                    }
                }
            }
        }
        private void groupBox1_VisibleChanged(object sender, EventArgs e)
        {
            if (ErrorVisibleChanged != null)
                ErrorVisibleChanged(sender, e);
        }
        private void subtitleTextEditor1_SubtitleTextLengthChanged(object sender, EventArgs e)
        {
            if (currentSubtitle != null)
                CheckForErrors();
            else
            {
                listBox1.Items.Clear();
                if (subtitleTextEditor1.TextLength == 0)
                {
                    listBox1.Items.Add(resources.GetString("Error_YouMustWriteAText"));
                }
                //set button enablation
                groupBox1.Visible = listBox1.Items.Count > 0;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!setSave)
                return;
            subtitleTextEditor1.SubtitleText = SubtitleTextWrapper.Clone(((SubtitleText)comboBox1.SelectedItem));
            subtitleTextEditor1_SubtitleTextChanged(this, null);
        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            //refresh
            if (comboBox1.Items.Count == 0)
            {
                if (subtitleTrack != null)
                {
                    foreach (Subtitle sub in subtitleTrack.Subtitles)
                    {
                        bool found = false;
                        foreach (SubtitleText text in comboBox1.Items)
                        {
                            if (text.ToString() == sub.Text.ToString())
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                            comboBox1.Items.Add(sub.Text);
                    }
                }
            }
        }
        private void subtitleTextEditor1_SubtitleTextRightToLeftValueChanged(object sender, EventArgs e)
        {
            if (askWhenChangingRTL)
            {
                MessageDialogResult result = MessageDialog.ShowMessage(this, resources.GetString("Message_ApplyRightToLeftAlignementValueToAllSubtitleOfThisSubtitlesTrack"),
                    resources.GetString("MessageCaption_RightToLeftChanged"), MessageDialogButtons.OkNo | MessageDialogButtons.Checked,
                    MessageDialogIcon.Question, askWhenChangingRTL,
                    resources.GetString("Button_Yes"), resources.GetString("Button_No"), "",
                   resources.GetString("CheckBox_AlwaysAskThisWhenChangingRtl"));
                if ((result & MessageDialogResult.Ok) == MessageDialogResult.Ok)
                {
                    foreach (Subtitle sub in subtitleTrack.Subtitles)
                        sub.Text.RighttoLeft = subtitleTextEditor1.SubtitleTextRightToLeft;
                }
                askWhenChangingRTL =
                    ((result & MessageDialogResult.Checked) == MessageDialogResult.Checked);
                if (SaveRequest != null)
                    SaveRequest(this, new EventArgs());
            }
        }
        private void subtitleTextEditor1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        private void subtitleTextEditor1_EditStylesRequest(object sender, EventArgs e)
        {
            EditStylesRequest?.Invoke(this, new EventArgs());
        }
    }
}
