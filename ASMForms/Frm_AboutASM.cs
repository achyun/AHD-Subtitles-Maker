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
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
namespace AHD.SM.Forms
{
    public partial class Frm_AboutASM : Form
    {
        public Frm_AboutASM(string version, string lang)
        {
            InitializeComponent();
            this.lang = lang;
            label_version.Text = resources.GetString("Version") + ": " + version;
            //richTextBox2.Rtf = Properties.Resources.CREDIT;
            //if (File.Exists(".\\Copyright Notices - Credits.txt"))
            //    richTextBox2.Lines = File.ReadAllLines(".\\Copyright Notices - Credits.txt");
        }

        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
         Assembly.GetExecutingAssembly());
        private string lang;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.gnu.org/licenses/");
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(".\\GNU GENERAL PUBLIC LICENSE 3.0.html");
        }
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(".\\Copyright Notices - Credits.txt");
        }
        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:alaahadidfreeware@gmail.com");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opensubtitles.org");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker");
        }
    }
}
