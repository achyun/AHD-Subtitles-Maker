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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.Diagnostics;
using AHD.SM.ASMP;
using AHD.Forms;

namespace AHD.SM.Controls
{
    public partial class TimeLine : UserControl
    {
        public TimeLine()
        {
            resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
            InitializeComponent();
            timeLine_TicksPanel1.TimeChangeRequest += new EventHandler<TimeChangeArgs>(timeLine_TicksPanel1_TimeChangeRequest);

            // Load settings
            toolStripButton_autoScrollWhenChangingTime.Checked = ControlsBase.Settings.Timeline_AutoScroll;

            WaveReader.WFPDLoaded += WaveReader_WFPDLoaded;
            WaveReader.WFPDChunckGenerated += WaveReader_WFPDChunckGenerated;
            comboBox_max_db.Items.Add(10);
            comboBox_max_db.Items.Add(20);
            comboBox_max_db.Items.Add(40);
            comboBox_max_db.Items.Add(90);
            comboBox_max_db.Items.Add(144);
            comboBox_max_db.SelectedItem = 40;
            soundMeter1.MaxDB = (int)comboBox_max_db.SelectedItem;

            toolStripButton15.Checked = ControlsBase.Settings.Timeline_S;
            timeLine_Panel1.ShowInviewSubtitleTime = toolStripButton15.Checked;

            toolStripButton17.Checked = ControlsBase.Settings.Timeline_L;
            timeLine_Panel1.AlwaysShowSubtitleLines = toolStripButton17.Checked;

            toolStripButton12.Checked = ControlsBase.Settings.Timeline_C;
            toolStrip_player.Visible = toolStripButton12.Checked;

            timeLine_Panel1.ShowGridLines = toolStripButton_gridlines.Checked = ControlsBase.Settings.Timeline_G;
        }
        ResourceManager resources;
        int TimeSpace = 1000;//the pixels of time space
        int ViewPortOffset = 0;//the pixel of viewport, view port is the panel_timeline width
        int MilliPixel = 10;//how milli each pixel presents
        double mediaDuration = 0;//this value will be used to calculate the timespace
        bool hasMedia = false;
        bool isPlaying = false;
        bool isZooming = false;
        bool isSelectingTrack = false;
        int waveFormQuality = 5;
        public bool ReplaySelectedSubtitle;
        public int ReplaySelectedSubtitleIndex;
        public double play_period_start;
        public double play_period_end;
        public bool play_period;
        public int subtitle_in_view_index;

        #region Properties
        public Color MediaRectangleColor
        { get { return timeLine_Panel1.MediaRectangleColor; } set { timeLine_Panel1.MediaRectangleColor = value; } }
        public Color MediaHeaderRectangleColor
        { get { return timeLine_Panel1.MediaHeaderRectangleColor; } set { timeLine_Panel1.MediaHeaderRectangleColor = value; } }
        public Color SubtitleSelectedColor
        { get { return timeLine_Panel1.SubtitleSelectedColor; } set { timeLine_Panel1.SubtitleSelectedColor = value; } }
        public Color SubtitleRectangleColor
        { get { return timeLine_Panel1.SubtitleRectangleColor; } set { timeLine_Panel1.SubtitleRectangleColor = value; } }
        public Color SubtitleHeaderRectangleColor
        { get { return timeLine_Panel1.SubtitleHeaderRectangleColor; } set { timeLine_Panel1.SubtitleHeaderRectangleColor = value; } }
        public Color SubtitleStringColor
        { get { return timeLine_Panel1.SubtitleStringColor; } set { timeLine_Panel1.SubtitleStringColor = value; } }
        public Color TimeLineColor
        { get { return timeLine_Panel1.TimeLineColor; } set { timeLine_TicksPanel1.TimeLineColor = timeLine_Panel1.TimeLineColor = value; } }
        public Color ToolTipRectangleColor
        { get { return timeLine_Panel1.ToolTipRectangleColor; } set { timeLine_Panel1.ToolTipRectangleColor = value; } }
        public Color ToolTipTextColor
        { get { return timeLine_Panel1.ToolTipTextColor; } set { timeLine_TicksPanel1.ToolTipTextColor = timeLine_Panel1.ToolTipTextColor = value; } }
        public Color SelectionRectangleColor
        { get { return timeLine_Panel1.SelectionRectangleColor; } set { timeLine_Panel1.SelectionRectangleColor = value; } }
        public Color BackgroundColor
        { get { return timeLine_Panel1.BackgroundColor; } set { timeLine_TicksPanel1.ToolTipColor = timeLine_Panel1.BackgroundColor = value; } }
        public Color SplitersColor
        { get { return timeLine_Panel1.SplitersColor; } set { timeLine_Panel1.SplitersColor = value; } }
        public Color TickColor
        { get { return timeLine_TicksPanel1.TickColor; } set { timeLine_TicksPanel1.TickColor = value; } }
        public Color MarkColor
        { get { return timeLine_TicksPanel1.MarkColor; } set { timeLine_TicksPanel1.MarkColor = value; } }
        public Color TickPanelToolTipColor
        { get { return timeLine_TicksPanel1.ToolTipColor; } set { timeLine_TicksPanel1.ToolTipColor = value; } }
        public bool PreviewMode
        { get { return timeLine_Panel1.PreviewMode; } set { timeLine_Panel1.PreviewMode = timeLine_TicksPanel1.PreviewMode = value; } }
        public int MegnaticAccuracy
        { get { return timeLine_Panel1.MegnaticAccuracy; } set { timeLine_Panel1.MegnaticAccuracy = value; } }
        public int SmartToolAccuracy
        { get { return timeLine_Panel1.SmartToolAccuracy; } set { timeLine_Panel1.SmartToolAccuracy = value; } }
        public int WaveFormQuality
        {
            get { return waveFormQuality; }
            set { waveFormQuality = value; }
        }
        public string MediaText
        {
            get { return timeLine_Panel1.MediaText; }
            set
            {
                //timeLine_Panel1.MediaText = System.IO.Path.GetFileName(value) + " (" + resources.GetString("Text_DoubleClickToShowWaveForm") + ")";
                timeLine_Panel1.MediaText = resources.GetString("Text_DoubleClickToShowWaveForm").ToUpper();
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = value;
                // Clear the waveform
                toolStripButton11.Checked = false;

                toolStripButton11_Click(this, new EventArgs());
            }
        }

