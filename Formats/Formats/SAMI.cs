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
    public class SAMI : SubtitlesFormat
    {
        public override string Name
        {
            get { return "SAMI (*.smi)"; }
        }

        public override string Description
        {
            get { return "SAMI\n\n"; }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".smi" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 1)
            {
                if (Lines[0] == "<SAMI>")
                    return true;
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
                    if (Lines[i].Length >= 5)
                    {
                        if (Lines[i].Substring(0, "<SYNC".Length) == "<SYNC")
                        {
                            Subtitle sub = new Subtitle();
                            string[] texts = Lines[i].Split(new string[] { "><" }, StringSplitOptions.None);
                            string time = texts[0].Substring("<SYNC Start=".Length, texts[0].Length - "<SYNC Start=".Length);
                            string txt = texts[1].Substring("P Class=ENUSCC>".Length,
                                (texts[1].Length - "P Class=ENUSCC>".Length) - "</P".Length);
                            sub.Text = SubtitleText.FromString(txt.Replace("<br />", "\n"));
                            sub.StartTime = GetTime(time);
                            i++;
                            texts = Lines[i].Split(new string[] { "><" }, StringSplitOptions.None);
                            time = texts[0].Substring("<SYNC Start=".Length, texts[0].Length - "<SYNC Start=".Length);
                            sub.EndTime = GetTime(time);

                            this.SubtitleTrack.Subtitles.Add(sub);
                        }
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
            List<string> lines = new List<string>();

            lines.Add("<SAMI>");
            lines.Add("");
            lines.Add("<HEAD>");
            lines.Add("<TITLE>Final Cut Pro Xml</TITLE>");
            lines.Add("");
            lines.Add("<SAMIParam>");
            lines.Add("  Metrics {time:ms;}");
            lines.Add("  Spec {MSFT:1.0;}");
            lines.Add("</SAMIParam>");
            lines.Add("");
            lines.Add(@"<STYLE TYPE=""text/css"">");
            lines.Add("<!--");
            lines.Add("  P { font-family: Arial; font-weight: normal; color: white; background-color: black; text-align: center; }");
            lines.Add("  .ENUSCC { name: English; lang: en-US ; SAMIType: CC ; }");
            lines.Add("-->");
            lines.Add("</STYLE>");
            lines.Add("");
            lines.Add("</HEAD>");
            lines.Add("");
            lines.Add("<BODY>");
            lines.Add("");
            lines.Add("<-- Open play menu, choose Captions and Subtiles, On if available -->");
            lines.Add("<-- Open tools menu, Security, Show local captions when present -->");
            lines.Add("");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                lines.Add("<SYNC Start=" + MakeTime(this.SubtitleTrack.Subtitles[i].StartTime) + "><P Class=ENUSCC>" +
                    this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "<br />") + "</P></SYNC>");
                lines.Add("<SYNC Start=" + MakeTime(this.SubtitleTrack.Subtitles[i].EndTime) + "><P Class=ENUSCC>" + "&nbsp;" + "</P></SYNC>");

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }
            lines.Add("</BODY>");
            lines.Add("</SAMI>");
            File.WriteAllLines(FilePath, lines.ToArray(), encoding);
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

        string MakeTime(double time)
        {
            string val = ((int)(time * 1000)).ToString();
            return val;
        }
        double GetTime(string time)
        {
            double tim = double.Parse(time);
            tim /= 1000;
            return tim;
        }
    }
}
