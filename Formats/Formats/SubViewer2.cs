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
    public class SubViewer2 : SubtitlesFormat
    {
        string[] _Extensions = { ".sub" };

        public string _Title = "";
        public string _Author = "";
        public string _Source = "Subtitles Made By AHD Subtitles Maker";
        public string _Program = "";
        public string _File_Path = "";
        public string _Comment = "";
        public int _Delay = 0;
        public int _CDTrack = 0;
        public string _FontName = "Tahoma";
        public int _FontSize = 14;
        public int _Color = 0xFFFFFF;

        public override string Name
        {
            get { return "SubViewer 2.0 (*.sub)"; }
        }

        public override string[] Extensions
        {
            get { return _Extensions; }
        }

        public override string Description
        {
            get { return "SubViewer 2.0 \n\nThis format type has this view:\n00:00:01.00,00:00:02.13\nText1\n\n00:00:04.26,00:00:06.23\nText2\n......."; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            bool Yes = false;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 0)
            {
                try
                {
                    if (Lines[0].Substring(0, "[INFORMATION]".Length) == "[INFORMATION]")
                    { Yes = true; }
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

            string[] textss = Lines[11].Split(new char[] { '[', ']', ',' });
            _FontName = textss[8];
            _FontSize = Convert.ToInt32(textss[5]);
            int col = int.Parse(textss[2].Substring(2, textss[2].Length - 2), System.Globalization.NumberStyles.AllowHexSpecifier);
            byte R = (byte)((col & 0xFF0000) >> 16);
            byte G = (byte)((col & 0x00FF00) >> 8);
            byte B = (byte)((col & 0x0000FF));
            _Color = System.Drawing.Color.FromArgb(0xFF, R, G, B).ToArgb();
            for (int i = 12; i < Lines.Length; i++)
            {
                try
                {
                    Subtitle Sub = new Subtitle();
                    string[] TextLines = Lines[i].Split(new char[] { ',' });
                    Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[0], MillisecondLength.N2);
                    Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                    System.Drawing.Font font = new System.Drawing.Font(_FontName, _FontSize, System.Drawing.FontStyle.Regular);
                    System.Drawing.Color color = System.Drawing.Color.FromArgb(_Color);
                    i++;
                    Sub.Text = SubtitleText.FromString(Lines[i].Replace("[br]", "\n"));
                    Sub.Text.SetFont(font);
                    Sub.Text.SetColor(color);
                    this.SubtitleTrack.Subtitles.Add(Sub);
                    i++;
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
            Lines.Add("[INFORMATION]");
            Lines.Add("[TITLE]" + _Title);
            Lines.Add("[AUTHOR]" + _Author);
            Lines.Add("[SOURCE]" + _Source);
            Lines.Add("[PRG]" + _Program);
            Lines.Add("[FILEPATH]" + _File_Path);
            Lines.Add("[DELAY]" + _Delay.ToString());
            Lines.Add("[CD TRACK]" + _CDTrack.ToString());
            Lines.Add("[COMMENT]" + _Comment);
            Lines.Add("[END INFORMATION]");
            Lines.Add("[SUBTITLE]");
            Lines.Add("[COLF]&H" + string.Format("{0:X}", _Color) + ",[SIZE]" + _FontSize.ToString() + ",[FONT]" + _FontName);
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                Lines.Add(TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ".", MillisecondLength.N2) + "," +
                  TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ".", MillisecondLength.N2));
                Lines.Add(this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "[br]"));
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
                return new cl_SubViewer2(this);
            }
        }
    }
}
