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
using AHD.SM.ASMP;
using System.Windows.Forms;

namespace AHD.SM.Formats
{
    public abstract class SubtitlesFormat
    {
        bool enabled = true;
        string path = "";
        double frameRate = 25;
        List<SubtitlesTrack> tracks = new List<SubtitlesTrack>();
        SubtitlesTrack selectedTrack = new SubtitlesTrack("SubtitlesTrack");
        //Properties
        /// <summary>
        /// Get the name of this format
        /// </summary>
        public abstract string Name { get; }
        /// <summary>
        /// SubtitlesFormat.ToString()
        /// </summary>
        /// <returns>The name of this format</returns>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// Get the description of this format
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// Get the supported extensions for this format, the extensions started with dot "."
        /// </summary>
        public abstract string[] Extensions { get; }
        /// <summary>
        /// Get or set the path of this format file
        /// </summary>
        public virtual string FilePath { get { return path; } set { path = value; } }
        /// <summary>
        /// Get or set the subtitle tracks, only if this format support multi tracks, otherwise use "SubtitleTrack" property
        /// </summary>
        public virtual List<SubtitlesTrack> SubtitleTracks { get { return tracks; } set { tracks = value; } }
        /// <summary>
        /// Get or set the subtitles track that will be saved to format file, or hold imported subtitles. If this track is multi track supported, use "SubtitleTracks" property instead
        /// </summary>
        public virtual SubtitlesTrack SubtitleTrack { get { return selectedTrack; } set { selectedTrack = value; } }
        /// <summary>
        /// Get if this format is multi-tracks format
        /// </summary>
        public virtual bool IsMultiTrack { get { return false; } }
        /// <summary>
        /// Get if this format support frameRate
        /// </summary>
        public virtual bool HasFrameRate { get { return false; } }
        /// <summary>
        /// Get or set selected frameRate
        /// </summary>
        public virtual double FrameRate { get { return frameRate; } set { frameRate = value; } }
        /// <summary>
        /// Get supported frameRates
        /// </summary>
        public virtual double[] FrameRates { get { return null; } }
        /// <summary>
        /// Get if this format has options editor
        /// </summary>
        public virtual bool HasOptions { get { return false; } }
        /// <summary>
        /// Get the control that edit options of this format
        /// </summary>
        public virtual UserControl OptionsControl { get { return null; } }
        /// <summary>
        /// Get or set a value indicate whether this format is enabled or not, not effect anything, just a property.
        /// </summary>
        public virtual bool Enabled { get { return enabled; } set { enabled = value; } }
        //Methods
        /// <summary>
        /// Check given file if it match this format
        /// </summary>
        /// <param name="filePath">The file full path</param>
        /// <param name="encoding">The encoding used to open the file</param>
        /// <returns>True if this file matches this format otherwise false</returns>
        public abstract bool CheckFile(string filePath, Encoding encoding);
        /// <summary>
        /// Open given file as this format
        /// </summary>
        /// <param name="filePath">The file full path</param>
        /// <param name="encoding">The encoding used to open the file</param>
        /// <returns>True if loaded success, false if not</returns>
        public abstract void Load(string filePath, Encoding encoding);
        /// <summary>
        /// Save format as file at given path
        /// </summary>
        /// <param name="filePath">The file full path</param>
        /// <param name="encoding">The encoding used to write the file</param>
        /// <returns>True if saved success, false if not</returns>
        public abstract void Save(string filePath, Encoding encoding);
        /// <summary>
        /// Get the filter of this format that used in open/save file browser dialog
        /// </summary>
        /// <returns></returns>
        public virtual string GetFilter()
        {
            string filter = Name + "|";
            foreach (string ex in Extensions)
            {
                filter += "*" + ex + ";";
            }
            return filter.Substring(0, filter.Length - 1);
        }
        /// <summary>
        /// Release the sources that used by this format and clear tracks
        /// </summary>
        public virtual void Dispose()
        {
            path = "";
            frameRate = 10;
            tracks = new List<SubtitlesTrack>();
            selectedTrack = new SubtitlesTrack("SubtitlesTrack");
        }
        //Events
        /// <summary>
        /// Event raised up when this format does a progress at loading operation
        /// </summary>
        public abstract event EventHandler<ProgressArgs> Progress;
        /// <summary>
        /// Event raised up when this format starts the load operation
        /// </summary>
        public abstract event EventHandler LoadStarted;
        /// <summary>
        /// Event raised up when this format finished the loading operation
        /// </summary>
        public abstract event EventHandler LoadFinished;
        /// <summary>
        /// Event raised up when this format starts the save operation
        /// </summary>
        public abstract event EventHandler SaveStarted;
        /// <summary>
        /// Event raised up when this format finished the saving operation
        /// </summary>
        public abstract event EventHandler SaveFinished;
    }
}
