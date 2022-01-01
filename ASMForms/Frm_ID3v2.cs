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
using System.Reflection;
using System.Resources;
using System.IO;
using System.Diagnostics;
using AHD.ID3;
using AHD.ID3.Types;
using AHD.ID3.Frames;
using AHD.SM.ASMP;
using AHD.Forms;
namespace AHD.SM.Forms
{
    public partial class Frm_ID3v2 : Form
    {
        public Frm_ID3v2(Project project)
        {
            InitializeComponent();
            this.project = project;

            //refresh tracks
            foreach (SubtitlesTrack track in project.SubtitleTracks)
                listBox_subtitleTracks.Items.Add(track);
            // load id3 tag
            id3v2 = new ID3v2();
            Result result = id3v2.Load(project.MediaPath);
            switch (result)
            {
                case Result.Failed:
                    MessageDialog.ShowMessage("Can't load ID3 Tag from this media file.",
                                    resources.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                    this.Close();
                    return;

                case Result.NoID3Exist:
                    MessageDialog.ShowMessage(resources.GetString("Message_ThisMp3FileDoesntHaveID3v2"),
               resources.GetString("MessageCaption_NoID3Tagv2Found"));
                    this.Close();
                    return;

                case Result.NotCompatibleVersion:
                    MessageDialog.ShowMessage("This version of ID3 Tag is not compatible with this version.",
                               resources.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                    this.Close();
                    return;
            }
            // load done !
            foreach (ID3TagFrame frame in id3v2.Frames)
            {
                if (frame is SynchronisedLyricsFrame)
                    listBox_id3Items.Items.Add(frame);
            }
            //others
            foreach (string lang in ID3FrameConsts.Languages)
                comboBox1.Items.Add(lang);
        }
        private ID3v2 id3v2;
        private Project project;
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
        Assembly.GetExecutingAssembly());

        private void SaveTag()
        {
            // load the original file. Success or not doesn't matter.
            ID3v2 v2 = new ID3v2();
            bool hadTag = v2.Load(project.MediaPath) == Result.Success;

            // we need to make sure about version. If we are creating id3v2, use default version.
            if (!hadTag)
                v2.TagVersion = new ID3Version((byte)ID3TagSettings.ID3V2Version, 0);

            // set flags
            v2.Compression = false;
            v2.Experimental = false;
            if (ID3TagSettings.DropExtendedHeader)
                v2.ExtendedHeader = false;
            v2.Footer = ID3TagSettings.WriteFooter;
            v2.SavePadding = ID3TagSettings.KeepPadding;
            v2.Unsynchronisation = ID3TagSettings.UseUnsynchronisation;
            //save frames
            // remove all comment frames
            v2.RemoveFrameAll("SLT");
            v2.RemoveFrameAll("SYLT");
            // add frames we have
            foreach (SynchronisedLyricsFrame frame in listBox_id3Items.Items)
            {
                if (v2.TagVersion.Major == 2)
                    frame.ID = "SLT";
                else
                    frame.ID = "SYLT";
                v2.Frames.Add(frame);
            }

            // save !
            v2.Save(project.MediaPath);
        }
        private void listBox_subtitleTracks_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (listBox_subtitleTracks.SelectedIndex < 0)
                return;
            groupBox3.Text = resources.GetString("Title_DataFor") + " '" + ((SubtitlesTrack)listBox_subtitleTracks.SelectedItem).Name +
                "' " + resources.GetString("SubtitlesTrack");
            int i = 1;
            foreach (Subtitle sub in ((SubtitlesTrack)listBox_subtitleTracks.SelectedItem).Subtitles)
            {
                ListViewItem_Subtitle item = new ListViewItem_Subtitle();
                item.Number = i;
                item.Subtitle = sub;
                item.ChangeTimingView(SubtitleTimingMode.Timespan_Milli);
                listView1.Items.Add(item);
                i++;
            }
        }

        private void listBox_id3Items_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (listBox_id3Items.SelectedIndex < 0)
                return;
            groupBox3.Text = resources.GetString("Title_DataFor") + " '" + ((SynchronisedLyricsFrame)listBox_id3Items.SelectedItem).ToString() +
                "' " + resources.GetString("ID3TagSynchronisedLyricItem");
            int i = 1;
            foreach (SynchronisedLyricsItem id3item in
                ((SynchronisedLyricsFrame)listBox_id3Items.SelectedItem).Items)
            {
                ListViewItem item = new ListViewItem();
                item.Text = i.ToString();
                item.SubItems.Add(((double)id3item.Time / 1000).ToString("F3"));
                item.SubItems.Add("N/A");
                item.SubItems.Add("N/A");
                item.SubItems.Add(id3item.Text);
                listView1.Items.Add(item);
                i++;
            }

