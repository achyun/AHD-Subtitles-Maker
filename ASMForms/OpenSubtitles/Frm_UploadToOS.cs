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
using System.Resources;
using System.IO;
using OpenSubtitlesHandler;
using AHD.SM.Formats;
using AHD.ID3.Types;
using AHD.Forms;
namespace AHD.SM.Forms
{
    public partial class Frm_UploadToOS : Form
    {
        public Frm_UploadToOS(string movieFile, string subtitleFile, string username, string password)
        {
            InitializeComponent(); 
            textBox_username.Text = username;
            textBox_password.Text = password;
            textBox_movieFile.Text = movieFile;
            textBox_subtitlesFile.Text = subtitleFile;
      
            // fill languages
            foreach (string lang in ID3FrameConsts.Languages)
            {
                comboBox_language.Items.Add(lang);
            }
            comboBox_language.SelectedItem = "English [eng]";
            // add fps values
            comboBox_fps.Items.Add("");
            comboBox_fps.Items.Add(23.976);
            comboBox_fps.Items.Add(23.980);
            comboBox_fps.Items.Add(24.000);
            comboBox_fps.Items.Add(25.000);
            comboBox_fps.Items.Add(29.970);
            comboBox_fps.Items.Add(30.000);
            comboBox_fps.SelectedIndex = 0;
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
         Assembly.GetExecutingAssembly());

