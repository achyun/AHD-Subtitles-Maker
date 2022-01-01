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
using System;
using System.Windows.Forms;
using AHD.SM.ASMP;

namespace AHD.ID3.Editor.GUI
{
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }
        private bool isMuted;
        private bool isPlaying;
        private bool isChangingTime;
        private double advanceTime = 0.500;
        private bool forward_down;
        private bool rewind_down;
        private bool ready = false;

        public void UpdatePlayStatus()
        {
            // Clear all
            ready = false;
            label_time.Text = label_duration.Text = "00:00:00.000";
            mediaSeeker1.MediaDuration = 100;
            button_play.Image = AHD.ID3.Viewer.Properties.Resources.control_play;
            // Setup new

            if (MediaPlayerManager.Initialized)
            {
                label_duration.Text = TimeFormatConvertor.To_TimeSpan_Milli(MediaPlayerManager.Duration);
                mediaSeeker1.MediaDuration = MediaPlayerManager.Duration;
                ready = true;
            }

        }

        private void button_play_Click(object sender, EventArgs e)
        {
            if (!ready)
                return;
            if (MediaPlayerManager.IsPlaying)
                MediaPlayerManager.Pause();
            else
                MediaPlayerManager.Play();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!ready)
                return;
            label_time.Text = TimeFormatConvertor.To_TimeSpan_Milli(MediaPlayerManager.Position);
            if (MediaPlayerManager.IsPlaying != isPlaying)
            {
                isPlaying = MediaPlayerManager.IsPlaying;
                if (isPlaying)
                {
                    button_play.Image = AHD.ID3.Viewer.Properties.Resources.control_pause;
                }
                else
                {
                    button_play.Image = AHD.ID3.Viewer.Properties.Resources.control_play;
                }
            }
            if (!isChangingTime)
                mediaSeeker1.TimePosition = MediaPlayerManager.Position;

            // Fast forward / Rewind
            if (forward_down)
            {
                double pos = MediaPlayerManager.Position;
                MediaPlayerManager.Position = pos + advanceTime;
            }
            if (rewind_down)
            {
                double pos = MediaPlayerManager.Position;
                if (pos - advanceTime > 0)
                    MediaPlayerManager.Position = pos - advanceTime;
            }
        }
        private void mediaSeeker1_TimeChangeRequest(object sender, TimeChangeArgs e)
        {
            if (!ready)
                return;
            isChangingTime = true;
            if (e.NewTime >= 0 && e.NewTime <= MediaPlayerManager.Duration)
                MediaPlayerManager.Position = e.NewTime;

            isChangingTime = false;
        }
        private void button_rwd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                rewind_down = true;
        }
        private void button_rwd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                rewind_down = false;
        }
        private void button_fwd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                forward_down = true;
        }
        private void button_fwd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                forward_down = false;
        }
        private void trackBar_volume_Scroll(object sender, EventArgs e)
        {
            MediaPlayerManager.Volume = trackBar_volume.Value;
            toolTip1.SetToolTip(trackBar_volume, "Volume: " + trackBar_volume.Value + "%");
        }
        private void button_mute_Click(object sender, EventArgs e)
        {
            if (!isMuted)
            {
                isMuted = true;
                MediaPlayerManager.Volume = 0;
                button_mute.Image = AHD.ID3.Viewer.Properties.Resources.sound_mute;
                toolTip1.SetToolTip(button_mute, "Enable sound");
            }
            else
            {
                isMuted = false;
                MediaPlayerManager.Volume = trackBar_volume.Value;
                button_mute.Image = AHD.ID3.Viewer.Properties.Resources.sound;
                toolTip1.SetToolTip(button_mute, "Mute");
            }
        }
    }
}
