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
    public partial class TimeSpaner : UserControl
    {
        bool RiseEvent = false;
        bool isNegative = false;
        /// <summary>
        /// Time Spanner control
        /// </summary>
        public TimeSpaner()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set time
        /// </summary>
        /// <param name="Seconds"></param>
        /// <param name="RiseEvent"></param>
        public void SetTime(double Seconds, bool RiseEvent)
        {
            isNegative = Seconds < 0;
            if (Seconds < 0)
            {
                Seconds *= -1;
                HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.Red;
            }
            else
                HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
            this.RiseEvent = RiseEvent;
            HH_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Hours;
            this.RiseEvent = RiseEvent;
            MM_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Minutes;
            this.RiseEvent = RiseEvent;
            SS_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Seconds;
            this.RiseEvent = RiseEvent;
            MILI_numericUpDown1.Value = TimeSpan.FromSeconds(Seconds).Milliseconds;
        }
        /// <summary>
        /// Get current value as seconds
        /// </summary>
        /// <returns></returns>
        public double GetSeconds()
        {
            double HH = (double)HH_numericUpDown1.Value * 3600;
            double MM = (double)MM_numericUpDown1.Value * 60;
            double SS = HH + MM + (double)SS_numericUpDown1.Value;
            double mill = (double)MILI_numericUpDown1.Value;
            mill /= 1000;
            return isNegative ? -(SS + mill) : (SS + mill);
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
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
        }
        private void SS_numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (SS_numericUpDown1.Value == 60)
            {
                SS_numericUpDown1.Value = 0;
                MM_numericUpDown1.Value++;
            }
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
        }
        private void MM_numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (MM_numericUpDown1.Value == 60)
            {
                MM_numericUpDown1.Value = 0;
                HH_numericUpDown1.Value++;
            }
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
        }
        private void HH_numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            HH_numericUpDown1.BackColor = Color.White;
            MM_numericUpDown1.BackColor = Color.White;
            SS_numericUpDown1.BackColor = Color.White;
            MILI_numericUpDown1.BackColor = Color.White;
            base.OnEnabledChanged(e);
        }

        private void HH_numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
            isNegative = false;
        }
        private void MM_numericUpDown1_KeyUp(object sender, KeyEventArgs e)
        {
            if (RiseEvent)
            {
                if (TimeChanged != null)
                { TimeChanged(this, e); }
            }
            else
                RiseEvent = true;
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
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
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
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
            isNegative = false;
            HH_numericUpDown1.BackColor = MM_numericUpDown1.BackColor = SS_numericUpDown1.BackColor = MILI_numericUpDown1.BackColor = Color.White;
        }
    }
}
