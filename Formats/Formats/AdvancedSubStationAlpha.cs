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
    public class AdvancedSubStationAlpha : SubtitlesFormat
    {
        public string name = "Default";//
        public string Fontname = "Tahoma";//
        public double Fontsize = 18;//

        public bool WriteColors = true;//
        public bool WriteFonts = true;//
        public bool WriteFontSizes = false;//
        public bool WritePositions = true;

        public int PrimaryColour = -1;//
        public int SecondaryColour = -1;//
        public int OutlineColour = 0;//
        public int BackColour = -1;//

        public int Bold = -1;//
        public int Italic = 0;//
        public int Underline = 0;//
        public int StrikeOut = 0;//
        public int ScaleX = 0;//
        public int ScaleY = 0;//
        public int Spacing = 0;//
        public int Angle = 0;//
        public int BorderStyle = 1;
        public int Outline = 0;//
        public int Shadow = 0;//
        public int Alignment = 0;//
        public int MarginL = 0;//
        public int MarginR = 0;//
        public int MarginV = 0;//
        public int Encoding = 0;//??


        public override string Name
        {
            get { return "Advanced SubStation Alpha (*.ass)"; }
        }
        public override string Description
        {
            get { return "Advanced SubStation Alpha\n\n"; }
        }
        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".ass" };
                return Exs;
            }
        }
        public override bool CheckFile(string filePath, Encoding encoding)
        {
            string[] Lines = File.ReadAllLines(filePath, encoding);
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] == "[V4+ Styles]")
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
            string[] Lines = File.ReadAllLines(filePath, encoding);

            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack("Imported");
            for (int i = 0; i < Lines.Length; i++)
            {
                try
                {
                    if (Lines[i] != "")
                    {
                        if (Lines[i].Substring(0, "Style:".Length) == "Style:")
                        {
                            string[] TextLines = Lines[i].Split(new char[] { ',' });
                            Fontname = TextLines[1];
                            Fontsize = Convert.ToDouble(TextLines[2]);
                            PrimaryColour = Convert.ToInt32(TextLines[3].Substring(1), 16);
                            Bold = Convert.ToInt32(TextLines[7]);
                            Italic = Convert.ToInt32(TextLines[8]);
                            Underline = Convert.ToInt32(TextLines[9]);
                            StrikeOut = Convert.ToInt32(TextLines[10]);
                        }
                        else if (Lines[i].Substring(0, "Dialogue".Length) == "Dialogue")
                        {
                            Subtitle Sub = new Subtitle();
                            string[] TextLines = Lines[i].Split(new char[] { ',' });
                            Sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[1], MillisecondLength.N2);
                            Sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(TextLines[2], MillisecondLength.N2);

                            System.Drawing.FontStyle style = ((Bold == 1) ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular);
                            if (Italic == 1)
                                style |= System.Drawing.FontStyle.Italic;
                            if (Underline == 1)
                                style |= System.Drawing.FontStyle.Underline;
                            if (StrikeOut == 1)
                                style |= System.Drawing.FontStyle.Strikeout;

                            //   Sub.Text = SubtitleText.FromString(TextLines[TextLines.Length - 1].Replace(@"\N", "\n"), new System.Drawing.Font(Fontname, (float)Fontsize, style), System.Drawing.Color.FromArgb(PrimaryColour));
                            Sub.Text = TextFormatter.SubtitleTextFromASSCode(TextLines[TextLines.Length - 1].Replace(@"\N", "\n"), new System.Drawing.Font(Fontname, (float)Fontsize, style), System.Drawing.Color.FromArgb(PrimaryColour));

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
            Lines.Add("[Script Info]");
            Lines.Add("ScriptType: v4.00");
            Lines.Add("Collisions: Normal");
            Lines.Add("PlayResX: 384");
            Lines.Add("PlayResY: 288");
            Lines.Add("Timer: 100.0000");
            Lines.Add("");
            Lines.Add("[V4+ Styles]");
            Lines.Add("Format: Name, Fontname, Fontsize, PrimaryColour, SecondaryColour, OutlineColour, BackColour, Bold, Italic, Underline, StrikeOut, ScaleX, ScaleY, Spacing, Angle, BorderStyle, Outline, Shadow, Alignment, MarginL, MarginR, MarginV, Encoding");
            Lines.Add("Style: " + name + "," + Fontname + "," + Fontsize + ",&" + string.Format("{0:X}", PrimaryColour) + ",&" + string.Format("{0:X}", SecondaryColour) + ",&" +
                string.Format("{0:X}", OutlineColour) + "," + BackColour + "," + Bold + "," + Italic + "," + Underline + "," + StrikeOut + "," + ScaleX + "," +
                ScaleY + "," + Spacing + "," + Angle + "," + BorderStyle + "," + Outline + "," + Shadow + "," + Alignment + "," +
                MarginL + "," + MarginR + "," + MarginV + "," + Encoding);
            Lines.Add("");
            Lines.Add("[Events]");
            Lines.Add("Format: Layer, Start, End, Style, Actor, MarginL, MarginR, MarginV, Effect, Text");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                string current = "Dialogue: 0," + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ".", MillisecondLength.N2).Substring(1, 10) + ","
                     + TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ".", MillisecondLength.N2).Substring(1, 10) + ",Default,,0000,0000,0000,,"
                     + TextFormatter.SubtitleTextToASSCode(this.SubtitleTrack.Subtitles[i].Text, WriteColors, WriteFonts, WriteFontSizes, WritePositions).Replace("\n", @"\N")
                     ;
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
                return new cl_AdvancedSubStationAlpha(this);
            }
        }
        public override event EventHandler<ASMP.ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
