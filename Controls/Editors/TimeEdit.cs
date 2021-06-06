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

namespace AHD.SM.Controls
{
    /// <summary>
    /// Time Spanner control
    /// </summary>
    public partial class TimeEdit : UserControl
    {
        bool RiseEvent = true;
        /// <summary>
        /// Time Spanner control
        /// </summary>
        public TimeEdit()
        {
            InitializeComponent();
        }
        /// <summary>
        /// SetTime
        /// </summary>
        /// <param name="Seconds">Seconds</param>
        /// <param name="RiseEvent">RiseEvent</param>
        public void SetTime(double Seconds, bool riseEvent)
        {
            this.RiseEvent = riseEvent;
            if (Seconds >= 0)
            {
                SS_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Seconds;
                this.RiseEvent = riseEvent;
                MILI_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Milliseconds;
                SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
            }
            else
            {
                this.RiseEvent = riseEvent;
                SS_numericUpDown1.Value = 0;
                this.RiseEvent = riseEvent;
                MILI_numericUpDown1.Value = 0;
                SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.Red;
            }
        }
        /// <summary>
        /// Get current value as seconds
        /// </summary>
        /// <returns></returns>
        public double GetSeconds()
        {
            double SS = (double)SS_numericUpDown1.Value;
            double mill = (double)MILI_numericUpDown1.Value;
            mill /= 1000;
            return SS + mill;
        }
        /// <summary>
        /// Rises when the user changes the time
        /// </summary>
        public event EventHandler<EventArgs> TimeChanged;
        private void MILI_numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (MILI_numericUpDown1.Value == 1000)
            {
                MILI_numericUpDown1.Value = 0;
                SS_numericUpDown1.Value++;
            }
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;

        }
        private void SS_numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (SS_numericUpDown1.Value == 60)
            {
                SS_numericUpDown1.Value = 0;
                // MM_numericUpDown1.Value++;
            }
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            SS_numericUpDown1.BackColor = Color.White;
            MILI_numericUpDown1.BackColor = Color.White;
            base.OnEnabledChanged(e);
        }
        private void SS_numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
        }
        private void MILI_numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
        }
    }
}
