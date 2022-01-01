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
namespace AHD.SM.Controls
{
    partial class SubtitleEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubtitleEditor));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_start = new System.Windows.Forms.Label();
            this.timeSpaner_start = new AHD.SM.Controls.TimeSpaner();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_end = new System.Windows.Forms.Label();
            this.timeSpaner_end = new AHD.SM.Controls.TimeSpaner();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_duration = new System.Windows.Forms.Label();
            this.timeEdit_duration = new AHD.SM.Controls.TimeEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.subtitleTextEditor1 = new AHD.SM.Controls.SubtitleTextEditor();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.label_start);
            this.panel1.Controls.Add(this.timeSpaner_start);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Name = "panel1";
            // 
            // label_start
            // 
            resources.ApplyResources(this.label_start, "label_start");
            this.label_start.Name = "label_start";
            // 
            // timeSpaner_start
            // 
            resources.ApplyResources(this.timeSpaner_start, "timeSpaner_start");
            this.timeSpaner_start.Name = "timeSpaner_start";
            this.timeSpaner_start.TimeChanged += new System.EventHandler<System.EventArgs>(this.timeSpaner_start_TimeChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.label_end);
            this.panel2.Controls.Add(this.timeSpaner_end);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Name = "panel2";
            // 
            // label_end
            // 
            resources.ApplyResources(this.label_end, "label_end");
            this.label_end.Name = "label_end";
            // 
            // timeSpaner_end
            // 
            resources.ApplyResources(this.timeSpaner_end, "timeSpaner_end");
            this.timeSpaner_end.Name = "timeSpaner_end";
            this.timeSpaner_end.TimeChanged += new System.EventHandler<System.EventArgs>(this.timeSpaner_end_TimeChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Controls.Add(this.label_duration);
            this.panel3.Controls.Add(this.timeEdit_duration);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Name = "panel3";
            // 
            // label_duration
            // 
            resources.ApplyResources(this.label_duration, "label_duration");
            this.label_duration.Name = "label_duration";
            // 
            // timeEdit_duration
            // 
            resources.ApplyResources(this.timeEdit_duration, "timeEdit_duration");
            this.timeEdit_duration.Name = "timeEdit_duration";
            this.timeEdit_duration.TimeChanged += new System.EventHandler<System.EventArgs>(this.timeEdit_duration_TimeChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.VisibleChanged += new System.EventHandler(this.groupBox1_VisibleChanged);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.ForeColor = System.Drawing.Color.Red;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.panel5.Controls.Add(this.comboBox1);
            this.panel5.Name = "panel5";
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.DropDown += new System.EventHandler(this.comboBox1_DropDown);
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // subtitleTextEditor1
            // 
            resources.ApplyResources(this.subtitleTextEditor1, "subtitleTextEditor1");
            this.subtitleTextEditor1.AutoHideStyleEditor = false;
            this.subtitleTextEditor1.BackColor = System.Drawing.Color.Black;
            this.subtitleTextEditor1.Name = "subtitleTextEditor1";
            this.subtitleTextEditor1.ReadOnly = false;
            this.subtitleTextEditor1.ShowMoveButton = true;
            this.subtitleTextEditor1.ShowStatusStrip = true;
            this.subtitleTextEditor1.ShowStyleEditor = true;
            this.subtitleTextEditor1.SubtitleText = null;
            this.subtitleTextEditor1.SubtitleTextChanged += new System.EventHandler(this.subtitleTextEditor1_SubtitleTextChanged);
            this.subtitleTextEditor1.SubtitleTextLengthChanged += new System.EventHandler(this.subtitleTextEditor1_SubtitleTextLengthChanged);
            this.subtitleTextEditor1.SubtitleTextRightToLeftValueChanged += new System.EventHandler(this.subtitleTextEditor1_SubtitleTextRightToLeftValueChanged);
            this.subtitleTextEditor1.EditStylesRequest += new System.EventHandler(this.subtitleTextEditor1_EditStylesRequest);
            this.subtitleTextEditor1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.subtitleTextEditor1_KeyDown);
            // 
            // SubtitleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subtitleTextEditor1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "SubtitleEditor";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private SM.Controls.TimeSpaner timeSpaner_start;
        private System.Windows.Forms.Panel panel2;
        private SM.Controls.TimeSpaner timeSpaner_end;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private SM.Controls.TimeEdit timeEdit_duration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_start;
        private System.Windows.Forms.Label label_end;
        private System.Windows.Forms.Label label_duration;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timer1;
        private SubtitleTextEditor subtitleTextEditor1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
