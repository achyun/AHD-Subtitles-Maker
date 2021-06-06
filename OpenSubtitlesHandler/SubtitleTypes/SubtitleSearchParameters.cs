﻿/* This file is part of OpenSubtitles Handler
   A library that handle OpenSubtitles.org XML-RPC methods.

   Copyright © Ala Ibrahim Hadid 2013 - 2021

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.

   Author email: mailto:alaahadidfreeware@gmail.com

 */
using System;

namespace OpenSubtitlesHandler
{
    /// <summary>
    /// Paramaters for subtitle search call
    /// </summary>
    public struct SubtitleSearchParameters
    {
        /// <summary>
        /// Paramaters for subtitle search call
        /// </summary>
        /// <param name="subLanguageId">List of language ISO639-3 language codes to search for, divided by ',' (e.g. 'cze,eng,slo')</param>
        /// <param name="movieHash">Video file hash as calculated by one of the implementation functions as seen on http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes</param>
        /// <param name="movieByteSize">Size of video file in bytes </param>
        public SubtitleSearchParameters(string subLanguageId, string movieHash, int movieByteSize)
        {
            this.subLanguageId = subLanguageId;
            this.movieHash = movieHash;
            this.movieByteSize = movieByteSize;
            this.imdbid = "";
        }
        /// <summary>
        /// Paramaters for subtitle search call
        /// </summary>
        /// <param name="subLanguageId">List of language ISO639-3 language codes to search for, divided by ',' (e.g. 'cze,eng,slo')</param>
        /// <param name="movieHash">Video file hash as calculated by one of the implementation functions as seen on http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes</param>
        /// <param name="movieByteSize">Size of video file in bytes </param>
        /// <param name="imdbid"> IMDb ID of movie this video is part of, belongs to.</param>
        public SubtitleSearchParameters(string subLanguageId, string movieHash, int movieByteSize, string imdbid)
        {
            this.subLanguageId = subLanguageId;
            this.movieHash = movieHash;
            this.movieByteSize = movieByteSize;
            this.imdbid = imdbid;
        }

        private string subLanguageId;
        private string movieHash;
        private int movieByteSize;
        private string imdbid;

        /// <summary>
        /// List of language ISO639-3 language codes to search for, divided by ',' (e.g. 'cze,eng,slo')
        /// </summary>
        public string SubLangaugeID { get { return subLanguageId; } set { subLanguageId = value; } }
        /// <summary>
        /// Video file hash as calculated by one of the implementation functions as seen on http://trac.opensubtitles.org/projects/opensubtitles/wiki/HashSourceCodes
        /// </summary>
        public string MovieHash { get { return movieHash; } set { movieHash = value; } }
        /// <summary>
        /// Size of video file in bytes 
        /// </summary>
        public int MovieByteSize { get { return movieByteSize; } set { movieByteSize = value; } }
        /// <summary>
        /// ​IMDb ID of movie this video is part of, belongs to.
        /// </summary>
        public string IMDbID  { get { return imdbid; } set { imdbid = value; } }
    }
}
