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
    public class AQTitle : SubtitlesFormat
    {
        public AQTitle()
        {
            FrameRate = 25;
        }
        public override string Name
        {
            get { return "AQ Title (*.aqt)"; }
        }
        public override string Description
        {
            get { return "AQ Title\n\nThis format type has this view:\n-->> 000720\ntext 1\n\n-->> 000900\n\n.........\nThis subtitle format supports only 2 text lines (one line or two), above line(s) will be removed."; }
        }
        public override string[] Extensions
        {
            get { string[] exs = { ".aqt" }; return exs; }
        }
        public override bool HasFrameRate
        {
            get
            {
                return true;
            }
        }
        public override bool CheckFile(string filePath, Encoding encoding)
        {
            bool Yes = false;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                if (Lines[0].Length >= 5)
                {
                    if (Lines[0].Substring(0, 4) == "-->>")
                    {
                        Yes = true;
                    }
                }
            }
            return Yes;
        }
        public override void Load(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.SubtitleTrack = new SubtitlesTrack("Imported");
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            Frm_Framerate Fram = new Frm_Framerate(this);
            Fram.ShowDialog();
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "")
                    {
                        if (Lines[i].Substring(0, 5) == "-->> ")
                        {
                            Subtitle Sub = new Subtitle();
                            Sub.StartTime = TimeFormatConvertor.From_Frame(Lines[i].Substring(5, Lines[i].Length - 5), FrameRate);
                            i++;
                            string TextRe = Lines[i];
                            i++;
                            if (Lines[i] != "")
                                TextRe += "\n" + Lines[i];
                            i++;
                            Sub.EndTime = TimeFormatConvertor.From_Frame(Lines[i].Substring(5, Lines[i].Length - 5), FrameRate);
                            Sub.Text = SubtitleText.FromString(TextRe,
                                new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular),
                                System.Drawing.Color.White);
                            this.SubtitleTrack.Subtitles.Add(Sub);
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            this.FilePath = filePath;
            List<string> Lines = new List<string>();
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("-->> " + TimeFormatConvertor.To_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate, "D6"));
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[0].ToString());
                if (this.SubtitleTrack.Subtitles[i].Text.TextLines.Count > 1)
                { Lines.Add(this.SubtitleTrack.Subtitles[i].Text.TextLines[1].ToString()); }
                else { Lines.Add(""); }
                Lines.Add("-->> " + TimeFormatConvertor.To_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate, "D6"));
                Lines.Add("");
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

        public override event EventHandler<ASMP.ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
