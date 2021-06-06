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
using System.Drawing;

namespace AHD.SM.ASMP
{
    class VLCMediaPlayer : IMediaPlayer
    {
        private bool initialized;
        IntPtr instance, player;

        public double Duration
        {
            get
            {
                if (!initialized)
                    return 0;
                return (double)LibVlc.libvlc_media_player_get_length(player) / 1000;
            }
        }
        public double Position
        {
            get
            {
                if (!initialized)
                    return 0;

                double ll = LibVlc.libvlc_media_player_get_time(player);
                ll /= 1000;

                return ll;
            }
            set
            {
                if (!initialized)
                    return;
                if (value < 0)
                    return;
                LibVlc.libvlc_media_player_set_time(player, (long)(value * 1000));
                PositionChanged?.Invoke(null, new EventArgs());
            }
        }
        public bool IsPlaying
        {
            get
            {
                if (!initialized)
                    return false;
                return LibVlc.libvlc_media_player_is_playing(player) == 1;
            }
        }
        public int Volume
        {
            get
            {
                if (!initialized)
                    return 0;
                return LibVlc.libvlc_audio_get_volume(player);
            }
            set
            {
                if (!initialized)
                    return;
                if (value < 0)
                    return;
                LibVlc.libvlc_audio_set_volume(player, value);
            }
        }
        public bool Initialized { get { return initialized; } }
        public event EventHandler PositionChanged;

        public void ClearMedia()
        {
            if (initialized)
            {
                DisposeMedia();
            }
        }
        public void DisposeMedia()
        {
            if (!initialized)
                return;
            LibVlc.libvlc_media_player_stop(player);
            System.Threading.Thread.Sleep(500);

            LibVlc.libvlc_media_player_release(player);
            LibVlc.libvlc_release(instance);
           
            GC.Collect();
            initialized = false;
        }
        public Size GetVideoSize()
        {
            if (!initialized)
                return Size.Empty;
            uint x = 0;
            uint y = 0;

            LibVlc.libvlc_video_get_size(player, 0, out x, out y);

            return new Size((int)x, (int)y);
        }
        public void LoadFile(string filePath, IntPtr window_handle)
        {
            if (initialized)
            {
                DisposeMedia();
                initialized = false;
            }

            if (filePath == "")
                return;
            string[] args = new string[] {
       // "-I", "dummy", "--ignore-config",
      //  @"--plugin-path=C:\Program Files (x86)\VideoLAN\VLC\plugins",
      //  "--vout-filter=deinterlace", "--deinterlace-mode=blend"
      };

            instance = LibVlc.libvlc_new(args.Length, args);

            IntPtr media = LibVlc.libvlc_media_new_path(instance, filePath);

            player = LibVlc.libvlc_media_player_new_from_media(media);

            LibVlc.libvlc_media_release(media);

            LibVlc.libvlc_media_player_set_hwnd(player, window_handle);

            LibVlc.libvlc_media_player_play(player);

            System.Threading.Thread.Sleep(100);

            LibVlc.libvlc_media_player_set_time(player, 0);
            LibVlc.libvlc_media_player_pause(player);

            initialized = true;
        }
        public void Mute()
        {
        }
        public void Pause()
        {
            if (!initialized)
                return;
            LibVlc.libvlc_media_player_pause(player);
        }
        public void Play()
        {
            if (!initialized)
                return;
            LibVlc.libvlc_media_player_play(player);
        }
        public void SetVideoSurface(IntPtr handle, int width, int height)
        {
            if (!initialized)
                return;
            LibVlc.libvlc_media_player_set_hwnd(player, handle);
        }
        public void Stop()
        {
            if (!initialized)
                return;
            Position = 0;
            Pause();
        }
    }
}
