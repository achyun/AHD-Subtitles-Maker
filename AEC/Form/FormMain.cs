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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AHD.Forms;
using System.IO;

namespace AEC
{
    public partial class FormMain : Form
    {
        public FormMain(string[] args)
        {
            if (Program.Settings.ShowLanguagesForm)
            {
                FrmLanguage frm = new FrmLanguage();
                frm.ShowDialog();
            }
            InitializeComponent();

            if (args != null)
            {
                if (args.Length == 0) return;
                textBox1.Text = textBox_orginal_path.Text = args[0];
                ShowOutput();
            }
        }
        private bool isDetectingInputEncoding = false;

        private void ShowOutput()
        {
            if (File.Exists(textBox_orginal_path.Text))
                richTextBox_out.Lines = File.ReadAllLines(textBox_orginal_path.Text, encodingsTool_output.SelectedEncoding);
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FrmLanguage frm = new FrmLanguage();
            frm.ShowDialog();
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker/wiki/AHD-Encoding-Convertor");
            }
            catch { }
        }
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_About"), "About AHD Encoding Converter " + Application.ProductVersion);
        }
        // Change input
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Message_OpenTheFileYouWantToConvertEncodingFor");
            op.Filter = Program.ResourceManager.GetString("Filter_AllFiles");
            op.FileName = textBox_orginal_path.Text;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_orginal_path.Text = op.FileName;
                textBox1.Text = op.FileName;
                ShowOutput();
            }
        }
        private void encodingsTool_input_SelectedEncodingChanged(object sender, EventArgs e)
        {
            if (isDetectingInputEncoding)
                return;
            if (File.Exists(textBox_orginal_path.Text))
                richTextBox_in.Lines = File.ReadAllLines(textBox_orginal_path.Text, encodingsTool_input.SelectedEncoding);
        }
        private void encodingsTool_output_SelectedEncodingChanged(object sender, EventArgs e)
        {
            ShowOutput();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Message_BrowseWhereYouWantToSaveTheFile");
            sav.Filter = Program.ResourceManager.GetString("Filter_AllFiles");
            sav.FileName = textBox1.Text;
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = sav.FileName;
            }
        }
        // save !
        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox_orginal_path.Text))
            {
                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseBrowseForInputFileFirst")
                    , "AHD Encoding Converter");
                return;
            }
            if (textBox1.Text.Length == 0)
            {
                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_PleaseBrowseWhereToSaveTheFileFirst"),
                    "AHD Encoding Converter");
                return;
            }
            // Read all lines from the file
            string[] lines = File.ReadAllLines(textBox_orginal_path.Text, encodingsTool_input.SelectedEncoding);
            // Save it !
            File.WriteAllLines(textBox1.Text, lines, encodingsTool_output.SelectedEncoding);
            // Done !!
            MessageDialogResult res = MessageDialog.ShowMessage(this, Program.ResourceManager.GetString("Message_Done"),
                "AHD Encoding Converter",
                MessageDialogButtons.Ok | MessageDialogButtons.Checked, MessageDialogIcon.Info, true, Program.ResourceManager.GetString("Button_OK"), "",
                "", Program.ResourceManager.GetString("CheckBox_OpenSavedFile"));
            if ((res & MessageDialogResult.Checked) == MessageDialogResult.Checked)
            {
                // Open destination file
                try
                {
                    System.Diagnostics.Process.Start(textBox1.Text);
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowErrorMessage(ex.Message, "AHD Encoding Converter");
                }
            }
        }
    }
}
