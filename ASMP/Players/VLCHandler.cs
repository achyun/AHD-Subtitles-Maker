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
using System.Runtime.InteropServices;

namespace AHD.SM.ASMP
{
    // http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html

    static class LibVlc
    {
        private const string nativeLibName = @"C:\Program Files (x86)\VideoLAN\VLC\libvlc.dll";
        // CREATE
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray,
        ArraySubType = UnmanagedType.LPStr)] string[] argv);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_release(IntPtr instance);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new(IntPtr p_instance);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_release(IntPtr media);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr media);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_path(IntPtr instance, [MarshalAs(UnmanagedType.LPStr)] string mediaFile);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_release(IntPtr player);

        // MEDIA PLAYER
        /// <summary>
        /// Set the drawable window
        /// </summary>
        /// <param name="player"></param>
        /// <param name="drawable"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_hwnd(IntPtr player, IntPtr drawable);
        /// <summary>
        /// Play
        /// </summary>
        /// <param name="player"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_play(IntPtr player);
        /// <summary>
        /// Pause
        /// </summary>
        /// <param name="player"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_pause(IntPtr player);
        /// <summary>
        /// Stop
        /// </summary>
        /// <param name="player"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_stop(IntPtr player);
        /// <summary>
        /// Get if playing or not
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_is_playing(IntPtr player);
        /// <summary>
        /// Get duration
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long libvlc_media_player_get_length(IntPtr player);
        /// <summary>
        /// Get time
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern long libvlc_media_player_get_time(IntPtr player);
        /// <summary>
        /// Set time
        /// </summary>
        /// <param name="player"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_time(IntPtr player, long time);
        /// <summary>
        /// Get movie position as percentage between 0.0 and 1.0
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern float libvlc_media_player_get_position(IntPtr player);
        /// <summary>
        /// Set movie position as percentage between 0.0 and 1.0. 
        /// </summary>
        /// <param name="player"></param>
        /// <param name="pos"></param>
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_position(IntPtr player, float pos);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_get_size(IntPtr player, uint num, out uint px, out uint py);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_set_volume(IntPtr player, int volume);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_audio_get_volume(IntPtr player);
    }

}
