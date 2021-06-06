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
using System.Windows.Forms;
namespace AHD.SM.ASMP
{
    /// <summary>
    /// Class represents subtitle's text
    /// </summary>
    [Serializable()]
    public class SubtitleText
    {
        /// <summary>
        /// Class represents subtitle's text
        /// </summary>
        public SubtitleText()
        {
            textLines = new List<SubtitleLine>();
            position = SubtitlePosition.Down_Middle;
            isCustomPosition = false;
            customPosition = new Point();
        }

        List<SubtitleLine> textLines;
        SubtitlePosition position;
        bool isCustomPosition;
        Point customPosition;
        bool rightToLeft;

        /// <summary>
        /// Get or set the text lines of this subtitle
        /// </summary>
        public List<SubtitleLine> TextLines
        { get { return textLines; } set { textLines = value; } }
        /// <summary>
        /// Get or set the position of this subtitle.
        /// </summary>
        public SubtitlePosition Position
        { get { return position; } set { position = value; } }
        /// <summary>
        /// Get or set if the position is custom, this is just a flag has no effect on the other properties.
        /// </summary>
        public bool IsCustomPosition
        { get { return isCustomPosition; } set { isCustomPosition = value; } }
        /// <summary>
        /// Get or set the custom position of this text. You should use this value only if "IsCustomPosition" flag is set.
        /// </summary>
        public Point CustomPosition
        { get { return customPosition; } set { customPosition = value; } }
        /// <summary>
        /// Get or set a value indecate wether this text is right to left aligne
        /// </summary>
        public bool RighttoLeft
        { get { return rightToLeft; } set { rightToLeft = value; } }
        /// <summary>
        /// Get subtitle text as lines (if multilines)
        /// </summary>
        public string[] Lines
        {
            get
            {
                List<string> ret = new List<string>();
                if (textLines == null)
                    return ret.ToArray();
                if (textLines.Count == 0)
                    return ret.ToArray();

                for (int i = 0; i < textLines.Count; i++)
                    ret.Add(textLines[i].ToString());
                return ret.ToArray();
            }
        }
        /// <summary>
        /// Get the subtitle lines as string
        /// </summary>
        public override string ToString()
        {
            if (textLines == null)
                return "";
            if (textLines.Count == 0)
                return "";
            string text = textLines[0].ToString();
            for (int i = 1; i < textLines.Count; i++)
                text += "\n" + textLines[i];
            return text;
        }
        /// <summary>
        /// Apply a font to all chars of this text
        /// </summary>
        /// <param name="font">The font</param>
        public void SetFont(Font font)
        {
            foreach (SubtitleLine line in this.TextLines)
            {
                foreach (SubtitleChar chr in line.Chars)
                {
                    chr.Font = font;
                }
            }
        }
        /// <summary>
        /// Apply a color to all chars of this text
        /// </summary>
        /// <param name="color">The color</param>
        public void SetColor(Color color)
        {
            foreach (SubtitleLine line in this.TextLines)
            {
                foreach (SubtitleChar chr in line.Chars)
                {
                    chr.Color = color;
                }
            }
        }

        public bool ContainsChar(SubtitleChar char_)
        {
            foreach (SubtitleLine line in textLines)
            {
                foreach (SubtitleChar cc in line.Chars)
                {
                    if (cc == char_)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Create new subttile text item from string with default font (Tahoma , 8) and white color
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>The subtitle text from givin string with default font (Tahoma , 8) and white color</returns>
        public static SubtitleText FromString(string text)
        { return FromString(text, new Font("Tahoma", 8, FontStyle.Regular), Color.White); }
        /// <summary>
        /// Create new subttile text item from string
        /// </summary>
        /// <param name="text">The text</param>
        /// <param name="font">The font that will be applyed to all chars</param>
        /// <param name="color">The color that will be applyed to all chars</param>
        /// <returns>The subtitle text from givin string with given font and color</returns>
        public static SubtitleText FromString(string text, Font font, Color color)
        {
            SubtitleText newText = new SubtitleText();
            newText.CustomPosition = new Point();
            newText.IsCustomPosition = false;
            newText.Position = SubtitlePosition.Down_Middle;
            newText.TextLines = new List<SubtitleLine>();
            string[] lines = text.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                SubtitleLine line = new SubtitleLine();
                line.Alignement = LineAlignement.Center;
                foreach (char chr in lines[i].ToCharArray())
                {
                    line.Chars.Add(new SubtitleChar(chr, font, color));
                }
                newText.TextLines.Add(line);
            }
            return newText;
        }
        public SubtitleText Clone()
        {
            SubtitleText text = new SubtitleText();
            text.textLines = new List<SubtitleLine>();
            for (int line = 0; line < textLines.Count; line++)
            {
                text.textLines.Add(textLines[line].Clone());
            }
            text.position = this.position;
            text.isCustomPosition = this.isCustomPosition;
            text.customPosition = this.customPosition;
            text.rightToLeft = this.rightToLeft;

            return text;
        }
    }
}