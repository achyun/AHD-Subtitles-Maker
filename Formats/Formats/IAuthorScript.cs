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
    public class IAuthorScript : SubtitlesFormat
    {
        public override string Name
        {
            get { return "I-Author Script (*.txt)"; }
        }

        public override string Description
        {
            get { return "I-Author Script\n\nWARNING !!\nDoesn't support multilines, so it will make all text lines as one line!!"; }
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
                    if (Lines[0].Substring(0, "BMPFILE:".Length) == "BMPFILE:")
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

            for (int i = 0; i < Lines.Length; i++)
            {
                if (Lines[i] != "")
                {
                    try
                    {
                        if (Lines[i].Substring(0, 9) == "BMPFILE: ")
                        {
                            Subtitle sub = new Subtitle();
                            sub.Text = SubtitleText.FromString(Lines[i].Substring(9, Lines[i].Length - 9));
                            i += 2;
                            string[] TextLines = Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sub.StartTime = Convert.ToDouble(TextLines[1]);
                            i += 28;
                            TextLines = Lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            sub.EndTime = Convert.ToDouble(TextLines[1]);
                            this.SubtitleTrack.Subtitles.Add(sub);
                            i += 3;
                        }
                    }
                    catch { }
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
            List<string> Lines = new List<string>();
            this.FilePath = filePath;
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add("BMPFILE: " + this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " "));
                Lines.Add("");
                Lines.Add("STARTTIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2"));
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString() + " SETCOLOR Primary 0, 16, 128, 128");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Primary 1, 234, 128, 128");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Primary 2, 16, 128, 128");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Primary 3, 125, 128, 128");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Highlight 0, 16, 128, 128");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Highlight 1, 209, 146, 17");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Highlight 2, 81, 239, 91");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETCOLOR Highlight 3, 144, 35, 54");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " region 207, 170 to 432, 190");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETBLEND Primary 0, 15, 15, 15");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " SETBLEND Hightlight 0, 15, 15, 15");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " FIELDINDEX 0, 1");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].StartTime.ToString("F2") + " ENABLE_OGT");
                Lines.Add("");
                Lines.Add("TIME: " + this.SubtitleTrack.Subtitles[i].EndTime.ToString("F2") + " DISABLE_OGT");
                Lines.Add("");
                Lines.Add("************ Page #" + (i + 1).ToString() + " Finished ***************");
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