            textBox1.Text = ((SynchronisedLyricsFrame)listBox_id3Items.SelectedItem).ContentDescriptor;
            comboBox1.SelectedItem = ID3FrameConsts.GetLanguage(((SynchronisedLyricsFrame)listBox_id3Items.SelectedItem).LanguageID);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox_id3Items.SelectedIndex >= 0)
            {
                SynchronisedLyricsFrame frame = (SynchronisedLyricsFrame)listBox_id3Items.SelectedItem;
                frame.ContentDescriptor = textBox1.Text;
                int index = listBox_id3Items.SelectedIndex;
                listBox_id3Items.Items.RemoveAt(index);
                listBox_id3Items.Items.Insert(index, frame);
                listBox_id3Items.SelectedIndex = index;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_id3Items.SelectedIndex >= 0)
            {
                SynchronisedLyricsFrame frame = (SynchronisedLyricsFrame)listBox_id3Items.SelectedItem;
                frame.LanguageID = ID3FrameConsts.GetLanguageID(comboBox1.SelectedItem.ToString());
                int index = listBox_id3Items.SelectedIndex;
                listBox_id3Items.Items.RemoveAt(index);
                listBox_id3Items.Items.Insert(index, frame);
                listBox_id3Items.SelectedIndex = index;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (listBox_id3Items.SelectedIndex < 0)
                return;
            MessageDialogResult result = MessageDialog.ShowMessage(resources.GetString("Message_AreYouSure"),
               resources.GetString("MessageCaption_DeleteItem"), MessageDialogButtons.OkNo, MessageDialogIcon.Question,
                false, resources.GetString("Button_Yes"), resources.GetString("Button_No"));
            if (result == MessageDialogResult.Ok)
            {
                listBox_id3Items.Items.RemoveAt(listBox_id3Items.SelectedIndex);
                listView1.Items.Clear();
                textBox1.Text = "";
                comboBox1.SelectedIndex = -1;
            }
        }
        //Add selected ID3v2 selected item as a Subtitles Track to the project
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (listBox_id3Items.SelectedIndex < 0)
                return;
            EnterNameForm frm = new EnterNameForm(resources.GetString("Title_EnterProjectName"), resources.GetString("NewSubtitlesTrack"),
                true, false);
            frm.OkPressed += new EventHandler<EnterNameFormOkPressedArgs>(frm_OkPressed);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                SynchronisedLyricsFrame frame = (SynchronisedLyricsFrame)listBox_id3Items.SelectedItem;
                SubtitlesTrack track = new SubtitlesTrack(frm.EnteredName);
                for (int i = 0; i < frame.Items.Count; i++)
                {
                    Subtitle sub = new Subtitle();
                    sub.StartTime = ((double)frame.Items[i].Time / 1000);
                    if (i != frame.Items.Count - 1)
                    {
                        sub.EndTime = ((double)frame.Items[i + 1].Time / 1000) - 0.001;
                    }
                    else
                        sub.EndTime = sub.StartTime + 2;
                    sub.Text = SubtitleText.FromString(frame.Items[i].Text);
                    track.Subtitles.Add(sub);
                }
                listBox_subtitleTracks.Items.Add(track);
            }
        }
        //Add selected Subtitles Track as ID3v2 item
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (listBox_subtitleTracks.SelectedIndex < 0)
                return;
            SubtitlesTrack track = ((SubtitlesTrack)listBox_subtitleTracks.SelectedItem);
            SynchronisedLyricsFrame frame = new SynchronisedLyricsFrame("SYLT", "Synchronised lyrics", null, 0);
            frame.ContentDescriptor = track.Name;
            frame.LanguageID = "ENG";
            frame.TimeStampFormat = TimeStampFormat.AbsoluteMilliseconds;
            frame.ContentType = "Lyrics";
            foreach (Subtitle sub in track.Subtitles)
            {
                SynchronisedLyricsItem item = new SynchronisedLyricsItem((int)(sub.StartTime * 1000), sub.Text.ToString());
                frame.Items.Add(item);
            }
            listBox_id3Items.Items.Add(frame);
            listBox_id3Items.SelectedIndex = listBox_id3Items.Items.Count - 1;
        }
        //ok
        private void button1_Click(object sender, EventArgs e)
        {
            //refresh tracks
            project.SubtitleTracks.Clear();
            foreach (SubtitlesTrack track in listBox_subtitleTracks.Items)
                project.SubtitleTracks.Add(track);
            //save ID3 tag
            SaveTag();
            // See if we need to preview
            if (checkBox1.Checked)
            {
                ProcessStartInfo str = new ProcessStartInfo();
                str.Arguments = "\"" + project.MediaPath + "\"" + " /lang " + "\"" + System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName + "\"";
                str.FileName = Path.Combine(Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]), "aslv.exe");
                Process.Start(str);
            }
            //close
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        void frm_OkPressed(object sender, EnterNameFormOkPressedArgs e)
        {
            if (project.IsSubtitlesTrackExist(e.NameEntered))
            {
                MessageDialog.ShowMessage(resources.GetString("Message_ThisNameAlreadyTaken"),
                  resources.GetString("MessageCaption_NameAlreadyTaken"));
                e.Cancel = true;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            string helpPath = ".\\" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + "\\Help.chm";
            if (System.IO.File.Exists(helpPath))
                Help.ShowHelp(this, helpPath, HelpNavigator.KeywordIndex, "How to, Save subtitle track(s) as ID3 Tag Synchronised Lyrics element to mp3 file");
            else
                Help.ShowHelp(this, ".\\en-US\\Help.chm", HelpNavigator.KeywordIndex, "How to, Save subtitle track(s) as ID3 Tag Synchronised Lyrics element to mp3 file");
        }
    }
}