        #endregion
        #region Events
        /// <summary>
        /// Rised when the user request a time change
        /// </summary>
        public event EventHandler<TimeChangeArgs> TimeChangeRequest;
        /// <summary>
        /// Rised when the user selecte subtite(s)
        /// </summary>
        public event EventHandler SubtitlesSelected;
        public event EventHandler SubtitleDoubleClick;
        public event EventHandler SubtitlePropertiesChanged;
        /// <summary>
        /// Rised when the user asks to jump into time
        /// </summary>
        public event EventHandler JumpIntoTimeRequest;
        /// <summary>
        /// Rised up when the user asks to add mark
        /// </summary>
        public event EventHandler AddMarkRequest;
        /// <summary>
        /// Rised up when the user asks to remove mark
        /// </summary>
        public event EventHandler RemoveMarkRequest;
        /// <summary>
        /// Rised up when the user asks to jump into mark (selected mark)
        /// </summary>
        public event EventHandler JumpIntoMarkRequest;
        /// <summary>
        /// Rised up when the user asks to select a mark (at given time)
        /// </summary>
        public event EventHandler<MarkEditArgs> SelectMarkRequest;
        /// <summary>
        /// Rised up when the user finished edit a mark
        /// </summary>
        public event EventHandler<MarkEditArgs> MarkEdit;
        /// <summary>
        /// Raised up when the user changes the selection of a mark.
        /// </summary>
        public event EventHandler<MarkSelectionArgs> MarkSelected;
        /// <summary>
        /// Raised to request a track change
        /// </summary>
        public event EventHandler SelectedTrackChanged;
        /// <summary>
        /// Raised to request a media change.
        /// </summary>
        public event EventHandler ChangeMediaRequest;
        /// <summary>
        /// Raised to request a subtitles format file import.
        /// </summary>
        public event EventHandler ImportSubtitlesFormatRequest;
        #endregion
        /// <summary>
        /// Re-assign the values of the hscroll bar
        /// </summary>
        void UpdateHScroll()
        {
            int max = TimeSpace + 100 - timeLine_Panel1.Width;
            if (max < 0)
                max = 1;
            hScrollBar1.Maximum = max;
        }
        public void ScrollToCurrentTime()
        {
            ViewPortOffset = GetPixelOftime(timeLine_TicksPanel1.CurrentTime) - (timeLine_TicksPanel1.Width / 2);
            if (ViewPortOffset >= hScrollBar1.Maximum)
                ViewPortOffset = hScrollBar1.Maximum - 1;
            if (ViewPortOffset < 0)
                ViewPortOffset = 0;
            timeLine_Panel1.ViewPortOffset = timeLine_TicksPanel1.ViewPortOffset =
                hScrollBar1.Value = ViewPortOffset;

            if (timeLine_Panel1.ShowWaveForm)
                timeLine_Panel1.CalculateWaveFormBuffers();
        }
        void UpdateViewport()
        {
            isZooming = true;
            MilliPixel = trackBar_zoom.Maximum - trackBar_zoom.Value;
            if (MilliPixel < 5)
                MilliPixel = 5;
            TimeSpace = (int)((mediaDuration * 1000) / MilliPixel) + 100;
            timeLine_Panel1.TimeSpace = timeLine_TicksPanel1.TimeSpace = TimeSpace;
            timeLine_Panel1.MilliPixel = timeLine_TicksPanel1.MilliPixel = MilliPixel;

            if (timeLine_Panel1.ShowWaveForm)
                timeLine_Panel1.CalculateWaveFormBuffers();

            numericUpDown1.Maximum = trackBar_zoom.Maximum;
            numericUpDown1.Value = MilliPixel;

            UpdateHScroll();

            ScrollToCurrentTime();

            timelineZoomingView1.SetParameters(MilliPixel, TimeSpace,
              timeLine_Panel1.Width, (int)((timeLine_Panel1.MediaObjectDuration * 1000) / MilliPixel));
            timelineZoomingView1.SetOffset(timeLine_Panel1.ViewPortOffset);

            isZooming = false;
        }
        /// <summary>
        /// Calculate time using pixel
        /// </summary>
        /// <param name="x">The pixel to calculate time for, this pixel should be the real pixel of time space, not from view port</param>
        /// <returns>The time in Sec.Milli format</returns>
        double GetTime(int x)
        {
            int tas = TimeSpace * MilliPixel;
            int milli = (x * tas) / TimeSpace;
            return milli / 1000;
        }
        int GetPixelOftime(double time)
        {
            double tas = TimeSpace * MilliPixel;
            return (int)(((time * 1000) * TimeSpace) / tas);
        }
        public void PlayPeriod(double start, double end)
        {
            timeLine_Panel1.play_period_start = play_period_start = start;
            timeLine_Panel1.play_period_end = play_period_end = end;
            play_period = true;
            MediaPlayerManager.Position = start;
            if (!MediaPlayerManager.IsPlaying)
                MediaPlayerManager.Play();

            string tt = "PLAYING SUBTITLE";
            tt += "\n" + TimeFormatConvertor.To_TimeSpan_Milli(play_period_start);
            tt += " -> " + TimeFormatConvertor.To_TimeSpan_Milli(play_period_end);
            if (ReplaySelectedSubtitle)
            {
                tt += "\n[WITH REPLAY]";
            }
            timeLine_Panel1.play_period = true;
            timeLine_Panel1.play_period_text = tt;
        }

