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
using System.Windows.Forms;
using AHD.ID3.Editor.Base;
namespace AHD.ID3.Editor.GUI
{
    public class EditorControl : UserControl
    {
        protected string[] files;

        /// <summary>
        /// Raised when a progress starts
        /// </summary>
        public event EventHandler ProgressStart;
        /// <summary>
        /// Raised when a progress finishes
        /// </summary>
        public event EventHandler ProgressFinished;
        /// <summary>
        /// Raised in progress
        /// </summary>
        public event EventHandler<ProgressArgs> Progress;
        /// <summary>
        /// Raised when files saved and need to be updated in the browsers.
        /// </summary>
        public event EventHandler UpdateRequired;
        /// <summary>
        /// Raised when the memory af artists, albums or genres updated.
        /// </summary>
        public event EventHandler MemoryUpdated;
        /// <summary>
        /// Raised when the control request a media stop
        /// </summary>
        public event EventHandler StopMediaRequest;
        /// <summary>
        /// Raised when the control request a media play
        /// </summary>
        public event EventHandler PlayMediaRequest;
        /// <summary>
        /// Raised when the control need to save and the media must be stopped (or unloaded)
        /// </summary>
        public event EventHandler ClearMediaRequest;
        /// <summary>
        /// Raised when the control need to reload the media
        /// </summary>
        public event EventHandler ReloadMediaRequest;

        /// <summary>
        /// Save current fields to ID3V2 object
        /// </summary>
        /// <param name="v2">The ID3V2 object to save fields as frames to. Must be loaded first, the save will NOT affect the file</param>
        public virtual void SaveTag(ID3v2 v2)
        {

        }
        /// <summary>
        /// Save current fields to ID3V1 object
        /// </summary>
        /// <param name="v1">The ID3V1 object to save fields as frames to. Must be loaded first, the save will NOT affect the file</param>
        public virtual void SaveTag(ID3v1 v1)
        {

        }
        /// <summary>
        /// Load frames from ID3V2 object
        /// </summary>
        /// <param name="v2">The ID3V2 object to save fields as frames to. Must be loaded first</param>
        public virtual void LoadTag(ID3v2 v2)
        {

        } 
        /// <summary>
        /// Load frames from ID3V1 object
        /// </summary>
        /// <param name="v1">The ID3V1 object to save fields as frames to. Must be loaded first</param>
        public virtual void LoadTag(ID3v1 v1)
        {

        }
        /// <summary>
        /// Clear the editor fields
        /// </summary>
        public virtual void ClearFields()
        {

        }
        /// <summary>
        /// Set one file as selected. No reload, no save ..etc jusr fill list with one file for further features of the editor lile fill fields from id3v1 for example
        /// </summary>
        /// <param name="filePath">The complete file path</param>
        public virtual void SetSelectedFile(string filePath)
        {
            files = new string[] { filePath };
        }
        /// <summary>
        /// Get or set the files collection. May reload feilds from these/this file(s) in some editors.
        /// </summary>
        public virtual string[] SelectedFiles
        {
            get { return files; }
            set { files = value; }
        }
        /// <summary>
        /// Stop and clear the media from file of this control if has one
        /// </summary>
        public virtual void ClearMedia()
        {

        }
        /// <summary>
        /// Reload the media with the file if this control has media
        /// </summary>
        public virtual void ReloadMedia()
        {

        }
        /// <summary>
        /// Play media if this control has one
        /// </summary>
        public virtual void PlayMedia()
        { }
        /// <summary>
        /// Stop media if this control has one
        /// </summary>
        public virtual void StopMedia()
        { 
        
        }
        /// <summary>
        /// Get if this control is playing (for media related controls)
        /// </summary>
        public virtual bool IsPlaying
        { get { return false; } }
        public virtual void DisableSaving()
        {

        }
        public virtual void OnProgressStart()
        {
            if (ProgressStart != null)
                ProgressStart(this, new EventArgs());
        }
        public virtual void OnProgressFinish()
        {
            if (ProgressFinished != null)
                ProgressFinished(this, new EventArgs());
        }
        public virtual void OnProgress(string status, int complete)
        {
            if (Progress != null)
                Progress(this, new ProgressArgs(complete, status));
        }
        public virtual void OnMemoryUpdate()
        {
            if (MemoryUpdated != null)
                MemoryUpdated(this, new EventArgs());
        }
        public virtual void OnUpdateRequired()
        {
            if (UpdateRequired != null)
                UpdateRequired(this, new EventArgs());
        }
        public virtual void OnClearMediaRequest()
        {
            if (ClearMediaRequest != null)
                ClearMediaRequest(this, new EventArgs());
        }
        public virtual void OnReloadMediaRequest()
        {
            if (ReloadMediaRequest != null)
                ReloadMediaRequest(this, new EventArgs());
        }
        public virtual void OnStopMediaRequest()
        {
            if (StopMediaRequest != null)
                StopMediaRequest(this, new EventArgs());
        }
        public virtual void OnPlayMediaRequest()
        {
            if (PlayMediaRequest != null)
                PlayMediaRequest(this, new EventArgs());
        }
    }
}
