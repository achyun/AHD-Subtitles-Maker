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
    public partial class cl_DVDSubtitle : UserControl
    {
        DVDSubtitle format;
        public cl_DVDSubtitle(DVDSubtitle format)
        {
            InitializeComponent();
            this.format = format;
            textBox1.Text = format.DiscID;
            textBox2.Text = format.DVDTitle;
            textBox3.Text = format.Language;
            textBox4.Text = format.Author;
            textBox5.Text = format.Web;
            textBox6.Text = format.Info;
            textBox7.Text = format.License;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            format.DiscID = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            format.DVDTitle = textBox2.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            format.Language = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            format.Author = textBox4.Text;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            format.Web = textBox5.Text;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            format.Info = textBox6.Text;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            format.License = textBox7.Text;
        }
    }
}