        private void label5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.opensubtitles.org");
        }
        // change movie
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = ASMP.Filters.Media;
            op.Title = resources.GetString("Title_OpenMovieFile");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_movieFile.Text = op.FileName;
            }
        }
        // open subtitle
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = resources.GetString("Title_OpenSubtitlesFile");
            op.Filter = SubtitleFormats.GetFilter();
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                textBox_subtitlesFile.Text = op.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
        // upload
        private void button3_Click(object sender, EventArgs e)
        {
            if (!File.Exists(textBox_movieFile.Text))
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseBrowseForMovieFirst"),
                      resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            if (!File.Exists(textBox_subtitlesFile.Text))
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseBrowseForSubtitlesFileFirst"),
                      resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            // Log out
            OpenSubtitles.LogOut();
            // Log in !
            /*OpenSubtitles.SetUserAgent("ASM");// NEVER CHANGE THIS !
            IMethodResponse rsp = OpenSubtitles.LogIn(textBox_username.Text, textBox_password.Text, "en");// Blank info should work.
            if (rsp is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(resources.GetString("Message_UnableToConnectToOSServer")
                  + ":\n" + rsp.Message + "\n" + resources.GetString("Message_PleaseTryAgainLater"),
                 resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            else
            {
                // Check out if it authorized
                MethodResponseLogIn loginResponse = (MethodResponseLogIn)rsp;
                if (!loginResponse.Status.Contains("200"))
                {
                    MessageDialog.ShowErrorMessage(resources.GetString("Message_InvailedUserrNameOrPassword"),
                resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
            }*/
            if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(textBox_username.Text, textBox_password.Text))
                return;
            string movieIMDB = textBox_imbd.Text;
            string movieHash = Utilities.ComputeHash(textBox_movieFile.Text);
            // Check to see if this movie is existed in database
            if (movieIMDB == "")
            {
                IMethodResponse checkMovieHashResponse = OpenSubtitles.CheckMovieHash(new string[] { movieHash });
                if (checkMovieHashResponse is MethodResponseError)
                {
                    MessageDialog.ShowMessage(checkMovieHashResponse.Message,
                        resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
                else
                {
                    MethodResponseCheckMovieHash checkResponse = (MethodResponseCheckMovieHash)checkMovieHashResponse;
                    if (checkResponse.Results.Count > 0)
                    {
                        foreach (CheckMovieHashResult res in checkResponse.Results)
                        {
                            if (res.MovieHash.ToLower() == movieHash.ToLower())
                            {
                                movieIMDB = res.MovieImdbID;
                                break;
                            }
                        }
                    }
                }
            }
            // Now make sure imdb is set, again !
            if (movieIMDB == "")
            {
                Frm_OSIMDBSet frm = new Frm_OSIMDBSet(movieIMDB);
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    movieIMDB = frm.IMDB;
                }
                else
                {
                    MessageDialog.ShowMessage(resources.GetString("Message_CanceldByUser"),
                             resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
            }
            // First we need to try upload
            FileInfo movieFileInfo = new FileInfo(textBox_movieFile.Text);

            FileInfo subFileInfo = new FileInfo(textBox_subtitlesFile.Text);
            TryUploadSubtitlesParameters par = new TryUploadSubtitlesParameters();
            par.moviebytesize = movieFileInfo.Length.ToString();
            par.moviefilename = Path.GetFileName(textBox_movieFile.Text);
            par.moviehash = movieHash;
            par.subfilename = Path.GetFileName(textBox_subtitlesFile.Text);
            par.subhash = Utilities.ComputeMd5(textBox_subtitlesFile.Text);
            if (comboBox_fps.SelectedItem != "")
                par.moviefps = (double)comboBox_fps.SelectedItem;
            else
                par.moviefps = 0;
            IMethodResponse response = OpenSubtitles.TryUploadSubtitles(new TryUploadSubtitlesParameters[] { par });
            if (response is MethodResponseError)
            {
                MessageDialog.ShowMessage(response.Message,
                        resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            else
            {
                MethodResponseTryUploadSubtitles tryResponse = (MethodResponseTryUploadSubtitles)response;
                if (tryResponse.AlreadyInDB == 1)
                {
                    MessageDialog.ShowMessage(resources.GetString("Message_ThisSubtitleFileIsAlreadyInOpenSubtitlesorgDatabase"),
                     resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
                else
                {
                    // MessageDialog.ShowMessage(tryResponse.Message,
                    //   resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    //  return;
                }
            }

            // So far so good, upload subtitles !!
            UploadSubtitleInfoParameters uploadParameters = new UploadSubtitleInfoParameters();
            uploadParameters.idmovieimdb = movieIMDB;
            uploadParameters.movieaka = textBox_movieAka.Text;
            uploadParameters.moviereleasename = textBox_movieReleaseName.Text;
            uploadParameters.subauthorcomment = textBox_comment.Text;
            uploadParameters.sublanguageid = ID3FrameConsts.GetLanguageID(comboBox_language.SelectedItem.ToString());
            uploadParameters.automatictranslation = checkBox_machine_translated.Checked;
            uploadParameters.hearingimpaired = checkBox_impaired.Checked;
            uploadParameters.highdefinition = checkBox_hd.Checked;
            UploadSubtitleParameters cd = new UploadSubtitleParameters();
            cd.moviebytesize = movieFileInfo.Length;
            cd.moviefilename = Path.GetFileName(textBox_movieFile.Text);
            cd.moviehash = movieHash;
            cd.subfilename = Path.GetFileName(textBox_subtitlesFile.Text);
            if (comboBox_fps.SelectedItem != "")
                cd.moviefps = (double)comboBox_fps.SelectedItem;
            else
                cd.moviefps = 0;
            cd.subhash = Utilities.ComputeMd5(textBox_subtitlesFile.Text);
            // No subtitles content
            // compress file
            byte[] fdata = Utilities.Compress(new FileStream(textBox_subtitlesFile.Text, FileMode.Open, FileAccess.Read));
            // base64 decode
            cd.subcontent = Convert.ToBase64String(fdata);
            uploadParameters.CDS = new List<UploadSubtitleParameters>();
            uploadParameters.CDS.Add(cd);
            // Now upload !
            IMethodResponse uploadResponse = OpenSubtitles.UploadSubtitles(uploadParameters);
            if (uploadResponse is MethodResponseError)
            {
                MessageDialog.ShowMessage(uploadResponse.Message,
                        resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            else
            {
                MethodResponseUploadSubtitles finalResponse = (MethodResponseUploadSubtitles)uploadResponse;
                if (finalResponse.Data != "")
                {
                    System.Windows.Forms.Clipboard.SetText(finalResponse.Data);// copy to clipboard
                    MessageDialog.ShowMessage(resources.GetString("Message_UploadDone") + "\n" +
                       resources.GetString("Message_LinkToYourSubtitlesFile") + ":\n" + finalResponse.Data
                       + "\n\n" + resources.GetString("Message_LinkCopiedToClipboard"),
                          resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                }
                else
                {
                    MessageDialog.ShowErrorMessage(response.Status,
                            resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
            }

            //this.Close();
        }
        private void textBox_movieFile_TextChanged(object sender, EventArgs e)
        {
            if (textBox_movieReleaseName.Text == "")
            {
                try
                {

                    textBox_movieReleaseName.Text = Path.GetFileNameWithoutExtension(textBox_movieFile.Text);
                    // Set hash
                    textBox_hash.Text = Utilities.ComputeHash(textBox_movieFile.Text);
                    textBox_imbd.Text = "";

                    if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(textBox_username.Text, textBox_password.Text))
                        return;

                    // Check to see if this movie is existed in database, set IMBD
                    IMethodResponse checkMovieHashResponse = OpenSubtitles.CheckMovieHash(new string[] { textBox_hash.Text });

                    if (checkMovieHashResponse is MethodResponseError)
                    {
                        //  MessageDialog.ShowMessage(checkMovieHashResponse.Message,
                        //      resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                        return;
                    }
                    else
                    {
                        MethodResponseCheckMovieHash checkResponse = (MethodResponseCheckMovieHash)checkMovieHashResponse;
                        if (checkResponse.Results.Count > 0)
                        {
                            foreach (CheckMovieHashResult res in checkResponse.Results)
                            {
                                if (res.MovieHash.ToLower() == textBox_hash.Text.ToLower())
                                {
                                    textBox_imbd.Text = res.MovieImdbID;
                                    break;
                                }
                            }
                        }
                    }
                    if (textBox_imbd.Text == "")
                    {
                        textBox_imbd.Text = resources.GetString("Message_PleaseEnterIMBID");
                        textBox_imbd.ReadOnly = false;
                    }
                }
                catch { }
            }
        }
        private void Frm_UploadToOS_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenSubtitles.LogOut();
        }
        // Auto detect language (not active)
        private void button5_Click(object sender, EventArgs e)
        {
            if (File.Exists(textBox_subtitlesFile.Text))
            {
                if (!Frm_DownloadSubtitlesFromOS.CheckLogIn(textBox_username.Text, textBox_password.Text))
                    return;
                string text = File.ReadAllText(textBox_subtitlesFile.Text);
                IMethodResponse response = OpenSubtitles.DetectLanguage(new string[] { text }, Encoding.ASCII);
                if (response is MethodResponseError)
                {
                    MessageDialog.ShowMessage(response.Message,
                        resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                }
                else
                {
                    MethodResponseDetectLanguage lang = (MethodResponseDetectLanguage)response;
                    if (lang.Results.Count > 0)
                    {
                        comboBox_language.SelectedItem = ID3FrameConsts.GetLanguage(lang.Results[0].LanguageID);
                    }
                }
            }
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.opensubtitles.org/en/newuser");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            textBox_password.UseSystemPasswordChar = !checkBox1.Checked;
        }
    }
}
