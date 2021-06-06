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
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;
using AHD.SM.ASMP;

namespace AHD.SM.Formats
{
    public class AHDSubtitles : SubtitlesFormat
    {
        string version = "";
        public override string Name
        {
            get { return "AHD Subtitles (*.asm)"; }
        }
        public override string Description
        {
            get { return "AHD Subtitles (version 5, older versions can be imported)\n\nCan save all subtitle tracks including full subtitles data like position, font and color, see the help document for more about this subtitles format."; }
        }
        public override string[] Extensions
        {
            get { return new string[] { ".asm" }; }
        }

        public override bool CheckFile(string filePath, Encoding encoding)
        {
            try
            {
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(filePath, sett);
                XMLread.Read();//Reads the XML definition <XML>
                XMLread.Read();//Reads the header
                //check the header
                if (XMLread.Name == "AHDSubtitles")
                {
                    return true;
                }
            }
            catch { }
            return false;
        }
        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());

            this.FilePath = filePath;
            this.SubtitleTracks = new List<SubtitlesTrack>();
            XmlReaderSettings sett = new XmlReaderSettings();
            sett.IgnoreWhitespace = true;
            XmlReader XMLread = XmlReader.Create(filePath, sett);
            XMLread.Read();//Reads the XML definition <XML>
            XMLread.Read();//Reads the header

            //check the header
            if (XMLread.Name == "AHDSubtitles")
            {
                XMLread.MoveToAttribute("Version");
                switch (version = XMLread.Value.ToString())
                {
                    case "5.0": LoadVersion5(XMLread); break;
                    case "3.1":
                    default: LoadVersion3(XMLread); break;
                }
            }
            XMLread.Close();

