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

namespace AHD.ID3.Editor.GUI
{
    /// <summary>
    /// Time Change Args for time change events
    /// </summary>
    public class TimeChangeArgs : EventArgs
    {
        /// <summary>
        /// Create new time change args
        /// </summary>
        /// <param name="newTime">The new time after the change</param>
        /// <param name="oldTime">The old time before the change</param>
        public TimeChangeArgs(double newTime, double oldTime)
        {
            this.newTime = newTime;
            this.oldTime = oldTime;
        }
        double newTime = 0;
        double oldTime = 0;
        /// <summary>
        /// Get the new time after the change
        /// </summary>
        public double NewTime
        { get { return newTime; } }
        /// <summary>
        /// Get the old time before the change
        /// </summary>
        public double OldTime
        { get { return oldTime; } }
    }
}
