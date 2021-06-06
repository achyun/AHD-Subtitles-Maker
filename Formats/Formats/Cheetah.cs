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
    public class Cheetah : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Cheetah (*.asc)"; }
        }

        public override string Description
        {
            get { return "Cheetah\n\nDoesn't support multi lines, so it will take the first line only from each subtitle text.\n"; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".asc" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length >= 2)
            {
                try
                {
                    if (Lines[0].Substring(0, 1) == "*")
                    { return true; }
                }
                catch { }
            }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            string[] Lines = File.ReadAllLines(FilePath, encoding);
            this.SubtitleTrack = new SubtitlesTrack("Imported");

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i].Substring(0, "** Caption Number".Length) == "** Caption Number")
                    {
                        Subtitle Sub = new Subtitle();
                        i += 2;
                        double StartRE = TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(3, 11), MillisecondLength.N2);
                        i += 3;
                        string TextRe = "";
                        if (Lines[i] != "")
                            TextRe = Lines[i];
                        i++;
                        while (Lines[i] != "")
                        {
                            TextRe += "\n" + Lines[i];
                            i++;
                        }
                        i += 3;
                        double EndRe = TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(3, 11), MillisecondLength.N2);
                        //i += 3;
                        Sub.StartTime = (StartRE);
                        Sub.EndTime = (EndRe);
                        Sub.Text = SubtitleText.FromString(TextRe);
                        this.SubtitleTrack.Subtitles.Add(Sub);
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
            List<string> Lines = new List<string>();
            this.FilePath = filePath;
            Lines.Add("*NonDropFrame");
            Lines.Add("*Width 32");
            Lines.Add("");
            int NO = 0;
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("** Caption Number " + (NO + 1).ToString());
                Lines.Add("*PopOn");
                Lines.Add("*T " + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ":", MillisecondLength.N2));
                Lines.Add("*BottomUp");
                Lines.Add("*Lf01");
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString());
                Lines.Add("");
                NO++;
                Lines.Add("** Caption Number " + (NO + 1).ToString());
                Lines.Add("*PopOn");
                Lines.Add("*T " + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2));
                Lines.Add("*BottomUp");
                Lines.Add("*Lf01");
                Lines.Add("");
                NO++;

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
