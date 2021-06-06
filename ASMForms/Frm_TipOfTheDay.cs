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
using System.Reflection;
using System.Resources;
namespace AHD.SM.Forms
{
    public partial class Frm_TipOfTheDay : Form
    {
        ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
           Assembly.GetExecutingAssembly());
        public Frm_TipOfTheDay(bool DisplayAtStartUpChecked, int TipIndex, string HelpPath)
        {
            InitializeComponent();
            for (int i = 0; i < Tips_OfDays.Length / 2; i++)
            {
                Tips_OfDays[i, 0] = resources.GetString("TipOfTheDay_" + i);
            }
            checkBox1.Checked = DisplayAtStartUpChecked;
            IndexOfTip = TipIndex;
            _HelpPath = HelpPath;
            if (!System.IO.File.Exists(_HelpPath))
                _HelpPath = ".\\en-US\\Help.chm";
            try { Tip_label2.Text = Tips_OfDays[IndexOfTip, 0]; }
            catch { IndexOfTip = 0; Tip_label2.Text = Tips_OfDays[IndexOfTip, 0]; }
        }
        public bool DisplayAtStartUp
        { get { return checkBox1.Checked; } }
        public int TipIndex
        { get { return IndexOfTip; } }

        string[,] Tips_OfDays =
        { 
          {"00" ,"How to, Create subtitles track"},
          {"01" ,"How to, Save subtitle track(s) as ID3 Tag Synchronised Lyrics element to mp3 file"},
          {"02" ,"How to, Check for errors and auto fix"},
          {"03" ,"How to, Import subtitles format file"},
          {"04" ,"How to, Add subtitle"},
          {"05" ,"How to, Check for errors and auto fix"},
          {"06" ,"TimeLine"},
          {"07" ,"AHD Subtitles Converter"},
          {"08" ,"How to, Translate subtitles track"},
          {"09" ,"Change tab locations"},
          {"10" ,"Marks"},
          {"11" ,"How to, Translate using GT Text"},
          {"12" ,"Synchronisation Tool"},
          {"13" ,"AHD Subtitles"},
          {"14" ,"Download subtitles from OpenSubtitles.org"},
          {"15" ,"Upload subtitles to OpenSubtitles.org"},
        };

        int IndexOfTip = 0;
        string _HelpPath = "";


        private void button1_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, _HelpPath, HelpNavigator.KeywordIndex, Tips_OfDays[IndexOfTip, 1]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            IndexOfTip++;
            if (IndexOfTip == Tips_OfDays.Length / 2)
                IndexOfTip = 0;
            Tip_label2.Text = Tips_OfDays[IndexOfTip, 0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, _HelpPath, HelpNavigator.KeywordIndex, "Workflow");
        }
        //close
        private void button4_Click(object sender, EventArgs e)
        {
            IndexOfTip++;
            if (IndexOfTip == Tips_OfDays.Length)
                IndexOfTip = 0;
            Close();
        }
    }
}
