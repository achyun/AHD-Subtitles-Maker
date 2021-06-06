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
    public class JACOSub : SubtitlesFormat
    {
        public override string Name
        {
            get { return "JACOSub 2.7 (*.js;*.jss)"; }
        }

        public override string Description
        {
            get { return "JACOSub 2.7\n\nThis format type has this view:\n0:00:01:00(hou:min:sec:Frame) 0:00:02:00 {NTP} Text1\n0:00:04:00 0:00:06:00 {NTP} Text2\n.......\n25.00 Frame rate for PAL\n29.97 Frame rate for NTSC\nyou should select the Unicode encoding for this format."; }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".js", ".jss" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            try
            {
                if (Lines[0] == "#T100")
                { return true; }
            }
            catch { }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 34; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "")
                    {
                        Subtitle sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new string[] { " ", "{NTP}" }, 5, StringSplitOptions.None);
                        sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[0], MillisecondLength.N2);
                        sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                        sub.Text = SubtitleText.FromString(TextLines[4].Replace("\\n", "\n"));
                        this.SubtitleTrack.Subtitles.Add(sub);
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
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            Lines.Add("#T100");
            Lines.Add("");
            Lines.Add("# Directive entries");
            Lines.Add("#D");
            Lines.Add("#D1");
            Lines.Add("#D2");
            Lines.Add("#D3");
            Lines.Add("#D4");
            Lines.Add("#D5");
            Lines.Add("#D6");
            Lines.Add("#D7");
            Lines.Add("#D8");
            Lines.Add("#D9");
            Lines.Add("#D10");
            Lines.Add("#D11");
            Lines.Add("#D12");
            Lines.Add("#D13");
            Lines.Add("#D14");
            Lines.Add("#D15");
            Lines.Add("#D16");
            Lines.Add("#D17");
            Lines.Add("#D18");
            Lines.Add("#D19");
            Lines.Add("#D20");
            Lines.Add("#D21");
            Lines.Add("#D22");
            Lines.Add("#D23");
            Lines.Add("#D24");
            Lines.Add("#D25");
            Lines.Add("#D26");
            Lines.Add("#D27");
            Lines.Add("#D28");
            Lines.Add("#D29");
            Lines.Add("");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string CurrentLine = TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ".", MillisecondLength.N2).Substring(1, 10)
                    + " " + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ".", MillisecondLength.N2).Substring(1, 10) + " {NTP} " +
                    this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "\\n");
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
    }
}
