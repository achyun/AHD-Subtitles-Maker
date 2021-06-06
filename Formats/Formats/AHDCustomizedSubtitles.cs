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
using System.Text;
using System.IO;
using AHD.SM.ASMP;
namespace AHD.SM.Formats
{
    public class AHDCustomizedSubtitles : SubtitlesFormat
    {
        public override string Name
        {
            get { return "AHD Customized Subtitles (*.acs)"; }
        }

        public override string Description
        {
            get
            {
                return "This format allows to export and import customized subtitle formats. Options window will show on export, allow to customize the subtitles data format.\n" +
                  "IMPORTANT NOTES:\n" +
                  ". Options window will open up in export, allows to choose the script to use.\n" +
                  ". This is a beta version, it may export subtitles without errors but fail to import sometimes (It can import files exported using simple scripts). Still working on improving import feature.\n" +
                  ". Please don't use complex timing format, it may make timing errors in import.\n" +
                  ". Mulit-lines subtitle texts are not supported in this version of the format.\n" +
                  ". The exported file will be saved under .acs extension, but it can be used as the format (script) it is exported as. For example, if we use SubRip script, the exported file will be under .acs format but it is a subrip file, just change the exported file extension to .srt, edit the file then remove all lines before ; SUBS (including ; SUBS).";
            }
        }

        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".acs" };
                return Exs;
            }
        }

        public override event EventHandler<ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            return Path.GetExtension(filePath) == ".acs";
        }
        public override void Load(string filePath, Encoding encoding)
        {
            string[] lines = File.ReadAllLines(filePath, encoding);
            string[] scriptLines = new string[0];
            int indexWithinLines = 0;
            if (lines[0] != "; AHD Customized")
            {
                FormAHDCustomizedScriptChoose frm = new FormAHDCustomizedScriptChoose();
                frm.ShowDialog();

                scriptLines = frm.EnteredScript;
            }
            else
            {
                List<string> ss = new List<string>();

                // Read script
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i] != "; SUBS")
                        ss.Add(lines[i]);
                    else
                    {
                        scriptLines = ss.ToArray();
                        indexWithinLines = i + 1;
                        break;
                    }
                }
            }

            string startf = "hh:mm:ss.iii";
            string endf = "hh:mm:ss.iii";
            string durf = "nnnn";
            string text_splitter = "";
            string text_format = "";
            List<string> sub_pattern = new List<string>();
            // Now read the script
            for (int i = 0; i < scriptLines.Length; i++)
            {
                if (scriptLines[i].StartsWith("; startf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    startf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; endf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    endf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; durf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    durf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; text_splitter"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    text_splitter = pl[1];
                }
                else if (scriptLines[i].StartsWith("; text_format"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    text_format = pl[1];
                }
                else if (scriptLines[i].StartsWith("; DATA"))
                {
                    // Now read the data
                    while (i < scriptLines.Length)
                    {
                        i++;
                        // Now read the data
                        while (i < scriptLines.Length)
                        {
                            if (!scriptLines[i].StartsWith("//") && scriptLines[i] != "")
                            {
                                if (scriptLines[i] != "; END")
                                    sub_pattern.Add(scriptLines[i]);
                            }
                            i++;
                        }
                    }
                }
            }
            // Now we have the subtitle pattern, do exportation
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack();

            for (int i = indexWithinLines; i < lines.Length; i++)
            {
                if (lines[i] == "")
                    continue;
                Subtitle sub = new Subtitle();
                double startTime = -1;
                double endTime = -1;
                double duration = -1;
                SubtitleText text = SubtitleText.FromString("");
                for (int s = 0; s < sub_pattern.Count; s++)
                {
                    if (sub_pattern[s].Contains("<start>") || sub_pattern[s].Contains("<end>") || sub_pattern[s].Contains("<dur>") || sub_pattern[s].Contains("<text>"))
                    {
                        // Decode data
                        if (sub_pattern[s] == "<start>")
                        {
                            startTime = AHDCustomizedTimingFormatter.TimeToSeconds(lines[i], startf);
                        }
                        else if (sub_pattern[s] == "<end>")
                        {
                            startTime = AHDCustomizedTimingFormatter.TimeToSeconds(lines[i], endf);
                        }
                        else if (sub_pattern[s] == "<dur>")
                        {
                            startTime = AHDCustomizedTimingFormatter.TimeToSeconds(lines[i], durf);
                        }
                        else if (sub_pattern[s] == "<text>")
                        {
                            text = DecodeText(lines[i], text_splitter, text_format);
                        }
                        else if (sub_pattern[s] == "<subi>" || sub_pattern[s] == "<subn>")
                        {
                            continue;
                        }
                        else if (sub_pattern[s] == "; NEW LINE")
                        {
                            // DO NOTHING, new line will be advanced anyway
                        }
                        else
                        {
                            // 1 Get splitter from the pattern
                            string[] codeSplitters = sub_pattern[s].Split(new string[] { "<subi>", "<subn>", "<start>", "<end>", "<dur>", "<text>" }, StringSplitOptions.None);
                            // 2 new we have the splitter, decode data
                            string[] codes = sub_pattern[s].Split(codeSplitters, StringSplitOptions.None);

                            string current_code = "";
                            int code_index = 0;
                            int charX = 0;
                            foreach (string acsSplitter in codeSplitters)
                            {
                                if (acsSplitter == "")
                                    continue;
                                for (int c = charX; c < lines[i].Length; c++)
                                {
                                    if (c + acsSplitter.Length < lines[i].Length)
                                    {
                                        if (lines[i].Substring(c, acsSplitter.Length) == acsSplitter)
                                        {
                                            // We have the code, decode it
                                            if (codes[code_index] == "<start>")
                                                startTime = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, startf);

                                            else if (codes[code_index] == "<end>")
                                                endTime = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, endf);

                                            else if (codes[code_index] == "<dur>")
                                                duration = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, durf);

                                            else if (codes[code_index] == "<text>")
                                                text = DecodeText(current_code, text_splitter, text_format);
                                            charX = c + acsSplitter.Length;
                                            break;
                                        }
                                        else
                                        {
                                            if (c + 1 < lines[i].Length)
                                                current_code += lines[i].Substring(c, 1);
                                        }
                                    }
                                }
                                current_code = "";
                                code_index++;
                            }
                            // Another phase for the last code line
                            code_index = codes.Length - 1;
                            for (int c = charX; c < lines[i].Length; c++)
                            {
                                if (c + 1 < lines[i].Length)
                                    current_code += lines[i].Substring(c, 1);
                                else
                                {
                                    current_code += lines[i].Substring(c, 1);
                                    // We have the code, decode it
                                    if (codes[code_index] == "<start>")
                                        startTime = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, startf);

                                    else if (codes[code_index] == "<end>")
                                        endTime = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, endf);

                                    else if (codes[code_index] == "<dur>")
                                        duration = AHDCustomizedTimingFormatter.TimeToSeconds(current_code, durf);

                                    else if (codes[code_index] == "<text>")
                                        text = DecodeText(current_code, text_splitter, text_format);
                                    break;
                                }
                            }
                        }
                    }
                    i++;
                }
                i--;
                if (startTime >= 0)
                {
                    bool valid = false;
                    sub.StartTime = startTime;
                    if (endTime >= 0)
                    {
                        sub.EndTime = endTime;
                        valid = true;
                    }
                    else
                    {
                        if (duration >= 0)
                        {
                            sub.EndTime = startTime + duration;
                            valid = true;
                        }
                    }
                    sub.Text = text;

                    if (valid)
                        this.SubtitleTrack.Subtitles.Add(sub);
                }

                int x = (100 * i) / lines.Length;
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
            FormAHDCustomizedScriptChoose frm = new FormAHDCustomizedScriptChoose();
            frm.ShowDialog();

            string[] scriptLines = frm.EnteredScript;

            string startf = "hh:mm:ss.iii";
            string endf = "hh:mm:ss.iii";
            string durf = "nnnn";
            string text_splitter = "";
            string text_format = "";
            List<string> sub_pattern = new List<string>();
            // Now read the script
            for (int i = 0; i < scriptLines.Length; i++)
            {
                if (scriptLines[i].StartsWith("; startf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    startf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; endf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    endf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; durf"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    durf = pl[1];
                }
                else if (scriptLines[i].StartsWith("; text_splitter"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    text_splitter = pl[1];
                }
                else if (scriptLines[i].StartsWith("; text_format"))
                {
                    string[] pl = scriptLines[i].Split('=');
                    text_format = pl[1];
                }
                else if (scriptLines[i].StartsWith("; DATA"))
                {
                    i++;
                    // Now read the data
                    while (i < scriptLines.Length)
                    {
                        if (!scriptLines[i].StartsWith("//") && scriptLines[i] != "")
                        {
                            if (scriptLines[i] != "; END")
                                sub_pattern.Add(scriptLines[i]);
                        }
                        i++;
                    }
                }
            }
            // Now we have the subtitle pattern, do exportation
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            List<string> Lines = new List<string>();
            // Add the script lines
            Lines.AddRange(scriptLines);
            Lines.Add("; SUBS");
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                for (int s = 0; s < sub_pattern.Count; s++)
                {
                    if (sub_pattern[s] != "; NEW LINE")
                    {
                        string line = sub_pattern[s].Replace("<subi>", i.ToString());

                        if (sub_pattern[s].Contains("<subn>"))
                            line = line.Replace("<subn>", (i + 1).ToString());

                        if (sub_pattern[s].Contains("<start>"))
                            line = line.Replace("<start>", AHDCustomizedTimingFormatter.SecondsToTime(SubtitleTrack.Subtitles[i].StartTime, startf));

                        if (sub_pattern[s].Contains("<end>"))
                            line = line.Replace("<end>", AHDCustomizedTimingFormatter.SecondsToTime(SubtitleTrack.Subtitles[i].EndTime, endf));

                        if (sub_pattern[s].Contains("<dur>"))
                            line = line.Replace("<dur>", AHDCustomizedTimingFormatter.SecondsToTime(SubtitleTrack.Subtitles[i].Duration, durf));

                        if (sub_pattern[s].Contains("<text>"))
                            line = line.Replace("<text>", DecodeText(SubtitleTrack.Subtitles[i].Text, text_splitter, text_format));

                        Lines.Add(line);
                    }
                    else
                    {
                        Lines.Add("");
                    }
                }

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
        private string DecodeText(SubtitleText text, string text_splitter, string text_format)
        {
            string val = "";
            if (text_format == "html" || text_format == "ass")
                val = TextFormatter.SubtitleTextToSubRipCode(text,
                          true, true, true, true, text_format == "ass");
            else
                val = text.ToString();

            if (text_splitter != "")
                val = val.Replace("\n", text_splitter);
            return val;
        }
        private SubtitleText DecodeText(string text, string text_splitter, string text_format)
        {
            if (text_splitter != "")
                text = text.Replace("\n", text_splitter);

            SubtitleText val = SubtitleText.FromString("");
            if (text_format == "html" || text_format == "ass")
                val = TextFormatter.SubtitleTextFromSubRipCode(text);
            else
                val = SubtitleText.FromString(text);

            return val;
        }
    }
}
