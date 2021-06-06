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
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml;

namespace AHD.SubtitlesMakerProfessional.Base
{
    public class ProjectManager
    {
        Project _Project = new Project();
        string _FilePath = "";
        /// <summary>
        /// Get or set the project
        /// </summary>
        public Project Project
        { get { return _Project; } set { _Project = value; } }
        /// <summary>
        /// Get or set the file path
        /// </summary>
        public string FilePath
        { get { return _FilePath; } set { _FilePath = value; } }
        /// <summary>
        /// Save the project into a file
        /// </summary>
        /// <param name="FilePath">The file path to save into</param>
        /// <returns>True if the project saved well, otherwise false.</returns>
        public bool SaveProject(string FilePath)
        {
            try
            {
                FileStream fs = new FileStream(FilePath, FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, _Project);
                fs.Close();
                _FilePath = FilePath;
                if (ProjectSaved != null)
                    ProjectSaved(this, null);
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// Load a project file
        /// </summary>
        /// <param name="FilePath">The project file path</param>
        /// <returns>True if the project loaded well, otherwise false.</returns>
        public bool LoadProject(string FilePath)
        {
            //Try the version 3.0
            try
            {
                if (File.Exists(FilePath))
                {
                    FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                    BinaryFormatter formatter = new BinaryFormatter();
                    _Project = (Project)formatter.Deserialize(fs);
                    fs.Close();
                    _FilePath = FilePath;
                    return true;
                }
            }
            catch { }
            //Lets try the version 1.0 or 1.1
            try
            {
                if (!File.Exists(FilePath))
                { return false; }
                XmlReader XMLread = XmlReader.Create(FilePath);
                //Clear lists
                _Project.Marks.Clear();
                _Project.SubtitleTracks.Clear();
                XMLread.Read();//Reads the XML definition <XML>
                XMLread.Read();//Reads the header
                //check header
                if (XMLread.Name != "AHDSubtitlesMakerProject")
                { return false; }
                if (!XMLread.HasAttributes)
                { return false; }
                //read ....
                while (XMLread.Read())
                {
                    if (XMLread.Name == "Name")
                    {
                        XMLread.MoveToAttribute("ID");
                        _Project.Name = XMLread.Value.ToString();
                    }
                    if (XMLread.Name == "MediaFile")
                    {
                        XMLread.MoveToAttribute("ID");
                        _Project.MediaPath = XMLread.Value.ToString();
                    }
                    //read language tracks
                    if (XMLread.Name == "Language")
                    {
                        SubtitlesTrack Lan = new SubtitlesTrack();
                        XMLread.MoveToAttribute("Name");
                        if (XMLread.Value.ToString() == "")
                        { continue; }
                        Lan.Name = XMLread.Value.ToString();
                        _Project.SubtitleTracks.Add(Lan);
                        while (XMLread.Read())
                        {
                            if (!XMLread.IsStartElement() & XMLread.Name == "Language")
                            { break; }
                            if (XMLread.Name == "Sub")
                            {
                                Subtitle Sub = new Subtitle();
                                XMLread.MoveToAttribute("Start");
                                Sub.StartTime = Convert.ToDouble(XMLread.Value.ToString());
                                XMLread.MoveToAttribute("End");
                                Sub.EndTime = Convert.ToDouble(XMLread.Value.ToString());
                                XMLread.MoveToAttribute("Text");
                                Sub.Text = XMLread.Value.ToString();
                                Lan.Subtitles.Add(Sub);
                            }
                        }
                    }
                    if (XMLread.Name == "Mark")
                    {
                        Mark Ma = new Mark();
                        XMLread.MoveToAttribute("Time");
                        Ma.Time = (Convert.ToDouble(XMLread.Value.ToString()));
                        XMLread.MoveToAttribute("Name");
                        Ma.Name = XMLread.Value.ToString();
                        _Project.Marks.Add(Ma);
                    }
                }
                XMLread.Close();
                _FilePath = FilePath;
                return true;
            }
            catch { }
            return false;
        }
        /// <summary>
        /// Rised when the project saved well
        /// </summary>
        public event EventHandler<EventArgs> ProjectSaved;
    }
}