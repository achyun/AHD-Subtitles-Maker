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
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class VerticalDataEditPanel : Control
    {
        public VerticalDataEditPanel()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);

            TimingPanelWidth = 80;

            TimingPanelColor = Color.White;
            TimingPanelBackgroundColor = Color.MediumAquamarine;
            TickColor = Color.White;
            TimeLineColor = Color.White;
            WaveformColor = Color.Black;
            showWaveForm = false;
            mono_pos_buffer = new List<int>();
            mono_neg_buffer = new List<int>();
            left_pos_buffer = new List<int>();
            left_neg_buffer = new List<int>();
            right_pos_buffer = new List<int>();
            right_neg_buffer = new List<int>();

            stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Trimming = StringTrimming.EllipsisWord;

            SubtitleSelectedColor = Color.Purple;
            SubtitleColor = Color.DodgerBlue;
            SubtitleHeaderColor = Color.RoyalBlue;
            SubtitleTextColor = Color.White;

            ShowTimeTicks = false;
            ShowViewSubtitleTimes = true;
        }
        Point DownPoint = new Point();
        Point DownPointToTimeSpace = new Point();
        bool save;
        public bool ShowTimeTicks;
        public bool ShowViewSubtitleTimes;
        // Timing panel split
        public int TimingPanelWidth;
        private Color t_panel_color;
        private Pen t_panel_pen;
        public Color TimingPanelColor
        {
            get { return t_panel_color; }
            set
            {
                if (t_panel_color != value)
                {
                    t_panel_color = value;
                    t_panel_pen = new Pen(new SolidBrush(value));
                    Invalidate();
                }
            }
        }
        // Timing panel background
        private Color t_panel_bk_color;
        private Brush t_panel_bk_brsh;
        public Color TimingPanelBackgroundColor
        {
            get { return t_panel_bk_color; }
            set
            {
                if (t_panel_bk_color != value)
                {
                    t_panel_bk_color = value;
                    t_panel_bk_brsh = new SolidBrush(value);
                    Invalidate();
                }
            }
        }
        // Timing panel ticks
        private Color tick_color;
        private Brush tick_brush;
        private Pen tick_pen;
        public Color TickColor
        {
            get { return tick_color; }
            set
            {
                if (tick_color != value)
                {
                    tick_color = value;
                    tick_pen = new Pen(tick_brush = new SolidBrush(value), 2);
                    Invalidate();
                }
            }
        }
        // Time Line
        private Color timeline_color;
        private Pen timeline_pen;
        public Color TimeLineColor
        {
            get { return tick_color; }
            set
            {
                if (timeline_color != value)
                {
                    timeline_color = value;
                    timeline_pen = new Pen(new SolidBrush(value), 2);
                    Invalidate();
                }
            }
        }
        public double MediaObjectDuration
        {
            get { return _mediaObjectDuration; }
            set { _mediaObjectDuration = value; }
        }
        public int TimeSpace = 1000;//the pixels of time space
        public int ViewPortOffset = 0;//the pixel of viewport, view port is the panel_timeline width
        public int MilliPixel = 10;//how milli each pixel presents
        public double _mediaObjectDuration = 0;//this is the real media duration, represents the media object
        public double CurrentTime;
        public bool AutoScrollWhenTimePassViewPort;
        public bool AlwaysScroll;
        public bool ReplaySelectedSubtitle;
        public int ReplaySelectedSubtitleIndex;
        public double play_period_start;
        public double play_period_end;
        public bool play_period;
        public int subtitle_in_view_index;
        public int CursorSensitivity = 5;
        public bool AlwaysShowSubitlesLines;

        public event EventHandler RequestScrollToTime;
        public event EventHandler MoreMillis;
        public event EventHandler LessMillis;
        public event EventHandler MoreScroll;
        public event EventHandler LessScroll;
        public event EventHandler MoreTime;
        public event EventHandler LessTime;

        // Waveform
        private Color wf_color;
        private Pen wf_pen;
        public Color WaveformColor
        {
            get { return wf_color; }
            set
            {
                if (wf_color != value)
                {
                    wf_color = value;
                    wf_pen = new Pen(new SolidBrush(value), 1);
                    Invalidate();
                }
            }
        }

        private bool showWaveForm;
        private bool media_buffersReady;
        private List<int> mono_pos_buffer;
        private List<int> mono_neg_buffer;
        private List<int> left_pos_buffer;
        private List<int> left_neg_buffer;
        private List<int> right_pos_buffer;
        private List<int> right_neg_buffer;
        internal bool ShowWaveForm
        {
            get { return showWaveForm; }
            set
            {
                if (showWaveForm != value)
                {
                    showWaveForm = value;
                    if (showWaveForm)
                    {
                        CalculateWaveFormBuffers();
                        Invalidate();
                    }
                }
            }
        }
        public bool TimerEnabled
        {
            get
            {
                return timer1.Enabled;
            }
            set
            {
                //  timer1.Enabled = value;
                if (value)
                    timer1.Start();
                else
                    timer1.Stop();
            }
        }
        public SubtitlesTrack SubtitlesTrack;
        private bool isEditingSubtitle;
        private bool isEditingVSplit;
        private bool isMovingTimeline;
        public List<Subtitle> SelectedSubtitles = new List<Subtitle>();
        List<Subtitle> TempSelectedSubtitles = new List<Subtitle>();
        /// <summary>
        /// Rised when the user selecte subtite(s)
        /// </summary>
        public event EventHandler SubtitlesSelected;
        public event EventHandler SubtitleDoubleClick;
        public event EventHandler SubtitleClick;
        public event EventHandler MovingTimeLine;
        public event EventHandler ViewPortYChanged;
        public event EventHandler SubtitlePropertiesChanged;
        // Subtitle
        private StringFormat stringFormat = new StringFormat();
        private Color sub_color;
        private Brush sub_brush;
        public Color SubtitleColor
        {
            get { return sub_color; }
            set
            {
                if (sub_color != value)
                {
                    sub_color = value;
                    sub_brush = new SolidBrush(value);
                    Invalidate();
                }
            }
        }
        private Color sub_select_color;
        private Brush sub_select_brush;
        public Color SubtitleSelectedColor
        {
            get { return sub_select_color; }
            set
            {
                if (sub_select_color != value)
                {
                    sub_select_color = value;
                    sub_select_brush = new SolidBrush(value);
                    Invalidate();
                }
            }
        }
        private Color sub_header_color;
        private Brush sub_header_brush;
        public Color SubtitleHeaderColor
        {
            get { return sub_header_color; }
            set
            {
                if (sub_header_color != value)
                {
                    sub_header_color = value;
                    sub_header_brush = new SolidBrush(value);
                    Invalidate();
                }
            }
        }
        private Color sub_text_color;
        private Brush sub_text_brush;
        public Color SubtitleTextColor
        {
            get { return sub_text_color; }
            set
            {
                if (sub_text_color != value)
                {
                    sub_text_color = value;
                    sub_text_brush = new SolidBrush(value);
                    Invalidate();
                }
            }
        }
        enum EditMode
        {
            None, Timeline, StartTime, EndTime, Move, MoveVSpliter
        }
        private EditMode edit_mode;

        Subtitle GetSutitleByTime(double time)
        {
            return GetSutitleByTime(time, null);
        }
        Subtitle GetSutitleByTime(double time, Subtitle ignoreThis)
        {
            if (this.SubtitlesTrack != null)
            {
                foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
                {
                    if (sub != ignoreThis)
                    {
                        if (sub.StartTime <= time & sub.EndTime >= time)
                        {
                            return sub;
                        }
                    }
                }
            }
            return null;
        }
        int GetSubtitleIndex(Subtitle subtitle)
        {
            if (this.SubtitlesTrack != null)
            {
                int i = 0;
                foreach (Subtitle sub in this.SubtitlesTrack.Subtitles)
                {
                    if (sub.StartTime == subtitle.StartTime)
                    {
                        return i;
                    }
                    i++;
                }
            }
            return -1;
        }
        public bool IsSelectedSubtitle(Subtitle sub)
        {
            if (SelectedSubtitles.Count == 0)
                return false;
            foreach (Subtitle sl in SelectedSubtitles)
            {
                if (sl == sub)
                    return true;
            }
            return false;
        }
        public void PlayPeriod(double start, double end)
        {
            if (play_period)
            {
                play_period = false;
                return;
            }
            play_period_start = start;
            play_period_end = end;
            play_period = true;
            MediaPlayerManager.Position = start;
            if (!MediaPlayerManager.IsPlaying)
                MediaPlayerManager.Play();
        }
        /// <summary>
        /// Calculate time using pixel
        /// </summary>
        /// <param name="x">The pixel to calculate time for, this pixel should be the real pixel of time space, not from view port</param>
        /// <returns>The time in Sec.Milli format</returns>
        double GetTime(int y)
        {
            // double tas = TimeSpace * MilliPixel;
            //double milli = (x * tas) / TimeSpace;

            //return milli / 1000;
            return ((double)MilliPixel * (double)y) / 1000;
        }
        public int GetPixelOftime(double time)
        {
            double tas = TimeSpace * MilliPixel;
            return (int)(((time * 1000) * TimeSpace) / tas);
        }
        public void CalculateWaveFormBuffers()
        {
            media_buffersReady = false;
            if (WaveReader.BufferNmChannel == 1)
            {
                mono_pos_buffer.Clear();
                mono_neg_buffer.Clear();
            }
            else
            {
                left_pos_buffer.Clear();
                left_neg_buffer.Clear();
                right_pos_buffer.Clear();
                right_neg_buffer.Clear();
            }

            if (WaveReader.BufferPresented)
            {
                for (int x = 0; x < Height; x++)
                {
                    // 1 get time
                    double time = GetTime(x + ViewPortOffset);
                    // 2 get sample at that time

                    if (WaveReader.BufferNmChannel == 1)
                    {
                        // MONO         
                        int samplePOS, sampleNEG = 0;
                        int channelHeight = (TimingPanelWidth / 2);
                        WaveReader.GetPixelValuesFromBufferMONO(time, MilliPixel, out samplePOS, out sampleNEG);

                        mono_pos_buffer.Add(WaveReader.ScaleBufferPixel(samplePOS, channelHeight));
                        mono_neg_buffer.Add(WaveReader.ScaleBufferPixel(-sampleNEG, channelHeight));
                    }
                    else
                    {
                        // Stereo
                        // We need to draw both left and right channels.
                        // Start with the left channel

                        int sampleLeftPOS, sampleLeftNEG, sampleRightPOS, sampleRightNEG = 0;
                        WaveReader.GetPixelValuesFromBufferSTEREO(time, MilliPixel, out sampleLeftPOS, out sampleLeftNEG, out sampleRightPOS, out sampleRightNEG);
                        // Add values with scalling ...
                        int channelHeight = (TimingPanelWidth / 4);
                        // Left sample is above
                        left_pos_buffer.Add(WaveReader.ScaleBufferPixel(sampleLeftPOS, channelHeight));
                        left_neg_buffer.Add(-WaveReader.ScaleBufferPixel(sampleLeftNEG, channelHeight));

                        // Right sample is down
                        right_pos_buffer.Add(WaveReader.ScaleBufferPixel(sampleRightPOS, channelHeight));
                        right_neg_buffer.Add(-WaveReader.ScaleBufferPixel(sampleRightNEG, channelHeight));
                    }
                }
                media_buffersReady = true;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            // Draw vertical line representing the splitter between timing panel and subtitles panel
            pe.Graphics.DrawLine(t_panel_pen, TimingPanelWidth, 0, TimingPanelWidth, Height);
            // Draw timing panel background
            pe.Graphics.FillRectangle(t_panel_bk_brsh, new Rectangle(0, 0, TimingPanelWidth, Height));
            int channelHeight = (TimingPanelWidth / 4);
            int channelHalf = (TimingPanelWidth / 2);
            if (media_buffersReady)
            {
                if (WaveReader.BufferNmChannel == 1)
                {
                    pe.Graphics.DrawLine(wf_pen, channelHalf, 0, channelHalf, Height);
                }
                else
                {
                    pe.Graphics.DrawLine(wf_pen, channelHeight, 0, channelHeight, Height);
                    pe.Graphics.DrawLine(wf_pen, channelHeight + channelHalf, 0, channelHeight + channelHalf, Height);
                }
                for (int y = 0; y < Height; y++)
                {
                    // 2 get sample at that time
                    if (WaveReader.BufferNmChannel == 1)
                    {
                        if (y < mono_pos_buffer.Count)
                        {
                            pe.Graphics.DrawLine(wf_pen, channelHalf - mono_pos_buffer[y], y, channelHalf, y);
                            pe.Graphics.DrawLine(wf_pen, channelHalf + -mono_neg_buffer[y], y, channelHalf, y);
                        }
                    }
                    else
                    {
                        // Stereo
                        // We need to draw both left and right channels.
                        // Start with the left channel
                        // Left sample is above
                        if (y < left_pos_buffer.Count)
                        {
                            pe.Graphics.DrawLine(wf_pen, channelHeight - left_pos_buffer[y], y, channelHeight, y);
                            pe.Graphics.DrawLine(wf_pen, channelHeight + -left_neg_buffer[y], y, channelHeight, y);

                        }
                        if (y < right_pos_buffer.Count)
                        {
                            // Right sample is down
                            pe.Graphics.DrawLine(wf_pen, channelHeight - right_pos_buffer[y] + channelHalf, y, channelHeight + channelHalf, y);
                            pe.Graphics.DrawLine(wf_pen, channelHeight + -right_neg_buffer[y] + channelHalf, y, channelHeight + channelHalf, y);
                        }
                    }
                }
            }

            // Draw ticks
            //calculate tick space
            int ticPixels = (1000 / MilliPixel);
            ticPixels = ((ticPixels % 10) + 10);
            int secPixels = ticPixels * 10;
            int time_pixel = GetPixelOftime(CurrentTime) - ViewPortOffset;
            //Draw ticks
            for (int y = 0; y < this.Height; y++)
            {
                //calculate which pixel we are at in the TimeSpace
                int pix = ViewPortOffset + y;
                //each ticksSpace pixels, draw small tick
                if (pix % ticPixels == 0)
                {
                    pe.Graphics.DrawLine(tick_pen, TimingPanelWidth - 5, y, TimingPanelWidth, y);
                }
                //each secPixels pixels, draw big tick and time
                if (pix % secPixels == 0)
                {
                    pe.Graphics.DrawLine(tick_pen, TimingPanelWidth - 8, y, TimingPanelWidth, y);
                    if (ShowTimeTicks)
                    {
                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(pix));
                        Size ss = TextRenderer.MeasureText(time, Font);
                        pe.Graphics.DrawString(time, Font, tick_brush, new PointF(TimingPanelWidth - (ss.Width + 4), y - (ss.Height / 2)));
                    }
                }
            }
            // Draw subtitles
            if (this.SubtitlesTrack != null)
            {
                Subtitle sub = null;
                int subIndex = -1;
                for (int i = ViewPortOffset; i < ViewPortOffset + this.Height; i++)
                {
                    sub = GetSutitleByTime(GetTime(i));
                    if (sub != null)
                    { subIndex = GetSubtitleIndex(sub); break; }
                }
                //if a subtitle found, draw using subtitles loop instead of pixels loop
                if (subIndex >= 0)
                {
                    while (subIndex < this.SubtitlesTrack.Subtitles.Count)
                    {
                        Subtitle currentSub = this.SubtitlesTrack.Subtitles[subIndex];
                        int subY = GetPixelOftime(currentSub.StartTime) - ViewPortOffset;
                        int subY1 = GetPixelOftime(currentSub.EndTime) - ViewPortOffset;
                        if (AlwaysShowSubitlesLines)
                        {
                            pe.Graphics.DrawLine(Pens.White, 0, subY, Width, subY);
                            pe.Graphics.DrawLine(Pens.White, 0, subY1, Width, subY1);
                        }
                        //check if we should break 'cause we need not to draw subtitles that not in range
                        if (subY >= this.Height)
                            break;
                        if (subY1 - subY > 0)
                        {
                            if (IsSelectedSubtitle(currentSub))
                            {
                                pe.Graphics.FillRectangle(sub_select_brush, TimingPanelWidth + 1, subY, Width - TimingPanelWidth - 2, subY1 - subY);
                                if (isEditingSubtitle)
                                {
                                    string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subY + ViewPortOffset));
                                    Size textSize = TextRenderer.MeasureText(time, this.Font);

                                    pe.Graphics.DrawLine(Pens.White, 0, subY, Width, subY);
                                    pe.Graphics.FillRectangle(Brushes.Blue, 3, subY - (textSize.Height + 2), textSize.Width, textSize.Height);
                                    pe.Graphics.DrawRectangle(Pens.Black, 3, subY - (textSize.Height + 2), textSize.Width, textSize.Height);
                                    pe.Graphics.DrawString(time, this.Font, Brushes.White, 4, subY - (textSize.Height + 1));

                                    time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subY1 + ViewPortOffset));
                                    textSize = TextRenderer.MeasureText(time, this.Font);

                                    pe.Graphics.DrawLine(Pens.White, 0, subY1, Width, subY1);
                                    pe.Graphics.FillRectangle(Brushes.Blue, 3, subY1 + 2, textSize.Width, textSize.Height);
                                    pe.Graphics.DrawRectangle(Pens.Black, 3, subY1 + 2, textSize.Width, textSize.Height);
                                    pe.Graphics.DrawString(time, this.Font, Brushes.White, 4, subY1 + 3);
                                }
                            }
                            else
                                pe.Graphics.FillRectangle(sub_brush, TimingPanelWidth + 1, subY, Width - TimingPanelWidth - 2, subY1 - subY);

                            // Draw surrounding rectangle
                            if (time_pixel >= subY && time_pixel <= subY1)
                            {
                                pe.Graphics.DrawRectangle(new Pen(Brushes.White, 2), new Rectangle(TimingPanelWidth + 1, subY, Width - TimingPanelWidth - 2, subY1 - subY));

                                subtitle_in_view_index = subIndex;

                                if (ShowViewSubtitleTimes)
                                {
                                    string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subY + ViewPortOffset));
                                    Size textSize = TextRenderer.MeasureText(time, this.Font);
                                    pe.Graphics.DrawLine(tick_pen, 0, subY, Width, subY);

                                    pe.Graphics.FillRectangle(Brushes.Blue, 3, subY - (textSize.Height + 2), textSize.Width, textSize.Height);
                                    pe.Graphics.DrawRectangle(Pens.Black, 3, subY - (textSize.Height + 2), textSize.Width, textSize.Height);
                                    pe.Graphics.DrawString(time, this.Font, Brushes.White, 4, subY - (textSize.Height + 1));

                                    time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subY1 + ViewPortOffset));
                                    textSize = TextRenderer.MeasureText(time, this.Font);

                                    pe.Graphics.DrawLine(tick_pen, 0, subY1, Width, subY1);
                                    pe.Graphics.FillRectangle(Brushes.Blue, 3, subY1 + 2, textSize.Width, textSize.Height);
                                    pe.Graphics.DrawRectangle(Pens.Black, 3, subY1 + 2, textSize.Width, textSize.Height);
                                    pe.Graphics.DrawString(time, this.Font, Brushes.White, 4, subY1 + 3);
                                }
                            }
                            else
                            {
                                pe.Graphics.DrawRectangle(Pens.LightGray, new Rectangle(TimingPanelWidth + 1, subY, Width - TimingPanelWidth - 2, subY1 - subY));
                            }
                            //Draw string
                            string[] lines = currentSub.Text.Lines;
                            int lineY = 1;
                            int SubtitleHeight = subY1 - subY - 2;
                            foreach (string line in lines)
                            {
                                if (lineY < SubtitleHeight && lineY + 13 < SubtitleHeight)
                                    pe.Graphics.DrawString(line, new Font("Tahoma", 8, FontStyle.Regular), sub_text_brush,
                                    new RectangleF(TimingPanelWidth + 2, subY + lineY, Width - TimingPanelWidth - 2, 13), stringFormat);
                                else
                                    break;
                                lineY += 15;
                            }
                        }
                        subIndex++;
                    }
                }

            }
            int timeY = 0;
            //Draw time line
            timeY = GetPixelOftime(CurrentTime);
            if (timeY >= ViewPortOffset)
            {
                string time = TimeFormatConvertor.To_TimeSpan_Milli(CurrentTime);
                Size ss = TextRenderer.MeasureText(time, Font);
                pe.Graphics.DrawString(time, Font, tick_brush, new PointF(4, timeY - (ss.Height + 2) - ViewPortOffset));
                pe.Graphics.DrawLine(timeline_pen, 4, timeY - ViewPortOffset, Width, timeY - ViewPortOffset);
            }
            // Draw play-period indicater
            if (play_period)
            {
                string tt = "PLAYING SUBTITLE";
                tt += "\n" + TimeFormatConvertor.To_TimeSpan_Milli(play_period_start);
                tt += " -> " + TimeFormatConvertor.To_TimeSpan_Milli(play_period_end);
                if (ReplaySelectedSubtitle)
                {
                    tt += "\n[WITH REPLAY]";
                }
                Size ss = TextRenderer.MeasureText(tt, Font);
                pe.Graphics.FillRectangle(Brushes.Black, new Rectangle(1, 1, ss.Width + 1, ss.Height + 1));
                pe.Graphics.DrawString(tt, Font, Brushes.Lime, new Point(2, 2));
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            bool shift_holded = (ModifierKeys & Keys.Shift) == Keys.Shift;
            bool control_holded = (ModifierKeys & Keys.Control) == Keys.Control;

            if (e.Delta > 0)
            {
                if (control_holded)
                {
                    LessMillis?.Invoke(this, new EventArgs());
                }
                else if (shift_holded)
                {
                    LessTime.Invoke(this, new EventArgs());
                }
                else
                {
                    LessScroll?.Invoke(this, new EventArgs());
                }
            }
            if (e.Delta < 0)
            {
                if (control_holded)
                {
                    MoreMillis?.Invoke(this, new EventArgs());
                }
                else if (shift_holded)
                {
                    MoreTime.Invoke(this, new EventArgs());
                }
                else
                {
                    MoreScroll?.Invoke(this, new EventArgs());
                }
            }
            base.OnMouseWheel(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                // Action
                switch (edit_mode)
                {
                    case EditMode.Timeline:
                        {
                            Cursor = Cursors.HSplit;
                            MediaPlayerManager.Position = GetTime(e.Y + ViewPortOffset);
                            isMovingTimeline = true;
                            break;
                        }
                    case EditMode.Move:
                        {
                            Cursor = Cursors.SizeAll;
                            MoveSelectedSubtitles(e.Y);
                            break;
                        }
                    case EditMode.StartTime:
                        {
                            Cursor = Cursors.PanNorth;
                            EditStartTime(e.Y);
                            break;
                        }
                    case EditMode.EndTime:
                        {
                            Cursor = Cursors.PanSouth;
                            EditEndTime(e.Y);
                            break;
                        }
                    case EditMode.MoveVSpliter:
                        {
                            Cursor = Cursors.VSplit;
                            TimingPanelWidth = e.X;
                            if (TimingPanelWidth < 80)
                                TimingPanelWidth = 80;
                            if (TimingPanelWidth > 250)
                                TimingPanelWidth = 250;
                            isEditingVSplit = true;
                            break;
                        }
                }
            }
            else
            {
                // Detection
                // 1 first priority is the time line
                int y1 = (GetPixelOftime(MediaPlayerManager.Position) - ViewPortOffset) - CursorSensitivity;
                int y2 = (GetPixelOftime(MediaPlayerManager.Position) - ViewPortOffset) + CursorSensitivity;
                if (e.Y >= y1 && e.Y <= y2)
                {
                    edit_mode = EditMode.Timeline;
                    Cursor = Cursors.HSplit;
                }
                else
                {
                    if (e.X > TimingPanelWidth)
                    {
                        // 2 second priority is subtitle edition
                        Subtitle sub = GetSutitleByTime(GetTime(e.Y + ViewPortOffset));
                        if (sub != null)
                        {
                            // Let's see where we at
                            y1 = (GetPixelOftime(sub.StartTime) - ViewPortOffset) - CursorSensitivity;
                            y2 = (GetPixelOftime(sub.StartTime) - ViewPortOffset) + CursorSensitivity;
                            if (e.Y >= y1 && e.Y <= y2)
                            {
                                edit_mode = EditMode.StartTime;
                                Cursor = Cursors.PanNorth;
                            }
                            else
                            {
                                y1 = (GetPixelOftime(sub.EndTime) - ViewPortOffset) - CursorSensitivity;
                                y2 = (GetPixelOftime(sub.EndTime) - ViewPortOffset) + CursorSensitivity;
                                if (e.Y >= y1 && e.Y <= y2)
                                {
                                    edit_mode = EditMode.EndTime;
                                    Cursor = Cursors.PanSouth;
                                }
                                else
                                {
                                    y1 = GetPixelOftime(sub.StartTime) - ViewPortOffset;
                                    y2 = GetPixelOftime(sub.EndTime) - ViewPortOffset;
                                    if (e.Y >= y1 && e.Y <= y2)
                                    {
                                        edit_mode = EditMode.Move;
                                    }
                                    else
                                    {
                                        edit_mode = EditMode.None;
                                    }
                                    Cursor = Cursors.Default;
                                }
                            }
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                            edit_mode = EditMode.None;
                        }
                    }
                    else
                    {
                        int x1 = TimingPanelWidth - CursorSensitivity;
                        int x2 = TimingPanelWidth + CursorSensitivity;
                        if (e.X >= x1 && e.X <= x2)
                        {
                            edit_mode = EditMode.MoveVSpliter;
                            Cursor = Cursors.VSplit;
                        }
                        else
                        {
                            Cursor = Cursors.Default;
                            edit_mode = EditMode.None;
                        }
                    }
                }
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (isEditingSubtitle || isMovingTimeline)
                return;
            if (e.Button == MouseButtons.Left)
            {
                if (e.X > TimingPanelWidth)
                {
                    SelectSubtitleOneClick(e.X, e.Y);
                }
                else if (!isEditingSubtitle)
                {
                    MediaPlayerManager.Position = GetTime(e.Y + ViewPortOffset);
                }
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            bool inSubtitles = (e.X > TimingPanelWidth);
            if (inSubtitles)
            {
                Subtitle sub = GetSutitleByTime(GetTime(e.Y + ViewPortOffset));
                if (sub != null)
                {
                    if (!IsSelectedSubtitle(sub))
                    {
                        // The subtitle is not selected, simply select it !
                        SelectedSubtitles.Add(sub);// Just select it !
                                                   //show_selected_subtitles_count = true;
                                                   //selected_subtitles_count_counter = 2;
                                                   //selected_sibtitles_count_timer.Start();
                        if (SubtitlesSelected != null)
                            SubtitlesSelected(this, new EventArgs());
                        Invalidate();
                    }
                    else
                    {
                        // Raise the event so the parent control shall edit the subtitle text.
                        SubtitleDoubleClick?.Invoke(this, new EventArgs());
                    }
                }
            }
            else
            {
                SelectedSubtitles = new List<Subtitle>();
            }

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            base.OnEnter(new EventArgs());
            DownPoint = e.Location;
            DownPointToTimeSpace = new Point(e.X + ViewPortOffset, e.Y);
            if (edit_mode == EditMode.Move || edit_mode == EditMode.StartTime
            || edit_mode == EditMode.EndTime)
            {
                if (SelectedSubtitles.Count == 0)
                    SelectSubtitleOneClick(e.X, e.Y);
                TempStoreSubtitleValues();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (edit_mode == EditMode.Move || edit_mode == EditMode.StartTime
             || edit_mode == EditMode.EndTime)
            {
                // toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                if (SubtitlePropertiesChanged != null && save)
                    SubtitlePropertiesChanged(this, new EventArgs());
            }
            if (edit_mode == EditMode.MoveVSpliter)
            {
                if (showWaveForm)
                    CalculateWaveFormBuffers();
                ControlsBase.Settings.Vertical_WW = TimingPanelWidth;
            }
            save = false;
            isEditingSubtitle = false;
            isEditingVSplit = false;
            isMovingTimeline = false;
        }
        // Timer tick
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Visible)
                return;
            CurrentTime = MediaPlayerManager.Position;
            if (MediaPlayerManager.IsPlaying)
            {
                if (AlwaysScroll)
                {
                    ViewPortOffset = GetPixelOftime(CurrentTime) - (Height / 2);
                    if (ViewPortOffset < 0)
                        ViewPortOffset = 0;
                    if (ShowWaveForm)
                        CalculateWaveFormBuffers();
                    RequestScrollToTime?.Invoke(this, new EventArgs());
                }
                else if (AutoScrollWhenTimePassViewPort)
                {
                    if ((GetPixelOftime(CurrentTime) > ViewPortOffset + Height))
                    {
                        ViewPortOffset = GetPixelOftime(CurrentTime);
                        if (ShowWaveForm)
                            CalculateWaveFormBuffers();
                        RequestScrollToTime?.Invoke(this, new EventArgs());
                    }
                }
            }

            if (play_period)
            {
                if (!MediaPlayerManager.IsPlaying)
                {
                    // Not playing anymore... this mean no need to do play-period anymore
                    play_period = false;
                }
                else if (MediaPlayerManager.Position < play_period_start)
                    play_period = false;// Stop the play-period, the time is not in the sub region
                else if (MediaPlayerManager.Position >= play_period_end)
                {
                    // This is it !!
                    if (ReplaySelectedSubtitle)
                    {
                        MediaPlayerManager.Position = play_period_start;
                    }
                    else
                    {
                        MediaPlayerManager.Pause();
                        play_period = false;
                    }
                }
            }

            Invalidate();
        }
        void SelectSubtitlesMovingRectangle(MouseEventArgs e)
        {
            if (e.X < 10)
            {
                if (ViewPortOffset - 10 > 0)
                {
                    ViewPortOffset -= 10;
                    if (ViewPortYChanged != null)
                        ViewPortYChanged(this, new EventArgs());
                }

            }
            else if (e.X > this.Width - 10)
            {
                ViewPortOffset += 10;
                if (ViewPortYChanged != null)
                    ViewPortYChanged(this, new EventArgs());
            }
            //draw select rectangle
            /* SelectRectanglex = DownPointToTimeSpace.X - ViewPortOffset;
             SelectRectangley = DownPoint.Y;
             SelectRectanglex1 = e.X;
             SelectRectangley1 = e.Y;
             if (SelectRectanglex1 < SelectRectanglex)
             {
                 SelectRectanglex = e.X;
                 SelectRectanglex1 = DownPointToTimeSpace.X - ViewPortOffset;
             }
             if (SelectRectangley1 < SelectRectangley)
             {
                 SelectRectangley = e.Y;
                 SelectRectangley1 = DownPoint.Y;
             }
             //this.Invalidate();
             //select the highlighted subtitles
             isSelecting = true;
             if (ModifierKeys != Keys.Control)
                 SelectedSubtitles = new List<Subtitle>();
             if (SelectRectangley < TimingPanelWidth + SubtitleHeight)
             {
                 //selecting using time is better than using pixels
                 //cause if the zoom is to large, we might miss up some subtitles...
                 double startSelectTime = GetTime(ViewPortOffset + SelectRectanglex);
                 double endSelectTime = GetTime(ViewPortOffset + SelectRectanglex1);
                 for (double i = startSelectTime; i < endSelectTime; i++)
                 {
                     Subtitle sub = GetSutitleByTime(i);
                     if (sub != null & !IsSelectedSubtitle(sub))
                     {
                         SelectedSubtitles.Add(sub);
                         i = sub.EndTime;
                     }
                 }
             }*/
        }
        void SelectSubtitleOneClick(int x, int y)
        {
            bool inSubtitles = (x > TimingPanelWidth);
            if (ModifierKeys != Keys.Control)
                SelectedSubtitles = new List<Subtitle>();
            if (inSubtitles)
            {
                Subtitle sub = GetSutitleByTime(GetTime(y + ViewPortOffset));
                if (sub != null)
                {
                    if (!IsSelectedSubtitle(sub))
                    {
                        SelectedSubtitles.Add(sub);
                        //show_selected_subtitles_count = true;
                        //selected_subtitles_count_counter = 2;
                        //selected_sibtitles_count_timer.Start();
                        Invalidate();
                    }
                    else
                    {
                        // Remove that subtitle from selection !
                        SelectedSubtitles.Remove(sub);
                        //show_selected_subtitles_count = true;
                        // selected_subtitles_count_counter = 2;
                        // selected_sibtitles_count_timer.Start();
                        Invalidate();
                    }
                    SubtitleClick?.Invoke(this, new EventArgs());
                }
            }
            else
            {
                SelectedSubtitles = new List<Subtitle>();
            }
            if (SubtitlesSelected != null)
                SubtitlesSelected(this, new EventArgs());
            //this.Invalidate();
        }
        public void SelectSubtitle(int index)
        {
            if (index < 0)
                return;
            if (index >= SubtitlesTrack.Subtitles.Count)
                return;
            Subtitle sub = SubtitlesTrack.Subtitles[index];
            if (sub != null)
            {
                if (!IsSelectedSubtitle(sub))
                {
                    SelectedSubtitles.Add(sub);
                    //show_selected_subtitles_count = true;
                    //selected_subtitles_count_counter = 2;
                    //selected_sibtitles_count_timer.Start();
                    Invalidate();
                }
                else
                {
                    // Remove that subtitle from selection !
                    SelectedSubtitles.Remove(sub);
                    //show_selected_subtitles_count = true;
                    // selected_subtitles_count_counter = 2;
                    // selected_sibtitles_count_timer.Start();
                    Invalidate();
                }
                SubtitleClick?.Invoke(this, new EventArgs());
            }
            if (SubtitlesSelected != null)
                SubtitlesSelected(this, new EventArgs());
        }
        void TempStoreSubtitleValues()
        {
            TempSelectedSubtitles = new List<Subtitle>();
            foreach (Subtitle sub in SelectedSubtitles)
            {
                Subtitle ss = new Subtitle();
                ss.StartTime = sub.StartTime;
                ss.EndTime = sub.EndTime;
                TempSelectedSubtitles.Add(ss);
            }
        }

        void MoveSelectedSubtitles(int newYPoint)
        {
            if (SelectedSubtitles.Count > 0)
            {
                //calculate shift
                double shiftTime = GetTime(newYPoint + ViewPortOffset) - GetTime(DownPoint.Y + ViewPortOffset);
                if (shiftTime == 0)
                    return;
                //megnatic
                double tempStartTime = shiftTime + TempSelectedSubtitles[0].StartTime;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + CursorSensitivity;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - CursorSensitivity;
                int mix = GetPixelOftime(tempStartTime) - ViewPortOffset;
                if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                {
                    shiftTime = CurrentTime - TempSelectedSubtitles[0].StartTime;
                }

                //hideToolTipInfo = showToolTip = true;
                //toolTipText = resources.GetString("ShiftTime") + " = " + shiftTime.ToString("F3");
                //toolTipLocation = new Point(newXPoint, SpliterY / 2);
                //isMovingSubtitles = true;
                //move each selected subtitle 
                //we MUST do double loop here if subtitles count larger than 1
                for (int i = 0; i < SelectedSubtitles.Count; i++)
                {
                    double startTime = shiftTime + TempSelectedSubtitles[i].StartTime;
                    double endTime = shiftTime + TempSelectedSubtitles[i].EndTime;
                    Subtitle subAtStart = GetSutitleByTime(startTime, SelectedSubtitles[i]);
                    Subtitle subAtEnd = GetSutitleByTime(endTime, SelectedSubtitles[i]);
                    //normal shift, don't allow user to override other subtitles
                    if (subAtStart == null && subAtEnd == null)
                    {
                        SelectedSubtitles[i].StartTime = startTime;
                        SelectedSubtitles[i].EndTime = endTime;
                    }
                }
                //if (SelectedSubtitles.Count > 1)
                {
                    //We MUST do a reverse loop here
                    for (int i = SelectedSubtitles.Count - 1; i >= 0; i--)
                    {
                        double startTime = shiftTime + TempSelectedSubtitles[i].StartTime;
                        double endTime = shiftTime + TempSelectedSubtitles[i].EndTime;
                        Subtitle subAtStart = GetSutitleByTime(startTime, SelectedSubtitles[i]);
                        Subtitle subAtEnd = GetSutitleByTime(endTime, SelectedSubtitles[i]);
                        //normal shift, don't allow user to override other subtitles
                        if (subAtStart == null && subAtEnd == null)
                        {
                            SelectedSubtitles[i].StartTime = startTime;
                            SelectedSubtitles[i].EndTime = endTime;
                        }
                    }
                }
                if (shiftTime != 0)
                    save = true;
                //this.Invalidate();
                isEditingSubtitle = true;
            }
        }
        void EditStartTime(int newXPoint)
        {
            if (SelectedSubtitles.Count > 0)
            {
                //calculate shift
                double shiftTime = GetTime(newXPoint + ViewPortOffset) - GetTime(DownPoint.Y + ViewPortOffset);
                if (shiftTime == 0)
                    return;
                // hideToolTipInfo = showToolTip = true;
                // toolTipText = resources.GetString("Shift") + " = " + shiftTime.ToString("F3");
                // toolTipLocation = new Point(newXPoint, SpliterY / 2);
                // isMovingSubtitles = true;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + CursorSensitivity;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - CursorSensitivity;

                if (SelectedSubtitles.Count == 1)
                {
                    double startTime = shiftTime + TempSelectedSubtitles[0].StartTime;
                    //edit selected subtitle's start time 
                    Subtitle subAtStart = GetSutitleByTime(startTime, SelectedSubtitles[0]);
                    //megnatic
                    int mix = GetPixelOftime(startTime) - ViewPortOffset;
                    if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                    {
                        startTime = CurrentTime;
                    }
                    //normal shift, don't allow user to override other subtitles
                    if (subAtStart == null)
                    {
                        SelectedSubtitles[0].StartTime = startTime;
                    }
                }
                else
                {

                    double currenttime = GetTime(newXPoint + ViewPortOffset);
                    int mix = GetPixelOftime(currenttime) - ViewPortOffset;
                    if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                    {
                        currenttime = CurrentTime;
                    }
                    double OrgenalStart = TempSelectedSubtitles[0].StartTime;
                    double OrgenalMax = TempSelectedSubtitles[TempSelectedSubtitles.Count - 1].EndTime - TempSelectedSubtitles[0].StartTime;
                    double NewMax = TempSelectedSubtitles[TempSelectedSubtitles.Count - 1].EndTime - currenttime;
                    int j = 0;
                    foreach (Subtitle sub in SelectedSubtitles)
                    {
                        double OldStart = TempSelectedSubtitles[j].StartTime - OrgenalStart;//- OrgenalStart to return to 0
                        double OldEnd = TempSelectedSubtitles[j].EndTime - OrgenalStart;

                        double NewStart = ((OldStart * NewMax) / OrgenalMax);
                        double NewEnd = ((OldEnd * NewMax) / OrgenalMax);
                        sub.StartTime = NewStart + currenttime;
                        sub.EndTime = NewEnd + currenttime;
                        j++;
                    }
                }
                if (shiftTime != 0)
                    save = true;
                isEditingSubtitle = true;
                //this.Invalidate();
            }
        }
        void EditEndTime(int newXPoint)
        {
            if (SelectedSubtitles.Count > 0)
            {
                //calculate shift
                double shiftTime = GetTime(newXPoint + ViewPortOffset) - GetTime(DownPoint.Y + ViewPortOffset);
                if (shiftTime == 0)
                    return;
                //hideToolTipInfo = showToolTip = true;
                //toolTipText = resources.GetString("Shift") + " = " + shiftTime.ToString("F3");
                //toolTipLocation = new Point(newXPoint, SpliterY / 2);
                // isMovingSubtitles = true;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + CursorSensitivity;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - CursorSensitivity;

                if (SelectedSubtitles.Count == 1)
                {
                    double endTime = shiftTime + TempSelectedSubtitles[0].EndTime;
                    //edit selected subtitle's start time 
                    Subtitle subAtEnd = GetSutitleByTime(endTime, SelectedSubtitles[0]);
                    //megnatic
                    int mix = GetPixelOftime(endTime) - ViewPortOffset;
                    if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                    {
                        endTime = CurrentTime;
                    }
                    //normal shift, don't allow user to override other subtitles
                    if (subAtEnd == null)
                    {
                        SelectedSubtitles[0].EndTime = endTime;
                    }
                }
                else
                {
                    double currenttime = GetTime(newXPoint + ViewPortOffset);
                    int mix = GetPixelOftime(currenttime) - ViewPortOffset;
                    if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                    {
                        currenttime = CurrentTime;
                    }
                    double OrgenalStart = TempSelectedSubtitles[0].StartTime;
                    double OrgenalMax = TempSelectedSubtitles[TempSelectedSubtitles.Count - 1].EndTime - OrgenalStart;
                    double NewMax = currenttime - OrgenalStart;

                    int j = 0;
                    foreach (Subtitle sub in SelectedSubtitles)
                    {
                        double OldStart = TempSelectedSubtitles[j].StartTime - OrgenalStart;//- OrgenalStart to return to 0
                        double OldEnd = TempSelectedSubtitles[j].EndTime - OrgenalStart;

                        double NewStart = ((OldStart * NewMax) / OrgenalMax);
                        double NewEnd = ((OldEnd * NewMax) / OrgenalMax);
                        sub.StartTime = NewStart + OrgenalStart;
                        sub.EndTime = NewEnd + OrgenalStart;
                        j++;
                    }
                    /*for (int i = SelectedSubtitles.Count - 1; i >= 0; i--)
                    {
                        double OldStart = TempSelectedSubtitles[i].StartTime - OrgenalStart;//- OrgenalStart to return to 0
                        double OldEnd = TempSelectedSubtitles[i].EndTime - OrgenalStart;

                        double NewStart = ((OldStart * NewMax) / OrgenalMax);
                        double NewEnd = ((OldEnd * NewMax) / OrgenalMax);
                        SelectedSubtitles[i].StartTime = NewStart + OrgenalStart;
                        SelectedSubtitles[i].EndTime = NewEnd + OrgenalStart;
                    }*/
                }
                if (shiftTime != 0)
                    save = true;
                //this.Invalidate();
                isEditingSubtitle = true;
            }

        }
    }
}
