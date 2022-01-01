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
using AHD.Forms;
using System.Reflection;
using System.Resources;

namespace AHD.SM.Forms
{
    public partial class Frm_Split : Form
    {
        Subtitle sub;
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        public double SplitTime
        { get { return timeSpaner1.GetSeconds(); } }
        public Frm_Split(Subtitle sub)
        {
            this.sub = sub;
            InitializeComponent();
            label_start.Text = TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime);
            label_end.Text = TimeFormatConvertor.To_TimeSpan_Milli(sub.EndTime);

            double splitTime = sub.StartTime + 0.002;
            if (MediaPlayerManager.Position > (sub.StartTime + 0.001) && MediaPlayerManager.Position < (sub.EndTime - 0.001))
                splitTime = MediaPlayerManager.Position;
            timeSpaner1.SetTime(splitTime, false);

            trackBar1.Maximum = (int)((sub.EndTime - 0.001) * 1000);
            trackBar1.Minimum = (int)((sub.StartTime + 0.001) * 1000);
            trackBar1.Value = (int)(splitTime * 1000);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (timeSpaner1.GetSeconds() <= sub.StartTime)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_TheSplitTimeMustBeLargerThanTheOriginalSubtitlesStartTime"),
                   resources.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
            }
            if (timeSpaner1.GetSeconds() >= sub.EndTime)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_TheSplitTimeMustBeSmallerThanTheOriginalSubtitlesEndTime"),
                    resources.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timeSpaner1.SetTime((double)trackBar1.Value / 1000, false);
        }
    }
}
