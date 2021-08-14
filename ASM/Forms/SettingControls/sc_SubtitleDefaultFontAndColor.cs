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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHD.SM
{
    public partial class sc_SubtitleDefaultFontAndColor : ASMSettingsControl
    {
        public sc_SubtitleDefaultFontAndColor()
        {
            InitializeComponent();
            font = Program.Settings.NewSubtitleFont;
            textBox_font.Text = Program.Settings.NewSubtitleFont.ToString();
            button_color.BackColor = Program.Settings.NewSubtitleColor;
        }
        Font font;

        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_DefaultFontAndColor");
        }
        public override void SaveSettings()
        {
            Program.Settings.NewSubtitleFont = font;
            Program.Settings.NewSubtitleColor = button_color.BackColor;
            base.SaveSettings();
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.NewSubtitleColor.ToArgb());

            FontConverter conv = new FontConverter();
            string st = conv.ConvertToString(font);
            byte[] fontBuffer = Encoding.UTF8.GetBytes(st);

            stream.Write(fontBuffer.Length);
            stream.Write(fontBuffer);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.NewSubtitleColor = Color.FromArgb(stream.ReadInt32());
            int length = stream.ReadInt32();
            byte[] fontBuffer = new byte[length];
            stream.Read(fontBuffer, 0, length);
            string name = Encoding.UTF8.GetString(fontBuffer);
            FontConverter conv = new FontConverter();
            Program.Settings.NewSubtitleFont = (Font)conv.ConvertFromString(name);

            // Load
            font = Program.Settings.NewSubtitleFont;
            textBox_font.Text = Program.Settings.NewSubtitleFont.ToString();
            button_color.BackColor = Program.Settings.NewSubtitleColor;
        }
        //font
        private void button1_Click(object sender, EventArgs e)
        {
            FontDialog dial = new FontDialog();
            dial.Font = font;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                font = dial.Font;
                textBox_font.Text = font.ToString();
            }
        }

        private void textBox_font_TextChanged(object sender, EventArgs e)
        {
            label_sample.Font = font;
        }
        private void button_color_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = button_color.BackColor;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                button_color.BackColor = dial.Color;
            }
        }
        private void button_color_BackColorChanged(object sender, EventArgs e)
        {
            label_sample.ForeColor = button_color.BackColor;
        }
        public override void DefaultSettings()
        {
            font = new Font("Tahoma", 8, FontStyle.Regular);
            label_sample.Font = font;
            textBox_font.Text = font.ToString();

            button_color.BackColor = Color.White;
            label_sample.ForeColor = button_color.BackColor;
        }
    }
}
