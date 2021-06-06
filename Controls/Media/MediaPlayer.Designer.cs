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
namespace AHD.SM.Controls
{
    partial class MediaPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaPlayer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackBar_sound = new System.Windows.Forms.TrackBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddButton = new System.Windows.Forms.ToolStripButton();
            this.EndSetButon = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_sub_edit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.Button_playPause = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_fastForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.status_time = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.status_duration = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.enableSubtitlePreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.showSubtitleFormattingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useCustomFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.customFormatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_mute = new System.Windows.Forms.ToolStripButton();
            this.mediaSeeker1 = new AHD.SM.Controls.MediaSeeker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label_info = new System.Windows.Forms.Label();
            this.panel_surface = new System.Windows.Forms.Panel();
            this.subtitleTextViewer1 = new AHD.SM.Controls.SubtitleTextViewer();
            this.panel_video_host = new System.Windows.Forms.Panel();
            this.subtitleTextEditor1 = new AHD.SM.Controls.SubtitleTextEditor();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_sound)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel_surface.SuspendLayout();
            this.panel_video_host.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.trackBar_sound);
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.mediaSeeker1);
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // trackBar_sound
            // 
            resources.ApplyResources(this.trackBar_sound, "trackBar_sound");
            this.trackBar_sound.BackColor = System.Drawing.SystemColors.Control;
            this.trackBar_sound.Cursor = System.Windows.Forms.Cursors.Hand;
            this.trackBar_sound.Maximum = 100;
            this.trackBar_sound.Name = "trackBar_sound";
            this.trackBar_sound.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.trackBar_sound, resources.GetString("trackBar_sound.ToolTip"));
            this.trackBar_sound.Value = 100;
            this.trackBar_sound.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(22, 22);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddButton,
            this.EndSetButon,
            this.toolStripSeparator1,
            this.toolStripButton_sub_edit,
            this.toolStripSeparator5,
            this.toolStripButton1,
            this.Button_playPause,
            this.toolStripButton4,
            this.toolStripButton_fastForward,
            this.toolStripSeparator2,
            this.status_time,
            this.toolStripSeparator3,
            this.status_duration,
            this.toolStripSeparator4,
            this.toolStripSplitButton1,
            this.toolStripSeparator6,
            this.toolStripButton_mute});
            this.toolStrip1.Name = "toolStrip1";
            this.toolTip1.SetToolTip(this.toolStrip1, resources.GetString("toolStrip1.ToolTip"));
            this.toolStrip1.Enter += new System.EventHandler(this.panel_surface_Enter);
            // 
            // AddButton
            // 
            resources.ApplyResources(this.AddButton, "AddButton");
            this.AddButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddButton.Name = "AddButton";
            this.AddButton.EnabledChanged += new System.EventHandler(this.AddButton_EnabledChanged);
            this.AddButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripButton1_MouseDown);
            this.AddButton.MouseLeave += new System.EventHandler(this.toolStripButton1_MouseLeave);
            this.AddButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripButton1_MouseUp);
            // 
            // EndSetButon
            // 
            resources.ApplyResources(this.EndSetButon, "EndSetButon");
            this.EndSetButon.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.EndSetButon.Name = "EndSetButon";
            this.EndSetButon.Click += new System.EventHandler(this.EndSetButon_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButton_sub_edit
            // 
            resources.ApplyResources(this.toolStripButton_sub_edit, "toolStripButton_sub_edit");
            this.toolStripButton_sub_edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_sub_edit.Name = "toolStripButton_sub_edit";
            this.toolStripButton_sub_edit.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            this.toolStripButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripButton1_MouseDown_1);
            this.toolStripButton1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripButton1_MouseUp_1);
            // 
            // Button_playPause
            // 
            resources.ApplyResources(this.Button_playPause, "Button_playPause");
            this.Button_playPause.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Button_playPause.Image = global::AHD.SM.Controls.Properties.Resources.control_play_blue;
            this.Button_playPause.Name = "Button_playPause";
            this.Button_playPause.Click += new System.EventHandler(this.Button_playPause_Click);
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton_fastForward
            // 
            resources.ApplyResources(this.toolStripButton_fastForward, "toolStripButton_fastForward");
            this.toolStripButton_fastForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_fastForward.Name = "toolStripButton_fastForward";
            this.toolStripButton_fastForward.Click += new System.EventHandler(this.toolStripButton2_Click);
            this.toolStripButton_fastForward.MouseDown += new System.Windows.Forms.MouseEventHandler(this.toolStripButton2_MouseDown);
            this.toolStripButton_fastForward.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripButton2_MouseUp);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // status_time
            // 
            resources.ApplyResources(this.status_time, "status_time");
            this.status_time.Name = "status_time";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // status_duration
            // 
            resources.ApplyResources(this.status_duration, "status_duration");
            this.status_duration.Name = "status_duration";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // toolStripSplitButton1
            // 
            resources.ApplyResources(this.toolStripSplitButton1, "toolStripSplitButton1");
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSubtitlePreviewToolStripMenuItem,
            this.toolStripSeparator8,
            this.showSubtitleFormattingToolStripMenuItem,
            this.useCustomFormatToolStripMenuItem,
            this.toolStripSeparator7,
            this.customFormatToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::AHD.SM.Controls.Properties.Resources.eye;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.ButtonClick += new System.EventHandler(this.toolStripSplitButton1_ButtonClick);
            // 
            // enableSubtitlePreviewToolStripMenuItem
            // 
            resources.ApplyResources(this.enableSubtitlePreviewToolStripMenuItem, "enableSubtitlePreviewToolStripMenuItem");
            this.enableSubtitlePreviewToolStripMenuItem.Checked = true;
            this.enableSubtitlePreviewToolStripMenuItem.CheckOnClick = true;
            this.enableSubtitlePreviewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.enableSubtitlePreviewToolStripMenuItem.Name = "enableSubtitlePreviewToolStripMenuItem";
            this.enableSubtitlePreviewToolStripMenuItem.Click += new System.EventHandler(this.enableSubtitlePreviewToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // showSubtitleFormattingToolStripMenuItem
            // 
            resources.ApplyResources(this.showSubtitleFormattingToolStripMenuItem, "showSubtitleFormattingToolStripMenuItem");
            this.showSubtitleFormattingToolStripMenuItem.Checked = true;
            this.showSubtitleFormattingToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSubtitleFormattingToolStripMenuItem.Name = "showSubtitleFormattingToolStripMenuItem";
            this.showSubtitleFormattingToolStripMenuItem.Click += new System.EventHandler(this.showSubtitleFormattingToolStripMenuItem_Click);
            // 
            // useCustomFormatToolStripMenuItem
            // 
            resources.ApplyResources(this.useCustomFormatToolStripMenuItem, "useCustomFormatToolStripMenuItem");
            this.useCustomFormatToolStripMenuItem.Name = "useCustomFormatToolStripMenuItem";
            this.useCustomFormatToolStripMenuItem.Click += new System.EventHandler(this.useCustomFormatToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // customFormatToolStripMenuItem
            // 
            resources.ApplyResources(this.customFormatToolStripMenuItem, "customFormatToolStripMenuItem");
            this.customFormatToolStripMenuItem.Name = "customFormatToolStripMenuItem";
            this.customFormatToolStripMenuItem.Click += new System.EventHandler(this.customFormatToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // toolStripButton_mute
            // 
            resources.ApplyResources(this.toolStripButton_mute, "toolStripButton_mute");
            this.toolStripButton_mute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_mute.Name = "toolStripButton_mute";
            this.toolStripButton_mute.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // mediaSeeker1
            // 
            resources.ApplyResources(this.mediaSeeker1, "mediaSeeker1");
            this.mediaSeeker1.BackColor = System.Drawing.Color.LightSlateGray;
            this.mediaSeeker1.MediaDuration = 10D;
            this.mediaSeeker1.MilliPixel = 29;
            this.mediaSeeker1.Name = "mediaSeeker1";
            this.mediaSeeker1.TickColor = System.Drawing.Color.White;
            this.mediaSeeker1.TimeLineColor = System.Drawing.Color.White;
            this.mediaSeeker1.TimePosition = 0D;
            this.toolTip1.SetToolTip(this.mediaSeeker1, resources.GetString("mediaSeeker1.ToolTip"));
            this.mediaSeeker1.ToolTipColor = System.Drawing.Color.LightGray;
            this.mediaSeeker1.ToolTipTextColor = System.Drawing.Color.Black;
            this.mediaSeeker1.ViewPortOffset = 0;
            this.mediaSeeker1.TimeChangeRequest += new System.EventHandler<AHD.SM.Controls.TimeChangeArgs>(this.mediaSeeker1_TimeChangeRequest);
            this.mediaSeeker1.Enter += new System.EventHandler(this.panel_surface_Enter);
            // 
            // label_info
            // 
            resources.ApplyResources(this.label_info, "label_info");
            this.label_info.BackColor = System.Drawing.Color.Black;
            this.label_info.ForeColor = System.Drawing.Color.Lime;
            this.label_info.Name = "label_info";
            this.toolTip1.SetToolTip(this.label_info, resources.GetString("label_info.ToolTip"));
            // 
            // panel_surface
            // 
            resources.ApplyResources(this.panel_surface, "panel_surface");
            this.panel_surface.BackColor = System.Drawing.Color.Black;
            this.panel_surface.Controls.Add(this.subtitleTextViewer1);
            this.panel_surface.Name = "panel_surface";
            this.toolTip1.SetToolTip(this.panel_surface, resources.GetString("panel_surface.ToolTip"));
            this.panel_surface.Enter += new System.EventHandler(this.panel_surface_Enter);
            this.panel_surface.Resize += new System.EventHandler(this.panel_surface_Resize);
            // 
            // subtitleTextViewer1
            // 
            resources.ApplyResources(this.subtitleTextViewer1, "subtitleTextViewer1");
            this.subtitleTextViewer1.Name = "subtitleTextViewer1";
            this.toolTip1.SetToolTip(this.subtitleTextViewer1, resources.GetString("subtitleTextViewer1.ToolTip"));
            this.subtitleTextViewer1.EditRequest += new System.EventHandler(this.subtitleTextViewer1_EditRequest);
            this.subtitleTextViewer1.Enter += new System.EventHandler(this.panel_surface_Enter);
            // 
            // panel_video_host
            // 
            resources.ApplyResources(this.panel_video_host, "panel_video_host");
            this.panel_video_host.BackColor = System.Drawing.Color.Black;
            this.panel_video_host.Controls.Add(this.subtitleTextEditor1);
            this.panel_video_host.Controls.Add(this.panel_surface);
            this.panel_video_host.Controls.Add(this.label_info);
            this.panel_video_host.Name = "panel_video_host";
            this.toolTip1.SetToolTip(this.panel_video_host, resources.GetString("panel_video_host.ToolTip"));
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
            this.toolTip1.SetToolTip(this.subtitleTextEditor1, resources.GetString("subtitleTextEditor1.ToolTip"));
            this.subtitleTextEditor1.SubtitleTextChanged += new System.EventHandler(this.subtitleTextEditor1_SubtitleTextChanged);
            this.subtitleTextEditor1.EditStylesRequest += new System.EventHandler(this.subtitleTextEditor1_EditStylesRequest);
            this.subtitleTextEditor1.Enter += new System.EventHandler(this.panel_surface_Enter);
            // 
            // MediaPlayer
            // 
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_video_host);
            this.Controls.Add(this.panel1);
            this.Name = "MediaPlayer";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.SizeChanged += new System.EventHandler(this.MediaPlayer_SizeChanged);
            this.Resize += new System.EventHandler(this.MediaPlayer_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_sound)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel_surface.ResumeLayout(false);
            this.panel_video_host.ResumeLayout(false);
            this.panel_video_host.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton AddButton;
        private System.Windows.Forms.ToolStripButton EndSetButon;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton Button_playPause;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_mute;
        private System.Windows.Forms.TrackBar trackBar_sound;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton_fastForward;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private SubtitleTextViewer subtitleTextViewer1;
        private SubtitleTextEditor subtitleTextEditor1;
        private System.Windows.Forms.Label label_info;
        private MediaSeeker mediaSeeker1;
        private System.Windows.Forms.Panel panel_surface;
        private System.Windows.Forms.ToolStripLabel status_time;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel status_duration;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.Panel panel_video_host;
        private System.Windows.Forms.ToolStripButton toolStripButton_sub_edit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem enableSubtitlePreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showSubtitleFormattingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem useCustomFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem customFormatToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
    }
}
