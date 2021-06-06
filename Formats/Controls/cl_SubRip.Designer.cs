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
namespace AHD.SM.Formats
{
    partial class cl_SubRip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cl_SubRip));
            this.checkBox_write_colors = new System.Windows.Forms.CheckBox();
            this.checkBox_write_fonts = new System.Windows.Forms.CheckBox();
            this.checkBox_write_font_sizes = new System.Windows.Forms.CheckBox();
            this.checkBox_use_ass = new System.Windows.Forms.CheckBox();
            this.checkBox_write_pos = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBox_write_colors
            // 
            resources.ApplyResources(this.checkBox_write_colors, "checkBox_write_colors");
            this.checkBox_write_colors.Name = "checkBox_write_colors";
            this.checkBox_write_colors.UseVisualStyleBackColor = true;
            this.checkBox_write_colors.CheckedChanged += new System.EventHandler(this.checkBox_write_colors_CheckedChanged);
            // 
            // checkBox_write_fonts
            // 
            resources.ApplyResources(this.checkBox_write_fonts, "checkBox_write_fonts");
            this.checkBox_write_fonts.Name = "checkBox_write_fonts";
            this.checkBox_write_fonts.UseVisualStyleBackColor = true;
            this.checkBox_write_fonts.CheckedChanged += new System.EventHandler(this.checkBox_write_fonts_CheckedChanged);
            // 
            // checkBox_write_font_sizes
            // 
            resources.ApplyResources(this.checkBox_write_font_sizes, "checkBox_write_font_sizes");
            this.checkBox_write_font_sizes.Name = "checkBox_write_font_sizes";
            this.checkBox_write_font_sizes.UseVisualStyleBackColor = true;
            this.checkBox_write_font_sizes.CheckedChanged += new System.EventHandler(this.checkBox_write_font_sizes_CheckedChanged);
            // 
            // checkBox_use_ass
            // 
            resources.ApplyResources(this.checkBox_use_ass, "checkBox_use_ass");
            this.checkBox_use_ass.Name = "checkBox_use_ass";
            this.checkBox_use_ass.UseVisualStyleBackColor = true;
            this.checkBox_use_ass.CheckedChanged += new System.EventHandler(this.checkBox_use_ass_CheckedChanged);
            // 
            // checkBox_write_pos
            // 
            resources.ApplyResources(this.checkBox_write_pos, "checkBox_write_pos");
            this.checkBox_write_pos.Name = "checkBox_write_pos";
            this.checkBox_write_pos.UseVisualStyleBackColor = true;
            this.checkBox_write_pos.CheckedChanged += new System.EventHandler(this.checkBox_write_pos_CheckedChanged);
            // 
            // cl_SubRip
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_write_pos);
            this.Controls.Add(this.checkBox_use_ass);
            this.Controls.Add(this.checkBox_write_font_sizes);
            this.Controls.Add(this.checkBox_write_fonts);
            this.Controls.Add(this.checkBox_write_colors);
            this.Name = "cl_SubRip";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_write_colors;
        private System.Windows.Forms.CheckBox checkBox_write_fonts;
        private System.Windows.Forms.CheckBox checkBox_write_font_sizes;
        private System.Windows.Forms.CheckBox checkBox_use_ass;
        private System.Windows.Forms.CheckBox checkBox_write_pos;
    }
}
