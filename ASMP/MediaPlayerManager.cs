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
    public class MediaPlayerManager
    {
        private static IMediaPlayer current_player;
        private static bool media_player_loaded;

        /// <summary>
        /// Load a media player and make it ready to use.
        /// </summary>
        /// <param name="player_id"></param>
        public static void LoadMediaPlayer(string player_id)
        {
            if (media_player_loaded)
            {
                // Dispose current
                current_player.ClearMedia();
                current_player.DisposeMedia();
                media_player_loaded = false;
            }
            switch (player_id)
            {
                case "player.directshow":
                    {
                        current_player = new DirectshowMediaPlayer();
                        break;
                    }
                case "player.vlc":
                    {
                        current_player = new VLCMediaPlayer();
                        break;
                    }
                case "player.wmp":
                    {
                        current_player = new WindowsMediaPlayerPlayer();
                        break;
                    }
            }
            media_player_loaded = current_player != null;
        }
        private static void DisposeMedia()
        {
            if (!media_player_loaded)
                return;
            current_player.DisposeMedia();
        }
        /// <summary>
        /// Load media file
        /// </summary>
        /// <param name="filePath">The complete media file path</param>
        public static void LoadFile(string filePath, IntPtr window_handle)
        {
            if (!media_player_loaded)
                return;
            if (WaveReader.IsGenerating)
                WaveReader.AbortGenerateProcess();
            current_player.LoadFile(filePath, window_handle);
        }
        /// <summary>
        /// Set the media surface (where to render video)
        /// </summary>
        /// <param name="handle">The video control handle</param>
        public static void SetVideoSurface(IntPtr handle, int width, int height)
        {
            if (!media_player_loaded)
                return;
            current_player.SetVideoSurface(handle, width, height);
        }
        /// <summary>
        /// Clear media file.
        /// </summary>
        public static void ClearMedia()
        {
            if (!media_player_loaded)
                return;
            current_player.ClearMedia();
        }
        /// <summary>
        /// Play media
        /// </summary>
        public static void Play()
        {
            if (!media_player_loaded)
                return;
            current_player.Play();
        }
        /// <summary>
        /// Pause media
        /// </summary>
        public static void Pause()
        {
            if (!media_player_loaded)
                return;
            current_player.Pause();
        }
        /// <summary>
        /// Stop media.
        /// </summary>
        public static void Stop()
        {
            if (!media_player_loaded)
                return;
            current_player.Stop();
        }
        /// <summary>
        /// Mute/UnMute
        /// </summary>
        public static void Mute()
        {
            if (!media_player_loaded)
                return;
            current_player.Mute();
        }
        // Properties
        /// <summary>
        /// Get the media duration.
        /// </summary>
        public static double Duration
        {
            get
            {
                if (!media_player_loaded)
                    return 0;
                return current_player.Duration;
            }
        }
        /// <summary>
        /// Get or set current media position
        /// </summary>
        public static double Position
        {
            get
            {
                if (!media_player_loaded)
                    return 0;
                return current_player.Position;
            }
            set
            {
                if (!media_player_loaded)
                    return;
                current_player.Position = value;
            }
        }
        /// <summary>
        /// Get current status
        /// </summary>
        public static bool IsPlaying
        {
            get
            {
                if (!media_player_loaded)
                    return false;
                return current_player.IsPlaying;
            }
        }
        public static int Volume
        {
            get
            {
                if (!media_player_loaded)
                    return 0;
                return current_player.Volume;
            }
            set
            {
                if (!media_player_loaded)
                    return;
                current_player.Volume = value;
            }
        }
        public static bool Initialized
        {
            get
            {
                if (!media_player_loaded)
                    return false;
                return current_player.Initialized;
            }
        }
        public static Size GetVideoSize()
        {
            if (!media_player_loaded)
                return Size.Empty;
            return current_player.GetVideoSize();
        }
        // Events
        /// <summary>
        /// Raised when the user changes the media play position
        /// </summary>
        public static event EventHandler PositionChanged;
    }
}
