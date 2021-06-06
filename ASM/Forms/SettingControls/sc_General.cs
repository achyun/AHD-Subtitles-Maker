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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHD.SM
{
    public partial class sc_General : ASMSettingsControl
    {
        public sc_General()
        {
            InitializeComponent();

            checkBox_newProject.Checked = Program.Settings.ShowNewProjectWizard;
            checkBox_autoSelecteSubtitlesTrack.Checked = Program.Settings.AutoSelectFirstTrack;
            checkBox_askWhenChangingRTL.Checked = Program.Settings.AskWhenChangingRTL;
            checkBox_trackTemplate.Checked = Program.Settings.NewSubtitlesTrackTemplate;
            checkBox_AutoCreateTrack.Checked = Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject;
            checkBox_askToExportBeforeUpload.Checked = Program.Settings.AskToExportBeforeUpload;
            checkBox_autoSaveEnable.Checked = Program.Settings.AutoSaveEnabled;
            numericUpDown_autoSavePeriod.Value = Program.Settings.AutoSavePeriodMinutes;
            checkBox_check_for_updates.Checked = Program.Settings.CheckForUpdatesAutoamtically;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_General");
        }
        public override void SaveSettings()
        {
            Program.Settings.ShowNewProjectWizard = checkBox_newProject.Checked;
            Program.Settings.AutoSelectFirstTrack = checkBox_autoSelecteSubtitlesTrack.Checked;
            Program.Settings.AskWhenChangingRTL = checkBox_askWhenChangingRTL.Checked;
            Program.Settings.NewSubtitlesTrackTemplate = checkBox_trackTemplate.Checked;
            Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject = checkBox_AutoCreateTrack.Checked;
            Program.Settings.AskToExportBeforeUpload = checkBox_askToExportBeforeUpload.Checked;
            Program.Settings.AutoSaveEnabled = checkBox_autoSaveEnable.Checked;
            Program.Settings.AutoSavePeriodMinutes = (int)numericUpDown_autoSavePeriod.Value;
            Program.Settings.CheckForUpdatesAutoamtically = checkBox_check_for_updates.Checked;
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            checkBox_newProject.Checked = true;
            checkBox_autoSelecteSubtitlesTrack.Checked = true;
            checkBox_askWhenChangingRTL.Checked = true;
            checkBox_trackTemplate.Checked = true;
            checkBox_AutoCreateTrack.Checked = true;
            checkBox_askToExportBeforeUpload.Checked = true;
            checkBox_autoSaveEnable.Checked = true;
            checkBox_check_for_updates.Checked = false;
            numericUpDown_autoSavePeriod.Value = 10;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.ShowNewProjectWizard);
            stream.Write(Program.Settings.AutoSelectFirstTrack);
            stream.Write(Program.Settings.AskWhenChangingRTL);
            stream.Write(Program.Settings.NewSubtitlesTrackTemplate);
            stream.Write(Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject);
            stream.Write(Program.Settings.AskToExportBeforeUpload);
            stream.Write(Program.Settings.AutoSaveEnabled);
            stream.Write(Program.Settings.AutoSavePeriodMinutes);
            stream.Write(Program.Settings.CheckForUpdatesAutoamtically);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.ShowNewProjectWizard = stream.ReadBoolean();
            Program.Settings.AutoSelectFirstTrack = stream.ReadBoolean();
            Program.Settings.AskWhenChangingRTL = stream.ReadBoolean();
            Program.Settings.NewSubtitlesTrackTemplate = stream.ReadBoolean();
            Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject = stream.ReadBoolean();
            Program.Settings.AskToExportBeforeUpload = stream.ReadBoolean();
            Program.Settings.AutoSaveEnabled = stream.ReadBoolean();
            Program.Settings.AutoSavePeriodMinutes = stream.ReadInt32();
            Program.Settings.CheckForUpdatesAutoamtically = stream.ReadBoolean();
            // Load
            checkBox_newProject.Checked = Program.Settings.ShowNewProjectWizard;
            checkBox_autoSelecteSubtitlesTrack.Checked = Program.Settings.AutoSelectFirstTrack;
            checkBox_askWhenChangingRTL.Checked = Program.Settings.AskWhenChangingRTL;
            checkBox_trackTemplate.Checked = Program.Settings.NewSubtitlesTrackTemplate;
            checkBox_AutoCreateTrack.Checked = Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject;
            checkBox_askToExportBeforeUpload.Checked = Program.Settings.AskToExportBeforeUpload;
            checkBox_autoSaveEnable.Checked = Program.Settings.AutoSaveEnabled;
            numericUpDown_autoSavePeriod.Value = Program.Settings.AutoSavePeriodMinutes;
            checkBox_check_for_updates.Checked = Program.Settings.CheckForUpdatesAutoamtically;
        }
    }
}
