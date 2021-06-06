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
    public class ZeroG : SubtitlesFormat
    {
        string[] _Extensions = { ".zeg" };
        public override string Name
        {
            get { return "ZeroG (*.zeg)"; }
        }

        public override string Description
        {
            get { return "ZeroG\n\nThis format type has this view:\nE 1 0:00:09.06 0:00:12.13 Default NTP text1\nE 1 0:00:12.20 0:00:14.22 Default NTP text2\n........."; }
        }

        public override string[] Extensions
        {
            get { return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            bool Yes = false;
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, "% Zero G 1.0".Length) == "% Zero G 1.0")
                    { Yes = true; }
                }
                catch { }
            }
            return Yes;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 2; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { ' ' }, 5);
                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[2], MillisecondLength.N2);
                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[3], MillisecondLength.N2);
                    TextLines = TextLines[4].Split(new string[] { "Default NTP " }, StringSplitOptions.None);
                    TextLines = TextLines[1].Split(new string[] { "\\n" }, StringSplitOptions.None);
                    sub.Text = SubtitleText.FromString(TextLines[0]);
                    for (int j = 1; j < TextLines.Length; j++)
                    { sub.Text.TextLines.Add(SubtitleLine.FromString(TextLines[j])); }
                    this.SubtitleTrack.Subtitles.Add(sub);
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
            Lines.Add("% Zero G 1.0");
            Lines.Add("");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string curr = "E 1 " + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ".", MillisecondLength.N2).Substring(1, 10) + " " +
          TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ".", MillisecondLength.N2).Substring(1, 10) + " Default NTP " +
                  this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "\\n");
                Lines.Add(curr);

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
