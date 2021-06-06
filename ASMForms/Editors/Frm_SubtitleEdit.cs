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

namespace AHD.SM.Forms
{
    public partial class Frm_SubtitleEdit : Form
    {
        /// <summary>
        /// A window of edit subtitle
        /// </summary>
        /// <param name="subtitleItem">The item to edit subtitle of</param>
        /// <param name="track">The parent track</param>
        /// <param name="editorBackColor">The back color of the editor</param>
        /// <param name="allowErrors">Allow the errors so that the ok button keep enabled even there are errors.</param>
        public Frm_SubtitleEdit(ListViewItem_Subtitle subtitleItem, SubtitlesTrack track,
            Color editorBackColor, bool allowErrors, bool showPreparedText, string preparedTextRTF,
            bool cutSelectionOfPreparedText, bool preparedTextWordWrap)
        {
            InitializeComponent();
            this.subtitleItem = subtitleItem;
            // Prepared Text
            this.splitContainer1.Panel2Collapsed = !showPreparedText;
            this.preparedTextEditor1.RefreshText(preparedTextRTF);
            this.enablePreparedText = showPreparedText;
            this.cutSelectionOfPreparedText = cutSelectionOfPreparedText;
            // Load subtitle text
            this.subtitleEditor1.SubtitlesTrack = track;
            this.subtitleEditor1.SelectedItems = new ListViewItem_Subtitle[] { subtitleItem };
            this.subtitleEditor1.EditorBackColor = editorBackColor;
            this.allowErrors = allowErrors;
            this.checkBox_word_wrap.Checked = preparedTextEditor1.WordWrap = preparedTextWordWrap;
            this.subtitleEditor1.Select();
        }
        private ListViewItem_Subtitle subtitleItem;
        private bool enablePreparedText;
        private bool allowErrors = false;
        private bool cutSelectionOfPreparedText;
        /// <summary>
        /// Use this event instead of "FormClosing"
        /// </summary>
        public event EventHandler OkButtonPressed;
        public event EventHandler EditStylesRequest;
        public string PreparedTextRTFAfterChange
        {
            get { return preparedTextEditor1.TextRTF; }
        }
        public void SelectAll()
        {
            subtitleEditor1.SelectAll();
        }
        public void RefreshStyles(ASMPFontStyle[] styles)
        {
            subtitleEditor1.RefreshStyles(styles);
            preparedTextEditor1.RefreshStyles(styles);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            subtitleEditor1.Save();
            if (enablePreparedText && cutSelectionOfPreparedText)
            {
                preparedTextEditor1.Delete();
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
            if (OkButtonPressed != null)
                OkButtonPressed(this, null);
        }
        //cancel
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
        private void subtitleEditor1_ErrorVisibleChanged(object sender, EventArgs e)
        {
            if (!allowErrors)
                button1.Enabled = !subtitleEditor1.HasErrors;
        }
        private void subtitleEditor1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ModifierKeys == Keys.Alt && e.KeyCode == Keys.Space)
                button1_Click(this, null);
            if (ModifierKeys == Keys.Alt && e.KeyCode == Keys.Return)
                button1_Click(this, null);
        }
        private void preparedTextEditor1_TextSelectionChanged(object sender, EventArgs e)
        {
            if (enablePreparedText)
                subtitleEditor1.RefreshText(preparedTextEditor1.SelectedTextRTF);
        }
        private void checkBox_word_wrap_CheckedChanged(object sender, EventArgs e)
        {
            preparedTextEditor1.WordWrap = checkBox_word_wrap.Checked;
        }
        private void subtitleEditor1_EditStylesRequest(object sender, EventArgs e)
        {
            EditStylesRequest?.Invoke(this, new EventArgs());
        }
    }
}
