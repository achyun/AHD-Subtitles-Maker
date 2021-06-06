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
using System.Windows.Forms;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Tree Node attached with Subtitles Track
    /// </summary>
    public class TreeNode_SubtitlesTrack : TreeNode
    {
        private SubtitlesTrack track;
        /// <summary>
        /// Get or set the Subtitles Track attached to this tree node
        /// </summary>
        public SubtitlesTrack SubtitlesTrack
        { get { return track; } set { track = value; RefreshText(); RefreshCheck(); } }
        /// <summary>
        /// Refresh the name of this node
        /// </summary>
        public void RefreshText()
        {
            this.Text = track.Name;
        }
        /// <summary>
        /// Refresh the check box of this node.
        /// </summary>
        public void RefreshCheck()
        {
            this.Checked = track.Preview;
        }
    }
    /// <summary>
    /// ListViewItem holds subtitle
    /// </summary>
    public class ListViewItem_Subtitle : ListViewItem
    {
        Subtitle sub;
        SubtitleTimingMode mode;
        int No;

        /// <summary>
        /// Get or set the subtitle
        /// </summary>
        public Subtitle Subtitle
        { get { return sub; } set { sub = value; RefreshText(); } }
        /// <summary>
        /// Get or set the No value
        /// </summary>
        public int Number
        { get { return No; } set { No = value; } }
        /// <summary>
        /// Get the current SubtitleTimingMode
        /// </summary>
        public SubtitleTimingMode CurrentTimingMode
        { get { return mode; } }
        /// <summary>
        /// Refresh the text
        /// </summary>
        public void RefreshText()
        {
            this.SubItems.Clear();
            this.Text = No.ToString();
            this.SubItems.Add(sub.StartTime.ToString("F3"));
            this.SubItems.Add(sub.EndTime.ToString("F3"));
            this.SubItems.Add(sub.Duration.ToString("F3"));
            //text
            if (sub.Text.TextLines.Count > 0)
            {
                string text = sub.Text.ToString().Replace("\n", "|");
                this.SubItems.Add(text);
            }
            else
                this.SubItems.Add("");
        }
        /// <summary>
        /// Refresh the text
        /// </summary>
        public void RefreshText(bool TextOnly)
        {
            if (TextOnly)
            {
                this.SubItems.Clear();
                //text
                if (sub.Text.TextLines.Count > 0)
                {
                    string text = sub.Text.TextLines[0].ToString().Replace("\n", "|");
                    this.Text = text;
                }
                else
                    this.SubItems.Add("");
            }
            else
                RefreshText();
        }
        public void ChangeTimingView()
        { ChangeTimingView(mode); }
        public void ChangeTimingView(SubtitleTimingMode mode)
        {
            this.mode = mode;
            switch (mode)
            {
                case SubtitleTimingMode.SecondMilli:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    this.SubItems.Add(sub.StartTime.ToString("F3"));
                    this.SubItems.Add(sub.EndTime.ToString("F3"));
                    this.SubItems.Add(sub.Duration.ToString("F3"));
                    break;
                case SubtitleTimingMode.Timespan_Milli:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    string Start = TimeSpan.FromSeconds(sub.StartTime).ToString();
                    if (Start.Length >= 12)
                    { Start = Start.Substring(0, 12); }
                    else
                    { Start += ".000"; }
                    string End = TimeSpan.FromSeconds(sub.EndTime).ToString();
                    if (End.Length >= 12)
                    { End = End.Substring(0, 12); }
                    else
                    { End += ".000"; }
                    this.SubItems.Add(Start);
                    this.SubItems.Add(End);
                    this.SubItems.Add(sub.Duration.ToString("F3"));
                    break;
                case SubtitleTimingMode.Timespan_PAL:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    int StartCurrmill = TimeSpan.FromSeconds(sub.StartTime).Milliseconds;
                    int EndCurrmill = TimeSpan.FromSeconds(sub.EndTime).Milliseconds;
                    int DurCurrmill = TimeSpan.FromSeconds(sub.Duration).Milliseconds;
                    string Start1 = ((int)(StartCurrmill * 25 / 1000)).ToString();
                    string End1 = ((int)(EndCurrmill * 25 / 1000)).ToString();
                    string Dur1 = ((int)(DurCurrmill * 25 / 1000)).ToString();
                    if (Start1.Length == 1)
                    { Start1 = "0" + Start1; }
                    if (End1.Length == 1)
                    { End1 = "0" + End1; }
                    if (Dur1.Length == 1)
                    { Dur1 = "0" + Dur1; }
                    this.SubItems.Add(TimeSpan.FromSeconds(sub.StartTime).ToString().Substring(0, 8) + ":" + Start1);
                    this.SubItems.Add(TimeSpan.FromSeconds(sub.EndTime).ToString().Substring(0, 8) + ":" + End1);
                    this.SubItems.Add(sub.Duration.ToString("F0") + "." + Dur1);
                    break;
                case SubtitleTimingMode.Timespan_NTSC:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    int StartCurrmill29 = TimeSpan.FromSeconds(sub.StartTime).Milliseconds;
                    int EndCurrmill29 = TimeSpan.FromSeconds(sub.EndTime).Milliseconds;
                    int DurCurrmill29 = TimeSpan.FromSeconds(sub.Duration).Milliseconds;
                    string Start29 = ((int)(StartCurrmill29 * 29.97 / 1000)).ToString();
                    string End29 = ((int)(EndCurrmill29 * 29.97 / 1000)).ToString();
                    string Dur29 = ((int)(DurCurrmill29 * 29.97 / 1000)).ToString();
                    if (Start29.Length == 1)
                    { Start29 = "0" + Start29; }
                    if (End29.Length == 1)
                    { End29 = "0" + End29; }
                    if (Dur29.Length == 1)
                    { Dur29 = "0" + Dur29; }
                    this.SubItems.Add(TimeSpan.FromSeconds(sub.StartTime).ToString().Substring(0, 8) + ":" + Start29);
                    this.SubItems.Add(TimeSpan.FromSeconds(sub.EndTime).ToString().Substring(0, 8) + ":" + End29);
                    this.SubItems.Add(sub.Duration.ToString("F0") + "." + Dur29);
                    break;
                case SubtitleTimingMode.Frames_NTSC:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    this.SubItems.Add((sub.StartTime * 29.97).ToString("F2"));
                    this.SubItems.Add((sub.EndTime * 29.97).ToString("F2"));
                    this.SubItems.Add((sub.Duration * 29.97).ToString("F2"));
                    break;
                case SubtitleTimingMode.Frames_Pal:
                    this.SubItems.Clear();
                    this.Text = No.ToString();
                    this.SubItems.Add((sub.StartTime * 25).ToString("F2"));
                    this.SubItems.Add((sub.EndTime * 25).ToString("F2"));
                    this.SubItems.Add((sub.Duration * 25).ToString("F2"));
                    break;
            }
            //text
            if (sub.Text.TextLines.Count > 0)
            {
                string text = sub.Text.ToString().Replace("\n", "|");
                this.SubItems.Add(text);
            }
            else
                this.SubItems.Add("");
        }
    }
    public class ListViewItem_SubtitlesTrack : ListViewItem
    {
        SubtitlesTrack track;
        public SubtitlesTrack SubtitlesTrack
        { get { return track; } set { track = value; RefreshText(); } }
        public void RefreshText()
        {
            this.Text = track.Name;
        }
    }
}
