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
    /// Class represents char
    /// </summary>
    [Serializable()]
    public class SubtitleChar
    {
        /// <summary>
        /// Class represents char
        /// </summary>
        /// <param name="theChar">The char</param>
        /// <param name="font">The font of this char</param>
        /// <param name="color">The color of this char</param>
        public SubtitleChar(char theChar, Font font, Color color)
        {
            this.theChar = theChar;
            this.Font = font;
            this.Color = color;
        }
        /*Saving in this way will reduce file size*/
        string font;
        int color;
        char theChar;

        /// <summary>
        /// Get or set the char
        /// </summary>
        public char TheChar
        { get { return theChar; } set { theChar = value; } }
        /// <summary>
        /// Get or set the char font
        /// </summary>
        public Font Font
        {
            get
            {
                if (font == "")
                    return new System.Drawing.Font("Tahoma", 8, FontStyle.Regular);
                string[] code = font.Split(new string[] { " - " }, StringSplitOptions.None);
                string name = code[0].Substring(1);
                float size = float.Parse(code[1]);
                string style = code[2].Substring(0, code[2].Length - 1);

                string[] styleCode = style.Split(new string[] { ", " }, StringSplitOptions.None);
                FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), styleCode[0]);
                for (int i = 1; i < styleCode.Length; i++)
                {
                    fontStyle |= (FontStyle)Enum.Parse(typeof(FontStyle), styleCode[i]);
                }

                return new Font(name, size, fontStyle);
            }
            set
            {
                if (value != null)
                    font = "<" + value.Name + " - " + value.Size + " - " + value.Style + ">";
                else
                    font = "";
            }
        }
        /// <summary>
        /// Get or set the color of this char
        /// </summary>
        public Color Color
        { get { return Color.FromArgb(color); } set { color = value.ToArgb(); } }

        /// <summary>
        /// Get the char as string
        /// </summary>
        /// <returns>String the char</returns>
        public override string ToString()
        {
            return theChar.ToString();
        }

        public static bool operator ==(SubtitleChar a, SubtitleChar b)
        {
            return (a.color == b.color && a.font == b.font && a.theChar == b.theChar);
        }
        public static bool operator !=(SubtitleChar a, SubtitleChar b)
        {
            return (a.color != b.color || a.font != b.font || a.theChar != b.theChar);
        }
    }
}
