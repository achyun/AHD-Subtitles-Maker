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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace AHD.SubtitlesMakerProfessional.Base
{
    /// <summary>
    /// The subtitle
    /// </summary>
    [Serializable()]
    public class Subtitle
    {
        string _Text = "";
        double _StartTime = 0;
        double _EndTime = 0;
        double _Duration = 0;
        Font _Font;
        Color _color;
        SubtitleLocation _Location;
        Point _CustomLocation = new Point();
        /// <summary>
        /// Get or set the start time
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("Start time")]
        public double StartTime
        { get { return _StartTime; } set { _StartTime = value; } }
        /// <summary>
        /// Get or set the end time
        /// </summary>
        [ReadOnly(true)]
        [DisplayName("End time")]
        public double EndTime
        { get { return _EndTime; } set { _EndTime = value; } }
        /// <summary>
        /// Get or set the duration time
        /// </summary>
        [ReadOnly(true)]
        public double Duration
        {
            get
            {
                _Duration = _EndTime - _StartTime;
                return _Duration;
            }
            set { _Duration = value; }
        }
        /// <summary>
        /// Get or set the text (lines) of this subtitle
        /// </summary>
        [Browsable(false)]
        public string[] TextLines
        {
            get
            {
                char[] Spliter = { '\n' };
                return _Text.Split(Spliter);
            }
        }
        /// <summary>
        /// Get or set the text of this subtitle
        /// </summary>
        public string Text
        {
            get
            { return _Text; }
            set { _Text = value; }
        }
        /// <summary>
        /// Get or set the font of this subtitle
        /// </summary>
        public Font Font
        { get { return _Font; } set { _Font = value; } }
        /// <summary>
        /// GEt or set the color of this subtitle's text
        /// </summary>
        public Color Color
        { get { return _color; } set { _color = value; } }
        /// <summary>
        /// Get or set the location of this subtitle
        /// </summary>
        public SubtitleLocation Location
        { get { return _Location; } set { _Location = value; } }
        /// <summary>
        /// Get or set the custom location
        /// </summary>
        [DisplayName("Custom Location")]
        public Point CustomLocation
        { get { return _CustomLocation; } set { _CustomLocation = value; } }
    }
    /// <summary>
    /// The location of subtitle
    /// </summary>
    [Serializable()]
    public enum SubtitleLocation
    {
        Bottom, Bottom_Right, Bottom_Left, Top, Top_Right, Top_Left, Middle, Middle_Right, Middle_Left, Custom
    }
}
