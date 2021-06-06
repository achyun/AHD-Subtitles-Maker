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
using System.Drawing;
using AHD.SM.ASMP;
namespace AHD.SM.Formats
{
    public class AutodeskSmoke : SubtitlesFormat
    {
        /// <summary>
        /// Name
        /// </summary>
        public string _Name = "SampleSubtitles";
        /// <summary>
        /// FrameRate
        /// </summary>
        public string _FrameRate = "30";
        /// <summary>
        /// RealFrameRate
        /// </summary>
        public double _RealFrameRate = 30;
        /// <summary>
        /// Depth
        /// </summary>
        public string _Depth = "8";
        /// <summary>
        /// aspect
        /// </summary>
        public string _aspect = "default";
        /// <summary>
        /// scanformat
        /// </summary>
        public string _scanformat = "default";
        /// <summary>
        /// DropFrame
        /// </summary>
        public bool _DropFrame = false;
        /// <summary>
        /// setup
        /// </summary>
        public string _setup = "";

        public override string Name
        {
            get { return "Autodesk Smoke's Subtitle XML (*.xml)"; }
        }
        public override string Description
        {
            get { return "Subtitle XML for AutodeskSmoke\n\n"; }
        }
        public override string[] Extensions
        {
            get { return new string[] { ".xml" }; }
        }
        public override bool CheckFile(string filePath, Encoding encoding)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    XmlReaderSettings sett = new XmlReaderSettings();
                    sett.DtdProcessing = DtdProcessing.Ignore;
                    XmlReader XMLread = XmlReader.Create(filePath, sett);
                    while (XMLread.Read())
                    {
                        //check the header
                        if (XMLread.Name == "subtitle")
                        {
                            XMLread.MoveToAttribute("version");
                            if (XMLread.Value == "1")
                            {
                                XMLread.Close();
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return false;
        }
        public override void Load(string filePath, Encoding encoding)
        {
            if (LoadStarted != null)
                LoadStarted(this, new EventArgs());
            this.FilePath = filePath;
            this.SubtitleTrack = new SubtitlesTrack("Imported");
            try
            {
                XmlReaderSettings sett = new XmlReaderSettings();
                sett.DtdProcessing = DtdProcessing.Ignore;
                sett.IgnoreWhitespace = true;
                XmlReader XMLread = XmlReader.Create(FilePath, sett);
                XMLread.Read();//Reads the XML definition <XML>
                XMLread.Read();//Reads the header
                while (XMLread.Read())
                {
                    if (XMLread.Name == "rate")
                    {
                        switch (XMLread.ReadString())
                        {
                            case "23.976": _RealFrameRate = 23.976; break;
                            case "24": _RealFrameRate = 24; break;
                            case "25": _RealFrameRate = 25; break;
                            case "29.97 DF": _RealFrameRate = 29.97; break;
                            case "29.97 NDF": _RealFrameRate = 29.97; break;
                            case "30": _RealFrameRate = 30; break;
                            case "59.94 DF": _RealFrameRate = 59.94; break;
                            case "59.94 NDF": _RealFrameRate = 59.94; break;
                        }
                    }
                    if (XMLread.Name == "title")//subtitle !!
                    {
                        Subtitle sub = new Subtitle();
                        string text = "";
                        string _fontName = "Tahoma";
                        float _fontSize = 8;
                        byte A = 0;
                        byte B = 0;
                        byte R = 0;
                        byte G = 0;
                        int x = 0;
                        int y = 0;
                        while (XMLread.Read())
                        {
                            if (XMLread.Name == "start")
                            {
                                sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(XMLread.ReadString(), _RealFrameRate);
                            }
                            if (XMLread.Name == "end")
                            {
                                sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(XMLread.ReadString(), _RealFrameRate);
                            }
                            if (XMLread.Name == "text")
                            {
                                text = XMLread.ReadString();
                            }
                            if (XMLread.Name == "font")
                            {
                                _fontName = XMLread.ReadString();
                            }
                            if (XMLread.Name == "size")
                            {
                                _fontSize = float.Parse(XMLread.ReadString());
                            }
                            if (XMLread.Name == "alpha")
                            {
                                A = Convert.ToByte(XMLread.ReadString());
                            }
                            if (XMLread.Name == "red")
                            {
                                R = Convert.ToByte(XMLread.ReadString());
                            }
                            if (XMLread.Name == "green")
                            {
                                G = Convert.ToByte(XMLread.ReadString());
                            }
                            if (XMLread.Name == "blue")
                            {
                                B = Convert.ToByte(XMLread.ReadString());
                                sub.Text = SubtitleText.FromString(text, new System.Drawing.Font(_fontName, _fontSize,
                                    System.Drawing.FontStyle.Regular), Color.FromArgb(A, R, G, B));
                            }
                            if (XMLread.Name == "vertical")
                            {
                                y = Convert.ToInt32(XMLread.ReadString());
                            }
                            if (XMLread.Name == "horizontal")
                            {
                                x = Convert.ToInt32(XMLread.ReadString());
                                sub.Text.CustomPosition = new Point(x, y);
                                this.SubtitleTrack.Subtitles.Add(sub);
                                break;
                            }
                        }
                    }
                }
                XMLread.Close();
            }
            catch { }
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
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            sett.Encoding = encoding;
            XmlWriter XMLwrt = XmlWriter.Create(FilePath, sett);
            XMLwrt.WriteComment("  Copyright 2006, Autodesk, Inc. ");
            XMLwrt.WriteComment("  Titles created by AHD Subtitles Maker ");
            XMLwrt.WriteStartElement("subtitle");//header
            XMLwrt.WriteAttributeString("version", "1");//header version
            //write the name
            XMLwrt.WriteStartElement("name");
            XMLwrt.WriteString(_Name);
            XMLwrt.WriteEndElement();
            //write frame rate
            XMLwrt.WriteStartElement("rate");
            XMLwrt.WriteString(_FrameRate);
            XMLwrt.WriteEndElement();
            //write resolution
            XMLwrt.WriteStartElement("resolution");
            //   width
            XMLwrt.WriteStartElement("width");
            XMLwrt.WriteString("default");
            XMLwrt.WriteEndElement();
            //   height
            XMLwrt.WriteStartElement("height");
            XMLwrt.WriteString("default");
            XMLwrt.WriteEndElement();
            //   depth
            XMLwrt.WriteStartElement("depth");
            XMLwrt.WriteString(_Depth);
            XMLwrt.WriteEndElement();
            //   aspect
            XMLwrt.WriteStartElement("aspect");
            XMLwrt.WriteString(_aspect);
            XMLwrt.WriteEndElement();
            //   aspect
            XMLwrt.WriteStartElement("scanformat");
            XMLwrt.WriteString(_scanformat);
            XMLwrt.WriteEndElement();
            XMLwrt.WriteEndElement();//resolution end
            //write video
            XMLwrt.WriteStartElement("video");
            //   write subtitles as 'titles'
            int i = 0;
            foreach (Subtitle sub in this.SubtitleTrack.Subtitles)
            {
                XMLwrt.WriteStartElement("title");
                //start
                XMLwrt.WriteStartElement("start");
                XMLwrt.WriteString(TimeFormatConvertor.To_TimeSpan_Frame(sub.StartTime, _RealFrameRate, _DropFrame ? ";" : ":"));
                XMLwrt.WriteEndElement();
                //end
                XMLwrt.WriteStartElement("end");
                XMLwrt.WriteString(TimeFormatConvertor.To_TimeSpan_Frame(sub.EndTime, _RealFrameRate, _DropFrame ? ";" : ":"));
                XMLwrt.WriteEndElement();
                //text
                XMLwrt.WriteStartElement("text");
                XMLwrt.WriteString(sub.Text.ToString());
                XMLwrt.WriteEndElement();
                //font
                XMLwrt.WriteStartElement("font");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Font.Name);
                XMLwrt.WriteEndElement();
                //font size
                XMLwrt.WriteStartElement("size");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Font.Size.ToString());
                XMLwrt.WriteEndElement();
                //fontcolor
                XMLwrt.WriteStartElement("fontcolor");
                //     alpha
                XMLwrt.WriteStartElement("alpha");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Color.A.ToString());
                XMLwrt.WriteEndElement();
                //     red
                XMLwrt.WriteStartElement("red");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Color.R.ToString());
                XMLwrt.WriteEndElement();
                //     green
                XMLwrt.WriteStartElement("green");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Color.G.ToString());
                XMLwrt.WriteEndElement();
                //     blue
                XMLwrt.WriteStartElement("blue");
                XMLwrt.WriteString(sub.Text.TextLines[0].Chars[0].Color.B.ToString());
                XMLwrt.WriteEndElement();
                XMLwrt.WriteEndElement();//fontcolor end
                //vertical
                XMLwrt.WriteStartElement("vertical");
                XMLwrt.WriteString(sub.Text.CustomPosition.Y.ToString());
                XMLwrt.WriteEndElement();
                //horizontal
                XMLwrt.WriteStartElement("horizontal");
                XMLwrt.WriteString(sub.Text.CustomPosition.X.ToString());
                XMLwrt.WriteEndElement();
                //setup
                XMLwrt.WriteStartElement("setup");
                XMLwrt.WriteString(_setup);
                XMLwrt.WriteEndElement();
                XMLwrt.WriteEndElement();//title end

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
                i++;
            }
            XMLwrt.WriteEndElement();//video end
            //the end
            XMLwrt.WriteEndElement();//header end
            XMLwrt.Flush();
            XMLwrt.Close();
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
                return new cl_AutodeskSmoke(this);
            }
        }
        public override event EventHandler<ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