        public void UpdateTime(double time)
        {
            if (!isZooming & !timeLine_Panel1.isDrawing)
            {
                linkLabel1.Text = TimeFormatConvertor.To_TimeSpan_Milli(time);
                if (!timeLine_Panel1.isMovingTimeLine)
                {
                    timeLine_TicksPanel1.CurrentTime = time;
                    timeLine_Panel1.CurrentTime = time;
                }
                // link
                if (toolStripButton_link_playback.Checked)
                {
                    if (isPlaying)
                    {
                        ScrollToCurrentTime();
                        timelineZoomingView1.SetOffset(ViewPortOffset);
                    }
                }
                //auto scroll
                else if (toolStripButton_auto_scroll.Checked)
                {
                    if ((GetPixelOftime(time) > ViewPortOffset + timeLine_TicksPanel1.Width) && isPlaying)
                    {
                        ViewPortOffset = GetPixelOftime(time);
                        if (ViewPortOffset >= hScrollBar1.Maximum)
                            ViewPortOffset = hScrollBar1.Maximum - 1;
                        timeLine_Panel1.ViewPortOffset = timeLine_TicksPanel1.ViewPortOffset =
                            hScrollBar1.Value = ViewPortOffset;
                        timelineZoomingView1.SetOffset(ViewPortOffset);

                        if (timeLine_Panel1.ShowWaveForm)
                            timeLine_Panel1.CalculateWaveFormBuffers();
                    }
                }

                timelineZoomingView1.SetTimeline(GetPixelOftime(time));

                timeLine_Panel1.Invalidate();
                timeLine_TicksPanel1.Invalidate();

                if (timeLine_Panel1.ShowWaveForm)
                    soundMeter1.SetTime(time, 92);

                if (play_period)
                {
                    if (!MediaPlayerManager.IsPlaying)
                    {
                        // Not playing anymore... this mean no need to do play-period anymore
                        timeLine_Panel1.play_period = play_period = false;
                    }
                    else if (time < play_period_start)
                        timeLine_Panel1.play_period = play_period = false;// Stop the play-period, the time is not in the sub region
                    else if (time >= play_period_end)
                    {
                        // This is it !!
                        if (ReplaySelectedSubtitle)
                        {
                            MediaPlayerManager.Position = play_period_start;
                        }
                        else
                        {
                            MediaPlayerManager.Pause();
                            timeLine_Panel1.play_period = play_period = false;
                        }
                    }
                }
            }
        }
        public SubtitlesTrack SubtitlesTrack
        {
            get { return timeLine_Panel1.SubtitlesTrack; }
            set
            {
                timeLine_Panel1.SubtitlesTrack = value;
                timelineZoomingView1.OnSubtitlesTrackChanged(value);
                if (timeLine_Panel1.SubtitlesTrack != null)
                {
                    for (int i = 0; i < comboBox1.Items.Count; i++)
                    {
                        if (comboBox1.Items[i].ToString() == timeLine_Panel1.SubtitlesTrack.Name)
                        {
                            isSelectingTrack = true;
                            comboBox1.SelectedIndex = i;
                            isSelectingTrack = false;
                            break;
                        }
                    }
                }
                //in case we have no media :
                if (value != null)
                {
                    if (value.Subtitles.Count > 0)
                    {
                        double trackDuration = value.Subtitles[value.Subtitles.Count - 1].EndTime;
                        if (!hasMedia)
                        {
                            if (mediaDuration != trackDuration && trackDuration >= 10)
                            {
                                mediaDuration = trackDuration;
                                trackBar_zoom.Maximum = (int)((mediaDuration * 1000) + 10000) / timeLine_Panel1.Width;
                                trackBar_zoom.Value = 5;
                                UpdateViewport();
                                hScrollBar1.Value = 0;
                                ViewPortOffset = 0;
                                //update scroll values
                                timeLine_TicksPanel1.ViewPortOffset = 0;
                                //timeLine_TicksPanel1.Invalidate();
                                timeLine_Panel1._mediaObjectDuration = 0;
                                timeLine_Panel1.ViewPortOffset = 0;
                                //timeLine_Panel1.Invalidate();
                            }
                        }
                        else
                        {
                            if (timeLine_Panel1.MediaObjectDuration < trackDuration)
                            {
                                mediaDuration = trackDuration;
                                trackBar_zoom.Maximum = (int)((mediaDuration * 1000) + 10000) / timeLine_Panel1.Width;
                                trackBar_zoom.Value = 5;
                                UpdateViewport();
                                hScrollBar1.Value = 0;
                                ViewPortOffset = 0;
                                //update scroll values
                                timeLine_TicksPanel1.ViewPortOffset = 0;
                                //timeLine_TicksPanel1.Invalidate();
                                timeLine_Panel1._mediaObjectDuration = 0;
                                timeLine_Panel1.ViewPortOffset = 0;
                                //timeLine_Panel1.Invalidate();
                            }
                            else
                            {
                                MediaDuration = timeLine_Panel1.MediaObjectDuration;
                            }
                        }
                    }
                }

                //timeLine_Panel1.Invalidate();
                SelectedSubtitles.Clear();
            }
        }
        public string SelectedTrackInComboboxName
        {
            get
            {
                if (comboBox1.SelectedIndex >= 0)
                    return comboBox1.SelectedItem.ToString();
                return "";
            }
        }
        public List<Subtitle> SelectedSubtitles
        {
            get { return timeLine_Panel1.SelectedSubtitles; }
            set
            {
                timeLine_Panel1.SelectedSubtitles = value;
                //timeLine_Panel1.Invalidate();
                List<bool> selected = new List<bool>();
                for (int i = 0; i < SubtitlesTrack.Subtitles.Count; i++)
                {
                    selected.Add(timeLine_Panel1.IsSelectedSubtitle(SubtitlesTrack.Subtitles[i]));
                }
                timelineZoomingView1.OnSubtitlesSelectionChanged(selected.ToArray());
            }
        }
        public List<TimeMark> TimeMarks
        {
            get { return timeLine_TicksPanel1.timeMarks; }
            set
            {
                timeLine_TicksPanel1.timeMarks = value;
                //timeLine_TicksPanel1.Invalidate();
                if (value != null)
                {
                    RefreshMarks(value.ToArray(), -1);
                }
                else
                {
                    RefreshMarks(new TimeMark[0], -1);
                }
            }
        }
        public double MediaDuration
        {
            get { return timeLine_Panel1.MediaObjectDuration; }
            set
            {
                timeLine_Panel1.MediaObjectDuration = value;
                double newDuration = value;
                if (timeLine_Panel1.SubtitlesTrack != null)
                {
                    if (timeLine_Panel1.SubtitlesTrack.Subtitles.Count > 0)
                    {
                        double trackDuration = timeLine_Panel1.SubtitlesTrack.Subtitles[timeLine_Panel1.SubtitlesTrack.Subtitles.Count - 1].EndTime;
                        if (trackDuration > newDuration)
                            newDuration = trackDuration;
                    }
                }
                if (newDuration != 0)
                { mediaDuration = newDuration; hasMedia = timeLine_Panel1.MediaObjectDuration > 0; timelineZoomingView1.ShowMedia = true; }
                else
                {
                    mediaDuration = 1000; hasMedia = false; timelineZoomingView1.ShowMedia = false;
                    textBox1.ForeColor = Color.Red;
                    textBox1.Text = "N/A";
                }
                trackBar_zoom.Maximum = (int)((mediaDuration * 1000) + 10000) / timeLine_Panel1.Width;

                trackBar_zoom.Value = 5;
                trackBar_zoom_Scroll(this, new EventArgs());
                hScrollBar1.Value = 0;
                ViewPortOffset = 0;
                //update scroll values
                timeLine_TicksPanel1.ViewPortOffset = 0;
                //timeLine_TicksPanel1.Invalidate();

                timeLine_Panel1.ViewPortOffset = 0;
                //timeLine_Panel1.Invalidate();
            }
        }

