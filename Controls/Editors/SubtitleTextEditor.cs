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
using System.Collections.Generic;
using AHD.SM.ASMP;
using System.Reflection;
using System.Resources;
namespace AHD.SM.Controls
{
    public partial class SubtitleTextEditor : UserControl
    {
        public SubtitleTextEditor()
        {
            InitializeComponent();
            //fill up fonts
            for (int i = 0; i < FontFamily.Families.Length; i++)
            {
                ComboBox_font.Items.Add(FontFamily.Families[i].Name);
            }
            toolStripComboBox_line_break_mode.SelectedIndex = 0;
        }

        string startRTF = "";
        SubtitleText text;
       public bool Save = false;
        bool isSaving = false;
        bool autoHideStrip = false;
        bool update = true;
        bool isloadingstyles = false;
        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource", Assembly.GetExecutingAssembly());

        /// <summary>
        /// Rised up when the user changed the text or color, fornt ... etc
        /// </summary>
        public event EventHandler SubtitleTextChanged;
        public event EventHandler StyleEditorVisibleChanged;
        public event EventHandler SubtitleTextLengthChanged;
        public event EventHandler SubtitleTextRightToLeftValueChanged;
        public event EventHandler EditStylesRequest;

        /// <summary>
        /// Get or set the subtitle text
        /// </summary>
        public SubtitleText SubtitleText
        {
            get { return text; }
            set
            {
                text = value;
                if (value != null)
                {
                    this.Enabled = true;
                    RefreshText();
                }
                else
                {
                    this.Enabled = false;
                    richTextBox1.Text = "";
                }
                startRTF = richTextBox1.Rtf;
                Save = false;
            }
        }
        /// <summary>
        /// Get if right to left
        /// </summary>
        public bool SubtitleTextRightToLeft
        { get { return toolStripButton_rtl.Checked; } }
        public bool ShowMoveButton
        {
            get { return toolStripButton2.Visible; }
            set { toolStripSeparator9.Visible = toolStripButton2.Visible = value; }
        }
        public void RefreshText()
        {
            if (isSaving)
                return;
            richTextBox1.Clear();
            richTextBox1.SelectionStart = 0;
            richTextBox1.RightToLeft = toolStripButton_rtl.Checked ? System.Windows.Forms.RightToLeft.Yes :
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

                        //richTextBox1.SelectionBullet = true;
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
                        //richTextBox1.SelectionBullet = false;
                    }
                    if (line != text.TextLines[text.TextLines.Count - 1])
                        richTextBox1.SelectedText = "\n";
                }
            }
            else
            {
                richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            }

            UpdateButtons();
        }
        public void SetTextRTF(string rtf)
        {
            richTextBox1.Rtf = rtf;
            UpdateButtons();
        }
        /// <summary>
        /// Get the length of the text in the editor (not the subtitle text)
        /// </summary>
        public int TextLength
        {
            get
            {
                if (text == null)
                    return 0;
                else
                {
                    return richTextBox1.Text.Length;
                }
            }
        }
        /// <summary>
        /// Get or set value indecate wether to active auto style editor hide
        /// </summary>
        public bool AutoHideStyleEditor
        { get { return autoHideStrip; } set { autoHideStrip = value; } }
        /// <summary>
        /// Get or set a value indecate wether to show the toolstrip that edit font, color and position
        /// </summary>
        public bool ShowStyleEditor
        {
            get { return toolStrip1.Visible; }
            set
            {
                toolStrip_line_break.Visible = toolStrip_styling.Visible = toolStrip1.Visible = value;
            }
        }
        public bool ShowStatusStrip
        { get { return statusStrip1.Visible; } set { statusStrip1.Visible = value; } }
        public bool ReadOnly
        { get { return richTextBox1.ReadOnly; } set { richTextBox1.ReadOnly = value; } }
        public void UpdateButtons()
        {
            update = false;
            if (richTextBox1.SelectionFont != null)
            {
                toolStripTextBox_fontSize.Text = richTextBox1.SelectionFont.Size.ToString();
                ComboBox_font.SelectedItem = richTextBox1.SelectionFont.Name;

                toolStripButton_bold.Checked = richTextBox1.SelectionFont.Bold;
                toolStripButton_italic.Checked = richTextBox1.SelectionFont.Italic;
                toolStripButton_underline.Checked = richTextBox1.SelectionFont.Underline;
                toolStripButton_strikeout.Checked = richTextBox1.SelectionFont.Strikeout;
            }
            switch (richTextBox1.SelectionAlignment)
            {
                case HorizontalAlignment.Left:
                    leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = true;
                    rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = false;
                    centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = false;
                    break;
                case HorizontalAlignment.Center:
                    leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = false;
                    rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = false;
                    centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = true;
                    break;
                case HorizontalAlignment.Right:
                    leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = false;
                    rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = true;
                    centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = false;
                    break;
            }
            if (text != null)
                toolStripButton_rtl.Checked = text.RighttoLeft;
            update = true;
        }
        public void UpdateSelectionFont()
        {
            if (ComboBox_font.SelectedIndex >= 0)
            {
                FontStyle style = FontStyle.Regular;
                if (!FontFamily.Families[ComboBox_font.SelectedIndex].IsStyleAvailable(FontStyle.Regular))
                    style ^= FontStyle.Regular;
                if (toolStripButton_bold.Checked)
                    style |= FontStyle.Bold;
                if (toolStripButton_italic.Checked)
                    style |= FontStyle.Italic;
                if (toolStripButton_underline.Checked)
                    style |= FontStyle.Underline;
                if (toolStripButton_strikeout.Checked)
                    style |= FontStyle.Strikeout;
                //create the font
                richTextBox1.SelectionFont = new Font(ComboBox_font.SelectedItem.ToString(), float.Parse(toolStripTextBox_fontSize.Text), style);
            }
        }
        public void SaveTextToSubtitle()
        {
            /*if (richTextBox1.ReadOnly)
                return;
            if (text == null)
                return;
            text.RighttoLeft = toolStripButton_rtl.Checked;
            text.TextLines.Clear();
            foreach (string ln in richTextBox1.Lines)
            {
                text.TextLines.Add(new SubtitleLine());
            }
            int currentLine = 0;
            int oldStart = richTextBox1.SelectionStart;
            string oldSelection = richTextBox1.SelectedText;
            richTextBox1.SelectionStart = 0;// 1 ?
            bool needToSetAlign = true;
           
            foreach (char chr in richTextBox1.Text.ToCharArray())
            {
                if (chr != '\n')
                {
                    if (needToSetAlign)
                    {
                        LineAlignement align = LineAlignement.Center;
                        switch (richTextBox1.SelectionAlignment)
                        {
                            case HorizontalAlignment.Left: align = LineAlignement.Left; break;
                            case HorizontalAlignment.Right: align = LineAlignement.Right; break;
                        }
                        text.TextLines[currentLine].Alignement = align;
                        needToSetAlign = false;
                    }
                    text.TextLines[currentLine].Chars.Add(new SubtitleChar(chr, richTextBox1.SelectionFont, richTextBox1.SelectionColor));
                }
                else
                {
                    currentLine++;
                    needToSetAlign = true;
                }
                richTextBox1.SelectionStart++;
            }
            richTextBox1.SelectionStart = oldStart + oldSelection.Length;
            richTextBox1.Select(oldStart, oldSelection.Length);*/
            if (richTextBox1.ReadOnly)
                return;
            if (text == null)
                return;
            if (isSaving)
                return;
            isSaving = true;
            text.RighttoLeft = toolStripButton_rtl.Checked;
            text.TextLines.Clear();
            foreach (string ln in richTextBox1.Lines)
            {
                text.TextLines.Add(new SubtitleLine());
            }
            int currentLine = 0;
            int oldStart = richTextBox1.SelectionStart;
            string oldSelection = richTextBox1.SelectedText;
            richTextBox1.Visible = false;//disable so user won't see the strange things that would happen to the box lol
            richTextBox1.DeselectAll();
            richTextBox1.SelectionStart = 0;// 1 ?
            bool needToSetAlign = true;

            for (int i = 0; i < richTextBox1.Text.ToCharArray().Length; i++)
            {
                // select the char
                richTextBox1.Select(i, 1);
                string currentChar = richTextBox1.SelectedText;

                if (currentChar != "\n")
                {
                    if (needToSetAlign)
                    {
                        LineAlignement align = LineAlignement.Center;
                        switch (richTextBox1.SelectionAlignment)
                        {
                            case HorizontalAlignment.Left: align = LineAlignement.Left; break;
                            case HorizontalAlignment.Right: align = LineAlignement.Right; break;
                        }
                        text.TextLines[currentLine].Alignement = align;
                        needToSetAlign = false;
                    }
                    text.TextLines[currentLine].Chars.Add(new SubtitleChar(currentChar.ToCharArray()[0],
                        richTextBox1.SelectionFont, richTextBox1.SelectionColor));
                }
                else
                {
                    currentLine++;
                    needToSetAlign = true;
                }
                richTextBox1.SelectionStart++;
            }
            richTextBox1.Visible = true;
            isSaving = false;
        }

        public void Copy()
        { richTextBox1.Copy(); }
        public void Cut()
        { richTextBox1.Cut(); }
        public void Paste()
        { richTextBox1.Paste(); }
        public void Delete()
        {
            richTextBox1.SelectedText = "";
        }
        public void DeselectAll()
        { richTextBox1.DeselectAll(); }
        public void SelectAll()
        { richTextBox1.SelectAll(); }
        public void SelectWord(int startIndex, int length)
        {
            richTextBox1.Select(startIndex, length);
        }
        public void SelectWord(string word)
        {
            string txt = text.ToString();

            for (int i = 0; i < txt.Length; i++)
            {
                if (txt.Length - i >= word.Length)
                {
                    if (txt.Substring(i, word.Length) == word)
                    {
                        richTextBox1.Select(i, word.Length);
                    }
                }
            }
        }
        public void Replace(string word, string replacement)
        {
            SelectWord(word);
            richTextBox1.SelectedText = replacement;
            SelectWord(replacement);
        }
        public void ChangeFont()
        {
            FontDialog dial = new FontDialog();
            dial.Font = richTextBox1.SelectionFont;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                richTextBox1.SelectionFont = dial.Font;
                Save = true;
                UpdateButtons();
            }
        }
        public void ChangeColor()
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = richTextBox1.SelectionColor;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                richTextBox1.SelectionColor = dial.Color;
                Save = true;
            }
        }
        public void UpdateStatus()
        {
            int lineIndex = richTextBox1.GetLineFromCharIndex(richTextBox1.GetFirstCharIndexOfCurrentLine());
            toolStripStatusLabel1.Text = resources.GetString("Status_Line") + " " +
                (lineIndex + 1) + "/" + richTextBox1.Lines.Length + ", " +
                resources.GetString("Status_LineCharsCount") + "=" +
                ((richTextBox1.Lines.Length > lineIndex) ? richTextBox1.Lines[lineIndex].Length.ToString() : "0") + ", " +
               resources.GetString("Status_TotalCharsCount") + "=" + richTextBox1.Text.Length;
        }
        public void RefreshStyles(ASMPFontStyle[] styles)
        {
            isloadingstyles = true;
            toolStripComboBox_styles.Items.Clear();
            foreach (ASMPFontStyle st in styles)
                toolStripComboBox_styles.Items.Add(st);
            if (toolStripComboBox_styles.Items.Count > 0)
                toolStripComboBox_styles.SelectedIndex = 0;
            isloadingstyles = false;
        }
        private void ApplyStyle(int index)
        {
            if (isloadingstyles)
                return;
            if (index >= 0 && index < toolStripComboBox_styles.Items.Count)
            {
                ASMPFontStyle fon = (ASMPFontStyle)toolStripComboBox_styles.Items[index];
                if (fon != null)
                {
                    // Set font
                    if (fon.Font != null)
                        richTextBox1.SelectionFont = fon.Font;
                    // Set color
                    if (fon.Color != null)
                        richTextBox1.SelectionColor = fon.Color;
                    UpdateButtons();
                }
            }
        }
        public void ApplyLineBreak()
        {
            if (text == null)
                return;
            int mode = toolStripComboBox_line_break_mode.SelectedIndex;
            // capital letter and all
            if ((mode == 0) || (mode == 3))
            {
                for (int line = 0; line < text.TextLines.Count; line++)
                {
                    for (int chr = 0; chr < text.TextLines[line].Chars.Count; chr++)
                    {
                        char currentChar = text.TextLines[line].Chars[chr].TheChar;
                        if (Char.IsUpper(currentChar))
                        {
                            // this is it !!
                            // insert a line-break here !
                            // we need to check the previous line first
                            bool can_do_it = true;
                            int bef = chr - 1;
                            if (bef >= 0)
                            {
                                if (text.TextLines[line].Chars[bef].TheChar == '\n')
                                {
                                    can_do_it = false;
                                }
                            }
                            else
                            {
                                // Already at the first of the line !
                                can_do_it = false;
                            }
                            if (can_do_it)
                            {
                                // insert it !!
                                text.TextLines[line].Chars.Insert(chr, new SubtitleChar('\n', text.TextLines[line].Chars[chr].Font, text.TextLines[line].Chars[chr].Color));
                                chr += 2;
                            }
                        }
                    }
                }
            }
            // char-limit and all
            if ((mode == 1) || (mode == 3))
            {
                int val = 0;
                if (int.TryParse(toolStripTextBox_brk_chars_limit.Text, out val))
                {
                    if (val > 0 && val <= 100)
                    {
                        for (int line = 0; line < text.TextLines.Count; line++)
                        {
                            int line_count = text.TextLines[line].Chars.Count;

                            if (line_count > val)
                            {
                                int segmants = line_count / val;
                                int ind = val;
                                for (int i = 0; i < segmants; i++)
                                {
                                    if (ind < text.TextLines[line].Chars.Count)
                                    {
                                        // We can only insert break when the index fall into a space
                                        if (text.TextLines[line].Chars[ind].TheChar != ' ')
                                        {
                                            while (ind < text.TextLines[line].Chars.Count)
                                            {
                                                ind++;
                                                if (ind >= text.TextLines[line].Chars.Count)
                                                    break;
                                                if (text.TextLines[line].Chars[ind].TheChar == ' ')
                                                    break;
                                            }
                                        }
                                        if (ind < text.TextLines[line].Chars.Count)
                                            text.TextLines[line].Chars.Insert(ind, new SubtitleChar('\n', text.TextLines[line].Chars[ind].Font, text.TextLines[line].Chars[ind].Color));
                                        ind += 1 + val;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Char limit value cannot be 0 nor larger than 100");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid char limit value.");
                }
            }
            // punctuation and all
            if ((mode == 2) || (mode == 3))
            {
                if (toolStripTextBox_punctuation.Text.Length == 0)
                {
                    MessageBox.Show("The punctuation text cannot be empty.");
                    return;
                }
                string[] punctuations = toolStripTextBox_punctuation.Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                List<string> check_list = new List<string>(punctuations);
                for (int line = 0; line < text.TextLines.Count; line++)
                {
                    for (int chr = 0; chr < text.TextLines[line].Chars.Count; chr++)
                    {
                        string currentChar = text.TextLines[line].Chars[chr].TheChar.ToString();
                        if (check_list.Contains(currentChar))
                        {
                            // this is it !!
                            // insert a line-break here !
                            // we need to check the next line first
                            bool can_do_it = true;
                            int af = chr + 1;
                            if (af < text.TextLines[line].Chars.Count)
                            {
                                if (text.TextLines[line].Chars[af].TheChar == '\n')
                                {
                                    can_do_it = false;
                                }
                            }
                            else
                            {
                                // Already at the first of the line !
                                can_do_it = false;
                            }
                            if (can_do_it)
                            {
                                // insert it !!
                                text.TextLines[line].Chars.Insert(af, new SubtitleChar('\n', text.TextLines[line].Chars[chr].Font, text.TextLines[line].Chars[chr].Color));
                                chr += 2;
                            }
                        }
                    }
                }
            }

            RefreshText();
            SaveTextToSubtitle();

            // Do a fix, remove space from the start of each line
            for (int line = 0; line < text.TextLines.Count; line++)
            {
                while (text.TextLines[line].Chars.Count > 0)
                {
                    if (text.TextLines[line].Chars[0].TheChar == ' ')
                    {
                        text.TextLines[line].Chars.RemoveAt(0);
                    }
                    else
                        break;
                }
            }
            RefreshText();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (startRTF != "")
            {
                if (richTextBox1.Rtf != startRTF)
                { Save = true; }
            }
            if (SubtitleTextLengthChanged != null)
                SubtitleTextLengthChanged(this, new EventArgs());

            UpdateStatus();
        }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateButtons();
            OnKeyDown(e);

            if ((ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.B:
                        {
                            ApplyLineBreak();
                            break;
                        }
                    case Keys.D1:
                        {
                            ApplyStyle(0);
                            break;
                        }
                    case Keys.D2:
                        {
                            ApplyStyle(1);
                            break;
                        }
                    case Keys.D3:
                        {
                            ApplyStyle(2);
                            break;
                        }
                    case Keys.D4:
                        {
                            ApplyStyle(3);
                            break;
                        }
                    case Keys.D5:
                        {
                            ApplyStyle(4);
                            break;
                        }
                    case Keys.D6:
                        {
                            ApplyStyle(5);
                            break;
                        }
                    case Keys.D7:
                        {
                            ApplyStyle(6);
                            break;
                        }
                    case Keys.D8:
                        {
                            ApplyStyle(7);
                            break;
                        }
                    case Keys.D9:
                        {
                            ApplyStyle(8);
                            break;
                        }
                }
            }
        }
        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (pselectionStart != richTextBox1.SelectionStart) 
            UpdateButtons(); UpdateStatus();
            // pselectionStart = richTextBox1.SelectionStart;
        }
        private void ComboBox_font_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox_font.SelectedIndex < 0)
                return;
            //enable style buttons
            if (!FontFamily.Families[ComboBox_font.SelectedIndex].IsStyleAvailable(FontStyle.Bold))
            { toolStripButton_bold.Checked = toolStripButton_bold.Enabled = false; }
            if (!FontFamily.Families[ComboBox_font.SelectedIndex].IsStyleAvailable(FontStyle.Italic))
            { toolStripButton_italic.Checked = toolStripButton_italic.Enabled = false; }
            if (!FontFamily.Families[ComboBox_font.SelectedIndex].IsStyleAvailable(FontStyle.Strikeout))
            { toolStripButton_strikeout.Checked = toolStripButton_strikeout.Enabled = false; }
            if (!FontFamily.Families[ComboBox_font.SelectedIndex].IsStyleAvailable(FontStyle.Underline))
            { toolStripButton_underline.Checked = toolStripButton_underline.Enabled = false; }
            UpdateSelectionFont();
        }
        private void toolStripButton_bold_Click(object sender, EventArgs e)
        {
            UpdateSelectionFont(); Save = true;
        }
        private void toolStripButton_italic_Click(object sender, EventArgs e)
        {
            UpdateSelectionFont(); Save = true;
        }
        private void toolStripButton_underline_Click(object sender, EventArgs e)
        {
            UpdateSelectionFont(); Save = true;
        }
        private void toolStripButton_strikeout_Click(object sender, EventArgs e)
        {
            UpdateSelectionFont(); Save = true;
        }
        private void toolStripButton_leftAlign_Click(object sender, EventArgs e)
        {
            leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = true;
            rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = false;
            centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = false;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Left; Save = true;
        }
        private void toolStripButton_centerAlign_Click(object sender, EventArgs e)
        {
            leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = false;
            rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = false;
            centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = true;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center; Save = true;
        }
        private void toolStripButton_rightAlign_Click(object sender, EventArgs e)
        {
            leftToolStripMenuItem.Checked = toolStripButton_leftAlign.Checked = false;
            rightToolStripMenuItem.Checked = toolStripButton_rightAlign.Checked = true;
            centerToolStripMenuItem.Checked = toolStripButton_centerAlign.Checked = false;
            richTextBox1.SelectionAlignment = HorizontalAlignment.Right; Save = true;
        }
        private void toolStripTextBox_fontSize_TextChanged(object sender, EventArgs e)
        {
            if (!update)
                return;
            float size = 0;
            if (float.TryParse(toolStripTextBox_fontSize.Text, out size))
            {
                UpdateSelectionFont(); Save = true;
            }
            else
            {
                toolStripTextBox_fontSize.Text = richTextBox1.SelectionFont.Size.ToString();
            }
        }
        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            ChangeColor();
        }
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            ChangeFont();
        }
        private void ComboBox_font_DropDownClosed(object sender, EventArgs e)
        {
            richTextBox1.Select();
            Save = true;
        }
        //Save methods
        private void richTextBox1_Validated(object sender, EventArgs e)
        {
            if (isSaving)
                return;
            if (Save)
            {
                if (startRTF != richTextBox1.Rtf)
                {
                    SaveTextToSubtitle();
                    if (SubtitleTextChanged != null)
                        SubtitleTextChanged(this, new EventArgs());
                    startRTF = richTextBox1.Rtf;
                }
            }
            Save = false;
        }
        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (isSaving)
                return;
            if (Save)
            {
                if (startRTF != richTextBox1.Rtf)
                {
                    SaveTextToSubtitle();
                    if (SubtitleTextChanged != null)
                        SubtitleTextChanged(this, new EventArgs());
                    startRTF = richTextBox1.Rtf;
                }
            }
            Save = false;

            if (autoHideStrip)
                toolStrip1.Visible = false;
        }
        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (autoHideStrip)
                toolStrip1.Visible = true;
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }
        private void toolStrip1_VisibleChanged(object sender, EventArgs e)
        {
            if (StyleEditorVisibleChanged != null)
                StyleEditorVisibleChanged(sender, e);
        }
        //change location
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (this.text == null) return;
            Frm_SubtitlePosition frm = new Frm_SubtitlePosition(this.text);
            frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y - frm.Size.Height);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                if (SubtitleTextChanged != null)
                    SubtitleTextChanged(this, new EventArgs());
            }
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            text.RighttoLeft = !text.RighttoLeft;
            toolStripButton_rtl.Checked = text.RighttoLeft;
            richTextBox1.RightToLeft = toolStripButton_rtl.Checked ? System.Windows.Forms.RightToLeft.Yes :
                System.Windows.Forms.RightToLeft.No;
            if (SubtitleTextRightToLeftValueChanged != null)
                SubtitleTextRightToLeftValueChanged(this, null);
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
        // Upper case
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Replace(richTextBox1.SelectedText, richTextBox1.SelectedText.ToUpper());
        }
        // Lower case
        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            Replace(richTextBox1.SelectedText, richTextBox1.SelectedText.ToLower());
        }
        private void toolStripComboBox_styles_DropDownClosed(object sender, EventArgs e)
        {
            richTextBox1.Select();
            Save = true;
        }
        private void toolStripComboBox_styles_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyStyle(toolStripComboBox_styles.SelectedIndex);
        }
        // Show / Hide styles
        private void toolStripButton5_CheckStateChanged(object sender, EventArgs e)
        {
            toolStrip_styling.Visible = toolStripButton5.Checked;
        }
        // Edit project styles.
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            EditStylesRequest?.Invoke(this, new EventArgs());
        }
        // apply 1st style
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            ApplyStyle(0);
        }
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            ApplyStyle(1);
        }
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            ApplyStyle(2);
        }
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            ApplyStyle(3);
        }
        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            ApplyStyle(4);
        }
        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            ApplyStyle(5);
        }
        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            ApplyStyle(6);
        }
        private void toolStripButton_line_break_tools_Click(object sender, EventArgs e)
        {
            toolStrip_line_break.Visible = !toolStrip_line_break.Visible;
            toolStripButton_line_break_tools.Checked = toolStrip_line_break.Visible;
        }
        // DO BREAK
        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            ApplyLineBreak();
        }
        private void toolStripComboBox_line_break_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            toolStripTextBox_brk_chars_limit.Enabled = (toolStripComboBox_line_break_mode.SelectedIndex == 1) || (toolStripComboBox_line_break_mode.SelectedIndex == 3);
            toolStripTextBox_punctuation.Enabled = (toolStripComboBox_line_break_mode.SelectedIndex == 2) || (toolStripComboBox_line_break_mode.SelectedIndex == 3);

            toolStripComboBox_line_break_mode.ToolTipText = toolStripComboBox_line_break_mode.SelectedItem.ToString();
        }
        private void toolStripTextBox_brk_chars_limit_TextChanged(object sender, EventArgs e)
        {

        }
        private void toolStripTextBox_punctuation_TextChanged(object sender, EventArgs e)
        {
        }
        private void toolStripTextBox_brk_chars_limit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int val = 0;
            int.TryParse(toolStripTextBox_brk_chars_limit.Text, out val);
            if (val >= 0 && val <= 100)
                toolStripTextBox_brk_chars_limit.Text = val.ToString();
            else
                toolStripTextBox_brk_chars_limit.Text = "0";
        }

        /*private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "text|*.txt";
            if (sav.ShowDialog(this) == DialogResult.OK)
            {
                richTextBox1.SaveFile(sav.FileName);
            }
        }*/
    }
}
