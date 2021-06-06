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
using System.Collections.Generic;
using System.Windows.Forms;
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class SubtitlesDataEditor : UserControl
    {
        public SubtitlesDataEditor()
        {
            InitializeComponent();
        }

        SubtitlesTrack track;

        int timer = 1;
        bool showViewed = true;

        public void SaveSettings()
        {
            ControlsBase.Settings.SubtitlesData_ViewIndex = toolStripComboBox1.SelectedIndex;
            ControlsBase.Settings.SubtitlesData_startime = columnHeader_StartTime.Width;
            ControlsBase.Settings.SubtitlesData_endime = columnHeader_EndTime.Width;
            ControlsBase.Settings.SubtitlesData_duration = columnHeader_duration.Width;
            ControlsBase.Settings.SubtitlesData_text = columnHeader_text.Width;
            ControlsBase.Settings.SubtitlesData_number = columnHeader_number.Width;
            ControlsBase.Settings.SubtitlesData_IsTimeline = tabControl1.SelectedIndex == 1;
            ControlsBase.Settings.Save();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //load settings
            toolStripComboBox1.SelectedIndex = ControlsBase.Settings.SubtitlesData_ViewIndex;
            columnHeader_StartTime.Width = ControlsBase.Settings.SubtitlesData_startime;
            columnHeader_EndTime.Width = ControlsBase.Settings.SubtitlesData_endime;
            columnHeader_duration.Width = ControlsBase.Settings.SubtitlesData_duration;
            columnHeader_text.Width = ControlsBase.Settings.SubtitlesData_text;
            columnHeader_number.Width = ControlsBase.Settings.SubtitlesData_number;
            tabControl1.SelectedIndex = ControlsBase.Settings.SubtitlesData_IsTimeline ? 1 : 0;
            verticalDataEdit1.TimerEnable = tabControl1.SelectedIndex == 1;
        }
        /// <summary>
        /// Get or set if the control should always show the subtitle that viewed
        /// </summary>
        public bool AlwayesEnsureViewed
        { get { return showViewed; } set { showViewed = value; } }
        public double MediaDuration
        {
            get { return verticalDataEdit1.MediaObjectDuration; }
            set
            {
                verticalDataEdit1.MediaObjectDuration = value;
            }
        }
        public void SelectItem(int index)
        {
            try
            {
                listView1.Items[index].Selected = true;
            }
            catch { }
        }
        public void SelectItem(Subtitle sub)
        {
            foreach (ListViewItem_Subtitle item in listView1.Items)
            {
                if (item.Subtitle == sub)
                { item.Selected = true; break; }
            }
        }
        public void ClearDataIcons()
        {
            foreach (ListViewItem_Subtitle item in listView1.Items)
                item.ImageIndex = 0;
        }
        public void SetViewedSubtitle(int index, bool IsViewed)
        {
            try
            {
                ClearDataIcons();
                verticalDataEdit1.in_view_sub_index = IsViewed ? index : -1;
                listView1.Items[index].ImageIndex = IsViewed ? 1 : 0;
                if (showViewed & IsViewed)
                    listView1.Items[index].EnsureVisible();
            }
            catch { }
        }
        /// <summary>
        /// Refresh the subtitles to review them
        /// </summary>
        public void RefreshSubtitles()
        {
            listView1.Visible = false;
            SubtitleTimingMode timingMode = SubtitleTimingMode.SecondMilli;
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0: timingMode = SubtitleTimingMode.SecondMilli; break;
                case 1: timingMode = SubtitleTimingMode.Timespan_Milli; break;
                case 2: timingMode = SubtitleTimingMode.Timespan_NTSC; break;
                case 3: timingMode = SubtitleTimingMode.Timespan_PAL; break;
                case 4: timingMode = SubtitleTimingMode.Frames_NTSC; break;
                case 5: timingMode = SubtitleTimingMode.Frames_Pal; break;
            }
            listView1.Items.Clear();
            if (track != null)
            {
                int i = 1;
                foreach (Subtitle sub in track.Subtitles)
                {
                    ListViewItem_Subtitle item = new ListViewItem_Subtitle();
                    item.Number = i;
                    item.Subtitle = sub;
                    item.ChangeTimingView(timingMode);
                    listView1.Items.Add(item);
                    i++;
                }
            }
            ViewSelectionText();
            listView1.Visible = true;
        }
        /// <summary>
        /// Clear subtitles from the list
        /// </summary>
        public void Clear()
        {
            listView1.Items.Clear();
        }
        public void SelectAll()
        {
            foreach (ListViewItem item in listView1.Items)
                item.Selected = true;
        }
        public void SelectNone()
        {
            foreach (ListViewItem item in listView1.Items)
                item.Selected = false;
        }
        public void SelectAllAfterSelected()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int ii = listView1.SelectedItems[0].Index;
                for (int i = ii; i < listView1.Items.Count; i++)
                    listView1.Items[i].Selected = true;
            }
        }
        public void SelectAllBeforeSelected()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                int ii = listView1.SelectedItems[listView1.SelectedItems.Count - 1].Index;
                for (int i = 0; i <= ii; i++)
                    listView1.Items[i].Selected = true;
            }
        }
        public void SelectNextSubtitle()
        {
            int index = -1;
            if (listView1.SelectedItems.Count == 1)
                index = listView1.SelectedItems[0].Index;
            else if (listView1.SelectedItems.Count > 0)
                index = listView1.SelectedItems[listView1.SelectedItems.Count - 1].Index;

            index++;
            try
            {
                listView1.SelectedItems.Clear();
                listView1.Items[index].Selected = true;
                if (tabControl1.SelectedIndex == 1)
                {
                    verticalDataEdit1.ScrollToTime(((ListViewItem_Subtitle)listView1.SelectedItems[listView1.SelectedItems.Count - 1]).Subtitle.StartTime, true);
                }
            }
            catch
            {

            }
        }
        public void SelectPreviousSubtitle()
        {
            int index = -1;
            if (listView1.SelectedItems.Count == 1)
                index = listView1.SelectedItems[0].Index;
            else if (listView1.SelectedItems.Count > 0)
                index = listView1.SelectedItems[listView1.SelectedItems.Count - 1].Index;

            index--;
            try
            {
                listView1.SelectedItems.Clear();
                listView1.Items[index].Selected = true;
                if (tabControl1.SelectedIndex == 1)
                {
                    verticalDataEdit1.ScrollToTime(((ListViewItem_Subtitle)listView1.SelectedItems[listView1.SelectedItems.Count - 1]).Subtitle.StartTime, true);
                }
            }
            catch
            {

            }
        }
        public void SelectWithSameText()
        {
            if (listView1.SelectedItems.Count == 1)
            {
                ListViewItem_Subtitle selected = (ListViewItem_Subtitle)listView1.SelectedItems[0];
                foreach (ListViewItem_Subtitle sub in listView1.Items)
                {
                    sub.Selected = (sub.Subtitle.Text.ToString() == selected.Subtitle.Text.ToString());
                }
            }
        }
        public void ShowCurrentlyInView()
        {
            foreach (ListViewItem_Subtitle item in listView1.Items)
            {
                if (item.ImageIndex == 1)
                {
                    item.EnsureVisible();
                    break;
                }
            }
        }
        public void ViewSelectionText()
        {
            toolStripLabel1.Text = listView1.SelectedItems.Count + " / " + listView1.Items.Count;
        }
        //Properties
        /// <summary>
        /// Get or set the subtitles track to show subtitles for
        /// </summary>
        public SubtitlesTrack SubtitlesTrack
        {
            get { return track; }
            set
            {
                track = value;
                verticalDataEdit1.SubtitlesTrack = value;
                if (track != null)
                {
                    RefreshSubtitles();
                }
                else
                {
                    listView1.Items.Clear();
                }
            }
        }
        /// <summary>
        /// Get selected subtitle items
        /// </summary>
        public ListViewItem_Subtitle[] SelectedSubtitleItems
        {
            get
            {
                List<ListViewItem_Subtitle> items = new List<ListViewItem_Subtitle>();
                foreach (ListViewItem_Subtitle item in listView1.SelectedItems)
                {
                    items.Add(item);
                }
                return items.ToArray();
            }
        }
        /// <summary>
        /// Get subtitle items
        /// </summary>
        public ListViewItem_Subtitle[] SubtitleItems
        {
            get
            {
                List<ListViewItem_Subtitle> items = new List<ListViewItem_Subtitle>();
                foreach (ListViewItem_Subtitle item in listView1.Items)
                {
                    items.Add(item);
                }
                return items.ToArray();
            }
        }
        /// <summary>
        /// Get Selected Subtitle Indices
        /// </summary>
        public int[] SelectedSubtitleIndices
        {
            get
            {
                List<int> items = new List<int>();
                foreach (int item in listView1.SelectedIndices)
                {
                    items.Add(item);
                }
                return items.ToArray();
            }
        }
        public int SubtitlesCount
        { get { return listView1.Items.Count; } }
        //Events
        public event EventHandler SubtitlesSelected;
        public event EventHandler<TimeChangeArgs> TimeChangeRequest;
        /// <summary>
        /// Rised up when the user double click on one subtitle
        /// </summary>
        public event EventHandler SubtitleEditRequest;
        public event EventHandler EditStylesRequest;
        public event EventHandler SubtitlePropertiesChanged;

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SubtitleTimingMode timingMode = SubtitleTimingMode.SecondMilli;
            switch (toolStripComboBox1.SelectedIndex)
            {
                case 0: timingMode = SubtitleTimingMode.SecondMilli; break;
                case 1: timingMode = SubtitleTimingMode.Timespan_Milli; break;
                case 2: timingMode = SubtitleTimingMode.Timespan_NTSC; break;
                case 3: timingMode = SubtitleTimingMode.Timespan_PAL; break;
                case 4: timingMode = SubtitleTimingMode.Frames_NTSC; break;
                case 5: timingMode = SubtitleTimingMode.Frames_Pal; break;
            }
            foreach (ListViewItem_Subtitle sub in listView1.Items)
            {
                sub.ChangeTimingView(timingMode);
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            SelectAll();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            SelectNone();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SelectWithSameText();
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            timer = 1;
            timer1.Start();
        }
        //Selection Timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer > 0)
                timer--;
            else
            {
                timer1.Stop();
                //rise event
                SubtitlesSelected?.Invoke(this, new EventArgs());
                verticalDataEdit1.SelectedSubtitles.Clear();
                List<Subtitle> subs = new List<Subtitle>();
                foreach (ListViewItem_Subtitle sub in SelectedSubtitleItems)
                {
                    subs.Add(sub.Subtitle);
                }
                verticalDataEdit1.SelectedSubtitles = subs;
                if (listView1.SelectedItems.Count == 1)
                {
                    TimeChangeRequest?.Invoke(this, new TimeChangeArgs(((ListViewItem_Subtitle)listView1.SelectedItems[0]).Subtitle.StartTime, 0));
                }
                ViewSelectionText();
            }
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (SubtitleEditRequest != null)
                    SubtitleEditRequest(this, new EventArgs());
            }
        }
        //focus on viewd
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            ShowCurrentlyInView();
        }
        //select next
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            SelectNextSubtitle();
        }
        //select previous
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            SelectPreviousSubtitle();
        }
        // auto scroll switch
        private void toolStripButton7_CheckedChanged(object sender, EventArgs e)
        {
            AlwayesEnsureViewed = toolStripButton7.Checked;
        }
        // select all before selected
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            SelectAllBeforeSelected();
        }
        // select all after selected.
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            SelectAllAfterSelected();
        }
        private void verticalDataEdit1_SubtitlesSelected(object sender, EventArgs e)
        {
            SelectNone();
            foreach (Subtitle sub in verticalDataEdit1.SelectedSubtitles)
            {
                SelectItem(sub);
            }
        }
        private void verticalDataEdit1_SubtitleClick(object sender, EventArgs e)
        {
        }
        private void verticalDataEdit1_SubtitleDoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (SubtitleEditRequest != null)
                    SubtitleEditRequest(this, new EventArgs());
            }
        }
        private void VerticalDataEdit1_SubtitlePropertiesChanged(object sender, System.EventArgs e)
        {
            SubtitlePropertiesChanged?.Invoke(this, new EventArgs());
        }
        private void TabControl1_TabIndexChanged(object sender, System.EventArgs e)
        {
            verticalDataEdit1.TimerEnable = tabControl1.SelectedIndex == 1;
        }
    }
}
