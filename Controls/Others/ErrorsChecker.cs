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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;
using AHD.Forms;

namespace AHD.SM.Controls
{
    public partial class ErrorsChecker : UserControl
    {
        SubtitlesTrack track;
        public event EventHandler<ProgressArgs> CheckProgress;
        public event EventHandler CheckFinished;
        public event EventHandler CheckStarted;
        public event EventHandler<ProgressArgs> AutoFixProgress;
        public event EventHandler AutoFixFinished;
        public event EventHandler AutoFixStarted;
        public event EventHandler<SubtitlesSelectArgs> SelectSubtitlesRequest;
        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        public ErrorsChecker()
        {
            InitializeComponent();
        }
        public SubtitlesTrack SubtitlesTrack
        {
            get { return track; }
            set
            {
                track = value;
                if (value != track)
                    listView1.Items.Clear();
            }
        }

        public void AddErrorItem(string SubtitleNumber, string ErrorCaption, string ErrorType)
        {
            listView1.Items.Add(SubtitleNumber);
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(ErrorCaption);
            listView1.Items[listView1.Items.Count - 1].SubItems.Add(ErrorType);
            listView1.Items[listView1.Items.Count - 1].Checked = true;
        }
        public void CheckForErrors()
        {
            if (track == null)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_SelectSubtitlesTrackFirst"),
                   resources.GetString("MessageCaption_CheckForErrors"));
                return;
            }
            if (CheckStarted != null)
                CheckStarted(this, new EventArgs());

