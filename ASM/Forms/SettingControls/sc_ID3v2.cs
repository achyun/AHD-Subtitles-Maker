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
    public partial class sc_ID3v2 : ASMSettingsControl
    {
        public sc_ID3v2()
        {
            InitializeComponent();
            comboBox_tagVersion.Items.Add(2);
            comboBox_tagVersion.Items.Add(3);
            comboBox_tagVersion.Items.Add(4);
            comboBox_tagVersion.SelectedItem = Program.Settings.DefaultID3V2Version;
            checkBox_dropExtendedHeader.Checked = Program.Settings.ID3V2_DropExtendedHeader;
            checkBox_footer.Checked = Program.Settings.ID3V2_WriteFooter;
            checkBox_keepPadding.Checked = Program.Settings.ID3V2_KeepPadding;
            checkBox_unsynchronisation.Checked = Program.Settings.ID3V2_UseUnsynchronisation;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_ID3");
        }
        public override void SaveSettings()
        {
            Program.Settings.DefaultID3V2Version = (int)comboBox_tagVersion.SelectedItem;
            Program.Settings.ID3V2_DropExtendedHeader = checkBox_dropExtendedHeader.Checked;
            Program.Settings.ID3V2_WriteFooter = checkBox_footer.Checked;
            Program.Settings.ID3V2_KeepPadding = checkBox_keepPadding.Checked;
            Program.Settings.ID3V2_UseUnsynchronisation = checkBox_unsynchronisation.Checked;
            Program.LoadID3V2Settings();// apply for id3v2
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            comboBox_tagVersion.SelectedItem = 3;
            checkBox_dropExtendedHeader.Checked = false;
            checkBox_footer.Checked = false;
            checkBox_keepPadding.Checked = true;
            checkBox_unsynchronisation.Checked = false;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(Program.Settings.DefaultID3V2Version);
            stream.Write(Program.Settings.ID3V2_DropExtendedHeader);
            stream.Write(Program.Settings.ID3V2_WriteFooter);
            stream.Write(Program.Settings.ID3V2_KeepPadding);
            stream.Write(Program.Settings.ID3V2_UseUnsynchronisation);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            Program.Settings.DefaultID3V2Version = stream.ReadInt32();
            Program.Settings.ID3V2_DropExtendedHeader = stream.ReadBoolean();
            Program.Settings.ID3V2_WriteFooter = stream.ReadBoolean();
            Program.Settings.ID3V2_KeepPadding = stream.ReadBoolean();
            Program.Settings.ID3V2_UseUnsynchronisation = stream.ReadBoolean();
            // Load
            comboBox_tagVersion.SelectedItem = Program.Settings.DefaultID3V2Version;
            checkBox_dropExtendedHeader.Checked = Program.Settings.ID3V2_DropExtendedHeader;
            checkBox_footer.Checked = Program.Settings.ID3V2_WriteFooter;
            checkBox_keepPadding.Checked = Program.Settings.ID3V2_KeepPadding;
            checkBox_unsynchronisation.Checked = Program.Settings.ID3V2_UseUnsynchronisation;
        }
    }
}
