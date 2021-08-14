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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.Net;
using System.IO;
using SevenZip;
using AHD.SM.ASMP.SpellCheck;
using AHD.Forms;

namespace AHD.SM.Forms
{
    public partial class Frm_DictionariesDownload : Form
    {
        public Frm_DictionariesDownload()
        {
            InitializeComponent();
            //load list
            checker = new SpellChecker(".\\Dictionaries");

            foreach (DictionaryLink link in checker.AvailableToDownload)
            {
                ListViewItem item = new ListViewItem();
                item.Text = link.Name;
                item.SubItems.Add(link.NativeName);
                item.SubItems.Add(link.Description);

                listView1.Items.Add(item);
            }
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
           Assembly.GetExecutingAssembly());
        private WebClient client = new WebClient();
        private SpellChecker checker;
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = false;
        }
        //download
        private void button1_Click(object sender, EventArgs e)
        {
            List<int> indiacs = new List<int>();
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    indiacs.Add(item.Index);
                }
            }
            if (indiacs.Count == 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectOneDictionaryAtLeast"),
                   resources.GetString("MessageCaption_DownloadDictionaries"));
                return;
            }
            label1_status.Visible = true;
            label1_status.Refresh();

            this.Cursor = Cursors.WaitCursor;
            foreach (int index in indiacs)
            {
                try
                {
                    //download
                    client.DownloadFile(checker.AvailableToDownload[index].Link,
                        Path.GetFullPath(".\\Dictionaries\\" + checker.AvailableToDownload[index].Name + ".oxt"));
                    //extract
                    SevenZipExtractor extractor = new SevenZipExtractor(Path.GetFullPath(".\\Dictionaries\\" + checker.AvailableToDownload[index].Name + ".oxt"));
                    Directory.CreateDirectory(Path.GetFullPath(".\\Dictionaries\\" + checker.AvailableToDownload[index].Name + "\\"));
                    extractor.ExtractArchive(Path.GetFullPath(".\\Dictionaries\\" + checker.AvailableToDownload[index].Name + "\\"));
                    //delete the file
                    File.Delete(Path.GetFullPath(".\\Dictionaries\\" + checker.AvailableToDownload[index].Name + ".oxt"));
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowErrorMessage(resources.GetString("Message_UnableToDownloadDictionary") + " (" +
                        checker.AvailableToDownload[index].Name + ")\n\n" + ex.Message + "\n\n" + ex.ToString(),
                        resources.GetString("MessageCaption_DownloadDictionaries"));
                }
            }
            this.Cursor = Cursors.Default;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
