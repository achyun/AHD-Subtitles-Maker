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
using AHD.Forms;
using AHD.SM.Forms;

namespace ASD
{
    public partial class FormLogIn : Form
    {
        public FormLogIn()
        {
            InitializeComponent();
        }
        public string UserName
        {
            get { return textBox_username.Text; }
        }
        public string Password
        {
            get { return textBox_password.Text; }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = !checkBox1.Checked;
        }
        // OK
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_username.Text == "")
            {
                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseEnterUsername"),
                    Program.ResourceManager.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            if (textBox_password.Text == "")
            {
                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseEnterPassword"),
                    Program.ResourceManager.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(textBox_username.Text, textBox_password.Text))
            {
                return;
            }
            DialogResult = DialogResult.OK;
        }
        // Cancel
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.opensubtitles.org/en/newuser");
        }
    }
}
