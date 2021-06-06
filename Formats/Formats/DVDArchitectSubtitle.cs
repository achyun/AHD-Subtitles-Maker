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
    public class DVDArchitectSubtitle : SubtitlesFormat
    {
        public override string Name
        {
            get { return "DVD Architect Subtitle (*.sub)"; }
        }

        public override string Description
        {
            get { return "DVD Architect Subtitle\n\nThis format type has this view:\n0000	00:00:01:00(hou:min:sec:Frame)	00:00:02:00	Text1\n......."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".sub" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    Convert.ToInt32(Lines[0].Substring(0, 4));
                    return true;
                }
                catch { }
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

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '\t', ' ' }, 4);
                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[2], MillisecondLength.N2);
                    string text = TextLines[3];
                    i++;
                    while (Lines[i] != "")
                    {
                        text += "\n" + Lines[i];
                        i++;
                    }
                    Sub.Text = SubtitleText.FromString(text);
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
            this.FilePath = filePath;
            List<string> Lines = new List<string>();
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string NO = "";
                for (int N = 0; N < 4 - i.ToString().Length; N++)
                {
                    NO += "0";
                }
                NO += i.ToString();
                string CurrentLine = NO + "\t" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ":", MillisecondLength.N2) + "\t" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2) + "\t" + this.SubtitleTrack.Subtitles[i].Text.TextLines[0];
                Lines.Add(CurrentLine);
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                {
                    for (int j = 1; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                    {
                        Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString());
                    }
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
