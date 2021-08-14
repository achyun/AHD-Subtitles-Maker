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
namespace AHD.SM.Controls
{
    partial class Frm_SubtitlePosition
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_SubtitlePosition));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDown_Y = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_X = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton_custom = new System.Windows.Forms.RadioButton();
            this.radioButton_Bottom_Right = new System.Windows.Forms.RadioButton();
            this.radioButton_Bottom = new System.Windows.Forms.RadioButton();
            this.radioButton_Bottom_Left = new System.Windows.Forms.RadioButton();
            this.radioButton_Middle_Right = new System.Windows.Forms.RadioButton();
            this.radioButton_middle = new System.Windows.Forms.RadioButton();
            this.radioButton_Middle_Left = new System.Windows.Forms.RadioButton();
            this.radioButton_Top_right = new System.Windows.Forms.RadioButton();
            this.radioButton_Top = new System.Windows.Forms.RadioButton();
            this.radioButton_Top_left = new System.Windows.Forms.RadioButton();
            this.label_Sample = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_X)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDown_Y);
            this.groupBox1.Controls.Add(this.numericUpDown_X);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButton_custom);
            this.groupBox1.Controls.Add(this.radioButton_Bottom_Right);
            this.groupBox1.Controls.Add(this.radioButton_Bottom);
            this.groupBox1.Controls.Add(this.radioButton_Bottom_Left);
            this.groupBox1.Controls.Add(this.radioButton_Middle_Right);
            this.groupBox1.Controls.Add(this.radioButton_middle);
            this.groupBox1.Controls.Add(this.radioButton_Middle_Left);
            this.groupBox1.Controls.Add(this.radioButton_Top_right);
            this.groupBox1.Controls.Add(this.radioButton_Top);
            this.groupBox1.Controls.Add(this.radioButton_Top_left);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // numericUpDown_Y
            // 
            resources.ApplyResources(this.numericUpDown_Y, "numericUpDown_Y");
            this.numericUpDown_Y.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_Y.Name = "numericUpDown_Y";
            this.numericUpDown_Y.ValueChanged += new System.EventHandler(this.numericUpDown_X_ValueChanged);
            // 
            // numericUpDown_X
            // 
            resources.ApplyResources(this.numericUpDown_X, "numericUpDown_X");
            this.numericUpDown_X.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDown_X.Name = "numericUpDown_X";
            this.numericUpDown_X.ValueChanged += new System.EventHandler(this.numericUpDown_X_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // radioButton_custom
            // 
            resources.ApplyResources(this.radioButton_custom, "radioButton_custom");
            this.radioButton_custom.Name = "radioButton_custom";
            this.radioButton_custom.TabStop = true;
            this.radioButton_custom.UseVisualStyleBackColor = true;
            this.radioButton_custom.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Bottom_Right
            // 
            resources.ApplyResources(this.radioButton_Bottom_Right, "radioButton_Bottom_Right");
            this.radioButton_Bottom_Right.Name = "radioButton_Bottom_Right";
            this.radioButton_Bottom_Right.TabStop = true;
            this.radioButton_Bottom_Right.UseVisualStyleBackColor = true;
            this.radioButton_Bottom_Right.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Bottom
            // 
            resources.ApplyResources(this.radioButton_Bottom, "radioButton_Bottom");
            this.radioButton_Bottom.Name = "radioButton_Bottom";
            this.radioButton_Bottom.TabStop = true;
            this.radioButton_Bottom.UseVisualStyleBackColor = true;
            this.radioButton_Bottom.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Bottom_Left
            // 
            resources.ApplyResources(this.radioButton_Bottom_Left, "radioButton_Bottom_Left");
            this.radioButton_Bottom_Left.Name = "radioButton_Bottom_Left";
            this.radioButton_Bottom_Left.TabStop = true;
            this.radioButton_Bottom_Left.UseVisualStyleBackColor = true;
            this.radioButton_Bottom_Left.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Middle_Right
            // 
            resources.ApplyResources(this.radioButton_Middle_Right, "radioButton_Middle_Right");
            this.radioButton_Middle_Right.Name = "radioButton_Middle_Right";
            this.radioButton_Middle_Right.TabStop = true;
            this.radioButton_Middle_Right.UseVisualStyleBackColor = true;
            this.radioButton_Middle_Right.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_middle
            // 
            resources.ApplyResources(this.radioButton_middle, "radioButton_middle");
            this.radioButton_middle.Name = "radioButton_middle";
            this.radioButton_middle.TabStop = true;
            this.radioButton_middle.UseVisualStyleBackColor = true;
            this.radioButton_middle.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Middle_Left
            // 
            resources.ApplyResources(this.radioButton_Middle_Left, "radioButton_Middle_Left");
            this.radioButton_Middle_Left.Name = "radioButton_Middle_Left";
            this.radioButton_Middle_Left.TabStop = true;
            this.radioButton_Middle_Left.UseVisualStyleBackColor = true;
            this.radioButton_Middle_Left.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Top_right
            // 
            resources.ApplyResources(this.radioButton_Top_right, "radioButton_Top_right");
            this.radioButton_Top_right.Name = "radioButton_Top_right";
            this.radioButton_Top_right.TabStop = true;
            this.radioButton_Top_right.UseVisualStyleBackColor = true;
            this.radioButton_Top_right.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Top
            // 
            resources.ApplyResources(this.radioButton_Top, "radioButton_Top");
            this.radioButton_Top.Name = "radioButton_Top";
            this.radioButton_Top.TabStop = true;
            this.radioButton_Top.UseVisualStyleBackColor = true;
            this.radioButton_Top.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // radioButton_Top_left
            // 
            resources.ApplyResources(this.radioButton_Top_left, "radioButton_Top_left");
            this.radioButton_Top_left.Name = "radioButton_Top_left";
            this.radioButton_Top_left.TabStop = true;
            this.radioButton_Top_left.UseVisualStyleBackColor = true;
            this.radioButton_Top_left.CheckedChanged += new System.EventHandler(this.radioButton_Top_left_CheckedChanged);
            // 
            // label_Sample
            // 
            resources.ApplyResources(this.label_Sample, "label_Sample");
            this.label_Sample.BackColor = System.Drawing.Color.Black;
            this.label_Sample.ForeColor = System.Drawing.Color.White;
            this.label_Sample.Name = "label_Sample";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.label_Sample);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // Frm_SubtitlePosition
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Frm_SubtitlePosition";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_X)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown_Y;
        private System.Windows.Forms.NumericUpDown numericUpDown_X;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton_custom;
        private System.Windows.Forms.RadioButton radioButton_Bottom_Right;
        private System.Windows.Forms.RadioButton radioButton_Bottom;
        private System.Windows.Forms.RadioButton radioButton_Bottom_Left;
        private System.Windows.Forms.RadioButton radioButton_Middle_Right;
        private System.Windows.Forms.RadioButton radioButton_middle;
        private System.Windows.Forms.RadioButton radioButton_Middle_Left;
        private System.Windows.Forms.RadioButton radioButton_Top_right;
        private System.Windows.Forms.RadioButton radioButton_Top;
        private System.Windows.Forms.RadioButton radioButton_Top_left;
        private System.Windows.Forms.Label label_Sample;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
    }
}