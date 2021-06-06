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
    public class KaraokeLyrics : SubtitlesFormat
    {   
        public string _Title = "";
        public string _Artist = "";
        public string _Album = "";
        public string _About = "Subtitles Created By AHD Subtitles Maker Pro";
        public int _Offset = 0;

        public override string Name
        {
            get { return "Karaoke Lyrics LRC (*.lrc)"; }
        }

        public override string Description
        {
            get { return "Karaoke Lyrics LRC\n\nDoesn't support multi lines, so it will take the first line only from each subtitle text.\nThis format type has this view:\n[01:00]([min:sec])Text1\n[03:00]Text2\n......."; }
        }

        public override string[] Extensions
        {
            get { string[] exs = { ".lrc" }; return exs; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, 1) == "[")
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
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { '[', ']' });
                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli("00:"+TextLines[1], MillisecondLength.N2);
                    Sub.Text =SubtitleText.FromString( TextLines[2]);
                    i++;
                    TextLines = Lines[i].Split(new char[] { '[', ']' });
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli("00:" + TextLines[1], MillisecondLength.N2);
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
            Lines.Add("[ti:" + _Title + "]");
            Lines.Add("[ar:" + _Artist + "]");
            Lines.Add("[al:" + _Album + "]");
            Lines.Add("[by:" + _About + "]");
            Lines.Add("[offset:" + _Offset.ToString() + "]");
            Lines.Add("");
            int xx = 0;
            foreach (Subtitle sub in this.SubtitleTrack.Subtitles)
            {
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime, ".", MillisecondLength.N2).Substring(3, 8) + "]" + sub.Text.ToString().Replace("\n", " "));
                Lines.Add("[" + TimeFormatConvertor.To_TimeSpan_Milli(sub.EndTime, ".", MillisecondLength.N2).Substring(3, 8) + "]");

                int x = (100 * xx) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                xx++;
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
                return new cl_KarokaLyrics(this);
            }
        }
    }
}
