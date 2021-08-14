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
using AHD.SM.ASMP;
using System;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
namespace AHD.SM.Controls
{
    public partial class MediaPlayer : UserControl
    {
        public MediaPlayer()
        {
            InitializeComponent();
        }

        private ResourceManager resources = new ResourceManager("AHD.SM.Controls.LanguageResources.Resource",
          Assembly.GetExecutingAssembly());
        private bool isMuted;
        private bool isPlaying;
        private bool isChangingTime;
        private double advanceTime = 0.500;
        private bool forward_down;
        private bool rewind_down;
        const int text_view_edge = 5;

        public bool EnableSubtitleView
        {
            get { return enableSubtitlePreviewToolStripMenuItem.Checked; }
        }
        public bool IsUsingSubtitleFormatting
        {
            get { return isSubtitleFormatting; }
        }
        public ASMPFontStyle CustomStyle
        {
            get { return customFormattingStyle; }
        }
        private bool isSubtitleFormatting;
        private ASMPFontStyle customFormattingStyle;

        public void ApplySettings(bool enableSubtitlePreview, bool showSubtitleFormatting, ASMPFontStyle customStyle)
        {
            enableSubtitlePreviewToolStripMenuItem.Checked = enableSubtitlePreview;
            isSubtitleFormatting = showSubtitleFormatting;
            showSubtitleFormattingToolStripMenuItem.Checked = isSubtitleFormatting;
            useCustomFormatToolStripMenuItem.Checked = !isSubtitleFormatting;

            customFormattingStyle = customStyle;

            HideSubtitle();
            trackBar_sound.Value = 100;
        }
        /// <summary>
        /// Open a media file
        /// </summary>
        /// <param name="FilePath">The complete path of the media file</param>
        public void LoadMedia(string FilePath)
        {
            MediaPlayerManager.Stop();
            MediaPlayerManager.LoadFile(FilePath, panel_surface.Handle);
            if (MediaPlayerManager.Duration > 0)
            {
                status_duration.Text = TimeFormatConvertor.To_TimeSpan_Milli(MediaPlayerManager.Duration);
                mediaSeeker1.MediaDuration = MediaPlayerManager.Duration;
                mediaSeeker1.Enabled = true;
            }
            else
            {
                mediaSeeker1.MediaDuration = 10;
                mediaSeeker1.Enabled = false;
                status_duration.Text = "00:00:00.000";
                panel_surface.Visible = false;
            }

            CalculateVideoSurfacePosition();
        }
        private void CalculateVideoSurfacePosition()
        {
            // Let's set the rectangel of the video surface
            // 1 Get the original video size
            Size videoSize = MediaPlayerManager.GetVideoSize();
            if (videoSize.Width == 0 || videoSize.Width == 0)
            {
                // Don't do anything, hide the surface
                // panel_surface.Visible = false;
                // return;
                // OR force it to be as big as the host
                videoSize = new Size(panel_video_host.Width, panel_video_host.Height);
            }
            // 2 Calculate the rectangle we need as stretched with aspect ratio
            int target_x = 0;
            int target_y = 0;
            int target_w = 0;
            int target_h = 0;
            GetRatioStretchRectangle(videoSize.Width, videoSize.Height, panel_video_host.Width, panel_video_host.Height,
               out target_x, out target_y, out target_w, out target_h);
            // Set these new parameters for the panel surface !!
            panel_surface.Parent = panel_video_host;
            panel_surface.Location = new Point(target_x, target_y);
            if (target_w > 0 && target_h > 0)
                panel_surface.Size = new Size(target_w, target_h);
            else
                panel_surface.Size = new Size(panel_video_host.Width, panel_video_host.Height);
            panel_surface.Visible = true;
       
            MediaPlayerManager.SetVideoSurface(panel_surface.Handle, panel_surface.Width, panel_surface.Height);
            panel_surface.BringToFront();

            trackBar1_Scroll(this, new EventArgs());
        }
        private void GetRatioStretchRectangle(int orgVideoWidth, int orgVideoHeight, int maxSurfaceWidth, int maxSurfaceHeight,
        out int out_x, out int out_y, out int out_w, out int out_h)
        {
            float hRatio = orgVideoHeight / maxSurfaceHeight;
            float wRatio = orgVideoWidth / maxSurfaceWidth;
            bool touchTargetFromOutside = false;
            if ((wRatio > hRatio) ^ touchTargetFromOutside)
            {
                out_w = maxSurfaceWidth;
                out_h = (orgVideoHeight * maxSurfaceWidth) / orgVideoWidth;
            }
            else
            {
                out_h = maxSurfaceHeight;
                out_w = (orgVideoWidth * maxSurfaceHeight) / orgVideoHeight;
            }

            out_x = (maxSurfaceWidth - out_w) / 2;
            out_y = (maxSurfaceHeight - out_h) / 2;
        }
        /// <summary>
        /// Stop and remove the media form the player
        /// </summary>
        public void ClearMedia()
        {
            MediaPlayerManager.Stop();
            MediaPlayerManager.ClearMedia();
        }
        public void TogglePlayPause()
        {
            if (IsPlaying)
                Pause();
            else
                Play();
        }
        public void NextFrame()
        {
            double pos = MediaPlayerManager.Position;
            MediaPlayerManager.Position = pos + advanceTime;
        }
        public void PreviousFrame()
        {
            double pos = MediaPlayerManager.Position;
            if (pos - advanceTime > 0)
                MediaPlayerManager.Position = pos - advanceTime;
        }
        public void HideViewer()
        {
            subtitleTextViewer1.Visible = false;
        }
        /// <summary>
        /// Play or resume
        /// </summary>
        public void Play()
        {
            if (!MediaPlayerManager.Initialized || MediaPlayerManager.Duration <= 0)
            {
                if (ChangeMediaRequest != null)
                    ChangeMediaRequest(this, null);
            }
            MediaPlayerManager.Play();
        }
        /// <summary>
        /// Pause
        /// </summary>
        public void Pause()
        {
            MediaPlayerManager.Pause();
        }
        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            MediaPlayerManager.Stop();
        }
        public void ToggleMute()
        {
            if (!isMuted)
            {
                isMuted = true;
                MediaPlayerManager.Volume = 0;
                toolStripButton_mute.Image = Properties.Resources.sound_mute;
                toolStripButton_mute.ToolTipText = resources.GetString("SoundOff");
            }
            else
            {
                isMuted = false;
                MediaPlayerManager.Volume = trackBar_sound.Value;
                toolStripButton_mute.Image = Properties.Resources.sound;
                toolStripButton_mute.ToolTipText = resources.GetString("SoundOn");
            }
            if (MuteToggle != null)
                MuteToggle(this, new EventArgs());
        }
        /// <summary>
        /// Get if the player is playing or not
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return MediaPlayerManager.IsPlaying;
            }
        }
        /// <summary>
        /// Update controls and scroll bar
        /// </summary>
        public void UpdateTimer()
        {
            status_time.Text = TimeFormatConvertor.To_TimeSpan_Milli(MediaPlayerManager.Position);
            if (MediaPlayerManager.IsPlaying != isPlaying)
            {
                isPlaying = MediaPlayerManager.IsPlaying;
                if (isPlaying)
                {
                    Button_playPause.Image = Properties.Resources.control_pause_blue;
                }
                else
                {
                    Button_playPause.Image = Properties.Resources.control_play_blue;
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
        /// <summary>
        /// Show the subtitle to the user
        /// </summary>
        /// <param name="text">The subtitle text you want to show</param>
        public void ShowSubtitle(SubtitleText text)
        {
            //show and hide
            if (subtitleTextEditor1.Visible)
                subtitleTextEditor1.Visible = false;
            subtitleTextEditor1.SubtitleText = text;
            Size textSize = Size.Empty;

            if (isSubtitleFormatting)
            {
                subtitleTextViewer1.ViewText(text);
                textSize = SubtitleTextWrapper.GetSize(text);
            }

            else
            {
                subtitleTextViewer1.ViewText(text.ToString(), customFormattingStyle.Font, customFormattingStyle.Color, text.RighttoLeft);
                textSize = TextRenderer.MeasureText(text.ToString(), customFormattingStyle.Font);
            }
            int w = textSize.Width;
            int h = textSize.Height;
            subtitleTextViewer1.Size = new Size(w, h);
            subtitleTextViewer1.Parent = panel_video_host;
            //location
            if (text.IsCustomPosition)
            {
                subtitleTextViewer1.Location = text.CustomPosition;
            }
            else
            {
                int NewX = panel_surface.Visible ? panel_surface.Location.X : 0;
                int NewY = 0;
                int surf_w = panel_surface.Visible ? panel_surface.Width : panel_video_host.Width;
                int surf_h = panel_surface.Visible ? panel_surface.Height : panel_video_host.Height;
                switch (text.Position)
                {
                    case SubtitlePosition.Down_Middle:
                        NewX += (surf_w / 2) - (subtitleTextViewer1.Size.Width / 2);
                        NewY = surf_h - (text_view_edge + subtitleTextViewer1.Height);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Down_Left:
                        NewX += text_view_edge;
                        NewY = surf_h - (text_view_edge + subtitleTextViewer1.Height);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Down_Right:
                        NewX += surf_w - (subtitleTextViewer1.Size.Width + text_view_edge);
                        NewY = surf_h - (text_view_edge + subtitleTextViewer1.Height);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;

                    case SubtitlePosition.Mid_Middle:
                        NewX += (surf_w / 2) - (subtitleTextViewer1.Size.Width / 2);
                        NewY = (surf_h / 2) - (subtitleTextViewer1.Size.Height / 2);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Mid_Left:
                        NewX += text_view_edge;
                        NewY = (surf_h / 2) - (subtitleTextViewer1.Size.Height / 2);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Mid_Right:
                        NewX += surf_w - (subtitleTextViewer1.Size.Width + text_view_edge);
                        NewY = (surf_h / 2) - (subtitleTextViewer1.Size.Height / 2);
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;

                    case SubtitlePosition.Top_Middle:
                        NewX += (surf_w / 2) - (subtitleTextViewer1.Size.Width / 2);
                        NewY = text_view_edge;
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Top_Left:
                        NewX += text_view_edge;
                        NewY = text_view_edge;
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                    case SubtitlePosition.Top_right:
                        NewX += surf_w - (subtitleTextViewer1.Size.Width + text_view_edge);
                        NewY = text_view_edge;
                        subtitleTextViewer1.Location = new Point(NewX, NewY);
                        break;
                }
            }
            if (!subtitleTextViewer1.Visible)
            {
                subtitleTextViewer1.Visible = true;
                subtitleTextViewer1.BringToFront();
            }
            panel_video_host.Controls.SetChildIndex(subtitleTextViewer1, 0);
        }
        /// <summary>
        /// Hide the subtitle
        /// </summary>
        public void HideSubtitle()
        {
            subtitleTextViewer1.HideText();
            subtitleTextEditor1.Visible = subtitleTextViewer1.Visible = false;
        }
        public void SetInfoTextOfAddButton(string text)
        {
            label_info.Visible = (text != "");
            label_info.Text = text;
            label_info.BringToFront();
        }
        public void RefreshStyles(ASMPFontStyle[] styles)
        {
            subtitleTextEditor1.RefreshStyles(styles);
        }
        //Properties
        /// <summary>
        /// Get or set current position of the player
        /// </summary>
        public double CurrentPosition
        {
            get { return MediaPlayerManager.Position; }
            set
            {
                if (MediaPlayerManager.Initialized && !isChangingTime)
                    // if (value >= 0 && value < MediaPlayerManager.Duration)
                    MediaPlayerManager.Position = value;
            }
        }
        /// <summary>
        /// Get the current media duration
        /// </summary>
        public double Duration
        {
            get
            {
                if (MediaPlayerManager.Initialized)
                    return MediaPlayerManager.Duration;
                return 0;
            }
        }
        /// <summary>
        /// Get a string represents current duration
        /// </summary>
        public string DurationString
        {
            get
            {
                return status_duration.Text;
            }
        }
        /// <summary>
        /// Get or set the volume (0 - 100)
        /// </summary>
        public int Volume
        {
            get { return MediaPlayerManager.Volume; }
            set
            {
                MediaPlayerManager.Volume = value;
                trackBar_sound.Value = MediaPlayerManager.Volume;
                toolTip1.SetToolTip(trackBar_sound, resources.GetString("Volume") + ": " + trackBar_sound.Value + "%");
            }
        }
        /// <summary>
        /// Get or set the advance time in seconds that used in next/previous frame buttons
        /// </summary>
        public double AdvanceTime
        {
            get { return advanceTime; }
            set
            {
                advanceTime = value;
                toolStripButton_fastForward.ToolTipText = "+ " + advanceTime.ToString("F3") + " sec";
                toolStripButton1.ToolTipText = "- " + advanceTime.ToString("F3") + " sec";
            }
        }
        /// <summary>
        /// Get or set if muted
        /// </summary>
        public bool Mute
        {
            get { return isMuted; }
            set
            {
                isMuted = !value;
                if (!isMuted)
                {
                    isMuted = true;
                    MediaPlayerManager.Volume = 0;
                    toolStripButton_mute.Image = Properties.Resources.sound_mute;
                    toolStripButton_mute.ToolTipText = resources.GetString("SoundOff");
                }
                else
                {
                    isMuted = false;
                    MediaPlayerManager.Volume = trackBar_sound.Value;
                    toolStripButton_mute.Image = Properties.Resources.sound;
                    toolStripButton_mute.ToolTipText = resources.GetString("SoundOn");
                }
            }
        }
        /// <summary>
        /// Get or set if the add button is enabled
        /// </summary>
        public bool AddButtonEnabled
        {
            get { return AddButton.Enabled; }
            set { AddButton.Enabled = value; }
        }
        /// <summary>
        /// Get or set if the end set button is enabled
        /// </summary>
        public bool EndSetButtonEnabled
        {
            get { return EndSetButon.Enabled; }
            set { EndSetButon.Enabled = value; }
        }
        public bool SubEditButtonEnabled
        {
            get { return toolStripButton_sub_edit.Enabled; }
            set { toolStripButton_sub_edit.Enabled = value; }
        }
        /// <summary>
        /// Get or set the back color of the subtitle textbox
        /// </summary>
        public Color EditorBackColor
        {
            get { return subtitleTextEditor1.BackColor; }
            set { subtitleTextViewer1.BackColor = subtitleTextEditor1.BackColor = value; }
        }
        public bool ShowEditorStatusStrip
        { get { return subtitleTextEditor1.ShowStatusStrip; } set { subtitleTextEditor1.ShowStatusStrip = value; } }
        //events
        public event EventHandler SaveRequest;
        /// <summary>
        /// Rised when the user starting hold the add button
        /// </summary>
        public event EventHandler AddButtonHoldPress;
        /// <summary>
        /// Rised when the user release the add button
        /// </summary>
        public event EventHandler AddButtonHoldRelease;
        /// <summary>
        /// Rised when the user clicked the end set button
        /// </summary>
        public event EventHandler EndSetButtonPressed;
        public event EventHandler MuteToggle;
        public event EventHandler TimeSlide;
        public event EventHandler ChangeMediaRequest;
        public event EventHandler SubtitleTextEditStarted;
        public event EventHandler EditStylesRequest;
        public event EventHandler EditSubtitleRequest;
        public event EventHandler EditCustomStyleRequest;

        private void Button_playPause_Click(object sender, EventArgs e)
        {
            if (IsPlaying)
                Pause();
            else
                Play();
            //if (TimeSlide != null)
            //   TimeSlide(sender, e);
        }
        //stop
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Stop();
        }
        private void subtitleTextEditor1_SubtitleTextChanged(object sender, EventArgs e)
        {
            if (SaveRequest != null)
                SaveRequest(sender, e);
        }
        private void toolStripButton1_MouseDown(object sender, MouseEventArgs e)
        {
            if (AddButtonHoldPress != null)
                AddButtonHoldPress(sender, e);
        }
        private void toolStripButton1_MouseUp(object sender, MouseEventArgs e)
        {
            if (AddButtonHoldRelease != null)
                AddButtonHoldRelease(sender, e);
        }
        private void toolStripButton1_MouseLeave(object sender, EventArgs e)
        {
            if (AddButtonHoldRelease != null)
                AddButtonHoldRelease(sender, e);
        }
        private void EndSetButon_Click(object sender, EventArgs e)
        {
            if (EndSetButtonPressed != null)
                EndSetButtonPressed(sender, e);
        }
        private void AddButton_EnabledChanged(object sender, EventArgs e)
        {
            if (!AddButton.Enabled)
            {
                if (AddButtonHoldRelease != null)
                    AddButtonHoldRelease(sender, e);
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            MediaPlayerManager.Volume = trackBar_sound.Value;
            toolTip1.SetToolTip(trackBar_sound, resources.GetString("Volume") + ": " + trackBar_sound.Value + "%");
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ToggleMute();
        }
        //next frame
        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }
        private void toolStripButton2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                forward_down = true;
        }
        private void toolStripButton1_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                rewind_down = true;
        }
        private void toolStripButton2_MouseUp(object sender, MouseEventArgs e)
        {
            forward_down = false;
        }
        private void toolStripButton1_MouseUp_1(object sender, MouseEventArgs e)
        {
            rewind_down = false;
        }
        private void subtitleTextViewer1_EditRequest(object sender, EventArgs e)
        {
            if (SubtitleTextEditStarted != null)
                SubtitleTextEditStarted(this, new EventArgs());
            //Stop();
            Pause();

            //subtitleTextViewer1.HideText();
            subtitleTextViewer1.Visible = false;

            // Set location of the editor
            subtitleTextEditor1.Parent = panel_video_host;
            subtitleTextEditor1.Location = new Point(subtitleTextViewer1.Location.X, subtitleTextViewer1.Location.Y - 130);
            subtitleTextEditor1.Size = new Size(subtitleTextViewer1.Width, subtitleTextViewer1.Height + 160);
            subtitleTextEditor1.Visible = true;
            subtitleTextEditor1.BringToFront();
        }
        //change subtitle's position
        private void subtitleTextEditor1_MoveButtonPressed(object sender, EventArgs e)
        {

        }
        private void mediaSeeker1_TimeChangeRequest(object sender, TimeChangeArgs e)
        {
            isChangingTime = true;
            if (e.NewTime >= 0 && e.NewTime <= MediaPlayerManager.Duration)
                MediaPlayerManager.Position = e.NewTime;

            if (TimeSlide != null)
                TimeSlide(sender, e);
            isChangingTime = false;
        }
        private void MediaPlayer_Resize(object sender, EventArgs e)
        {
            CalculateVideoSurfacePosition();
        }
        private void panel_surface_Resize(object sender, EventArgs e)
        {
        }
        private void panel_surface_Enter(object sender, EventArgs e)
        {
            OnEnter(e);
        }
        private void MediaPlayer_SizeChanged(object sender, EventArgs e)
        {
            if (subtitleTextViewer1.Visible)
                ShowSubtitle(subtitleTextEditor1.SubtitleText);
        }
        private void subtitleTextEditor1_EditStylesRequest(object sender, EventArgs e)
        {
            EditStylesRequest?.Invoke(this, new EventArgs());
        }
        // Edit subtitle
        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            Pause();
            EditSubtitleRequest?.Invoke(this, new EventArgs());
        }
        private void enableSubtitlePreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!enableSubtitlePreviewToolStripMenuItem.Checked)
                HideSubtitle();
        }
        private void showSubtitleFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSubtitleFormatting = true;
            showSubtitleFormattingToolStripMenuItem.Checked = true;
            useCustomFormatToolStripMenuItem.Checked = false;
        }
        private void useCustomFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSubtitleFormatting = false;
            showSubtitleFormattingToolStripMenuItem.Checked = false;
            useCustomFormatToolStripMenuItem.Checked = true;
        }
        // Set custom style for subtitle
        private void customFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditCustomStyleRequest?.Invoke(this, new EventArgs());
        }
        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            EditCustomStyleRequest?.Invoke(this, new EventArgs());
        }
    }
}
