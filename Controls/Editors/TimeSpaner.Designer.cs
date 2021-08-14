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
    partial class TimeSpaner
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
            this.HH_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.MM_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.SS_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.MILI_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.HH_numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MM_numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MILI_numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // HH_numericUpDown1
            // 
            this.HH_numericUpDown1.InterceptArrowKeys = false;
            this.HH_numericUpDown1.Location = new System.Drawing.Point(0, 1);
            this.HH_numericUpDown1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.HH_numericUpDown1.Name = "HH_numericUpDown1";
            this.HH_numericUpDown1.Size = new System.Drawing.Size(32, 20);
            this.HH_numericUpDown1.TabIndex = 0;
            this.HH_numericUpDown1.ValueChanged += new System.EventHandler(this.HH_numericUpDown1_ValueChanged);
            this.HH_numericUpDown1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.HH_numericUpDown1_KeyUp);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = ":";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MM_numericUpDown1
            // 
            this.MM_numericUpDown1.InterceptArrowKeys = false;
            this.MM_numericUpDown1.Location = new System.Drawing.Point(30, 1);
            this.MM_numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.MM_numericUpDown1.Name = "MM_numericUpDown1";
            this.MM_numericUpDown1.Size = new System.Drawing.Size(32, 20);
            this.MM_numericUpDown1.TabIndex = 2;
            this.MM_numericUpDown1.ValueChanged += new System.EventHandler(this.MM_numericUpDown1_ValueChanged);
            this.MM_numericUpDown1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MM_numericUpDown1_KeyUp);
            // 
            // SS_numericUpDown1
            // 
            this.SS_numericUpDown1.InterceptArrowKeys = false;
            this.SS_numericUpDown1.Location = new System.Drawing.Point(60, 1);
            this.SS_numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.SS_numericUpDown1.Name = "SS_numericUpDown1";
            this.SS_numericUpDown1.Size = new System.Drawing.Size(32, 20);
            this.SS_numericUpDown1.TabIndex = 4;
            this.SS_numericUpDown1.ValueChanged += new System.EventHandler(this.SS_numericUpDown1_ValueChanged);
            this.SS_numericUpDown1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SS_numericUpDown1_KeyUp);
            // 
            // MILI_numericUpDown1
            // 
            this.MILI_numericUpDown1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.MILI_numericUpDown1.InterceptArrowKeys = false;
            this.MILI_numericUpDown1.Location = new System.Drawing.Point(90, 1);
            this.MILI_numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MILI_numericUpDown1.Name = "MILI_numericUpDown1";
            this.MILI_numericUpDown1.Size = new System.Drawing.Size(38, 20);
            this.MILI_numericUpDown1.TabIndex = 6;
            this.MILI_numericUpDown1.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.MILI_numericUpDown1.ValueChanged += new System.EventHandler(this.MILI_numericUpDown1_ValueChanged);
            this.MILI_numericUpDown1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MILI_numericUpDown1_KeyUp);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(46, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = ":";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(76, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = ".";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TimeSpaner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MILI_numericUpDown1);
            this.Controls.Add(this.SS_numericUpDown1);
            this.Controls.Add(this.MM_numericUpDown1);
            this.Controls.Add(this.HH_numericUpDown1);
            this.Name = "TimeSpaner";
            this.Size = new System.Drawing.Size(129, 22);
            ((System.ComponentModel.ISupportInitialize)(this.HH_numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MM_numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SS_numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MILI_numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown HH_numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown MM_numericUpDown1;
        private System.Windows.Forms.NumericUpDown SS_numericUpDown1;
        private System.Windows.Forms.NumericUpDown MILI_numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
