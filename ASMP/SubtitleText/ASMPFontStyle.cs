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
using System.Drawing;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Represents a font style.
    /// </summary>
    [Serializable]
    public class ASMPFontStyle
    {
        public ASMPFontStyle()
        {
            Name = "Default";
            Font = new Font("Tahoma", 8, System.Drawing.FontStyle.Regular);
            Color = Color.White;
        }
        public ASMPFontStyle(string name, Font font, Color color)
        {
            Name = name;
            Font = font;
            Color = color;
        }
        /// <summary>
        /// Get or set the name of this font.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Get or set the font of this style.
        /// </summary>
        public Font Font { get; set; }
        /// <summary>
        /// Get or set the color of this style.
        /// </summary>
        public Color Color { get; set; }

        public override string ToString()
        {
            return string.Format("{0} [{1}, {2}, {3}, {4}]", Name, Font.Name, Font.Size, Font.Style, Color);
        }
    }
}
