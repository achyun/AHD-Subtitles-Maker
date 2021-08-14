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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using AHD.Forms;

namespace AHD.SM
{
    public partial class Frm_Settings : Form
    {
        List<ASMSettingsControl> controls = new List<ASMSettingsControl>();
        public Frm_Settings(string id = "")
        {
            InitializeComponent();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(ASMSettingsControl)))
                {
                    //listBox1.Items.Add(control);//Stupid vc, it never show names at the list
                    controls.Add(Activator.CreateInstance(tp) as ASMSettingsControl);
                }
            }
            controls.Sort(new ASMSettingsControlComparer(true));
            foreach (ASMSettingsControl control in controls)
                listBox1.Items.Add(control.ToString());
            if (id == "")
                listBox1.SelectedIndex = 0;
            else
            {
                for (int i = 0; i < controls.Count; i++)
                {
                    if (controls[i].ID == id)
                    {
                        listBox1.SelectedIndex = i;
                        break;
                    }
                }
                if (listBox1.SelectedIndex < 0)
                    listBox1.SelectedIndex = 0;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            ASMSettingsControl control = controls[listBox1.SelectedIndex];
            control.Location = new Point(0, 0);
            string test = control.ToString();
            panel1.Controls.Add(control);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
        //save and apply
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ASMSettingsControl control in controls)
                control.SaveSettings();
            Program.Settings.Save();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (ASMSettingsControl control in controls)
                control.DefaultSettings();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            controls[listBox1.SelectedIndex].DefaultSettings();
        }
        // Export
        private void button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Title_ExportSettingsToFile");
            sav.Filter = Program.ResourceManager.GetString("Filter_Settings");
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                MessageDialogResult result = MessageDialog.ShowQuestionMessage(
                    Program.ResourceManager.GetString("Message_ThisWillSaveCurrentSettingsContinue"),
                    Program.ResourceManager.GetString("Title_ExportSettingsToFile"),
                    Program.ResourceManager.GetString("Button_Yes"),
                    Program.ResourceManager.GetString("Button_No"));
                if (result == MessageDialogResult.Ok)
                {
                    // Save
                    foreach (ASMSettingsControl control in controls)
                        control.SaveSettings();
                    Program.Settings.Save();

                    Stream str = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                    BinaryWriter writer = new BinaryWriter(str);
                    foreach (ASMSettingsControl control in controls)
                        control.ExportSettings(ref writer);
                    str.Flush();
                    str.Close();
                    writer.Close();
                    // Show ok
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Status_Done"),
                    Program.ResourceManager.GetString("Title_ExportSettingsToFile"));
                }
            }
        }
        // Import
        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ImportSettingsToFile");
            op.Filter = Program.ResourceManager.GetString("Filter_Settings");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                MessageDialogResult result = MessageDialog.ShowQuestionMessage(
                       Program.ResourceManager.GetString("Message_ThisWillDiscardCurrentChangesContinue"),
                       Program.ResourceManager.GetString("Title_ImportSettingsToFile"),
                       Program.ResourceManager.GetString("Button_Yes"),
                       Program.ResourceManager.GetString("Button_No"));
                if (result == MessageDialogResult.Ok)
                {
                    Stream str = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader reader = new BinaryReader(str);
                    foreach (ASMSettingsControl control in controls)
                        control.ImportSettings(ref reader);
                    str.Flush();
                    str.Close();
                    reader.Close();
                    // Show ok
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Status_Done"),
                    Program.ResourceManager.GetString("Title_ImportSettingsToFile"));
                }
            }
        }
    }
    public class ASMSettingsControlComparer : IComparer<ASMSettingsControl>
    {
        bool AtoZ;
        public ASMSettingsControlComparer(bool AtoZ)
        {
            this.AtoZ = AtoZ;
        }
        public int Compare(ASMSettingsControl x, ASMSettingsControl y)
        {
            if (AtoZ)
                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString());
            else
                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.ToString(), y.ToString()));
        }
    }
}
