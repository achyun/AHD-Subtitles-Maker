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
using AHD.SM.ASMP;
namespace AHD.SM.Controls
{
    public partial class VerticalDataEdit : UserControl
    {
        public VerticalDataEdit()
        {
            InitializeComponent();

            verticalDataEditPanel1.AutoScrollWhenTimePassViewPort = true;
            verticalDataEditPanel1.AlwaysScroll = true;

            WaveReader.WFPDLoaded += WaveReader_WFPDLoaded;
            WaveReader.WFPDChunckGenerated += WaveReader_WFPDChunckGenerated;

            toolStripButton8.Checked = ControlsBase.Settings.Vertical_L;
            verticalDataEditPanel1.AlwaysShowSubitlesLines = toolStripButton8.Checked;

            toolStripButton6.Checked = ControlsBase.Settings.Vertical_S;
            verticalDataEditPanel1.ShowViewSubtitleTimes = toolStripButton6.Checked;

            toolStripButton5.Checked = ControlsBase.Settings.Vertical_T;
            verticalDataEditPanel1.ShowTimeTicks = toolStripButton5.Checked;

            verticalDataEditPanel1.TimingPanelWidth = ControlsBase.Settings.Vertical_WW;
        }
        public double MediaObjectDuration
        {
            get { return verticalDataEditPanel1.MediaObjectDuration; }
            set
            {
                verticalDataEditPanel1.MediaObjectDuration = value;

                trackBar_zoom.Maximum = (int)((value * 1000) + 10000) / verticalDataEditPanel1.Height;

                trackBar_zoom.Value = 5;
                trackBar_zoom_Scroll(this, new EventArgs());
                vScrollBar1.Value = 0;
                verticalDataEditPanel1.ViewPortOffset = 0;
            }
        }
        private bool isZooming;
        private bool isScrolling;
        public event EventHandler SubtitleDoubleClick;
        public event EventHandler SubtitleClick;
        public event EventHandler EditStylesRequest;
        public event EventHandler SaveRequest;
        public int in_view_sub_index;
        /// <summary>
        /// Re-assign the values of the hscroll bar
        /// </summary>
        void UpdateVScroll()
        {
            int max = verticalDataEditPanel1.TimeSpace + 100 - verticalDataEditPanel1.Height;
            if (max < 0)
                max = 1;
            vScrollBar1.Maximum = max;
        }
        public void ScrollToCurrentTime(bool middle = true)
        {
            verticalDataEditPanel1.ViewPortOffset = verticalDataEditPanel1.GetPixelOftime(verticalDataEditPanel1.CurrentTime) - (middle ? (verticalDataEditPanel1.Height / 2) : 0);
            if (verticalDataEditPanel1.ViewPortOffset >= vScrollBar1.Maximum)
                verticalDataEditPanel1.ViewPortOffset = vScrollBar1.Maximum - 1;
            if (verticalDataEditPanel1.ViewPortOffset < 0)
                verticalDataEditPanel1.ViewPortOffset = 0;
            if (verticalDataEditPanel1.ShowWaveForm)
                verticalDataEditPanel1.CalculateWaveFormBuffers();
        }
        public void ScrollToTime(double time, bool middle = true)
        {
            verticalDataEditPanel1.ViewPortOffset = verticalDataEditPanel1.GetPixelOftime(time) - (middle ? (verticalDataEditPanel1.Height / 2) : 0);
            if (verticalDataEditPanel1.ViewPortOffset >= vScrollBar1.Maximum)
                verticalDataEditPanel1.ViewPortOffset = vScrollBar1.Maximum - 1;
            if (verticalDataEditPanel1.ViewPortOffset < 0)
                verticalDataEditPanel1.ViewPortOffset = 0;
            if (verticalDataEditPanel1.ShowWaveForm)
                verticalDataEditPanel1.CalculateWaveFormBuffers();
        }
        void UpdateViewport()
        {
            isZooming = true;
            verticalDataEditPanel1.MilliPixel = trackBar_zoom.Maximum - trackBar_zoom.Value;
            if (verticalDataEditPanel1.MilliPixel < 5)
                verticalDataEditPanel1.MilliPixel = 5;
            verticalDataEditPanel1.TimeSpace = (int)((verticalDataEditPanel1.MediaObjectDuration * 1000) / verticalDataEditPanel1.MilliPixel) + 100;

            UpdateVScroll();

            ScrollToCurrentTime();
            if (verticalDataEditPanel1.ShowWaveForm)
                verticalDataEditPanel1.CalculateWaveFormBuffers();
            isZooming = false;
        }
        public SubtitlesTrack SubtitlesTrack
        {
            get { return verticalDataEditPanel1.SubtitlesTrack; }
            set
            {
                verticalDataEditPanel1.SubtitlesTrack = value;

                //in case we have no media :
                if (value != null && verticalDataEditPanel1.MediaObjectDuration == 0)
                {
                    if (value.Subtitles.Count > 0)
                    {
                        double newDuration = value.Subtitles[value.Subtitles.Count - 1].EndTime;
                        if (verticalDataEditPanel1.MediaObjectDuration != newDuration && newDuration >= 10)
                        {
                            verticalDataEditPanel1.MediaObjectDuration = newDuration;
                            trackBar_zoom.Maximum = (int)((verticalDataEditPanel1.MediaObjectDuration * 1000) + 10000) / verticalDataEditPanel1.Height;
                            trackBar_zoom.Value = 5;
                            UpdateViewport();
                            vScrollBar1.Value = 0;
                            verticalDataEditPanel1.ViewPortOffset = 0;
                        }
                    }
                }
                //timeLine_Panel1.Invalidate();
                //SelectedSubtitles.Clear();
            }
        }
        public List<Subtitle> SelectedSubtitles
        {
            get { return verticalDataEditPanel1.SelectedSubtitles; }
            set
            {
                verticalDataEditPanel1.SelectedSubtitles = value;
            }
        }
        public bool TimerEnable
        {
            get
            {
                return verticalDataEditPanel1.TimerEnabled;
            }
            set
            {
                verticalDataEditPanel1.TimerEnabled = value;
            }
        }
        public event EventHandler SubtitlesSelected;
        public event EventHandler SubtitlePropertiesChanged;

        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            UpdateViewport();
        }
        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (isScrolling)
                return;
            verticalDataEditPanel1.ViewPortOffset = vScrollBar1.Value;
            if (verticalDataEditPanel1.ShowWaveForm)
                verticalDataEditPanel1.CalculateWaveFormBuffers();
        }
        private void verticalDataEditPanel1_RequestScrollToTime(object sender, EventArgs e)
        {
            isScrolling = true;
            if (verticalDataEditPanel1.ViewPortOffset >= 0 && verticalDataEditPanel1.ViewPortOffset <= vScrollBar1.Maximum)
                vScrollBar1.Value = verticalDataEditPanel1.ViewPortOffset;
            isScrolling = false;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            verticalDataEditPanel1.AutoScrollWhenTimePassViewPort = !verticalDataEditPanel1.AutoScrollWhenTimePassViewPort;
            toolStripButton1.Checked = verticalDataEditPanel1.AutoScrollWhenTimePassViewPort;
        }
        private void WaveReader_WFPDLoaded(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                ShowDB();
            else
                this.Invoke(new Action(ShowDB));
        }
        private void WaveReader_WFPDChunckGenerated(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
                ShowDBChunk();
            else
                this.Invoke(new Action(ShowDBChunk));
        }
        private void ShowDBChunk()
        {
            if (WaveReader.BufferPresented)
            {
                verticalDataEditPanel1.ShowWaveForm = true;
                verticalDataEditPanel1.CalculateWaveFormBuffers();
            }
        }
        private void ShowDB()
        {
            //timeLine_Panel1.ShowWaveForm = WaveReader.BufferPresented;
            verticalDataEditPanel1.ShowWaveForm = WaveReader.BufferPresented;
            verticalDataEditPanel1.CalculateWaveFormBuffers();
        }
        private void verticalDataEditPanel1_MoreMillis(object sender, EventArgs e)
        {

            if (trackBar_zoom.Value - 10 >= trackBar_zoom.Minimum)
                trackBar_zoom.Value -= 10;
            else
            {
                trackBar_zoom.Value = trackBar_zoom.Minimum;
            }
            UpdateViewport();
        }
        private void verticalDataEditPanel1_MoreScroll(object sender, EventArgs e)
        {
            int scroll_amm = trackBar_zoom.Value;
            if (vScrollBar1.Value + scroll_amm <= vScrollBar1.Maximum)
                vScrollBar1.Value += scroll_amm;
            else
            {
                vScrollBar1.Value = vScrollBar1.Maximum;
            }
            vScrollBar1_Scroll(this, null);
        }
        private void verticalDataEditPanel1_MoreTime(object sender, EventArgs e)
        {
            double scroll_amm = trackBar_zoom.Value;
            scroll_amm *= 2;
            scroll_amm /= 1000;
            MediaPlayerManager.Position += scroll_amm;

            if ((verticalDataEditPanel1.GetPixelOftime(MediaPlayerManager.Position) > verticalDataEditPanel1.ViewPortOffset + Height) ||
            (verticalDataEditPanel1.GetPixelOftime(MediaPlayerManager.Position) < verticalDataEditPanel1.ViewPortOffset))
                ScrollToCurrentTime(false);
        }
        private void verticalDataEditPanel1_LessTime(object sender, EventArgs e)
        {
            double scroll_amm = trackBar_zoom.Value;
            scroll_amm *= 2;
            scroll_amm /= 1000;
            MediaPlayerManager.Position -= scroll_amm;
            if ((verticalDataEditPanel1.GetPixelOftime(MediaPlayerManager.Position) > verticalDataEditPanel1.ViewPortOffset + Height) ||
                (verticalDataEditPanel1.GetPixelOftime(MediaPlayerManager.Position) < verticalDataEditPanel1.ViewPortOffset))
                ScrollToCurrentTime(false);
        }
        private void verticalDataEditPanel1_LessMillis(object sender, EventArgs e)
        {
            if (trackBar_zoom.Value + 10 <= trackBar_zoom.Maximum)
                trackBar_zoom.Value += 10;
            else
            {
                trackBar_zoom.Value = trackBar_zoom.Maximum;
            }
            UpdateViewport();
        }
        private void verticalDataEditPanel1_LessScroll(object sender, EventArgs e)
        {
            int scroll_amm = trackBar_zoom.Value;
            if (vScrollBar1.Value - scroll_amm >= vScrollBar1.Minimum)
                vScrollBar1.Value -= scroll_amm;
            else
            {
                vScrollBar1.Value = vScrollBar1.Minimum;
            }
            vScrollBar1_Scroll(this, null);
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ScrollToCurrentTime();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton3.Checked = !toolStripButton3.Checked;
            verticalDataEditPanel1.AlwaysScroll = toolStripButton3.Checked;
        }
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (!MediaPlayerManager.IsPlaying)
                MediaPlayerManager.Play();
            else
                MediaPlayerManager.Pause();
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MediaPlayerManager.Stop();
        }
        // Replay selected Subtitle, replay lock
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            toolStripButton_replay_selected.Checked = !toolStripButton_replay_selected.Checked;

            verticalDataEditPanel1.ReplaySelectedSubtitle = toolStripButton_replay_selected.Checked;
        }
        // Play selected subtitle no-replay
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (SubtitlesTrack == null)
                return;
            // if (SelectedSubtitles.Count == 0)
            // {
            // nothing is selected, select the subtitle that currently displayed and play it from it's start.
            in_view_sub_index = -1;
            for (int i = 0; i < SubtitlesTrack.Subtitles.Count; i++)
            {
                if (SubtitlesTrack.Subtitles[i].StartTime <= MediaPlayerManager.Position &&
                    SubtitlesTrack.Subtitles[i].EndTime >= MediaPlayerManager.Position)
                {
                    in_view_sub_index = i;
                    break;
                }
            }
            if (in_view_sub_index >= 0 && in_view_sub_index < SubtitlesTrack.Subtitles.Count)
            {
                Subtitle sub = SubtitlesTrack.Subtitles[in_view_sub_index];
                // verticalDataEditPanel1.SelectSubtitle(verticalDataEditPanel1.subtitle_in_view_index);
                verticalDataEditPanel1.PlayPeriod(sub.StartTime, sub.EndTime);
            }
            /*}
            else if (SelectedSubtitles.Count == 1)
            {
                // One subtitle is selected, simply play it
                verticalDataEditPanel1.PlayPeriod(SelectedSubtitles[0].StartTime, SelectedSubtitles[0].EndTime);
            }
            else
            {
                // Nothing to do ...
            }*/
        }

        private void verticalDataEditPanel1_SubtitlesSelected(object sender, EventArgs e)
        {
            SubtitlesSelected?.Invoke(this, new EventArgs());
        }

        private void verticalDataEditPanel1_SubtitleDoubleClick(object sender, EventArgs e)
        {
            SubtitleDoubleClick?.Invoke(this, new EventArgs());
        }
        private void verticalDataEditPanel1_SubtitleClick(object sender, EventArgs e)
        {
            SubtitleClick?.Invoke(this, new EventArgs());
        }
        private void verticalDataEditPanel1_SubtitlePropertiesChanged(object sender, EventArgs e)
        {
            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(sender, e);
        }
        // Show times
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            toolStripButton5.Checked = !toolStripButton5.Checked;
            ControlsBase.Settings.Vertical_T = toolStripButton5.Checked;
            verticalDataEditPanel1.ShowTimeTicks = toolStripButton5.Checked;
        }
        // Show subtitle-in-view timings
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            toolStripButton6.Checked = !toolStripButton6.Checked;
            ControlsBase.Settings.Vertical_S = toolStripButton6.Checked;
            verticalDataEditPanel1.ShowViewSubtitleTimes = toolStripButton6.Checked;
        }
        private void verticalDataEditPanel1_Resize(object sender, EventArgs e)
        {
            int old = trackBar_zoom.Value;
            trackBar_zoom.Maximum = (int)((MediaPlayerManager.Duration * 1000) + 10000) / verticalDataEditPanel1.Height;
            //trackBar_zoom.Value = 5;
            if (old < trackBar_zoom.Maximum)
                trackBar_zoom.Value = old;

            UpdateViewport();
        }
        private void toolStripButton8_Click_1(object sender, EventArgs e)
        {
            toolStripButton8.Checked = !toolStripButton8.Checked;
            verticalDataEditPanel1.AlwaysShowSubitlesLines = toolStripButton8.Checked;
            ControlsBase.Settings.Vertical_L = toolStripButton8.Checked;
        }
    }
}
