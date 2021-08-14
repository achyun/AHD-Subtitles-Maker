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
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Project manager, a class handles save/load projects
    /// </summary>
    public class ProjectManager
    {
        string filePath;
        Project project;
        ResourceManager resources = new ResourceManager("AHD.SM.ASMP.LanguageResources.Resource",
          Assembly.GetExecutingAssembly());

        /// <summary>
        /// Get or set the file path of the project
        /// </summary>
        public string FilePath
        { get { return filePath; } set { filePath = value; } }
        /// <summary>
        /// Get or set the project
        /// </summary>
        public Project Project
        { get { return project; } set { project = value; } }

        /// <summary>
        /// Rised when the project saved successfuly
        /// </summary>
        public event EventHandler ProjectSaved;
        /// <summary>
        /// Rised when error occur when trying to save
        /// </summary>
        public event EventHandler<ErrorEventArgs> ProjectDidntSave;
        /// <summary>
        /// Rised when the project loaded successfuly
        /// </summary>
        public event EventHandler ProjectLoaded;
        /// <summary>
        /// Rised when error occur when trying to load
        /// </summary>
        public event EventHandler<ErrorEventArgs> ProjectDidntLoad;

        /// <summary>
        /// Save the project at "FilePath" property path
        /// </summary>
        /// <returns>True if saved successfuly, False if not</returns>
        public bool Save()
        {
            return Save(filePath);
        }
        /// <summary>
        /// Save the project into file, this will assign the "FilePath" property to the given path when save success
        /// </summary>
        /// <param name="FileName">The file path where to save</param>
        /// <returns>True if saved successfuly, False if not</returns>
        public bool Save(string FileName)
        {
            try
            {
                Stream str = new FileStream(FileName, FileMode.Create, FileAccess.Write);
                BinaryFormatter formater = new BinaryFormatter();
                formater.Serialize(str, project);
                str.Close();
                filePath = FileName;
                if (ProjectSaved != null)
                    ProjectSaved(this, new EventArgs());
                return true;
            }
            catch (Exception ex)
            {
                if (ProjectDidntSave != null)
                    ProjectDidntSave(this, new ErrorEventArgs(ex));
                return false;
            }
        }
        /// <summary>
        /// Load the project at "FilePath" property path
        /// </summary>
        /// <returns>True if loaded successfuly, False if not</returns>
        public bool Load()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(resources.GetString("Message_CantFindTheFileAt") + " " + filePath);
            return Load(filePath, true);
        }
        /// <summary>
        /// Load a project from file, this will assign the "FilePath" property to the given path when load success
        /// </summary>
        /// <param name="FileName">The project file path</param>
        /// <returns>True if loaded successfuly, False if not</returns>
        public bool Load(string FileName)
        { return Load(FileName, true); }
        /// <summary>
        /// Load a project from file, this will assign the "FilePath" property to the given path when load success
        /// </summary>
        /// <param name="FileName">The project file path</param>
        /// <param name="riseLoadEvent">If true, rise the ProjectLoaded event when project loaded success</param>
        /// <returns>True if loaded successfuly, False if not</returns>
        public bool Load(string FileName, bool riseLoadEvent)
        {
            try
            {
                Stream str = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter formater = new BinaryFormatter();
                object openedProject = formater.Deserialize(str);
                if (openedProject.GetType() == typeof(AHD.SM.ASMP.Project))
                {
                    project = (Project)openedProject;
                    str.Close();
                    str.Dispose();
                    filePath = FileName;
                    if (riseLoadEvent)
                        if (ProjectLoaded != null)
                            ProjectLoaded(this, new EventArgs());
                    return true;
                }
                //Old projects
                else if (openedProject.GetType() == typeof(AHD.SubtitlesMakerProfessional.Base.Project))
                {
                    AHD.SubtitlesMakerProfessional.Base.ProjectManager oldManager = new SubtitlesMakerProfessional.Base.ProjectManager();
                    if (oldManager.LoadProject(FileName))
                    {
                        //name, log...
                        project = new Project(oldManager.Project.Name);
                        project.Description = oldManager.Project.Description;
                        project.Log = oldManager.Project.Log;
                        project.MediaPath = oldManager.Project.MediaPath;
                        //marks
                        project.TimeMarks = new List<TimeMark>();
                        foreach (AHD.SubtitlesMakerProfessional.Base.Mark mrk in oldManager.Project.Marks)
                        {
                            TimeMark TM = new TimeMark(mrk.Name, mrk.Time);
                            project.TimeMarks.Add(TM);
                        }
                        //subtitle tracks
                        project.SubtitleTracks = new List<SubtitlesTrack>();
                        foreach (AHD.SubtitlesMakerProfessional.Base.SubtitlesTrack track in oldManager.Project.SubtitleTracks)
                        {
                            SubtitlesTrack newTrack = new SubtitlesTrack(track.Name);
                            //subtitles
                            foreach (AHD.SubtitlesMakerProfessional.Base.Subtitle sub in track.Subtitles)
                            {
                                Subtitle newSubtitle = new Subtitle();
                                newSubtitle.EndTime = sub.EndTime;
                                newSubtitle.StartTime = sub.StartTime;
                                newSubtitle.Text = new SubtitleText();
                                newSubtitle.Text.CustomPosition = sub.CustomLocation;
                                newSubtitle.Text.IsCustomPosition = sub.Location == SubtitlesMakerProfessional.Base.SubtitleLocation.Custom;
                                switch (sub.Location)
                                {
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Bottom:
                                        newSubtitle.Text.Position = SubtitlePosition.Down_Middle; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Bottom_Left:
                                        newSubtitle.Text.Position = SubtitlePosition.Down_Left; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Bottom_Right:
                                        newSubtitle.Text.Position = SubtitlePosition.Down_Right; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Middle:
                                        newSubtitle.Text.Position = SubtitlePosition.Mid_Middle; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Middle_Left:
                                        newSubtitle.Text.Position = SubtitlePosition.Mid_Left; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Middle_Right:
                                        newSubtitle.Text.Position = SubtitlePosition.Mid_Right; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Top:
                                        newSubtitle.Text.Position = SubtitlePosition.Top_Middle; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Top_Left:
                                        newSubtitle.Text.Position = SubtitlePosition.Top_Left; break;
                                    case SubtitlesMakerProfessional.Base.SubtitleLocation.Top_Right:
                                        newSubtitle.Text.Position = SubtitlePosition.Top_right; break;
                                    default: newSubtitle.Text.Position = SubtitlePosition.Down_Middle; break;
                                }
                                //lines
                                foreach (string line in sub.TextLines)
                                {
                                    SubtitleLine newLine = new SubtitleLine();
                                    newLine.Alignement = LineAlignement.Center;
                                    char[] lineChars = line.ToCharArray();
                                    foreach (char oldChar in lineChars)
                                    {
                                        newLine.Chars.Add(new SubtitleChar(oldChar, sub.Font, sub.Color));
                                    }
                                    newSubtitle.Text.TextLines.Add(newLine);
                                }
                                newTrack.Subtitles.Add(newSubtitle);
                            }
                            project.SubtitleTracks.Add(newTrack);
                        }
                        str.Close();
                        filePath = FileName;
                        if (riseLoadEvent)
                            if (ProjectLoaded != null)
                                ProjectLoaded(this, new EventArgs());
                        MessageBox.Show(resources.GetString("Message_ThisProjectInAnOld"),
                            resources.GetString("WARNING"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return true;
                    }
                    else
                    {
                        if (ProjectDidntLoad != null)
                            ProjectDidntLoad(this, new ErrorEventArgs(new Exception(resources.GetString("Message_ThisFileIsNot ASMP"))));
                    }
                }
            }
            catch (Exception ex)
            {
                if (ProjectDidntLoad != null)
                    ProjectDidntLoad(this, new ErrorEventArgs(ex));
                return false;
            }
            if (ProjectDidntLoad != null)
                ProjectDidntLoad(this, new ErrorEventArgs(new Exception(resources.GetString("Message_UnableToLoadThisProject"))));
            return false;
        }
    }
}