        /// <summary>
        /// Set if media is playing, required for auto scroll feature
        /// </summary>
        public bool IsPlaying
        { set { isPlaying = value; } }
        public void UpdateSubtitlesReview()
        {
            timelineZoomingView1.RefreshTrack();
        }
        /// <summary>
        /// Refresh all marks on the list
        /// </summary>
        /// <param name="marks">The marks list to refresh with</param>
        /// <param name="index">Current selected mark index. -1 for select nothing.</param>
        public void RefreshMarks(TimeMark[] marks, int index)
        {
            timeLine_TicksPanel1.timeMarks = new List<TimeMark>(marks);
            ComboBox_marks.Items.Clear();
            foreach (TimeMark mark in marks)
            {
                ComboBox_marks.Items.Add(mark.Name + " (" + mark.Time.ToString("F3") + ")");
            }
            SelectMark(index);
        }
        public void SelectMark(int index)
        {
            if (index >= 0 && index < ComboBox_marks.Items.Count)
            {
                ComboBox_marks.SelectedIndex = index;
            }
        }
        public void RefreshSubtitleTracksList(SubtitlesTrack[] tracks)
        {
            comboBox1.Items.Clear();
            for (int i = 0; i < tracks.Length; i++)
            {
                comboBox1.Items.Add(tracks[i].Name);
                if (tracks[i] == timeLine_Panel1.SubtitlesTrack)
                {
                    isSelectingTrack = true;
                    comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
                    isSelectingTrack = false;
                }
            }
        }
        public void ShowWaveform(bool toggleShow)
        {
            if (WaveReader.IsGenerating)
                return;
            if (textBox1.Text.Length == 0)
            {
                return;
            }
            if (!File.Exists(textBox1.Text))
            {
                return;
            }
            if (!toggleShow)
            {
                if (!toolStripButton11.Checked)
                {
                    timeLine_Panel1.ShowWaveForm = false;
                    panel_db.Visible = false;
                    return;
                }
            }
            else
            {
                toolStripButton11.Checked = true;
            }

            // 1 Load the wave file ...
            string buff_file = textBox1.Text.Replace(Path.GetExtension(textBox1.Text), ".wfpa");
            if (File.Exists(buff_file))
            {
                WaveReader.OpenBuffer(buff_file);
            }
            else
            {
                // Generate the buffer
                if (Path.GetExtension(textBox1.Text).ToLower() == ".wav" || Path.GetExtension(textBox1.Text).ToLower() == ".wave")
                {
                    WaveReader.LoadFile(textBox1.Text);// This will generate the buffer
                    // now generate from output
                    System.Threading.Thread myNewThread = new System.Threading.Thread(() => WaveReader.GenerateBuffer(buff_file, 1, true, false, waveFormQuality));
                    myNewThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                    myNewThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                    myNewThread.Start();
                }
                else
                {
                    // Do the operation manually. 
                    // 1 Convert the data using the ffmpeg
                    // ffmpeg -i video.avi -vn soundfile.wav

                    string TempPath = Path.Combine(Path.GetTempPath(), "AHD Subtitles Maker");
                    Directory.CreateDirectory(TempPath);
                    TempPath = Path.Combine(TempPath, "Temp");
                    Directory.CreateDirectory(TempPath);
                    TempPath = TempPath + "\\HIST";
                    Directory.CreateDirectory(TempPath);

                    string outPath = Path.Combine(TempPath, "wave.wav");
                    if (File.Exists(outPath))
                        File.Delete(outPath);
                    Process pr = Process.Start("ffmpeg.exe", "-i " + "\"" + textBox1.Text + "\"" + " -vn " + "\"" + outPath + "\"");
                    // wait for the process
                    while (!pr.HasExited)
                    {

                    }
                    // now generate from output
                    WaveReader.LoadFile(outPath);
                    System.Threading.Thread myNewThread = new System.Threading.Thread(() => WaveReader.GenerateBuffer(buff_file, 1, true, true, waveFormQuality));
                    myNewThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
                    myNewThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                    myNewThread.Start();
                }
            }
        }
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            ViewPortOffset = hScrollBar1.Value;
            //update scroll values
            timeLine_Panel1.ViewPortOffset = ViewPortOffset;
            timeLine_TicksPanel1.ViewPortOffset = ViewPortOffset;
            timelineZoomingView1.SetOffset(ViewPortOffset);

