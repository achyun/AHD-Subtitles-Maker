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
    /// Comparer used to compare two subtitles
    /// </summary>
    public class SubtitleComparer : IComparer<Subtitle>
    {
        public SubtitleComparer()
        {
            mode = SubtitleCompareType.StartTime;
        }
        public SubtitleComparer(SubtitleCompareType compareMode)
        {
            mode = compareMode;
        }
        SubtitleCompareType mode = SubtitleCompareType.StartTime;
        public int Compare(Subtitle x, Subtitle y)
        { /*
             * Less than zero   : x is less than y. 
             * Zero             : x equals y. 
             * Greater than zero: x is greater than y. 
             */
            switch (mode)
            {
                default:
                case SubtitleCompareType.StartTime:
                    return (int)(x.StartTime - y.StartTime);
                case SubtitleCompareType.EndTime:
                    return (int)(x.EndTime - y.EndTime);
            }
        }
    }
    /// <summary>
    /// Comparer used to compare two timemarks
    /// </summary>
    public class TimeMarkComparer : IComparer<TimeMark>
    {
        /// <summary>
        /// Compare timemarks
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(TimeMark x, TimeMark y)
        {
            return (int)(x.Time - y.Time);
        }
    }
    public enum SubtitleCompareType
    { StartTime, EndTime }
}