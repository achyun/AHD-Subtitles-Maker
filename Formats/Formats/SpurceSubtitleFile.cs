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
    public class SpurceSubtitleFile : SubtitlesFormat
    {
        public string _FontName = "MS Sans Serif";//
        public double _FontSize = 8;
        public bool _Bold = false;//
        public bool _UnderLined = false;//
        public bool _Italic = false;//
        public string _HorzAlign = "Center";//
        public string _VertAlign = "Bottom";//
        public int _XOffset = 0;//
        public int _YOffset = 2;//
        public int _ColorIndex1 = 0;
        public int _ColorIndex2 = 1;
        public int _ColorIndex3 = 2;
        public int _ColorIndex4 = 3;
        public int _Contrast1 = 15;//
        public int _Contrast2 = 15;//
        public int _Contrast3 = 15;//
        public int _Contrast4 = 0;//
        public int _FadeIn = 0;//
        public bool _ForceDisplay = false;//
        public int _FadeOut = 0;//
        public bool _TapeOffset = false;//

        public override string Name
        {
            get { return "Spurce Subtitle File (*.stl)"; }
        }

        public override string Description
        {
            get
            {
                return "Spurce Subtitle File (Also : DVD Studio Pro)\n\n" +
                    "This format type has this view:\n00:00:01:00,00:00:02:13,Text1\n00:00:04:73,00:00:07:63,Text2\n.......";
            }
        }

        public override string[] Extensions
        {
            get { string[] _Extensions = { ".stl" }; return _Extensions; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            bool Yes = false;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i].Substring(0, 2) == "00")
                    { Yes = true; break; }
                    else
                    { Yes = false; }
                }
                catch { Yes = false; }
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
                    if (Lines[i].Length > 0)
                    {
                        if (Lines[i].Substring(0, 1) == "$")
                        {
                            if (Lines[i].Substring(0, "$FontName       = ".Length) == "$FontName       = ")
                            {
                                _FontName = Lines[i].Substring("$FontName       = ".Length);
                            }
                            if (Lines[i].Substring(0, "$FontSize       = ".Length) == "$FontSize       = ")
                            {
                                _FontSize = Convert.ToInt32(Lines[i].Substring("$FontSize       = ".Length));
                            }
                            if (Lines[i].Substring(0, "$Bold           = ".Length) == "$Bold           = ")
                            {
                                _Bold = Convert.ToBoolean(Lines[i].Substring("$Bold           = ".Length));
                            }
                            if (Lines[i].Substring(0, "$UnderLined     = ".Length) == "$UnderLined     = ")
                            {
                                _UnderLined = Convert.ToBoolean(Lines[i].Substring("$UnderLined     = ".Length));
                            }
                            if (Lines[i].Substring(0, "$Italic         = ".Length) == "$Italic         = ")
                            {
                                _Italic = Convert.ToBoolean(Lines[i].Substring("$Italic         = ".Length));
                            }
                        }
                        else
                        {
                            Subtitle sub = new Subtitle();
                            string[] TextLines = Lines[i].Split(new char[] { ',' }, 3, StringSplitOptions.RemoveEmptyEntries);
                            sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[0], FrameRate);
                            sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                            System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
                            if (_Bold)
                                style |= System.Drawing.FontStyle.Bold;
                            if (_Italic)
                                style |= System.Drawing.FontStyle.Italic;
                            if (_UnderLined)
                                style |= System.Drawing.FontStyle.Underline;

                            string text = TextLines[2].Replace("|", "\n");
                            sub.Text = SubtitleText.FromString(text, new System.Drawing.Font(_FontName, (float)_FontSize, style),
                                System.Drawing.Color.White);
                            this.SubtitleTrack.Subtitles.Add(sub);
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
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            Lines.Add("//");
            Lines.Add("// Subtitles created using AHD Subtitles Maker Pro");
            Lines.Add("//");
            Lines.Add("//Font select and font size");
            Lines.Add("$FontName       = " + _FontName);
            Lines.Add("$FontSize       = " + _FontSize.ToString());
            Lines.Add("");
            Lines.Add("//Character attributes (global)");
            Lines.Add("$Bold           = " + _Bold.ToString().ToUpper());
            Lines.Add("$UnderLined     = " + _UnderLined.ToString().ToUpper());
            Lines.Add("$Italic         = " + _Italic.ToString().ToUpper());
            Lines.Add("");
            Lines.Add("//Position Control");
            Lines.Add("$HorzAlign      = " + _HorzAlign);
            Lines.Add("$VertAlign      = " + _VertAlign);
            Lines.Add("$XOffset        = " + _XOffset.ToString());
            Lines.Add("$YOffset        = " + _YOffset.ToString());
            Lines.Add("");
            Lines.Add("//Colors");
            Lines.Add("$ColorIndex1    = " + _ColorIndex1.ToString());
            Lines.Add("$ColorIndex2    = " + _ColorIndex2.ToString());
            Lines.Add("$ColorIndex3    = " + _ColorIndex3.ToString());
            Lines.Add("$ColorIndex4    = " + _ColorIndex4.ToString());
            Lines.Add("");
            Lines.Add("//Contrast Control");
            Lines.Add("$Contrast1      = " + _Contrast1.ToString());
            Lines.Add("$Contrast2      = " + _Contrast2.ToString());
            Lines.Add("$Contrast3      = " + _Contrast3.ToString());
            Lines.Add("$Contrast4      = " + _Contrast4.ToString());
            Lines.Add("");
            Lines.Add("//Effects Control");
            Lines.Add("$ForceDisplay   = " + _ForceDisplay.ToString().ToUpper());
            Lines.Add("$FadeIn         = " + _FadeIn.ToString());
            Lines.Add("$FadeOut        = " + _FadeOut.ToString());
            Lines.Add("");
            Lines.Add("//Other Controls");
            Lines.Add("$TapeOffset     = " + _TapeOffset.ToString().ToUpper());
            Lines.Add("");
            Lines.Add("//Subtitles");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string current = TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate, ":") + "," +
                    TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate, ":") + "," +
                  this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "|");
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
                return new cl_SpurceSubtitleFile(this);
            }
        }
    }
}
