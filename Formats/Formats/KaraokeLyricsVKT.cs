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
    public class KaraokeLyricsVKT : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Karaoke Lyrics VKT (*.vkt)"; }
        }

        public override string Description
        {
            get { return "Karaoke Lyrics VKT\n\nDoesn't support multi lines, so it will make all text lines as one line."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".vkt" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0] == "# <HEAD>")
                    {
                        return true;
                    }
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

            for (int i = 7; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '{', ' ' }, 3);
                    Sub.StartTime = ConvertTime(TextLines[1]);
                    Sub.Text = SubtitleText.FromString(TextLines[2].Substring(0, TextLines[2].Length - 1));
                    i++;
                    TextLines = Lines[i].Split(new char[] { '{', ' ', '{' });
                    Sub.EndTime = ConvertTime(TextLines[1]);
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
            Lines.Add("# <HEAD>");
            Lines.Add("# FRAME RATE=MP3");
            Lines.Add("# CREATOR=Project author");
            Lines.Add(@"# VIDEO SOURCE=C:\Untitled.avi");
            Lines.Add("# DATE=" + DateTime.Now);
            Lines.Add("# </HEAD>");
            Lines.Add("#");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("{" + GetTime(this.SubtitleTrack.Subtitles[i].StartTime) + " " +
                    this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " ") + "}");
                Lines.Add("{" + GetTime(this.SubtitleTrack.Subtitles[i].EndTime) + " }");
                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }
            Lines.Add("");
            Lines.Add("#");
            Lines.Add("# THE END.");
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

        string GetTime(double TimeInMilliSecond)
        {
            string MILI = TimeInMilliSecond.ToString("F3").Substring(TimeInMilliSecond.ToString("F3").Length - 3, 2);
            string SEC = TimeInMilliSecond.ToString("F0");
            string FinalTime = SEC + MILI;
            string ToReturn = "";
            for (int i = 0; i < 5 - FinalTime.Length; i++)
            {
                ToReturn += "0";
            }
            return ToReturn + FinalTime.ToString();
        }
        double ConvertTime(string TimeInHMS)
        {
            double Tim = 0;
            try
            {
                int S = Convert.ToInt32(TimeInHMS.Substring(0, TimeInHMS.Length - 2));
                double Mili = Convert.ToInt32(TimeInHMS.Substring(TimeInHMS.Length - 2, 2));
                Mili /= 100;
                Tim = S + Mili;
            }
            catch { }
            return Tim;
        }
    }
}
