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

namespace AHD.SM
{
    public partial class Frm_FirstRun : Form
    {
        public Frm_FirstRun()
        {
            InitializeComponent();

            //languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                listBox1.Items.Add(Program.SupportedLanguages[i, 2]);
                if (Program.SupportedLanguages[i, 2] == "English (United States)")
                    listBox1.SelectedIndex = i;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listBox1.SelectedIndex >= 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.Language = Program.SupportedLanguages[listBox1.SelectedIndex, 0];
            Program.Settings.Language = Program.SupportedLanguages[listBox1.SelectedIndex, 0];
            Program.Settings.FirstRun = true;
            Close();
        }
    }
}
