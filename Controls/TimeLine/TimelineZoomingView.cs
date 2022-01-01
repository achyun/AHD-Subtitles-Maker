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
using System.Drawing;
using System.Windows.Forms;
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class TimelineZoomingView : Control
    {
        public TimelineZoomingView()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);
        }

        private bool show_media;
        private bool show_subtitles;
        private int viewPortWidth = 10;
        private int mediaWidth = 10;
        private int viewPortX = 0;
        private int timeLineX = 0;
        private int original_viewport_x;
        private int original_viewport_width;
        private int timeSpace;
        private int millipixel;

        private bool low_scroll;
        private bool high_scroll;
        private bool mid_scroll;

        public bool ShowMedia
        {
            get { return show_media; }
            set
            {
                show_media = value;
                Activated = show_media | show_subtitles;
            }
        }
        public bool Activated { get; set; }
        public bool ShowSubtitles
        {
            get { return show_subtitles; }
            set
            {
                show_subtitles = value;
                Activated = show_media | show_subtitles;
            }
        }

        private Point downPoint = new Point();
        private Point upPoint = new Point();

        private SubtitlesTrack track;
        private List<SubtitleBufferObject> subBuffer = new List<SubtitleBufferObject>();

        public event EventHandler<ViewPortCoordinateChangeArgs> ViewPortXChangeRequest;
        public event EventHandler<ViewPortCoordinateChangeArgs> ViewPortWidthChangeRequest;

        public void SetParameters(int millipixel, int timeSpace, int viewport, int mediaWidth)
        {
            this.millipixel = millipixel;
            this.timeSpace = timeSpace;
            this.viewPortWidth = (this.Width * viewport) / timeSpace;
            this.mediaWidth = (this.Width * mediaWidth) / timeSpace;

            CalculatePixelsForSubtitleObjects();
        }
        public void SetOffset(int offset)
        {
            if (timeSpace > 0)
                viewPortX = (this.Width * offset) / timeSpace;
        }
        public void SetTimeline(int timeLine)
        {
            if (timeSpace > 0)
            {
                timeLineX = (this.Width * timeLine) / timeSpace;
                this.Invalidate();
            }
        }
        public int CalculatePixlesToTimespace(int pixels)
        {
            return (timeSpace * pixels) / this.Width;
        }

        /// <summary>
        /// Call this when subtitles track changes. Can be null to disable subtitles draw. This will clear up
        /// the subtitles buffer. For best perfomance, call it only when changing subtitles track and when subtitle(s) 
        /// added or removed
        /// </summary>
        /// <param name="track">The subtitles track. Can be null to disable subtitles draw.</param>
        public void OnSubtitlesTrackChanged(SubtitlesTrack track)
        {
            this.track = track;
            RefreshTrack();
        }
        public void RefreshTrack()
        {
            this.subBuffer = new List<SubtitleBufferObject>();
            if (track == null)
            {
                ShowSubtitles = false;
                return;
            }
            ShowSubtitles = true;
            RebuildSubtitlesBuffer(new bool[track.Subtitles.Count]);
            CalculatePixelsForSubtitleObjects();
        }
        /// <summary>
        /// Call this when a subtitle properties change to update the subtitles buffer.
        /// </summary>
        /// <param name="sub">The subtitle to update</param>
        /// <param name="index">The index of the subtitle within the collection. Must match the exact location
        /// in the collection within the same track loaded here.</param>
        public void OnSubtitlePropertiesChanged(Subtitle sub, int index, bool selected)
        {
            // Get the object
            subBuffer[index].SubtitleStartTime = sub.StartTime;
            subBuffer[index].SubtitleEndTime = sub.EndTime;
            subBuffer[index].Text = sub.Text.ToString();
            subBuffer[index].IsSelected = selected;
            // Do the calulcation
            CalculatePixelsForSubtitlesObject(subBuffer[index]);
        }
        /// <summary>
        /// Update the selection state for a subtitle
        /// </summary>
        /// <param name="index">The index of the subtitle within the collection. Must match the exact location
        /// in the collection within the same track loaded here.</param>
        /// <param name="selected">The selection state</param>
        public void OnSubtitleSelectionChanged(int index, bool selected)
        {
            // Get the object
            subBuffer[index].IsSelected = selected;
        }
        /// <summary>
        /// Update the selection state for all subtitles.
        /// </summary>
        public void OnSubtitlesSelectionChanged(bool[] selected)
        {
            for (int i = 0; i < track.Subtitles.Count; i++)
            {
                subBuffer[i].IsSelected = selected[i];
            }
        }
        private void RebuildSubtitlesBuffer(bool[] selected)
        {
            if (track == null) return;
            // Refresh the subtitles buffer
            for (int i = 0; i < track.Subtitles.Count; i++)
            {
                SubtitleBufferObject subObject = new SubtitleBufferObject();
                subObject.SubtitleEndTime = track.Subtitles[i].EndTime;
                subObject.SubtitleStartTime = track.Subtitles[i].StartTime;
                subObject.Text = track.Subtitles[i].Text.ToString();
                subObject.IsSelected = selected[i];
                // Calculate pixels ...
                CalculatePixelsForSubtitlesObject(subObject);
                // Add it !
                this.subBuffer.Add(subObject);
            }
        }
        private void CalculatePixelsForSubtitleObjects()
        {
            foreach (SubtitleBufferObject subObject in subBuffer)
            {
                subObject.StartPixel = GetPixelOftime(subObject.SubtitleStartTime);
                subObject.EndPixel = GetPixelOftime(subObject.SubtitleEndTime);
                subObject.WidthPixels = subObject.EndPixel - subObject.StartPixel;
            }
        }
        private void CalculatePixelsForSubtitlesObject(SubtitleBufferObject subObject)
        {
            subObject.StartPixel = GetPixelOftime(subObject.SubtitleStartTime);
            subObject.EndPixel = GetPixelOftime(subObject.SubtitleEndTime);
            subObject.WidthPixels = subObject.EndPixel - subObject.StartPixel;
        }
        private int GetPixelOftime(double time)
        {
            double tas = timeSpace * millipixel;

            return (int)((this.Width * (time * 1000)) / tas);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            pevent.Graphics.DrawLine(new Pen(new SolidBrush(Color.Gray)), 0, 0, this.Width, 0);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (!Activated)
                return;
            if (ShowMedia)
            {
                // Draw a rectangel represents the media
                pe.Graphics.FillRectangle(Brushes.MediumAquamarine,
                    new Rectangle(0, 4, mediaWidth, 10));
            }
            // Draw subtitles
            if (ShowSubtitles)
            {
                foreach (SubtitleBufferObject subObject in subBuffer)
                {
                    if (subObject.WidthPixels > 0)
                    {
                        pe.Graphics.FillRectangle(subObject.IsSelected ? Brushes.Purple : Brushes.Navy,
                        new Rectangle(subObject.StartPixel,
                        16,
                        subObject.WidthPixels,
                        8));
                    }
                    else
                    {
                        // Draw simple line
                        pe.Graphics.DrawLine(new Pen(subObject.IsSelected ? Brushes.Purple : Brushes.Navy),
                            subObject.StartPixel, 16,
                            subObject.StartPixel, 24);
                    }
                }
            }

            // Draw the timeline
            pe.Graphics.DrawLine(Pens.Black, new Point(timeLineX, 0), new Point(timeLineX, this.Height));

            // Draw a rectangel represents the view port
            pe.Graphics.DrawRectangle(new Pen(Brushes.Blue, 2),
                new Rectangle(viewPortX, 1, viewPortWidth, this.Height - 2));
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            downPoint = e.Location;
            original_viewport_x = viewPortX;
            original_viewport_width = viewPortWidth;
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            upPoint = e.Location;
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                int shift = e.X - downPoint.X;
                if (mid_scroll)
                {
                    // Doing a mid movment
                    int newviewPortX = original_viewport_x + shift;

                    if (newviewPortX < 0)
                        newviewPortX = 0;

                    if (newviewPortX + viewPortWidth >= this.Width)
                        newviewPortX = this.Width - viewPortWidth;

                    if (ViewPortXChangeRequest != null)
                        ViewPortXChangeRequest(this, new ViewPortCoordinateChangeArgs(CalculatePixlesToTimespace(newviewPortX),
                            CalculatePixlesToTimespace(original_viewport_x)));

                    viewPortX = newviewPortX;
                }
                else if (high_scroll)
                {
                    // Calculate the viewport position
                    int original_max_dist = original_viewport_x + original_viewport_width;

                    int newviewPortX = original_viewport_x;// View port should not get affect

                    if (newviewPortX < 0)
                        newviewPortX = 0;

                    if (newviewPortX + viewPortWidth >= this.Width)
                        newviewPortX = this.Width - viewPortWidth;

                    // Calculate the new width depending on the new viewport
                    int newWidth = original_viewport_width + shift;
                    if (newWidth <= 0)
                        newWidth = 1;

                    if (newWidth + viewPortX > this.Width)
                        newWidth = this.Width - viewPortX;

                    if (ViewPortWidthChangeRequest != null)
                        ViewPortWidthChangeRequest(this, new ViewPortCoordinateChangeArgs(CalculatePixlesToTimespace(newWidth),
                            CalculatePixlesToTimespace(original_viewport_width)));

                    if (ViewPortXChangeRequest != null)
                        ViewPortXChangeRequest(this, new ViewPortCoordinateChangeArgs(CalculatePixlesToTimespace(newviewPortX),
                            CalculatePixlesToTimespace(original_viewport_x)));

                    // No need to update the viewport width, the parent control
                    // should do this for us.


                    viewPortX = newviewPortX;
                }
                else if (low_scroll)
                {
                    // Calculate the viewport position
                    int original_max_dist = original_viewport_x + original_viewport_width;

                    int newviewPortX = original_viewport_x + shift;

                    if (newviewPortX < 0)
                        newviewPortX = 0;

                    if (newviewPortX + viewPortWidth >= this.Width)
                        newviewPortX = this.Width - viewPortWidth;

                    // Calculate the new width depending on the new viewport
                    int newWidth = original_max_dist - newviewPortX;
                    if (newWidth <= 0)
                        newWidth = 1;

                    if (newWidth + viewPortX > this.Width)
                        newWidth = this.Width - viewPortX;

                    if (ViewPortWidthChangeRequest != null)
                        ViewPortWidthChangeRequest(this, new ViewPortCoordinateChangeArgs(CalculatePixlesToTimespace(newWidth),
                            CalculatePixlesToTimespace(original_viewport_width)));

                    if (ViewPortXChangeRequest != null)
                        ViewPortXChangeRequest(this, new ViewPortCoordinateChangeArgs(CalculatePixlesToTimespace(newviewPortX),
                            CalculatePixlesToTimespace(original_viewport_x)));


                    // No need to update the viewport width, the parent control
                    // should do this for us.


                    viewPortX = newviewPortX;
                }
            }
            else
            {
                int max_dist = viewPortX + viewPortWidth;
                if (e.X >= viewPortX - 3 && e.X <= max_dist + 3)
                {
                    // On the rectangle area
                    int min = viewPortX - 3;
                    int max = viewPortX + 3;

                    if (e.X >= min && e.X <= max)
                    {
                        Cursor = Cursors.PanWest;
                        low_scroll = true;
                        mid_scroll = false;
                        high_scroll = false;
                    }
                    else
                    {
                        low_scroll = false;
                        min = max_dist - 3;
                        max = max_dist + 3;
                        if (e.X >= min && e.X <= max)
                        {
                            Cursor = Cursors.PanEast;
                            high_scroll = true;
                            mid_scroll = false;
                        }
                        else
                        {
                            // in the middle !!
                            Cursor = Cursors.SizeAll;
                            high_scroll = false;
                            mid_scroll = true;
                        }
                    }
                }
                else
                {
                    low_scroll = false;
                    high_scroll = false;
                    mid_scroll = false;
                    Cursor = Cursors.Default;
                }
            }
        }
    }
    public class ViewPortCoordinateChangeArgs : EventArgs
    {
        public ViewPortCoordinateChangeArgs(int newValue, int oldValue)
        {
            this.NewValue = newValue;
            this.OldValue = oldValue;
        }
        public int OldValue { get; private set; }
        public int NewValue { get; private set; }
    }
}
