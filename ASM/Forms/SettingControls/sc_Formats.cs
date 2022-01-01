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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AHD.SM.Formats;

namespace AHD.SM
{
    public partial class sc_Formats : ASMSettingsControl
    {
        public sc_Formats()
        {
            InitializeComponent();
            if (Program.Settings.EnabledFormats == null)
                Program.Settings.EnabledFormats = new BooleansCollection();
            if (Program.Settings.EnabledFormats.Count != SubtitleFormats.Formats.Length)
            {
                Program.Settings.EnabledFormats = new BooleansCollection();
                foreach (SubtitlesFormat format in SubtitleFormats.Formats)
                {
                    Program.Settings.EnabledFormats.Add(true);
                }
            }
            //fill the list
            int i = 0;
            foreach (SubtitlesFormat format in SubtitleFormats.Formats)
            {
                ListViewItem item = new ListViewItem();
                item.Text = format.Name;
                item.Checked = Program.Settings.EnabledFormats[i];
                listView1.Items.Add(item);
                i++;
            }

            checkBox_autoCheckForErrors.Checked = Program.Settings.AutoCheckForErrors;
            checkBox_ImportDeepSearch.Checked = Program.Settings.ImportDeepSearch;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_SubtitleFormats");
        }
        public override void SaveSettings()
        {
            //we should make sure there's an enabled format at least
            int itemsCount = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                    itemsCount++;
            }
            if (itemsCount == 0)
            {
                listView1.Items[0].Checked = true;
            }
            //save
            Program.Settings.EnabledFormats.Clear();
            foreach (ListViewItem item in listView1.Items)
            {
                Program.Settings.EnabledFormats.Add(item.Checked);
            }
            //apply
            for (int i = 0; i < Program.Settings.EnabledFormats.Count; i++)
            {
                SubtitleFormats.Formats[i].Enabled = Program.Settings.EnabledFormats[i];
            }

            Program.Settings.AutoCheckForErrors = checkBox_autoCheckForErrors.Checked;
            Program.Settings.ImportDeepSearch = checkBox_ImportDeepSearch.Checked;
        }
        public override void DefaultSettings()
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
            checkBox_autoCheckForErrors.Checked = true;
            checkBox_ImportDeepSearch.Checked = false;
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            foreach (bool en in Program.Settings.EnabledFormats)
                stream.Write(en);
            stream.Write(Program.Settings.AutoCheckForErrors);
            stream.Write(Program.Settings.ImportDeepSearch);
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            for (int i = 0; i < Program.Settings.EnabledFormats.Count; i++)
                Program.Settings.EnabledFormats[i] = stream.ReadBoolean();
            Program.Settings.AutoCheckForErrors = stream.ReadBoolean();
            Program.Settings.ImportDeepSearch = stream.ReadBoolean();
            // Load
            if (Program.Settings.EnabledFormats == null)
                Program.Settings.EnabledFormats = new BooleansCollection();
            if (Program.Settings.EnabledFormats.Count != SubtitleFormats.Formats.Length)
            {
                Program.Settings.EnabledFormats = new BooleansCollection();
                foreach (SubtitlesFormat format in SubtitleFormats.Formats)
                {
                    Program.Settings.EnabledFormats.Add(true);
                }
            }
            //fill the list
            int index = 0;
            listView1.Items.Clear();
            foreach (SubtitlesFormat format in SubtitleFormats.Formats)
            {
                ListViewItem item = new ListViewItem();
                item.Text = format.Name;
                item.Checked = Program.Settings.EnabledFormats[index];
                listView1.Items.Add(item);
                index++;
            }

            checkBox_autoCheckForErrors.Checked = Program.Settings.AutoCheckForErrors;
            checkBox_ImportDeepSearch.Checked = Program.Settings.ImportDeepSearch;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DefaultSettings();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
    }
}
