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
    public class TimeFormatChecker
    {
        /// <summary>
        /// check if given time format is in Timespan.X form (Timespan.milli or Timespan:frame)
        /// </summary>
        /// <param name="time">The time in Timespan form (Timespan.milli or Timespan.frame)</param>
        /// <returns>True if the time is is in Timespan form (Timespan.milli or Timespan.frame), otherwise false</returns>
        public static bool IsTimeSpanX(string time)
        {
            string[] ent = time.Split(new char[] { ':', ';', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (ent.Length == 4)
            {
                int x = 0;
                if (int.TryParse(ent[0], out x) & int.TryParse(ent[1], out x)
                  & int.TryParse(ent[2], out x) & int.TryParse(ent[3], out x))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// check if given time format is in Timespan form
        /// </summary>
        /// <param name="time">The time in Timespan form</param>
        /// <returns>True if the time is is in Timespan form, otherwise false</returns>
        public static bool IsTimeSpan(string time)
        {
            string[] ent = time.Split(new char[] { ':', ';', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (ent.Length == 4)
            {
                int x = 0;
                if (int.TryParse(ent[0], out x) & int.TryParse(ent[1], out x)
                  & int.TryParse(ent[2], out x))
                    return true;
            }
            return false;
        }
    }
}
