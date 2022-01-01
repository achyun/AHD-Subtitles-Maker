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
    partial class sc_ID3v2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sc_ID3v2));
            this.checkBox_keepPadding = new System.Windows.Forms.CheckBox();
            this.checkBox_dropExtendedHeader = new System.Windows.Forms.CheckBox();
            this.checkBox_footer = new System.Windows.Forms.CheckBox();
            this.checkBox_unsynchronisation = new System.Windows.Forms.CheckBox();
            this.comboBox_tagVersion = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // checkBox_keepPadding
            // 
            resources.ApplyResources(this.checkBox_keepPadding, "checkBox_keepPadding");
            this.checkBox_keepPadding.Name = "checkBox_keepPadding";
            this.checkBox_keepPadding.UseVisualStyleBackColor = true;
            // 
            // checkBox_dropExtendedHeader
            // 
            resources.ApplyResources(this.checkBox_dropExtendedHeader, "checkBox_dropExtendedHeader");
            this.checkBox_dropExtendedHeader.Name = "checkBox_dropExtendedHeader";
            this.checkBox_dropExtendedHeader.UseVisualStyleBackColor = true;
            // 
            // checkBox_footer
            // 
            resources.ApplyResources(this.checkBox_footer, "checkBox_footer");
            this.checkBox_footer.Name = "checkBox_footer";
            this.checkBox_footer.UseVisualStyleBackColor = true;
            // 
            // checkBox_unsynchronisation
            // 
            resources.ApplyResources(this.checkBox_unsynchronisation, "checkBox_unsynchronisation");
            this.checkBox_unsynchronisation.Name = "checkBox_unsynchronisation";
            this.checkBox_unsynchronisation.UseVisualStyleBackColor = true;
            // 
            // comboBox_tagVersion
            // 
            this.comboBox_tagVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_tagVersion.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox_tagVersion, "comboBox_tagVersion");
            this.comboBox_tagVersion.Name = "comboBox_tagVersion";
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
            // sc_ID3v2
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox_tagVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkBox_keepPadding);
            this.Controls.Add(this.checkBox_dropExtendedHeader);
            this.Controls.Add(this.checkBox_footer);
            this.Controls.Add(this.checkBox_unsynchronisation);
            this.Controls.Add(this.label1);
            this.Name = "sc_ID3v2";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_keepPadding;
        private System.Windows.Forms.CheckBox checkBox_dropExtendedHeader;
        private System.Windows.Forms.CheckBox checkBox_footer;
        private System.Windows.Forms.CheckBox checkBox_unsynchronisation;
        private System.Windows.Forms.ComboBox comboBox_tagVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;

    }
}
