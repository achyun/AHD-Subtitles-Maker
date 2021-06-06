﻿// This file is part of AHD Subtitles Maker
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
using AHD.SM.ASMP;

namespace AHD.SM.Forms
{
    public partial class Frm_ChooseSubtitlesTrack : Form
    {
        public SubtitlesTrack SelectedSubtitlesTrack
        { get { return (SubtitlesTrack)listBox1.SelectedItem; } }
        public Frm_ChooseSubtitlesTrack(SubtitlesTrack original, Project project)
        {
            InitializeComponent();
            foreach (SubtitlesTrack track in project.SubtitleTracks)
                if (original != track)
                    listBox1.Items.Add(track);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
