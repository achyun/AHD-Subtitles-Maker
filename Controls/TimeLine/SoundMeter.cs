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
using System.Drawing;
using System.Windows.Forms;
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class SoundMeter : Control
    {
        public SoundMeter()
        {
            InitializeComponent();
            ControlStyles flag = ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint;
            this.SetStyle(flag, true);
            MaxDB = 40;
        }
        public bool IsStereo { get; set; }
        public double MaxDB { get; set; }
        double channelLeft;
        double channelRight;

        private int dbToPixel(double db, int height)
        {
            return (int)((db * height) / MaxDB);
        }
        public void SetTime(double time, int millis)
        {
            if (!WaveReader.BufferPresented)
                return;
            if (WaveReader.BufferNmChannel == 2)
            {
                int sampleLeftPOS, sampleLeftNEG, sampleRightPOS, sampleRightNEG = 0;
                WaveReader.GetPixelValuesFromBufferSTEREO(time, millis, out sampleLeftPOS, out sampleLeftNEG, out sampleRightPOS, out sampleRightNEG);

                SetValues((sampleLeftPOS + sampleLeftNEG) / 2, (sampleRightPOS + sampleRightNEG) / 2);
            }
            else if (WaveReader.BufferNmChannel == 1)
            {
                int sampleLeftPOS, sampleLeftNEG = 0;
                WaveReader.GetPixelValuesFromBufferMONO(time, millis, out sampleLeftPOS, out sampleLeftNEG);

                SetValues((sampleLeftPOS + sampleLeftNEG) / 2);
            }
        }
        public void SetValues(int channelLeft, int channelRight)
        {
            double left = WaveReader.ScaleBufferPixel((double)channelLeft, (double)1);
            this.channelLeft = 20 * Math.Log10(left);

            double right = WaveReader.ScaleBufferPixel((double)channelRight, (double)1);
            this.channelRight = 20 * Math.Log10(right);

            Invalidate();
        }
        public void SetValues(int channelMono)
        {
            double left = WaveReader.ScaleBufferPixel((double)channelMono, (double)1);
            this.channelLeft = 20 * Math.Log10(left);

            Invalidate();
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (WaveReader.BufferPresented)
            {
                if (WaveReader.BufferNmChannel == 2)
                {
                    // Draw both channels
                    int left = dbToPixel(channelLeft, Height);
                    int right = dbToPixel(channelRight, Height);
                    pe.Graphics.FillRectangle(Brushes.MediumSeaGreen, new Rectangle(0, -left, Width / 2, Height + left));
                    pe.Graphics.FillRectangle(Brushes.MediumSeaGreen, new Rectangle(Width / 2, -right, Width / 2, Height + right));
                }
                else if (WaveReader.BufferNmChannel == 1)
                {
                    int left = dbToPixel(channelLeft, Height);
                    pe.Graphics.FillRectangle(Brushes.MediumSeaGreen, new Rectangle(0, -left, Width, Height + left));
                }
            }

            // Draw grid lines
            pe.Graphics.DrawString("0dB", Font, Brushes.White, 1, 1);
            pe.Graphics.DrawString(-MaxDB + "dB", Font, Brushes.White, 1, Height - 12);

            for (int i = 10; i < MaxDB; i += 10)
            {
                int dd = dbToPixel(i, Height);

                pe.Graphics.DrawLine(Pens.White, 0, dd, Width, dd);
                pe.Graphics.DrawString(-i + "dB", Font, Brushes.White, 1, dd - 12);
            }

            pe.Graphics.DrawRectangle(Pens.White, 0, 0, Width - 1, Height - 1);
        }
    }
}
