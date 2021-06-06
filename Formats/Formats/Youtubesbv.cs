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
    public class Youtubesbv : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Youtube sbv (*.sbv)"; }
        }

        public override string Description
        {
            get { return "Youtube sbv\n\nThis format has this view:\n0:01:50.840,0:01:53.160\ntext1\n0:01:53.240,0:01:55.400\ntext2\n....."; }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".sbv" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 2)
            {
                try
                {
                    if (Lines[0].Split(new char[] { ',' }).Length == 2)
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
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    sub.StartTime = ConvertTime(TextLines[0]);
                    sub.EndTime = ConvertTime(TextLines[1]);
                    i++;
                    while (Lines[i] != "")
                    {
                        sub.Text.TextLines.Add(SubtitleLine.FromString(Lines[i]));
                        i++;
                    }
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
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                //add timecodes
                string lin = GetTime(this.SubtitleTrack.Subtitles[i].StartTime) + "," + 
                    GetTime(this.SubtitleTrack.Subtitles[i].EndTime);
                Lines.Add(lin);
                //add text lines
                for (int j = 0; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                {
                    Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString());
                }
                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                //add sperater
                Lines.Add("");
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

        public string GetTime(double TimeInMilliSecond)
        {
            TimeSpan FinalTime = TimeSpan.FromSeconds(TimeInMilliSecond);
            string TimeCode = TimeInMilliSecond.ToString("F3").Substring(TimeInMilliSecond.ToString("F3").Length - 3, 3);
            string returnvalue = FinalTime.ToString().Substring(0, 8) + "." + TimeCode;
            if (((int)FinalTime.TotalHours).ToString().Length == 1)
                return returnvalue.Substring(1, returnvalue.Length - 1);
            else
                return returnvalue;
        }
        double ConvertTime(string TimeInHMS)
        {
            string[] tt = TimeInHMS.Split(new char[] { ':', '.' }, StringSplitOptions.RemoveEmptyEntries);
            int HH = Convert.ToInt32(tt[0]);
            int MM = Convert.ToInt32(tt[1]);
            int SS = Convert.ToInt32(tt[2]);
            double Milli = Convert.ToInt32(tt[3]);
            Milli /= 1000;
            return ((HH * 3600) + (MM * 60) + SS + Milli);
        }
    }
}
