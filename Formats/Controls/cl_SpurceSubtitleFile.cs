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
    public partial class cl_SpurceSubtitleFile : UserControl
    {
        SpurceSubtitleFile format;
        public cl_SpurceSubtitleFile(SpurceSubtitleFile format)
        {
            InitializeComponent();
            this.format = format;
            cl_frameRate1.SubtitlesFormat = format;
            textBox1_FontName.Text = format._FontName;
            textBox1_FontSize.Text = format._FontSize.ToString();
            checkBox1_Bold.Checked = format._Bold;
            checkBox1_ForceDisplay.Checked = format._ForceDisplay;
            checkBox1_Italic.Checked = format._Italic;
            checkBox1_TapeOffset.Checked = format._TapeOffset;
            checkBox1_UnderLined.Checked = format._UnderLined;
            comboBox1_HorzAlign.SelectedItem = format._HorzAlign;
            comboBox1_VertAlign.SelectedItem = format._VertAlign;
            numericUpDown1_Contrast1.Value = format._Contrast1;
            numericUpDown1_Contrast2.Value = format._Contrast2;
            numericUpDown1_Contrast3.Value = format._Contrast3;
            numericUpDown1_Contrast4.Value = format._Contrast4;
            numericUpDown1_FadeIn.Value = format._FadeIn;
            numericUpDown1_FadeOut.Value = format._FadeOut;
            numericUpDown1_XOffset.Value = format._XOffset;
            numericUpDown1_YOffset.Value = format._YOffset;
        }

        private void textBox1_FontName_TextChanged(object sender, EventArgs e)
        {
           format._FontName = textBox1_FontName.Text;
        }

        private void textBox1_FontSize_TextChanged(object sender, EventArgs e)
        {
            format._FontSize = Convert.ToDouble(textBox1_FontSize.Text);
        }

        private void checkBox1_Bold_CheckedChanged(object sender, EventArgs e)
        {
            format._Bold = checkBox1_Bold.Checked;
        }

        private void checkBox1_ForceDisplay_CheckedChanged(object sender, EventArgs e)
        {
            format._ForceDisplay = checkBox1_ForceDisplay.Checked;
        }

        private void checkBox1_Italic_CheckedChanged(object sender, EventArgs e)
        {
            format._Italic = checkBox1_Italic.Checked;
        }

        private void checkBox1_TapeOffset_CheckedChanged(object sender, EventArgs e)
        {
            format._TapeOffset = checkBox1_TapeOffset.Checked;
        }

        private void checkBox1_UnderLined_CheckedChanged(object sender, EventArgs e)
        {
            format._UnderLined = checkBox1_UnderLined.Checked;
        }

        private void comboBox1_HorzAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._HorzAlign = comboBox1_HorzAlign.SelectedItem.ToString();
        }

        private void comboBox1_VertAlign_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._VertAlign = comboBox1_VertAlign.SelectedItem.ToString();
        }

        private void numericUpDown1_Contrast1_ValueChanged(object sender, EventArgs e)
        {
            format._Contrast1 = (int)numericUpDown1_Contrast1.Value;
        }

        private void numericUpDown1_Contrast2_ValueChanged(object sender, EventArgs e)
        {
            format._Contrast2 = (int)numericUpDown1_Contrast2.Value;
        }

        private void numericUpDown1_Contrast3_ValueChanged(object sender, EventArgs e)
        {
            format._Contrast3 = (int)numericUpDown1_Contrast3.Value;
        }

        private void numericUpDown1_Contrast4_ValueChanged(object sender, EventArgs e)
        {
            format._Contrast4 = (int)numericUpDown1_Contrast4.Value;
        }

        private void numericUpDown1_FadeIn_ValueChanged(object sender, EventArgs e)
        {
            format._FadeIn = (int)numericUpDown1_FadeIn.Value;
        }

        private void numericUpDown1_FadeOut_ValueChanged(object sender, EventArgs e)
        {
            format._FadeOut = (int)numericUpDown1_FadeOut.Value;
        }

        private void numericUpDown1_XOffset_ValueChanged(object sender, EventArgs e)
        {
            format._XOffset = (int)numericUpDown1_XOffset.Value;
        }

        private void numericUpDown1_YOffset_ValueChanged(object sender, EventArgs e)
        {
            format._YOffset = (int)numericUpDown1_YOffset.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FontDialog Dial = new FontDialog();
            Dial.Font = new Font(textBox1_FontName.Text, (int)Convert.ToDouble(textBox1_FontSize.Text));
            Dial.ShowColor = false;
            Dial.ShowApply = false;
            Dial.ShowHelp = false;
            if (Dial.ShowDialog() == DialogResult.OK)
            {
                textBox1_FontName.Text = Dial.Font.Name;
                textBox1_FontSize.Text = Dial.Font.Size.ToString();
                checkBox1_Bold.Checked = Dial.Font.Bold;
                checkBox1_Italic.Checked = Dial.Font.Italic;
                checkBox1_UnderLined.Checked = Dial.Font.Underline;
            }
        }
    }
}
