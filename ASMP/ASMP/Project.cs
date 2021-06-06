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
using System.Reflection;
using System.Resources;
namespace AHD.SM.ASMP
{
    /// <summary>
    /// AHD Subtitles Maker Project class.
    /// </summary>
    [Serializable()]
    public class Project
    {
        /* AHD Subtitles Maker Project "ASMP" version 5 */

        /// <summary>
        /// AHD Subtitles Maker Project class.
        /// </summary>
        /// <param name="name">The name of this project</param>
        public Project(string name)
        {
            this.name = name;
            this.description = "";
            this.log = "";
            this.mediaPath = "";
            this.subtitleTracks = new List<SubtitlesTrack>();
            this.timeMarks = new List<TimeMark>();
            RebuildStyles();
            this.usePreparedText = false;
            this.cutPreparedTextAfterAdd = true;
        }

        const string version = "5.0.0.0";
        string name;
        string description;
        bool usePreparedText;
        bool cutPreparedTextAfterAdd;
        bool wordWrapPreparedText;
        string preparedTextRTF;
        string log;
        string mediaPath;
        List<SubtitlesTrack> subtitleTracks;
        List<TimeMark> timeMarks;
        List<ASMPFontStyle> styles;

        //Properties
        /// <summary>
        /// Get or set the name of this project
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the description of this project
        /// </summary>
        public string Description
        { get { return description; } set { description = value; } }
        /// <summary>
        /// Get or set the prepared text as RTF code.
        /// </summary>
        public string PreparedTextRTF
        {
            get { return preparedTextRTF; }
            set { preparedTextRTF = value; }
        }
        /// <summary>
        /// Get or set if the prepared text can be used in adding/editing subtitle.
        /// </summary>
        public bool UsePreparedText
        {
            get { return usePreparedText; }
            set { usePreparedText = value; }
        }
        /// <summary>
        /// Get or set if the prepared text will be cut after each use.
        /// </summary>
        public bool CutPreparedTextAfterAdd
        {
            get { return cutPreparedTextAfterAdd; }
            set { cutPreparedTextAfterAdd = value; }
        }
        public bool WordWrapPreparedText
        {
            get { return wordWrapPreparedText; }
            set { wordWrapPreparedText = value; }
        }
        /// <summary>
        /// Get or set the Log of this project
        /// </summary>
        public string Log
        { get { return log; } set { log = value; } }
        /// <summary>
        /// Get current ASMP version.
        /// </summary>
        public string Version
        { get { return version; } }
        /// <summary>
        /// Get or set the media path associated with this project
        /// </summary>
        public string MediaPath
        { get { return mediaPath; } set { mediaPath = value; } }
        /// <summary>
        /// Get or set the Subtitle Tracks collection
        /// </summary>
        public List<SubtitlesTrack> SubtitleTracks
        { get { return subtitleTracks; } set { subtitleTracks = value; } }
        /// <summary>
        /// Get or set the time marks
        /// </summary>
        public List<TimeMark> TimeMarks
        { get { return timeMarks; } set { timeMarks = value; } }
        /// <summary>
        /// Get or set the font style collection.
        /// </summary>
        public List<ASMPFontStyle> Styles
        { get { return styles; } set { styles = value; } }

        /// <summary>
        /// Search Subtitle Tracks to find if givin track exists
        /// </summary>
        /// <param name="Name">Subtitles Track name to search for, not case sensitive</param>
        /// <returns>True if exists, False if not</returns>
        public bool IsSubtitlesTrackExist(string Name)
        {
            foreach (SubtitlesTrack track in subtitleTracks)
                if (track.Name.ToLower() == Name.ToLower())
                    return true;
            return false;
        }
        public void RebuildStyles()
        {
            this.styles = new List<ASMPFontStyle>();
            ResourceManager resources = new ResourceManager("AHD.SM.ASMP.LanguageResources.Resource", Assembly.GetExecutingAssembly());
            // Default
            styles.Add(new ASMPFontStyle(resources.GetString("FontStyle_Default"), new System.Drawing.Font("Tahoma", 8), System.Drawing.Color.White));
            // Title
            styles.Add(new ASMPFontStyle(resources.GetString("FontStyle_Title"), new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold), System.Drawing.Color.White));
            // Link
            styles.Add(new ASMPFontStyle(resources.GetString("FontStyle_Link"), new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular), System.Drawing.Color.Blue));
        }
    }
}
