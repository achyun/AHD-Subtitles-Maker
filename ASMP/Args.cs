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
    /// Args for name changing event
    /// </summary>
    public class NameChangedArgs : EventArgs
    {
        /// <summary>
        /// Args for name changing event 
        /// </summary>
        /// <param name="OldName">OldName</param>
        public NameChangedArgs(string OldName)
        { _OldName = OldName; }
        string _OldName = "";
        /// <summary>
        /// Get the old name
        /// </summary>
        public string OldName
        { get { return _OldName; } }
    }
    /// <summary>
    /// Args for progress event
    /// </summary>
    public class ProgressArgs : EventArgs
    {
        int _PrecentageCompleted = 0;
        string _Status = "";
        /// <summary>
        /// Args for progress event
        /// </summary>
        /// <param name="precentageCompleted">Progress precentage</param>
        /// <param name="status">Status of this progress</param>
        public ProgressArgs(int precentageCompleted, string status)
        {
            _PrecentageCompleted = precentageCompleted;
            _Status = status;
        }
        /// <summary>
        /// Get the Precentage Completed
        /// </summary>
        public int PrecentageCompleted
        { get { return _PrecentageCompleted; } }
        /// <summary>
        /// Get the status of this progress
        /// </summary>
        public string Status
        { get { return _Status; } }
    }
    public class MarkEditArgs : EventArgs
    {
        int index = 0;
        public MarkEditArgs(int index)
        {
            this.index = index;
        }
        public int MarkIndex
        { get { return index; } }
    }
    public class MarkSelectionArgs : EventArgs
    {
        int index = 0;
        public MarkSelectionArgs(int index)
        {
            this.index = index;
        }
        public int MarkIndex
        { get { return index; } }
    }
    /// <summary>
    /// Args for selecting subtitle events
    /// </summary>
    public class SubtitlesSelectArgs : EventArgs
    {
        int[] indices;
        /// <summary>
        /// Args for selecting subtitle events
        /// </summary>
        /// <param name="indices">The indices of subtitles to select</param>
        public SubtitlesSelectArgs(int[] indices)
        {
            this.indices = indices;
        }
        /// <summary>
        /// The indices of subtitles to select
        /// </summary>
        public int[] Indices
        { get { return indices; } }
    }
}
