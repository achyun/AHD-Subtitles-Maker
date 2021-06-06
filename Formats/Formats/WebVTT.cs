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
using System.IO;
using AHD.SM.ASMP;

namespace AHD.SM.Formats
{
    public class WebVTT : SubtitlesFormat
    {
        public override string Name
        {
            get { return "WebVTT (*.vtt)"; }
        }

        public override string Description
        {
            get { return "The WebVTT format (Web Video Text Tracks) is a format intended for marking up external text track resources.\n\nPlease note that this format should be saved in UTF-8 encoding."; }
        }

        public override string[] Extensions
        {
            get { return new string[] { ".vtt" }; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);

            if (Lines.Length > 0)
            {
                if (Lines[0] == "WEBVTT")
                {
                    return true;
                }
            }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 1; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i].Length == 0)
                        continue;
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new string[] { " " }, StringSplitOptions.None);

                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[0]);
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[2]);
                    //the codes
                    LineAlignement al = LineAlignement.Center;
                    for (int k = 3; k < TextLines.Length; k++)
                    {
                        if (TextLines[k].ToLower().Substring(0, 1) == "a")
                        {
                            switch (TextLines[k].ToLower().Substring(2, TextLines[k].Length - 2))
                            {
                                case "start": al = LineAlignement.Left; break;
                                case "end": al = LineAlignement.Right; break;
                                case "middle": al = LineAlignement.Center; break;
                            }
                            break;
                        }
                    }
                    i++;
                    while (Lines[i] != "")
                    {
                        string text = Lines[i];
                        SubtitleLine lin = new SubtitleLine();
                        lin.Alignement = al;
                        System.Drawing.Font currentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular);
                        char[] chars = text.ToCharArray();
                        for (int c = 0; c < chars.Length; c++)
                        {
                            if (chars[c] == '<')
                            {
                                c++;
                                switch (chars[c])
                                {
                                    case 'b': currentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Bold); break;
                                    case 'i': currentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Italic); break;
                                    case 'u': currentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Underline); break;
                                    case '/': currentFont = new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular); break;
                                }
                                while (chars[c] != '>')
                                    c++;
                            }
                            else
                            {
                                lin.Chars.Add(new SubtitleChar(chars[c], currentFont, System.Drawing.Color.White));
                            }
                        }
                        Sub.Text.TextLines.Add(lin);
                        i++;
                    }
                  
                    this.SubtitleTrack.Subtitles.Add(Sub);
                }
                catch { }
                int x = (100 * i) / Lines.Length;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Loading file ...."));
            }
            if (Progress != null)
                Progress(this, new ProgressArgs(100, "Load Completed."));
            if (LoadFinished != null)
                LoadFinished(this, new EventArgs());
        }

        public override void Save(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            Lines.Add("WEBVTT");
            Lines.Add("");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add((i + 1).ToString());

                string lineCode = TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime) + " --> " +
                   TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime);
                //alignement: we can take the alignement of 1st line only
                lineCode += " A:";
                switch (this.SubtitleTrack.Subtitles[i].Text.TextLines[0].Alignement)
                {
                    case LineAlignement.Center: lineCode += "middle"; break;
                    case LineAlignement.Left: lineCode += "start"; break;
                    case LineAlignement.Right: lineCode += "end"; break;
                }
                Lines.Add(lineCode);
                for (int j = 0; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                {
                    //string lineOfText = this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString();
                    //lineOfText = lineOfText.Replace("&", "&amp;");
                    //lineOfText = lineOfText.Replace("<", "&lt;");
                    //lineOfText = lineOfText.Replace(">", "&gt;");
                    string lineOfText = "";
                    string currentStyle = "";
                    bool shouldEnd = false;
                    foreach (SubtitleChar chr in this.SubtitleTrack.Subtitles[i].Text.TextLines[j].Chars)
                    {
                        string st = "";
                        switch (chr.Font.Style)
                        {
                            case System.Drawing.FontStyle.Bold: st = "b"; break;
                            case System.Drawing.FontStyle.Italic: st = "i"; break;
                            case System.Drawing.FontStyle.Underline: st = "u"; break;
                        }
                        //end last code
                        if (currentStyle != st && shouldEnd)
                        {
                            lineOfText += "</" + currentStyle + ">";
                            shouldEnd = false;
                        }
                        //set new code if we have to
                        if (st != "" && currentStyle != st)
                        {
                            lineOfText += "<" + st + ">";
                            currentStyle = st;
                            shouldEnd = true;
                        }
                        //the char
                        string theChar = chr.TheChar.ToString();
                        if (theChar == "&")
                            theChar = "&amp;";
                        if (theChar == "<")
                            theChar = "&lt;";
                        if (theChar == ">")
                            theChar = "&gt;";
                        //replacement
                        lineOfText += theChar;
                    }
                    //should end unfinished code ?
                    if (currentStyle != "")
                        lineOfText += "</" + currentStyle + ">";
                    Lines.Add(lineOfText);
                }

                Lines.Add("");

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }

            File.WriteAllLines(FilePath, Lines.ToArray(), encoding);
            if (Progress != null)
                Progress(this, new ProgressArgs(100, "Save Completed."));
            if (SaveFinished != null)
                SaveFinished(this, new EventArgs());
        }

        public override event EventHandler<ProgressArgs> Progress;

        public override event EventHandler LoadStarted;

        public override event EventHandler LoadFinished;

        public override event EventHandler SaveStarted;

        public override event EventHandler SaveFinished;
    }
}
