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
    public class InscriberCG : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Inscriber CG (*.txt)"; }
        }

        public override string Description
        {
            get { return "Inscriber CG\n\nThis format type has this view:\n@@9 Text1<00:00:04:00><00:00:06:00>\n@@9 Text2<00:00:08:00><00:00:10:00>\n.......\n25.00 Frame rate for PAL\n29.97 Frame rate for NTSC\nyou should select the Unicode encoding for this format."; }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".txt" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 3) == "@@9")
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

            List<string> PrevSubtitles = new List<string>();
            for (int i = 8; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "")
                    {
                        Subtitle sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new string[] { "<", ">", "@@9 " }, StringSplitOptions.None);
                        if (TextLines.Length > 2)
                        {
                            if (PrevSubtitles.Count > 0)
                            {
                                string text = PrevSubtitles[0];
                                for (int j = 1; j < PrevSubtitles.Count; j++)
                                {
                                    text = "\n" + PrevSubtitles[j];
                                }
                                text += "\n" + TextLines[1];
                                sub.Text = SubtitleText.FromString(text);
                                PrevSubtitles.Clear();
                            }
                            else
                            {
                                sub.Text = SubtitleText.FromString(TextLines[1]);
                            }
                            sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[2], MillisecondLength.N2);
                            sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[4], MillisecondLength.N2);
                            this.SubtitleTrack.Subtitles.Add(sub);
                        }
                        else
                        {
                            PrevSubtitles.Add(TextLines[1]);
                        }
                    }
                }
                catch
                {
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
            Lines.Add("@@9 Title");
            Lines.Add("@@9 Created by URUSoft Subtitle API (http://www.urusoft.net)");
            Lines.Add("@@9 STORY:");
            Lines.Add("@@9 LANG: ENG");
            Lines.Add("@@9");
            Lines.Add("@@9");
            Lines.Add("@@9");
            Lines.Add("@@9");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string Curr = "@@9 " + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "\n@@9 ");
                Curr += "<" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ":", MillisecondLength.N2) +
                    "><" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2) + ">";
                Lines.Add(Curr);
                Lines.Add("");

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
