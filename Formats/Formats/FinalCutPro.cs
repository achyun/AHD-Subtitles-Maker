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
    public class FinalCutPro : SubtitlesFormat
    {
        public FinalCutPro()
        {
            FrameRate = 25;
        }
        public override string Name
        {
            get { return "Final Cut Pro (*.xml)"; }
        }

        public override string Description
        {
            get { return "Final Cut Pro\n\n"; }
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
                        if (XMLread.Name == "xmeml")
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
                    if (XMLread.Name == "timebase")//subtitle ?
                    {
                        FrameRate = Convert.ToDouble(XMLread.ReadString());
                        Subtitle sub = new Subtitle();
                        while (XMLread.Read())
                        {
                            if (XMLread.Name == "start")
                            {
                                sub.StartTime = TimeFormatConvertor.From_TimeSpan_Frame(XMLread.ReadString(), FrameRate);
                            }
                            if (XMLread.Name == "end")
                            {
                                sub.EndTime = TimeFormatConvertor.From_TimeSpan_Frame(XMLread.ReadString(), FrameRate);
                            }
                            if (XMLread.Name == "value")
                            {
                                sub.Text = SubtitleText.FromString(XMLread.ReadString());
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
            this.FilePath = filePath;
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            sett.Encoding = encoding;
            XmlWriter XMLwrt = XmlWriter.Create(FilePath, sett);

            XMLwrt.WriteStartElement("xmeml");//header
            XMLwrt.WriteAttributeString("version", "3");

            XMLwrt.WriteStartElement("sequence");//sequence
            XMLwrt.WriteAttributeString("id", "Subtitles");

            XMLwrt.WriteStartElement("name");//name
            XMLwrt.WriteString("Subtitles");
            XMLwrt.WriteEndElement();//name
            XMLwrt.WriteStartElement("media");//media
            XMLwrt.WriteStartElement("video");//video

            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                XMLwrt.WriteStartElement("track");//track
                XMLwrt.WriteStartElement("generatoritem");//generatoritem
                XMLwrt.WriteStartElement("name");//name
                XMLwrt.WriteString("Text");
                XMLwrt.WriteEndElement();//name

                XMLwrt.WriteStartElement("rate");//rate
                XMLwrt.WriteStartElement("ntsc");//ntsc
                XMLwrt.WriteString(FrameRate == 29.970 ? "TRUE" : "FALSE");
                XMLwrt.WriteEndElement();//ntsc
                XMLwrt.WriteStartElement("timebase");//timebase
                XMLwrt.WriteString(FrameRate.ToString());
                XMLwrt.WriteEndElement();//timebase
                XMLwrt.WriteEndElement();//rate

                XMLwrt.WriteStartElement("start");//start
                XMLwrt.WriteString(TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].StartTime, FrameRate, ":"));
                XMLwrt.WriteEndElement();//start
                XMLwrt.WriteStartElement("end");//end
                XMLwrt.WriteString(TimeFormatConvertor.To_TimeSpan_Frame(this.SubtitleTrack.Subtitles[i].EndTime, FrameRate, ":"));
                XMLwrt.WriteEndElement();//end
                XMLwrt.WriteStartElement("enabled");//enabled
                XMLwrt.WriteString("TRUE");
                XMLwrt.WriteEndElement();//enabled
                XMLwrt.WriteStartElement("anamorphic");//anamorphic
                XMLwrt.WriteString("FALSE");
                XMLwrt.WriteEndElement();//anamorphic
                XMLwrt.WriteStartElement("alphatype");//alphatype
                XMLwrt.WriteString("black");
                XMLwrt.WriteEndElement();//alphatype

                XMLwrt.WriteStartElement("effect");//effect
                XMLwrt.WriteAttributeString("id", (i + 1).ToString());

                XMLwrt.WriteStartElement("name");//name
                XMLwrt.WriteString("Text");
                XMLwrt.WriteEndElement();//name
                XMLwrt.WriteStartElement("effectcategory");//effectcategory
                XMLwrt.WriteString("Text");
                XMLwrt.WriteEndElement();//effectcategory
                XMLwrt.WriteStartElement("effecttype");//effecttype
                XMLwrt.WriteString("generator");
                XMLwrt.WriteEndElement();//effecttype
                XMLwrt.WriteStartElement("mediatype");//mediatype
                XMLwrt.WriteString("video");
                XMLwrt.WriteEndElement();//mediatype

                XMLwrt.WriteStartElement("parameter");//parameter

                XMLwrt.WriteStartElement("parameterid");//parameterid
                XMLwrt.WriteString("str");
                XMLwrt.WriteEndElement();//parameterid

                XMLwrt.WriteStartElement("name");//name
                XMLwrt.WriteString("Text");
                XMLwrt.WriteEndElement();//name

                XMLwrt.WriteStartElement("value");//value
                XMLwrt.WriteString(this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " "));
                XMLwrt.WriteEndElement();//value

                XMLwrt.WriteEndElement();//parameter
                XMLwrt.WriteEndElement();//effect
                XMLwrt.WriteEndElement();//generatoritem
                XMLwrt.WriteEndElement();//track

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }

            XMLwrt.WriteEndElement();//video
            XMLwrt.WriteEndElement();//media
            XMLwrt.WriteEndElement();//sequence
            XMLwrt.WriteEndElement();//header

            XMLwrt.Flush();
            XMLwrt.Close();
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
        public override double[] FrameRates
        {
            get { double[] frms = { 23.976, 24, 25, 29.970, 30 }; return frms; }
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
                return new Cl_frameRate(this);
            }
        }
    }
}
