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
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using System.IO;
using OpenSubtitlesHandler;
using AHD.ID3.Types;
using AHD.Forms;
using AHD.SM.ASMP;
namespace AHD.SM.Forms
{
    public partial class Frm_DownloadSubtitlesFromOS : Form
    {
        public Frm_DownloadSubtitlesFromOS(string fileName, string username, string password)
        {
            InitializeComponent();

            textBox_username.Text = username;
            textBox_password.Text = password;
            // fill languages
            int i = 0;
            foreach (string lang in ID3FrameConsts.Languages)
            {
                checkedListBox1.Items.Add(lang);

                if (lang == "English [eng]")
                    checkedListBox1.SetItemChecked(i, true);
                i++;
            }
            // load media file
            if (File.Exists(fileName))
            {
                textBox_movieHash.Text = Utilities.ComputeHash(fileName);
                FileInfo inf = new FileInfo(fileName);
                textBox_movieSize.Text = inf.Length.ToString();
            }
        }
        private static ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        private bool az = false;
        private string downloadedFile = "";
        public string DownloadedFile
        {
            get { return downloadedFile; }
        }

        public static bool CheckLogIn(string userName, string password)
        {
            if (userName == "" || userName == null)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_PleaseEnterUsername"), resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return false;
            }
            if (password == "" || password == null)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_PleaseEnterPassword"), resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return false;
            }
            OpenSubtitles.LogOut();
            // Log in !
            OpenSubtitles.SetUserAgent("ASM");// NEVER CHANGE THIS !
            IMethodResponse rsp = OpenSubtitles.LogIn(userName, password, "en");// Blank info will work.
            if (rsp is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_UnableToConnectToOSServer")
                    + ":\n" + rsp.Message + "\n" + resources.GetString("Message_PleaseTryAgainLater"),
                   resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return false;
            }
            else if (!rsp.Status.Contains("200"))
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_UnableToConnectToOSServer") + ":\n" + rsp.Status,
                                 resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return false;
            }
            return true;
        }
        // Search
        private void button2_Click(object sender, EventArgs e)
        {
            if (!CheckLogIn(textBox_username.Text, textBox_password.Text))
                return;
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectSubtitleLanguagesFirst"),
                    resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            if (textBox_movieHash.Text.Length == 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseEnterTheMovieHashFirst"),
                    resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            if (textBox_movieSize.Text.Length == 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseEnterTheMovieSizeInBytesFirst"),
                   resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            // get language ids
            string langs = "";
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                langs += ID3FrameConsts.GetLanguageID(checkedListBox1.CheckedItems[i].ToString()).ToLower() + ",";
            }
            langs = langs.Substring(0, langs.Length - 1);
            // Do search !
            SubtitleSearchParameters par = new SubtitleSearchParameters(langs,
                textBox_movieHash.Text, int.Parse(textBox_movieSize.Text));

            IMethodResponse result = OpenSubtitles.SearchSubtitles(new SubtitleSearchParameters[] { par });
            if (result is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(result.Message,
                 resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            else
            {
                // load results
                listView1.Items.Clear();
                if (((MethodResponseSubtitleSearch)result).Results.Count > 0)
                {
                    foreach (SubtitleSearchResult res in ((MethodResponseSubtitleSearch)result).Results)
                    {
                        listView1.Items.Add(res.SubFileName + " (" + res.SubFormat + ")");
                        int val = 0;
                        if (int.TryParse(res.SubSize, out val))
                            listView1.Items[listView1.Items.Count - 1].SubItems.Add(HelperTools.GetSize(val));
                        else
                            listView1.Items[listView1.Items.Count - 1].SubItems.Add("??");
                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(res.LanguageName);
                        listView1.Items[listView1.Items.Count - 1].Tag = res;
                    }
                }
                else
                {
                    MessageDialog.ShowMessage(resources.GetString("Message_NoResultNormalOSSearch"),
                    resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                }
            }
        }

        private void Frm_DownloadSubtitlesFromOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenSubtitles.LogOut();
        }
        // download
        private void button3_Click(object sender, EventArgs e)
        {
            if (!CheckLogIn(textBox_username.Text, textBox_password.Text))
                return;
            if (listView1.SelectedItems.Count != 1)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectAResult"),
                resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            try
            {
                SubtitleSearchResult sub = (SubtitleSearchResult)listView1.SelectedItems[0].Tag;
                Cursor = Cursors.WaitCursor;
                SaveFileDialog sav = new SaveFileDialog();
                string ext = Path.GetExtension(sub.SubFileName);
                sav.Filter = "Subtitle (*" + ext + ")|*" + ext;
                if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    IMethodResponse resp = OpenSubtitles.DownloadSubtitles(new int[] { int.Parse(sub.IDSubtitleFile) });
                    if (resp is MethodResponseError)
                    {
                        MessageDialog.ShowErrorMessage(resp.Message,
                      resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                        return;
                    }
                    else
                    {
                        MethodResponseSubtitleDownload rd = (MethodResponseSubtitleDownload)resp;
                        if (rd.Results.Count > 0)
                        {
                            byte[] data = Convert.FromBase64String(rd.Results[0].Data);
                            byte[] target = Utilities.Decompress(new MemoryStream(data));
                            // write data
                            Stream stream = new FileStream(sav.FileName, FileMode.Create, FileAccess.Write);
                            stream.Write(target, 0, target.Length);
                            stream.Close();

                            downloadedFile = sav.FileName;
                            MessageDialogResult mres = MessageDialog.ShowQuestionMessage(this,
                                resources.GetString("Message_Done"),
                                resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"),
                                        resources.GetString("Button_Browse"),
                                       resources.GetString("Button_Close"));
                            if (mres == MessageDialogResult.Ok)
                            {
                                try
                                {
                                    System.Diagnostics.Process.Start("explorer.exe", @"/select, " + sav.FileName);
                                }
                                catch { }
                            }
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();// To allow import to project !
                        }
                        else
                        {
                            MessageDialog.ShowErrorMessage(
                                resources.GetString("Message_ErrorNoResultReturnedFromServerPleaseTryAgainLater"),
                           resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message,
                    resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
            }
            Cursor = Cursors.Default;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // search movie name
        private void button5_Click(object sender, EventArgs e)
        {
            if (!CheckLogIn(textBox_username.Text, textBox_password.Text))
                return;
            if (textBox_movieName.Text.Length == 0)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_PleaseEnterMovieName"),
                          resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectSubtitleLanguagesFirst"),
                  resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            IMethodResponse result = OpenSubtitles.SearchMoviesOnIMDB(textBox_movieName.Text);
            if (result is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(result.Message,
               resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            else
            {
                MethodResponseMovieSearch resp = (MethodResponseMovieSearch)result;
                Frm_SelectMovieResult frm = new Frm_SelectMovieResult(resp.Results.ToArray());
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    // Do search !
                    // get language ids
                    string langs = "";
                    for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                    {
                        langs += ID3FrameConsts.GetLanguageID(checkedListBox1.CheckedItems[i].ToString()).ToLower() + ",";
                    }
                    langs = langs.Substring(0, langs.Length - 1);

                    SubtitleSearchParameters par = new SubtitleSearchParameters();
                    par.IMDbID = frm.SelectedResult.ID;
                    par.MovieByteSize = 0;
                    par.MovieHash = "";
                    par.SubLangaugeID = langs;

                    IMethodResponse searchResult = OpenSubtitles.SearchSubtitles(new SubtitleSearchParameters[] { par });
                    if (searchResult is MethodResponseError)
                    {
                        MessageDialog.ShowErrorMessage(searchResult.Message,
                        resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                        return;
                    }
                    else
                    {
                        // load results
                        listView1.Items.Clear();
                        if (((MethodResponseSubtitleSearch)searchResult).Results.Count > 0)
                        {
                            foreach (SubtitleSearchResult res in ((MethodResponseSubtitleSearch)searchResult).Results)
                            {
                                listView1.Items.Add(res.SubFileName + " (" + res.SubFormat + ")");
                                int val = 0;
                                if (int.TryParse(res.SubSize, out val))
                                    listView1.Items[listView1.Items.Count - 1].SubItems.Add(HelperTools.GetSize(val));
                                else
                                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("??");
                                listView1.Items[listView1.Items.Count - 1].SubItems.Add(res.LanguageName);
                                listView1.Items[listView1.Items.Count - 1].Tag = res;
                            }
                        }
                        else
                        {
                            MessageDialog.ShowMessage(resources.GetString("Message_NoResultMovieNameOSSearch"),
                            resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                        }
                    }
                }
            }
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = ASMP.Filters.Media;
            op.Title = resources.GetString("Title_OpenMovieFile");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_movieHash.Text = Utilities.ComputeHash(op.FileName);
                FileInfo inf = new FileInfo(op.FileName);
                textBox_movieSize.Text = inf.Length.ToString();
            }
        }
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string langs = "";
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                langs += ID3FrameConsts.GetLanguageID(checkedListBox1.CheckedItems[i].ToString()).ToLower() + ",";
            }
            if (langs != "")
                label_selectedLanguages.Text = langs.Substring(0, langs.Length - 1);
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string langs = "";
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                langs += ID3FrameConsts.GetLanguageID(checkedListBox1.CheckedItems[i].ToString()).ToLower() + ",";
            }
            if (langs != "")
                label_selectedLanguages.Text = langs.Substring(0, langs.Length - 1);
        }
        // show sub result info
        private void button6_Click(object sender, EventArgs e)
        {
            if (!CheckLogIn(textBox_username.Text, textBox_password.Text))
                return;
            if (listView1.SelectedItems.Count != 1)
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectAResult"),
                    resources.GetString("MessageCaption_DownloadSubtitlesFromOpenSubtitlesorg"));
                return;
            }
            SubtitleSearchResult sub = (SubtitleSearchResult)listView1.SelectedItems[0].Tag;
            SubtitleSearchResult tsub = new SubtitleSearchResult();
            // make a clone so that user edit will not effect original result
            PropertyInfo[] properties = typeof(SubtitleSearchResult).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                object val = properties[i].GetValue(sub, null);
                properties[i].SetValue(tsub, val, null);
            }
            Frm_SubSearchResultDetails frm = new Frm_SubSearchResultDetails(tsub);
            frm.ShowDialog(this);
        }
        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, az);
            az = !az;
        }
        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opensubtitles.org");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = !checkBox1.Checked;
        }
        // Register
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.opensubtitles.org/en/newuser");
        }
    }
}
