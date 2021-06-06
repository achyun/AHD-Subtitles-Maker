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
    public partial class Cl_ABCiView : UserControl
    {
        ABCiView format;
        public Cl_ABCiView(ABCiView format)
        {
            InitializeComponent();
            foreach (System.Drawing.FontFamily fam in System.Drawing.FontFamily.Families)
            {
                comboBox2.Items.Add(fam.Name);
            }
            cl_frameRate1.SubtitlesFormat = format;
            this.format = format;
            textBox1.Text = format._movie;
            textBox2.Text = format._language;
            comboBox2.SelectedItem = format._font;
            comboBox1.SelectedItem = format._style;
            timeEdit1.SetTime(format._size, false);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            format._movie = textBox1.Text;
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            format._language = textBox2.Text;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._style = comboBox1.SelectedItem.ToString();
        }
        private void timeEdit1_TimeChanged(object sender, EventArgs e)
        {
            format._size = timeEdit1.GetSeconds();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._font = comboBox2.SelectedItem.ToString();
        }
    }
}
