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
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHD.ID3.Editor.GUI
{
    public partial class ImagePanel : Control
    {
        public ImagePanel()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);
        }
        public ImageViewMode ImageViewMode = ImageViewMode.StretchIfLarger;
        Bitmap imageToView;

        public bool DrawDefaultImageWhenViewImageIsNull = true;
        public string DefaultStringToDraw = "No image.";
        public int viewImageWidth = 0;
        public int viewImageHeight = 0;
        public int drawX = 0;
        public int drawY = 0;
        public int zoom = 0;
        public Color ImageBackgroundColor = Color.WhiteSmoke;

        int olddrawX = 0;
        int olddrawY = 0;
        Point downPoint;

        //events
        public event EventHandler DisableScrollBars;
        public event EventHandler EnableScrollBars;
        public event EventHandler CalculateScrollValues;

        //properties
        public Bitmap ImageToView
        {
            get { return imageToView; }
            set
            {
                imageToView = value;
                zoom = 0;
                CalculateImageValues();
                if (value != null)
                    if (ImageAnimator.CanAnimate(value))
                    {
                        ImageAnimator.Animate(value, new EventHandler(onAnimate));
                    }
            }
        }

        public void CalculateImageValues()
        {
            if (imageToView == null)
            {
                if (DisableScrollBars != null)
                    DisableScrollBars(this, null);
                return;
            } 
            
            switch (this.ImageViewMode)
            {
                case ImageViewMode.StretchIfLarger:
                    CalculateStretchedImageValues();
                    CenterImage();
                    if (DisableScrollBars != null)
                        DisableScrollBars(this, null);
                    break;
                case ImageViewMode.StretchToFit:
                    CalculateStretchToFitImageValues();
                    CenterImage();
                    if (DisableScrollBars != null)
                        DisableScrollBars(this, null);
                    break;
                case ImageViewMode.Normal:
                    CalculateNormalImageValues();
                    CenterImage();
                    if (EnableScrollBars != null)
                        EnableScrollBars(this, null);
                    if (CalculateScrollValues != null)
                        CalculateScrollValues(this, null);
                    break;
            }
        }
        void CalculateStretchedImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)imageToView.Width / imageToView.Height;

            if (this.Width >= imageToView.Width && this.Height >= imageToView.Height)
            {
                viewImageWidth = imageToView.Width;
                viewImageHeight = imageToView.Height;
            }
            else if (this.Width > imageToView.Width && this.Height < imageToView.Height)
            {
                viewImageHeight = this.Height;
                viewImageWidth = (int)(this.Height * imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height > imageToView.Height)
            {
                viewImageWidth = this.Width;
                viewImageHeight = (int)(this.Width / imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height < imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        void CalculateStretchToFitImageValues()
        {
            float pRatio = (float)this.Width / this.Height;
            float imRatio = (float)imageToView.Width / imageToView.Height;

            if (this.Width >= imageToView.Width && this.Height >= imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }
            else if (this.Width > imageToView.Width && this.Height < imageToView.Height)
            {
                viewImageHeight = this.Height;
                viewImageWidth = (int)(this.Height * imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height > imageToView.Height)
            {
                viewImageWidth = this.Width;
                viewImageHeight = (int)(this.Width / imRatio);
            }
            else if (this.Width < imageToView.Width && this.Height < imageToView.Height)
            {
                if (this.Width >= this.Height)
                {
                    //width image
                    if (imageToView.Width >= imageToView.Height && imRatio >= pRatio)
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                    else
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                }
                else
                {
                    if (imageToView.Width < imageToView.Height && imRatio < pRatio)
                    {
                        viewImageHeight = this.Height;
                        viewImageWidth = (int)(this.Height * imRatio);
                    }
                    else
                    {
                        viewImageWidth = this.Width;
                        viewImageHeight = (int)(this.Width / imRatio);
                    }
                }
            }

        }
        void CalculateNormalImageValues()
        {
            viewImageWidth = imageToView.Width + zoom;
            viewImageHeight = imageToView.Height + zoom;
        }
        void CenterImage()
        {
            int y = (int)((this.Height - viewImageHeight) / 2.0);
            int x = (int)((this.Width - viewImageWidth) / 2.0);
            drawY = y;
            drawX = x;
        }
        private void onAnimate(object sender, EventArgs e) 
        {
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (ImageToView == null)
            {
                if (DrawDefaultImageWhenViewImageIsNull)
                {
                    pe.Graphics.FillRectangle(new LinearGradientBrush(new Point(), new Point(0, this.Height),
                        Color.White, ImageBackgroundColor), 
                        new Rectangle(0, 0, this.Width, this.Height));
                    pe.Graphics.DrawString(DefaultStringToDraw, new Font("Tahoma", 8, FontStyle.Regular),
                        Brushes.Black, new PointF(5, 5));
                }
                return;
            }
            ImageAnimator.UpdateFrames();
            switch (this.ImageViewMode)
            {
                case ImageViewMode.StretchIfLarger:
                    CalculateStretchedImageValues();
                    CenterImage();
                    break;
                case ImageViewMode.StretchToFit:
                    CalculateStretchToFitImageValues();
                    CenterImage();
                    break;
                case ImageViewMode.Normal:
                    CalculateNormalImageValues();
                    break;
            }
            pe.Graphics.DrawImage(imageToView,
            new Rectangle(drawX, drawY, viewImageWidth, viewImageHeight));
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.ImageViewMode == ImageViewMode.Normal)
            {
                CenterImage();
                if (CalculateScrollValues != null)
                    CalculateScrollValues(this, null);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
           // downPoint = e.Location;
           // olddrawX = drawX;
           // olddrawY = drawY;
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            /*if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (e.X != downPoint.X || e.Y != downPoint.Y)
                {
                    this.Cursor = Cursors.Hand;
                    int xShift = e.X - downPoint.X;
                    int yShift = e.Y - downPoint.Y;

                    drawX = olddrawX + xShift;
                    drawY = olddrawY + yShift;

                    this.Invalidate();
                }
            }*/
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
           // this.Cursor = Cursors.Default;
            base.OnMouseUp(e);
        }
    }
}
