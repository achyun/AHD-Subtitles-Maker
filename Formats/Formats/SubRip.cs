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
using System.Drawing;
using AHD.SM.ASMP;
using System.Diagnostics;

namespace AHD.SM.Formats
{
    public class SubRip : SubtitlesFormat
    {
        public bool WriteColors = true;
        public bool WriteFonts = true;
        public bool WriteFontSizes = false;
        public bool WriteAlignments = true;
        public bool UseAss = true;

        public override string Name
        {
            get { return "SubRip (*.srt)"; }
        }

        public override string Description
        {
            get { return "SubRip\n\nThis format supports font and color, you should enable font and color via format's options GUI. Please note that the font and color will be writen as HTML tags, not Mplayer tags, these kind of tags are not supported.\nThis format type has this view:\n1\n00:00:01,120 --> 00:00:02,340 \nText1\n\n2\n00:00:04,004 --> 00:00:06,023 \nText2\n......."; }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".srt" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 2)
            {
                try
                {
                    if (Lines[1].Substring(13, 3) == "-->")
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
            string[] lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("-->"))
                {
                    // Get timings
                    string[] codes = lines[i].Split(new string[] { "-->" }, StringSplitOptions.None);

                    if (codes == null)
                        continue;
                    if (codes.Length < 2)
                    {
                        Trace.WriteLine("**Error at line " + i + ": not valid timing code; line skipped.", "Formats [SubRip]");
                        continue;
                    }
                    double startTime = TimeFormatConvertor.From_TimeSpan_Milli(codes[0]);
                    double endTime = TimeFormatConvertor.From_TimeSpan_Milli(codes[1]);
                    // Get text
                    string text = "";
                    i++;
                    while (i < lines.Length)
                    {
                        if (lines[i] == "") break;
                        text += lines[i] + "\n"; i++;
                    }
                    if (text.Length > 1)
                        text = text.Substring(0, text.Length - 1);

                    // Add the subtitle
                    Subtitle sub = new Subtitle();
                    sub.StartTime = startTime;
                    sub.EndTime = endTime;
                    sub.Text = TextFormatter.SubtitleTextFromSubRipCode(text);
                    this.SubtitleTrack.Subtitles.Add(sub);
                }
                int x = (100 * i) / lines.Length;
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
                // Add line number
                Lines.Add((i + 1).ToString());
                // Add timing codes
                Lines.Add(string.Format("{0} --> {1}",
                    TimeFormatConvertor.To_TimeSpan_Milli(SubtitleTrack.Subtitles[i].StartTime, ",", MillisecondLength.N3),
                    TimeFormatConvertor.To_TimeSpan_Milli(SubtitleTrack.Subtitles[i].EndTime, ",", MillisecondLength.N3)));
                // Add text
                Lines.Add(TextFormatter.SubtitleTextToSubRipCode(SubtitleTrack.Subtitles[i].Text,
                    WriteColors, WriteFonts, WriteFontSizes, WriteAlignments, UseAss));

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                // add separate
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

        public override bool HasOptions
        {
            get
            {
                return true;
            }
        }
        public override System.Windows.Forms.UserControl OptionsControl
        {
            get
            {
                return new cl_SubRip(this);
            }
        }
    }
}