            listView1.Items.Clear();
            for (int i = 0; i < track.Subtitles.Count; i++)
            {
                //text errors
                if (track.Subtitles[i].Text.ToString().Length == 0)
                {
                    AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleHasNoText"),
                        resources.GetString("ErrorType_TextError"));
                }
                //timing errors
                if (track.Subtitles[i].Duration <= 0)
                {
                    AddErrorItem((i + 1).ToString(),resources.GetString( "Error_ThisSubtitlesDurationIs0OrLess"),
                        resources.GetString("ErrorType_StartAndEndTime"));
                }
                if (track.Subtitles[i].StartTime <= 0)
                {
                    AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleStartTimeIsLessOrEqual0"), 
                        resources.GetString("ErrorType_StartTime"));
                }
                if (track.Subtitles[i].EndTime <= 0)
                {
                    AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleEndTimeIsLessOrEqual0"),
                       resources.GetString("ErrorType_EndTime"));
                }
                foreach (Subtitle sub in track.Subtitles)
                {
                    if (track.Subtitles[i] != sub)
                    {
                        if (track.Subtitles[i].StartTime >= sub.StartTime &&
                            track.Subtitles[i].EndTime <= sub.EndTime)
                        {
                            AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleIsInsideAnotherOne"), 
                                resources.GetString("ErrorType_StartAndEndTime"));
                        }
                        if (track.Subtitles[i].StartTime < sub.StartTime &&
                              track.Subtitles[i].EndTime > sub.EndTime)
                        {
                            AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleIsOverAnotherOne"), 
                                resources.GetString("ErrorType_StartAndEndTime"));
                        }
                        if (track.Subtitles[i].EndTime > sub.EndTime &&
                              track.Subtitles[i].StartTime >= sub.StartTime && track.Subtitles[i].StartTime <= sub.EndTime)
                        {
                            AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleStartTimeIsOverAnotherOne"), 
                                resources.GetString("ErrorType_StartTime"));
                        }
                        if (track.Subtitles[i].StartTime < sub.StartTime &&
                            track.Subtitles[i].EndTime <= sub.EndTime && track.Subtitles[i].EndTime >= sub.StartTime)
                        {
                            AddErrorItem((i + 1).ToString(), resources.GetString("Error_ThisSubtitleEndTimeIsOverAnotherOne"), 
                                resources.GetString("ErrorType_EndTime"));
                        }
                    }
                }
                int x = (i * 100) / track.Subtitles.Count; 
                if (CheckProgress != null)
                    CheckProgress(this, new ProgressArgs(x, resources.GetString("Status_CheckingForBasicErrors") + " " + x + " %"));
            }
            if (CheckProgress != null)
                CheckProgress(this, new ProgressArgs(100, resources.GetString("Status_Done") + ", " + listView1.Items.Count +
                    " "+resources.GetString("Status_ErrorsFound")));
            if (CheckFinished != null)
                CheckFinished(this, new EventArgs());
        }
        public void SelectErrorSubtitles()
        {
            if (listView1.SelectedItems.Count > 0)
            {
                List<int> indices = new List<int>();
                foreach (ListViewItem item in listView1.SelectedItems)
                {
                    if (!indices.Contains(int.Parse(item.Text) - 1))
                        indices.Add(int.Parse(item.Text) - 1);
                }
                if (SelectSubtitlesRequest != null)
                    SelectSubtitlesRequest(this, new SubtitlesSelectArgs(indices.ToArray()));
            }
        }
        public void SelectAllItems()
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Selected = true;
            }
        }
        public void AutoFixErrors()
        {
            //TODO: more smart ways to fix !!
            if (listView1.Items.Count == 0)
            {
                CheckForErrors();
            }
            if (AutoFixStarted != null)
                AutoFixStarted(this, new EventArgs());
            int i = 0;
     
            foreach (ListViewItem item in listView1.Items)
            {
                //progress
                int x = (i * 100) / listView1.Items.Count;
                if (CheckProgress != null)
                    CheckProgress(this, new ProgressArgs(x, resources.GetString("Status_Fixing") + " " + x + " %"));
                if (!item.Checked)
                {   
                    i++;
                    continue;
                }
                //get needed values
                int subtitleIndex = int.Parse(item.Text) - 1;
                string errorMessage = item.SubItems[1].Text; 
                string errorType = item.SubItems[2].Text;
                //check what kind of error
                if (errorType == resources.GetString("ErrorType_TextError"))
                {
                    track.Subtitles[subtitleIndex].Text = SubtitleText.FromString("<MISSED TEXT>");
                    item.ImageIndex = 1;
                }
                else if (errorType == resources.GetString("ErrorType_StartAndEndTime"))
                {
                    if (errorMessage == resources.GetString("Error_ThisSubtitleIsInsideAnotherOne") ||
                        errorMessage == resources.GetString("Error_ThisSubtitleStartTimeIsOverAnotherOne"))
                    {
                        //unfixable errors, this subtitle is inside/over another one and we don't know 
                        //exactly where the subtitle is
                        item.ImageIndex = 2;
                    }
                    else if (errorMessage == resources.GetString("Error_ThisSubtitlesDurationIs0OrLess"))
                    {
                        if (subtitleIndex < track.Subtitles.Count)
                            track.Subtitles[subtitleIndex].EndTime = track.Subtitles[subtitleIndex + 1].StartTime
                                - 0.001;
                        else
                            track.Subtitles[subtitleIndex].EndTime = track.Subtitles[subtitleIndex].StartTime + 0.01;
                        item.ImageIndex = 1;
                    }
                }
                else if (errorType == resources.GetString("ErrorType_StartTime"))
                {
                    double oldStart = track.Subtitles[subtitleIndex].StartTime;
                    if (subtitleIndex > 0)
                    {
                        track.Subtitles[subtitleIndex].StartTime = track.Subtitles[subtitleIndex - 1].EndTime
                             + 0.001;
                        while (track.Subtitles[subtitleIndex].StartTime == track.Subtitles[subtitleIndex - 1].EndTime)
                            track.Subtitles[subtitleIndex].StartTime += 0.001;
                    }
                    else
                    {
                        if (track.Subtitles[subtitleIndex].StartTime < 0)
                            track.Subtitles[subtitleIndex].StartTime = 0;
                        else
                            track.Subtitles[subtitleIndex].StartTime = track.Subtitles[subtitleIndex].EndTime - 0.01;
                    }
                    item.ImageIndex = 1;
                }
                else if (errorType == resources.GetString("ErrorType_EndTime"))
                {
                    double oldEnd = track.Subtitles[subtitleIndex].EndTime;
                    if (subtitleIndex < track.Subtitles.Count - 1)
                    {
                        track.Subtitles[subtitleIndex].EndTime = track.Subtitles[subtitleIndex + 1].StartTime
                           - 0.001;
                        while (track.Subtitles[subtitleIndex].EndTime == track.Subtitles[subtitleIndex + 1].StartTime)
                            track.Subtitles[subtitleIndex].EndTime -= 0.001;
                    }
                    else
                        track.Subtitles[subtitleIndex].EndTime = track.Subtitles[subtitleIndex].StartTime + 0.01;

                    item.ImageIndex = 1;
                }
                i++;
               
            }
            if (AutoFixProgress != null)
                AutoFixProgress(this, new ProgressArgs(100, resources.GetString("Status_Done")));
            if (AutoFixFinished != null)
                AutoFixFinished(this, new EventArgs());
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SelectErrorSubtitles();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SelectAllItems();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AutoFixErrors();
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectErrorSubtitles();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = false;
        }
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            CheckForErrors();
        }
    }
}
