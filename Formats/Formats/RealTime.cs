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
    public class RealTime : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Real Time (*.rt)"; }
        }

        public override string Description
        {
            get
            {
                string Des = "Real Time\n\nThis format has this view:\n" + @"<Time begin=""0:00:02.4"" end=""0:00:03.4"" /><clear/>text 1" + "\n" + @"<Time begin=""0:00:04.0"" end=""0:00:05.4"" /><clear/>text 2" + "\n......";
                return Des;
            }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".rt" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 1)
            {
                if (Lines[0] == "<Window")
                    return true;
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
                    if (Lines[i].Substring(0, "<Time".Length) == "<Time")
                    {
                        Subtitle sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new char[] { '"' });
                        sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N1);
                        sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[3], MillisecondLength.N1);
                        sub.Text = SubtitleText.FromString(TextLines[4].Substring(" /><clear/>".Length, TextLines[4].Length - " /><clear/>".Length));

                        this.SubtitleTrack.Subtitles.Add(sub);
                    }
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

            List<string> lines = new List<string>();

            lines.Add("<Window");
            lines.Add(@"  Width    = ""640""");
            lines.Add(@"  Height   = ""480""");
            lines.Add(@"  WordWrap = ""true""");
            lines.Add(@"  Loop     = ""true""");
            lines.Add(@"  bgcolor  = ""black""");
            lines.Add(">");
            lines.Add("<Font");
            lines.Add(@"  Color = ""white""");
            lines.Add(@"  Face  = ""Arial""");
            lines.Add(@"  Size  = ""+2""");
            lines.Add(">");
            lines.Add("<center>");
            lines.Add("<b>");
            lines.Add("");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                lines.Add("<Time begin=" + @"""" +
               TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ".", MillisecondLength.N1).Substring(1, 9)
               + @"""" + " end=" + @"""" +
                    TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ".", MillisecondLength.N1).Substring(1, 9)
                    + @"""" + " /><clear/>" + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " "));

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }
            lines.Add("</b>");
            lines.Add("</center>");

            File.WriteAllLines(FilePath, lines.ToArray(), encoding);

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
