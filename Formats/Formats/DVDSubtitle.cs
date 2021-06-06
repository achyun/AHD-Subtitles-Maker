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
    public class DVDSubtitle : SubtitlesFormat
    {
        public string DiscID = "";

        public string DVDTitle = "";

        public string Language = "";

        public string Author = "";

        public string Web = "";

        public string Info = "Subtitles Created By AHD Subtitles Maker Pro";

        public string License = "";

        public override string Name
        {
            get { return "DVD Subtitle (*.sub)"; }
        }

        public override string Description
        {
            get { return "DVD Subtitle\n\nThis format type has this view:\n{T 00:00:00:00 (hou:min:sec:milisec)\nText\n}"; }
        }

        public override string[] Extensions
        {
            get
            {
                string[] exs = { ".sub" };
                return exs;
            }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);

            if (Lines.Length >= 14)
            {
                try
                {
                    if (Lines[13].Substring(0, 3) == "{T ")
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
            this.SubtitleTrack = new SubtitlesTrack();
            this.FilePath = filePath;
            string[] Lines = File.ReadAllLines(FilePath, encoding);

            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "" | Lines[i] != null)
                    {
                        if (Lines[i].Substring(0, 3) == "{T ")
                        {
                            Subtitle Sub = new Subtitle();
                            Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(3, 11), MillisecondLength.N2);
                            i++;
                            string text = Lines[i];
                            i++;
                            while (Lines[i].Length > 0)
                            {
                                if (Lines[i].Substring(0, 1) != "}")
                                {
                                    text += "\n" + Lines[i];
                                    i++;
                                }
                                else
                                { break; }
                            }
                            i++;
                            Sub.Text = SubtitleText.FromString(text);
                            Sub.EndTime = Convert.ToDouble(TimeFormatConvertor.From_TimeSpan_Milli(Lines[i].Substring(3, 11), MillisecondLength.N2));
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
            List<string> Lines = new List<string>();
            this.FilePath = filePath;
            Lines.Add("{HEAD");
            Lines.Add("DICID=" + DiscID);
            Lines.Add("DVDTITLE=" + DVDTitle);
            Lines.Add("CODEPAGE=1250");
            Lines.Add("FORMAT=" + encoding.EncodingName);
            Lines.Add("LANG=" + Language);
            Lines.Add("TITLE=1");
            Lines.Add("ORIGINAL=ORIGINAL");
            Lines.Add("AUTHOR=" + Author);
            Lines.Add("WEB=" + Web);
            Lines.Add("INFO=" + Info);
            Lines.Add("LICENSE=" + License);
            Lines.Add("}");
            int xx = 0;
            foreach (Subtitle Sub in this.SubtitleTrack.Subtitles)
            {
                Lines.Add("{T " + TimeFormatConvertor.To_TimeSpan_Milli(Sub.StartTime, ":", MillisecondLength.N2));
                foreach (SubtitleLine Txt in Sub.Text.TextLines)
                {
                    Lines.Add(Txt.ToString());
                }
                Lines.Add("}");
                Lines.Add("{T " + TimeFormatConvertor.To_TimeSpan_Milli(Sub.EndTime, ":", MillisecondLength.N2));
                Lines.Add("");
                Lines.Add("}\n");

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
                return new cl_DVDSubtitle(this);
            }
        }
    }
}
