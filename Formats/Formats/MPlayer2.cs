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
    public class MPlayer2 : SubtitlesFormat
    {
        public override string Name
        {
            get { return "MPlayer2 (*.mpl)"; }
        }

        public override string Description
        {
            get { return "MPlayer2\n\nThis format type has this view:\n[132(SubStartTime)][143(SubEndTime)]Text1\n[152][163]Text2\n......."; }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".mpl" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 1) == "[")
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
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '[', ']' });
                    Sub.StartTime = ConvertTime(TextLines[1]);
                    Sub.EndTime = ConvertTime(TextLines[3]);
                    Sub.Text = SubtitleText.FromString(TextLines[4].Replace("|", "\n"));

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
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string CurrentLine = "[" + GetTime(this.SubtitleTrack.Subtitles[i].StartTime) + "][" +
                    GetTime(this.SubtitleTrack.Subtitles[i].EndTime) + "]" + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "|");
                Lines.Add(CurrentLine);

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

        double ConvertTime(string TimeInHMS)
        {
            int ss = Convert.ToInt32(TimeInHMS.Substring(0, TimeInHMS.Length - 1));
            double Mili = Convert.ToDouble(TimeInHMS.Substring(TimeInHMS.Length - 1, 1));
            Mili /= 10;
            return ss + Mili;
        }
        string GetTime(double Time)
        {
            return (Time * 10).ToString("N0");
        }
    }
}
