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
    public class Scenarist : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Scenarist (*.txt)"; }
        }

        public override string Description
        {
            get { return "Scenarist\n\nThis format has this view\n0001 00:00:01:24 00:00:02:13 text1\n0001 00:00:04:22 00:00:06:15 text2\n....."; }
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
            if (Lines.Length > 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    try
                    {
                        Convert.ToInt32(Lines[i].Substring(0, 1));
                        if (Lines[i].Substring(0, 4) == "0001")
                        { return true; ; }
                    }
                    catch { }
                }
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
                        if (Lines[i].Length >= 23)
                        {
                            Subtitle sub = new Subtitle();
                            string[] TextLines = Lines[i].Split(new char[] { ' ', '\t', '\r' }, 4);
                            if (TextLines[0].Length >= 11)//lines without numbers
                            {
                                int f = 0;
                                if (int.TryParse(TextLines[0].Substring(0, 2), out f))
                                {
                                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[0], FrameRate);
                                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                                    string text = TextLines[2];
                                    if (TextLines.Length >= 4)
                                        text += " " + TextLines[3];
                                    sub.Text = SubtitleText.FromString(text);
                                    this.SubtitleTrack.Subtitles.Add(sub);
                                }
                                else
                                {
                                    this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(SubtitleLine.FromString(Lines[i]));
                                }
                            }
                            else //lines with numbers
                            {
                                int f = 0;
                                if (int.TryParse(TextLines[0], out f))
                                {
                                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[2], FrameRate);
                                    sub.Text = SubtitleText.FromString(TextLines[3]);
                                    this.SubtitleTrack.Subtitles.Add(sub);
                                }
                                else
                                {
                                    this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(SubtitleLine.FromString(Lines[i]));
                                }
                            }
                            int x = (100 * i) / Lines.Length;
                            if (Progress != null)
                                Progress(this, new ProgressArgs(x, "Loading file ...."));
                        }
                        else
                        {
                            this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(SubtitleLine.FromString(Lines[i]));
                        }
                    }
                }
                catch
                {
                }
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
            int i = 1;
            int xx = 0;
            foreach (Subtitle Sub in this.SubtitleTrack.Subtitles)
            {
                string Lin = "";
                string no = "";
                for (int j = i.ToString().Length; j < 4; j++)
                {
                    no += "0";
                }
                Lin = no + i.ToString() + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + "\t" +
                  TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":") + "\t" + Sub.Text.ToString().Replace("\n", "\r");
                lines.Add(Lin);

                i++;
                int x = (100 * xx) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                xx++;
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
