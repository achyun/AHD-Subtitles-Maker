// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
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
using System.Reflection;
using System.Resources;

namespace AHD.SM.Forms
{
    public partial class Frm_LineBreakRule : Form
    {
        public Frm_LineBreakRule()
        {
            InitializeComponent();
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
   Assembly.GetExecutingAssembly());
        public bool RuleCapialLatter { get { return checkBox_capital.Checked; } }
        public bool RuleCharLimit { get { return checkBox_char_limit.Checked; } }
        public bool RulePuncuation { get { return checkBox_punc.Checked; } }
        public bool RulePuncuationAfter { get { return radioButton_after_punc.Checked; } }
        public string RulePuncationText { get { return textBox1.Text; } }
        public int RuleCharLimitValue { get { return (int)numericUpDown1.Value; } }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // OK
            if (!checkBox_punc.Checked && !checkBox_capital.Checked && !checkBox_char_limit.Checked)
            {
                MessageBox.Show(resources.GetString("Message_0"));
                return;
            }
            if (!checkBox_punc.Checked && textBox1.Text.Length == 0)
            {
                MessageBox.Show(resources.GetString("Message_1"));
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
