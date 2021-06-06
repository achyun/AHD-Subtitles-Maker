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
    partial class Frm_SubtitleEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_SubtitleEdit));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.subtitleEditor1 = new AHD.SM.Controls.SubtitleEditor();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.preparedTextEditor1 = new AHD.SM.Controls.PreparedTextEditor();
            this.checkBox_word_wrap = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.subtitleEditor1);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // subtitleEditor1
            // 
            this.subtitleEditor1.AskWhenChangingRTLToApplyToAll = false;
            this.subtitleEditor1.DirectApplyToItem = false;
            resources.ApplyResources(this.subtitleEditor1, "subtitleEditor1");
            this.subtitleEditor1.EditorBackColor = System.Drawing.Color.Black;
            this.subtitleEditor1.Name = "subtitleEditor1";
            this.subtitleEditor1.SelectedItems = null;
            this.subtitleEditor1.ShowStatusStrip = true;
            this.subtitleEditor1.SubtitlesTrack = null;
            this.subtitleEditor1.ErrorVisibleChanged += new System.EventHandler(this.subtitleEditor1_ErrorVisibleChanged);
            this.subtitleEditor1.EditStylesRequest += new System.EventHandler(this.subtitleEditor1_EditStylesRequest);
            this.subtitleEditor1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.subtitleEditor1_KeyDown);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.preparedTextEditor1);
            this.groupBox1.Controls.Add(this.checkBox_word_wrap);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // preparedTextEditor1
            // 
            this.preparedTextEditor1.AutoHideStyleEditor = false;
            this.preparedTextEditor1.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.preparedTextEditor1, "preparedTextEditor1");
            this.preparedTextEditor1.Name = "preparedTextEditor1";
            this.preparedTextEditor1.ReadOnly = false;
            this.preparedTextEditor1.ShowStatusStrip = true;
            this.preparedTextEditor1.ShowStyleEditor = true;
            this.preparedTextEditor1.WordWrap = false;
            this.preparedTextEditor1.TextSelectionChanged += new System.EventHandler(this.preparedTextEditor1_TextSelectionChanged);
            this.preparedTextEditor1.EditStylesRequest += new System.EventHandler(this.subtitleEditor1_EditStylesRequest);
            // 
            // checkBox_word_wrap
            // 
            resources.ApplyResources(this.checkBox_word_wrap, "checkBox_word_wrap");
            this.checkBox_word_wrap.Name = "checkBox_word_wrap";
            this.checkBox_word_wrap.UseVisualStyleBackColor = true;
            this.checkBox_word_wrap.CheckedChanged += new System.EventHandler(this.checkBox_word_wrap_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Frm_SubtitleEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Frm_SubtitleEdit";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.subtitleEditor1_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.SubtitleEditor subtitleEditor1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Controls.PreparedTextEditor preparedTextEditor1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox_word_wrap;
    }
}