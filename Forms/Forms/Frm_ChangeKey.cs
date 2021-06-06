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
    public partial class Frm_ChangeKey : Form
    {
        public Frm_ChangeKey(string old_key)
        {
            InitializeComponent();
            textBox1.Text = old_key;
            textBox1.Select();
        }

        public event EventHandler<KeyPressedArgs> KeyPressed;

        public string EnteredValue
        {
            get { return textBox1.Text; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Keys k = Keys.None;
            if (textBox1.Text.Contains("+"))
            {
                string[] codes = textBox1.Text.Split('+');

                if (codes.Length > 0)
                {
                    for (int i = 0; i < codes.Length; i++)
                    {
                        switch (codes[i])
                        {
                            case "Shift": k |= Keys.Shift; break;
                            case "Control": k |= Keys.Control; break;
                            case "Alt": k |= Keys.Alt; break;
                            default: k |= (Keys)Enum.Parse(typeof(Keys), codes[i]); break;
                        }
                    }
                }
            }
            else
            {
                k = (Keys)Enum.Parse(typeof(Keys), textBox1.Text);
            }
            KeyPressedArgs args = new KeyPressedArgs(k);
            if (KeyPressed != null)
            {
                KeyPressed(this, args);
            }
            if (args.Cancel)
                return;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void Frm_ChangeKey_KeyDown(object sender, KeyEventArgs e)
        {
            // Keys not allowed to be alone
            switch (e.KeyCode)
            {
                case Keys.Shift:
                case Keys.ShiftKey:
                case Keys.LShiftKey:
                case Keys.RShiftKey:

                case Keys.ControlKey:
                case Keys.LControlKey:
                case Keys.RControlKey:
                case Keys.Control:

                case Keys.Alt:
                case Keys.Menu:
                case Keys.LMenu:
                case Keys.RMenu:
                case Keys.IMENonconvert:
                    return;
            }
            string k = "";
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
            {
                k += "Shift+";
            }
            if ((ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                k += "Alt+";
            }
            if ((ModifierKeys & Keys.Control) == Keys.Control)
            {
                k += "Control+";
            }
            k += e.KeyCode.ToString();
            textBox1.Text = k;
        }
        private void label_thekey_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = textBox1.Text.Length > 0;
        }
    }
}
