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
namespace AHD.SM.Forms
{
    partial class Frm_SplitCaptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_SplitCaptions));
            this.radioButton_split_into_one_track = new System.Windows.Forms.RadioButton();
            this.groupBox_split_one_track_options = new System.Windows.Forms.GroupBox();
            this.radioButton_one_track_texts_only = new System.Windows.Forms.RadioButton();
            this.radioButton_one_track_captions_only = new System.Windows.Forms.RadioButton();
            this.radioButton_split_into_two = new System.Windows.Forms.RadioButton();
            this.textBox_first_char = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_last_char = new System.Windows.Forms.TextBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton_caption_is_surround = new System.Windows.Forms.RadioButton();
            this.groupBox_split_one_track_options.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton_split_into_one_track
            // 
            resources.ApplyResources(this.radioButton_split_into_one_track, "radioButton_split_into_one_track");
            this.radioButton_split_into_one_track.Checked = true;
            this.radioButton_split_into_one_track.Name = "radioButton_split_into_one_track";
            this.radioButton_split_into_one_track.TabStop = true;
            this.radioButton_split_into_one_track.UseVisualStyleBackColor = true;
            this.radioButton_split_into_one_track.CheckedChanged += new System.EventHandler(this.radioButton_split_into_one_track_CheckedChanged);
            // 
            // groupBox_split_one_track_options
            // 
            this.groupBox_split_one_track_options.Controls.Add(this.radioButton_one_track_texts_only);
            this.groupBox_split_one_track_options.Controls.Add(this.radioButton_one_track_captions_only);
            resources.ApplyResources(this.groupBox_split_one_track_options, "groupBox_split_one_track_options");
            this.groupBox_split_one_track_options.Name = "groupBox_split_one_track_options";
            this.groupBox_split_one_track_options.TabStop = false;
            // 
            // radioButton_one_track_texts_only
            // 
            resources.ApplyResources(this.radioButton_one_track_texts_only, "radioButton_one_track_texts_only");
            this.radioButton_one_track_texts_only.Name = "radioButton_one_track_texts_only";
            this.radioButton_one_track_texts_only.UseVisualStyleBackColor = true;
            // 
            // radioButton_one_track_captions_only
            // 
            resources.ApplyResources(this.radioButton_one_track_captions_only, "radioButton_one_track_captions_only");
            this.radioButton_one_track_captions_only.Checked = true;
            this.radioButton_one_track_captions_only.Name = "radioButton_one_track_captions_only";
            this.radioButton_one_track_captions_only.TabStop = true;
            this.radioButton_one_track_captions_only.UseVisualStyleBackColor = true;
            // 
            // radioButton_split_into_two
            // 
            resources.ApplyResources(this.radioButton_split_into_two, "radioButton_split_into_two");
            this.radioButton_split_into_two.Name = "radioButton_split_into_two";
            this.radioButton_split_into_two.TabStop = true;
            this.radioButton_split_into_two.UseVisualStyleBackColor = true;
            // 
            // textBox_first_char
            // 
            resources.ApplyResources(this.textBox_first_char, "textBox_first_char");
            this.textBox_first_char.Name = "textBox_first_char";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBox_last_char
            // 
            resources.ApplyResources(this.textBox_last_char, "textBox_last_char");
            this.textBox_last_char.Name = "textBox_last_char";
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            resources.ApplyResources(this.linkLabel2, "linkLabel2");
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.TabStop = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            resources.ApplyResources(this.linkLabel3, "linkLabel3");
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.TabStop = true;
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // linkLabel4
            // 
            resources.ApplyResources(this.linkLabel4, "linkLabel4");
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.TabStop = true;
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // linkLabel5
            // 
            resources.ApplyResources(this.linkLabel5, "linkLabel5");
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.TabStop = true;
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // linkLabel6
            // 
            resources.ApplyResources(this.linkLabel6, "linkLabel6");
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.TabStop = true;
            this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel6_LinkClicked);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton_caption_is_surround);
            this.groupBox1.Controls.Add(this.textBox_first_char);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.linkLabel6);
            this.groupBox1.Controls.Add(this.textBox_last_char);
            this.groupBox1.Controls.Add(this.linkLabel5);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.linkLabel4);
            this.groupBox1.Controls.Add(this.linkLabel2);
            this.groupBox1.Controls.Add(this.linkLabel3);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton_caption_is_surround
            // 
            this.radioButton_caption_is_surround.Checked = true;
            resources.ApplyResources(this.radioButton_caption_is_surround, "radioButton_caption_is_surround");
            this.radioButton_caption_is_surround.Name = "radioButton_caption_is_surround";
            this.radioButton_caption_is_surround.TabStop = true;
            this.radioButton_caption_is_surround.UseVisualStyleBackColor = true;
            // 
            // Frm_SplitCaptions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton_split_into_two);
            this.Controls.Add(this.groupBox_split_one_track_options);
            this.Controls.Add(this.radioButton_split_into_one_track);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Frm_SplitCaptions";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox_split_one_track_options.ResumeLayout(false);
            this.groupBox_split_one_track_options.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_split_into_one_track;
        private System.Windows.Forms.GroupBox groupBox_split_one_track_options;
        private System.Windows.Forms.RadioButton radioButton_one_track_texts_only;
        private System.Windows.Forms.RadioButton radioButton_one_track_captions_only;
        private System.Windows.Forms.RadioButton radioButton_split_into_two;
        private System.Windows.Forms.TextBox textBox_first_char;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_last_char;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton_caption_is_surround;
    }
}