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
namespace AHD.ID3.Editor.GUI
{
    partial class PlayerControl
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
            this.label_time = new System.Windows.Forms.Label();
            this.label_duration = new System.Windows.Forms.Label();
            this.trackBar_volume = new System.Windows.Forms.TrackBar();
            this.button_mute = new System.Windows.Forms.Button();
            this.button_fwd = new System.Windows.Forms.Button();
            this.button_rwd = new System.Windows.Forms.Button();
            this.button_play = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mediaSeeker1 = new AHD.ID3.Editor.GUI.MediaSeeker();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_volume)).BeginInit();
            this.SuspendLayout();
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(152, 27);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(73, 13);
            this.label_time.TabIndex = 4;
            this.label_time.Text = "00:00:00.000";
            this.toolTip1.SetToolTip(this.label_time, "Current position");
            // 
            // label_duration
            // 
            this.label_duration.AutoSize = true;
            this.label_duration.Location = new System.Drawing.Point(152, 40);
            this.label_duration.Name = "label_duration";
            this.label_duration.Size = new System.Drawing.Size(73, 13);
            this.label_duration.TabIndex = 5;
            this.label_duration.Text = "00:00:00.000";
            this.toolTip1.SetToolTip(this.label_duration, "Duration");
            // 
            // trackBar_volume
            // 
            this.trackBar_volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBar_volume.AutoSize = false;
            this.trackBar_volume.Location = new System.Drawing.Point(234, 31);
            this.trackBar_volume.Maximum = 100;
            this.trackBar_volume.Name = "trackBar_volume";
            this.trackBar_volume.Size = new System.Drawing.Size(95, 20);
            this.trackBar_volume.TabIndex = 6;
            this.trackBar_volume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar_volume.Value = 100;
            this.trackBar_volume.Scroll += new System.EventHandler(this.trackBar_volume_Scroll);
            // 
            // button_mute
            // 
            this.button_mute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_mute.Image = global::AHD.ID3.Viewer.Properties.Resources.sound;
            this.button_mute.Location = new System.Drawing.Point(331, 29);
            this.button_mute.Name = "button_mute";
            this.button_mute.Size = new System.Drawing.Size(35, 23);
            this.button_mute.TabIndex = 7;
            this.toolTip1.SetToolTip(this.button_mute, "Mute/Enable sound");
            this.button_mute.UseVisualStyleBackColor = true;
            this.button_mute.Click += new System.EventHandler(this.button_mute_Click);
            // 
            // button_fwd
            // 
            this.button_fwd.Image = global::AHD.ID3.Viewer.Properties.Resources.control_fastforward;
            this.button_fwd.Location = new System.Drawing.Point(108, 29);
            this.button_fwd.Name = "button_fwd";
            this.button_fwd.Size = new System.Drawing.Size(35, 23);
            this.button_fwd.TabIndex = 3;
            this.toolTip1.SetToolTip(this.button_fwd, "FWD");
            this.button_fwd.UseVisualStyleBackColor = true;
            this.button_fwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_fwd_MouseDown);
            this.button_fwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_fwd_MouseUp);
            // 
            // button_rwd
            // 
            this.button_rwd.Image = global::AHD.ID3.Viewer.Properties.Resources.control_rewind;
            this.button_rwd.Location = new System.Drawing.Point(67, 29);
            this.button_rwd.Name = "button_rwd";
            this.button_rwd.Size = new System.Drawing.Size(35, 23);
            this.button_rwd.TabIndex = 2;
            this.toolTip1.SetToolTip(this.button_rwd, "RWD");
            this.button_rwd.UseVisualStyleBackColor = true;
            this.button_rwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_rwd_MouseDown);
            this.button_rwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_rwd_MouseUp);
            // 
            // button_play
            // 
            this.button_play.Image = global::AHD.ID3.Viewer.Properties.Resources.control_pause;
            this.button_play.Location = new System.Drawing.Point(3, 29);
            this.button_play.Name = "button_play";
            this.button_play.Size = new System.Drawing.Size(58, 23);
            this.button_play.TabIndex = 0;
            this.toolTip1.SetToolTip(this.button_play, "Play / Pause");
            this.button_play.UseVisualStyleBackColor = true;
            this.button_play.Click += new System.EventHandler(this.button_play_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mediaSeeker1
            // 
            this.mediaSeeker1.BackColor = System.Drawing.Color.LightSlateGray;
            this.mediaSeeker1.Dock = System.Windows.Forms.DockStyle.Top;
            this.mediaSeeker1.Location = new System.Drawing.Point(0, 0);
            this.mediaSeeker1.MediaDuration = 10D;
            this.mediaSeeker1.MilliPixel = 54;
            this.mediaSeeker1.Name = "mediaSeeker1";
            this.mediaSeeker1.Size = new System.Drawing.Size(369, 23);
            this.mediaSeeker1.TabIndex = 1;
            this.mediaSeeker1.Text = "mediaSeeker1";
            this.mediaSeeker1.TickColor = System.Drawing.Color.White;
            this.mediaSeeker1.TimeLineColor = System.Drawing.Color.Blue;
            this.mediaSeeker1.TimePosition = 0D;
            this.mediaSeeker1.ToolTipColor = System.Drawing.Color.Blue;
            this.mediaSeeker1.ToolTipTextColor = System.Drawing.Color.White;
            this.mediaSeeker1.ViewPortOffset = 0;
            this.mediaSeeker1.TimeChangeRequest += new System.EventHandler<AHD.ID3.Editor.GUI.TimeChangeArgs>(this.mediaSeeker1_TimeChangeRequest);
            // 
            // PlayerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_mute);
            this.Controls.Add(this.trackBar_volume);
            this.Controls.Add(this.label_duration);
            this.Controls.Add(this.label_time);
            this.Controls.Add(this.button_fwd);
            this.Controls.Add(this.button_rwd);
            this.Controls.Add(this.mediaSeeker1);
            this.Controls.Add(this.button_play);
            this.MinimumSize = new System.Drawing.Size(369, 55);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(369, 55);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_volume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_play;
        private MediaSeeker mediaSeeker1;
        private System.Windows.Forms.Button button_rwd;
        private System.Windows.Forms.Button button_fwd;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Label label_duration;
        private System.Windows.Forms.TrackBar trackBar_volume;
        private System.Windows.Forms.Button button_mute;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Timer timer1;
    }
}
