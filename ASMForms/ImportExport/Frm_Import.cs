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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AHD.SM.ASMP;
using AHD.SM.Formats;

namespace AHD.SM.Forms
{
    public partial class Frm_Import : Form
    {
        string filePath;
        bool deepSearch;

        public SubtitlesFormat SubtitlesFormat
        { get { return ((TreeNode_SubtitleFormat)treeView1.SelectedNode).Format; } }
        public Encoding SelectedEncoding
        { get { return encodingsTool1.SelectedEncoding; } }
        public bool RightToLeftSubtitlesFormat
        { get { return checkBox1.Checked; } }

        public Frm_Import(string filePath, bool deepSearch)
        {
            this.filePath = filePath;
            InitializeComponent();

        }
        private void DetectFormat()
        {
            treeView1.Nodes.Clear();
            //detect import formats
            SubtitlesFormat[] detected = SubtitleFormats.CheckFile(filePath, deepSearch, encodingsTool1.SelectedEncoding);
            foreach (SubtitlesFormat format in SubtitleFormats.EnabledFormats)
            {
                TreeNode_SubtitleFormat node = new TreeNode_SubtitleFormat();
                node.Format = format;
                node.ImageIndex = node.SelectedImageIndex = detected.Contains(format) ? 1 : 0;
                treeView1.Nodes.Add(node);
                if (detected.Contains(format))
                {
                    if (treeView1.SelectedNode == null)
                        treeView1.SelectedNode = node;
                }
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            richTextBox1.Text = ((TreeNode_SubtitleFormat)treeView1.SelectedNode).Format.Description;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            FileStream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[str.Length];
            str.Read(buf, 0, (int)str.Length);
            str.Close();
            encodingsTool1.SelectedEncoding = Encoding.ASCII;
            richTextBox2.Lines = File.ReadAllLines(filePath, Encoding.ASCII);
            DetectFormat();
        }
        private void encodingsTool1_SelectedEncodingChanged(object sender, EventArgs e)
        {
            richTextBox2.Lines = File.ReadAllLines(filePath, encodingsTool1.SelectedEncoding);
        }
        private void Frm_Import_FormClosed(object sender, FormClosedEventArgs e)
        {
            encodingsTool1.SaveSettings();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            string helpPath = ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpPath))
                Help.ShowHelp(this, helpPath, HelpNavigator.KeywordIndex, "How to, Import subtitles format file");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", HelpNavigator.KeywordIndex, "How to, Import subtitles format file");
        }
        private void Frm_Import_Shown(object sender, EventArgs e)
        {
            // do auto-detect
            FileStream str = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[str.Length];
            str.Read(buf, 0, (int)str.Length);
            str.Close();
            encodingsTool1.SelectedEncoding = Encoding.ASCII;
            richTextBox2.Lines = File.ReadAllLines(filePath, Encoding.ASCII);

            this.deepSearch = deepSearch;
            DetectFormat();
        }
    }
    /// <summary>
    /// Tree node holds a subtitle format
    /// </summary>
    public class TreeNode_SubtitleFormat : TreeNode
    {
        SubtitlesFormat _SubtitleFormat;
        /// <summary>
        /// Get or set the subtitle format
        /// </summary>
        public SubtitlesFormat Format
        {
            get { return _SubtitleFormat; }
            set
            {
                _SubtitleFormat = value;
                this.Text = _SubtitleFormat.Name;
            }
        }
    }
}
