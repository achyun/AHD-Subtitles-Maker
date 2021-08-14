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
using AHD.SM.ASMP;
using System.Reflection;
using System.Resources;
namespace AHD.SM.Controls
{
    public partial class PreparedTextEditor : UserControl
    {
        public PreparedTextEditor()
        {
            InitializeComponent();
            //fill up fonts
            for (int i = 0; i < FontFamily.Families.Length; i++)
            {
                ComboBox_font.Items.Add(FontFamily.Families[i].Name);
            }
        }

        string startText = "";
        bool Save = false;
        bool autoHideStrip = false;
        bool update = true;
        bool isloadingstyles = false;
        ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource", Assembly.GetExecutingAssembly());
        public event EventHandler TextChanged;
        public event EventHandler SaveRequest;
        public event EventHandler TextSelectionChanged;
        public event EventHandler EditStylesRequest;

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
        /// <summary>
        /// Get if right to left
        /// </summary>
        public bool TextRightToLeft
        { get { return toolStripButton_rtl.Checked; } }
        public void RefreshText(string rtf)
        {
            //richTextBox1.Clear();
            richTextBox1.Rtf = rtf;

            // UpdateButtons();
        }
        public string TextRTF
        {
            get { return richTextBox1.Rtf; }
        }
        public string SelectedTextRTF
        {
            get { return richTextBox1.SelectedRtf; }
        }
        public bool WordWrap
        {
            get { return richTextBox1.WordWrap; }
            set { richTextBox1.WordWrap = value; }
        }
        /// <summary>
        /// Get the length of the text in the editor (not the subtitle text)
        /// </summary>
        public int TextLength
        {
            get
            {
                return richTextBox1.Text.Length;
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
        { get { return toolStrip1.Visible; } set { toolStrip1.Visible = value; } }
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

        public void Copy()
        { richTextBox1.Copy(); }
        public void Cut()
        { richTextBox1.Cut(); }
        public void Paste()
        { richTextBox1.Paste(); }
        public void Delete()
        {
            richTextBox1.SelectedText = "";
            int index = richTextBox1.GetLineFromCharIndex(richTextBox1.GetFirstCharIndexOfCurrentLine());
            if (index < richTextBox1.Lines.Length)
            {
                if (richTextBox1.Lines[index].Length == 0)
                {
                    richTextBox1.Select(richTextBox1.GetFirstCharIndexOfCurrentLine(), 1);
                    richTextBox1.SelectedText = "";
                }
            }
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
            string txt = richTextBox1.Text;

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
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (startText != "")
                if (richTextBox1.Text != startText)
                    Save = true;
            if (TextChanged != null)
                TextChanged(this, new EventArgs());

            UpdateStatus();
        }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtons();
            if (TextSelectionChanged != null)
                TextSelectionChanged(sender, e);
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateButtons();
            OnKeyDown(e);

            if ((ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                switch (e.KeyCode)
                {
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
            if (Save)
            {
                if (SaveRequest != null)
                    SaveRequest(this, new EventArgs());
            }
            Save = false;
        }
        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (Save)
            {
                if (SaveRequest != null)
                    SaveRequest(this, new EventArgs());
            }
            Save = false;

            if (autoHideStrip)
                toolStrip1.Visible = false;
        }
        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (autoHideStrip)
            {
                toolStrip1.Visible = true;
                OnEnter(e);
            }
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
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            toolStripButton_rtl.Checked = !toolStripButton_rtl.Checked;
            richTextBox1.RightToLeft = toolStripButton_rtl.Checked ? System.Windows.Forms.RightToLeft.Yes :
                System.Windows.Forms.RightToLeft.No;
        }
        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dial = new ColorDialog();
            dial.Color = richTextBox1.BackColor;
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                richTextBox1.BackColor = dial.Color;
                Save = true;
            }
        }
        // Lower case
        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            Replace(richTextBox1.SelectedText, richTextBox1.SelectedText.ToLower());
        }
        // Upper case
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Replace(richTextBox1.SelectedText, richTextBox1.SelectedText.ToUpper());
        }
        private void toolStripButton5_CheckedChanged(object sender, EventArgs e)
        {
            toolStrip_styling.Visible = toolStripButton5.Checked;
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            EditStylesRequest?.Invoke(this, new EventArgs());
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
    }
}
