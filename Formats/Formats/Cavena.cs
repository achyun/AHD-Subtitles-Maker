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
    public class Cavena : SubtitlesFormat
    {
        public Cavena()
        {
            FrameRate = 23.976;
        }
        public override string Name
        {
            get { return "Cavena (*.txt)"; }
        }

        public override string Description
        {
            get { return "Cavena\n\nThis format type has this view:\n   1   00:00:24:00   00:00:30:00\ntext 1\n\n   2   00:00:31:00   00:00:37:00\n\n........."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".txt" }; return exs; }
        }
        public override bool HasFrameRate
        {
            get
            {
                return true; ;
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
        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            try
            {
                if (Lines.Length > 0)
                {
                    if (Lines[0].Substring(0, 4) == "    " & Lines[0].Substring(5, 7) == "       ")
                    { return true; }
                }
            }
            catch { }
            return false;
        }

        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            string[] Lines = File.ReadAllLines(FilePath, encoding);
            this.SubtitleTrack = new SubtitlesTrack();
            Frm_Framerate Fram = new Frm_Framerate(this);
            Fram.ShowDialog();

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "")
                    {
                        Subtitle Sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                        Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[2], FrameRate);
                        i++;
                        string TextRe = "";
                        if (Lines[i] != "")
                            TextRe = Lines[i];
                        i++;
                        while (Lines[i] != "")
                        {
                            TextRe += "\n" + Lines[i];
                            i++;
                        }
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
            this.FilePath = filePath;
            List<string> Lines = new List<string>();
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("    " + (i + 1).ToString() + "       " +
                    TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate, ":") + "   " +
                 TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate, ":"));
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString());
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                {
                    for (int j = 1; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                    {
                        Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString());
                    }
                }
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
