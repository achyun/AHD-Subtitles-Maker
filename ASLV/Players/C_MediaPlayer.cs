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
using AHD.ID3.Frames;
using AHD.ID3.Types;
using AHD.SM.ASMP;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace AHD.ID3.Editor.GUI
{
    public partial class C_MediaPlayer : EditorControl
    {
        public C_MediaPlayer(bool autoStart)
        {
            InitializeComponent();
            imagePanel1.DrawDefaultImageWhenViewImageIsNull = false;
            // axWindowsMediaPlayer1.settings.autoStart = autoStart;
            this.AutoStart = autoStart;
            this.playerControl1.UpdatePlayStatus();
        }
        public SynchronisedLyricsFrame currentFrameInDisplay = null;
        public bool AutoStart
        {
            // get { return axWindowsMediaPlayer1.settings.autoStart; }
            // set { axWindowsMediaPlayer1.settings.autoStart = value; }
            get;
            set;
        }
        public override void ClearMedia()
        {
            // axWindowsMediaPlayer1.Ctlcontrols.stop();
            // axWindowsMediaPlayer1.currentPlaylist.clear();
            //  axWindowsMediaPlayer1.URL = null;
            MediaPlayerManager.ClearMedia();
        }
        public override void ClearFields()
        {
            currentFrameInDisplay = null;
            lyricsLanguageToolStripMenuItem.DropDownItems.Clear();
            ClearMedia();
            label1.Text = "";
            imagePanel1.ImageToView = null;
            imagePanel1.Invalidate();
        }
        public override string[] SelectedFiles
        {
            get
            {
                return base.SelectedFiles;
            }
            set
            {
                base.SelectedFiles = value; LoadFiles();
            }
        }
        private void LoadFiles()
        {
            ClearFields();
            if (files == null) return;
            if (files.Length == 1)
            {
                // load image
                ID3v2 v2 = new ID3v2();
                if (v2.Load(files[0]) == Result.Success)
                {
                    AttachedPictureFrame frame;
                    if (v2.TagVersion.Major == 2)
                        frame = (AttachedPictureFrame)v2.GetFrameLoaded("PIC");
                    else
                        frame = (AttachedPictureFrame)v2.GetFrameLoaded("APIC");

                    if (frame != null)
                    {
                        try
                        {
                            System.IO.Stream stream = new System.IO.MemoryStream(frame.PictureData);
                            imagePanel1.ImageToView = new Bitmap(stream);
                            imagePanel1.Invalidate();
                        }
                        catch
                        {
                            imagePanel1.ImageToView = null;
                            imagePanel1.Invalidate();
                        }
                    }
                    // load synchronized lyrics item
                    lyricsLanguageToolStripMenuItem.DropDownItems.Clear();
                    bool isFirst = true;
                    foreach (ID3TagFrame sframe in v2.Frames)
                    {
                        if (sframe is SynchronisedLyricsFrame)
                        {
                            ToolStripMenuItem item = new ToolStripMenuItem();
                            item.Text = ID3FrameConsts.GetLanguage(((SynchronisedLyricsFrame)sframe).LanguageID);
                            if (item.Text == "")
                                item.Text = "Unknown";
                            item.Tag = sframe;
                            if (isFirst)
                            {
                                isFirst = false;
                                item.Checked = true;
                                currentFrameInDisplay = (SynchronisedLyricsFrame)sframe;
                            }
                            lyricsLanguageToolStripMenuItem.DropDownItems.Add(item);
                        }
                    }
                }
                ReloadMedia();
            }
        }
        public override void ReloadMedia()
        {
            if (files != null)
            {
                if (files.Length > 0)
                {
                    if (File.Exists(files[0]))
                    {
                        //   WMPLib.IWMPMedia m1 = axWindowsMediaPlayer1.newMedia(files[0]);
                        //   axWindowsMediaPlayer1.currentPlaylist.appendItem(m1);
                        MediaPlayerManager.LoadFile(files[0], panel1.Handle);
                        if (this.AutoStart)
                            MediaPlayerManager.Play();
                        playerControl1.UpdatePlayStatus();
                    }
                }
            }
        }
        public void ChooseLyricsLanguage(int index)
        {
            foreach (ToolStripMenuItem item in lyricsLanguageToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
            }
            currentFrameInDisplay = (SynchronisedLyricsFrame)lyricsLanguageToolStripMenuItem.DropDownItems[index].Tag;
            ((ToolStripMenuItem)lyricsLanguageToolStripMenuItem.DropDownItems[index]).Checked = true;
        }
        public void LoadFrames(SynchronisedLyricsFrame[] frames)
        {
            lyricsLanguageToolStripMenuItem.DropDownItems.Clear();
            bool isFirst = true;
            foreach (SynchronisedLyricsFrame sframe in frames)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = ID3FrameConsts.GetLanguage(sframe.LanguageID);
                if (item.Text == "")
                    item.Text = "Unknown";
                item.Tag = sframe;
                if (isFirst)
                {
                    isFirst = false;
                    item.Checked = true;
                    currentFrameInDisplay = (SynchronisedLyricsFrame)sframe;
                }
                lyricsLanguageToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        public override void PlayMedia()
        {
            // axWindowsMediaPlayer1.Ctlcontrols.play();
            MediaPlayerManager.Play();
        }
        public override void StopMedia()
        {
            // axWindowsMediaPlayer1.Ctlcontrols.stop();
            MediaPlayerManager.Stop();
        }
        public override bool IsPlaying
        {
            get
            {
                // return axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying;
                return MediaPlayerManager.IsPlaying;
            }
        }
        // the timer
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (currentFrameInDisplay == null)
            {
                label1.Text = "";
                return;
            }
            string text = "";
            int index = -1;
            foreach (SynchronisedLyricsItem item in currentFrameInDisplay.Items)
            {
                // if (axWindowsMediaPlayer1.Ctlcontrols.currentPosition <
                //     ((double)item.Time / 1000))
                if (MediaPlayerManager.Position < ((double)item.Time / 1000))
                {
                    break;
                }
                index++;
            }
            if (index >= 0)
            {
                text = currentFrameInDisplay.Items[index].Text;
            }
            label1.Text = text;
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            label1.Visible = label1.Text.Length > 0;
        }
        private void lyricsLanguageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ToolStripMenuItem item in lyricsLanguageToolStripMenuItem.DropDownItems)
            {
                item.Checked = false;
                if (item == e.ClickedItem)
                {
                    item.Checked = true;
                    currentFrameInDisplay = (SynchronisedLyricsFrame)item.Tag;
                }
            }
        }
        private void C_MediaPlayer_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible) StopMedia();
        }
        private void C_MediaPlayer_Paint(object sender, PaintEventArgs e)
        {
            imagePanel1.Invalidate();
        }
        private void C_MediaPlayer_Resize(object sender, EventArgs e)
        {
            imagePanel1.Invalidate();
        }
    }
}
