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

namespace ASD
{
    public partial class FrmLanguage : Form
    {
        public FrmLanguage()
        {
            InitializeComponent();

            //languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                comboBox1.Items.Add(Program.SupportedLanguages[i, 2]);
                if (Program.SupportedLanguages[i, 0] == Program.Settings.Language)
                    comboBox1.SelectedItem = Program.SupportedLanguages[i, 2];
            }
            checkBox1.Checked = Program.Settings.ShowLanguagesForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.Settings.ShowLanguagesForm = checkBox1.Checked;
            Program.Language = Program.Settings.Language = Program.SupportedLanguages[comboBox1.SelectedIndex, 0];
            Program.Settings.Save();

            Close();
        }
    }
}
