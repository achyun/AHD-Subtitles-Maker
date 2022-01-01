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

namespace AHD.SubtitlesMakerProfessional.Base
{
    /// <summary>
    /// The subtitles track
    /// </summary>
    [Serializable()]
    public class SubtitlesTrack
    {
        string _Name = "";
        List<Subtitle> _subtitles = new List<Subtitle>();
        /// <summary>
        /// Get or set the name of the subtitles track
        /// </summary>
        public string Name
        { get { return _Name; } set { _Name = value; } }
        /// <summary>
        /// Get or set the subtitles collection
        /// </summary>
        public List<Subtitle> Subtitles
        { get { return _subtitles; } set { _subtitles = value; } }
    }
}
