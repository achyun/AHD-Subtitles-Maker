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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using AxWMPLib;

namespace AHD.SM.ASMP
{
    class WindowsMediaPlayerPlayer : IMediaPlayer
    {
        public double Duration
        {
            get
            {
                if (!mediaPlayerInitialized)
                    return 0;
                if (current_item == null)
                    return 0;
                return current_item.duration;
            }
        }

        public double Position
        {
            get
            {
                if (!mediaPlayerInitialized)
                    return 0;
                return mediaPlayer.Ctlcontrols.currentPosition;
            }
            set
            {
                if (!mediaPlayerInitialized)
                    return;
                mediaPlayer.Ctlcontrols.currentPosition = value;
                PositionChanged?.Invoke(null, new EventArgs());
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (!mediaPlayerInitialized)
                    return false;
                return mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying;
            }
        }

        public int Volume
        {
            get
            {
                if (!mediaPlayerInitialized)
                    return 0;
                return mediaPlayer.settings.volume;
            }
            set
            {
                if (!mediaPlayerInitialized)
                    return;
                mediaPlayer.settings.volume = value;
            }
        }

        public bool Initialized { get { return mediaPlayerInitialized; } }

        public event EventHandler PositionChanged;
        public WMPLib.IWMPMedia current_item;
        private AxWindowsMediaPlayer mediaPlayer;
        private Control surface_control;
        private bool mediaPlayerInitialized;
        private bool isDisposingMediaPlayer = false;
        private bool isInitializingMediaPlayer = false;

        public void ClearMedia()
        {
            if (!isInitializingMediaPlayer)
                return;
            ClearMediaList();
        }
        private void InitializeMediaPlayer(IntPtr handle)
        {
            if (mediaPlayer == null && !isInitializingMediaPlayer && !isDisposingMediaPlayer && !mediaPlayerInitialized)
            {
                isInitializingMediaPlayer = true;
                mediaPlayer = new AxWindowsMediaPlayer();
                ((System.ComponentModel.ISupportInitialize)(mediaPlayer)).BeginInit();

                // Player
                mediaPlayer.Parent = surface_control = Control.FromHandle(handle);
                mediaPlayer.Enabled = true;
                mediaPlayer.Dock = DockStyle.Fill;
                mediaPlayer.BringToFront();
                mediaPlayer.Name = "Player";
                //mediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange);
                //mediaPlayer.MouseDownEvent += mediaPlayer_MouseDownEvent;
                ((System.ComponentModel.ISupportInitialize)(mediaPlayer)).EndInit();

                //mediaPlayer.enableContextMenu = false;
                //mediaPlayer.ContextMenuStrip = contextMenuStrip1;
                mediaPlayer.uiMode = "none";


                isInitializingMediaPlayer = false;
                mediaPlayerInitialized = true;
            }
        }
        public void DisposeMedia()
        {
            if (mediaPlayer != null && !isInitializingMediaPlayer && !isDisposingMediaPlayer && mediaPlayerInitialized)
            {
                isDisposingMediaPlayer = true;
                try
                {
                    mediaPlayer.Ctlcontrols.stop();
                    mediaPlayer.close();

                    //_WMPOCXEvents_PlayStateChangeEventHandler(this.axWindowsMediaPlayer1_PlayStateChange);
                    //mediaPlayer.MouseDownEvent -= mediaPlayer_MouseDownEvent;
                    if (mediaPlayer.currentMedia != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.currentMedia);
                    if (mediaPlayer.mediaCollection != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.mediaCollection);
                    if (mediaPlayer.playlistCollection != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.playlistCollection);
                    if (mediaPlayer.Ctlcontrols != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.Ctlcontrols);
                    if (mediaPlayer.currentPlaylist != null)
                        Marshal.FinalReleaseComObject(mediaPlayer.currentPlaylist);
                    surface_control.Controls.Remove(mediaPlayer);
                    mediaPlayer.Dispose();
                    while (!mediaPlayer.IsDisposed)
                    {
                        // Wait until it get disposed
                    }
                }
                catch { }
                mediaPlayer = null;
                isDisposingMediaPlayer = false;
                mediaPlayerInitialized = false;
            }
            GC.Collect();
        }
        private void ClearMediaList()
        {
            // Only stop media
            if (mediaPlayer != null)
            {
                mediaPlayer.Ctlcontrols.stop();
                mediaPlayer.currentPlaylist.clear();
                mediaPlayer.URL = null;
            }
        }
        public Size GetVideoSize()
        {
            if (!mediaPlayerInitialized)
                return Size.Empty;
            return new Size(mediaPlayer.currentMedia.imageSourceWidth, mediaPlayer.currentMedia.imageSourceHeight);
        }

        public void LoadFile(string filePath, IntPtr window_handle)
        {
            if (!mediaPlayerInitialized)
                InitializeMediaPlayer(window_handle);
            // Check again to see if it is initialized
            if (!mediaPlayerInitialized)
                return;
            ClearMediaList();

            current_item = mediaPlayer.newMedia(filePath);
            mediaPlayer.currentPlaylist.appendItem(current_item);

            mediaPlayer.Ctlcontrols.playItem(current_item);
            System.Threading.Thread.Sleep(100);

            mediaPlayer.Ctlcontrols.stop();
        }

        public void Mute()
        {
        }

        public void Pause()
        {
            if (!mediaPlayerInitialized)
                return;
            mediaPlayer.Ctlcontrols.pause();
        }

        public void Play()
        {
            if (!mediaPlayerInitialized)
                return;
            mediaPlayer.Ctlcontrols.play();
        }

        public void SetVideoSurface(IntPtr handle, int width, int height)
        {
            if (!mediaPlayerInitialized)
                InitializeMediaPlayer(handle);
            if (!mediaPlayerInitialized)
                return;
            mediaPlayer.Enabled = true;
            mediaPlayer.Name = "axWindowsMediaPlayer1";
            mediaPlayer.Parent = surface_control = Control.FromHandle(handle);
            mediaPlayer.Dock = DockStyle.Fill;
            mediaPlayer.uiMode = "none";// Hide controls
        }

        public void Stop()
        {
            if (!mediaPlayerInitialized)
                return;
            mediaPlayer.Ctlcontrols.stop();
        }
    }
}
