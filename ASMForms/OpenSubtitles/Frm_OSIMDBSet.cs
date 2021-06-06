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
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using AHD.Forms;
using OpenSubtitlesHandler;

namespace AHD.SM.Forms
{
    public partial class Frm_OSIMDBSet : Form
    {
        public Frm_OSIMDBSet(string imdb)
        {
            InitializeComponent();
            textBox_imdb.Text = imdb;
            button3_Click(this, null);
        }
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
          Assembly.GetExecutingAssembly());
        public string IMDB { get { return textBox_imdb.Text; } }
        // get details from imdb
        private void button3_Click(object sender, EventArgs e)
        {
            IMethodResponse response = OpenSubtitles.GetIMDBMovieDetails(textBox_imdb.Text);
            if (response is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(response.Message, 
                    resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
            }
            else
            {
                propertyGrid1.SelectedObject = response;
            }
        }
        // ok
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_imdb.Text == "")
            {
                MessageDialog.ShowErrorMessage(  resources.GetString("Message_PleaseEnterIMDBFirst"),
                    resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            IMethodResponse response = OpenSubtitles.GetIMDBMovieDetails(textBox_imdb.Text);
            if (response is MethodResponseError)
            {
                MessageDialog.ShowErrorMessage(response.Message,
                    resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                return;
            }
            else
            {
                MethodResponseMovieDetails details = (MethodResponseMovieDetails)response;
                if (textBox_imdb.Text != details.ID)
                {
                    MessageDialog.ShowErrorMessage(resources.GetString("Message_ThisIMDBIsInvailed"),
                     resources.GetString("MessageCaption_UploadSubtitlesToOpenSubtitlesorg"));
                    return;
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
