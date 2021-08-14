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
namespace AHD.SM
{
    partial class sc_General
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sc_General));
            this.checkBox_newProject = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_askWhenChangingRTL = new System.Windows.Forms.CheckBox();
            this.checkBox_trackTemplate = new System.Windows.Forms.CheckBox();
            this.checkBox_AutoCreateTrack = new System.Windows.Forms.CheckBox();
            this.checkBox_askToExportBeforeUpload = new System.Windows.Forms.CheckBox();
            this.checkBox_autoSaveEnable = new System.Windows.Forms.CheckBox();
            this.checkBox_check_for_updates = new System.Windows.Forms.CheckBox();
            this.checkBox_autoSelecteSubtitlesTrack = new System.Windows.Forms.CheckBox();
            this.numericUpDown_autoSavePeriod = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_autoSavePeriod)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_newProject
            // 
            resources.ApplyResources(this.checkBox_newProject, "checkBox_newProject");
            this.checkBox_newProject.Name = "checkBox_newProject";
            this.toolTip1.SetToolTip(this.checkBox_newProject, resources.GetString("checkBox_newProject.ToolTip"));
            this.checkBox_newProject.UseVisualStyleBackColor = true;
            // 
            // checkBox_askWhenChangingRTL
            // 
            resources.ApplyResources(this.checkBox_askWhenChangingRTL, "checkBox_askWhenChangingRTL");
            this.checkBox_askWhenChangingRTL.Name = "checkBox_askWhenChangingRTL";
            this.toolTip1.SetToolTip(this.checkBox_askWhenChangingRTL, resources.GetString("checkBox_askWhenChangingRTL.ToolTip"));
            this.checkBox_askWhenChangingRTL.UseVisualStyleBackColor = true;
            // 
            // checkBox_trackTemplate
            // 
            resources.ApplyResources(this.checkBox_trackTemplate, "checkBox_trackTemplate");
            this.checkBox_trackTemplate.Name = "checkBox_trackTemplate";
            this.toolTip1.SetToolTip(this.checkBox_trackTemplate, resources.GetString("checkBox_trackTemplate.ToolTip"));
            this.checkBox_trackTemplate.UseVisualStyleBackColor = true;
            // 
            // checkBox_AutoCreateTrack
            // 
            resources.ApplyResources(this.checkBox_AutoCreateTrack, "checkBox_AutoCreateTrack");
            this.checkBox_AutoCreateTrack.Name = "checkBox_AutoCreateTrack";
            this.toolTip1.SetToolTip(this.checkBox_AutoCreateTrack, resources.GetString("checkBox_AutoCreateTrack.ToolTip"));
            this.checkBox_AutoCreateTrack.UseVisualStyleBackColor = true;
            // 
            // checkBox_askToExportBeforeUpload
            // 
            resources.ApplyResources(this.checkBox_askToExportBeforeUpload, "checkBox_askToExportBeforeUpload");
            this.checkBox_askToExportBeforeUpload.Name = "checkBox_askToExportBeforeUpload";
            this.toolTip1.SetToolTip(this.checkBox_askToExportBeforeUpload, resources.GetString("checkBox_askToExportBeforeUpload.ToolTip"));
            this.checkBox_askToExportBeforeUpload.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoSaveEnable
            // 
            resources.ApplyResources(this.checkBox_autoSaveEnable, "checkBox_autoSaveEnable");
            this.checkBox_autoSaveEnable.Name = "checkBox_autoSaveEnable";
            this.toolTip1.SetToolTip(this.checkBox_autoSaveEnable, resources.GetString("checkBox_autoSaveEnable.ToolTip"));
            this.checkBox_autoSaveEnable.UseVisualStyleBackColor = true;
            // 
            // checkBox_check_for_updates
            // 
            resources.ApplyResources(this.checkBox_check_for_updates, "checkBox_check_for_updates");
            this.checkBox_check_for_updates.Name = "checkBox_check_for_updates";
            this.toolTip1.SetToolTip(this.checkBox_check_for_updates, resources.GetString("checkBox_check_for_updates.ToolTip"));
            this.checkBox_check_for_updates.UseVisualStyleBackColor = true;
            // 
            // checkBox_autoSelecteSubtitlesTrack
            // 
            resources.ApplyResources(this.checkBox_autoSelecteSubtitlesTrack, "checkBox_autoSelecteSubtitlesTrack");
            this.checkBox_autoSelecteSubtitlesTrack.Name = "checkBox_autoSelecteSubtitlesTrack";
            this.checkBox_autoSelecteSubtitlesTrack.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_autoSavePeriod
            // 
            resources.ApplyResources(this.numericUpDown_autoSavePeriod, "numericUpDown_autoSavePeriod");
            this.numericUpDown_autoSavePeriod.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDown_autoSavePeriod.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_autoSavePeriod.Name = "numericUpDown_autoSavePeriod";
            this.numericUpDown_autoSavePeriod.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            // sc_General
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox_check_for_updates);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown_autoSavePeriod);
            this.Controls.Add(this.checkBox_autoSaveEnable);
            this.Controls.Add(this.checkBox_askToExportBeforeUpload);
            this.Controls.Add(this.checkBox_AutoCreateTrack);
            this.Controls.Add(this.checkBox_trackTemplate);
            this.Controls.Add(this.checkBox_askWhenChangingRTL);
            this.Controls.Add(this.checkBox_autoSelecteSubtitlesTrack);
            this.Controls.Add(this.checkBox_newProject);
            this.Name = "sc_General";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_autoSavePeriod)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox_newProject;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_autoSelecteSubtitlesTrack;
        private System.Windows.Forms.CheckBox checkBox_askWhenChangingRTL;
        private System.Windows.Forms.CheckBox checkBox_trackTemplate;
        private System.Windows.Forms.CheckBox checkBox_AutoCreateTrack;
        private System.Windows.Forms.CheckBox checkBox_askToExportBeforeUpload;
        private System.Windows.Forms.CheckBox checkBox_autoSaveEnable;
        private System.Windows.Forms.NumericUpDown numericUpDown_autoSavePeriod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox_check_for_updates;
    }
}
