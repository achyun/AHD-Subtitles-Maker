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
    partial class cl_AutodeskSmoke
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(cl_AutodeskSmoke));
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1_Name = new System.Windows.Forms.TextBox();
            this.comboBox1_Aspect = new System.Windows.Forms.ComboBox();
            this.textBox1_setup = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1_Depth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1_ScanFormat = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1_Rate = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox1_Name
            // 
            resources.ApplyResources(this.textBox1_Name, "textBox1_Name");
            this.textBox1_Name.Name = "textBox1_Name";
            this.textBox1_Name.TextChanged += new System.EventHandler(this.textBox1_Name_TextChanged);
            // 
            // comboBox1_Aspect
            // 
            this.comboBox1_Aspect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1_Aspect.FormattingEnabled = true;
            this.comboBox1_Aspect.Items.AddRange(new object[] {
            resources.GetString("comboBox1_Aspect.Items"),
            resources.GetString("comboBox1_Aspect.Items1"),
            resources.GetString("comboBox1_Aspect.Items2")});
            resources.ApplyResources(this.comboBox1_Aspect, "comboBox1_Aspect");
            this.comboBox1_Aspect.Name = "comboBox1_Aspect";
            this.comboBox1_Aspect.SelectedIndexChanged += new System.EventHandler(this.comboBox1_Aspect_SelectedIndexChanged);
            // 
            // textBox1_setup
            // 
            resources.ApplyResources(this.textBox1_setup, "textBox1_setup");
            this.textBox1_setup.Name = "textBox1_setup";
            this.textBox1_setup.TextChanged += new System.EventHandler(this.textBox1_setup_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // comboBox1_Depth
            // 
            this.comboBox1_Depth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1_Depth.FormattingEnabled = true;
            this.comboBox1_Depth.Items.AddRange(new object[] {
            resources.GetString("comboBox1_Depth.Items"),
            resources.GetString("comboBox1_Depth.Items1"),
            resources.GetString("comboBox1_Depth.Items2"),
            resources.GetString("comboBox1_Depth.Items3"),
            resources.GetString("comboBox1_Depth.Items4")});
            resources.ApplyResources(this.comboBox1_Depth, "comboBox1_Depth");
            this.comboBox1_Depth.Name = "comboBox1_Depth";
            this.comboBox1_Depth.SelectedIndexChanged += new System.EventHandler(this.comboBox1_Depth_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // comboBox1_ScanFormat
            // 
            this.comboBox1_ScanFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1_ScanFormat.FormattingEnabled = true;
            this.comboBox1_ScanFormat.Items.AddRange(new object[] {
            resources.GetString("comboBox1_ScanFormat.Items"),
            resources.GetString("comboBox1_ScanFormat.Items1"),
            resources.GetString("comboBox1_ScanFormat.Items2")});
            resources.ApplyResources(this.comboBox1_ScanFormat, "comboBox1_ScanFormat");
            this.comboBox1_ScanFormat.Name = "comboBox1_ScanFormat";
            this.comboBox1_ScanFormat.SelectedIndexChanged += new System.EventHandler(this.comboBox1_ScanFormat_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboBox1_Rate
            // 
            this.comboBox1_Rate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1_Rate.FormattingEnabled = true;
            this.comboBox1_Rate.Items.AddRange(new object[] {
            resources.GetString("comboBox1_Rate.Items"),
            resources.GetString("comboBox1_Rate.Items1"),
            resources.GetString("comboBox1_Rate.Items2"),
            resources.GetString("comboBox1_Rate.Items3"),
            resources.GetString("comboBox1_Rate.Items4"),
            resources.GetString("comboBox1_Rate.Items5"),
            resources.GetString("comboBox1_Rate.Items6"),
            resources.GetString("comboBox1_Rate.Items7")});
            resources.ApplyResources(this.comboBox1_Rate, "comboBox1_Rate");
            this.comboBox1_Rate.Name = "comboBox1_Rate";
            this.comboBox1_Rate.SelectedIndexChanged += new System.EventHandler(this.comboBox1_Rate_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // cl_AutodeskSmoke
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox1_Name);
            this.Controls.Add(this.comboBox1_Aspect);
            this.Controls.Add(this.textBox1_setup);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox1_Depth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1_ScanFormat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1_Rate);
            this.Name = "cl_AutodeskSmoke";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox1_Name;
        private System.Windows.Forms.ComboBox comboBox1_Aspect;
        private System.Windows.Forms.TextBox textBox1_setup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox1_Depth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1_ScanFormat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1_Rate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
    }
}
