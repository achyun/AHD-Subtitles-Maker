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
    public partial class sc_Subtitle : ASMSettingsControl
    {
        public sc_Subtitle()
        {
            InitializeComponent();
            checkBox_MediaSeekOnSelect.Checked = Program.Settings.MediaSeekOnSelect;
            checkBox_editMoreThanOneSubtitleUsingEditor.Checked = Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow;
            checkBox_editCreatedSubtitle.Checked = Program.Settings.EditSubtitleAfterAddUsingQuickMode;
            checkBox_pauseMedia.Checked = Program.Settings.PauseAfterAddUsingQuickMode;
            checkBox_seekMediaToNewSubtitleStartTimeAfterAdd.Checked = Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd;
            checkBox_allowErrors.Checked = Program.Settings.AllowErrorsInEditControl;
            checkBox_addNewSubtitleTextWhenAddingNewSubtitle.Checked = Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle;
            checkBox_askBeforeDeletingSubtitles.Checked = Program.Settings.AskBeforeDeletingSubtitle;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_Subtitle");
        }
        public override void SaveSettings()
        {
            Program.Settings.MediaSeekOnSelect = checkBox_MediaSeekOnSelect.Checked;
            Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow = checkBox_editMoreThanOneSubtitleUsingEditor.Checked;
            Program.Settings.EditSubtitleAfterAddUsingQuickMode = checkBox_editCreatedSubtitle.Checked;
            Program.Settings.PauseAfterAddUsingQuickMode = checkBox_pauseMedia.Checked;
            Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd = checkBox_seekMediaToNewSubtitleStartTimeAfterAdd.Checked;
            Program.Settings.AllowErrorsInEditControl = checkBox_allowErrors.Checked;
            Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle = checkBox_addNewSubtitleTextWhenAddingNewSubtitle.Checked;
            Program.Settings.AskBeforeDeletingSubtitle = checkBox_askBeforeDeletingSubtitles.Checked;
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            checkBox_editMoreThanOneSubtitleUsingEditor.Checked = true;
            checkBox_editCreatedSubtitle.Checked = true;
            checkBox_pauseMedia.Checked = true;
            checkBox_seekMediaToNewSubtitleStartTimeAfterAdd.Checked = false;
            checkBox_MediaSeekOnSelect.Checked = true;
            checkBox_allowErrors.Checked = false;
            checkBox_addNewSubtitleTextWhenAddingNewSubtitle.Checked = false;
            checkBox_askBeforeDeletingSubtitles.Checked = true;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.MediaSeekOnSelect);
            stream.Write(Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow);
            stream.Write(Program.Settings.EditSubtitleAfterAddUsingQuickMode);
            stream.Write(Program.Settings.PauseAfterAddUsingQuickMode);
            stream.Write(Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd);
            stream.Write(Program.Settings.AllowErrorsInEditControl);
            stream.Write(Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle);
            stream.Write(Program.Settings.AskBeforeDeletingSubtitle);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.MediaSeekOnSelect = stream.ReadBoolean();
            Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow = stream.ReadBoolean();
            Program.Settings.EditSubtitleAfterAddUsingQuickMode = stream.ReadBoolean();
            Program.Settings.PauseAfterAddUsingQuickMode = stream.ReadBoolean();
            Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd = stream.ReadBoolean();
            Program.Settings.AllowErrorsInEditControl = stream.ReadBoolean();
            Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle = stream.ReadBoolean();
            Program.Settings.AskBeforeDeletingSubtitle = stream.ReadBoolean();
            // Load
            checkBox_MediaSeekOnSelect.Checked = Program.Settings.MediaSeekOnSelect;
            checkBox_editMoreThanOneSubtitleUsingEditor.Checked = Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow;
            checkBox_editCreatedSubtitle.Checked = Program.Settings.EditSubtitleAfterAddUsingQuickMode;
            checkBox_pauseMedia.Checked = Program.Settings.PauseAfterAddUsingQuickMode;
            checkBox_seekMediaToNewSubtitleStartTimeAfterAdd.Checked = Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd;
            checkBox_allowErrors.Checked = Program.Settings.AllowErrorsInEditControl;
            checkBox_addNewSubtitleTextWhenAddingNewSubtitle.Checked = Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle;
            checkBox_askBeforeDeletingSubtitles.Checked = Program.Settings.AskBeforeDeletingSubtitle;
        }
    }
}
