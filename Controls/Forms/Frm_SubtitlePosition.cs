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
    public partial class Frm_SubtitlePosition : Form
    {
        SubtitleText text;

        public Frm_SubtitlePosition(SubtitleText text)
        {
            this.text = text;
            InitializeComponent();
            //set position
            if (text.IsCustomPosition)
            {
                radioButton_custom.Checked = true;
            }
            else
            {
                switch (text.Position)
                {
                    case SubtitlePosition.Down_Middle: radioButton_Bottom.Checked = true; break;
                    case SubtitlePosition.Down_Left: radioButton_Bottom_Left.Checked = true; break;
                    case SubtitlePosition.Down_Right: radioButton_Bottom_Right.Checked = true; break;
                    case SubtitlePosition.Mid_Middle: radioButton_middle.Checked = true; break;
                    case SubtitlePosition.Mid_Left: radioButton_Middle_Left.Checked = true; break;
                    case SubtitlePosition.Mid_Right: radioButton_Middle_Right.Checked = true; break;
                    case SubtitlePosition.Top_Middle: radioButton_Top.Checked = true; break;
                    case SubtitlePosition.Top_Left: radioButton_Top_left.Checked = true; break;
                    case SubtitlePosition.Top_right: radioButton_Top_right.Checked = true; break;
                }
            }
            numericUpDown_X.Value = text.CustomPosition.X;
            numericUpDown_Y.Value = text.CustomPosition.Y;
            ShowSample();
        }
        void ShowSample()
        {
            int x = 0;
            int y = 0;
            if (radioButton_Bottom.Checked)
            {
                x = (panel1.Width / 2) - (label_Sample.Width / 2);
                y = panel1.Height - (5 + label_Sample.Height);
            }
            else if (radioButton_Bottom_Left.Checked)
            {
                x = 5;
                y = panel1.Height - (5 + label_Sample.Height);
            }
            else if (radioButton_Bottom_Right.Checked)
            {
                x = panel1.Width - (label_Sample.Width + 5);
                y = panel1.Height - (5 + label_Sample.Height);
            }
            else if (radioButton_middle.Checked)
            {
                x = (panel1.Width / 2) - (label_Sample.Width / 2);
                y = (panel1.Height / 2) - (label_Sample.Height / 2);
            }
            else if (radioButton_Middle_Left.Checked)
            {
                x = 5;
                y = (panel1.Height / 2) - (label_Sample.Height / 2);
            }
            else if (radioButton_Middle_Right.Checked)
            {
                x = panel1.Width - (label_Sample.Width + 5);
                y = (panel1.Height / 2) - (label_Sample.Height / 2);
            }
            else if (radioButton_Top.Checked)
            {
                x = (panel1.Width / 2) - (label_Sample.Width / 2);
                y = 5;
            }
            else if (radioButton_Top_left.Checked)
            {
                x = 5;
                y = 5;
            }
            else if (radioButton_Top_right.Checked)
            {
                x = panel1.Width - (label_Sample.Width + 5);
                y = 5;
            }
            else if (radioButton_custom.Checked)
            {
                x = (int)numericUpDown_X.Value;
                y = (int)numericUpDown_Y.Value;
            }
            label_Sample.Location = new Point(x, y);
        }
        private void radioButton_Top_left_CheckedChanged(object sender, EventArgs e)
        {
            ShowSample();
        }
        private void numericUpDown_X_ValueChanged(object sender, EventArgs e)
        {
            ShowSample();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SubtitlePosition _NewLocation = SubtitlePosition.Down_Middle;
            text.IsCustomPosition = radioButton_custom.Checked;
            text.CustomPosition = new Point((int)numericUpDown_X.Value,
                (int)numericUpDown_Y.Value);
            if (radioButton_Bottom.Checked)
            {
                _NewLocation = SubtitlePosition.Down_Middle;
            }
            else if (radioButton_Bottom_Left.Checked)
            {
                _NewLocation = SubtitlePosition.Down_Left;
            }
            else if (radioButton_Bottom_Right.Checked)
            {
                _NewLocation = SubtitlePosition.Down_Right;
            }
            else if (radioButton_middle.Checked)
            {
                _NewLocation = SubtitlePosition.Mid_Middle;
            }
            else if (radioButton_Middle_Left.Checked)
            {
                _NewLocation = SubtitlePosition.Mid_Left;
            }
            else if (radioButton_Middle_Right.Checked)
            {
                _NewLocation = SubtitlePosition.Mid_Right;
            }
            else if (radioButton_Top.Checked)
            {
                _NewLocation = SubtitlePosition.Top_Middle;
            }
            else if (radioButton_Top_left.Checked)
            {
                _NewLocation = SubtitlePosition.Top_Left;
            }
            else if (radioButton_Top_right.Checked)
            {
                _NewLocation = SubtitlePosition.Top_right;
            }
            text.Position = _NewLocation;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
