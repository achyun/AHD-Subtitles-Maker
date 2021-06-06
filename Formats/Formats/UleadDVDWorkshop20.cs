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
    public class UleadDVDWorkshop20 : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Ulead DVD Workshop 2.0 (*.txt)"; }
        }

        public override string Description
        {
            get
            {
                return "Ulead DVD Workshop 2.0\n\nThis format has this view:\n" +
                    "#0 00:00:55;19 00:00:59;07\n" +
                    "Text 1\n" +
                    "#1 00:01:00;08 00:01:04;16\n" +
                    "Text 2\n.......";
            }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".txt" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
                if (Lines[0] == "#Ulead subtitle format")
                    return true;
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            FrameRate = Convert.ToDouble(Lines[3].Substring(4, Lines[3].Length - 4));
            Subtitle sub = new Subtitle();
            for (int i = 7; i < Lines.Length; i++)
            {
                if (Lines[i] == "#Subtitle text end")
                    break;
                if (Lines[i].Length > 1)
                {
                    if (Lines[i].Substring(0, 1) == "#")
                    {
                        sub = new Subtitle();
                        string[] tims = Lines[i].Split(new char[] { ' ' });
                        sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(tims[1], FrameRate);
                        sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(tims[2], FrameRate);
                        this.SubtitleTrack.Subtitles.Add(sub);
                    }
                    else
                    {
                        sub.Text.TextLines.Add(SubtitleLine.FromString( Lines[i]));
                    }
                }
                else
                {
                    sub.Text.TextLines.Add(SubtitleLine.FromString(Lines[i]));
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            this.FilePath = filePath;
            List<string> lines = new List<string>();
            lines.Add("#Ulead subtitle format");
            lines.Add("");
            lines.Add("#Subtitle stream attribute begin");
            lines.Add("#FR:" + FrameRate.ToString("F2"));
            lines.Add("#Subtitle stream attribute end");
            lines.Add("");
            lines.Add("#Subtitle text begin");
            int i = 0;
            foreach (Subtitle Sub in this.SubtitleTrack.Subtitles)
            {
                lines.Add("#" + i.ToString() + " " +
                   TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ";")
                    + " " +
                   TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ";"));
                lines.Add(Sub.Text.TextLines[0].ToString());
                for (int j = 1; j < Sub.Text.TextLines.Count; j++)
                    lines.Add(Sub.Text.TextLines[j].ToString());
                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                i++;
            }
            lines.Add("#Subtitle text end");
            lines.Add("");
            lines.Add("#Subtitle text attribute begin");
            lines.Add("#/R:1," + i + " /FP:8  /FS:24");
            lines.Add("#Subtitle text attribute end");
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
        public override bool HasFrameRate
        {
            get
            {
                return true;
            }
        }
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
                return new Cl_frameRate(this);
            }
        }
    }
}
