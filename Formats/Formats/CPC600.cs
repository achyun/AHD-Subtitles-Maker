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
    public class CPC600 : SubtitlesFormat
    {
        public override string Name
        {
            get { return "CPC-600 (*.txt)"; }
        }

        public override string Description
        {
            get { return "CPC-600\n\nThis format type has this view:\n00:00:09:06_0NEN_text 1\n00:00:12:13_0NEN_[endtime]\n\n00:00:12:20_0NEN_text 2\n\n........."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".txt" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, "~CPCC6.38~;".Length) == "~CPCC6.38~;")
                    { return true; }
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
            string[] Lines = File.ReadAllLines(FilePath, encoding);
            this.SubtitleTrack = new SubtitlesTrack("Imported");

            for (int i = 1; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(0, 11), MillisecondLength.N2);

                    Sub.Text = SubtitleText.FromString(Lines[i].Substring(17, Lines[i].Length - 17).Replace(@"\", "\n"));

                    i++;
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(0, 11), MillisecondLength.N2);
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
            Lines.Add("~CPCC6.38~;UpperLower;PopOn;01;");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string Current = TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime,":", MillisecondLength.N2) + "_0NEN_" + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n",@"\");
                Lines.Add(Current);
                Lines.Add(TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2) + "_0NEN_");

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
