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
    public class Subviewer1 : SubtitlesFormat
    {
        string[] _Extensions = { ".sub" };
        public string _Title = "";
        public string _Author = "";
        public string _Source = "";
        public string _Program = "";
        public string _File_path = "";
        public int _Delay = 0;
        public int _CDtrack = 0;
        public override string Name
        {
            get { return "Subviewer 1.0 (*.sub)"; }
        }

        public override string Description
        {
            get { return "Subviewer 1.0 \n\nThis format type has this view:\n[00:00:00] (Start time in hou:min:sec)\nText\n[00:00:00] (End time in hou:min:sec)"; }
        }

        public override string[] Extensions
        {
            get { return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            bool Yes = false;
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 1) == "[")
                    { Yes = true; }
                }
                catch { }
            }
            return Yes;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            bool found = false;
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (!found)
                    {
                        if (Lines[i].Substring(0, "******** START SCRIPT ********".Length) == "******** START SCRIPT ********")
                        { found = true; }
                        continue;
                    }
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '[', ']' });
                    sub.StartTime = TimeFormatConvertor.From_TimeSpan(TextLines[1]);
                    i++;
                    sub.Text = SubtitleText.FromString(Lines[i].Replace("|", "\n"));
                    i++;
                    TextLines = Lines[i].Split(new char[] { '[', ']' });
                    sub.EndTime = TimeFormatConvertor.From_TimeSpan(TextLines[1]);
                    i++;
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
            Lines.Add("[TITLE]");
            Lines.Add(_Title);
            Lines.Add("[AUTHOR]");
            Lines.Add(_Author);
            Lines.Add("[SOURCE]");
            Lines.Add(_Source);
            Lines.Add("[PRG]");
            Lines.Add(_Program);
            Lines.Add("[FILEPATH]");
            Lines.Add(_File_path);
            Lines.Add("[DELAY]");
            Lines.Add(_Delay.ToString());
            Lines.Add("[CDTRACK]");
            Lines.Add(_CDtrack.ToString());
            Lines.Add("[BEGIN]");
            Lines.Add("******** START SCRIPT ********");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].StartTime) + "]");
                string text = this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "|");
                Lines.Add(text);
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan(this.SubtitleTrack.Subtitles[i].EndTime) + "]");
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
                return new cl_Subviewer1(this);
            }
        }
    }
}
