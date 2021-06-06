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
    public partial class cl_Subviewer1 : UserControl
    {
        Subviewer1 format;
        public cl_Subviewer1(Subviewer1 format)
        {
            InitializeComponent();
            this.format = format;
            textBox1.Text = format._Title;
            textBox2.Text = format._Author;
            textBox3.Text = format._Source;
            textBox4.Text = format._Program;
            textBox5.Text = format._File_path;
            numericUpDown1.Value = format._Delay;
            numericUpDown2.Value = format._CDtrack;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            format._Title = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            format._Author = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            format._Source = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            format._Program = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            format._File_path = textBox5.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            format._Delay = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            format._CDtrack = (int)numericUpDown2.Value;
        }
    }
}
