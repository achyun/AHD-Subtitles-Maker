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
    public class Captions32 : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Captions 32 (*.txt)"; }
        }
        public override string Description
        {
            get { return "Captions 32\n\nThis format type has this view:\n1 00:00:01:00(hou:min:sec:Frame) , 00:00:02:00 , Text1\n2 00:00:04:00 , 00:00:06:00 , Text2\n.......\n\nSupports only 2 lines, the lines after will be removed. Also a line can hold only 33 letters, the rest of the letters (if more than 33) will be removed."; }
        }
        public override string[] Extensions
        {
            get { string[] exs = { ".txt" }; return exs; }
        }
        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            bool Yes = false;
            if (Lines.Length > 0)
            {
                if (Lines[0].Length >= 14)
                {
                    if (Lines[0].Substring(11, 3) == " , ")
                    { Yes = true; }
                }
            }
            return Yes;
        }
        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            FilePath = filePath;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack("Imported");

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[0], MillisecondLength.N2);
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                    TextLines = TextLines[2].Split(new char[] { '|' });
                    string[] texx = TextLines[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string TEXT = "";
                    for (int o = 0; o < texx.Length; o++)
                    {
                        if (o != texx.Length - 1)
                            TEXT += texx[o] + " ";
                        else
                            TEXT += texx[o];
                    }
                    if (TextLines[1] != "                                 ")
                    {
                        TEXT += "\n";
                        texx = TextLines[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        for (int l = 0; l < texx.Length; l++)
                        {
                            if (l != texx.Length - 1)
                                TEXT += texx[l] + " ";
                            else
                                TEXT += texx[l];
                        }
                    }
                    Sub.Text = SubtitleText.FromString(TEXT);
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            this.FilePath = filePath;
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string Current = TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ":", MillisecondLength.N2) +
                    " , " + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2) + " , ";
                string text = this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString();
                if (text.Length >= 33)
                { text = text.Substring(0, 33); }
                else
                {
                    int len = 33 - text.Length;
                    for (int j = 0; j < len; j++)
                    { text += " "; }
                }
                Current += text + "|";
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                {
                    string text2 = this.SubtitleTrack.Subtitles[i].Text.TextLines[1].ToString();
                    if (text2.Length >= 33)
                    { text2 = text2.Substring(0, 33); }
                    else
                    {
                        int len = 33 - text2.Length;
                        for (int j = 0; j < len; j++)
                        { text2 += " "; }
                    }
                    Current += text2;
                }
                else
                {
                    for (int j = 0; j < 33; j++)
                    { Current += " "; }
                }
                Lines.Add(Current);
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
        public override event EventHandler<ASMP.ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
