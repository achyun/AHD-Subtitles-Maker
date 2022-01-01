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
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AHD.SM
{
    public partial class sc_EditorOptions : ASMSettingsControl
    {
        public sc_EditorOptions()
        {
            InitializeComponent();

            button1.BackColor = Program.Settings.SubtitleTextEditorBackColor;
            checkBox1.Checked = Program.Settings.SubtitleTextEditorShowStatusStrip;
            numericUpDown_megnaticAccuracy.Value = Program.Settings.MegnaticAccuracy;
            numericUpDown_smartToolAccuracy.Value = Program.Settings.SmartToolAccuracy;
            trackBar1.Value = Program.Settings.WaveFormQuality;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_Editors");
        }
        public override void SaveSettings()
        {
            Program.Settings.SubtitleTextEditorShowStatusStrip = checkBox1.Checked;
            Program.Settings.SubtitleTextEditorBackColor = button1.BackColor;
            Program.Settings.MegnaticAccuracy = (int)numericUpDown_megnaticAccuracy.Value;
            Program.Settings.SmartToolAccuracy = (int)numericUpDown_smartToolAccuracy.Value;
            Program.Settings.WaveFormQuality = trackBar1.Value;
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            checkBox1.Checked = true;
            button1.BackColor = Color.Black;
            numericUpDown_megnaticAccuracy.Value = 5;
            numericUpDown_smartToolAccuracy.Value = 7;
            trackBar1.Value = 5;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.SubtitleTextEditorShowStatusStrip);
            stream.Write(Program.Settings.SubtitleTextEditorBackColor.ToArgb());
            stream.Write(Program.Settings.MegnaticAccuracy);
            stream.Write(Program.Settings.SmartToolAccuracy);
            stream.Write(Program.Settings.WaveFormQuality);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.SubtitleTextEditorShowStatusStrip = stream.ReadBoolean();
            Program.Settings.SubtitleTextEditorBackColor = Color.FromArgb(stream.ReadInt32());
            Program.Settings.MegnaticAccuracy = stream.ReadInt32();
            Program.Settings.SmartToolAccuracy = stream.ReadInt32();
            Program.Settings.WaveFormQuality = stream.ReadInt32();

            button1.BackColor = Program.Settings.SubtitleTextEditorBackColor;
            checkBox1.Checked = Program.Settings.SubtitleTextEditorShowStatusStrip;
            numericUpDown_megnaticAccuracy.Value = Program.Settings.MegnaticAccuracy;
            numericUpDown_smartToolAccuracy.Value = Program.Settings.SmartToolAccuracy;
            trackBar1.Value = Program.Settings.WaveFormQuality;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button1.BackColor;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                button1.BackColor = dial.Color;
            }
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
