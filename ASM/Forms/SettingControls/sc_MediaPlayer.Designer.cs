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
namespace AHD.SM
{
    partial class sc_MediaPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sc_MediaPlayer));
            this.checkBox_enable_sub_preview = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.radioButton_use_subtitle_formating = new System.Windows.Forms.RadioButton();
            this.radioButton_use_custom_style = new System.Windows.Forms.RadioButton();
            this.textBox_style = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_media_player = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox_enable_sub_preview
            // 
            resources.ApplyResources(this.checkBox_enable_sub_preview, "checkBox_enable_sub_preview");
            this.checkBox_enable_sub_preview.Name = "checkBox_enable_sub_preview";
            this.toolTip1.SetToolTip(this.checkBox_enable_sub_preview, resources.GetString("checkBox_enable_sub_preview.ToolTip"));
            this.checkBox_enable_sub_preview.UseVisualStyleBackColor = true;
            // 
            // radioButton_use_subtitle_formating
            // 
            resources.ApplyResources(this.radioButton_use_subtitle_formating, "radioButton_use_subtitle_formating");
            this.radioButton_use_subtitle_formating.Checked = true;
            this.radioButton_use_subtitle_formating.Name = "radioButton_use_subtitle_formating";
            this.radioButton_use_subtitle_formating.TabStop = true;
            this.radioButton_use_subtitle_formating.UseVisualStyleBackColor = true;
            // 
            // radioButton_use_custom_style
            // 
            resources.ApplyResources(this.radioButton_use_custom_style, "radioButton_use_custom_style");
            this.radioButton_use_custom_style.Name = "radioButton_use_custom_style";
            this.radioButton_use_custom_style.TabStop = true;
            this.radioButton_use_custom_style.UseVisualStyleBackColor = true;
            // 
            // textBox_style
            // 
            resources.ApplyResources(this.textBox_style, "textBox_style");
            this.textBox_style.Name = "textBox_style";
            this.textBox_style.ReadOnly = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBox1);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.richTextBox1, "richTextBox1");
            this.richTextBox1.ForeColor = System.Drawing.Color.White;
            this.richTextBox1.Name = "richTextBox1";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // comboBox_media_player
            // 
            this.comboBox_media_player.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_media_player.FormattingEnabled = true;
            this.comboBox_media_player.Items.AddRange(new object[] {
            resources.GetString("comboBox_media_player.Items"),
            resources.GetString("comboBox_media_player.Items1"),
            resources.GetString("comboBox_media_player.Items2")});
            resources.ApplyResources(this.comboBox_media_player, "comboBox_media_player");
            this.comboBox_media_player.Name = "comboBox_media_player";
            this.comboBox_media_player.SelectedIndexChanged += new System.EventHandler(this.comboBox_media_player_SelectedIndexChanged);
            // 
            // sc_MediaPlayer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_media_player);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox_style);
            this.Controls.Add(this.radioButton_use_custom_style);
            this.Controls.Add(this.radioButton_use_subtitle_formating);
            this.Controls.Add(this.checkBox_enable_sub_preview);
            this.Name = "sc_MediaPlayer";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_enable_sub_preview;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.RadioButton radioButton_use_subtitle_formating;
        private System.Windows.Forms.RadioButton radioButton_use_custom_style;
        private System.Windows.Forms.TextBox textBox_style;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_media_player;
    }
}
