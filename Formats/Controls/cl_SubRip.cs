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
using System.Windows.Forms;

namespace AHD.SM.Formats
{
    public partial class cl_SubRip : UserControl
    {
        SubRip format;
        public cl_SubRip(SubRip format)
        {
            InitializeComponent();
            this.format = format;

            checkBox_use_ass.Checked = format.UseAss;
            checkBox_write_colors.Checked = format.WriteColors;
            checkBox_write_font_sizes.Checked = format.WriteFontSizes;
            checkBox_write_fonts.Checked = format.WriteFonts;
            checkBox_write_pos.Checked = format.WriteAlignments;
        }

        private void checkBox_write_colors_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteColors = checkBox_write_colors.Checked;
        }
        private void checkBox_write_fonts_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteFonts = checkBox_write_fonts.Checked;
        }
        private void checkBox_write_font_sizes_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteFontSizes = checkBox_write_font_sizes.Checked;
        }
        private void checkBox_use_ass_CheckedChanged(object sender, EventArgs e)
        {
            format.UseAss = checkBox_use_ass.Checked;
        }
        private void checkBox_write_pos_CheckedChanged(object sender, EventArgs e)
        {
            format.WriteAlignments = checkBox_write_pos.Checked;
        }
    }
}
