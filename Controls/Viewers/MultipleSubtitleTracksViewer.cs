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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class MultipleSubtitleTracksViewer : UserControl
    {
        public MultipleSubtitleTracksViewer()
        {
            InitializeComponent();
            resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource",
        Assembly.GetExecutingAssembly());
        }
        private ResourceManager resources;
        private SubtitlesTrack[] tracksOfProject;
        private List<SubtitleTextViewer> textViewerControls = new List<SubtitleTextViewer>();
        private List<int> sIndex = new List<int>();//current viewed subtitle index
        private List<int> pIndex = new List<int>();//set to -1 to refresh view

        public event EventHandler UpdateTrackChecks;

        private int FindSubtitleIndex(double time, SubtitlesTrack selectedTrack)
        {
            int i = 0;
            foreach (Subtitle sub in selectedTrack.Subtitles)
            {
                if (sub.StartTime <= time & sub.EndTime >= time)
                    return i;

                i++;
            }

            return -1;
        }
        public void RefreshTracks(SubtitlesTrack[] tracksOfProject)
        {
            this.tracksOfProject = tracksOfProject;
            UpdateTrackControls();
        }
        public void ClearView()
        {
            this.tracksOfProject = null;
            UpdateTrackControls();
        }
        public void UpdateTrackControls()
        {
            this.Controls.Clear();
            textViewerControls.Clear();
            sIndex.Clear();
            pIndex.Clear();
            if (this.tracksOfProject == null)
            {
                goto SHOWMESSAGE;
            }
            if (this.tracksOfProject.Length == 0)
            {
                goto SHOWMESSAGE;
            }
            // Add the controls
            int index = 0;
            bool disable = true;
            foreach (SubtitlesTrack tr in tracksOfProject)
            {
                if (tr.Preview)
                {
                    GroupBox gr = new GroupBox();
                    gr.Text = tr.Name;
                    gr.Dock = DockStyle.Top;
                    gr.Height = 70;
                    gr.Tag = index;
                    this.Controls.Add(gr);
                    gr.BringToFront();

                    // Add a subtitle text view control
                    SubtitleTextViewer v = new SubtitleTextViewer();
                    v.Tag = index;
                    v.Dock = DockStyle.Fill;
                    textViewerControls.Add(v);
                    gr.Controls.Add(v);

                    sIndex.Add(-1);
                    pIndex.Add(-1);
                    disable = false;
                }
                index++;
            }
            if (disable)
                goto SHOWMESSAGE;
            return;

        SHOWMESSAGE:
            Label l = new Label();
            l.AutoSize = false; 
            l.Dock = DockStyle.Fill;
            l.Text = resources.GetString("Message_PleaseCheckTracksToPreview");
            this.Controls.Add(l);
        }

        private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.tracksOfProject[(int)((LinkLabel)sender).Tag].Preview = false;
            this.UpdateTrackControls();
            if (UpdateTrackChecks != null)
                UpdateTrackChecks(this, new EventArgs());
        }
        public void UpdateTimer(double mediaCurrentTime)
        {
            if (!this.Visible)
                return;
            int i = 0;
            foreach (SubtitleTextViewer viewer in textViewerControls)
            {
                int trackIndex = (int)viewer.Tag;
                // Look for subtitle to view
                sIndex[i] = FindSubtitleIndex(mediaCurrentTime, tracksOfProject[trackIndex]);
                if (sIndex[i] >= 0)
                {
                    if (sIndex[i] != pIndex[i])
                    {
                        viewer.ViewText(tracksOfProject[trackIndex].Subtitles[sIndex[i]].Text);
                    }
                    pIndex[i] = sIndex[i];
                }
                else
                {
                    viewer.HideText();
                }
                i++;
            }
        }
    }
}
