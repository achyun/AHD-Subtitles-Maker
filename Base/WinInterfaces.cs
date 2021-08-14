// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHD.SubtitlesMakerProfessional.Base
{
    /// <summary>
    /// TreeNode holds up subtitles track
    /// </summary>
    public class TreeNode_SubtitlesTrack : TreeNode
    {
        SubtitlesTrack _Subtitletrack;
        /// <summary>
        /// Get or set the subtitles track
        /// </summary>
        public SubtitlesTrack SubtitlesTrack
        {
            get { return _Subtitletrack; }
            set
            {
                _Subtitletrack = value;
                this.Text = _Subtitletrack.Name;
            }
        }
    }
    public class ListViewItem_Subtitle : ListViewItem
    {
        Subtitle Sub = new Subtitle();
        int No = 0;
        /// <summary>
        /// List View Item holds a subtitle
        /// </summary>
        /// <param name="Number">The number of this subtitle</param>
        public ListViewItem_Subtitle(int Number)
        { No = Number; }
        public Subtitle Subtitle
        {
            get
            {
                return Sub;
            }
            set
            {
                Sub = value;
                RefreshTexts();
            }
        }
        /// <summary>
        /// Change the time view
        /// </summary>
        /// <param name="Mode"></param>
        public void ChangeTimeView(TimeMode Mode)
        {
            string text = "";
            switch (Mode)
            {
                case TimeMode.SSMili:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    this.SubItems.Add(Sub.StartTime.ToString("F3"));
                    this.SubItems.Add(Sub.EndTime.ToString("F3"));
                    this.SubItems.Add(Sub.Duration.ToString("F3"));
                    //text
                    text = Sub.TextLines[0];
                    for (int i = 1; i < Sub.TextLines.Length; i++)
                        text += @"|" + Sub.TextLines[i];
                    this.SubItems.Add(text);
                    return;
                case TimeMode.HHMMSSMili:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    string Start = TimeSpan.FromSeconds(Sub.StartTime).ToString();
                    if (Start.Length >= 12)
                    { Start = Start.Substring(0, 12); }
                    else
                    { Start += ".000"; }
                    string End = TimeSpan.FromSeconds(Sub.EndTime).ToString();
                    if (End.Length >= 12)
                    { End = End.Substring(0, 12); }
                    else
                    { End += ".000"; }
                    this.SubItems.Add(Start);
                    this.SubItems.Add(End);
                    this.SubItems.Add(Sub.Duration.ToString("F3"));
                    //text
                    text = Sub.TextLines[0];
                    for (int i = 1; i < Sub.TextLines.Length; i++)
                        text += @"|" + Sub.TextLines[i];
                    this.SubItems.Add(text);
                    return;
                case TimeMode.Frame25:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    int StartCurrmill = TimeSpan.FromSeconds(Sub.StartTime).Milliseconds;
                    int EndCurrmill = TimeSpan.FromSeconds(Sub.EndTime).Milliseconds;
                    int DurCurrmill = TimeSpan.FromSeconds(Sub.Duration).Milliseconds;
                    string Start1 = ((int)(StartCurrmill * 25 / 1000)).ToString();
                    string End1 = ((int)(EndCurrmill * 25 / 1000)).ToString();
                    string Dur1 = ((int)(DurCurrmill * 25 / 1000)).ToString();
                    if (Start1.Length == 1)
                    { Start1 = "0" + Start1; }
                    if (End1.Length == 1)
                    { End1 = "0" + End1; }
                    if (Dur1.Length == 1)
                    { Dur1 = "0" + Dur1; }
                    this.SubItems.Add(TimeSpan.FromSeconds(Sub.StartTime).ToString().Substring(0, 8) + ":" + Start1);
                    this.SubItems.Add(TimeSpan.FromSeconds(Sub.EndTime).ToString().Substring(0, 8) + ":" + End1);
                    this.SubItems.Add(Sub.Duration.ToString("F0") + "." + Dur1);
                    //text
                    text = Sub.TextLines[0];
                    for (int i = 1; i < Sub.TextLines.Length; i++)
                        text += @"|" + Sub.TextLines[i];
                    this.SubItems.Add(text);
                    return;
                case TimeMode.Frame29:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    int StartCurrmill29 = TimeSpan.FromSeconds(Sub.StartTime).Milliseconds;
                    int EndCurrmill29 = TimeSpan.FromSeconds(Sub.EndTime).Milliseconds;
                    int DurCurrmill29 = TimeSpan.FromSeconds(Sub.Duration).Milliseconds;
                    string Start29 = ((int)(StartCurrmill29 * 29.97 / 1000)).ToString();
                    string End29 = ((int)(EndCurrmill29 * 29.97 / 1000)).ToString();
                    string Dur29 = ((int)(DurCurrmill29 * 29.97 / 1000)).ToString();
                    if (Start29.Length == 1)
                    { Start29 = "0" + Start29; }
                    if (End29.Length == 1)
                    { End29 = "0" + End29; }
                    if (Dur29.Length == 1)
                    { Dur29 = "0" + Dur29; }
                    this.SubItems.Add(TimeSpan.FromSeconds(Sub.StartTime).ToString().Substring(0, 8) + ":" + Start29);
                    this.SubItems.Add(TimeSpan.FromSeconds(Sub.EndTime).ToString().Substring(0, 8) + ":" + End29);
                    this.SubItems.Add(Sub.Duration.ToString("F0") + "." + Dur29);
                    //text
                    text = Sub.TextLines[0];
                    for (int i = 1; i < Sub.TextLines.Length; i++)
                        text += @"|" + Sub.TextLines[i];
                    this.SubItems.Add(text);
                    return;
            }
        }
        /// <summary>
        /// Get or set the No value
        /// </summary>
        public int Number
        { get { return No; } set { No = value; } }

        public void RefreshTexts()
        {
            this.SubItems.Clear();
            this.Text = No.ToString();
            this.SubItems.Add(Sub.StartTime.ToString("F3"));
            this.SubItems.Add(Sub.EndTime.ToString("F3"));
            this.SubItems.Add(Sub.Duration.ToString("F3"));
            //text
            string text = Sub.TextLines[0];
            for (int i = 1; i < Sub.TextLines.Length; i++)
                text += @"|" + Sub.TextLines[i];
            this.SubItems.Add(text);
        }
    }
    /// <summary>
    /// List View Item holds a SubtitlesTrack
    /// </summary>
    public class ListViewItem_SubtitlesTrack : ListViewItem
    {
        SubtitlesTrack Lan = new SubtitlesTrack();
        /// <summary>
        /// Get or set the SubtitlesTrack
        /// </summary>
        public SubtitlesTrack SubtitlesTrack
        {
            get { return Lan; }
            set
            {
                Lan = value;
                this.Text = Lan.Name;
            }
        }
    }

    public enum TimeMode
    {
        SSMili, HHMMSSMili, Frame25, Frame29
    }
}
