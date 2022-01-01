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
    public partial class MediaSeeker : Control
    {
        public MediaSeeker()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);

            TimeLineColor = Color.Black;
            TickColor = Color.White;
            ToolTipColor = Color.Blue;
            ToolTipTextColor = Color.White;
            mediaDuration = 10;
            timeSpace = 1000;
            MilliPixel = 50;
            ViewPortOffset = 0;
            currentTime = 0;

            toolTipTimer.Tick += toolTipTimer_Tick;
        }
        private double mediaDuration;
        private int timeSpace;
        private double currentTime;
        private string toolTipText = "";
        private Point toolTipLocation = new Point();
        private Timer toolTipTimer = new Timer();
        private int toolTipTimerCounter = 3;
        private bool showToolTip = false;
        private bool isTimeCursorMove;

        // Properties
        /// <summary>
        /// Get or set the time line color
        /// </summary>
        public Color TimeLineColor { get; set; }
        /// <summary>
        /// Get or set the ticks color
        /// </summary>
        public Color TickColor { get; set; }
        /// <summary>
        /// Get or set the tooltip color
        /// </summary>
        public Color ToolTipColor { get; set; }
        /// <summary>
        /// Get or set the tooltip text color
        /// </summary>
        public Color ToolTipTextColor { get; set; }
        /// <summary>
        /// Get or set the time space in Milliseconds.
        /// </summary>
        public double MediaDuration
        {
            get { return mediaDuration; }
            set
            {
                mediaDuration = value;
                CaclulateMillipixels();
            }
        }
        /// <summary>
        /// Get or set the milli-pixel value; how many milliseconds each pixel presents.
        /// </summary>
        public int MilliPixel { get; set; }
        /// <summary>
        /// Get or set the viewport offset
        /// </summary>
        public int ViewPortOffset { get; set; }
        /// <summary>
        /// Get or set current time in seconds.
        /// </summary>
        public double TimePosition
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                this.Invalidate();
            }
        }

        // Events
        public event EventHandler<TimeChangeArgs> TimeChangeRequest;

        private void CaclulateMillipixels()
        {
            if (this.Width > 0)
            {
                MilliPixel = (int)((mediaDuration * 1000) + 10000) / this.Width;

                timeSpace = (int)((mediaDuration * 1000) / MilliPixel) + 100;
            }
        }
        private double GetTime(int x)
        {
            double tas = timeSpace * MilliPixel;
            double milli = (x * tas) / timeSpace;
            return milli / 1000;
        }
        private int GetPixelOftime(double time)
        {
            double tas = timeSpace * MilliPixel;
            return (int)(((time * 1000) * timeSpace) / tas);
        }
        private void toolTipTimer_Tick(object sender, EventArgs e)
        {
            if (toolTipTimerCounter > 0)
                toolTipTimerCounter--;
            else
            {
                toolTipTimer.Stop();
                showToolTip = true;
            }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            //calculate tick space
            int ticPixels = (1000 / MilliPixel);
            ticPixels = ((ticPixels % 10) + 10);
            int secPixels = ticPixels * 10;
            // Draw ticks
            for (int x = 0; x < this.Width; x++)
            {
                //calculate which pixel we are at in the TimeSpace
                int pix = ViewPortOffset + x;
                //each ticksSpace pixels, draw small tick
                if (pix % ticPixels == 0)
                {
                    pe.Graphics.DrawLine(new Pen(new SolidBrush(TickColor)), x, this.Height - 3, x, this.Height);
                }
                //each secPixels pixels, draw big tick and time
                if (pix % secPixels == 0)
                {
                    pe.Graphics.DrawLine(new Pen(new SolidBrush(TickColor)), x, this.Height - 5, x, this.Height);
                    //calculate the time of this pixel
                    if (x > 0)
                        pe.Graphics.DrawString(TimeFormatConvertor.To_TimeSpan_Milli(GetTime(pix)),
                            new Font("Tahoma", 8, FontStyle.Regular),
                            new SolidBrush(TickColor), new PointF(x - 32, this.Height - 20));
                    else
                        pe.Graphics.DrawString("0", new Font("Tahoma", 8, FontStyle.Regular),
                            new SolidBrush(TickColor), new PointF(0, this.Height - 20));
                }
            }
            //Draw time line
            int timeX = GetPixelOftime(currentTime);
            if (timeX >= ViewPortOffset)
            {
                pe.Graphics.DrawLine(new Pen(new SolidBrush(TimeLineColor),2),
                    timeX - ViewPortOffset, 0, timeX - ViewPortOffset, this.Height);
            }
            //Tool tip, draw only if the text is not null
            if (toolTipText != "" & showToolTip)
            {
                Size textSize = TextRenderer.MeasureText(toolTipText, new Font("Tahoma", 8, FontStyle.Regular));

                pe.Graphics.FillRectangle(new SolidBrush(ToolTipColor), toolTipLocation.X, 1,
                       textSize.Width, textSize.Height + 4);
                pe.Graphics.DrawLine(new Pen(new SolidBrush(ToolTipColor)),
                toolTipLocation.X, this.Height - 15, toolTipLocation.X, this.Height);
                pe.Graphics.DrawString(toolTipText,
                new Font("Tahoma", 8, FontStyle.Regular), new SolidBrush(ToolTipTextColor),
                new RectangleF(toolTipLocation.X, 1, textSize.Width, textSize.Height));
            }
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CaclulateMillipixels();
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (TimeChangeRequest != null)
                    TimeChangeRequest(this, new TimeChangeArgs(GetTime(e.X + ViewPortOffset), currentTime));
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
            // Cursor
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (isTimeCursorMove)
                {
                    Cursor = Cursors.VSplit;
                    toolTipLocation = e.Location;
                    showToolTip = true;
                    toolTipText = TimeFormatConvertor.To_TimeSpan_Milli(GetTime(e.X + ViewPortOffset));
                }
                else
                {
                    Cursor = Cursors.Default;
                }
            }
            else
            {
                int timeX = GetPixelOftime(currentTime);
                int max = timeX + 3;
                int min = timeX - 3;
                if (e.X >= min && e.X <= max)
                {
                    Cursor = Cursors.VSplit;
                    isTimeCursorMove = true;
                }
                else
                {
                    Cursor = Cursors.Default;
                    isTimeCursorMove = false;
                }
            }
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
