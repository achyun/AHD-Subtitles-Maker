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
    public partial class cl_AutodeskSmoke : UserControl
    {
      
        public cl_AutodeskSmoke(AutodeskSmoke format)
        {
            InitializeComponent();
            this.format = format;
            textBox1_Name.Text = format._Name;
            comboBox1_Rate.SelectedItem = format._FrameRate;
            comboBox1_Depth.SelectedItem = format._Depth;
            comboBox1_Aspect.SelectedItem = format._aspect;
            comboBox1_ScanFormat.SelectedItem = format._scanformat;
            textBox1_setup.Text = format._setup;
            checkBox1.Checked = format._DropFrame;
        }
        AutodeskSmoke format;
        double[] frrates = { 23.976, 24, 25, 29.97, 
                               29.97, 30, 59.94, 59.94 };

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "All Files (*.*)|*.*";
            op.Multiselect = false;
            op.Title = "";
            op.FileName = textBox1_setup.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1_setup.Text = op.FileName;
            }
        }

        private void textBox1_Name_TextChanged(object sender, EventArgs e)
        {
            format._Name = textBox1_Name.Text;
        }
        private void comboBox1_Rate_SelectedIndexChanged(object sender, EventArgs e)
        {
          format._FrameRate = comboBox1_Rate.SelectedItem.ToString();
        }
        private void comboBox1_Depth_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._Depth = comboBox1_Depth.SelectedItem.ToString();
        }
        private void comboBox1_Aspect_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._aspect = comboBox1_Aspect.SelectedItem.ToString();
        }
        private void comboBox1_ScanFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            format._scanformat = comboBox1_ScanFormat.SelectedItem.ToString();
        }
        private void textBox1_setup_TextChanged(object sender, EventArgs e)
        {
            format._setup = textBox1_setup.Text;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            format._DropFrame = checkBox1.Checked;
        }
    }
}
