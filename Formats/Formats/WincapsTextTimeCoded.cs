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
    public class WincapsTextTimeCoded : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Wincaps Text Timecoded  (*.txt)"; }
        }

        public override string Description
        {
            get
            {
                return "Wincaps Text TimeCoded\n\nThis format has this view:\n0001 00:00:12:23 00:00:15:02\n\ntext1\n\n0002 00:00:15:06 00:00:17:06\n\ntext2\n\n.......";
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
            bool Yes = false;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    Convert.ToInt32(Lines[0].Substring(0, 1));
                    if (Lines[0].Substring(0, 4) == "0001")
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

            Frm_Framerate Fram = new Frm_Framerate(this);
            Fram.ShowDialog();
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] == "")
                        continue;
                    Subtitle sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { ' ' });
                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[2], FrameRate);
                    i += 2;
                    sub.Text = SubtitleText.FromString(Lines[i]);
                    i++;
                    while (Lines[i] != "")
                    {
                        sub.Text.TextLines.Add(SubtitleLine.FromString(Lines[i])); i++;
                    }
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            this.FilePath = filePath;
            List<string> lines = new List<string>();
            int i = 1;
            foreach (Subtitle Sub in this.SubtitleTrack.Subtitles)
            {
                string No = i.ToString();
                string zz = "";
                for (int h = No.Length; h < 4; h++)
                    zz += "0";
                No = zz + No;
                string Lin = No + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + " " +
                 TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":");
                lines.Add(Lin);
                lines.Add(""); 

                for (int j = 0; j < Sub.Text.TextLines.Count; j++)
                {
                    lines.Add(Sub.Text.TextLines[j].ToString());
                }
                lines.Add("");

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                i++;
            }
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