            if (timeLine_Panel1.ShowWaveForm)
                timeLine_Panel1.CalculateWaveFormBuffers();
        }
        private void trackBar_zoom_Scroll(object sender, EventArgs e)
        {
            UpdateViewport();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.Smart;
            timeLine_Panel1.smartEditMode = TimeLineSmartEditMode.None;
            toolStripButton_smart.Checked = true;
            toolStripButton_editStart.Checked = false;
            toolStripButton_move.Checked = false;
            toolStripButton_split.Checked = false;
            toolStripButton_editEnd.Checked = false;
            toolStripButton_selectTool.Checked = false;
        }
        private void toolStripButton_move_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.Move;
            toolStripButton_smart.Checked = false;
            toolStripButton_editStart.Checked = false;
            toolStripButton_move.Checked = true;
            toolStripButton_split.Checked = false;
            toolStripButton_editEnd.Checked = false;
            toolStripButton_selectTool.Checked = false;
        }
        private void toolStripButton_edit_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.EditStartTime;
            toolStripButton_smart.Checked = false;
            toolStripButton_editStart.Checked = true;
            toolStripButton_move.Checked = false;
            toolStripButton_split.Checked = false;
            toolStripButton_editEnd.Checked = false;
            toolStripButton_selectTool.Checked = false;
        }
        private void toolStripButton_split_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.Split;
            toolStripButton_smart.Checked = false;
            toolStripButton_editStart.Checked = false;
            toolStripButton_move.Checked = false;
            toolStripButton_split.Checked = true;
            toolStripButton_editEnd.Checked = false;
            toolStripButton_selectTool.Checked = false;
        }
        private void toolStripButton_editEnd_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.EditEndTime;
            toolStripButton_smart.Checked = false;
            toolStripButton_editStart.Checked = false;
            toolStripButton_move.Checked = false;
            toolStripButton_split.Checked = false;
            toolStripButton_editEnd.Checked = true;
            toolStripButton_selectTool.Checked = false;
        }
        private void toolStripButton_selectTool_Click(object sender, EventArgs e)
        {
            timeLine_Panel1.editMode = TimeLineEditMode.Select;
            toolStripButton_smart.Checked = false;
            toolStripButton_editStart.Checked = false;
            toolStripButton_move.Checked = false;
            toolStripButton_split.Checked = false;
            toolStripButton_editEnd.Checked = false;
            toolStripButton_selectTool.Checked = true;
        }
        private void timeLine_Panel1_SubtitlesSelected(object sender, EventArgs e)
        {
            if (SubtitlesSelected != null)
                SubtitlesSelected(sender, e);
        }
        private void timeLine_Panel1_SubtitleDoubleClick(object sender, EventArgs e)
        {
            if (SubtitleDoubleClick != null)
                SubtitleDoubleClick(sender, e);
        }
        private void timeLine_TicksPanel1_TimeChangeRequest(object sender, TimeChangeArgs e)
        {
            timeLine_TicksPanel1.CurrentTime = timeLine_Panel1.CurrentTime = e.NewTime;
            if (TimeChangeRequest != null)
                TimeChangeRequest(sender, e);
            if (toolStripButton_autoScrollWhenChangingTime.Checked)
                ScrollToCurrentTime();
            //timeLine_Panel1.Invalidate();
            //timeLine_TicksPanel1.Invalidate();
        }
        private void timeLine_Panel1_MovingTimeLine(object sender, EventArgs e)
        {
            timeLine_TicksPanel1.CurrentTime = timeLine_Panel1.CurrentTime;
        }
        private void timeLine_Panel1_SubtitlePropertiesChanged(object sender, EventArgs e)
        {
            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(sender, e);
            for (int i = 0; i < SubtitlesTrack.Subtitles.Count; i++)
                if (timeLine_Panel1.IsSelectedSubtitle(SubtitlesTrack.Subtitles[i]))
                    timelineZoomingView1.OnSubtitlePropertiesChanged(SubtitlesTrack.Subtitles[i], i, true);
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (JumpIntoTimeRequest != null)
                JumpIntoTimeRequest(sender, new EventArgs());
        }
        //set current as track start, shift
        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            if (this.SubtitlesTrack == null)
                return;
            if (this.SubtitlesTrack.Subtitles.Count == 0)
                return;
            double subsstart = this.SubtitlesTrack.Subtitles[0].StartTime;
            double Shift = timeLine_Panel1.CurrentTime - subsstart;
            if (this.SubtitlesTrack.Subtitles[this.SubtitlesTrack.Subtitles.Count - 1].EndTime + Shift > mediaDuration)
            {
                if ((MessageDialog.ShowMessage(this, resources.GetString("Message_TheTimeYouSetWillMakeTheSubtitlesTrackEndAtAPositionMoreThanTheMediaDuration"),
                   resources.GetString("MessageCaption_Warning"), (MessageDialogButtons.Ok | MessageDialogButtons.Checked), MessageDialogIcon.Warning, false,
                    resources.GetString("Button_Ok"), "", "", resources.GetString("Button_Ignore")) & MessageDialogResult.Checked) != MessageDialogResult.Checked)
                {
                    return;
                }
            }
            foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
            {
                sub.StartTime += Shift;
                sub.EndTime += Shift;
            }

            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(this, new EventArgs());
        }
        //set current as track end, shift
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            if (this.SubtitlesTrack == null)
                return;
            if (this.SubtitlesTrack.Subtitles.Count == 0)
                return;
            double subsend = this.SubtitlesTrack.Subtitles[0].EndTime;
            double Shift = timeLine_Panel1.CurrentTime - subsend;
            if (this.SubtitlesTrack.Subtitles[0].StartTime + Shift < 0)
            {
                MessageDialog.ShowMessage(this, resources.GetString("Message_TheTimeYouSetWillMakeTheSubtitlesTrackStartAtAPositionLessThan0"),
                        resources.GetString("MessageCaption_Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
            {
                sub.StartTime += Shift;
                sub.EndTime += Shift;
            }

            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(this, new EventArgs());
        }
        //Set start with compress
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (this.SubtitlesTrack == null)
                return;
            if (this.SubtitlesTrack.Subtitles.Count == 0)
                return;
            double currenttime = timeLine_Panel1.CurrentTime;
            double OrgenalStart = this.SubtitlesTrack.Subtitles[0].StartTime;
            double OrgenalMax = this.SubtitlesTrack.Subtitles[this.SubtitlesTrack.Subtitles.Count - 1].EndTime - this.SubtitlesTrack.Subtitles[0].StartTime;
            double NewMax = this.SubtitlesTrack.Subtitles[this.SubtitlesTrack.Subtitles.Count - 1].EndTime - currenttime;

            if (NewMax <= 0)
            {
                MessageDialog.ShowMessage(this, resources.GetString("Message_TheNewDurationWillBeLessOrEqual0"),
                resources.GetString("MessageCaption_Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }

            foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
            {
                double OldStart = sub.StartTime - OrgenalStart;//- OrgenalStart to return to 0
                double OldEnd = sub.EndTime - OrgenalStart;

                double NewStart = ((OldStart * NewMax) / OrgenalMax);
                double NewEnd = ((OldEnd * NewMax) / OrgenalMax);
                sub.StartTime = NewStart + currenttime;
                sub.EndTime = NewEnd + currenttime;
            }
            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(this, new EventArgs());
        }
        //Set end with compress
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            if (this.SubtitlesTrack == null)
                return;
            if (this.SubtitlesTrack.Subtitles.Count == 0)
                return;
            double currenttime = timeLine_Panel1.CurrentTime;
            double OrgenalStart = this.SubtitlesTrack.Subtitles[0].StartTime;
            double subsend = this.SubtitlesTrack.Subtitles[this.SubtitlesTrack.Subtitles.Count - 1].EndTime;

            double OrgenalMax = this.SubtitlesTrack.Subtitles[this.SubtitlesTrack.Subtitles.Count - 1].EndTime - OrgenalStart;
            double NewMax = currenttime - OrgenalStart;
            if (NewMax <= 0)
            {
                MessageDialog.ShowMessage(this, resources.GetString("Message_TheNewDurationWillBeLessOrEqual0"),
                resources.GetString("MessageCaption_Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
            {
                double OldStart = sub.StartTime - OrgenalStart;//- OrgenalStart to return to 0
                double OldEnd = sub.EndTime - OrgenalStart;

                double NewStart = ((OldStart * NewMax) / OrgenalMax);
                double NewEnd = ((OldEnd * NewMax) / OrgenalMax);

                sub.StartTime = NewStart + OrgenalStart;
                sub.EndTime = NewEnd + OrgenalStart;
            }
            if (SubtitlePropertiesChanged != null)
                SubtitlePropertiesChanged(this, new EventArgs());
        }
        //marks
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            if (AddMarkRequest != null)
                AddMarkRequest(sender, e);
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            if (RemoveMarkRequest != null)
                RemoveMarkRequest(sender, e);
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            if (JumpIntoMarkRequest != null)
                JumpIntoMarkRequest(sender, e);
        }
        private void timeLine_TicksPanel1_MarkEdit(object sender, MarkEditArgs e)
        {
            if (MarkEdit != null)
                MarkEdit(sender, e);
        }
        private void timeLine_TicksPanel1_SelectMarkRequest(object sender, MarkEditArgs e)
        {
            if (SelectMarkRequest != null)
                SelectMarkRequest(sender, e);
        }
        //scroll to current time
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            ScrollToCurrentTime(); timelineZoomingView1.SetOffset(ViewPortOffset);
        }
        private void timeLine_Panel1_ShowToolTipRequest(object sender, ShowToolTipArgs e)
        {
            toolTip1.SetToolTip(timeLine_Panel1, e.Text);
        }
        private void timeLine_Panel1_HideToolTip(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(timeLine_Panel1, "");
        }
        private void numericUpDown1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                trackBar_zoom.Value = (int)(numericUpDown1.Maximum - numericUpDown1.Value);
                UpdateViewport();
            }
            catch { }
        }
        //zoom in/out
        private void timer_zoomIn_Tick(object sender, EventArgs e)
        {
            try
            {
                trackBar_zoom.Value += 5;
                UpdateViewport();
            }
            catch { timer_zoomIn.Stop(); }
        }
        private void toolStripButton3_MouseUp(object sender, MouseEventArgs e)
        {
            timer_zoomIn.Stop();
        }
        private void toolStripButton3_MouseLeave(object sender, EventArgs e)
        {
            timer_zoomIn.Stop();
        }
        private void timer_zoomOut_Tick(object sender, EventArgs e)
        {
            try
            {
                trackBar_zoom.Value -= 5;
                UpdateViewport();
            }
            catch { timer_zoomOut.Stop(); }
        }
        private void toolStripButton4_MouseUp(object sender, MouseEventArgs e)
        {
            timer_zoomOut.Stop();
        }
        private void toolStripButton4_MouseLeave(object sender, EventArgs e)
        {
            timer_zoomOut.Stop();
        }
        private void toolStripButton3_MouseDown_1(object sender, MouseEventArgs e)
        {
            timer_zoomIn.Start();
        }
        private void toolStripButton4_MouseDown(object sender, MouseEventArgs e)
        {
            timer_zoomOut.Start();
        }
        private void toolStripButton_autoScrollWhenChangingTime_CheckedChanged(object sender, EventArgs e)
        {
            ControlsBase.Settings.Timeline_AutoScroll = toolStripButton_autoScrollWhenChangingTime.Checked;
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // 
        }
        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                try
                {
                    trackBar_zoom.Value = (int)(numericUpDown1.Maximum - numericUpDown1.Value);
                    UpdateViewport();
                }
                catch { }
            }
        }
        private void toolStrip1_Enter(object sender, EventArgs e)
        {
            base.OnEnter(e);
        }
        private void timelineZoomingView1_ViewPortXChangeRequest(object sender, ViewPortCoordinateChangeArgs e)
        {
            ViewPortOffset = e.NewValue;
            if (e.NewValue < hScrollBar1.Maximum)
                hScrollBar1.Value = e.NewValue;

            //ViewPortOffset = hScrollBar1.Value = e.NewValue;
            //update scroll values
            timeLine_Panel1.ViewPortOffset = ViewPortOffset;
            timeLine_TicksPanel1.ViewPortOffset = ViewPortOffset;

            if (timeLine_Panel1.ShowWaveForm)
                timeLine_Panel1.CalculateWaveFormBuffers();
        }
        private void timelineZoomingView1_ViewPortWidthChangeRequest(object sender, ViewPortCoordinateChangeArgs e)
        {
            int newMillipixelValue = (e.NewValue * MilliPixel) / timeLine_Panel1.Width;

            if (newMillipixelValue > 0 && newMillipixelValue <= numericUpDown1.Maximum)
            {
                numericUpDown1.Value = newMillipixelValue;

                try
                {
                    trackBar_zoom.Value = (int)(numericUpDown1.Maximum - numericUpDown1.Value);
                    UpdateViewport();
                }
                catch { }
            }
        }
        private void timeLine_Panel1_ViewPortXChanged(object sender, EventArgs e)
        {
            // ViewPortOffset = timeLine_Panel1.ViewPortOffset;
            //update scroll values
            //hScrollBar1.Value = ViewPortOffset;

            ViewPortOffset = timeLine_Panel1.ViewPortOffset;
            if (ViewPortOffset < hScrollBar1.Maximum)
                hScrollBar1.Value = ViewPortOffset;

            timeLine_TicksPanel1.ViewPortOffset = ViewPortOffset;
            timelineZoomingView1.SetOffset(ViewPortOffset);
        }
        private void ComboBox_marks_DropDownClosed(object sender, EventArgs e)
        {
            if (MarkSelected != null)
                MarkSelected(this, new MarkSelectionArgs(ComboBox_marks.SelectedIndex));
        }
        // ON RESIZING
        private void TimeLine_Resize(object sender, EventArgs e)
        {
            // Update zooming
            /*
            int old = trackBar_zoom.Value;
            trackBar_zoom.Maximum = (int)((mediaDuration * 1000) + 10000) / timeLine_Panel1.Width;
            //trackBar_zoom.Value = 5;
            if (old < trackBar_zoom.Maximum)
                trackBar_zoom.Value = old;
            UpdateViewport();
            //hScrollBar1.Value = 0;
            //ViewPortOffset = 0;
            //update scroll values
            timeLine_TicksPanel1.ViewPortOffset = 0;
            timeLine_Panel1._mediaObjectDuration = 0;
            timeLine_Panel1.ViewPortOffset = 0;*/
        }
        private void timeLine_Panel1_Resize(object sender, EventArgs e)
        {
            int old = trackBar_zoom.Value;
            trackBar_zoom.Maximum = (int)((mediaDuration * 1000) + 10000) / timeLine_Panel1.Width;
            //trackBar_zoom.Value = 5;
            if (old < trackBar_zoom.Maximum)
                trackBar_zoom.Value = old;

            UpdateViewport();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isSelectingTrack)
                return;
            SelectedTrackChanged?.Invoke(this, new EventArgs());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ChangeMediaRequest?.Invoke(this, new EventArgs());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ImportSubtitlesFormatRequest?.Invoke(this, new EventArgs());
        }
        private void toolStripButton_link_playback_CheckedChanged(object sender, EventArgs e)
        {
            toolStripButton_auto_scroll.Enabled = !toolStripButton_link_playback.Checked;
        }
        private void toolStripButton_gridlines_CheckedChanged(object sender, EventArgs e)
        {
            ControlsBase.Settings.Timeline_G = timeLine_Panel1.ShowGridLines = toolStripButton_gridlines.Checked;
        }
        private void timeLine_Panel1_MoreMillis(object sender, EventArgs e)
        {
            if (trackBar_zoom.Value + 10 <= trackBar_zoom.Maximum)
                trackBar_zoom.Value += 10;
            else
            {
                trackBar_zoom.Value = trackBar_zoom.Maximum;
            }
            UpdateViewport();
        }
        private void timeLine_Panel1_MoreScroll(object sender, EventArgs e)
        {
            int scroll_amm = trackBar_zoom.Value;
            if (hScrollBar1.Value - scroll_amm >= hScrollBar1.Minimum)
                hScrollBar1.Value -= scroll_amm;
            else
            {
                hScrollBar1.Value = trackBar_zoom.Minimum;
            }
            hScrollBar1_Scroll(this, null);
        }
        private void timeLine_Panel1_MoreTime(object sender, EventArgs e)
        {
            double scroll_amm = trackBar_zoom.Value;
            scroll_amm *= 2;
            scroll_amm /= 1000;
            MediaPlayerManager.Position -= scroll_amm;
        }
        private void timeLine_Panel1_LessTime(object sender, EventArgs e)
        {
            double scroll_amm = trackBar_zoom.Value;
            scroll_amm *= 2;
            scroll_amm /= 1000;
            MediaPlayerManager.Position += scroll_amm;
        }
        private void timeLine_Panel1_LessMillis(object sender, EventArgs e)
        {
            if (trackBar_zoom.Value - 10 >= trackBar_zoom.Minimum)
                trackBar_zoom.Value -= 10;
            else
            {
                trackBar_zoom.Value = trackBar_zoom.Minimum;
            }
            UpdateViewport();
        }
        private void timeLine_Panel1_LessScroll(object sender, EventArgs e)
        {
            int scroll_amm = trackBar_zoom.Value;
            if (hScrollBar1.Value + scroll_amm <= hScrollBar1.Maximum)
                hScrollBar1.Value += scroll_amm;
            else
            {
                hScrollBar1.Value = hScrollBar1.Maximum;
            }
            hScrollBar1_Scroll(this, null);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            trackBar_zoom.Value = trackBar_zoom.Maximum;
            UpdateViewport();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            trackBar_zoom.Value = trackBar_zoom.Minimum;
            UpdateViewport();
        }
        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            timeLine_Panel1.SpliterY = splitContainer2.SplitterDistance;
            if (timeLine_Panel1.ShowWaveForm)
                timeLine_Panel1.CalculateWaveFormBuffers();
            ControlsBase.Settings.Timeline_Spl2 = splitContainer2.SplitterDistance;
        }
        // Generate and show wave form
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            ShowWaveform(false);
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
            if (toolStripButton11.Checked & WaveReader.BufferPresented)
            {
                timeLine_Panel1.ShowWaveForm = true;
                timeLine_Panel1.CalculateWaveFormBuffers();
                panel_db.Visible = timeLine_Panel1.ShowWaveForm;
            }
        }
        private void ShowDB()
        {
            //timeLine_Panel1.ShowWaveForm = WaveReader.BufferPresented;
            timeLine_Panel1.ShowWaveForm = toolStripButton11.Checked && WaveReader.BufferPresented;
            panel_db.Visible = timeLine_Panel1.ShowWaveForm;
            timeLine_Panel1.CalculateWaveFormBuffers();
        }
        private void toolStripButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (WaveReader.IsGenerating)
                return;
            panel_db.Visible = timeLine_Panel1.ShowWaveForm;
        }
        private void comboBox_max_db_SelectedIndexChanged(object sender, EventArgs e)
        {
            soundMeter1.MaxDB = (int)comboBox_max_db.SelectedItem;
        }
        private void splitContainer3_SplitterMoved(object sender, SplitterEventArgs e)
        {
            timeLine_Panel1.SubtitleHeight = splitContainer3.SplitterDistance;
            ControlsBase.Settings.Timeline_Spl3 = splitContainer3.SplitterDistance;
        }
        private void timeLine_Panel1_ShowWaveFormRequest(object sender, EventArgs e)
        {
            toolStripButton11.Checked = !toolStripButton11.Checked;
            toolStripButton11_Click(this, new EventArgs());
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            toolStripButton12.Checked = !toolStripButton12.Checked;
            toolStrip_player.Visible = toolStripButton12.Checked;
            ControlsBase.Settings.Timeline_C = toolStripButton12.Checked;
        }
        // Play / Pause
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            if (MediaPlayerManager.IsPlaying)
                MediaPlayerManager.Pause();
            else
                MediaPlayerManager.Play();
        }
        // Play period
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            if (SelectedSubtitles.Count > 0)
            {
                if (play_period)
                {
                    timeLine_Panel1.play_period = play_period = false;
                }
                else
                    PlayPeriod(SelectedSubtitles[0].StartTime, SelectedSubtitles[SelectedSubtitles.Count - 1].EndTime);
            }
        }
        // Replay period
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            toolStripButton_replay_period.Checked = !toolStripButton_replay_period.Checked;
            ReplaySelectedSubtitle = toolStripButton_replay_period.Checked;
            string tt = "PLAYING SUBTITLE";
            tt += "\n" + TimeFormatConvertor.To_TimeSpan_Milli(play_period_start);
            tt += " -> " + TimeFormatConvertor.To_TimeSpan_Milli(play_period_end);
            if (ReplaySelectedSubtitle)
            {
                tt += "\n[WITH REPLAY]";
            }
            timeLine_Panel1.play_period_text = tt;
        }
        // Stop
        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            MediaPlayerManager.Stop();
        }

        private void toolStripButton15_Click_1(object sender, EventArgs e)
        {
            toolStripButton15.Checked = !toolStripButton15.Checked;
            ControlsBase.Settings.Timeline_S = toolStripButton15.Checked;
            timeLine_Panel1.ShowInviewSubtitleTime = toolStripButton15.Checked;
        }
        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            toolStripButton17.Checked = !toolStripButton17.Checked;
            ControlsBase.Settings.Timeline_L = toolStripButton17.Checked;
            timeLine_Panel1.AlwaysShowSubtitleLines = toolStripButton17.Checked;
        }
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            ControlsBase.Settings.Timeline_Spl1 = splitContainer1.SplitterDistance;
        }
        private void TimeLine_Load(object sender, EventArgs e)
        {

            splitContainer3.SplitterDistance = ControlsBase.Settings.Timeline_Spl3;
            splitContainer2.SplitterDistance = ControlsBase.Settings.Timeline_Spl2;
            splitContainer1.SplitterDistance = ControlsBase.Settings.Timeline_Spl1;

            timeLine_Panel1.SpliterY = splitContainer2.SplitterDistance;
            timeLine_Panel1.SubtitleHeight = splitContainer3.SplitterDistance;
        }
    }
}