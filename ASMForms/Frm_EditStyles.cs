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
using AHD.SM.ASMP;
using AHD.Forms;
using System.Reflection;
using System.Resources;

namespace AHD.SM.Forms
{
    public partial class Frm_EditStyles : Form
    {
        public Frm_EditStyles(ASMPFontStyle[] styles)
        {
            InitializeComponent();
            if (styles != null)
                foreach (ASMPFontStyle st in styles)
                    listBox1.Items.Add(st);
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
       Assembly.GetExecutingAssembly());
        public List<ASMPFontStyle> EnteredStyles
        {
            get
            {
                List<ASMPFontStyle> styles = new List<ASMPFontStyle>();
                foreach (ASMPFontStyle st in listBox1.Items)
                    styles.Add(st);
                return styles;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show(resources.GetString("Message_PleaseAddAtLeastOneStyle"));
                return;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }
        // Add
        private void button3_Click(object sender, EventArgs e)
        {
            EnterNameForm frm_name = new EnterNameForm(resources.GetString("Title_PleaseEnterTheStyleName"), resources.GetString("Word_Default"), true, false);
            if (frm_name.ShowDialog(this) == DialogResult.OK)
            {
                FontDialog frm_font = new FontDialog();
                if (frm_font.ShowDialog(this) == DialogResult.OK)
                {
                    ColorDialog frm_color = new ColorDialog();
                    if (frm_color.ShowDialog(this) == DialogResult.OK)
                    {
                        ASMPFontStyle FF = new ASMPFontStyle(frm_name.EnteredName, frm_font.Font, frm_color.Color);
                        listBox1.Items.Add(FF);
                    }
                }
            }
        }
        // Remove
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.Black;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            richTextBox1.BackColor = Color.White;
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                ASMPFontStyle ff = (ASMPFontStyle)listBox1.SelectedItem;
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = ff.Color;
                richTextBox1.SelectionFont = ff.Font;

                richTextBox1.SelectedText = "";
            }
        }
    }
}
