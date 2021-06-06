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
    public class DKS : SubtitlesFormat
    {
        public override string Name
        {
            get { return "DKS (*.dks)"; }
        }

        public override string Description
        {
            get { return "DKS\n\nUses timespan without milliseconds. This format type has this view:\n[00:01:00]([hou:min:sec])Text1\n[00:03:00]Text2\n.......\n\nNote: Please note that the time MilliSecond Will Be adducting and this may cause delay in some subtitles!!\nFor Example: 1.324 will be 1\n2.677 will be 2....."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".dks" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            bool Yes = false;
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 1) == "[" & Lines[0].Substring(9, 1) == "]")
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

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    double start = TimeFormatConvertor.From_TimeSpan(Lines[i].Substring(1, 8));
                    string txt = Lines[i].Substring(10, Lines[i].Length - 10).Replace("[br]", "\n");
                    i++;
                    double end = TimeFormatConvertor.From_TimeSpan(Lines[i].Substring(1, 8));
                    Sub.StartTime = start;
                    Sub.EndTime = end;
                    Sub.Text = SubtitleText.FromString(txt);
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
                string curr = "[" + TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].StartTime) + "]" + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "[br]");
                Lines.Add(curr);
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].EndTime) + "]");

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
