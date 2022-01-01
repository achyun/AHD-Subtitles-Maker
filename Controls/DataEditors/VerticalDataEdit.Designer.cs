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
    partial class VerticalDataEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VerticalDataEdit));
            this.panel_scroll_controls = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_replay_selected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_scroll_to_time = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.trackBar_zoom = new System.Windows.Forms.TrackBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.verticalDataEditPanel1 = new AHD.SM.Controls.VerticalDataEditPanel();
            this.panel_scroll_controls.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_scroll_controls
            // 
            resources.ApplyResources(this.panel_scroll_controls, "panel_scroll_controls");
            this.panel_scroll_controls.Controls.Add(this.toolStrip2);
            this.panel_scroll_controls.Controls.Add(this.toolStrip1);
            this.panel_scroll_controls.Controls.Add(this.trackBar_zoom);
            this.panel_scroll_controls.Name = "panel_scroll_controls";
            // 
            // toolStrip2
            // 
            resources.ApplyResources(this.toolStrip2, "toolStrip2");
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripButton7,
            this.toolStripButton_replay_selected,
            this.toolStripSeparator3,
            this.toolStripButton4});
            this.toolStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip2.Name = "toolStrip2";
            // 
            // toolStripButton2
            // 
            resources.ApplyResources(this.toolStripButton2, "toolStripButton2");
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::AHD.SM.Controls.Properties.Resources.control_play;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click_1);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // toolStripButton7
            // 
            resources.ApplyResources(this.toolStripButton7, "toolStripButton7");
            this.toolStripButton7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton7.Image = global::AHD.SM.Controls.Properties.Resources.control_play_blue;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripButton_replay_selected
            // 
            resources.ApplyResources(this.toolStripButton_replay_selected, "toolStripButton_replay_selected");
            this.toolStripButton_replay_selected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_replay_selected.Image = global::AHD.SM.Controls.Properties.Resources.control_repeat_blue;
            this.toolStripButton_replay_selected.Name = "toolStripButton_replay_selected";
            this.toolStripButton_replay_selected.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // toolStripButton4
            // 
            resources.ApplyResources(this.toolStripButton4, "toolStripButton4");
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = global::AHD.SM.Controls.Properties.Resources.control_stop;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStrip1
            // 
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripButton8,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton_scroll_to_time,
            this.toolStripSeparator1,
            this.toolStripButton3});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Name = "toolStrip1";
            // 
            // toolStripButton5
            // 
            resources.ApplyResources(this.toolStripButton5, "toolStripButton5");
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripButton6
            // 
            resources.ApplyResources(this.toolStripButton6, "toolStripButton6");
            this.toolStripButton6.Checked = true;
            this.toolStripButton6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripButton8
            // 
            resources.ApplyResources(this.toolStripButton8, "toolStripButton8");
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click_1);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // toolStripButton1
            // 
            resources.ApplyResources(this.toolStripButton1, "toolStripButton1");
            this.toolStripButton1.Checked = true;
            this.toolStripButton1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::AHD.SM.Controls.Properties.Resources.hourglass;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton_scroll_to_time
            // 
            resources.ApplyResources(this.toolStripButton_scroll_to_time, "toolStripButton_scroll_to_time");
            this.toolStripButton_scroll_to_time.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_scroll_to_time.Image = global::AHD.SM.Controls.Properties.Resources.hourglass_go;
            this.toolStripButton_scroll_to_time.Name = "toolStripButton_scroll_to_time";
            this.toolStripButton_scroll_to_time.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // toolStripButton3
            // 
            resources.ApplyResources(this.toolStripButton3, "toolStripButton3");
            this.toolStripButton3.Checked = true;
            this.toolStripButton3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::AHD.SM.Controls.Properties.Resources.hourglass_link;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // trackBar_zoom
            // 
            resources.ApplyResources(this.trackBar_zoom, "trackBar_zoom");
            this.trackBar_zoom.Minimum = 5;
            this.trackBar_zoom.Name = "trackBar_zoom";
            this.trackBar_zoom.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_zoom.Value = 5;
            this.trackBar_zoom.Scroll += new System.EventHandler(this.trackBar_zoom_Scroll);
            // 
            // vScrollBar1
            // 
            resources.ApplyResources(this.vScrollBar1, "vScrollBar1");
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // verticalDataEditPanel1
            // 
            resources.ApplyResources(this.verticalDataEditPanel1, "verticalDataEditPanel1");
            this.verticalDataEditPanel1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.verticalDataEditPanel1.MediaObjectDuration = 0D;
            this.verticalDataEditPanel1.Name = "verticalDataEditPanel1";
            this.verticalDataEditPanel1.SubtitleColor = System.Drawing.Color.DodgerBlue;
            this.verticalDataEditPanel1.SubtitleHeaderColor = System.Drawing.Color.RoyalBlue;
            this.verticalDataEditPanel1.SubtitleSelectedColor = System.Drawing.Color.Purple;
            this.verticalDataEditPanel1.SubtitleTextColor = System.Drawing.Color.White;
            this.verticalDataEditPanel1.TickColor = System.Drawing.Color.White;
            this.verticalDataEditPanel1.TimeLineColor = System.Drawing.Color.White;
            this.verticalDataEditPanel1.TimerEnabled = false;
            this.verticalDataEditPanel1.TimingPanelBackgroundColor = System.Drawing.Color.MediumAquamarine;
            this.verticalDataEditPanel1.TimingPanelColor = System.Drawing.Color.White;
            this.verticalDataEditPanel1.WaveformColor = System.Drawing.Color.Black;
            this.verticalDataEditPanel1.RequestScrollToTime += new System.EventHandler(this.verticalDataEditPanel1_RequestScrollToTime);
            this.verticalDataEditPanel1.MoreMillis += new System.EventHandler(this.verticalDataEditPanel1_MoreMillis);
            this.verticalDataEditPanel1.LessMillis += new System.EventHandler(this.verticalDataEditPanel1_LessMillis);
            this.verticalDataEditPanel1.MoreScroll += new System.EventHandler(this.verticalDataEditPanel1_MoreScroll);
            this.verticalDataEditPanel1.LessScroll += new System.EventHandler(this.verticalDataEditPanel1_LessScroll);
            this.verticalDataEditPanel1.MoreTime += new System.EventHandler(this.verticalDataEditPanel1_MoreTime);
            this.verticalDataEditPanel1.LessTime += new System.EventHandler(this.verticalDataEditPanel1_LessTime);
            this.verticalDataEditPanel1.SubtitlesSelected += new System.EventHandler(this.verticalDataEditPanel1_SubtitlesSelected);
            this.verticalDataEditPanel1.SubtitleDoubleClick += new System.EventHandler(this.verticalDataEditPanel1_SubtitleDoubleClick);
            this.verticalDataEditPanel1.SubtitleClick += new System.EventHandler(this.verticalDataEditPanel1_SubtitleClick);
            this.verticalDataEditPanel1.SubtitlePropertiesChanged += new System.EventHandler(this.verticalDataEditPanel1_SubtitlePropertiesChanged);
            this.verticalDataEditPanel1.Resize += new System.EventHandler(this.verticalDataEditPanel1_Resize);
            // 
            // VerticalDataEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.verticalDataEditPanel1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.panel_scroll_controls);
            this.Name = "VerticalDataEdit";
            this.panel_scroll_controls.ResumeLayout(false);
            this.panel_scroll_controls.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_zoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel_scroll_controls;
        private System.Windows.Forms.TrackBar trackBar_zoom;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private VerticalDataEditPanel verticalDataEditPanel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton_scroll_to_time;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton_replay_selected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
    }
}
