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
    public class QuickTime : SubtitlesFormat
    {
        public string _FontName = "Tahoma";
        public float _FontSize = 8;
        public override string Name
        {
            get { return "Quick Time (*.txt)"; }
        }

        public override string Description
        {
            get { return "Quick Time\n\nDoesn't support multilines, so it will make all the subtitle text lines as one.\nThis format type has this view:\n[00:00:24.00]\ntext 1\n\n[00:00:30.00] end time..\n\n........."; }
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
                    if (Lines[0].Substring(0, "{QTtext}".Length) == "{QTtext}")
                    { return true; }
                }
                catch { }
            }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            string[] texttss = Lines[0].Split(new char[] { '{', '}' });
            _FontName = texttss[3].Substring("font:".Length);
            texttss = Lines[1].Split(new char[] { '{', '}' });
            _FontSize = Convert.ToInt32(texttss[3].Substring("size:".Length));
            for (int i = 5; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '[', ']' });
                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                    i++;
                    sub.Text = SubtitleText.FromString(Lines[i],
                        new System.Drawing.Font(_FontName, _FontSize, System.Drawing.FontStyle.Regular), System.Drawing.Color.White);
                    i++;
                    TextLines = Lines[i].Split(new char[] { '[', ']' });
                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
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
            Lines.Add("{QTtext} {font:" + _FontName + "}");
            Lines.Add("{plain} {size:" + _FontSize + "}");
            Lines.Add("{timeScale:30}");
            Lines.Add("{width:160} {height:32}");
            Lines.Add("{timeStamps:absolute} {language:0}");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime,":", MillisecondLength.N2) + "]");
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " "));
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N2) + "]");
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
