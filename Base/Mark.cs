﻿// This file is part of AHD Subtitles Maker
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
    /// Mark !!
    /// </summary>
    [Serializable()]
    public class Mark
    {
        string _Name = "";
        double _Time = 0;
        /// <summary>
        /// Get or set the name of this mark
        /// </summary>
        public string Name
        { get { return _Name; } set { _Name = value; } }
        /// <summary>
        /// Get or set the time of this mark
        /// </summary>
        public double Time
        { get { return _Time; } set { _Time = value; } }
    }
}
