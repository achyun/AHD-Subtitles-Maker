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
using System.Drawing;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Class represents line
    /// </summary>
    [Serializable()]
    public class SubtitleLine
    {
        /// <summary>
        /// Class represents line
        /// </summary>
        public SubtitleLine()
        {
            chars = new List<SubtitleChar>();
            alignement = LineAlignement.Center;
        }

        List<SubtitleChar> chars;
        LineAlignement alignement;

        /// <summary>
        /// Get or set the chars collection of this line
        /// </summary>
        public List<SubtitleChar> Chars
        { get { return chars; } set { chars = value; } }
        /// <summary>
        /// Get or set the line alignement
        /// </summary>
        public LineAlignement Alignement
        { get { return alignement; } set { alignement = value; } }
        /// <summary>
        /// Get the line text
        /// </summary>
        /// <returns>The line as string</returns>
        public override string ToString()
        {
            if (chars == null)
                return "";

            string text = "";
            foreach (SubtitleChar chr in chars)
                text += chr.TheChar;
            return text;
        }
        public static SubtitleLine FromString(string text)
        { return FromString(text, new Font("Tahoma", 8, FontStyle.Regular), Color.White); }
        public static SubtitleLine FromString(string text, Font font, Color color)
        {
            SubtitleLine line = new SubtitleLine();
            line.Alignement = LineAlignement.Center;
            foreach (char chr in text.ToCharArray())
            {
                line.Chars.Add(new SubtitleChar(chr, font, color));
            }
            return line;
        }
        public SubtitleLine Clone()
        {
            SubtitleLine line = new SubtitleLine();
            line.alignement = alignement;
            for (int chr = 0; chr < chars.Count; chr++)
            {
                line.Chars.Add(new SubtitleChar(chars[chr].TheChar, chars[chr].Font, chars[chr].Color));
            }
            return line;
        }
    }
}
