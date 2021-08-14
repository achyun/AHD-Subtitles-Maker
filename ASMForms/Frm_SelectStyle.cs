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
using System.Reflection;
using System.Resources;
namespace AHD.SM.Forms
{
    public partial class Frm_SelectStyle : Form
    {
        public Frm_SelectStyle(ASMPFontStyle[] styles)
        {
            InitializeComponent();
            foreach (ASMPFontStyle st in styles)
                comboBox1.Items.Add(st);
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
 Assembly.GetExecutingAssembly());
        public ASMPFontStyle SelectedStyle
        {
            get
            {
                if (comboBox1.SelectedIndex >= 0)
                    return (ASMPFontStyle)comboBox1.SelectedItem;
                return null;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show(resources.GetString("Message_PleaseSelectAStyleFirst"));
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
