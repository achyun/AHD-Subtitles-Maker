// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
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
namespace AHD.SM
{
    public partial class sc_PlayBack : ASMSettingsControl
    {
        public sc_PlayBack()
        {
            InitializeComponent();
            checkBox_auto_generate_wave.Checked = Program.Settings.TimelineAutoGenerateWaveform;
            checkBox_warnWhenID3Detected.Checked = Program.Settings.WarnMeWhenID3Detected;
            checkBox_wanrMeWhenMKVLoaded.Checked = Program.Settings.WarnMeWhenMKVDetected;
            checkBox_scrollTimeLine.Checked = Program.Settings.ScrollTimelineToMediaOnMediaSlide;
            numericUpDown_AdvanceTime.Value = (int)(Program.Settings.AdvanceTime * 1000);
            numericUpDown_NewSubtitleDuration.Value = Program.Settings.NewSubtitleDuration;
            numericUpDown_ShiftTime.Value = Program.Settings.ShiftTime;
            numericUpDown_playerTimer.Value = Program.Settings.PlayerTimer;
        }
        public override void SaveSettings()
        {
            Program.Settings.TimelineAutoGenerateWaveform = checkBox_auto_generate_wave.Checked;
            Program.Settings.WarnMeWhenID3Detected = checkBox_warnWhenID3Detected.Checked;
            Program.Settings.WarnMeWhenMKVDetected = checkBox_wanrMeWhenMKVLoaded.Checked;
            Program.Settings.ScrollTimelineToMediaOnMediaSlide = checkBox_scrollTimeLine.Checked;
            Program.Settings.AdvanceTime = ((double)numericUpDown_AdvanceTime.Value / 1000);
            Program.Settings.NewSubtitleDuration = (int)numericUpDown_NewSubtitleDuration.Value;
            Program.Settings.ShiftTime = (int)numericUpDown_ShiftTime.Value;
            Program.Settings.PlayerTimer = (int)numericUpDown_playerTimer.Value;
            base.SaveSettings();
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_PlayBack");
        }
        public override void DefaultSettings()
        {
            checkBox_auto_generate_wave.Checked = true;
            checkBox_warnWhenID3Detected.Checked = true;
            checkBox_wanrMeWhenMKVLoaded.Checked = true;
            checkBox_scrollTimeLine.Checked = true;
            numericUpDown_AdvanceTime.Value = 500;
            numericUpDown_NewSubtitleDuration.Value = 2;
            numericUpDown_ShiftTime.Value = 100;
            numericUpDown_playerTimer.Value = 10;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.TimelineAutoGenerateWaveform);
            stream.Write(Program.Settings.WarnMeWhenID3Detected);
            stream.Write(Program.Settings.WarnMeWhenMKVDetected);
            stream.Write(false);
            stream.Write(Program.Settings.ScrollTimelineToMediaOnMediaSlide);
            stream.Write(Program.Settings.AdvanceTime);
            stream.Write(Program.Settings.NewSubtitleDuration);
            stream.Write(Program.Settings.ShiftTime);
            stream.Write(Program.Settings.PlayerTimer);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.TimelineAutoGenerateWaveform = stream.ReadBoolean();
            Program.Settings.WarnMeWhenID3Detected = stream.ReadBoolean();
            Program.Settings.WarnMeWhenMKVDetected = stream.ReadBoolean();
            stream.ReadBoolean();// For compatibility with older versions
            Program.Settings.ScrollTimelineToMediaOnMediaSlide = stream.ReadBoolean();
            Program.Settings.AdvanceTime = stream.ReadDouble();
            Program.Settings.NewSubtitleDuration = stream.ReadInt32();
            Program.Settings.ShiftTime = stream.ReadInt32();
            Program.Settings.PlayerTimer = stream.ReadInt32();
            // Load
            checkBox_auto_generate_wave.Checked = Program.Settings.TimelineAutoGenerateWaveform;
            checkBox_warnWhenID3Detected.Checked = Program.Settings.WarnMeWhenID3Detected;
            checkBox_wanrMeWhenMKVLoaded.Checked = Program.Settings.WarnMeWhenMKVDetected;
            checkBox_scrollTimeLine.Checked = Program.Settings.ScrollTimelineToMediaOnMediaSlide;
            numericUpDown_AdvanceTime.Value = (int)(Program.Settings.AdvanceTime * 1000);
            numericUpDown_NewSubtitleDuration.Value = Program.Settings.NewSubtitleDuration;
            numericUpDown_ShiftTime.Value = Program.Settings.ShiftTime;
            numericUpDown_playerTimer.Value = Program.Settings.PlayerTimer;
        }
    }
}
