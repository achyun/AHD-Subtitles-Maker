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
using AHD.SM.ASMP;

namespace AHD.SM.Formats
{
    public class ABCiView : SubtitlesFormat
    {
        public ABCiView()
        {
            FrameRate = 25;
        }
        public string _movie = "program title";
        public string _language = "English (UK)";
        public string _font = "Arial";
        public string _style = "Regular";
        public double _size = 10;

        public override string Name
        {
            get { return "ABCiView (*.xml)"; }
        }
        public override string Description
        {
            get { return "ABC iView"; }
        }
        public override string[] Extensions
        {
            get
            {
                string[] Exs = { ".xml" };
                return Exs;
            }
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
                        if (XMLread.Name == "root")
                        {
                            XMLread.Close();
                            return true;
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
            this.SubtitleTrack = new SubtitlesTrack();
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
                    if (XMLread.Name == "root")
                    {
                        XMLread.MoveToAttribute("fps");
                        FrameRate = double.Parse(XMLread.Value);
                        XMLread.MoveToAttribute("font");
                        _font = XMLread.Value;
                        XMLread.MoveToAttribute("style");
                        _style = XMLread.Value;
                        XMLread.MoveToAttribute("size");
                        _size = double.Parse(XMLread.Value);
                    }
                    if (XMLread.Name == "title")//subtitle !!
                    {
                        Subtitle sub = new Subtitle();
                        XMLread.MoveToAttribute("start");
                        sub.StartTime = TimeFormatConvertor.From_TimeSpan_Milli(XMLread.Value);
                        XMLread.MoveToAttribute("end");
                        sub.EndTime = TimeFormatConvertor.From_TimeSpan_Milli(XMLread.Value);

                        string txt = XMLread.ReadString().Replace("|", "\n");
                        sub.Text = SubtitleText.FromString(txt, new System.Drawing.Font("Tahoma", 8, System.Drawing.FontStyle.Regular), System.Drawing.Color.White);

                        this.SubtitleTrack.Subtitles.Add(sub);
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
            this.FilePath = filePath;
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            sett.Encoding = encoding;
            XmlWriter XMLwrt = XmlWriter.Create(FilePath, sett);
            XMLwrt.WriteStartElement("root");//header
            XMLwrt.WriteAttributeString("fps", FrameRate.ToString());
            XMLwrt.WriteAttributeString("movie", _movie);
            XMLwrt.WriteAttributeString("language", "GBR:" + _language);
            XMLwrt.WriteAttributeString("font", _font);
            XMLwrt.WriteAttributeString("style", _style);
            XMLwrt.WriteAttributeString("size", _size.ToString());

            XMLwrt.WriteStartElement("reel");//reel ?
            XMLwrt.WriteAttributeString("start", "");
            XMLwrt.WriteAttributeString("first", "");
            XMLwrt.WriteAttributeString("last", "");
            XMLwrt.WriteEndElement();//reel

            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                XMLwrt.WriteStartElement("title");//title
                XMLwrt.WriteAttributeString("start", TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].StartTime, ":", MillisecondLength.N3));
                XMLwrt.WriteAttributeString("end", TimeFormatConvertor.To_TimeSpan_Milli(this.SubtitleTrack.Subtitles[i].EndTime, ":", MillisecondLength.N3));

                string txt = this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", "|");
                XMLwrt.WriteString(txt);

                XMLwrt.WriteEndElement();//title

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }

            XMLwrt.WriteEndElement();//root
            XMLwrt.Flush();
            XMLwrt.Close();
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
                return new Cl_ABCiView(this);
            }
        }
        public override event EventHandler<ASMP.ProgressArgs> Progress;
        public override event EventHandler LoadStarted;
        public override event EventHandler LoadFinished;
        public override event EventHandler SaveStarted;
        public override event EventHandler SaveFinished;
    }
}
