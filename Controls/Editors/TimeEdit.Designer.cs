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
namespace AHD.SM.Controls
{
    partial class TimeEdit
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
            this.SS_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.MILI_numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SS_numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MILI_numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // SS_numericUpDown1
            // 
            this.SS_numericUpDown1.InterceptArrowKeys = false;
            this.SS_numericUpDown1.Location = new System.Drawing.Point(0, 0);
            this.SS_numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.SS_numericUpDown1.Name = "SS_numericUpDown1";
            this.SS_numericUpDown1.Size = new System.Drawing.Size(40, 20);
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
            this.MILI_numericUpDown1.Location = new System.Drawing.Point(38, 0);
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
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = ".";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TimeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MILI_numericUpDown1);
            this.Controls.Add(this.SS_numericUpDown1);
            this.Name = "TimeEdit";
            this.Size = new System.Drawing.Size(79, 20);
            ((System.ComponentModel.ISupportInitialize)(this.SS_numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MILI_numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown SS_numericUpDown1;
        private System.Windows.Forms.NumericUpDown MILI_numericUpDown1;
        private System.Windows.Forms.Label label3;
    }
}
