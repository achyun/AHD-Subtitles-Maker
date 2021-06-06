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
using AHD.SM.ASMP;

namespace AHD.SM.Formats
{
    public class TextFormatter
    {
        private static RichTextBox cod = new RichTextBox();
        struct CodeInfo
        {
            public CodeInfo(string code, int line, int charIndex)
            {
                Code = code;
                Line = line;
                CharIndexInLine = charIndex;
            }
            public string Code;
            public int Line;
            public int CharIndexInLine;
        }
        public static SubtitleText GetCaptionsOnly(SubtitleText subText, bool isSurrounds, string char_surr_first, string char_surr_second)
        {
            SubtitleText sub_text = new SubtitleText();
            // Collect code locations
            List<CodeInfo> codes = new List<CodeInfo>();
            string[] text_lines = subText.Lines;
            for (int line = 0; line < text_lines.Length; line++)
            {
                for (int ch = 0; ch < text_lines[line].Length; ch++)
                {
                    if (text_lines[line][ch].ToString() == char_surr_first)
                    {
                        codes.Add(new CodeInfo(char_surr_first, line, ch));
                    }
                    if (text_lines[line][ch].ToString() == char_surr_second)
                    {
                        codes.Add(new CodeInfo(char_surr_second, line, ch));
                    }
                }
            }
            // Now we have the information we need, let's start with the process.
            bool canDoIt = false;
            foreach (CodeInfo cc in codes)
            {
                if (cc.Code == char_surr_first)
                {
                    foreach (CodeInfo cc1 in codes)
                    {
                        if (cc1.Code == char_surr_second)
                        {
                            // Final check
                            if (cc1.Line == cc.Line && cc1.CharIndexInLine > cc.CharIndexInLine)
                            {
                                canDoIt = true;
                                break;
                            }
                            else if (cc1.Line > cc.Line && cc1.CharIndexInLine != cc.CharIndexInLine)
                            {
                                canDoIt = true;
                                break;
                            }
                        }
                    }
                    if (canDoIt)
                        break;
                }
            }

            if (isSurrounds)
            {
                // Surround mode is difficult...
                if (canDoIt)
                {
                    int code_index = 0;
                    for (int line = codes[code_index].Line; line < subText.TextLines.Count; line++)
                    {
                        SubtitleLine current_line = subText.TextLines[line];
                        SubtitleLine new_line = new SubtitleLine();
                        new_line.Alignement = current_line.Alignement;
                        for (int ch = codes[code_index].CharIndexInLine; ch < current_line.Chars.Count; ch++)
                        {
                            new_line.Chars.Add(new SubtitleChar(
                                current_line.Chars[ch].TheChar,
                                current_line.Chars[ch].Font,
                                current_line.Chars[ch].Color));
                            ch++;
                            code_index++;
                            while (ch < current_line.Chars.Count)
                            {
                                if ((codes[code_index].Line == line) && (codes[code_index].CharIndexInLine == ch))
                                {
                                    // This is it !!
                                    new_line.Chars.Add(new SubtitleChar(
                                        current_line.Chars[ch].TheChar,
                                        current_line.Chars[ch].Font,
                                        current_line.Chars[ch].Color));
                                    sub_text.TextLines.Add(new_line);
                                    code_index++;
                                    if (code_index < codes.Count)
                                    {
                                        line = codes[code_index].Line;
                                        if (line < subText.TextLines.Count)
                                            current_line = subText.TextLines[line];
                                    }
                                    break;
                                }
                                else
                                {
                                    // Add normally
                                    new_line.Chars.Add(new SubtitleChar(
                                        current_line.Chars[ch].TheChar,
                                        current_line.Chars[ch].Font,
                                        current_line.Chars[ch].Color));
                                }
                                ch++;
                                if (ch == current_line.Chars.Count)
                                {
                                    // New line is needed !!
                                    line++;
                                    if (line < subText.TextLines.Count)
                                    {
                                        ch = 0;
                                        sub_text.TextLines.Add(new_line);

                                        current_line = subText.TextLines[line];
                                        new_line = new SubtitleLine();
                                        new_line.Alignement = current_line.Alignement;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            if (code_index == codes.Count)
                                break;
                        }
                        if (code_index == codes.Count)
                            break;
                    }
                }
            }
            else
            {
                // No surround, simply look for the first code, keep adding till the end of the text.
                if (codes.Count > 0)
                {
                    int code_index = 0;
                    for (int line = codes[code_index].Line; line < subText.TextLines.Count; line++)
                    {
                        SubtitleLine current_line = subText.TextLines[line];
                        SubtitleLine new_line = new SubtitleLine();
                        for (int ch = codes[code_index].CharIndexInLine; ch < current_line.Chars.Count; ch++)
                        {
                            new_line.Chars.Add(new SubtitleChar(
                                current_line.Chars[ch].TheChar,
                                current_line.Chars[ch].Font,
                                current_line.Chars[ch].Color));
                            ch++;

                            while (ch < current_line.Chars.Count)
                            {

                                // Add normally
                                new_line.Chars.Add(new SubtitleChar(
                                    current_line.Chars[ch].TheChar,
                                    current_line.Chars[ch].Font,
                                    current_line.Chars[ch].Color));

                                ch++;
                                if (ch == current_line.Chars.Count)
                                {
                                    // New line is needed !!
                                    sub_text.TextLines.Add(new_line);
                                    line++;
                                    if (line < subText.TextLines.Count)
                                    {
                                        ch = 0;
                                        current_line = subText.TextLines[line];
                                        new_line = new SubtitleLine();
                                        new_line.Alignement = current_line.Alignement;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sub_text;
        }
        public static SubtitleText GetTextOnly(SubtitleText subText, bool isSurrounds, string char_surr_first, string char_surr_second)
        {
            SubtitleText sub_text = new SubtitleText();
            // Collect code locations
            List<CodeInfo> codes = new List<CodeInfo>();
            string[] text_lines = subText.Lines;
            for (int line = 0; line < text_lines.Length; line++)
            {
                for (int ch = 0; ch < text_lines[line].Length; ch++)
                {
                    if (text_lines[line][ch].ToString() == char_surr_first)
                    {
                        codes.Add(new CodeInfo(char_surr_first, line, ch));
                    }
                    if (text_lines[line][ch].ToString() == char_surr_second)
                    {
                        codes.Add(new CodeInfo(char_surr_second, line, ch));
                    }
                }
            }

            if (codes.Count == 0)
                return subText.Clone();
            if (isSurrounds)
            {
                // Surround mode is difficult...

                int code_index = 0;
                for (int line = 0; line < subText.TextLines.Count; line++)
                {
                    SubtitleLine current_line = subText.TextLines[line];
                    SubtitleLine new_line = new SubtitleLine();
                    new_line.Alignement = current_line.Alignement;
                    int ch = 0;
                    while (ch < current_line.Chars.Count)
                    {
                        if ((codes[code_index].Line == line) && (codes[code_index].CharIndexInLine == ch))
                        {
                            // This is it !!
                            sub_text.TextLines.Add(new_line);
                            code_index++;
                            if (code_index < codes.Count)
                            {
                                line = codes[code_index].Line;
                                if (line < subText.TextLines.Count)
                                    current_line = subText.TextLines[line];
                            }
                            break;
                        }
                        else
                        {
                            // Add normally
                            new_line.Chars.Add(new SubtitleChar(
                                current_line.Chars[ch].TheChar,
                                current_line.Chars[ch].Font,
                                current_line.Chars[ch].Color));
                        }
                        ch++;
                        if (ch == current_line.Chars.Count)
                        {
                            // New line is needed !!
                            line++;
                            if (line < subText.TextLines.Count)
                            {
                                ch = 0;
                                sub_text.TextLines.Add(new_line);

                                current_line = subText.TextLines[line];
                                new_line = new SubtitleLine();
                                new_line.Alignement = current_line.Alignement;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (code_index == codes.Count)
                        break;
                }
            }
            else
            {
                // No surround, simply look for the first code, keep adding till the end of the text.
                if (codes.Count > 0)
                {
                    int code_index = 0;
                    for (int line = 0; line < subText.TextLines.Count; line++)
                    {
                        SubtitleLine current_line = subText.TextLines[line];
                        SubtitleLine new_line = new SubtitleLine();
                        int ch = 0;
                        while (ch < current_line.Chars.Count)
                        {
                            if ((codes[code_index].Line == line) && (codes[code_index].CharIndexInLine == ch))
                            {
                                // This is it !!
                                sub_text.TextLines.Add(new_line);
                                if (code_index < codes.Count)
                                {
                                    line = codes[code_index].Line;
                                    if (line < subText.TextLines.Count)
                                        current_line = subText.TextLines[line];
                                }
                                break;
                            }
                            else
                            {
                                // Add normally
                                new_line.Chars.Add(new SubtitleChar(
                                    current_line.Chars[ch].TheChar,
                                    current_line.Chars[ch].Font,
                                    current_line.Chars[ch].Color));
                            }
                            ch++;
                            if (ch == current_line.Chars.Count)
                            {
                                // New line is needed !!
                                line++;
                                if (line < subText.TextLines.Count)
                                {
                                    ch = 0;
                                    sub_text.TextLines.Add(new_line);

                                    current_line = subText.TextLines[line];
                                    new_line = new SubtitleLine();
                                    new_line.Alignement = current_line.Alignement;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return sub_text;
        }
        /*SUB RIP*/
        public static SubtitleText SubtitleTextFromSubRipCode(string text)
        {
            cod.Text = "";
            cod.Clear();
            cod.SelectionColor = Color.White;
            cod.SelectionAlignment = HorizontalAlignment.Center;

            text = text.Replace(@"\N", "\n");
            SubtitleText sub_text = new SubtitleText();
            FontStyle style = FontStyle.Regular;
            List<Color> colors = new List<Color>();
            List<string> fontNames = new List<string>();
            List<float> fontSizes = new List<float>();
            List<string> resetSeq = new List<string>();

            for (int j = 0; j < text.Length; j++)
            {
                #region SubRip HTML CODE !
                if (text[j] == '<')
                {
                    j++;
                    string code = "";
                    while (text[j] != '>')
                    {
                        code += text[j];
                        j++;
                        if (j >= text.Length)
                            break;
                    }
                    if (code.StartsWith("/"))//this is end of code, reset
                    {
                        if (code.Length == 2)//style code
                        {
                            switch (code.ToLower())
                            {
                                case "/b": style &= ~FontStyle.Bold; break;
                                case "/i": style &= ~FontStyle.Italic; break;
                                case "/s": style &= ~FontStyle.Strikeout; break;
                                case "/u": style &= ~FontStyle.Underline; break;
                            }
                        }
                        else if (code.Contains("font"))
                        {
                            if (resetSeq.Count == 0)//no chances to take, if there's nothing to reset, reset all
                            {
                                if (colors.Count > 0)
                                    colors.RemoveAt(colors.Count - 1);
                                if (fontNames.Count > 0)
                                    fontNames.RemoveAt(fontNames.Count - 1);
                                if (fontSizes.Count > 0)
                                    fontSizes.RemoveAt(fontSizes.Count - 1);
                            }
                            else
                            {
                                string resetCode = resetSeq[resetSeq.Count - 1];
                                if (resetCode.Contains("color"))
                                {
                                    //color = Color.White;
                                    if (colors.Count > 0)
                                        colors.RemoveAt(colors.Count - 1);
                                }
                                if (resetCode.Contains("size"))
                                {
                                    if (fontSizes.Count > 0)
                                        fontSizes.RemoveAt(fontSizes.Count - 1);
                                }
                                if (resetCode.Contains("face"))
                                {
                                    if (fontNames.Count > 0)
                                        fontNames.RemoveAt(fontNames.Count - 1);
                                }
                                resetSeq.Remove(resetCode);
                            }
                        }
                    }
                    else if (code.Length == 1)//style code
                    {
                        switch (code.ToLower())
                        {
                            case "b": style |= FontStyle.Bold; break;
                            case "i": style |= FontStyle.Italic; break;
                            case "s": style |= FontStyle.Strikeout; break;
                            case "u": style |= FontStyle.Underline; break;
                        }
                    }
                    else if (code.Contains("font"))//font and/or color
                    {
                        string[] codes = code.Split(new char[] { ' ' });
                        //the first one should be 'font', so start from second one
                        for (int c = 1; c < codes.Length; c++)
                        {
                            if (codes[c].Contains("color"))
                            {
                                string colorCode = codes[c].Replace("color=", "");
                                colorCode = colorCode.Replace(@"""", "");
                                if (colorCode.StartsWith("#"))
                                {
                                    int col = int.Parse(colorCode.Substring(1), System.Globalization.NumberStyles.AllowHexSpecifier);
                                    byte R = (byte)((col & 0xFF0000) >> 16);
                                    byte G = (byte)((col & 0x00FF00) >> 8);
                                    byte B = (byte)((col & 0x0000FF));
                                    //color = System.Drawing.Color.FromArgb(0xFF, R, G, B);
                                    colors.Add(System.Drawing.Color.FromArgb(0xFF, R, G, B));
                                }
                                else//the color is a string... try to parse
                                {
                                    try
                                    {
                                        int col = 0xffffff;
                                        switch (colorCode.ToLower())
                                        {
                                            case "red": col = 0x0000ff; break;
                                            case "blue": col = 0xff0000; break;
                                            case "lime": col = 0x00ff00; break;
                                            case "aqua": col = 0xffff00; break;
                                            case "purple": col = 0x800080; break;
                                            case "yellow": col = 0x00ffff; break;
                                            case "fuchsia": col = 0xff00ff; break;
                                            case "white": col = 0xffffff; break;
                                            case "gray": col = 0x808080; break;
                                            case "maroon": col = 0x000080; break;
                                            case "olive": col = 0x008080; break;
                                            case "black": col = 0x000000; break;
                                            case "silver": col = 0xc0c0c0; break;
                                            case "teal": col = 0x808000; break;
                                            case "green": col = 0x008000; break;
                                            case "navy": col = 0x800000; break;
                                        }

                                        byte B = (byte)((col & 0xFF0000) >> 16);
                                        byte G = (byte)((col & 0x00FF00) >> 8);
                                        byte R = (byte)((col & 0x0000FF));
                                        //color = System.Drawing.Color.FromArgb(0xFF, R, G, B);
                                        colors.Add(System.Drawing.Color.FromArgb(0xFF, R, G, B));
                                    }
                                    catch
                                    {
                                        //color = Color.White;
                                        colors.Add(Color.White);
                                    }
                                }
                            }
                            else if (codes[c].Contains("size"))
                            {
                                string sizeCode = codes[c].Replace("size=", "");
                                sizeCode = sizeCode.Replace(@"""", "");

                                float val = 0;
                                if (float.TryParse(sizeCode, out val))
                                    fontSizes.Add(val);
                                else
                                    fontSizes.Add(8);
                            }
                            else if (codes[c].Contains("face"))
                            {
                                string nameCode = codes[c].Replace("face=", "");
                                nameCode = nameCode.Replace(@"""", "");

                                fontNames.Add(nameCode);
                            }
                        }
                        //set this so we will know which to reset later
                        string resetCode = "";
                        if (code.Contains("color"))
                            resetCode = "color";
                        if (code.Contains("size"))
                            resetCode += ",size";
                        if (code.Contains("face"))
                            resetCode += ",face";
                        resetSeq.Add(resetCode);
                    }
                }
                #endregion
                #region SUBSTATION ALPHA CODE (ASS 1)
                else if (text[j] == '{')
                {
                    j++;
                    string code = "";
                    while (text[j] != '}')
                    {
                        code += text[j];
                        j++;
                    }
                    // DECODE THE CODE ....
                    // Color
                    if (code.Contains(@"\a"))
                    {
                        string alignCode = "";
                        int index = code.IndexOf(@"\a");
                        index += 2;
                        while (code[index].ToString() != @"}" && index < code.Length)
                        {
                            alignCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }
                        switch (alignCode)
                        {
                            case "1": sub_text.Position = SubtitlePosition.Down_Left; break;
                            case "2": sub_text.Position = SubtitlePosition.Down_Middle; break;
                            case "3": sub_text.Position = SubtitlePosition.Down_Right; break;
                            case "9": sub_text.Position = SubtitlePosition.Mid_Left; break;
                            case "10": sub_text.Position = SubtitlePosition.Mid_Middle; break;
                            case "11": sub_text.Position = SubtitlePosition.Mid_Right; break;
                            case "5": sub_text.Position = SubtitlePosition.Top_Left; break;
                            case "6": sub_text.Position = SubtitlePosition.Top_Middle; break;
                            case "7": sub_text.Position = SubtitlePosition.Top_right; break;
                        }
                    }
                    if (code.Contains(@"\pos"))
                    {
                        string alignCode = "";
                        int index = code.IndexOf(@"\pos");
                        index += 5;
                        while (code[index].ToString() != @")" && index < code.Length)
                        {
                            alignCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }

                        string[] posCodes = alignCode.Split(',');
                        if (posCodes.Length == 2)
                        {
                            int pos_x = 0;
                            int pos_y = 0;
                            int.TryParse(posCodes[0], out pos_x);
                            int.TryParse(posCodes[1], out pos_y);
                            sub_text.IsCustomPosition = true;
                            sub_text.CustomPosition = new Point(pos_x, pos_y);
                        }
                    }
                    if (code.Contains(@"\c"))
                    {
                        string colorCode = "";
                        int index = code.IndexOf(@"\c");
                        index += 2;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            colorCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }

                        colorCode = colorCode.Replace("&", "");
                        colorCode = colorCode.Replace("H", "");

                        int col = int.Parse(colorCode, System.Globalization.NumberStyles.AllowHexSpecifier);
                        byte B = (byte)((col & 0xFF0000) >> 16);
                        byte G = (byte)((col & 0x00FF00) >> 8);
                        byte R = (byte)((col & 0x0000FF));
                        colors.Add(System.Drawing.Color.FromArgb(0xFF, R, G, B));
                    }
                    // Font name
                    if (code.Contains(@"\fn"))
                    {
                        string fontCode = "";
                        int index = code.IndexOf(@"\fn");
                        index += 3;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            fontCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }
                        fontNames.Add(fontCode);
                    }
                    // Font size
                    if (code.Contains(@"\fs"))
                    {
                        string fontCode = "";
                        int index = code.IndexOf(@"\fs");
                        index += 3;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            fontCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }

                        fontCode = fontCode.Replace(@"\fs", "");
                        int s = 0;
                        if (int.TryParse(fontCode, out s))
                            fontSizes.Add(s);
                    }
                    // Font styles
                    if (code.Contains(@"\b1")) style |= FontStyle.Bold;
                    if (code.Contains(@"\b0")) style &= ~FontStyle.Bold;
                    if (code.Contains(@"\i1")) style |= FontStyle.Italic;
                    if (code.Contains(@"\i0")) style &= ~FontStyle.Italic;
                    if (code.Contains(@"\u1")) style |= FontStyle.Underline;
                    if (code.Contains(@"\u0")) style &= ~FontStyle.Underline;
                    if (code.Contains(@"\s1")) style |= FontStyle.Strikeout;
                    if (code.Contains(@"\s0")) style &= ~FontStyle.Strikeout;
                    // TODO: ASS alignement !
                    //if (code.Contains(@"\a1")) cod.SelectionAlignment = HorizontalAlignment.Left;
                    //if (code.Contains(@"\a2")) cod.SelectionAlignment = HorizontalAlignment.Center;
                    //if (code.Contains(@"\a3")) cod.SelectionAlignment = HorizontalAlignment.Right;
                }
                #endregion
                else//text char, add to line
                {
                    Color color = Color.White;
                    if (colors.Count > 0)
                        color = colors[colors.Count - 1];
                    string fontName = "Tahoma";
                    if (fontNames.Count > 0)
                        fontName = fontNames[fontNames.Count - 1];
                    float fontSize = 8;
                    if (fontSizes.Count > 0)
                        fontSize = fontSizes[fontSizes.Count - 1];

                    cod.SelectionFont = new Font(fontName, fontSize, style);
                    cod.SelectionColor = color;
                    cod.SelectedText += text[j];
                }
            }
            //return cod.Rtf;
            // RTF to SubText


            foreach (string ln in cod.Lines)
            {
                sub_text.TextLines.Add(new SubtitleLine());
            }
            int currentLine = 0;
            int oldStart = cod.SelectionStart;
            string oldSelection = cod.SelectedText;
            cod.Visible = false;//disable so user won't see the strange things that would happen to the box lol
            cod.DeselectAll();
            cod.SelectionStart = 0;// 1 ?
            bool needToSetAlign = true;

            for (int i = 0; i < cod.Text.ToCharArray().Length; i++)
            {
                // select the char
                cod.Select(i, 1);
                string currentChar = cod.SelectedText;

                if (currentChar != "\n")
                {
                    if (needToSetAlign)
                    {
                        LineAlignement align = LineAlignement.Center;
                        switch (cod.SelectionAlignment)
                        {
                            case HorizontalAlignment.Left: align = LineAlignement.Left; break;
                            case HorizontalAlignment.Right: align = LineAlignement.Right; break;
                        }
                        sub_text.TextLines[currentLine].Alignement = align;
                        needToSetAlign = false;
                    }
                    sub_text.TextLines[currentLine].Chars.Add(new SubtitleChar(currentChar.ToCharArray()[0],
                        cod.SelectionFont, cod.SelectionColor));
                }
                else
                {
                    currentLine++;
                    needToSetAlign = true;
                }
                cod.SelectionStart++;
            }
            return sub_text;
        }
        public static string SubtitleTextToSubRipCode(SubtitleText text,
            bool writeColors, bool writeFonts, bool writeFontSizes, bool writeAlignments, bool useASS)
        {
            if (useASS)
            {
                if (!writeColors && !writeFonts && !writeFontSizes && !writeAlignments)
                    return text.ToString();
            }
            else
            {
                if (!writeColors && !writeFonts && !writeFontSizes)
                    return text.ToString();
            }
            // Subtitle text to RTF
            cod.Text = "";
            cod.Clear();
            cod.SelectionStart = 0;
            if (text.TextLines.Count > 0)
            {
                foreach (SubtitleLine line in text.TextLines)
                {
                    foreach (SubtitleChar chr in line.Chars)
                    {
                        if (chr.Font != null)
                            cod.SelectionFont = chr.Font;
                        else
                            cod.SelectionFont = new Font("Tahoma", 8, FontStyle.Regular);

                        //richTextBox1.SelectionBullet = true;
                        if (chr.Color != null)
                            cod.SelectionColor = chr.Color;
                        else
                            cod.SelectionColor = Color.White;
                        cod.SelectedText = chr.TheChar.ToString();
                        switch (line.Alignement)
                        {
                            case LineAlignement.Center: cod.SelectionAlignment = HorizontalAlignment.Center; break;
                            case LineAlignement.Left: cod.SelectionAlignment = HorizontalAlignment.Left; break;
                            case LineAlignement.Right: cod.SelectionAlignment = HorizontalAlignment.Right; break;
                        }
                        //richTextBox1.SelectionBullet = false;
                    }
                    if (line != text.TextLines[text.TextLines.Count - 1])
                        cod.SelectedText = "\n";
                }
            }
            else
            {
                cod.SelectionAlignment = HorizontalAlignment.Center;
            }

            cod.DeselectAll();
            cod.SelectionStart = 0;// 1 ?

            int currentLine = 0;
            string returnValue = "";
            string fontName = "Arial";
            float fontSize = 8;
            Color fontColor = Color.White;
            FontStyle fontStyle = FontStyle.Regular;
            // Set default font and color values depending on first char
            if (cod.Text.ToCharArray().Length > 0)
            {
                cod.Select(0, 1);
                fontName = cod.SelectionFont.Name;
                fontSize = cod.SelectionFont.Size;
                fontColor = cod.SelectionColor;
                fontStyle = cod.SelectionFont.Style;
            }
            #region HTML Tags
            if (!useASS)
            {
                // Write code for first char
                returnValue += "<font";
                if (writeColors)
                {
                    int color = (fontColor.R << 16) | (fontColor.G << 8) | (fontColor.B);
                    returnValue += @" color=""#" + string.Format("{0:X}", color) + @"""";
                }
                if (writeFontSizes)
                {
                    returnValue += @" size=""" + fontSize.ToString() + @"""";
                }
                if (writeFonts)
                {
                    returnValue += @" face=""" + fontName + @"""";
                }
                returnValue += ">";
                if (writeFonts)
                {
                    if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                        returnValue += "<b>";
                    if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                        returnValue += "<i>";
                    if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                        returnValue += "<s>";
                    if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                        returnValue += "<u>";
                }
                for (int i = 0; i < cod.Text.ToCharArray().Length; i++)
                {
                    // select the char
                    cod.Select(i, 1);
                    string currentChar = cod.SelectedText;

                    if (currentChar != "\n")
                    {
                        if (fontStyle == cod.SelectionFont.Style && fontName == cod.SelectionFont.Name &&
                            fontSize == cod.SelectionFont.Size && fontColor == cod.SelectionColor)
                        {
                            // Nothing changed, just add the line ...
                            returnValue += currentChar;
                        }
                        else
                        {
                            //style ?
                            //end previous if it have to
                            if (((fontStyle & FontStyle.Bold) == FontStyle.Bold) &&
                                ((cod.SelectionFont.Style & FontStyle.Bold) != FontStyle.Bold))
                            {
                                fontStyle &= ~FontStyle.Bold;
                                returnValue += "</b>";
                            }
                            if (((fontStyle & FontStyle.Italic) == FontStyle.Italic) &&
                              ((cod.SelectionFont.Style & FontStyle.Italic) != FontStyle.Italic))
                            {
                                fontStyle &= ~FontStyle.Italic;
                                returnValue += "</i>";
                            }
                            if (((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout) &&
                             ((cod.SelectionFont.Style & FontStyle.Strikeout) != FontStyle.Strikeout))
                            {
                                fontStyle &= ~FontStyle.Strikeout;
                                returnValue += "</s>";
                            }
                            if (((fontStyle & FontStyle.Underline) == FontStyle.Underline) &&
                         ((cod.SelectionFont.Style & FontStyle.Underline) != FontStyle.Underline))
                            {
                                fontStyle &= ~FontStyle.Underline;
                                returnValue += "</u>";
                            }
                            //start new style if it have to
                            if (((fontStyle & FontStyle.Bold) != FontStyle.Bold) &&
                             ((cod.SelectionFont.Style & FontStyle.Bold) == FontStyle.Bold))
                            {
                                fontStyle |= FontStyle.Bold;
                                returnValue += "<b>";
                            }
                            if (((fontStyle & FontStyle.Italic) != FontStyle.Italic) &&
                              ((cod.SelectionFont.Style & FontStyle.Italic) == FontStyle.Italic))
                            {
                                fontStyle |= FontStyle.Italic;
                                returnValue += "<i>";
                            }
                            if (((fontStyle & FontStyle.Strikeout) != FontStyle.Strikeout) &&
                             ((cod.SelectionFont.Style & FontStyle.Strikeout) == FontStyle.Strikeout))
                            {
                                fontStyle |= FontStyle.Strikeout;
                                returnValue += "<s>";
                            }
                            if (((fontStyle & FontStyle.Underline) != FontStyle.Underline) &&
                         ((cod.SelectionFont.Style & FontStyle.Underline) == FontStyle.Underline))
                            {
                                fontStyle |= FontStyle.Underline;
                                returnValue += "<u>";
                            }
                            //font
                            if (fontName != cod.SelectionFont.Name ||
                              fontSize != cod.SelectionFont.Size || fontColor != cod.SelectionColor)
                            {
                                returnValue += "</font>";
                                //start new values
                                fontName = cod.SelectionFont.Name;
                                fontSize = cod.SelectionFont.Size;
                                fontColor = cod.SelectionColor;
                                //write current values for first time
                                returnValue += "<font";
                                if (writeColors)
                                {
                                    int color = (fontColor.R << 16) | (fontColor.G << 8) | (fontColor.B);
                                    returnValue += @" color=""#" + string.Format("{0:X}", color) + @"""";
                                }
                                if (writeFontSizes)
                                {
                                    returnValue += @" size=""" + fontSize.ToString() + @"""";
                                }
                                if (writeFonts)
                                {
                                    returnValue += @" face=""" + fontName + @"""";
                                }
                                returnValue += ">";
                            }
                            //add the char
                            returnValue += cod.SelectedText;
                        }
                    }
                    else
                    {
                        currentLine++;
                        returnValue += cod.Text[i];
                    }
                    //final ending code
                    if (i == cod.Text.ToCharArray().Length - 1)
                    {
                        if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                            returnValue += "</b>";
                        if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                            returnValue += "</i>";
                        if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                            returnValue += "</s>";
                        if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                            returnValue += "</u>";
                        returnValue += "</font>";
                    }
                    cod.SelectionStart++;
                }
            }
            #endregion
            #region ASS Tags
            else
            {
                if (writeAlignments)
                {
                    if (text.IsCustomPosition)
                    {
                        returnValue += @"{\pos(" + text.CustomPosition.X + "," + text.CustomPosition.Y + ")}";
                    }
                    else
                    {
                        switch (text.Position)
                        {
                            case SubtitlePosition.Down_Left:
                                {
                                    returnValue += @"{\a1}";
                                    break;
                                }
                            case SubtitlePosition.Down_Middle:
                                {
                                    // This is the default one
                                    returnValue += @"{\a2}";
                                    break;
                                }
                            case SubtitlePosition.Down_Right:
                                {
                                    returnValue += @"{\a3}";
                                    break;
                                }
                            case SubtitlePosition.Mid_Left:
                                {
                                    returnValue += @"{\a9}";
                                    break;
                                }
                            case SubtitlePosition.Mid_Middle:
                                {
                                    returnValue += @"{\a10}";
                                    break;
                                }
                            case SubtitlePosition.Mid_Right:
                                {
                                    returnValue += @"{\a11}";
                                    break;
                                }
                            case SubtitlePosition.Top_Left:
                                {
                                    returnValue += @"{\a5}";
                                    break;
                                }
                            case SubtitlePosition.Top_Middle:
                                {
                                    returnValue += @"{\a6}";
                                    break;
                                }
                            case SubtitlePosition.Top_right:
                                {
                                    returnValue += @"{\a7}";
                                    break;
                                }
                        }
                    }
                }
                // Write code for first char
                if (writeColors)
                {
                    int color = (fontColor.B << 16) | (fontColor.G << 8) | (fontColor.R);
                    returnValue += @"{\c&H" + string.Format("{0:X}", color) + "&}";
                }
                if (writeFontSizes)
                {
                    returnValue += @"{\fs" + fontSize.ToString() + "}";
                }
                if (writeFonts)
                {
                    returnValue += @"{\fn" + fontName + "}";

                    if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                        returnValue += @"{\b1}";
                    if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                        returnValue += @"{\i1}";
                    if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                        returnValue += @"{\s1}";
                    if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                        returnValue += @"{\u1}";
                }
                for (int i = 0; i < cod.Text.ToCharArray().Length; i++)
                {
                    // select the char
                    cod.Select(i, 1);
                    string currentChar = cod.SelectedText;

                    if (currentChar != "\n")
                    {
                        if (fontStyle == cod.SelectionFont.Style && fontName == cod.SelectionFont.Name &&
                            fontSize == cod.SelectionFont.Size && fontColor == cod.SelectionColor)
                        {
                            // Nothing changed, just add the line ...
                            returnValue += currentChar;
                        }
                        else
                        {
                            //style ?
                            //end previous if it have to
                            if (((fontStyle & FontStyle.Bold) == FontStyle.Bold) &&
                                ((cod.SelectionFont.Style & FontStyle.Bold) != FontStyle.Bold))
                            {
                                fontStyle &= ~FontStyle.Bold;
                                returnValue += @"{\b0}";
                            }
                            if (((fontStyle & FontStyle.Italic) == FontStyle.Italic) &&
                              ((cod.SelectionFont.Style & FontStyle.Italic) != FontStyle.Italic))
                            {
                                fontStyle &= ~FontStyle.Italic;
                                returnValue += @"{\i0}";
                            }
                            if (((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout) &&
                             ((cod.SelectionFont.Style & FontStyle.Strikeout) != FontStyle.Strikeout))
                            {
                                fontStyle &= ~FontStyle.Strikeout;
                                returnValue += @"{\s0}";
                            }
                            if (((fontStyle & FontStyle.Underline) == FontStyle.Underline) &&
                         ((cod.SelectionFont.Style & FontStyle.Underline) != FontStyle.Underline))
                            {
                                fontStyle &= ~FontStyle.Underline;
                                returnValue += @"{\u0}";
                            }
                            //start new style if it have to
                            if (((fontStyle & FontStyle.Bold) != FontStyle.Bold) &&
                             ((cod.SelectionFont.Style & FontStyle.Bold) == FontStyle.Bold))
                            {
                                fontStyle |= FontStyle.Bold;
                                returnValue += @"{\b1}";
                            }
                            if (((fontStyle & FontStyle.Italic) != FontStyle.Italic) &&
                              ((cod.SelectionFont.Style & FontStyle.Italic) == FontStyle.Italic))
                            {
                                fontStyle |= FontStyle.Italic;
                                returnValue += @"{\i1}";
                            }
                            if (((fontStyle & FontStyle.Strikeout) != FontStyle.Strikeout) &&
                             ((cod.SelectionFont.Style & FontStyle.Strikeout) == FontStyle.Strikeout))
                            {
                                fontStyle |= FontStyle.Strikeout;
                                returnValue += @"{\s1}";
                            }
                            if (((fontStyle & FontStyle.Underline) != FontStyle.Underline) &&
                         ((cod.SelectionFont.Style & FontStyle.Underline) == FontStyle.Underline))
                            {
                                fontStyle |= FontStyle.Underline;
                                returnValue += @"{\u1}";
                            }
                            //font
                            if (fontColor != cod.SelectionColor && writeColors)
                            {
                                fontColor = cod.SelectionColor;
                                int color = (fontColor.B << 16) | (fontColor.G << 8) | (fontColor.R);
                                returnValue += @"{\c&H" + string.Format("{0:X}", color) + "&}";
                            }
                            if (writeFontSizes && fontSize != cod.SelectionFont.Size)
                            {
                                fontSize = cod.SelectionFont.Size;
                                returnValue += @"{\fs" + fontSize.ToString() + "}";
                            }
                            if (fontName != cod.SelectionFont.Name && writeFonts)
                            {
                                fontName = cod.SelectionFont.Name;
                                returnValue += @"{\fn" + fontName + "}";
                            }
                            //add the char
                            returnValue += cod.SelectedText;
                        }
                    }
                    else
                    {
                        currentLine++;
                        returnValue += cod.Text[i];
                    }
                    //final ending code
                    if (i == cod.Text.ToCharArray().Length - 1)
                    {
                        if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                            returnValue += @"{\b0}";
                        if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                            returnValue += @"{\i0}";
                        if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                            returnValue += @"{\s0}";
                        if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                            returnValue += @"{\u0}";
                    }
                    cod.SelectionStart++;
                }
            }
            #endregion
            return returnValue;
        }

        /*SubStation Alpha*/
        public static SubtitleText SubtitleTextFromASSCode(string text, Font defaultFont, Color defaultColor)
        {
            cod.Text = "";
            cod.Clear();
            cod.SelectionColor = Color.White;
            cod.SelectionAlignment = HorizontalAlignment.Center;

            text = text.Replace(@"\N", "\n");

            FontStyle style = defaultFont.Style;
            List<Color> colors = new List<Color>();
            colors.Add(defaultColor);
            List<string> fontNames = new List<string>();
            fontNames.Add(defaultFont.Name);
            List<float> fontSizes = new List<float>();
            fontSizes.Add(defaultFont.Size);
            List<string> resetSeq = new List<string>();

            for (int j = 0; j < text.Length; j++)
            {
                if (text[j] == '{')
                {
                    j++;
                    string code = "";
                    while (text[j] != '}')
                    {
                        code += text[j];
                        j++;
                    }
                    // DECODE THE CODE ....
                    // Color
                    if (code.Contains(@"\c"))
                    {
                        string colorCode = "";
                        int index = code.IndexOf(@"\c");
                        index += 2;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            colorCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }

                        colorCode = colorCode.Replace("&", "");
                        colorCode = colorCode.Replace("H", "");

                        int col = int.Parse(colorCode, System.Globalization.NumberStyles.AllowHexSpecifier);
                        byte B = (byte)((col & 0xFF0000) >> 16);
                        byte G = (byte)((col & 0x00FF00) >> 8);
                        byte R = (byte)((col & 0x0000FF));
                        colors.Add(System.Drawing.Color.FromArgb(0xFF, R, G, B));
                    }
                    // Font name
                    if (code.Contains(@"\fn"))
                    {
                        string fontCode = "";
                        int index = code.IndexOf(@"\fn");
                        index += 3;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            fontCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }
                        fontNames.Add(fontCode);
                    }
                    // Font size
                    if (code.Contains(@"\fs"))
                    {
                        string fontCode = "";
                        int index = code.IndexOf(@"\fs");
                        index += 3;
                        while (code[index].ToString() != @"\" && index < code.Length)
                        {
                            fontCode += code[index];
                            index++;
                            if (index >= code.Length)
                                break;
                        }

                        fontCode = fontCode.Replace(@"\fs", "");
                        int s = 0;
                        if (int.TryParse(fontCode, out s))
                            fontSizes.Add(s);
                    }
                    // Font styles
                    if (code.Contains(@"\b1")) style |= FontStyle.Bold;
                    if (code.Contains(@"\b0")) style &= ~FontStyle.Bold;
                    if (code.Contains(@"\i1")) style |= FontStyle.Italic;
                    if (code.Contains(@"\i0")) style &= ~FontStyle.Italic;
                    if (code.Contains(@"\u1")) style |= FontStyle.Underline;
                    if (code.Contains(@"\u0")) style &= ~FontStyle.Underline;
                    if (code.Contains(@"\s1")) style |= FontStyle.Strikeout;
                    if (code.Contains(@"\s0")) style &= ~FontStyle.Strikeout;
                    // TODO: ASS alignement !
                    //if (code.Contains(@"\a1")) cod.SelectionAlignment = HorizontalAlignment.Left;
                    //if (code.Contains(@"\a2")) cod.SelectionAlignment = HorizontalAlignment.Center;
                    //if (code.Contains(@"\a3")) cod.SelectionAlignment = HorizontalAlignment.Right;
                }
                else//text char, add to line
                {
                    Color color = Color.White;
                    if (colors.Count > 0)
                        color = colors[colors.Count - 1];
                    string fontName = "Tahoma";
                    if (fontNames.Count > 0)
                        fontName = fontNames[fontNames.Count - 1];
                    float fontSize = 8;
                    if (fontSizes.Count > 0)
                        fontSize = fontSizes[fontSizes.Count - 1];

                    cod.SelectionFont = new Font(fontName, fontSize, style);
                    cod.SelectionColor = color;
                    cod.SelectedText += text[j];
                }
            }
            //return cod.Rtf;
            // RTF to SubText
            SubtitleText sub_text = new SubtitleText();

            foreach (string ln in cod.Lines)
            {
                sub_text.TextLines.Add(new SubtitleLine());
            }
            int currentLine = 0;
            int oldStart = cod.SelectionStart;
            string oldSelection = cod.SelectedText;
            cod.Visible = false;//disable so user won't see the strange things that would happen to the box lol
            cod.DeselectAll();
            cod.SelectionStart = 0;// 1 ?
            bool needToSetAlign = true;

            for (int i = 0; i < cod.Text.ToCharArray().Length; i++)
            {
                // select the char
                cod.Select(i, 1);
                string currentChar = cod.SelectedText;

                if (currentChar != "\n")
                {
                    if (needToSetAlign)
                    {
                        LineAlignement align = LineAlignement.Center;
                        switch (cod.SelectionAlignment)
                        {
                            case HorizontalAlignment.Left: align = LineAlignement.Left; break;
                            case HorizontalAlignment.Right: align = LineAlignement.Right; break;
                        }
                        sub_text.TextLines[currentLine].Alignement = align;
                        needToSetAlign = false;
                    }
                    sub_text.TextLines[currentLine].Chars.Add(new SubtitleChar(currentChar.ToCharArray()[0],
                        cod.SelectionFont, cod.SelectionColor));
                }
                else
                {
                    currentLine++;
                    needToSetAlign = true;
                }
                cod.SelectionStart++;
            }
            return sub_text;
        }
        public static string SubtitleTextToASSCode(SubtitleText text, bool writeColors, bool writeFonts,
            bool writeFontSizes, bool writeAlignments)
        {
            if (!writeColors && !writeFonts && !writeFontSizes)
                return text.ToString();
            // Subtitle text to RTF 
            cod.Text = "";
            cod.Clear();
            cod.SelectionStart = 0;
            if (text.TextLines.Count > 0)
            {
                foreach (SubtitleLine line in text.TextLines)
                {
                    foreach (SubtitleChar chr in line.Chars)
                    {
                        if (chr.Font != null)
                            cod.SelectionFont = chr.Font;
                        else
                            cod.SelectionFont = new Font("Tahoma", 8, FontStyle.Regular);

                        //richTextBox1.SelectionBullet = true;
                        if (chr.Color != null)
                            cod.SelectionColor = chr.Color;
                        else
                            cod.SelectionColor = Color.White;
                        cod.SelectedText = chr.TheChar.ToString();
                        switch (line.Alignement)
                        {
                            case LineAlignement.Center: cod.SelectionAlignment = HorizontalAlignment.Center; break;
                            case LineAlignement.Left: cod.SelectionAlignment = HorizontalAlignment.Left; break;
                            case LineAlignement.Right: cod.SelectionAlignment = HorizontalAlignment.Right; break;
                        }
                        //richTextBox1.SelectionBullet = false;
                    }
                    if (line != text.TextLines[text.TextLines.Count - 1])
                        cod.SelectedText = "\n";
                }
            }
            else
            {
                cod.SelectionAlignment = HorizontalAlignment.Center;
            }

            cod.DeselectAll();
            cod.SelectionStart = 0;// 1 ?

            int currentLine = 0;
            string returnValue = "";
            string fontName = "Arial";
            float fontSize = 8;
            Color fontColor = Color.White;
            FontStyle fontStyle = FontStyle.Regular;
            // Set default font and color values depending on first char
            if (cod.Text.ToCharArray().Length > 0)
            {
                cod.Select(0, 1);
                fontName = cod.SelectionFont.Name;
                fontSize = cod.SelectionFont.Size;
                fontColor = cod.SelectionColor;
                fontStyle = cod.SelectionFont.Style;
            }
            if (writeAlignments)
            {
                if (text.IsCustomPosition)
                {
                    returnValue += @"{\pos(" + text.CustomPosition.X + "," + text.CustomPosition.Y + ")}";
                }
                else
                {
                    switch (text.Position)
                    {
                        case SubtitlePosition.Down_Left:
                            {
                                returnValue += @"{\a1}";
                                break;
                            }
                        case SubtitlePosition.Down_Middle:
                            {
                                // This is the default one
                                returnValue += @"{\a2}";
                                break;
                            }
                        case SubtitlePosition.Down_Right:
                            {
                                returnValue += @"{\a3}";
                                break;
                            }
                        case SubtitlePosition.Mid_Left:
                            {
                                returnValue += @"{\a9}";
                                break;
                            }
                        case SubtitlePosition.Mid_Middle:
                            {
                                returnValue += @"{\a10}";
                                break;
                            }
                        case SubtitlePosition.Mid_Right:
                            {
                                returnValue += @"{\a11}";
                                break;
                            }
                        case SubtitlePosition.Top_Left:
                            {
                                returnValue += @"{\a5}";
                                break;
                            }
                        case SubtitlePosition.Top_Middle:
                            {
                                returnValue += @"{\a6}";
                                break;
                            }
                        case SubtitlePosition.Top_right:
                            {
                                returnValue += @"{\a7}";
                                break;
                            }
                    }
                }
            }
            // Write code for first char
            if (writeColors)
            {
                int color = (fontColor.B << 16) | (fontColor.G << 8) | (fontColor.R);
                returnValue += @"{\c&H" + string.Format("{0:X}", color) + "&}";
            }
            if (writeFontSizes)
            {
                returnValue += @"{\fs" + fontSize.ToString() + "}";
            }
            if (writeFonts)
            {
                returnValue += @"{\fn" + fontName + "}";

                if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                    returnValue += @"{\b1}";
                if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                    returnValue += @"{\i1}";
                if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                    returnValue += @"{\s1}";
                if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                    returnValue += @"{\u1}";
            }
            for (int i = 0; i < cod.Text.ToCharArray().Length; i++)
            {
                // select the char
                cod.Select(i, 1);
                string currentChar = cod.SelectedText;

                if (currentChar != "\n")
                {
                    if (fontStyle == cod.SelectionFont.Style && fontName == cod.SelectionFont.Name &&
                        fontSize == cod.SelectionFont.Size && fontColor == cod.SelectionColor)
                    {
                        // Nothing changed, just add the line ...
                        returnValue += currentChar;
                    }
                    else
                    {
                        //style ?
                        //end previous if it have to
                        if (((fontStyle & FontStyle.Bold) == FontStyle.Bold) &&
                            ((cod.SelectionFont.Style & FontStyle.Bold) != FontStyle.Bold))
                        {
                            fontStyle &= ~FontStyle.Bold;
                            returnValue += @"{\b0}";
                        }
                        if (((fontStyle & FontStyle.Italic) == FontStyle.Italic) &&
                          ((cod.SelectionFont.Style & FontStyle.Italic) != FontStyle.Italic))
                        {
                            fontStyle &= ~FontStyle.Italic;
                            returnValue += @"{\i0}";
                        }
                        if (((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout) &&
                         ((cod.SelectionFont.Style & FontStyle.Strikeout) != FontStyle.Strikeout))
                        {
                            fontStyle &= ~FontStyle.Strikeout;
                            returnValue += @"{\s0}";
                        }
                        if (((fontStyle & FontStyle.Underline) == FontStyle.Underline) &&
                     ((cod.SelectionFont.Style & FontStyle.Underline) != FontStyle.Underline))
                        {
                            fontStyle &= ~FontStyle.Underline;
                            returnValue += @"{\u0}";
                        }
                        //start new style if it have to
                        if (((fontStyle & FontStyle.Bold) != FontStyle.Bold) &&
                         ((cod.SelectionFont.Style & FontStyle.Bold) == FontStyle.Bold))
                        {
                            fontStyle |= FontStyle.Bold;
                            returnValue += @"{\b1}";
                        }
                        if (((fontStyle & FontStyle.Italic) != FontStyle.Italic) &&
                          ((cod.SelectionFont.Style & FontStyle.Italic) == FontStyle.Italic))
                        {
                            fontStyle |= FontStyle.Italic;
                            returnValue += @"{\i1}";
                        }
                        if (((fontStyle & FontStyle.Strikeout) != FontStyle.Strikeout) &&
                         ((cod.SelectionFont.Style & FontStyle.Strikeout) == FontStyle.Strikeout))
                        {
                            fontStyle |= FontStyle.Strikeout;
                            returnValue += @"{\s1}";
                        }
                        if (((fontStyle & FontStyle.Underline) != FontStyle.Underline) &&
                     ((cod.SelectionFont.Style & FontStyle.Underline) == FontStyle.Underline))
                        {
                            fontStyle |= FontStyle.Underline;
                            returnValue += @"{\u1}";
                        }
                        //font
                        if (fontColor != cod.SelectionColor && writeColors)
                        {
                            fontColor = cod.SelectionColor;
                            int color = (fontColor.B << 16) | (fontColor.G << 8) | (fontColor.R);
                            returnValue += @"{\c&H" + string.Format("{0:X}", color) + "&}";
                        }
                        if (writeFontSizes && fontSize != cod.SelectionFont.Size)
                        {
                            fontSize = cod.SelectionFont.Size;
                            returnValue += @"{\fs" + fontSize.ToString() + "}";
                        }
                        if (fontName != cod.SelectionFont.Name && writeFonts)
                        {
                            fontName = cod.SelectionFont.Name;
                            returnValue += @"{\fn" + fontName + "}";
                        }
                        //add the char
                        returnValue += cod.SelectedText;
                    }
                }
                else
                {
                    currentLine++;
                    returnValue += cod.Text[i];
                }
                //final ending code
                if (i == cod.Text.ToCharArray().Length - 1)
                {
                    if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
                        returnValue += @"{\b0}";
                    if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
                        returnValue += @"{\i0}";
                    if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
                        returnValue += @"{\s0}";
                    if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
                        returnValue += @"{\u0}";
                }
                cod.SelectionStart++;
            }
            return returnValue;
        }
    }
}
