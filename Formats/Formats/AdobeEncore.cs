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
    public class AdobeEncore : SubtitlesFormat
    {
        public AdobeEncore()
        {
            FrameRate = 29.97;
        }
        public bool UseNumbers = false;
        public bool TabsMode = false;

        public override string Name
        {
            get { return "Adobe Encore (*.txt)"; }
        }
        public override string Description
        {
            get
            {
                return "Adobe Encore\n\nCan be used by Adobe(R) Encore (Encore DVD, Encore CSx ..etc), you should see the options for more. This format script looks like this :\n1 00:00:01:04 00:00:05:23 Text 1\nText 1 line 2\n2 00:00:12:05 00:00:20:23 Text 2\n........\n\n Can import if there's no number at the start of the line.";
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
            string[] Lines = File.ReadAllLines(filePath, encoding);
            if (Lines.Length > 1)
            {
                for (int i = 0; i < Lines.Length; i++)
                {
                    if (Lines[i] != "")
                    {
                        string[] items = Lines[i].Split(new char[] { ' ', '\t', '\r' });
                        if (items.Length >= 3)
                        {
                            if (TimeFormatChecker.IsTimeSpanX(items[0]) & TimeFormatChecker.IsTimeSpanX(items[1]))
                                return true;
                        }
                        return false;
                    }
                }
            }
            return false;
        }
        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs()); 
            this.FilePath = filePath;
            string[] Lines = File.ReadAllLines(filePath, encoding);
            this.SubtitleTrack = new SubtitlesTrack();
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
                                    sub.Text = SubtitleText.FromString(text, new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular), System.Drawing.Color.White);

                                    this.SubtitleTrack.Subtitles.Add(sub);
                                }
                                else
                                {
                                    this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(
                                        SubtitleLine.FromString("\n" + Lines[i],
                                        new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular),
                                        System.Drawing.Color.White));
                                }
                            }
                            else //lines with numbers
                            {
                                int f = 0;
                                if (int.TryParse(TextLines[0], out f))
                                {
                                    sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[1], FrameRate);
                                    sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(TextLines[2], FrameRate);
                                    string text = TextLines[3];
                                    sub.Text = SubtitleText.FromString(text, new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular), System.Drawing.Color.White);
                                    this.SubtitleTrack.Subtitles.Add(sub);
                                }
                                else
                                {
                                    this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(
                                      SubtitleLine.FromString("\n" + Lines[i],
                                      new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular),
                                      System.Drawing.Color.White));
                                }
                            }
                            int x = (100 * i) / Lines.Length;
                            if (Progress != null)
                                Progress(this, new ProgressArgs(x, "Loading file ...."));
                        }
                        else
                        {
                            this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text.TextLines.Add(
                                              SubtitleLine.FromString("\n" + Lines[i],
                                              new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular),
                                              System.Drawing.Color.White));
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
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            FilePath = filePath;
            List<string> lines = new List<string>();
            if (!TabsMode)
            {
                int i = 1;
                int xx = 0;
                foreach (Subtitle Sub in SubtitleTrack.Subtitles)
                {
                    string Lin = "";
                    if (UseNumbers)
                    {
                        if (FrameRate == 25)
                        {
                            Lin = i.ToString() + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":") + " " + Sub.Text.TextLines[0];
                        }
                        else
                        {
                            Lin = i.ToString() + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ";") + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ";") + " " + Sub.Text.TextLines[0];
                        }
                    }
                    else
                    {
                        if (FrameRate == 25)
                        {
                            Lin = TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":") + " " + Sub.Text.TextLines[0];
                        }
                        else
                        {
                            Lin = TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ";") + " " + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ";") + " " + Sub.Text.TextLines[0];
                        }
                    }
                    lines.Add(Lin);
                    i++;
                    for (int j = 1; j < Sub.Text.TextLines.Count; j++)
                    {
                        lines.Add(Sub.Text.TextLines[j].ToString());
                    }
                    int x = (100 * xx) / SubtitleTrack.Subtitles.Count;
                    if (Progress != null)
                        Progress(this, new ProgressArgs(x, "Saving ...."));
                    xx++;
                }
                File.WriteAllLines(FilePath, lines.ToArray(), encoding);
            }
            else//Tabs mode
            {
                int i = 1;
                int xx = 0;
                foreach (Subtitle Sub in SubtitleTrack.Subtitles)
                {
                    string Lin = "";
                    if (UseNumbers)
                    {
                        if (FrameRate == 25)
                        {
                            Lin = i.ToString() + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":") + "\t" + Sub.Text.TextLines[0];
                        }
                        else
                        {
                            Lin = i.ToString() + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ";") + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ";") + "\t" + Sub.Text.TextLines[0];
                        }
                    }
                    else
                    {
                        if (FrameRate == 25)
                        {
                            Lin = TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ":") + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ":") + "\t" + Sub.Text.TextLines[0];
                        }
                        else
                        {
                            Lin = TimeFormatConvertor.To_TimeSpan_Frame(Sub.StartTime, FrameRate, ";") + "\t" + TimeFormatConvertor.To_TimeSpan_Frame(Sub.EndTime, FrameRate, ";") + "\t" + Sub.Text.TextLines[0];
                        }
                    }
                    for (int j = 1; j < Sub.Text.TextLines.Count; j++)
                    {
                        Lin += "\r" + (Sub.Text.TextLines[j]);
                    }
                    lines.Add(Lin);
                    i++;
                    int x = (100 * xx) / SubtitleTrack.Subtitles.Count;
                    if (Progress != null)
                        Progress(this, new ProgressArgs(x, "Saving ...."));
                    xx++;
                }
                File.WriteAllLines(FilePath, lines.ToArray(), encoding);
            }
            if (Progress != null)
                Progress(this, new ProgressArgs(100, "Save Completed."));
            if (SaveFinished != null)
                SaveFinished(this, new EventArgs());
        }

        public override bool HasFrameRate
        {
            get
            {
                return true;
            }
        }
        public override double[] FrameRates
        {
            get
            {
                double[] frms = { 25, 29.97 };
                return frms;
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
                return new Cl_AdobeEncore(this);
            }
        }

        public override event EventHandler<ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
