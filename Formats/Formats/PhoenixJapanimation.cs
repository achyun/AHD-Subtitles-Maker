﻿// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
    public class PhoenixJapanimation : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Phoenix Japanimation (*.pjs)"; }
        }

        public override string Description
        {
            get
            {
                return "Phoenix Japanimation\n\nThis format type has this view:\n" +
                    @"    132,    142, ""Text1""" + "\n" +
                    @"    152,    162, ""Text2""\n.......";
            }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".pjs" };
                return Exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 3) == "   ")
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

            Frm_Framerate Fram = new Frm_Framerate(this);
            Fram.ShowDialog();
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
                    Sub.StartTime = TimeFormatConvertor.From_Frame(TextLines[0],FrameRate);
                    Sub.EndTime = TimeFormatConvertor.From_Frame(TextLines[1], FrameRate);
                    TextLines = Lines[i].Split(new char[] { '"' });
                    Sub.Text = SubtitleText.FromString(TextLines[1].Replace("|", "\n"));
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
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string current = "    " + TimeFormatConvertor.To_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate) + ",    " +
                    TimeFormatConvertor.To_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate) + ", " + @"""" +
                    this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "|");
                current += @"""";
                Lines.Add(current);

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
