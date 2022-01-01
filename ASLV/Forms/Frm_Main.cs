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
using System.Windows.Forms;
using System.IO;
using AHD.ID3.Editor.GUI;
namespace AHD.ID3.Viewer
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
            LoadSettings();
            // add the media player
            this.Controls.Add(mediaPlayer);
            mediaPlayer.Dock = DockStyle.Fill;
            mediaPlayer.BringToFront();

            if (args != null)
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        try
                        {
                            if (File.Exists(args[i]))
                            {
                                OpenFile(args[i]);
                            }
                        }
                        catch { }
                    }
                }
            }
        }
        private void LoadSettings()
        {
            //languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Program.SupportedLanguages[i, 2];
                item.Checked = Program.SupportedLanguages[i, 0] == Program.Settings.Language;
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        private C_MediaPlayer mediaPlayer = new C_MediaPlayer(true);

        private void OpenFile(string fileName)
        {
            mediaPlayer.SelectedFiles = new string[] { fileName };
            mediaPlayer.PlayMedia();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_OpenMp3File");
            op.Filter = "mp3 (*.mp3)|*.mp3;*.MP3";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                OpenFile(op.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exportToSubtitlesFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mediaPlayer.currentFrameInDisplay != null)
            {
                mediaPlayer.StopMedia();
                Frm_ExportSubtitlesFormat frm = new Frm_ExportSubtitlesFormat(mediaPlayer.currentFrameInDisplay);
                frm.ShowDialog(this);
            }
        }

        private void optionsToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            // fill languages 
            lyricsLanguageToolStripMenuItem.DropDownItems.Clear();
            foreach (ToolStripMenuItem item in mediaPlayer.lyricsLanguageToolStripMenuItem.DropDownItems)
            {
                ToolStripMenuItem newitem = new ToolStripMenuItem();
                newitem.Text = item.Text;
                newitem.Checked = item.Checked;
                lyricsLanguageToolStripMenuItem.DropDownItems.Add(newitem);
            }
        }

        private void lyricsLanguageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = 0;
            foreach (ToolStripMenuItem item in lyricsLanguageToolStripMenuItem.DropDownItems)
            {
                if (item == e.ClickedItem)
                {
                    break;
                }
                index++;
            }
            mediaPlayer.ChooseLyricsLanguage(index);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_About frm = new Frm_About();
            frm.ShowDialog(this);
        }

        private void languageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int i = 0;
            int index = 0;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                    index = i;
                }
                else
                    item.Checked = false;
                i++;
            }
            Program.Language = Program.SupportedLanguages[index, 0];
            Program.Settings.Language = Program.SupportedLanguages[index, 0];

            MessageBox.Show(Program.ResourceManager.GetString("Message_YouMustRestartTheProgramToApplyLanguage"),
                Program.ResourceManager.GetString("MessageCaption_ApplyLanguage"));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.Settings.Save();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            mediaPlayer.PlayMedia();
        }
    }
}
