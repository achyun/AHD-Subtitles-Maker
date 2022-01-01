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
using AHD.Forms;
using AHD.SM.ASMP;
using System.IO;
using System.Windows.Forms;

namespace AHD.SM
{
    public partial class sc_MediaPlayer : ASMSettingsControl
    {
        public sc_MediaPlayer()
        {
            InitializeComponent();
            message_shown = false;
            checkBox_enable_sub_preview.Checked = Program.Settings.MediaPlayerEnableSubtitlePreview;
            radioButton_use_custom_style.Checked = !Program.Settings.MediaPlayerUseSubtitleFormatting;
            radioButton_use_subtitle_formating.Checked = Program.Settings.MediaPlayerUseSubtitleFormatting;
            custom_style = new ASMP.ASMPFontStyle("", Program.Settings.MediaPlayerCustomFont, Program.Settings.MediaPlayerCustomColor);
            switch (Program.Settings.MediaPlayerCurrent)
            {
                case "player.directshow":
                    {
                        comboBox_media_player.SelectedIndex = 0;
                        break;
                    }
                case "player.vlc":
                    {
                        message_shown = true;
                        comboBox_media_player.SelectedIndex = 1;
                        break;
                    }
                case "player.wmp":
                    {
                        comboBox_media_player.SelectedIndex = 2;
                        break;
                    }
            }
            ApplyStylePreview();
        }
        private ASMP.ASMPFontStyle custom_style;
        private bool message_shown = false;
        public override string ID { get { return "media-player"; } }
        private void ApplyStylePreview()
        {
            richTextBox1.SelectAll();
            richTextBox1.SelectionFont = custom_style.Font;
            richTextBox1.SelectionColor = custom_style.Color;
            textBox_style.Text = custom_style.ToString();
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_MediaPlayer");
        }
        public override void SaveSettings()
        {
            Program.Settings.MediaPlayerEnableSubtitlePreview = checkBox_enable_sub_preview.Checked;
            Program.Settings.MediaPlayerUseSubtitleFormatting = radioButton_use_subtitle_formating.Checked;
            Program.Settings.MediaPlayerCustomFont = custom_style.Font;
            Program.Settings.MediaPlayerCustomColor = custom_style.Color;
            switch (comboBox_media_player.SelectedIndex)
            {
                case 0: Program.Settings.MediaPlayerCurrent = "player.directshow"; break;
                case 1: Program.Settings.MediaPlayerCurrent = "player.vlc"; break;
                case 2: Program.Settings.MediaPlayerCurrent = "player.wmp"; break;
            }
        }
        public override void DefaultSettings()
        {
            checkBox_enable_sub_preview.Checked = true;
            radioButton_use_custom_style.Checked = false;
            radioButton_use_subtitle_formating.Checked = true;
            custom_style = new ASMP.ASMPFontStyle("", new System.Drawing.Font("Tahoma", 9, System.Drawing.FontStyle.Regular), System.Drawing.Color.White);
            comboBox_media_player.SelectedIndex = 0;
            ApplyStylePreview();
        }
        public override void ExportSettings(ref BinaryWriter stream)
        {
            base.ExportSettings(ref stream);
            stream.Write(Program.Settings.MediaPlayerEnableSubtitlePreview);
            stream.Write(Program.Settings.MediaPlayerUseSubtitleFormatting);
        }
        public override void ImportSettings(ref BinaryReader stream)
        {
            base.ImportSettings(ref stream);
            checkBox_enable_sub_preview.Checked = stream.ReadBoolean();
            radioButton_use_subtitle_formating.Checked = stream.ReadBoolean();
        }
        // Change style
        private void button1_Click(object sender, System.EventArgs e)
        {
            FontDialog frm_font = new FontDialog();
            frm_font.Font = custom_style.Font;
            if (frm_font.ShowDialog(this) == DialogResult.OK)
            {
                ColorDialog frm_color = new ColorDialog();
                frm_color.Color = custom_style.Color;
                if (frm_color.ShowDialog(this) == DialogResult.OK)
                {
                    custom_style = new ASMPFontStyle("", frm_font.Font, frm_color.Color);
                    ApplyStylePreview();
                }
            }
        }
        private void comboBox_media_player_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!message_shown && comboBox_media_player.SelectedIndex == 1)
            {
                message_shown = true;
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_VLCPlayerMustBeInstalled"), Program.ResourceManager.GetString("MessageCaption_ChangePlayer"));
            }
        }
    }
}
