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

namespace AHD.Forms
{
    /// <summary>
    /// A dialog used to enter name
    /// </summary>
    public partial class EnterNameForm : Form
    {
        /// <summary>
        /// A dialog used to enter name
        /// </summary>
        public EnterNameForm()
        {
            InitializeComponent();
            this.Text = "Enter Name";
        }
        /// <summary>
        /// A dialog used to enter name
        /// </summary>
        /// <param name="caption">The text of the dialog</param>
        /// <param name="defaultName">The default name of the text box</param>
        /// <param name="AutoDisableButton">If true, the dialog will auto disable ok button when the entered text is null</param>
        /// <param name="PasswordChars">If true, use system password chars to hide entered text</param>
        public EnterNameForm(string caption, string defaultName,
            bool AutoDisableButton, bool PasswordChars)
        {
            InitializeComponent();
            this.Text = caption;
            textBox1.Text = defaultName;
            this.AutoDisableButton = AutoDisableButton;
            if (!AutoDisableButton)
                button1.Enabled = true;
            textBox1.UseSystemPasswordChar = PasswordChars;
        }

        bool AutoDisableButton = true;
        /// <summary>
        /// Rised up when the user clicked the ok button, allows to cancel the form closing using the args
        /// </summary>
        public event EventHandler<EnterNameFormOkPressedArgs> OkPressed;

        /// <summary>
        /// Get the text entered by the name
        /// </summary>
        public string EnteredName
        { get { return textBox1.Text; } }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (AutoDisableButton)
                button1.Enabled = textBox1.Text.Length > 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            EnterNameFormOkPressedArgs args = new EnterNameFormOkPressedArgs(textBox1.Text);
            if (OkPressed != null)
                OkPressed(this, args);
            if (!args.Cancel)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}
