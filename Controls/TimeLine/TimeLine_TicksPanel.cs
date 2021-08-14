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
    public partial class TimeLine_TicksPanel : Control
    {
        public TimeLine_TicksPanel()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);

            toolTipTimer.Tick += new EventHandler(toolTipTimer_Tick);
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
        #region Drawing Values
        public Color TickColor = Color.White;
        public Color MarkColor = Color.Red;
        public Color TimeLineColor = Color.White;
        public Color ToolTipColor = Color.Blue;
        public Color ToolTipTextColor = Color.White;
        public Color BackgroundColor = Color.LightSlateGray;

        public int TimeSpace = 1000;//the pixels of time space
        public int ViewPortOffset = 0;//the pixel of viewport, view port is the panel_timeline width
        public int MilliPixel = 10;//how milli each pixel presents
        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource", Assembly.GetExecutingAssembly());
        public double CurrentTime = 0;
        #endregion
        public bool PreviewMode = false;
        string toolTipText = "";
        Point toolTipLocation = new Point();
        Timer toolTipTimer = new Timer();
        int toolTipTimerCounter = 3;
        bool showToolTip = false;
        public List<TimeMark> timeMarks;
        bool EditMark = false;
        public event EventHandler<TimeChangeArgs> TimeChangeRequest;
        public event EventHandler<MarkEditArgs> MarkEdit;
        public event EventHandler<MarkEditArgs> SelectMarkRequest;
        TimeMark markToEdit = null;
        double oldMarkTime = 0;
        int editMarkIndex = 0;
      

        /// <summary>
        /// Calculate time using pixel
        /// </summary>
        /// <param name="x">The pixel to calculate time for, this pixel should be the real pixel of time space, not from view port</param>
        /// <returns>The time in Sec.Milli format</returns>
        double GetTime(int x)
        {
            double tas = TimeSpace * MilliPixel;
            double milli = (x * tas) / TimeSpace;
            return milli / 1000;
        }
        int GetPixelOftime(double time)
        {
            double tas = TimeSpace * MilliPixel;
            return (int)(((time * 1000) * TimeSpace) / tas);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics gr = pe.Graphics;
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
                    gr.DrawLine(new Pen(new SolidBrush(TickColor), 2), x, this.Height - 5, x, this.Height);
                }
                //each secPixels pixels, draw big tick and time
                if (pix % secPixels == 0)
                {
                    gr.DrawLine(new Pen(new SolidBrush(TickColor), 2), x, this.Height - 10, x, this.Height);
                    //calculate the time of this pixel
                    string time = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(pix));
                    gr.DrawString(time, new Font("Tahoma", 8, FontStyle.Regular),
                         new SolidBrush(TickColor), new PointF(x - 32, this.Height - 25));
                }
            }
            int timeX = 0;
            //Draw marks
            if (timeMarks != null)
            {
                foreach (TimeMark mark in timeMarks)
                {
                    timeX = GetPixelOftime(mark.Time);
                    if (timeX >= ViewPortOffset && timeX < this.Width + ViewPortOffset)
                    {
                        gr.DrawLine(new Pen(new SolidBrush(MarkColor), 2),
                            timeX - ViewPortOffset, this.Height - 15, timeX - ViewPortOffset, this.Height);
                        gr.FillRectangle(new SolidBrush(MarkColor), new Rectangle(timeX - ViewPortOffset, this.Height - 15, 2, 2));
                    }
                }
            }
            //Draw time line
            timeX = GetPixelOftime(CurrentTime);
            if (timeX >= ViewPortOffset)
            {
                gr.DrawLine(new Pen(new SolidBrush(TimeLineColor),2),
                    timeX - ViewPortOffset, this.Height - 15, timeX - ViewPortOffset, this.Height);
            }
            //Tool tip, draw only if the text is not null
            if (toolTipText != "" & showToolTip)
            {
                Size textSize = TextRenderer.MeasureText(toolTipText, new Font("Tahoma", 8, FontStyle.Regular));

                pe.Graphics.FillRectangle(new SolidBrush(ToolTipColor), toolTipLocation.X, 5,
                       textSize.Width, textSize.Height + 4);
                gr.DrawLine(new Pen(new SolidBrush(ToolTipColor)),
                toolTipLocation.X, this.Height - 15, toolTipLocation.X, this.Height);
                pe.Graphics.DrawString(toolTipText,
                new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(ToolTipTextColor),
                new RectangleF(toolTipLocation.X, 5, textSize.Width, textSize.Height));
            }
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            pevent.Graphics.FillRectangle(new SolidBrush(BackgroundColor),
                new Rectangle(0, 0, this.Width, this.Height));
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (PreviewMode)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (TimeChangeRequest != null)
                    TimeChangeRequest(this, new TimeChangeArgs(GetTime(e.X + ViewPortOffset), 0));
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            toolTipLocation = e.Location;
            if (!showToolTip)
            {
                toolTipTimerCounter = 3;
                toolTipTimer.Start();
            }
            toolTipText = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(e.X + ViewPortOffset));
            int max = e.X + 3;
            int min = e.X - 3;
            bool found = false;
            if (timeMarks != null)
            {
                foreach (TimeMark mark in timeMarks)
                {
                    int timeX = GetPixelOftime(mark.Time) - ViewPortOffset;
                    if (max >= timeX & min <= timeX)
                    {
                        found = true;
                        toolTipText = resources.GetString("Mark") + ": " + mark.Name + " (" + mark.Time.ToString("F3") + ")";
                        Cursor = Cursors.VSplit;
                        break;
                    }
                }
            }
            if (!found)
            {
                Cursor = Cursors.Default;
            }
            //edit mark
            if (e.Button == System.Windows.Forms.MouseButtons.Left && EditMark)
            {
                markToEdit.Time = GetTime(e.X + ViewPortOffset);
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (PreviewMode)
                return;
            //for mark
            int max = e.X + 3;
            int min = e.X - 3;
            if (timeMarks != null)
            {
                int i = 0;
                foreach (TimeMark mark in timeMarks)
                {
                    int timeX = GetPixelOftime(mark.Time) - ViewPortOffset;
                    if (max >= timeX & min <= timeX)
                    {
                        EditMark = true;
                        markToEdit = mark;
                        editMarkIndex = i;
                        oldMarkTime = mark.Time;
                        if (SelectMarkRequest != null)
                            SelectMarkRequest(this, new MarkEditArgs(i));
                        break;
                    }
                    i++;
                }
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (PreviewMode)
                return;
            if (markToEdit != null)
            {
                if (EditMark & oldMarkTime != markToEdit.Time)
                {
                    if (MarkEdit != null)
                        MarkEdit(this, new MarkEditArgs(editMarkIndex));
                }
            }
            EditMark = false;
            markToEdit = null;
            Cursor = Cursors.Default;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            toolTipText = "";
            showToolTip = false;
            toolTipTimer.Stop();
        }
    }
}
