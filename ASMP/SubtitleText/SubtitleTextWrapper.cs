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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AHD.SM.ASMP
{
    public class SubtitleTextWrapper
    {
        /// <summary>
        /// Get a list of used fonts in a subtitle text
        /// </summary>
        /// <param name="text">The subtitle text that includes the fonts</param>
        /// <returns>A list of used fonts</returns>
        public static Font[] GetFonts(SubtitleText text)
        {
            List<Font> fonts = new List<Font>();
            foreach (SubtitleLine line in text.TextLines)
            {
                foreach (SubtitleChar chr in line.Chars)
                {
                    if (!fonts.Contains(chr.Font))
                    { fonts.Add(chr.Font); }
                }
            }
            return fonts.ToArray();
        }
        /// <summary>
        /// Get a list of used colors in a subtitle text
        /// </summary>
        /// <param name="text">The subtitle text that includes the colors</param>
        /// <returns>A list of used colors</returns>
        public static Color[] GetColors(SubtitleText text)
        {
            List<Color> colors = new List<Color>();
            foreach (SubtitleLine line in text.TextLines)
            {
                foreach (SubtitleChar chr in line.Chars)
                {
                    if (!colors.Contains(chr.Color))
                    { colors.Add(chr.Color); }
                }
            }
            return colors.ToArray();
        }
        /// <summary>
        /// Calculate the size of subtitle text
        /// </summary>
        /// <param name="text">The subtitle text</param>
        /// <returns>The size of the text</returns>
        public static Size GetSize(SubtitleText text)
        {
            Size size;
            int height = 0;
            int width = 0;
            foreach (SubtitleLine line in text.TextLines)
            {
                int lineW = 0;
                int lineH = 0;
                foreach (SubtitleChar chr in line.Chars)
                {
                    Size chrSize = TextRenderer.MeasureText(chr.ToString(), chr.Font);
                    lineW += chrSize.Width;
                    if (chrSize.Height > lineH)
                        lineH = chrSize.Height;
                }
                //update original
                height += lineH;
                if (lineW > width)
                    width = lineW;
            }
            size = new Size(width / 2, height);
            return size;
        }
        /// <summary>
        /// Create a clone of a subtitle text
        /// </summary>
        /// <param name="text">The subtitle text to clone</param>
        /// <returns>A clone of given subtitle text</returns>
        public static SubtitleText Clone(SubtitleText text)
        {
            SubtitleText newText = new SubtitleText();
            newText.CustomPosition = text.CustomPosition;
            newText.IsCustomPosition = text.IsCustomPosition;
            newText.Position = text.Position;
            foreach (SubtitleLine line in text.TextLines)
            {
                SubtitleLine newline = new SubtitleLine();
                newline.Alignement = line.Alignement;
                foreach (SubtitleChar chr in line.Chars)
                {
                    newline.Chars.Add(new SubtitleChar(chr.TheChar, chr.Font, chr.Color));
                }
                newText.TextLines.Add(newline);
            }
            newText.RighttoLeft = text.RighttoLeft;
            return newText;
        }
    }
}
