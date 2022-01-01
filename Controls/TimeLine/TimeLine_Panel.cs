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
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class TimeLine_Panel : Control
    {
        public TimeLine_Panel()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);

            toolTipTimer.Interval = 100;
            toolTipTimer.Tick += new EventHandler(toolTipTimer_Tick);

            selected_sibtitles_count_timer.Interval = 1000;
            selected_sibtitles_count_timer.Tick += new EventHandler(selected_sibtitles_count_timer_Tick);

            stringFormat = new StringFormat(StringFormatFlags.NoWrap);
            stringFormat.Trimming = StringTrimming.EllipsisWord;

            ShowGridLines = false;
            showWaveForm = false;
            mono_pos_buffer = new List<int>();
            mono_neg_buffer = new List<int>();
            left_pos_buffer = new List<int>();
            left_neg_buffer = new List<int>();
            right_pos_buffer = new List<int>();
            right_neg_buffer = new List<int>();

            ShowInviewSubtitleTime = true;
        }

        void toolTipTimer_Tick(object sender, EventArgs e)
        {
            if (toolTipTimerCounter > 0)
                toolTipTimerCounter--;
            else
            {
                toolTipTimer.Stop();
                showToolTip = true;
            }
        }
        void selected_sibtitles_count_timer_Tick(object sender, EventArgs e)
        {
            if (selected_subtitles_count_counter > 0)
            {
                show_selected_subtitles_count = true;
                selected_subtitles_count_counter--;
                this.Invalidate();
            }
            else
            {
                selected_sibtitles_count_timer.Stop();
                show_selected_subtitles_count = false;
                this.Invalidate();
            }
        }

        public int TimeSpace = 1000;//the pixels of time space
        public int ViewPortOffset = 0;//the pixel of viewport, view port is the panel_timeline width
        public int MilliPixel = 10;//how milli each pixel presents
        public double _mediaObjectDuration = 0;//this is the real media duration, represents the media object
        public double CurrentTime = 0;
        public int SpliterY = 35;
        public int SubtitleHeight = 35;
        public bool PreviewMode = false;
        public bool SelectSubtitleThatNotSelectedOnceEditEvenSubtitlesAreSelected = true;
        public string MediaText = "";
        public Color MediaRectangleColor = Color.MediumAquamarine;
        public Color MediaHeaderRectangleColor = Color.MediumSeaGreen;
        public Color SubtitleSelectedColor = Color.Purple;
        public Color SubtitleRectangleColor = Color.DodgerBlue;
        public Color SubtitleHeaderRectangleColor = Color.RoyalBlue;
        public Color SubtitleStringColor = Color.White;
        public Color TimeLineColor = Color.White;
        public Color ToolTipRectangleColor = Color.RoyalBlue;
        public Color ToolTipTextColor = Color.White;
        public Color SelectionRectangleColor = Color.Navy;
        public Color BackgroundColor = Color.LightSteelBlue;
        public Color SplitersColor = Color.Gray;
        private StringFormat stringFormat = new StringFormat();
        public bool ShowGridLines;
        private bool showWaveForm;
        private bool media_buffersReady;
        private List<int> mono_pos_buffer;
        private List<int> mono_neg_buffer;
        private List<int> left_pos_buffer;
        private List<int> left_neg_buffer;
        private List<int> right_pos_buffer;
        private List<int> right_neg_buffer;
        public SubtitlesTrack SubtitlesTrack;
        public List<Subtitle> SelectedSubtitles = new List<Subtitle>();
        List<Subtitle> TempSelectedSubtitles = new List<Subtitle>();
        /// <summary>
        /// Rised when the user selecte subtite(s)
        /// </summary>
        public event EventHandler SubtitlesSelected;
        public event EventHandler SubtitleDoubleClick;
        public event EventHandler MovingTimeLine;
        public event EventHandler<TimeChangeArgs> TimeChangeRequest;
        public event EventHandler SubtitlePropertiesChanged;
        public event EventHandler<ShowToolTipArgs> ShowToolTipRequest;
        public event EventHandler HideToolTip;
        public event EventHandler ViewPortXChanged;
        public event EventHandler MoreMillis;
        public event EventHandler LessMillis;
        public event EventHandler MoreScroll;
        public event EventHandler LessScroll;
        public event EventHandler MoreTime;
        public event EventHandler LessTime;
        public event EventHandler ShowWaveFormRequest;

        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource",
        Assembly.GetExecutingAssembly());
        string toolTipText = "";
        Point toolTipLocation = new Point();
        Timer toolTipTimer = new Timer();
        int toolTipTimerCounter = 3;
        bool showToolTip = false;
        Timer selected_sibtitles_count_timer = new Timer();
        int selected_subtitles_count_counter = 3;
        bool show_selected_subtitles_count;
        bool hideToolTipInfo = false;
        int currentSubtitleOfToolTip = 0;
        public bool play_period;
        public double play_period_start;
        public double play_period_end;
        public string play_period_text;
        //edit
        public TimeLineEditMode editMode = TimeLineEditMode.Smart;
        public TimeLineSmartEditMode smartEditMode = TimeLineSmartEditMode.None;
        Point DownPoint = new Point();
        Point DownPointToTimeSpace = new Point();
        bool DrawSelectRectangle = false;
        int SelectRectanglex = 0;
        int SelectRectangley = 0;
        int SelectRectanglex1 = 0;
        int SelectRectangley1 = 0;
        bool isMovingSubtitles = false;
        bool isSelecting = false;
        public bool isMovingTimeLine = false;
        bool save = false;
        public bool isDrawing = false;
        private bool isEditingSubtitle = false;
        public bool ShowInviewSubtitleTime;
        public int MegnaticAccuracy = 3;
        public int SmartToolAccuracy = 7;
        public bool AlwaysShowSubtitleLines;
        public double MediaObjectDuration
        {
            get { return _mediaObjectDuration; }
            set { _mediaObjectDuration = value; }
        }
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
        /// <summary>
        /// Calculate time using pixel
        /// </summary>
        /// <param name="x">The pixel to calculate time for, this pixel should be the real pixel of time space, not from view port</param>
        /// <returns>The time in Sec.Milli format</returns>
        double GetTime(int x)
        {
            // double tas = TimeSpace * MilliPixel;
            //double milli = (x * tas) / TimeSpace;

            //return milli / 1000;
            return ((double)MilliPixel * (double)x) / 1000;
        }
        int GetPixelOftime(double time)
        {
            double tas = TimeSpace * MilliPixel;
            return (int)(((time * 1000) * TimeSpace) / tas);
        }
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
                for (int x = 0; x < Width; x++)
                {
                    // 1 get time
                    double time = GetTime(x + ViewPortOffset);
                    // 2 get sample at that time

                    if (WaveReader.BufferNmChannel == 1)
                    {
                        // MONO         
                        int samplePOS, sampleNEG = 0;
                        int channelHeight = (SpliterY / 2);
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
                        int channelHeight = (SpliterY / 4);
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
        protected override void OnPaint(PaintEventArgs pe)
        {
            isDrawing = true;
            base.OnPaint(pe);
            #region Draw media
            int mediaDx = GetPixelOftime(_mediaObjectDuration);
            int channelHeight = (SpliterY / 4);
            int channelHalf = (SpliterY / 2);
            if (mediaDx >= ViewPortOffset && mediaDx > 0)
            {
                if (!showWaveForm)
                {
                    if (mediaDx - ViewPortOffset < this.Width)
                    {
                        pe.Graphics.FillRectangle(new SolidBrush(MediaRectangleColor), 0, 1, mediaDx - ViewPortOffset,
                            SpliterY - 1);
                        pe.Graphics.FillRectangle(new SolidBrush(MediaHeaderRectangleColor), 0, 1, mediaDx - ViewPortOffset, 15);
                        //draw string
                        pe.Graphics.DrawString(WaveReader.IsGenerating ? resources.GetString("Status_GeneratingWaveFormPleaseWait") : MediaText,
                                   new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(WaveReader.IsGenerating ? Color.Red : SubtitleStringColor),
                                   new RectangleF(0, 1, mediaDx - ViewPortOffset, 13), stringFormat);
                        pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle((ViewPortOffset == 0) ? 0 : -1, 1, mediaDx - ViewPortOffset,
                            SpliterY - 1));
                    }
                    else
                    {
                        pe.Graphics.FillRectangle(new SolidBrush(MediaRectangleColor), 0, 1, this.Width,
                             SpliterY - 1);
                        pe.Graphics.FillRectangle(new SolidBrush(MediaHeaderRectangleColor), 0, 1, this.Width, 15);
                        //draw string
                        pe.Graphics.DrawString(WaveReader.IsGenerating ? resources.GetString("Status_GeneratingWaveFormPleaseWait") : MediaText,
                                   new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(WaveReader.IsGenerating ? Color.Red : SubtitleStringColor),
                                   new RectangleF(0, 1, this.Width, 13), stringFormat);
                        pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle((ViewPortOffset == 0) ? 0 : -1, 1, this.Width + 1,
                             SpliterY - 1));
                    }
                }
                else
                {
                    if (mediaDx - ViewPortOffset < this.Width)
                    {
                        pe.Graphics.FillRectangle(new SolidBrush(MediaRectangleColor), 0, 1, mediaDx - ViewPortOffset,
                            SpliterY - 1);
                        pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle((ViewPortOffset == 0) ? 0 : -1, 1, mediaDx - ViewPortOffset,
                            SpliterY - 1));

                        if (WaveReader.BufferNmChannel == 1)
                        {
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHalf, mediaDx - ViewPortOffset, channelHalf);
                        }
                        else
                        {
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHeight, mediaDx - ViewPortOffset, channelHeight);
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHeight + channelHalf, mediaDx - ViewPortOffset, channelHeight + channelHalf);
                        }
                    }
                    else
                    {
                        pe.Graphics.FillRectangle(new SolidBrush(MediaRectangleColor), 0, 1, this.Width,
                             SpliterY - 1);
                        pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle((ViewPortOffset == 0) ? 0 : -1, 1, this.Width + 1,
                             SpliterY - 1));

                        if (WaveReader.BufferNmChannel == 1)
                        {
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHalf, Width, channelHalf);
                        }
                        else
                        {
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHeight, Width, channelHeight);
                            pe.Graphics.DrawLine(Pens.Black, 0, channelHeight + channelHalf, Width, channelHeight + channelHalf);
                        }
                    }
                    if (media_buffersReady)
                    {
                        for (int x = 0; x < Width; x++)
                        {
                            // 2 get sample at that time
                            if (WaveReader.BufferNmChannel == 1)
                            {
                                if (x < mono_pos_buffer.Count)
                                {
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHalf - mono_pos_buffer[x], x, channelHalf);
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHalf + -mono_neg_buffer[x], x, channelHalf);
                                }
                            }
                            else
                            {
                                // Stereo
                                // We need to draw both left and right channels.
                                // Start with the left channel
                                // Left sample is above
                                if (x < left_pos_buffer.Count)
                                {
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHeight - left_pos_buffer[x], x, channelHeight);
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHeight + -left_neg_buffer[x], x, channelHeight);

                                }
                                if (x < right_pos_buffer.Count)
                                {
                                    // Right sample is down
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHeight - right_pos_buffer[x] + channelHalf, x, channelHeight + channelHalf);
                                    pe.Graphics.DrawLine(Pens.Black, x, channelHeight + -right_neg_buffer[x] + channelHalf, x, channelHeight + channelHalf);
                                }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Draw Subtitles
            if (this.SubtitlesTrack != null)
            {
                //determine the first subtitle to draw
                if (isMovingSubtitles)
                {
                    foreach (Subtitle sub in SubtitlesTrack.Subtitles)
                    {
                        int subX = GetPixelOftime(sub.StartTime) - ViewPortOffset;
                        int subX1 = GetPixelOftime(sub.EndTime) - ViewPortOffset;
                        //check if we should break 'cause we need not to draw subtitles that not in range
                        if (subX >= this.Width)
                            break;
                        if (subX1 - subX > 0)
                        {
                            if (AlwaysShowSubtitleLines)
                            {
                                pe.Graphics.DrawLine(Pens.White, subX1, 0, subX1, Height);
                                pe.Graphics.DrawLine(Pens.White, subX, 0, subX, Height);
                            }
                            if (IsSelectedSubtitle(sub))
                            {
                                pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX, SpliterY + 1,
                                             subX1 - subX, SubtitleHeight - 1);
                                if (isEditingSubtitle)
                                {
                                    pe.Graphics.DrawLine(Pens.White, subX1, 0, subX1, Height);
                                    pe.Graphics.DrawLine(Pens.White, subX, 0, subX, Height);
                                    int ss_y = SpliterY + SubtitleHeight + 3;
                                    if (SelectedSubtitles.Count == 1)
                                    {
                                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX + ViewPortOffset));
                                        Size textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX + 2, ss_y);
                                        ss_y += 18;
                                        time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX1 + ViewPortOffset));
                                        textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX1 + 2, ss_y);
                                    }
                                    else if (sub == SelectedSubtitles[0])
                                    {
                                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX + ViewPortOffset));
                                        Size textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX + 2, ss_y);
                                    }
                                    else if (sub == SelectedSubtitles[SelectedSubtitles.Count - 1])
                                    {
                                        ss_y += 18;
                                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX1 + ViewPortOffset));
                                        Size textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX1 + 2, ss_y);
                                    }
                                }
                            }
                            else
                                pe.Graphics.FillRectangle(new SolidBrush(SubtitleRectangleColor), subX, SpliterY + 1,
                                                                   subX1 - subX, SubtitleHeight - 1);
                            // Draw header
                            pe.Graphics.FillRectangle(new SolidBrush(SubtitleHeaderRectangleColor), subX, SpliterY + 1,
                                                                subX1 - subX, 15);
                            // Drawtimings
                            string time1 = TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime) + " -> " + TimeFormatConvertor.To_TimeSpan_Milli(sub.EndTime) + " (" + sub.Duration.ToString("F3") + ")";
                            pe.Graphics.DrawString(time1, this.Font, new SolidBrush(SubtitleStringColor),
                             new RectangleF(subX + 1, SpliterY + 1, subX1 - subX, 13), stringFormat);
                            // Draw surrounding rectangle
                            pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(subX, SpliterY + 1,
                                subX1 - subX, SubtitleHeight - 1));
                            //Draw string
                            string[] lines = sub.Text.Lines;
                            int lineY = 17;
                            foreach (string line in lines)
                            {
                                if (lineY < SubtitleHeight && lineY + 13 < SubtitleHeight)
                                    pe.Graphics.DrawString(line, new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(SubtitleStringColor),
                                    new RectangleF(subX + 1, SpliterY + lineY, subX1 - subX, 13), stringFormat);
                                else
                                    break;
                                lineY += 15;
                            }
                        }
                    }
                }
                else
                {
                    Subtitle sub = null;
                    int subIndex = -1;
                    for (int i = ViewPortOffset; i < ViewPortOffset + this.Width; i++)
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
                            int subX = GetPixelOftime(currentSub.StartTime) - ViewPortOffset;
                            int subX1 = GetPixelOftime(currentSub.EndTime) - ViewPortOffset;
                            if (AlwaysShowSubtitleLines)
                            {
                                pe.Graphics.DrawLine(Pens.White, subX1, 0, subX1, Height);
                                pe.Graphics.DrawLine(Pens.White, subX, 0, subX, Height);
                            }
                            //check if we should break 'cause we need not to draw subtitles that not in range
                            if (subX >= this.Width)
                                break;
                            if (subX1 - subX > 0)
                            {
                                if (IsSelectedSubtitle(currentSub))
                                {
                                    pe.Graphics.FillRectangle(new SolidBrush(SubtitleSelectedColor), subX, SpliterY + 1,
                                             subX1 - subX, SubtitleHeight - 1);
                                    if (isEditingSubtitle)
                                    {
                                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX + ViewPortOffset));
                                        Size textSize = TextRenderer.MeasureText(time, this.Font);

                                        pe.Graphics.DrawLine(Pens.Black, subX, 0, subX, Height);
                                        pe.Graphics.FillRectangle(Brushes.Blue, subX, 3, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX, 3, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX + 2, 3);

                                        time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX1 + ViewPortOffset));
                                        textSize = TextRenderer.MeasureText(time, this.Font);

                                        pe.Graphics.DrawLine(Pens.Black, subX1, 0, subX1, Height);
                                        pe.Graphics.FillRectangle(Brushes.Blue, subX1, 18, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX1, 18, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX1 + 2, 18);
                                    }
                                }
                                else
                                    pe.Graphics.FillRectangle(new SolidBrush(SubtitleRectangleColor), subX, SpliterY + 1,
                                                                       subX1 - subX, SubtitleHeight - 1);
                                // Draw header
                                pe.Graphics.FillRectangle(new SolidBrush(SubtitleHeaderRectangleColor), subX, SpliterY + 1,
                                                                    subX1 - subX, 15);
                                // Drawtimings
                                string time1 = TimeFormatConvertor.To_TimeSpan_Milli(currentSub.StartTime) + " -> " + TimeFormatConvertor.To_TimeSpan_Milli(currentSub.EndTime) + " (" + currentSub.Duration.ToString("F3") + ")";
                                pe.Graphics.DrawString(time1, new Font("Tahoma", 7, FontStyle.Regular), new SolidBrush(SubtitleStringColor),
                                 new RectangleF(subX + 1, SpliterY + 2, subX1 - subX, 13), stringFormat);

                                // Draw surrounding rectangle
                                // Draw surrounding rectangle
                                int time_pixel = GetPixelOftime(CurrentTime) - ViewPortOffset;
                                if (time_pixel >= subX && time_pixel <= subX1)
                                {
                                    pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.White)), new Rectangle(subX, SpliterY + 1, subX1 - subX, SubtitleHeight - 1));

                                    if (ShowInviewSubtitleTime)
                                    {
                                        pe.Graphics.DrawLine(Pens.White, subX1, 0, subX1, Height);
                                        pe.Graphics.DrawLine(Pens.White, subX, 0, subX, Height);
                                        int ss_y = SpliterY + SubtitleHeight + 3;

                                        string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX + ViewPortOffset));
                                        Size textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleHeaderRectangleColor), subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX + 2, ss_y);
                                        ss_y += 18;
                                        time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(subX1 + ViewPortOffset));
                                        textSize = TextRenderer.MeasureText(time, this.Font);
                                        pe.Graphics.FillRectangle(new SolidBrush(SubtitleHeaderRectangleColor), subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawRectangle(Pens.Black, subX1, ss_y, textSize.Width, textSize.Height);
                                        pe.Graphics.DrawString(time, this.Font, Brushes.White, subX1 + 2, ss_y);

                                    }
                                }
                                else
                                    pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(subX, SpliterY + 1,
                                    subX1 - subX, SubtitleHeight - 1));

                                //Draw string
                                string[] lines = currentSub.Text.Lines;
                                int lineY = 17;
                                foreach (string line in lines)
                                {
                                    if (lineY < SubtitleHeight && lineY + 13 < SubtitleHeight)
                                        pe.Graphics.DrawString(line, new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(SubtitleStringColor),
                                        new RectangleF(subX + 1, SpliterY + lineY, subX1 - subX, 13), stringFormat);
                                    else
                                        break;
                                    lineY += 15;
                                }
                            }
                            subIndex++;
                        }
                    }
                }
            }
            #endregion
            //Draw time line
            int timeX = GetPixelOftime(CurrentTime);
            if (timeX >= ViewPortOffset)
            {
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TimeLineColor), 2),
                    timeX - ViewPortOffset, 0, timeX - ViewPortOffset, this.Height);
            }
            //Tool tip, draw only if the text is not null
            if (showToolTip & toolTipText != "")
            {
                Size textSize = TextRenderer.MeasureText(toolTipText, new Font("Tahoma", 8, FontStyle.Regular));

                pe.Graphics.FillRectangle(new SolidBrush(ToolTipRectangleColor), toolTipLocation.X + 5, toolTipLocation.Y + 5,
                       textSize.Width + 10, textSize.Height + 10);
                pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(toolTipLocation.X + 5, toolTipLocation.Y + 5,
                       textSize.Width + 10, textSize.Height + 10));
                pe.Graphics.DrawString(toolTipText,
                new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(ToolTipTextColor),
                new RectangleF(toolTipLocation.X + 10, toolTipLocation.Y + 10, textSize.Width, textSize.Height));
            }
            //select rectangle
            if (DrawSelectRectangle)
            {
                pe.Graphics.DrawRectangle(new Pen(new SolidBrush(SelectionRectangleColor), 2),
                     SelectRectanglex, SelectRectangley, SelectRectanglex1 - SelectRectanglex, SelectRectangley1 - SelectRectangley);
                DrawSubtitlesCount(pe);
            }
            if (ShowGridLines)
            {
                //calculate tick space
                int ticPixels = (1000 / MilliPixel);
                ticPixels = ((ticPixels % 10) + 10);
                int secPixels = ticPixels * 10;
                //Draw ticks
                for (int x = 0; x < this.Width; x++)
                {
                    //calculate which pixel we are at in the TimeSpace
                    int pix = ViewPortOffset + x;
                    //each ticksSpace pixels, draw small tick
                    if (pix % ticPixels == 0)
                    {
                        pe.Graphics.DrawLine(new Pen(new SolidBrush(Color.WhiteSmoke)), x, 0, x, this.Height);
                    }
                }
            }
            if (show_selected_subtitles_count)
                DrawSubtitlesCount(pe);
            if (play_period)
            {
                Size ss = TextRenderer.MeasureText(play_period_text, Font);
                pe.Graphics.FillRectangle(Brushes.Black, new Rectangle(1, 1, ss.Width + 1, ss.Height + 1));
                pe.Graphics.DrawString(play_period_text, Font, Brushes.Lime, new Point(2, 2));

                int x1 = GetPixelOftime(play_period_start) - ViewPortOffset;
                int x2 = GetPixelOftime(play_period_end) - ViewPortOffset;
                pe.Graphics.DrawLine(new Pen(Color.Yellow, 2), x1, 0, x1, Height);
                pe.Graphics.DrawLine(new Pen(Color.Yellow, 2), x2, 0, x2, Height);
            }
            isDrawing = false;
        }
        private void DrawSubtitlesCount(PaintEventArgs pe)
        {
            Size textSize = TextRenderer.MeasureText(SelectedSubtitles.Count.ToString(), new Font("Tahoma", 8, FontStyle.Regular));

            pe.Graphics.FillRectangle(new SolidBrush(ToolTipRectangleColor), 5, 5,
                   textSize.Width + 10, textSize.Height + 10);
            pe.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Black)), new Rectangle(5, 5,
                   textSize.Width + 10, textSize.Height + 10));
            pe.Graphics.DrawString(SelectedSubtitles.Count.ToString(),
            new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(ToolTipTextColor),
            new RectangleF(10, 10, textSize.Width, textSize.Height));
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            //draw background color to clear
            pevent.Graphics.FillRectangle(new SolidBrush(BackgroundColor),
                new Rectangle(0, 0, this.Width, this.Height));
            //draw spliters
            pevent.Graphics.DrawLine(new Pen(new SolidBrush(SplitersColor)), 0, 0, this.Width, 0);
            pevent.Graphics.DrawLine(new Pen(new SolidBrush(SplitersColor)), 0, SpliterY, this.Width, SpliterY);
            pevent.Graphics.DrawLine(new Pen(new SolidBrush(SplitersColor)), 0, SpliterY + SubtitleHeight, this.Width, SpliterY + SubtitleHeight);
        }
        //Here everything goes...
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Parent.Parent.Select();
            base.OnMouseDown(e);
            base.OnEnter(new EventArgs());
            if (PreviewMode)
                return;
            DownPoint = e.Location;
            DownPointToTimeSpace = new Point(e.X + ViewPortOffset, e.Y);
            if (editMode == TimeLineEditMode.Smart)
            {
                switch (smartEditMode)
                {
                    case TimeLineSmartEditMode.Select: DrawSelectRectangle = true; break;
                    case TimeLineSmartEditMode.EditStartTime:
                    case TimeLineSmartEditMode.EditEndTime:
                    case TimeLineSmartEditMode.Move:
                        if (SelectedSubtitles.Count == 0)
                            SelectSubtitleOneClick(e.X, e.Y);
                        else
                        {
                            if (SelectSubtitleThatNotSelectedOnceEditEvenSubtitlesAreSelected)
                            {
                                Subtitle sub = GetSutitleByTime(GetTime(e.X + ViewPortOffset));
                                if (sub != null)
                                {
                                    if (!IsSelectedSubtitle(sub))
                                    {
                                        SelectSubtitleOneClick(e.X, e.Y);
                                    }
                                }
                            }
                        }
                        TempStoreSubtitleValues();
                        break;
                }
            }
            if (editMode == TimeLineEditMode.Select)
            {
                DrawSelectRectangle = true;
            }
            if (editMode == TimeLineEditMode.Move || editMode == TimeLineEditMode.EditStartTime
                || editMode == TimeLineEditMode.EditEndTime)
            {
                if (SelectedSubtitles.Count == 0)
                    SelectSubtitleOneClick(e.X, e.Y);
                TempStoreSubtitleValues();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (PreviewMode)
                return;
            if (editMode == TimeLineEditMode.Smart)
            {
                //if smart select mode, switch to none
                if (smartEditMode == TimeLineSmartEditMode.Select)
                {
                    if (DrawSelectRectangle)
                    {
                        show_selected_subtitles_count = true;
                        selected_subtitles_count_counter = 1;
                        selected_sibtitles_count_timer.Start();
                        Invalidate();
                    }
                    smartEditMode = TimeLineSmartEditMode.None;
                    isSelecting = DrawSelectRectangle = false;
                    SelectRectanglex = 0;
                    SelectRectangley = 0;
                    SelectRectanglex1 = 0;
                    SelectRectangley1 = 0;
                    if (SubtitlesSelected != null)
                        SubtitlesSelected(this, e);
                }
                else if (smartEditMode == TimeLineSmartEditMode.Move)
                {
                    toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                    if (SubtitlePropertiesChanged != null && save)
                        SubtitlePropertiesChanged(this, new EventArgs());
                }
                else if (smartEditMode == TimeLineSmartEditMode.MoveLine)
                {
                    toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                    isMovingTimeLine = false;
                    if (TimeChangeRequest != null)
                    {
                        int x = e.X + ViewPortOffset;
                        if (x < 0)
                            x = 0;
                        TimeChangeRequest(this, new TimeChangeArgs(GetTime(x), 0));
                    }
                }
                else if (smartEditMode == TimeLineSmartEditMode.EditStartTime)
                {
                    toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                    if (SubtitlePropertiesChanged != null && save)
                        SubtitlePropertiesChanged(this, new EventArgs());
                }
                else if (smartEditMode == TimeLineSmartEditMode.EditEndTime)
                {
                    toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                    if (SubtitlePropertiesChanged != null && save)
                        SubtitlePropertiesChanged(this, new EventArgs());
                }
            }
            else if (editMode == TimeLineEditMode.Move)
            {
                toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                if (SubtitlePropertiesChanged != null && save)
                    SubtitlePropertiesChanged(this, new EventArgs());
            }
            else if (editMode == TimeLineEditMode.EditStartTime)
            {
                toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                if (SubtitlePropertiesChanged != null && save)
                    SubtitlePropertiesChanged(this, new EventArgs());
            }
            else if (editMode == TimeLineEditMode.EditEndTime)
            {
                toolTipText = ""; toolTipTimer.Stop(); isMovingSubtitles = showToolTip = hideToolTipInfo = false;
                if (SubtitlePropertiesChanged != null && save)
                    SubtitlePropertiesChanged(this, new EventArgs());
            }
            else if (editMode == TimeLineEditMode.Select)
            {
                if (DrawSelectRectangle)
                {
                    show_selected_subtitles_count = true;
                    selected_subtitles_count_counter = 1;
                    selected_sibtitles_count_timer.Start();
                    Invalidate();
                }
                isSelecting = DrawSelectRectangle = false;
                SelectRectanglex = 0;
                SelectRectangley = 0;
                SelectRectanglex1 = 0;
                SelectRectangley1 = 0;
                if (SubtitlesSelected != null)
                    SubtitlesSelected(this, e);
            }
            save = false;
            isEditingSubtitle = false;
            //this.Invalidate();
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (PreviewMode)
                return;
            #region Cursor
            switch (editMode)
            {
                case TimeLineEditMode.Smart:
                    switch (smartEditMode)
                    {
                        case TimeLineSmartEditMode.EditEndTime: Cursor = Cursors.PanEast; break;
                        case TimeLineSmartEditMode.EditStartTime: Cursor = Cursors.PanWest; break;
                        case TimeLineSmartEditMode.Move: Cursor = Cursors.Default; break;
                        case TimeLineSmartEditMode.MoveLine: Cursor = Cursors.VSplit; break;
                        case TimeLineSmartEditMode.Select:
                        case TimeLineSmartEditMode.None: Cursor = Cursors.Default; break;
                    }
                    break;
                case TimeLineEditMode.EditStartTime: Cursor = Cursors.PanWest; break;
                case TimeLineEditMode.EditEndTime: Cursor = Cursors.PanEast; break;
                case TimeLineEditMode.Move: Cursor = Cursors.SizeAll; break;
                case TimeLineEditMode.Split: Cursor = Cursors.VSplit; break;
                case TimeLineEditMode.Select: Cursor = Cursors.Default; break;
            }
            #endregion
            #region SMART tool
            if (editMode == TimeLineEditMode.Smart)
            {
                //Do Action
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    switch (smartEditMode)
                    {
                        case TimeLineSmartEditMode.Select:
                            {
                                SelectSubtitlesMovingRectangle(e);
                                break;
                            }
                        case TimeLineSmartEditMode.Move:
                            {
                                MoveSelectedSubtitles(e.X);
                                break;
                            }
                        case TimeLineSmartEditMode.MoveLine:
                            {
                                isMovingTimeLine = true;
                                double tt = GetTime(e.X + ViewPortOffset);
                                if (tt < 0)
                                    tt = 0;
                                CurrentTime = tt;
                                if (MovingTimeLine != null)
                                    MovingTimeLine(this, new EventArgs());
                                hideToolTipInfo = showToolTip = true;
                                toolTipText = TimeFormatConvertor.To_TimeSpan_Milli(CurrentTime) + "\n" + CurrentTime.ToString("F3") + " sec";
                                toolTipLocation = new Point(e.X > 0 ? e.X : 0, e.Y);
                                //this.Invalidate();
                                break;
                            }
                        case TimeLineSmartEditMode.EditStartTime: { EditStartTime(e.X); break; }
                        case TimeLineSmartEditMode.EditEndTime: { EditEndTime(e.X); break; }
                    }
                }
                else//Determine which tool should be change to
                {
                    bool inSubtitles = (e.Y > SpliterY) && (e.Y < SpliterY + SubtitleHeight);
                    int timeX = GetPixelOftime(CurrentTime) - ViewPortOffset;
                    int maxT = timeX + SmartToolAccuracy;
                    int minT = timeX - SmartToolAccuracy;
                    if (inSubtitles)
                    {
                        Subtitle sub = GetSutitleByTime(GetTime(e.X + ViewPortOffset));
                        if (sub != null)
                        {
                            //if the mouse over the start time of the subitle, set start time edit mode
                            int startPixel = GetPixelOftime(sub.StartTime) - ViewPortOffset;
                            int endPixel = GetPixelOftime(sub.EndTime) - ViewPortOffset;

                            int maxs = startPixel + SmartToolAccuracy;
                            int mins = startPixel - SmartToolAccuracy;
                            int maxe = endPixel + SmartToolAccuracy;
                            int mine = endPixel - SmartToolAccuracy;

                            if (e.X >= mins & e.X <= maxs)
                            {
                                smartEditMode = TimeLineSmartEditMode.EditStartTime;
                            }
                            else if (e.X >= mine & e.X <= maxe)
                            {
                                smartEditMode = TimeLineSmartEditMode.EditEndTime;
                            }
                            else if (e.X < endPixel & e.X > startPixel)
                            {
                                smartEditMode = TimeLineSmartEditMode.Move;
                            }
                        }
                        else
                        {
                            if (e.X >= minT & e.X <= maxT)
                            {
                                smartEditMode = TimeLineSmartEditMode.MoveLine;
                            }
                            else
                            {
                                smartEditMode = TimeLineSmartEditMode.Select;
                            }
                        }
                    }
                    else
                    {
                        if (e.X >= minT & e.X <= maxT)
                        {
                            smartEditMode = TimeLineSmartEditMode.MoveLine;
                        }
                        else
                        {
                            smartEditMode = TimeLineSmartEditMode.Select;
                        }
                    }
                }
            }
            #endregion
            #region Select tool
            else if (editMode == TimeLineEditMode.Select)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    SelectSubtitlesMovingRectangle(e);
            }
            #endregion
            #region Move tool
            else if (editMode == TimeLineEditMode.Move)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    MoveSelectedSubtitles(e.X);
                }
            }
            #endregion
            #region EditStartTime
            else if (editMode == TimeLineEditMode.EditStartTime)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    EditStartTime(e.X);
                }
            }
            #endregion
            #region EditEndTime
            else if (editMode == TimeLineEditMode.EditEndTime)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    EditEndTime(e.X);
                }
            }
            #endregion
            #region ToolTip
            if (!hideToolTipInfo)
            {
                if (e.Y > SpliterY & e.Y < (SpliterY + SubtitleHeight))
                {
                    Subtitle sub = GetSutitleByTime(GetTime(e.X + ViewPortOffset));
                    if (sub != null)
                    {
                        int index = GetSubtitleIndex(sub);
                        if (currentSubtitleOfToolTip != index)
                        {
                            string info = sub.Text.ToString() + "\n\n" + resources.GetString("StartTime") + ": " +
                                TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime)
                                + "\n" + resources.GetString("EndTime") + ": " + TimeFormatConvertor.To_TimeSpan_Milli(sub.EndTime) +
                                "\n" + resources.GetString("Duration") + ": " + sub.Duration.ToString("F3");
                            //toolTipText = info;

                            //toolTipLocation = e.Location;
                            //toolTipTimerCounter = 3;
                            //toolTipTimer.Start();
                            if (ShowToolTipRequest != null)
                                ShowToolTipRequest(this, new ShowToolTipArgs(info));
                        }
                        currentSubtitleOfToolTip = index;
                    }
                    else
                    {
                        //toolTipText = ""; toolTipTimer.Stop(); showToolTip = false;
                        if (HideToolTip != null)
                            HideToolTip(this, null);
                        currentSubtitleOfToolTip = -1;
                    }
                }
                else
                {
                    //toolTipText = ""; toolTipTimer.Stop(); showToolTip = false;
                    if (HideToolTip != null)
                        HideToolTip(this, null);
                    currentSubtitleOfToolTip = -1;
                }
            }
            #endregion
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (PreviewMode)
                return;

            if (editMode == TimeLineEditMode.Smart)
            {
                //if the mouse is smart and clicked under the subtitles, then switch to smart select mode
                switch (smartEditMode)
                {
                    case TimeLineSmartEditMode.Move:
                        if (!isMovingSubtitles)
                            SelectSubtitleOneClick(e.X, e.Y);
                        break;
                    case TimeLineSmartEditMode.Select:
                        if (!isSelecting)
                            SelectSubtitleOneClick(e.X, e.Y);
                        break;
                }
            }
            else if (editMode == TimeLineEditMode.Select)
            {
                if (!isSelecting)
                    SelectSubtitleOneClick(e.X, e.Y);
            }
            else if (editMode == TimeLineEditMode.Move)
            {
                if (e.Y < SpliterY + SubtitleHeight & e.Y > SpliterY && !isMovingSubtitles)
                {
                    SelectSubtitleOneClick(e.X, e.Y);
                }
            }
            else if (editMode == TimeLineEditMode.Split)
            {
                if (e.Y < SpliterY + SubtitleHeight & e.Y > SpliterY)
                {
                    Split(e.X);
                }
            }
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Y < SpliterY)
            {
                ShowWaveFormRequest?.Invoke(this, new EventArgs());
            }
            base.OnMouseDoubleClick(e);
            if (PreviewMode)
                return;
            //SubtitleDoubleClick
            if (e.Y < SpliterY + SubtitleHeight & e.Y > SpliterY)
            {
                if (SelectedSubtitles.Count > 0)
                    if (SubtitleDoubleClick != null)
                        SubtitleDoubleClick(this, new EventArgs());
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

        #region Methods
        void SelectSubtitlesMovingRectangle(MouseEventArgs e)
        {
            if (e.X < 10)
            {
                if (ViewPortOffset - 10 > 0)
                {
                    ViewPortOffset -= 10;
                    if (ViewPortXChanged != null)
                        ViewPortXChanged(this, new EventArgs());
                }

            }
            else if (e.X > this.Width - 10)
            {
                ViewPortOffset += 10;
                if (ViewPortXChanged != null)
                    ViewPortXChanged(this, new EventArgs());
            }
            //draw select rectangle
            SelectRectanglex = DownPointToTimeSpace.X - ViewPortOffset;
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
            if (SelectRectangley < SpliterY + SubtitleHeight)
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
            }
        }
        void SelectSubtitleOneClick(int x, int y)
        {
            bool inSubtitles = (y > SpliterY) && (y < SpliterY + SubtitleHeight);
            if (ModifierKeys != Keys.Control)
                SelectedSubtitles = new List<Subtitle>();
            if (inSubtitles)
            {
                Subtitle sub = GetSutitleByTime(GetTime(x + ViewPortOffset));
                if (sub != null)
                {
                    if (!IsSelectedSubtitle(sub))
                    {
                        SelectedSubtitles.Add(sub);
                        show_selected_subtitles_count = true;
                        selected_subtitles_count_counter = 2;
                        selected_sibtitles_count_timer.Start();
                        Invalidate();
                    }
                    else
                    {
                        // Remove that subtitle from selection !
                        SelectedSubtitles.Remove(sub);
                        show_selected_subtitles_count = true;
                        selected_subtitles_count_counter = 2;
                        selected_sibtitles_count_timer.Start();
                        Invalidate();
                    }
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
        void MoveSelectedSubtitles(int newXPoint)
        {
            if (SelectedSubtitles.Count > 0)
            {
                if (newXPoint < 10)
                {
                    if (ViewPortOffset - 10 > 0)
                    {
                        ViewPortOffset -= 10;
                        ViewPortXChanged?.Invoke(this, new EventArgs());
                    }

                }
                else if (newXPoint > this.Width - 10)
                {
                    ViewPortOffset += 10;
                    ViewPortXChanged?.Invoke(this, new EventArgs());
                }
                //calculate shift
                double shiftTime = GetTime(newXPoint + ViewPortOffset) - GetTime(DownPointToTimeSpace.X);
                //megnatic
                double tempStartTime = shiftTime + TempSelectedSubtitles[0].StartTime;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + MegnaticAccuracy;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - MegnaticAccuracy;
                int mix = GetPixelOftime(tempStartTime) - ViewPortOffset;
                if (mix >= min && mix <= max && ModifierKeys != Keys.Shift)
                {
                    shiftTime = CurrentTime - TempSelectedSubtitles[0].StartTime;
                }

                hideToolTipInfo = showToolTip = true;
                toolTipText = resources.GetString("ShiftTime") + " = " + shiftTime.ToString("F3");
                toolTipLocation = new Point(newXPoint, SpliterY / 2);
                isMovingSubtitles = true;
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
                if (newXPoint < 10)
                {
                    if (ViewPortOffset - 10 > 0)
                    {
                        ViewPortOffset -= 10;
                        ViewPortXChanged?.Invoke(this, new EventArgs());
                    }

                }
                else if (newXPoint > this.Width - 10)
                {
                    ViewPortOffset += 10;
                    ViewPortXChanged?.Invoke(this, new EventArgs());
                }

                //calculate shift
                double shiftTime = GetTime(newXPoint + ViewPortOffset) - GetTime(DownPointToTimeSpace.X);
                hideToolTipInfo = showToolTip = true;
                toolTipText = resources.GetString("Shift") + " = " + shiftTime.ToString("F3");
                toolTipLocation = new Point(newXPoint, SpliterY / 2);
                isMovingSubtitles = true;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + MegnaticAccuracy;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - MegnaticAccuracy;

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
                if (newXPoint < 10)
                {
                    if (ViewPortOffset - 10 > 0)
                    {
                        ViewPortOffset -= 10;
                        ViewPortXChanged?.Invoke(this, new EventArgs());
                    }

                }
                else if (newXPoint > this.Width - 10)
                {
                    ViewPortOffset += 10;
                    ViewPortXChanged?.Invoke(this, new EventArgs());
                }

                //calculate shift
                double shiftTime = GetTime(newXPoint + ViewPortOffset) - GetTime(DownPointToTimeSpace.X);
                hideToolTipInfo = showToolTip = true;
                toolTipText = resources.GetString("Shift") + " = " + shiftTime.ToString("F3");
                toolTipLocation = new Point(newXPoint, SpliterY / 2);
                isMovingSubtitles = true;
                int max = GetPixelOftime(CurrentTime) - ViewPortOffset + MegnaticAccuracy;
                int min = GetPixelOftime(CurrentTime) - ViewPortOffset - MegnaticAccuracy;

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
        void Split(int newXPoint)
        {
            //get the subtitle at clicked time
            Subtitle subAtclick = GetSutitleByTime(GetTime(newXPoint + ViewPortOffset));
            if (subAtclick != null)
            {
                //calculate the start and end time of new subtitle
                double newStart = GetTime(newXPoint + ViewPortOffset);
                double newEnd = subAtclick.EndTime;
                //set old subtitle end
                subAtclick.EndTime = newStart - 0.001;
                //insert new subtitle
                SubtitleText newText = new SubtitleText();
                newText.CustomPosition = subAtclick.Text.CustomPosition;
                newText.IsCustomPosition = subAtclick.Text.IsCustomPosition;
                newText.Position = subAtclick.Text.Position;
                foreach (SubtitleLine line in subAtclick.Text.TextLines)
                {
                    SubtitleLine newLine = new SubtitleLine();
                    newLine.Alignement = line.Alignement;
                    foreach (SubtitleChar chr in line.Chars)
                    {
                        newLine.Chars.Add(new SubtitleChar(chr.TheChar, chr.Font, chr.Color));
                    }
                    newText.TextLines.Add(newLine);
                }
                Subtitle newSub = new Subtitle(newStart, newEnd, newText);
                this.SubtitlesTrack.Subtitles.Insert(this.SubtitlesTrack.Subtitles.IndexOf(subAtclick), newSub);
                if (SubtitlePropertiesChanged != null)
                    SubtitlePropertiesChanged(this, new EventArgs());
                //this.Invalidate();
            }
        }
        #endregion                   
    }
    public enum TimeLineEditMode
    { Select, Smart, Move, Split, EditStartTime, EditEndTime, }
    public enum TimeLineSmartEditMode
    { Select, MoveLine, Move, EditStartTime, EditEndTime, None }
}
