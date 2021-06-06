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
    /// Class of subtitle
    /// </summary>
    [Serializable()]
    public class Subtitle
    {
        /// <summary>
        /// Class of subtitle
        /// </summary>
        public Subtitle()
        {
            startTime = 0;
            endTime = 0;
            text = new SubtitleText();
        }
        /// <summary>
        /// Class of subtitle
        /// </summary>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="text">The text</param>
        public Subtitle(double startTime, double endTime, SubtitleText text)
        {
            this.startTime = startTime;
            this.endTime = endTime;
            this.text = text;
        }

        double startTime;
        double endTime;
        SubtitleText text;

        /// <summary>
        /// Get or set the start time
        /// </summary>
        public double StartTime
        { get { return startTime; } set { startTime = value; } }
        /// <summary>
        /// Get or set the end time
        /// </summary>
        public double EndTime
        { get { return endTime; } set { endTime = value; } }
        /// <summary>
        /// Get the duration = end time - start time
        /// </summary>
        public double Duration
        { get { return endTime - startTime; } }
        /// <summary>
        /// Get or set the text of this subtitle as SubtitleText type.
        /// </summary>
        public SubtitleText Text
        { get { return text; } set { text = value; } }
        /// <summary>
        /// Get this subtitle text
        /// </summary>
        /// <returns>Subtitle's text</returns>
        public override string ToString()
        {
            return text.ToString();
        }
        /// <summary>
        /// Clone this subtitle
        /// </summary>
        /// <returns>Subtitle has the same values but not the subtitle itself</returns>
        public Subtitle Clone()
        {
            Subtitle sub = new Subtitle();
            sub.StartTime = this.startTime;
            sub.EndTime = this.endTime;
            sub.Text = SubtitleTextWrapper.Clone(this.Text);
            return sub;
        }
    }
}
