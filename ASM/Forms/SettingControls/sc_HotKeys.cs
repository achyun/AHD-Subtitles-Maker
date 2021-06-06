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
using System.Text;
using System.Windows.Forms;
using AHD.Forms;
namespace AHD.SM
{
    public partial class sc_HotKeys : ASMSettingsControl
    {
        public sc_HotKeys()
        {
            InitializeComponent();
            listView1.Items[0].SubItems.Add(Program.Settings.ShortcutAddSubtitle);
            listView1.Items[0].Tag = Program.Settings.ShortcutAddSubtitle;
            listView1.Items[1].SubItems.Add(Program.Settings.ShortcutNextSubtitle);
            listView1.Items[1].Tag = Program.Settings.ShortcutNextSubtitle;
            listView1.Items[2].SubItems.Add(Program.Settings.ShortcutPreviousSubtitle);
            listView1.Items[2].Tag = Program.Settings.ShortcutPreviousSubtitle;
            listView1.Items[3].SubItems.Add(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime);
            listView1.Items[3].Tag = Program.Settings.ShortcutJumpIntoSelectedSubtitleTime;
            listView1.Items[4].SubItems.Add(Program.Settings.ShortcutFWD);
            listView1.Items[4].Tag = Program.Settings.ShortcutFWD;
            listView1.Items[5].SubItems.Add(Program.Settings.ShortcutREW);
            listView1.Items[5].Tag = Program.Settings.ShortcutREW;
            listView1.Items[6].SubItems.Add(Program.Settings.ShortcutStretchToNext);
            listView1.Items[6].Tag = Program.Settings.ShortcutStretchToNext;
            listView1.Items[7].SubItems.Add(Program.Settings.ShortcutStretchToPrevious);
            listView1.Items[7].Tag = Program.Settings.ShortcutStretchToPrevious;
        }
        public override string ToString()
        {
            return Program.ResourceManager.GetString("Title_HotKeys");
        }
        public override void SaveSettings()
        {
            Program.Settings.ShortcutAddSubtitle = (string)listView1.Items[0].Tag;
            Program.Settings.ShortcutNextSubtitle = (string)listView1.Items[1].Tag;
            Program.Settings.ShortcutPreviousSubtitle = (string)listView1.Items[2].Tag;
            Program.Settings.ShortcutJumpIntoSelectedSubtitleTime = (string)listView1.Items[3].Tag;
            Program.Settings.ShortcutFWD = (string)listView1.Items[4].Tag;
            Program.Settings.ShortcutREW = (string)listView1.Items[5].Tag;
            Program.Settings.ShortcutStretchToNext = (string)listView1.Items[6].Tag;
            Program.Settings.ShortcutStretchToPrevious = (string)listView1.Items[7].Tag;
            base.SaveSettings();
        }
        public override void DefaultSettings()
        {
            listView1.Items[0].SubItems[1].Text = "Space";
            listView1.Items[0].Tag = "Space";
            listView1.Items[1].SubItems[1].Text = "F6";
            listView1.Items[1].Tag = "F6";
            listView1.Items[2].SubItems[1].Text = "F5";
            listView1.Items[2].Tag = "F5";
            listView1.Items[3].SubItems[1].Text = "F7";
            listView1.Items[3].Tag = "F7";
            listView1.Items[4].SubItems[1].Text = "Right";
            listView1.Items[4].Tag = "Right";
            listView1.Items[5].SubItems[1].Text = "Left";
            listView1.Items[5].Tag = "Left";
            listView1.Items[6].SubItems[1].Text = "F10";
            listView1.Items[6].Tag = "F10";
            listView1.Items[7].SubItems[1].Text = "F9";
            listView1.Items[7].Tag = "F9";
        }
        public override void ExportSettings(ref System.IO.BinaryWriter stream)
        {
            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutAddSubtitle));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutAddSubtitle));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutNextSubtitle));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutNextSubtitle));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutPreviousSubtitle));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutPreviousSubtitle));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutFWD));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutFWD));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutREW));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutREW));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutStretchToNext));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutStretchToNext));

            stream.Write(ASCIIEncoding.ASCII.GetByteCount(Program.Settings.ShortcutStretchToPrevious));
            stream.Write(ASCIIEncoding.ASCII.GetBytes(Program.Settings.ShortcutStretchToPrevious));
        }
        public override void ImportSettings(ref System.IO.BinaryReader stream)
        {
            int count = stream.ReadInt32();
            byte[] stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutAddSubtitle = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutNextSubtitle = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutPreviousSubtitle = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutJumpIntoSelectedSubtitleTime = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutFWD = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutREW = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutStretchToNext = ASCIIEncoding.ASCII.GetString(stringBytes);

            count = stream.ReadInt32();
            stringBytes = new byte[count];
            stream.Read(stringBytes, 0, count);
            Program.Settings.ShortcutStretchToPrevious = ASCIIEncoding.ASCII.GetString(stringBytes);
            // Load
            listView1.Items[0].SubItems.RemoveAt(1);
            listView1.Items[0].SubItems.Add(Program.Settings.ShortcutAddSubtitle);
            listView1.Items[0].Tag = Program.Settings.ShortcutAddSubtitle;
            listView1.Items[1].SubItems.RemoveAt(1);
            listView1.Items[1].SubItems.Add(Program.Settings.ShortcutNextSubtitle);
            listView1.Items[1].Tag = Program.Settings.ShortcutNextSubtitle;
            listView1.Items[2].SubItems.RemoveAt(1);
            listView1.Items[2].SubItems.Add(Program.Settings.ShortcutPreviousSubtitle);
            listView1.Items[2].Tag = Program.Settings.ShortcutPreviousSubtitle;
            listView1.Items[3].SubItems.RemoveAt(1);
            listView1.Items[3].SubItems.Add(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime);
            listView1.Items[3].Tag = Program.Settings.ShortcutJumpIntoSelectedSubtitleTime;
            listView1.Items[4].SubItems.RemoveAt(1);
            listView1.Items[4].SubItems.Add(Program.Settings.ShortcutFWD);
            listView1.Items[4].Tag = Program.Settings.ShortcutFWD;
            listView1.Items[5].SubItems.RemoveAt(1);
            listView1.Items[5].SubItems.Add(Program.Settings.ShortcutREW);
            listView1.Items[5].Tag = Program.Settings.ShortcutREW;
            listView1.Items[6].SubItems.Add(Program.Settings.ShortcutStretchToNext);
            listView1.Items[6].Tag = Program.Settings.ShortcutStretchToNext;
            listView1.Items[7].SubItems.Add(Program.Settings.ShortcutStretchToPrevious);
            listView1.Items[7].Tag = Program.Settings.ShortcutStretchToPrevious;
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = listView1.SelectedItems.Count == 1;
        }
        // Change key
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            Frm_ChangeKey frm = new Frm_ChangeKey(listView1.SelectedItems[0].SubItems[1].Text);
            frm.KeyPressed += frm_KeyPressed;
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                listView1.SelectedItems[0].Tag = frm.EnteredValue;
                listView1.SelectedItems[0].SubItems[1].Text = frm.EnteredValue;
            }
        }
        private void frm_KeyPressed(object sender, KeyPressedArgs e)
        {
            Console.WriteLine(e.Key.ToString());
            if (Program.MainForm.CheckShortcut(e.Key))
                e.Cancel = true;
        }
    }
}
