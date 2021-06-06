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
    public class Panimator : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Panimator (*.pan)"; }
        }

        public override string Description
        {
            get
            {
                return "Panimator/n/nThis format type has this view:" +
                    "\n/d 05 95 " +
                    "\nText1\n" +
                    "/d 08 13\n" +
                    "/c\n" +
                    "/d 08 22\n" +
                    "Text2\n" +
                    "/d 10 36 \n........";
            }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".pan" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 2) == "/d")
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
                if (Lines[i] != "")
                {
                    try
                    {
                        Subtitle Sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new char[] { ' ' });
                        int SS = Convert.ToInt32(TextLines[1]);
                        double mm = Convert.ToDouble(TextLines[2]);
                        mm /= 100;
                        Sub.StartTime = SS + mm;
                        i++;
                        string text = Lines[i];
                        i++;
                        while (Lines[i].Substring(0, 2) != "/d")
                        {
                            text += "\n" + Lines[i];
                            i++;
                        }
                        Sub.Text = SubtitleText.FromString(text);
                        TextLines = Lines[i].Split(new char[] { ' ' });
                        SS = Convert.ToInt32(TextLines[1]);
                        mm = Convert.ToDouble(TextLines[2]);
                        mm /= 100;
                        Sub.EndTime = SS + mm;
                        this.SubtitleTrack.Subtitles.Add(Sub);
                    }
                    catch { }
                }
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
                Lines.Add("/d " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F0") + " " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F3").Substring(this.SubtitleTrack.Subtitles[i].StartTime.ToString("F3").Length - 3, 2) + " ");
                for (int j = 0; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                {
                    Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString());
                }
                Lines.Add("/d " + this.SubtitleTrack.Subtitles[i].EndTime.ToString("F0") + " " + this.SubtitleTrack.Subtitles[i].EndTime.ToString("F3").Substring(this.SubtitleTrack.Subtitles[i].EndTime.ToString("F3").Length - 3, 2) + " ");
                Lines.Add("/c");

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
