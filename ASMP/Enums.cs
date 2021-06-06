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
    /// The alignement of a line
    /// </summary>
    public enum LineAlignement
    {
        /// <summary>
        /// Left
        /// </summary>
        Left,
        /// <summary>
        /// Center
        /// </summary>
        Center,
        /// <summary>
        /// Right
        /// </summary>
        Right
    }
    /// <summary>
    /// The position of subtitle
    /// </summary>
    public enum SubtitlePosition
    {
        /// <summary>
        /// Top_Left
        /// </summary>
        Top_Left,
        /// <summary>
        /// Top_Middle
        /// </summary>
        Top_Middle,
        /// <summary>
        /// Top_right
        /// </summary>
        Top_right,
        /// <summary>
        /// Mid_Left
        /// </summary>
        Mid_Left,
        /// <summary>
        /// Mid_Middle
        /// </summary>
        Mid_Middle,
        /// <summary>
        /// Mid_Right
        /// </summary>
        Mid_Right,
        /// <summary>
        /// Down_Left
        /// </summary>
        Down_Left,
        /// <summary>
        /// Down_Middle
        /// </summary>
        Down_Middle,
        /// <summary>
        /// Down_Right
        /// </summary>
        Down_Right
    }
    public enum SubtitleTimingMode
    {
        SecondMilli,
        Timespan_Milli,
        Timespan_NTSC,
        Timespan_PAL,
        Frames_NTSC, 
        Frames_Pal
    }
}