// This file is part of AHD Subtitles Maker.
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AHD.SM.ASMP;

namespace AHD.SM.Forms
{
    public partial class Frm_SetAlignment : Form
    {
        public Frm_SetAlignment()
        {
            InitializeComponent();
        }
        public LineAlignement LineAlignement
        {
            get
            {
                LineAlignement data = ASMP.LineAlignement.Center;
                if (radioButton1.Checked)
                    data = ASMP.LineAlignement.Left;
                if (radioButton2.Checked)
                    data = ASMP.LineAlignement.Center;
                if (radioButton3.Checked)
                    data = ASMP.LineAlignement.Right;
                return data;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
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
