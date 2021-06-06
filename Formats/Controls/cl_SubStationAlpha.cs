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

namespace AHD.SM.Formats
{
    public partial class cl_SubStationAlpha : UserControl
    {
        SubStationAlpha format;
        public cl_SubStationAlpha(SubStationAlpha format)
        {
            InitializeComponent();
            this.format = format;
            checkBox_write_colors.Checked = format.WriteColors;
            checkBox_write_font_sizes.Checked = format.WriteFontSizes;
            checkBox_write_fonts.Checked = format.WriteFonts;
            checkBox_write_pos.Checked = format.WritePositions;
            textBox1_BackColour.Text = format.BackColour.ToString();
            textBox1_fontname.Text = format.Fontname;
            textBox1_fontsize.Text = format.Fontsize.ToString();
            textBox1_name.Text = format.name;
            textBox1_PrimaryColour.Text = format.PrimaryColour.ToString();
            textBox1_TertiaryColour.Text = format.TertiaryColour.ToString();
            textBox2_SecondaryColour.Text = format.SecondaryColour.ToString();
            checkBox1_Bold.Checked = (format.Bold == 1);
            checkBox2_Italic.Checked = (format.Italic == 1);
            numericUpDown_MarginL.Value = format.MarginL;
            numericUpDown_MarginR.Value = format.MarginR;
            numericUpDown_MarginV.Value = format.MarginV;

            button_SecondaryColour.BackColor = Color.FromArgb(format.SecondaryColour);
            button_TertiaryColour.BackColor = Color.FromArgb(format.TertiaryColour);
            button1_BackColour.BackColor = Color.FromArgb(format.BackColour);
            button1_PrimaryColour.BackColor = Color.FromArgb(format.PrimaryColour);
        }

        private void textBox1_name_TextChanged(object sender, EventArgs e)
        {
            format.name = textBox1_name.Text;
        }

        private void textBox1_fontname_TextChanged(object sender, EventArgs e)
        {
            format.Fontname = textBox1_fontname.Text;
        }

        private void textBox1_fontsize_TextChanged(object sender, EventArgs e)
        {
            format.Fontsize = Convert.ToDouble(textBox1_fontsize.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FontDialog dial = new FontDialog();
            FontStyle style = checkBox1_Bold.Checked ? FontStyle.Bold : FontStyle.Regular;
            if (checkBox2_Italic.Checked)
                style |= System.Drawing.FontStyle.Italic;
            dial.Font = new Font(textBox1_fontname.Text, (float)Convert.ToDouble(textBox1_fontsize.Text), style);
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1_fontname.Text = dial.Font.Name;
                textBox1_fontsize.Text = dial.Font.Size.ToString();
                checkBox1_Bold.Checked = (dial.Font.Style & FontStyle.Bold) == FontStyle.Bold;
                checkBox2_Italic.Checked = (dial.Font.Style & FontStyle.Italic) == FontStyle.Italic;
            }
        }

        private void checkBox1_Bold_CheckedChanged(object sender, EventArgs e)
        {
            format.Bold = checkBox1_Bold.Checked ? 1 : 0;
        }

        private void checkBox2_Italic_CheckedChanged(object sender, EventArgs e)
        {
            format.Italic = checkBox2_Italic.Checked ? 1 : 0;
        }

        private void numericUpDown_MarginL_ValueChanged(object sender, EventArgs e)
        {
            format.MarginL = (int)numericUpDown_MarginL.Value;
        }

        private void numericUpDown_MarginR_ValueChanged(object sender, EventArgs e)
        {
            format.MarginR = (int)numericUpDown_MarginR.Value;
        }

        private void numericUpDown_MarginV_ValueChanged(object sender, EventArgs e)
        {
            format.MarginV = (int)numericUpDown_MarginV.Value;
        }

        private void button1_PrimaryColour_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button1_PrimaryColour.BackColor;
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button1_PrimaryColour.BackColor = dial.Color;
                textBox1_PrimaryColour.Text = dial.Color.ToArgb().ToString();
                format.PrimaryColour = button1_PrimaryColour.BackColor.ToArgb();
            }
        }

        private void button_SecondaryColour_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button_SecondaryColour.BackColor;
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button_SecondaryColour.BackColor = dial.Color;
                textBox2_SecondaryColour.Text = dial.Color.ToArgb().ToString();
                format.SecondaryColour = textBox2_SecondaryColour.BackColor.ToArgb();
            }
        }

        private void button_TertiaryColour_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button_TertiaryColour.BackColor;
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button_TertiaryColour.BackColor = dial.Color;
                textBox1_TertiaryColour.Text = dial.Color.ToArgb().ToString();
                format.TertiaryColour = textBox1_TertiaryColour.BackColor.ToArgb();
            }
        }

        private void button1_BackColour_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button1_BackColour.BackColor;
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                button1_BackColour.BackColor = dial.Color;
                textBox1_BackColour.Text = dial.Color.ToArgb().ToString();
                format.BackColour = button1_BackColour.BackColor.ToArgb();
            }
        }
        private void checkBox_write_colors_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteColors = checkBox_write_colors.Checked;
        }
        private void checkBox_write_font_sizes_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteFontSizes = checkBox_write_font_sizes.Checked;
        }
        private void checkBox_write_fonts_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteFonts = checkBox_write_fonts.Checked;
        }
        private void checkBox_write_pos_CheckedChanged(object sender, EventArgs e)
        {
            format.WritePositions = checkBox_write_pos.Checked;
        }
    }
}
