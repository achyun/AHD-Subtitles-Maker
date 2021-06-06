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
    partial class Frm_UploadToOS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_UploadToOS));
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_movieFile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox_subtitlesFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_language = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_comment = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_movieReleaseName = new System.Windows.Forms.TextBox();
            this.textBox_movieAka = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_username = new System.Windows.Forms.TextBox();
            this.textBox_password = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBox_fps = new System.Windows.Forms.ComboBox();
            this.checkBox_impaired = new System.Windows.Forms.CheckBox();
            this.checkBox_hd = new System.Windows.Forms.CheckBox();
            this.checkBox_machine_translated = new System.Windows.Forms.CheckBox();
            this.textBox_imbd = new System.Windows.Forms.TextBox();
            this.textBox_hash = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label15 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_movieFile
            // 
            resources.ApplyResources(this.textBox_movieFile, "textBox_movieFile");
            this.textBox_movieFile.Name = "textBox_movieFile";
            this.textBox_movieFile.ReadOnly = true;
            this.textBox_movieFile.TextChanged += new System.EventHandler(this.textBox_movieFile_TextChanged);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox_subtitlesFile
            // 
            resources.ApplyResources(this.textBox_subtitlesFile, "textBox_subtitlesFile");
            this.textBox_subtitlesFile.Name = "textBox_subtitlesFile";
            this.textBox_subtitlesFile.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBox_language
            // 
            this.comboBox_language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_language.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_language, "comboBox_language");
            this.comboBox_language.Name = "comboBox_language";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBox_comment
            // 
            resources.ApplyResources(this.textBox_comment, "textBox_comment");
            this.textBox_comment.Name = "textBox_comment";
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label5
            // 
            this.label5.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label5.Image = global::AHD.SM.Forms.Properties.Resources.OSLogoTransparent;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // textBox_movieReleaseName
            // 
            resources.ApplyResources(this.textBox_movieReleaseName, "textBox_movieReleaseName");
            this.textBox_movieReleaseName.Name = "textBox_movieReleaseName";
            // 
            // textBox_movieAka
            // 
            resources.ApplyResources(this.textBox_movieAka, "textBox_movieAka");
            this.textBox_movieAka.Name = "textBox_movieAka";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // textBox_username
            // 
            resources.ApplyResources(this.textBox_username, "textBox_username");
            this.textBox_username.Name = "textBox_username";
            // 
            // textBox_password
            // 
            resources.ApplyResources(this.textBox_password, "textBox_password");
            this.textBox_password.Name = "textBox_password";
            this.textBox_password.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // comboBox_fps
            // 
            this.comboBox_fps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_fps.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_fps, "comboBox_fps");
            this.comboBox_fps.Name = "comboBox_fps";
            // 
            // checkBox_impaired
            // 
            resources.ApplyResources(this.checkBox_impaired, "checkBox_impaired");
            this.checkBox_impaired.Name = "checkBox_impaired";
            this.checkBox_impaired.UseVisualStyleBackColor = true;
            // 
            // checkBox_hd
            // 
            resources.ApplyResources(this.checkBox_hd, "checkBox_hd");
            this.checkBox_hd.Name = "checkBox_hd";
            this.checkBox_hd.UseVisualStyleBackColor = true;
            // 
            // checkBox_machine_translated
            // 
            resources.ApplyResources(this.checkBox_machine_translated, "checkBox_machine_translated");
            this.checkBox_machine_translated.Name = "checkBox_machine_translated";
            this.checkBox_machine_translated.UseVisualStyleBackColor = true;
            // 
            // textBox_imbd
            // 
            resources.ApplyResources(this.textBox_imbd, "textBox_imbd");
            this.textBox_imbd.Name = "textBox_imbd";
            this.textBox_imbd.ReadOnly = true;
            // 
            // textBox_hash
            // 
            resources.ApplyResources(this.textBox_hash, "textBox_hash");
            this.textBox_hash.Name = "textBox_hash";
            this.textBox_hash.ReadOnly = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // Frm_UploadToOS
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox_imbd);
            this.Controls.Add(this.textBox_hash);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.checkBox_machine_translated);
            this.Controls.Add(this.checkBox_hd);
            this.Controls.Add(this.checkBox_impaired);
            this.Controls.Add(this.comboBox_fps);
            this.Controls.Add(this.textBox_password);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox_username);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox_movieAka);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_movieReleaseName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox_comment);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_language);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox_subtitlesFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_movieFile);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_UploadToOS";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_UploadToOS_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_movieFile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox_subtitlesFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_language;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_comment;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_movieReleaseName;
        private System.Windows.Forms.TextBox textBox_movieAka;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_username;
        private System.Windows.Forms.TextBox textBox_password;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBox_fps;
        private System.Windows.Forms.CheckBox checkBox_impaired;
        private System.Windows.Forms.CheckBox checkBox_hd;
        private System.Windows.Forms.CheckBox checkBox_machine_translated;
        private System.Windows.Forms.TextBox textBox_imbd;
        private System.Windows.Forms.TextBox textBox_hash;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label15;
    }
}