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
using DirectShowLib;
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AHD.SM.ASMP
{
    class DirectshowMediaPlayer : IMediaPlayer
    {
        private FilterGraph mediaDS;
        private IMediaPosition mediaPos;
        private IVideoWindow mediaVid;
        private IMediaControl mediaCon;
        private IBasicAudio mediaS;
        private bool initialized;

        public double Duration
        {
            get
            {
                double v = 0;
                if (mediaPos != null)
                {
                    try { mediaPos.get_Duration(out v); }
                    catch { }
                }
                return v;
            }
        }
        public double Position
        {
            get
            {
                double v = 0;
                if (mediaPos != null)
                    mediaPos.get_CurrentPosition(out v);
                return v;
            }
            set
            {
                if (value < 0)
                    return;
                if (mediaPos != null)
                {
                    mediaPos.put_CurrentPosition(value);
                    if (PositionChanged != null)
                        PositionChanged(null, new EventArgs());
                }
            }
        }
        public bool IsPlaying
        {
            get
            {
                FilterState st = FilterState.Stopped;
                if (mediaCon != null)
                    mediaCon.GetState(2, out st);
                return st == FilterState.Running;
            }
        }
        public int Volume
        {
            get
            {
                int v = 0;
                if (mediaS != null)
                {
                    mediaS.get_Volume(out v);
                }
                return (v / 35) + 100;
            }
            set
            {
                if (mediaS != null)
                {
                    if (value == 0)
                        mediaS.put_Volume(-10000);
                    else
                        mediaS.put_Volume((value - 100) * 35);
                }
            }
        }
        public bool Initialized
        {
            get { return initialized; }
        }

        public void ClearMedia()
        {
            // TODO: clear media player
            if (initialized)
            {
                DisposeMedia();
            }
        }
        public void DisposeMedia()
        {
            if (!initialized) return;
            // Dispose objects
            Marshal.ReleaseComObject(mediaDS); mediaDS = null;
            Marshal.ReleaseComObject(mediaCon); mediaCon = null;
            Marshal.ReleaseComObject(mediaPos); mediaPos = null;
            Marshal.ReleaseComObject(mediaVid); mediaVid = null;
            Marshal.ReleaseComObject(mediaS); mediaS = null;

            GC.Collect();
            initialized = false;
        }
        public Size GetVideoSize()
        {
            if (mediaVid != null)
            {
                try
                {
                    int w = 0;
                    int h = 0;
                    mediaVid.get_Width(out w);
                    mediaVid.get_Height(out h);

                    return new Size(w, h);
                }
                catch { }
            }
            return Size.Empty;
        }
        public void LoadFile(string filePath, IntPtr window_handle)
        {
            if (initialized)
            {
                DisposeMedia();
            }
            if (!initialized)
            {
                // Initialize
                mediaDS = new FilterGraph();
                mediaCon = (IMediaControl)mediaDS;
                mediaPos = (IMediaPosition)mediaDS;
                mediaVid = (IVideoWindow)mediaDS;
                mediaS = (IBasicAudio)mediaDS;
            }

            mediaCon.RenderFile(filePath);

            initialized = true;
        }
        public void Mute()
        {
        }
        public void Pause()
        {
            if (mediaCon != null)
                mediaCon.Pause();
        }
        public void Play()
        {
            if (mediaCon != null)
                mediaCon.Run();
        }
        public void SetVideoSurface(IntPtr handle, int width, int height)
        {
            if (mediaVid != null)
            {
                //try
                {
                    IntPtr owner = IntPtr.Zero;
                    mediaVid.get_Owner(out owner);
                    if (owner != handle)
                    {
                        mediaVid.put_Owner(handle);
                    }
                    mediaVid.SetWindowPosition(0, 0, width, height);
                    mediaVid.put_WindowStyle(WindowStyle.Child);
                }
                //catch(Exception ex)
                {

                }
            }
        }
        public void Stop()
        {
            if (mediaCon != null)
            {
                mediaCon.Stop();
                Position = 0;
            }
        }

        public event EventHandler PositionChanged;
    }
}
