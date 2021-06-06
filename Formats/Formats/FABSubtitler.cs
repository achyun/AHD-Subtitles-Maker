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
    public class FABSubtitler : SubtitlesFormat
    {
        public FABSubtitler()
        {
            FrameRate = 25;
        }
        public override string Name
        {
            get { return "FAB Subtitler (*.txt)"; }
        }

        public override string Description
        {
            get { return "FAB Subtitler\n\nThis format type has this view:\n00:00:24:00 00:00:30:00\ntext 1\n\n00:00:31:00 00:00:37:00\n\n........."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".txt" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);

            if (Lines.Length > 0)
            {
                try
                {
                    Convert.ToInt32(Lines[0].Substring(0, 2));
                    if (Lines[0].Substring(11, 1) == " ")
                    { return true; }
                }
                catch {  }
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
                    if (Lines[i] != "")
                    {
                        Subtitle Sub = new Subtitle();
                        string[] TextLines = Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[0], FrameRate);
                        Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                        i++;
                        string text = Lines[i];
                        i++;
                        while (Lines[i] != "")
                        {
                            text += "\n" + Lines[i]; i++;
                        }
                        Sub.Text = SubtitleText.FromString(text);
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
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add(TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate, ":") + "  " + TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate, ":"));
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString());
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                {
                    for (int j = 1; j < this.SubtitleTrack.Subtitles[i].Text.TextLines.Count; j++)
                    { Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[j].ToString()); }
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
