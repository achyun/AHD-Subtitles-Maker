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
    public class Csv : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Csv (*.csv)"; }
        }

        public override string Description
        {
            get { return "Csv\n\n" + @"This format has this view:" + "\n" + @"1;2440;3400;""text1""" + "\n" + @"2;4080;5440;""text2""" + "\n....."; }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".csv" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 1)
            {
                if (Lines[0] == @"Number;Start time in milliseconds;End time in milliseconds;""Text""")
                    return true;
            }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            string[] Lines = File.ReadAllLines(FilePath, encoding);
            this.SubtitleTrack = new SubtitlesTrack();
            for (int i = 1; i < Lines.Length; i++)
            {
                try
                {
                    string[] TextLines = Lines[i].Split(new char[] { ';' });
                    if (TextLines.Length == 4)
                    {
                        Subtitle sub = new Subtitle();
                        sub.StartTime = double.Parse(TextLines[1]) / 1000;
                        sub.EndTime = double.Parse(TextLines[2]) / 1000;
                        string[] txts = TextLines[3].Split(new char[] { '"' });
                        sub.Text = SubtitleText.FromString(txts[1]);
                        this.SubtitleTrack.Subtitles.Add(sub);
                    }
                    else
                    {
                        string[] txts = Lines[i].Split(new char[] { '"' });
                        this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(SubtitleLine.FromString(txts[0]));
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            this.FilePath = filePath;
            List<string> lines = new List<string>();
            lines.Add(@"Number;Start time in milliseconds;End time in milliseconds;""Text""");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count == 1)
                {
                    lines.Add((i + 1).ToString() + ";" + (int)(this.SubtitleTrack.Subtitles[i].StartTime * 1000) + ";" + (int)(this.SubtitleTrack.Subtitles[i].EndTime * 1000) + ";" + @"""" + this.SubtitleTrack.Subtitles[i].Text + @"""");
                }
                else
                {
                    string line = (i + 1).ToString() + ";" + (int)(this.SubtitleTrack.Subtitles[i].StartTime * 1000) + ";" + (int)(this.SubtitleTrack.Subtitles[i].EndTime * 1000) + ";" + @"""" + this.SubtitleTrack.Subtitles[i].Text.TextLines[0];
                    lines.Add(line);
                    for (int j = 1; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                    {
                        if (j != this.SubtitleTrack.Subtitles[i].Text.TextLines.Count - 1)
                            lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString());
                        else
                            lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j] + @"""");
                    }
                }

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }
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
