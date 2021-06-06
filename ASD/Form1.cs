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
using AHD.SM.ASMP;

namespace ASD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string username = "";
        string password = "";
        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opensubtitles.org");
        }
        // About
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_About"), "About AHD Subtitles Downloader " + Application.ProductVersion);
        }
        // Help
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string helpFile = ".\\" + Program.CultureInfo.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpFile))
                Help.ShowHelp(this, helpFile, "AHD Subtitles Downloader");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", "AHD Subtitles Downloader");
        }
        // Language
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLanguage frm = new FrmLanguage();
            frm.ShowDialog();
        }
        // Download
        private void button1_Click(object sender, EventArgs e)
        {
            if (username == "" || password == "")
            {
                FormLogIn ff = new FormLogIn();
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    username = ff.UserName;
                    password = ff.Password;
                }
                else
                {
                    return;
                }
            }
            else if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(username, password))
            {
                FormLogIn ff = new FormLogIn();
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    username = ff.UserName;
                    password = ff.Password;
                }
                else
                {
                    return;
                }
            }
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ChangeMedia");
            op.Filter = Filters.Media;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (Frm_DownloadSubtitlesFromOS.CheckLogIn(username, password))
                {
                    Frm_DownloadSubtitlesFromOS frm = new Frm_DownloadSubtitlesFromOS(op.FileName, username, password);

                    frm.ShowDialog(this);
                }
            }
            else
            {
                if (Frm_DownloadSubtitlesFromOS.CheckLogIn(username, password))
                {
                    Frm_DownloadSubtitlesFromOS frm = new Frm_DownloadSubtitlesFromOS(op.FileName, username, password);
                    frm.ShowDialog(this);
                }
            }
        }
        // Upload
        private void button2_Click(object sender, EventArgs e)
        {
            if (username == "" || password == "")
            {
                FormLogIn ff = new FormLogIn();
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    username = ff.UserName;
                    password = ff.Password;
                }
                else
                {
                    return;
                }
            }
            else if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(username, password))
            {
                FormLogIn ff = new FormLogIn();
                if (ff.ShowDialog(this) == DialogResult.OK)
                {
                    username = ff.UserName;
                    password = ff.Password;
                }
                else
                {
                    return;
                }
            }
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ChangeMedia");
            op.Filter = Filters.Media;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string movieFileName = op.FileName;

                OpenFileDialog op1 = new OpenFileDialog();
                op1.Title = Program.ResourceManager.GetString("Title_ChangeSubtitleFile");
                op1.Filter = "All files|*.*";
                if (op1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    string subFileName = op1.FileName;
                    Frm_UploadToOS frm = new Frm_UploadToOS(movieFileName, subFileName, username, password);
                    frm.ShowDialog(this);
                }
            }
        }
    }
}
