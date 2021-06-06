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
    public class SubtitleEditorProject : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Subtitle Editor Project (*.xml)"; }
        }

        public override string Description
        {
            get { return "Subtitle Editor Project"; }
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
                        if (XMLread.Name == "SubtitleEditorProject")
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
                    if (XMLread.Name == "subtitles")
                    {
                        XMLread.MoveToAttribute("framerate");
                        FrameRate = double.Parse(XMLread.Value);
                    }
                    if (XMLread.Name == "subtitle")//subtitle !!
                    {
                        Subtitle sub = new Subtitle();
                        XMLread.MoveToAttribute("end");
                        sub.EndTime = double.Parse(XMLread.Value) / 1000;
                        XMLread.MoveToAttribute("start");
                        sub.StartTime = double.Parse(XMLread.Value) / 1000;
                        XMLread.MoveToAttribute("text");
                        sub.Text =SubtitleText.FromString( XMLread.Value);
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
            this.FilePath = filePath;
            if (SaveStarted != null)
                SaveStarted(this, new EventArgs());

            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            sett.Encoding = encoding;
            XmlWriter XMLwrt = XmlWriter.Create(FilePath, sett);
            XMLwrt.WriteStartElement("SubtitleEditorProject");//SubtitleEditorProject 
            XMLwrt.WriteAttributeString("version", "1.0");

            XMLwrt.WriteStartElement("player");//player 
            XMLwrt.WriteEndElement();//player 
            XMLwrt.WriteStartElement("waveform");//waveform 
            XMLwrt.WriteEndElement();//waveform 
            XMLwrt.WriteStartElement("styles");//styles  
            XMLwrt.WriteEndElement();//styles  

            XMLwrt.WriteStartElement("subtitles");//subtitles 
            XMLwrt.WriteAttributeString("timing_mode", "TIME");
            XMLwrt.WriteAttributeString("edit_timing_mode", "TIME");
            XMLwrt.WriteAttributeString("framerate", FrameRate.ToString());

            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                XMLwrt.WriteStartElement("subtitle");//subtitle
                XMLwrt.WriteAttributeString("duration", ((int)(this.SubtitleTrack.Subtitles[i].Duration * 1000)).ToString());
                XMLwrt.WriteAttributeString("effect", "");
                XMLwrt.WriteAttributeString("end", ((int)(this.SubtitleTrack.Subtitles[i].EndTime * 1000)).ToString());
                XMLwrt.WriteAttributeString("layer", "0");
                XMLwrt.WriteAttributeString("margin-l", "0");
                XMLwrt.WriteAttributeString("margin-r", "0");
                XMLwrt.WriteAttributeString("margin-v", "0");
                XMLwrt.WriteAttributeString("name", "");
                XMLwrt.WriteAttributeString("note", "");
                XMLwrt.WriteAttributeString("path", "0");
                XMLwrt.WriteAttributeString("start", ((int)(this.SubtitleTrack.Subtitles[i].StartTime * 1000)).ToString());
                XMLwrt.WriteAttributeString("style", "Default");
                XMLwrt.WriteAttributeString("text", this.SubtitleTrack.Subtitles[i].Text.ToString().Replace("\n", " "));
                XMLwrt.WriteAttributeString("translation", "");
                XMLwrt.WriteEndElement();//subtitle

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }

            XMLwrt.WriteEndElement();//subtitles 
            XMLwrt.WriteStartElement("subtitles-selection");//subtitles-selection  
            XMLwrt.WriteEndElement();//subtitles-selection  
            XMLwrt.WriteEndElement();//SubtitleEditorProject 
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
            get
            {
                double[] frms = { 23.976, 24, 25, 29.970, 30 }; return frms; 
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
                return new Cl_frameRate(this);
            }
        }
    }
}