            if (Progress != null)
                Progress(this, new ProgressArgs(100, "Load Completed."));
            if (LoadFinished != null)
                LoadFinished(this, new EventArgs());
        }
        void LoadVersion3(XmlReader XMLread)
        {
              SubtitlesTrack track = new SubtitlesTrack();
              while (XMLread.Read())
              {
                  //track
                  if ((XMLread.Name == "Language" || XMLread.Name == "Track") && XMLread.IsStartElement())
                  {
                      track = new SubtitlesTrack();
                      XMLread.MoveToAttribute("Name");
                      track.Name = XMLread.Value.ToString();
                      this.SubtitleTracks.Add(track);
                  }
                  //subtitle
                  if (XMLread.Name == "Sub" && XMLread.IsStartElement())
                  {
                      Subtitle sub = new Subtitle();
                      XMLread.MoveToAttribute("Start");
                      sub.StartTime = Convert.ToDouble(XMLread.Value.ToString());
                      XMLread.MoveToAttribute("End");
                      sub.EndTime = Convert.ToDouble(XMLread.Value.ToString());
                      XMLread.MoveToAttribute("Text");
                      sub.Text = SubtitleText.FromString(XMLread.Value.ToString());
                      if (version == "3.1")
                      {
                          XMLread.MoveToAttribute("Color");
                          sub.Text.SetColor(Color.FromArgb(int.Parse(XMLread.Value.ToString())));
                          XMLread.MoveToAttribute("Location");
                          try
                          {
                              sub.Text.Position = (SubtitlePosition)Enum.Parse(typeof(SubtitlePosition), XMLread.Value.ToString());
                          }
                          catch { sub.Text.IsCustomPosition = true; }

                          XMLread.Read();//read font
                          XMLread.MoveToAttribute("FontName");
                          string name = XMLread.Value.ToString();
                          XMLread.MoveToAttribute("FontSize");
                          float size = float.Parse(XMLread.Value.ToString());
                          XMLread.MoveToAttribute("FontStyle");
                          FontStyle style = (FontStyle)Enum.Parse(typeof(FontStyle), XMLread.Value);
                          sub.Text.SetFont(new Font(name, (float)size, style));

                          XMLread.Read();//read Custom Location
                          XMLread.MoveToAttribute("X");
                          int x = Convert.ToInt32(XMLread.Value.ToString());
                          XMLread.MoveToAttribute("Y");
                          int y = Convert.ToInt32(XMLread.Value.ToString());
                          sub.Text.CustomPosition = new Point(x, y);
                      }
                      track.Subtitles.Add(sub);
                  }
              }
        }
        void LoadVersion5(XmlReader XMLread)
        {
            SubtitlesTrack track = new SubtitlesTrack();
            while (XMLread.Read())
            {
                //track
                if (XMLread.Name == "Track" && XMLread.IsStartElement())
                {
                    track = new SubtitlesTrack();
                    XMLread.MoveToAttribute("Name");
                    track.Name = XMLread.Value.ToString();
                    this.SubtitleTracks.Add(track);
                }
                //subtitle
                if (XMLread.Name == "Sub" && XMLread.IsStartElement())
                {
                    Subtitle sub = new Subtitle();
                    List<Font> fonts = new List<Font>();
                    List<Color> colors = new List<Color>();
                    XMLread.MoveToAttribute("Start");
                    sub.StartTime = Convert.ToDouble(XMLread.Value.ToString());
                    XMLread.MoveToAttribute("End");
                    sub.EndTime = Convert.ToDouble(XMLread.Value.ToString());
                    while (XMLread.Read())
                    {
                        if (XMLread.Name == "Position" && XMLread.IsStartElement())
                        {
                            XMLread.MoveToAttribute("Location");
                            sub.Text.Position = (SubtitlePosition)Enum.Parse(typeof(SubtitlePosition), XMLread.Value.ToString());
                            XMLread.MoveToAttribute("IsCustomLocation");
                            sub.Text.IsCustomPosition = Convert.ToBoolean(XMLread.Value.ToString());
                        }
                        if (XMLread.Name == "CustomLocation" && XMLread.IsStartElement())
                        {
                            int x = 0;
                            int y = 0;
                            XMLread.MoveToAttribute("X");
                            x = Convert.ToInt32(XMLread.Value.ToString());
                            XMLread.MoveToAttribute("Y");
                            y = Convert.ToInt32(XMLread.Value.ToString());
                            sub.Text.CustomPosition = new Point(x, y);
                        }
                        if (XMLread.Name == "Font" && XMLread.IsStartElement())
                        {
                            XMLread.MoveToAttribute("Name");
                            string name = XMLread.Value.ToString();
                            XMLread.MoveToAttribute("Size");
                            float size = float.Parse(XMLread.Value.ToString());
                            XMLread.MoveToAttribute("Style");
                            string style = XMLread.Value.ToString();
                            string[] styles = style.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            FontStyle fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), styles[0]);
                            for (int i = 1; i < styles.Length; i++)
                            {
                                fontStyle |= (FontStyle)Enum.Parse(typeof(FontStyle), styles[i]);
                            }

                            fonts.Add(new Font(name, size, fontStyle));
                        }
                        if (XMLread.Name == "Color" && XMLread.IsStartElement())
                        {
                            XMLread.MoveToAttribute("ARGB");
                            int argb = int.Parse(XMLread.Value.ToString());
                            colors.Add(Color.FromArgb(argb));
                        }
                        if (XMLread.Name == "Text" && XMLread.IsStartElement())
                        {
                            char[] textChars = XMLread.ReadString().ToCharArray();
                            SubtitleLine currentLine = new SubtitleLine();
                            Font currentFont = fonts[0];
                            Color currentColor = colors[0];
                            //decode the text
                            for (int c = 0; c < textChars.Length; c++)
                            {
                                if (textChars[c] == '[')//this is a code
                                {
                                    c++;//advance [
                                    string code = "";
                                    while (textChars[c] != ']')
                                    {
                                        code += textChars[c].ToString();
                                        c++;
                                    }
                                   
                                    //decode the code
                                    switch (code.Substring(0, 1))
                                    {
                                        case "A"://new line !!
                                            currentLine = new SubtitleLine();
                                            currentLine.Alignement = (LineAlignement)Enum.Parse(typeof(LineAlignement),
                                                code.Substring(2, code.Length - 2));
                                            sub.Text.TextLines.Add(currentLine);
                                            break;
                                        case "f"://font and color set
                                            string[] fc = code.Split(new char[] { ',' });
                                            int findex = int.Parse(fc[0].Substring(1, fc[0].Length - 1)) - 1;
                                            int cindex = int.Parse(fc[1].Substring(1, fc[1].Length - 1)) - 1;

                                            currentFont = fonts[findex];
                                            currentColor = colors[cindex];
                                            break;
                                    }
                                }
                                else//a char
                                {
                                    currentLine.Chars.Add(new SubtitleChar(textChars[c], currentFont, currentColor));
                                }
                            }
                        }
                        if (XMLread.Name == "Sub" && !XMLread.IsStartElement())
                        {
                            track.Subtitles.Add(sub);
                            break;
                        }
                    }
                }
            }
        }
        public override void Save(string filePath, Encoding encoding)
        {
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true; sett.Encoding = encoding;
            XmlWriter XMLwrt = XmlWriter.Create(FilePath, sett);
            XMLwrt.WriteStartElement("AHDSubtitles");//header
            XMLwrt.WriteAttributeString("Version", "5.0");//header version
            foreach (SubtitlesTrack Lan in this.SubtitleTracks)
            {
                XMLwrt.WriteStartElement("Track");
                XMLwrt.WriteAttributeString("Name", Lan.Name);
                for (int N = 0; N < Lan.Subtitles.Count; N++)
                {
                    XMLwrt.WriteStartElement("Sub");
                    //timings
                    XMLwrt.WriteAttributeString("Start", Lan.Subtitles[N].StartTime.ToString());
                    XMLwrt.WriteAttributeString("End", Lan.Subtitles[N].EndTime.ToString());
                    //position
                    XMLwrt.WriteStartElement("Position");
                    XMLwrt.WriteAttributeString("Location", Lan.Subtitles[N].Text.Position.ToString());
                    XMLwrt.WriteAttributeString("IsCustomLocation", Lan.Subtitles[N].Text.IsCustomPosition.ToString());
                    XMLwrt.WriteStartElement("CustomLocation");
                    XMLwrt.WriteAttributeString("X", Lan.Subtitles[N].Text.CustomPosition.X.ToString());
                    XMLwrt.WriteAttributeString("Y", Lan.Subtitles[N].Text.CustomPosition.Y.ToString());
                    XMLwrt.WriteEndElement();//CustomLocation End
                    XMLwrt.WriteEndElement();//Position End
                    //fonts and colors
                    XMLwrt.WriteStartElement("FontsAndColors");
                    List<Font> fonts = new List<Font>(SubtitleTextWrapper.GetFonts(Lan.Subtitles[N].Text));
                    for (int f = 0; f < fonts.Count; f++)
                    {
                        XMLwrt.WriteStartElement("Font");
                        XMLwrt.WriteAttributeString("ID", "f" + (f + 1).ToString());//this is the assign
                        XMLwrt.WriteAttributeString("Name", fonts[f].Name);
                        XMLwrt.WriteAttributeString("Size", fonts[f].Size.ToString());
                        XMLwrt.WriteAttributeString("Style", fonts[f].Style.ToString());
                        XMLwrt.WriteEndElement();
                    }
                    List<Color> colors = new List<Color>(SubtitleTextWrapper.GetColors(Lan.Subtitles[N].Text));
                    for (int c = 0; c < colors.Count; c++)
                    {
                        XMLwrt.WriteStartElement("Color");
                        XMLwrt.WriteAttributeString("ID", "c" + (c + 1).ToString());//this is the assign
                        XMLwrt.WriteAttributeString("ARGB", colors[c].ToArgb().ToString());
                        XMLwrt.WriteEndElement();
                    }
                    XMLwrt.WriteEndElement();//FontsAndColors End
                    //the text !!
                    XMLwrt.WriteStartElement("Text");
                    string text = "";
                    int usedFontIndex = -1;
                    int usedColorIndex = -1;
                    foreach (SubtitleLine line in Lan.Subtitles[N].Text.TextLines)
                    {
                        text += "[A:" + line.Alignement.ToString() + "]";
                        foreach (SubtitleChar chr in line.Chars)
                        {
                            //font and color
                            int fIndex = fonts.IndexOf(chr.Font);
                            int cIndex = colors.IndexOf(chr.Color);
                            if (fIndex != usedFontIndex || cIndex != usedColorIndex)
                            {
                                usedFontIndex = fIndex;
                                usedColorIndex = cIndex;
                                text += "[" +
                                    ("f" + (fIndex + 1).ToString()) + "," +
                                    ("c" + (cIndex + 1).ToString()) +
                                    "]";
                            }
                            //the char
                            text += chr.TheChar.ToString();
                        }
                    }
                    XMLwrt.WriteString(text);
                    XMLwrt.WriteEndElement();//Text End

                    XMLwrt.WriteEndElement();//Sub End

                    int x = (100 * N) / Lan.Subtitles.Count;
                    if (Progress != null)
                        Progress(this, new ProgressArgs(x, "Saving ...."));
                }
                XMLwrt.WriteEndElement();//Language End
            }
            XMLwrt.WriteEndElement();//header end
            XMLwrt.Flush();
            XMLwrt.Close();
            if (Progress != null)
                Progress(this, new ProgressArgs(100, "Save Completed."));
            if (SaveFinished != null)
                SaveFinished(this, new EventArgs());
        }
        public override bool IsMultiTrack
        {
            get
            {
                return true;
            }
        }
        public override event EventHandler<ASMP.ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
