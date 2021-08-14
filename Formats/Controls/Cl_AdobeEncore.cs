﻿// This file is part of AHD Subtitles Maker.
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

namespace AHD.SM.Formats
{
    public partial class Cl_AdobeEncore : UserControl
    {
        AdobeEncore format;
        public Cl_AdobeEncore(AdobeEncore format)
        {
            this.format = format;
            InitializeComponent();

            cl_frameRate1.SubtitlesFormat = format;
            checkBox1.Checked = format.UseNumbers;
            checkBox2.Checked = format.TabsMode;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            format.UseNumbers = checkBox1.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            format.TabsMode = checkBox2.Checked;
        }
    }
}
