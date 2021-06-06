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
    public class OpenDVT : SubtitlesFormat
    {
        public override string Name
        {
            get { return "Open DVT (*.xml)"; }
        }

        public override string Description
        {
            get { return "Open DVT\n\nThis format doesn't support 'End time' so it will save the 'Start time' only."; }
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
                        if (XMLread.Name == "OpenDVT")
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
                    if (XMLread.Name == "Line" & XMLread.IsStartElement())//subtitle !!
                    {
                        Subtitle sub = new Subtitle();
                        this.SubtitleTrack.Subtitles.Add(sub);
                    }
                    if (XMLread.Name == "TimeMs")
                    {
                        double time = double.Parse(XMLread.ReadString());
                        time /= 1000;
                        this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].StartTime = time;
                    }
                    if (XMLread.Name == "Text")
                    {
                        this.SubtitleTrack.Subtitles[this.SubtitleTrack.Subtitles.Count - 1].Text = SubtitleText.FromString(XMLread.ReadString());
                    }
                }
                XMLread.Close();

                for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
                {
                    if (i + 1 != this.SubtitleTrack.Subtitles.Count)
                        this.SubtitleTrack.Subtitles[i].EndTime = this.SubtitleTrack.Subtitles[i + 1].StartTime - 0.001;
                    else
                        this.SubtitleTrack.Subtitles[i].EndTime = this.SubtitleTrack.Subtitles[i].StartTime + 2;
                }
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

            XMLwrt.WriteStartElement("OpenDVT");//header
            XMLwrt.WriteAttributeString("UUID", "{00000000-0000-0000-0000-000000000000");
            XMLwrt.WriteAttributeString("ShortID", "Final Cut Pro Xml");
            XMLwrt.WriteAttributeString("Type", "Deposition");
            XMLwrt.WriteAttributeString("Version", "1.3");

            XMLwrt.WriteStartElement("Information");//Information

            XMLwrt.WriteStartElement("Origination");//Origination
            XMLwrt.WriteStartElement("ID");//ID
            XMLwrt.WriteString("00000000-0000-0000-0000-000000000000");
            XMLwrt.WriteEndElement();//ID
            XMLwrt.WriteStartElement("AppName");//AppName
            XMLwrt.WriteString("AHD Subtitles Maker");
            XMLwrt.WriteEndElement();//AppName
            XMLwrt.WriteStartElement("AppVersion");//AppVersion
            XMLwrt.WriteString("4.0");
            XMLwrt.WriteEndElement();//AppVersion
            XMLwrt.WriteStartElement("VendorName");//VendorName
            XMLwrt.WriteString("AHD");
            XMLwrt.WriteEndElement();//VendorName
            XMLwrt.WriteStartElement("VendorPhone");//VendorPhone 
            XMLwrt.WriteEndElement();//VendorPhone 
            XMLwrt.WriteStartElement("VendorURL");//VendorURL
            XMLwrt.WriteString("https://sourceforge.net/projects/ahdsubtitles/");
            XMLwrt.WriteEndElement();//VendorURL
            XMLwrt.WriteEndElement();//Origination

            XMLwrt.WriteStartElement("Case");//Case
            XMLwrt.WriteStartElement("MatterNumber");//MatterNumber 
            XMLwrt.WriteEndElement();//MatterNumber 
            XMLwrt.WriteEndElement();//Case

            XMLwrt.WriteStartElement("Deponent");//Deponent
            XMLwrt.WriteStartElement("FirstName");//FirstName 
            XMLwrt.WriteEndElement();//FirstName 
            XMLwrt.WriteStartElement("LastName");//LastName 
            XMLwrt.WriteEndElement();//LastName 
            XMLwrt.WriteEndElement();//Deponent

            XMLwrt.WriteStartElement("ReportingFirm");//ReportingFirm
            XMLwrt.WriteStartElement("Name");//Name 
            XMLwrt.WriteEndElement();//Name 
            XMLwrt.WriteEndElement();//ReportingFirm

            XMLwrt.WriteStartElement("FirstPageNo");//FirstPageNo
            XMLwrt.WriteString("1");
            XMLwrt.WriteEndElement();//FirstPageNo

            XMLwrt.WriteStartElement("LastPageNo");//LastPageNo
            XMLwrt.WriteString("3");
            XMLwrt.WriteEndElement();//LastPageNo

            XMLwrt.WriteStartElement("MaxLinesPerPage");//MaxLinesPerPage
            XMLwrt.WriteString("25");
            XMLwrt.WriteEndElement();//MaxLinesPerPage

            XMLwrt.WriteStartElement("Volume");//Volume
            XMLwrt.WriteString("1");
            XMLwrt.WriteEndElement();//Volume

            XMLwrt.WriteStartElement("TakenOn");//TakenOn
            XMLwrt.WriteString("10/04/2011");
            XMLwrt.WriteEndElement();//TakenOn

            XMLwrt.WriteStartElement("TranscriptVerify");//TranscriptVerify  
            XMLwrt.WriteEndElement();//TranscriptVerify  

            XMLwrt.WriteStartElement("PrintVerify");//PrintVerify   
            XMLwrt.WriteEndElement();//PrintVerify   

            XMLwrt.WriteEndElement();//Information

            XMLwrt.WriteStartElement("Lines");//Lines   
            XMLwrt.WriteAttributeString("Count", this.SubtitleTrack.Subtitles.Count.ToString());
            for (int i = 0; i < this.SubtitleTrack.Subtitles.Count; i++)
            {
                XMLwrt.WriteStartElement("Line");//Line 
                XMLwrt.WriteAttributeString("ID", i.ToString());

                XMLwrt.WriteStartElement("Stream");//Stream
                XMLwrt.WriteString("0");
                XMLwrt.WriteEndElement();//Stream

                XMLwrt.WriteStartElement("TimeMs");//TimeMs
                XMLwrt.WriteString(((int)(this.SubtitleTrack.Subtitles[i].StartTime * 1000)).ToString());
                XMLwrt.WriteEndElement();//TimeMs

                XMLwrt.WriteStartElement("PageNo");//PageNo
                XMLwrt.WriteString("1");
                XMLwrt.WriteEndElement();//PageNo

                XMLwrt.WriteStartElement("LineNo");//LineNo
                XMLwrt.WriteString("1");
                XMLwrt.WriteEndElement();//LineNo

                XMLwrt.WriteStartElement("QA");//QA
                XMLwrt.WriteString("-");
                XMLwrt.WriteEndElement();//QA

                XMLwrt.WriteStartElement("Text");//Text
                XMLwrt.WriteString(this.SubtitleTrack.Subtitles[i].Text.ToString());
                XMLwrt.WriteEndElement();//Text

                XMLwrt.WriteEndElement();//Line 

                int x = (100 * i) / this.SubtitleTrack.Subtitles.Count;
                if (Progress != null)
                    Progress(this, new ProgressArgs(x, "Saving ...."));
            }
            XMLwrt.WriteEndElement();//Lines   

            XMLwrt.WriteStartElement("Streams");//Streams    
            XMLwrt.WriteAttributeString("Count", "0");

            XMLwrt.WriteStartElement("Stream");//Stream
            XMLwrt.WriteAttributeString("ID", "0");
            XMLwrt.WriteEndElement();//Stream

            XMLwrt.WriteEndElement();//Streams    

            XMLwrt.WriteEndElement();//OpenDVT
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
    }
}
