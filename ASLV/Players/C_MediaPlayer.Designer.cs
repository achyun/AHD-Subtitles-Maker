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
namespace AHD.ID3.Editor.GUI
{
    partial class C_MediaPlayer
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
            AHD.SM.ASMP.MediaPlayerManager.ClearMedia();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(C_MediaPlayer));
            this.label1 = new System.Windows.Forms.Label();
            this.imagePanel1 = new AHD.ID3.Editor.GUI.ImagePanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lyricsLanguageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.playerControl1 = new AHD.ID3.Editor.GUI.PlayerControl();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            this.label1.TextChanged += new System.EventHandler(this.label1_TextChanged);
            // 
            // imagePanel1
            // 
            this.imagePanel1.BackColor = System.Drawing.Color.Black;
            this.imagePanel1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.imagePanel1, "imagePanel1");
            this.imagePanel1.ImageToView = null;
            this.imagePanel1.Name = "imagePanel1";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lyricsLanguageToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // lyricsLanguageToolStripMenuItem
            // 
            this.lyricsLanguageToolStripMenuItem.Name = "lyricsLanguageToolStripMenuItem";
            resources.ApplyResources(this.lyricsLanguageToolStripMenuItem, "lyricsLanguageToolStripMenuItem");
            this.lyricsLanguageToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.lyricsLanguageToolStripMenuItem_DropDownItemClicked);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // playerControl1
            // 
            resources.ApplyResources(this.playerControl1, "playerControl1");
            this.playerControl1.Name = "playerControl1";
            // 
            // C_MediaPlayer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imagePanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.playerControl1);
            this.Name = "C_MediaPlayer";
            this.VisibleChanged += new System.EventHandler(this.C_MediaPlayer_VisibleChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.C_MediaPlayer_Paint);
            this.Resize += new System.EventHandler(this.C_MediaPlayer_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private ImagePanel imagePanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.ToolStripMenuItem lyricsLanguageToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private PlayerControl playerControl1;
    }
}
