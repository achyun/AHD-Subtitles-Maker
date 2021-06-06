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

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Subtitles Track, a class holds subtitles.
    /// </summary>
    [Serializable()]
    public class SubtitlesTrack
    {
        /// <summary>
        /// Subtitles Track, a class holds subtitles.
        /// </summary>
        public SubtitlesTrack()
        {
            this.name = "";
            subtitles = new List<Subtitle>();
            this.Preview = true;
        }
        /// <summary>
        /// Subtitles Track, a class holds subtitles.
        /// </summary>
        /// <param name="name">The name of this track</param>
        public SubtitlesTrack(string name)
        {
            this.name = name;
            subtitles = new List<Subtitle>();
            this.Preview = true;
        }

        private string name;
        private List<Subtitle> subtitles;
        private bool rtl;

        /// <summary>
        /// Get or set the name of this track
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the subtitles collection
        /// </summary>
        public List<Subtitle> Subtitles
        { get { return subtitles; } set { subtitles = value; } }
        /// <summary>
        /// Get or set a value indecate wether to set right to left alignement by default to new subtitle added
        /// </summary>
        public bool RightToLeft
        { get { return rtl; } set { rtl = value; } }
        /// <summary>
        /// Get or set if this track can be shown on some controls.
        /// </summary>
        public bool Preview
        { get; set; }
        /// <summary>
        /// Subtitles Track name
        /// </summary>
        /// <returns>The name of this track</returns>
        public override string ToString()
        {
            return name;
        }
        public void CheckSubtitlesOrder()
        {
            double min = 0;
            for (int i = 0; i < subtitles.Count; i++)
            {
                if (min < subtitles[i].StartTime)
                    min = subtitles[i].StartTime;
                else
                {
                    // This is it !!
                    // 1 Sort the subtitles
                    subtitles.Sort(new SubtitlesComparer(true));
                    break;
                }
            }
        }
        /// <summary>
        /// Clone this track
        /// </summary>
        /// <returns>An exact copy of this track</returns>
        public SubtitlesTrack Clone()
        {
            SubtitlesTrack newTrack = new SubtitlesTrack(this.name);
            newTrack.rtl = this.rtl;
            newTrack.Preview = this.Preview;

            foreach (Subtitle sub in this.subtitles)
            {
                newTrack.subtitles.Add(sub.Clone());
            }

            return newTrack;
        }
    }
}
