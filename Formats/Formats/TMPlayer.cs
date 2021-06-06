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
    public class TMPlayer : SubtitlesFormat
    {
        string[] _Extensions = { ".txt", ".sub" };
        public override string Name
        {
            get { return "TM Player (*.txt, *.sub)"; }
        }

        public override string Description
        {
            get { return "TM Player\n\nThis format type has this view:\n00:00:01:Text1\n00:00:04:Text2\n.......\nSupports only 2 subtitle text lines."; }
        }

        public override string[] Extensions
        {
            get { return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 2) == "00")
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
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new string[] { ",1=" }, StringSplitOptions.None);
                    sub.StartTime = TimeFormatConvertor.From_TimeSpan(TextLines[0]);
                    sub.Text = SubtitleText.FromString(TextLines[1]);
                    i++;
                    TextLines = Lines[i].Split(new string[] { ",2=" }, StringSplitOptions.None);
                    if (TextLines.Length > 1)
                        sub.Text.TextLines.Add(SubtitleLine.FromString(TextLines[1]));
                    i++;
                    TextLines = Lines[i].Split(new string[] { ",1=" }, StringSplitOptions.None);
                    sub.EndTime = TimeFormatConvertor.From_TimeSpan(TextLines[0]);
                    this.SubtitleTrack.Subtitles.Add(sub);
                    i++;
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
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add(TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].StartTime) + ",1=" +
                    this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString());
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                {
                    Lines.Add(TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].StartTime) + ",2=" +
                    this.SubtitleTrack.Subtitles[i].Text.TextLines[1].ToString());
                }
                else { Lines.Add(TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].StartTime) + ",2="); }

                Lines.Add(TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].EndTime) + ",1=");
                Lines.Add(TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].EndTime) + ",2=");

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
