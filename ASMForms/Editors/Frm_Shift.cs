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
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
namespace AHD.SM.Forms
{
    /// <summary>
    /// Time shift dialog
    /// </summary>
    public partial class Frm_Shift : Form
    {
        bool _Ok = false;
        int _CurrentStart = 0;
        int _CurrentEnd = 0;
        int _PeviousSubtitle = 0;
        int _NextSubtitle = 0;
        double _Shifted = 0;
        int _ShiftPeriod = 0;
        ToolTip tipp = new ToolTip();
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
           Assembly.GetExecutingAssembly());
        /// <summary>
        /// If the user made changes
        /// </summary>
        public bool OK
        { get { return _Ok; } }
        /// <summary>
        /// Get the shifted time by the user
        /// </summary>
        public double Shifted
        { get { _Shifted /= 1000; return _Shifted; } }
        /// <summary>
        /// Time shift dialog
        /// </summary>
        /// <param name="CurrentStart">The current subtitle (or first subtitle if group)
        /// start time</param>
        /// <param name="CurrentEnd">The current subtitle (or first subtitle if group)
        /// end time</param>
        /// <param name="PeviousSubtitle">The previous subtitle for current subtitle (or first subtitle if group) end time</param>
        /// <param name="NextSubtitle">The next subtitle for current subtitle (or last subtitle if group) start time</param>
        /// <param name="ShiftTimePeriod">The period of the shift</param>
        public Frm_Shift(double CurrentStart, double CurrentEnd, double Duration, double PeviousSubtitle, double NextSubtitle, int ShiftTimePeriod)
        {
            InitializeComponent();
            _ShiftPeriod = ShiftTimePeriod;
            button2.Text = "+ 0." + _ShiftPeriod.ToString();
            button4.Text = "- 0." + _ShiftPeriod.ToString();
            //View the lables
            if (TimeSpan.FromSeconds(PeviousSubtitle).ToString().Length >= 12)
                label1_prev.Text = "|\n" + TimeSpan.FromSeconds(PeviousSubtitle).ToString().Substring(0, 12);
            else
                label1_prev.Text = "|\n" + TimeSpan.FromSeconds(PeviousSubtitle).ToString() + ".000";
            if (TimeSpan.FromSeconds(NextSubtitle - Duration).ToString().Length >= 12)
                label2_nxt.Text = "                    |\n" + TimeSpan.FromSeconds(NextSubtitle - Duration).ToString().Substring(0, 12);
            else
                label2_nxt.Text = "                    |\n" + TimeSpan.FromSeconds(NextSubtitle - Duration).ToString() + ".000";
            //assign values
            CurrentStart *= 1000;
            _CurrentStart = (int)(CurrentStart);
            CurrentEnd *= 1000;
            _CurrentEnd = (int)(CurrentEnd);
            PeviousSubtitle *= 1000;
            _PeviousSubtitle = (int)(PeviousSubtitle);
            NextSubtitle *= 1000;
            _NextSubtitle = (int)(NextSubtitle);
            Duration *= 1000;
            int Dura = (int)(Duration);
            //Set tracks values
            trackBar1_Start.Maximum = _NextSubtitle - Dura;
            trackBar1_Start.Minimum = _PeviousSubtitle;
            trackBar1_Start.Value = _CurrentStart;
            timeSpaner1.SetTime(0, true);
            //Start
            tipp.SetToolTip(button2, resources.GetString("ToolTip_ShiftRight") + " (+ 10)");
            tipp.SetToolTip(button4, resources.GetString("ToolTip_ShiftLeft") + " (- 10)");
            tipp.SetToolTip(button6, resources.GetString("ToolTip_ResetTheShiftToZero"));
            tipp.SetToolTip(label1, resources.GetString("ToolTip_TheRealTime"));
            //The real time
            double REAL = trackBar1_Start.Value / 1000.000;
            if (TimeSpan.FromSeconds(REAL).ToString().Length >= 12)
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString().Substring(0, 12) + " >";
            else
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString() + ".000 >";
        }
        private void button3_Click(object sender, EventArgs e)
        {
            _Ok = false;
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _Ok = true;
            this.Close();
        }
        private void trackBar1_Start_Scroll(object sender, EventArgs e)
        {
            _Shifted = trackBar1_Start.Value - _CurrentStart;
            double shhh = _Shifted / 1000;
            if (shhh >= 0)
            {
                timeSpaner1.SetTime(shhh, true);
            }
            else
            {
                shhh *= -1;
                timeSpaner1.SetTime(shhh, true);
            }
            double REAL = trackBar1_Start.Value / 1000.000;
            if (TimeSpan.FromSeconds(REAL).ToString().Length >= 12)
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString().Substring(0, 12) + " >";
            else
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString() + ".000 >";
        }
        private void timeSpaner1_TimeChanged(object sender, EventArgs e)
        {
            textBox1.Text = timeSpaner1.GetSeconds().ToString("F3");
            if (_Shifted == 0)
            { textBox1.Text = timeSpaner1.GetSeconds().ToString(); }
            else if (_Shifted < 0)
            { textBox1.Text = "- " + timeSpaner1.GetSeconds(); }
            else if (_Shifted > 0)
            { textBox1.Text = "+ " + timeSpaner1.GetSeconds(); }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            string helpPath = ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpPath))
                Help.ShowHelp(this, helpPath, HelpNavigator.KeywordIndex, "How to, Shift time");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", HelpNavigator.KeywordIndex, "How to, Shift time");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar1_Start.Value += _ShiftPeriod;
                _Shifted = trackBar1_Start.Value - _CurrentStart;
                double shhh = _Shifted / 1000;
                if (shhh >= 0)
                {
                    timeSpaner1.SetTime(shhh, true);
                }
                else
                {
                    shhh *= -1;
                    timeSpaner1.SetTime(shhh, true);
                }
                double REAL = trackBar1_Start.Value / 1000.000;
                if (TimeSpan.FromSeconds(REAL).ToString().Length >= 12)
                    label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString().Substring(0, 12) + " >";
                else
                    label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString() + ".000 >";
            }
            catch { }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                trackBar1_Start.Value -= _ShiftPeriod;
                _Shifted = trackBar1_Start.Value - _CurrentStart;
                double shhh = _Shifted / 1000;
                if (shhh >= 0)
                {
                    timeSpaner1.SetTime(shhh, true);
                }
                else
                {
                    shhh *= -1;
                    timeSpaner1.SetTime(shhh, true);
                }
                double REAL = trackBar1_Start.Value / 1000.000;
                if (TimeSpan.FromSeconds(REAL).ToString().Length >= 12)
                    label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString().Substring(0, 12) + " >";
                else
                    label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString() + ".000 >";
            }
            catch { }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            trackBar1_Start.Value = _CurrentStart;
            timeSpaner1.SetTime(0, true);
            double REAL = trackBar1_Start.Value / 1000.000;
            if (TimeSpan.FromSeconds(REAL).ToString().Length >= 12)
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString().Substring(0, 12) + " >";
            else
                label1.Text = "< " + TimeSpan.FromSeconds(REAL).ToString() + ".000 >";
        }
    }
}
