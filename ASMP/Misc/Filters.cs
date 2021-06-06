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
using System.Linq;
using System.Text;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Filters class, includes general file type filters
    /// </summary>
    public class Filters
    {
        public const string ASMP = "AHD Subtitles Maker Project (*asmp)|*.asmp;*ASMP";
        public const string Media = "Common Media Formats|*.avi;*.AVI;*.mp3;*.MP3;*.mpeg;*.MPEG;*.mpg;*.MPG;*.mpa;*.MPA;*.mpe;*.MPE;*.mpeg;*.MPEG;*.mpe;*.MPE;*.mpg;*.MPG;*.m2v;*.M2V;*.mpa;*.MPA;*.mp2;*.MP2;*.m2a;*.M2A;*.mpv;*.MPV;*.m2p;*.M2P;*.m2t;*.M2T;*.m2ts;*.M2TS;*.m1v;*.M1V;*.mp4;*.MP4;*.m4v;*.M4V;*.m4a;*.M4A;*.aac;*.AAC;*.3gp;*.3GP;*.avc;*.AVC;*.264;*.aif;*.AIF;*.aiff;*.AIFF;*.pic;*.PIC;*.pct;*.PCT;*.pict;*.PICT;*.mov;*.MOV;*.wmv;*.WMV;*.wma;*.WMA;*.asf;*.ASF;*.wav;*.WAV;*.aaf;*.AAF;*.flv;*.FLV;*.mkv;*.mks;*.MKV;*.MKS;|"
                + "AVI Movie(*.avi)|*.avi;*.AVI;|"
                + "FLV Movie(*.flv)|*.flv;*.FLV;|"
                + "Matroska (*.mkv;*.mks)|*.mkv;*.mks;*.MKV;*.MKS;|"
                + "Mp3 Audio(*.mp3;*.mpeg;*.mpg;*.mpa;*.mpe)|*.mp3;*.MP3;*.mpeg;*.MPEG;*.mpg;*.MPG;*.mpa;*.MPA;*.mpe;*.MPE;|"
                + "MPEG Movie(*.mpeg;*.mpe;*.mpg;*.m2v;*.mpa;*.mp2;*.m2a;*.mpv;*.m2p;*.m2t;*.m2ts;*.m1v;*.mp4;*.m4v;*.m4a;*.aac;**.3gp;*.avc;*.264)|*.mpeg;*.MPEG;*.mpe;*.MPE;*.mpg;*.MPG;*.m2v;*.M2V;*.mpa;*.MPA;*.mp2;*.MP2;*.m2a;*.M2A;*.mpv;*.MPV;*.m2p;*.M2P;*.m2t;*.M2T;*.m2ts;*.M2TS;*.m1v;*.M1V;*.mp4;*.MP4;*.m4v;*.M4V;*.m4a;*.M4A;*.aac;*.AAC;*.3gp;*.3GP;*.avc;*.AVC;*.264;|"
                + "Macintoch Audio AIFF (*.aif;*.aiff)|*.aif;*.AIF;*.aiff;*.AIFF;|"
                + "Macintoch Audio PICT (*.pic;*.pct;*.pict)|*.pic;*.PIC;*.pct;*.PCT;*.pict;*.PICT;|"
                + "Quick Time Movie(*.mov)|*.mov;*.MOV;|"
                + "Windows Media(*.wmv;*.wma;*.asf)|*.wmv;*.WMV;*.wma;*.WMA;*.asf;*.ASF;|"
                + "Windows WAVE Audio File(*.wav)|*.wav;*.WAV;|"
                + "AAF (*.aaf)|*.aaf;*.AAF|"
                + "All Files|*";
        public const string MKV = "Matroska (*.mkv;*.mks)|*.mkv;*.mks;*.MKV;*.MKS;";
    }
}
