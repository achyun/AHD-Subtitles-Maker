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
using System.Collections.Generic;

namespace AHD.SM.ASMP
{
    class SubtitlesComparer : IComparer<Subtitle>
    {
        public SubtitlesComparer(bool isStartTimeCompare)
        {
            this.isStartTimeCompare = isStartTimeCompare;
        }

        private bool isStartTimeCompare;

        public int Compare(Subtitle x, Subtitle y)
        {
            if (isStartTimeCompare)
                return (int)((x.StartTime * 1000) - (y.StartTime * 1000));
            else
                return (int)((x.EndTime * 1000) - (y.EndTime * 1000));
        }
    }
}
