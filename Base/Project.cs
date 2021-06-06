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

namespace AHD.SubtitlesMakerProfessional.Base
{
    /// <summary>
    /// The project
    /// </summary>
    [Serializable()]
    public class Project
    {
        string _Name = "";
        string _MediaPath = "";
        string _Description = "";
        List<SubtitlesTrack> _subtitlesTracks = new List<SubtitlesTrack>();
        List<Mark> _Marks = new List<Mark>();
        string _Log = "";
        /// <summary>
        /// Get or set the name of this project
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set
            {
                string old = _Name;
                _Name = value;
                if (NameChanged != null)
                    NameChanged(this, new NameChangedArgs(old));
            }
        }
        /// <summary>
        /// Get or set the description of this project
        /// </summary>
        public string Description
        { get { return _Description; } set { _Description = value; } }
        /// <summary>
        /// Get or set the media file path
        /// </summary>
        public string MediaPath
        {
            get { return _MediaPath; }
            set
            {
                string old = _MediaPath;
                _MediaPath = value;
                if (MediaPathChanged != null)
                    MediaPathChanged(this, new NameChangedArgs(old));
            }
        }
        /// <summary>
        /// Get or set the subtitle tracks collection
        /// </summary>
        public List<SubtitlesTrack> SubtitleTracks
        { get { return _subtitlesTracks; } set { _subtitlesTracks = value; } }
        /// <summary>
        /// Get or set the marks collection
        /// </summary>
        public List<Mark> Marks
        { get { return _Marks; } set { _Marks = value; } }
        /// <summary>
        /// Get or set the log
        /// </summary>
        public string Log
        { get { return _Log; } set { _Log = value; } }
        /// <summary>
        /// Rised when the name changed of this project
        /// </summary>
        public event EventHandler<NameChangedArgs> NameChanged;
        /// <summary>
        /// Rised up when the user changes the path of the media
        /// </summary>
        public event EventHandler<NameChangedArgs> MediaPathChanged;
    }
}
