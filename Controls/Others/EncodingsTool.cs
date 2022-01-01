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

namespace AHD.SM.Controls
{
    public partial class EncodingsTool : UserControl
    {
        bool ClearCheck = false;
        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public Encoding SelectedEncoding
        {
            get
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    //look up for the checked item
                    foreach (ListViewItem_Encoding item in listView1.Items)
                    {
                        if (item.Checked)
                            return item.Encoding;
                    }
                }
                else
                {
                    //look up for the checked item
                    foreach (ListViewItem_Encoding item in listView2.Items)
                    {
                        if (item.Checked)
                            return item.Encoding;
                    }
                }
                return null;
            }
            set
            {
                foreach (ListViewItem_Encoding item in listView1.Items)
                {
                    if (item.Encoding.CodePage == value.CodePage)
                    {
                        item.Checked = true;
                        item.EnsureVisible();
                        break;
                    }
                }
            }
        }
        public EncodingsTool()
        {
            InitializeComponent();
            ClearCheck = true;
            //fill up encodings
            foreach (EncodingInfo ei in Encoding.GetEncodings())
            {
                ListViewItem_Encoding item = new ListViewItem_Encoding();
                item.Encoding = ei.GetEncoding();
                listView1.Items.Add(item);
            }
            //fill up favorites
            if (ControlsBase.Settings.FavoriteEncodings == null)
                ControlsBase.Settings.FavoriteEncodings = new EncodingsCollection();
            foreach (int ei in ControlsBase.Settings.FavoriteEncodings)
            {
                ListViewItem_Encoding item = new ListViewItem_Encoding();
                item.Encoding = Encoding.GetEncoding(ei);
                listView2.Items.Add(item);
            }
            ClearCheck = false;
        }
        ~EncodingsTool()
        { SaveSettings(); }
        public void SaveSettings()
        {
            ControlsBase.Settings.Save();
        }
        public event EventHandler SelectedEncodingChanged;
        //Add to favorite
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //look up for the checked item
            foreach (ListViewItem_Encoding item in listView1.Items)
            {
                if (item.Checked)
                {
                    //the item found, see if it already exists in the favorites
                    bool found = false;
                    foreach (ListViewItem_Encoding Fitem in listView2.Items)
                    {
                        if (Fitem.Encoding == item.Encoding)
                        {
                            found = true; break;
                        }
                    }
                    //if the item is not found, add it to the settings list
                    if (!found)
                    {
                        ControlsBase.Settings.FavoriteEncodings.Add(item.Encoding.CodePage);
                        //Add it to the list
                        ListViewItem_Encoding newItem = new ListViewItem_Encoding();
                        newItem.Encoding = item.Encoding;
                        listView2.Items.Add(newItem);
                        //exit
                        break;
                    }
                }
            }
            if (listView2.Items.Count > 0)
                listView2.Items[listView2.Items.Count - 1].Checked = true;
            ControlsBase.Settings.Save();
        }
        //Remove from favorite
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            int i = -1;
            foreach (ListViewItem Fitem in listView2.Items)
            {
                if (Fitem.Checked)
                {
                    ControlsBase.Settings.FavoriteEncodings.RemoveAt(Fitem.Index);
                    i = Fitem.Index;
                    break;
                }
            }
            if (i != -1)
                listView2.Items.RemoveAt(i);
            ControlsBase.Settings.Save();
        }
        //search encodings
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            ClearCheck = true;
            listView1.Items.Clear();
            if (toolStripTextBox1.Text.Length == 0)
            {
                //fill up encodings
                foreach (EncodingInfo ei in Encoding.GetEncodings())
                {
                    ListViewItem_Encoding item = new ListViewItem_Encoding();
                    item.Encoding = ei.GetEncoding();
                    listView1.Items.Add(item);
                }
                return;
            }
            for (int i = 0; i < Encoding.GetEncodings().Length; i++)
            {
                EncodingInfo en = Encoding.GetEncodings()[i];
                if (en.GetEncoding().EncodingName.Length >= toolStripTextBox1.Text.Length)
                {
                    for (int SearchWordIndex = 0; SearchWordIndex <
                              (en.GetEncoding().EncodingName.Length - toolStripTextBox1.Text.Length) + 1; SearchWordIndex++)
                    {
                        string Ser = en.GetEncoding().EncodingName.Substring(SearchWordIndex, toolStripTextBox1.Text.Length);
                        if (Ser.ToLower() == toolStripTextBox1.Text.ToLower())
                        {
                            ListViewItem_Encoding item = new ListViewItem_Encoding();
                            item.Encoding = en.GetEncoding();
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            if (listView1.Items.Count > 0)
                listView1.Items[0].Checked = true;
            ClearCheck = false;
        }
        //search favorites
        private void toolStripTextBox2_TextChanged(object sender, EventArgs e)
        {
            ClearCheck = true;
            listView2.Items.Clear();
            if (toolStripTextBox2.Text.Length == 0)
            {
                //fill up encodings
                foreach (int ei in ControlsBase.Settings.FavoriteEncodings)
                {
                    ListViewItem_Encoding item = new ListViewItem_Encoding();
                    item.Encoding = Encoding.GetEncoding(ei);
                    listView2.Items.Add(item);
                }
                return;
            }
            for (int i = 0; i < ControlsBase.Settings.FavoriteEncodings.Count; i++)
            {
                Encoding en = Encoding.GetEncoding(ControlsBase.Settings.FavoriteEncodings[i]);
                if (en.EncodingName.Length >= toolStripTextBox2.Text.Length)
                {
                    for (int SearchWordIndex = 0; SearchWordIndex <
                              (en.EncodingName.Length - toolStripTextBox2.Text.Length) + 1; SearchWordIndex++)
                    {
                        string Ser = en.EncodingName.Substring(SearchWordIndex, toolStripTextBox2.Text.Length);
                        if (Ser.ToLower() == toolStripTextBox2.Text.ToLower())
                        {
                            ListViewItem_Encoding item = new ListViewItem_Encoding();
                            item.Encoding = en;
                            listView2.Items.Add(item);
                        }
                    }
                }
            }
            if (listView2.Items.Count > 0)
                listView2.Items[0].Checked = true;

            ClearCheck = false;
        }
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ClearCheck)
                return;

            ClearCheck = true;
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
            e.Item.Checked = true;
            ClearCheck = false;
            toolStripLabel1_selectedEncoding.Text = e.Item.Text;

            if (SelectedEncodingChanged != null)
                SelectedEncodingChanged(this, new EventArgs());
        }
        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ClearCheck)
                return;

            ClearCheck = true;
            foreach (ListViewItem item in listView2.Items)
            {
                item.Checked = false;
            }
            e.Item.Checked = true;
            ClearCheck = false;
            toolStripLabel1_selectedEncoding.Text = e.Item.Text;

            if (SelectedEncodingChanged != null)
                SelectedEncodingChanged(this, new EventArgs());
        }
        private void EncodingsTool_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                foreach (ListViewItem_Encoding item in listView1.Items)
                {
                    if (item.Checked)
                    {
                        item.EnsureVisible();
                        break;
                    }
                }
            }
        }
    
    }
    public class EncodingsCollection : List<int>//encoding codepages, for save
    { }
    public class ListViewItem_Encoding : ListViewItem
    {
        Encoding encoding;
        public Encoding Encoding
        { get { return encoding; } set { encoding = value; RefreshText(); } }
        public void RefreshText()
        {
            SubItems.Clear();
            this.Text = (encoding.EncodingName);
            SubItems.Add(encoding.BodyName);
            SubItems.Add(encoding.CodePage.ToString());
        }
    }
}
