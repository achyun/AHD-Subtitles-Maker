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

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Time mark class, represents a mark that can set on time
    /// </summary>
    [Serializable()]
    public class TimeMark
    {
        /// <summary>
        /// Time mark class, represents a mark that can set on time
        /// </summary>
        /// <param name="name">The name of this mark</param>
        /// <param name="time">The time of this mark</param>
        public TimeMark(string name,double time)
        {
            this.name = name;
            this.time = time;
        }

        string name;
        double time;

        /// <summary>
        /// Get or set the name of this mark
        /// </summary>
        public string Name
        { get { return name; } set { name = value; } }
        /// <summary>
        /// Get or set the time of this mark
        /// </summary>
        public double Time
        { get { return time; } set { time = value; } }
        /// <summary>
        /// The time of this mark
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return name;
        }
    }
}
