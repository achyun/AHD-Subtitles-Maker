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
namespace AHD.SM.Forms
{
    partial class Frm_LineBreakRule
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_LineBreakRule));
            this.checkBox_capital = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBox_char_limit = new System.Windows.Forms.CheckBox();
            this.checkBox_punc = new System.Windows.Forms.CheckBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.radioButton_after_punc = new System.Windows.Forms.RadioButton();
            this.radioButton_before_punc = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBox_capital
            // 
            resources.ApplyResources(this.checkBox_capital, "checkBox_capital");
            this.checkBox_capital.Checked = true;
            this.checkBox_capital.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_capital.Name = "checkBox_capital";
            this.toolTip1.SetToolTip(this.checkBox_capital, resources.GetString("checkBox_capital.ToolTip"));
            this.checkBox_capital.UseVisualStyleBackColor = true;
            // 
            // checkBox_char_limit
            // 
            resources.ApplyResources(this.checkBox_char_limit, "checkBox_char_limit");
            this.checkBox_char_limit.Name = "checkBox_char_limit";
            this.toolTip1.SetToolTip(this.checkBox_char_limit, resources.GetString("checkBox_char_limit.ToolTip"));
            this.checkBox_char_limit.UseVisualStyleBackColor = true;
            // 
            // checkBox_punc
            // 
            resources.ApplyResources(this.checkBox_punc, "checkBox_punc");
            this.checkBox_punc.Name = "checkBox_punc";
            this.toolTip1.SetToolTip(this.checkBox_punc, resources.GetString("checkBox_punc.ToolTip"));
            this.checkBox_punc.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.toolTip1.SetToolTip(this.numericUpDown1, resources.GetString("numericUpDown1.ToolTip"));
            this.numericUpDown1.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            this.toolTip1.SetToolTip(this.textBox1, resources.GetString("textBox1.ToolTip"));
            // 
            // radioButton_after_punc
            // 
            resources.ApplyResources(this.radioButton_after_punc, "radioButton_after_punc");
            this.radioButton_after_punc.Checked = true;
            this.radioButton_after_punc.Name = "radioButton_after_punc";
            this.radioButton_after_punc.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton_after_punc, resources.GetString("radioButton_after_punc.ToolTip"));
            this.radioButton_after_punc.UseVisualStyleBackColor = true;
            // 
            // radioButton_before_punc
            // 
            resources.ApplyResources(this.radioButton_before_punc, "radioButton_before_punc");
            this.radioButton_before_punc.Name = "radioButton_before_punc";
            this.toolTip1.SetToolTip(this.radioButton_before_punc, resources.GetString("radioButton_before_punc.ToolTip"));
            this.radioButton_before_punc.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.toolTip1.SetToolTip(this.button2, resources.GetString("button2.ToolTip"));
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Frm_LineBreakRule
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.radioButton_before_punc);
            this.Controls.Add(this.radioButton_after_punc);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBox_punc);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.checkBox_char_limit);
            this.Controls.Add(this.checkBox_capital);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Frm_LineBreakRule";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox checkBox_capital;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox_char_limit;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox checkBox_punc;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RadioButton radioButton_after_punc;
        private System.Windows.Forms.RadioButton radioButton_before_punc;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}