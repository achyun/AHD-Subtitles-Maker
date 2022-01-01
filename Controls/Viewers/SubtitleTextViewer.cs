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
using AHD.SM.ASMP;

namespace AHD.SM.Controls
{
    public partial class SubtitleTextViewer : UserControl
    {
        public SubtitleTextViewer()
        {
            InitializeComponent();
        }
        public event EventHandler EditRequest;
        public void ViewText(SubtitleText text)
        {
            richTextBox1.Clear();
            richTextBox1.SelectionStart = 0;
            richTextBox1.RightToLeft = text.RighttoLeft ? System.Windows.Forms.RightToLeft.Yes :
            System.Windows.Forms.RightToLeft.No;
            if (text.TextLines.Count > 0)
            {
                foreach (SubtitleLine line in text.TextLines)
                {
                    foreach (SubtitleChar chr in line.Chars)
                    {
                        if (chr.Font != null)
                            richTextBox1.SelectionFont = chr.Font;
                        else
                            richTextBox1.SelectionFont = new Font("Tahoma", 8, FontStyle.Regular);
                        richTextBox1.SelectionBullet = true;
                        if (chr.Color != null)
                            richTextBox1.SelectionColor = chr.Color;
                        else
                            richTextBox1.SelectionColor = Color.White;
                        richTextBox1.SelectedText = chr.TheChar.ToString();
                        switch (line.Alignement)
                        {
                            case LineAlignement.Center: richTextBox1.SelectionAlignment = HorizontalAlignment.Center; break;
                            case LineAlignement.Left: richTextBox1.SelectionAlignment = HorizontalAlignment.Left; break;
                            case LineAlignement.Right: richTextBox1.SelectionAlignment = HorizontalAlignment.Right; break;
                        }
                        richTextBox1.SelectionBullet = false;
                    }
                    if (line != text.TextLines[text.TextLines.Count - 1])
                        richTextBox1.SelectedText = "\n";
                }
            }
            else
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }
        }
        public void ViewText(string text, Font font, Color color, bool rtl)
        {
            richTextBox1.Clear();
            richTextBox1.SelectionStart = 0;
            richTextBox1.RightToLeft = rtl ? System.Windows.Forms.RightToLeft.Yes :
            System.Windows.Forms.RightToLeft.No;
            richTextBox1.SelectionFont = font;
            richTextBox1.SelectionBullet = true;
            richTextBox1.SelectionColor = color;
            richTextBox1.SelectedText = text;
            richTextBox1.SelectionBullet = false;
        }
        public void HideText()
        {
            richTextBox1.Clear();
        }
        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (EditRequest != null)
                EditRequest(sender, e);
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                richTextBox1.BackColor = base.BackColor = value;
            }
        }
    }
}
