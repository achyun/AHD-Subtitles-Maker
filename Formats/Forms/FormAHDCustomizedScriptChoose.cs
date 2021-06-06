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
using System.IO;
using System.Windows.Forms;

namespace AHD.SM.Formats
{
    public partial class FormAHDCustomizedScriptChoose : Form
    {
        public FormAHDCustomizedScriptChoose()
        {
            InitializeComponent();
            filePaths = new List<string>(Directory.GetFiles(".\\scripts"));

            RefreshFiles();

            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            if (File.Exists(".\\scripts\\README.txt"))
            {
                richTextBox2.Lines = File.ReadAllLines(".\\scripts\\README.txt");
            }
        }

        List<string> filePaths = new List<string>();

        public string[] EnteredScript
        {
            get { return richTextBox1.Lines; }
        }
        private void RefreshFiles()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < filePaths.Count; i++)
            {
                string[] lines = File.ReadAllLines(filePaths[i]);
                if (lines[0] == "; AHD Customized")
                    listBox1.Items.Add(Path.GetFileNameWithoutExtension(filePaths[i]));
                else
                {
                    filePaths.RemoveAt(i);
                    i--;
                }
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                richTextBox1.Lines = File.ReadAllLines(filePaths[listBox1.SelectedIndex]);
            }
        }
    }
}
