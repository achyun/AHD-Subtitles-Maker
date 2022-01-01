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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Resources;
using AHD.SM.ASMP;
using AHD.SM.ASMP.SpellCheck;
using AHD.Forms;

namespace AHD.SM.Forms
{
    public partial class Frm_SpellCheckSubtitle : Form
    {
        public Frm_SpellCheckSubtitle(Subtitle subtitle, SpellCheckerDictionary dic)
        {
            this.dic = dic;
            this.subtitle = subtitle;
            InitializeComponent();
            //view the text in the editor
            subtitleTextEditor1.SubtitleText = subtitle.Text;
            //get words as array to use in the spell checker
            words = subtitle.Text.ToString().Split(" .,-?!:;\"“”()[]{}|<>/+\r\n¿¡…—".ToCharArray());
            //spell the next word
            wordIndex = 0;
            CheckNextWord();
        }

        private SpellCheckerDictionary dic;
        private Subtitle subtitle;
        private ResourceManager resources = new ResourceManager("AHD.SM.Forms.LanguageResources.Resource",
            Assembly.GetExecutingAssembly());
        private string[] words;
        private int wordIndex = 0;
        private CheckResult result = CheckResult.Abort;

        private void CheckNextWord()
        {
            if (wordIndex < words.Length)
            {
                //is this a checkable word
                if (words[wordIndex].Length > 0 &&
                    !words[wordIndex].Contains("0") &&
                    !words[wordIndex].Contains("1") &&
                    !words[wordIndex].Contains("2") &&
                    !words[wordIndex].Contains("3") &&
                    !words[wordIndex].Contains("4") &&
                    !words[wordIndex].Contains("5") &&
                    !words[wordIndex].Contains("6") &&
                    !words[wordIndex].Contains("7") &&
                    !words[wordIndex].Contains("8") &&
                    !words[wordIndex].Contains("9") &&
                    !words[wordIndex].Contains("%") &&
                    !words[wordIndex].Contains("&") &&
                    !words[wordIndex].Contains("@") &&
                    !words[wordIndex].Contains("$") &&
                    !words[wordIndex].Contains("*") &&
                    !words[wordIndex].Contains("=") &&
                    !words[wordIndex].Contains("£") &&
                    !words[wordIndex].Contains("#") &&
                    !words[wordIndex].Contains("_") &&
                    !words[wordIndex].Contains("½") &&
                    !words[wordIndex].Contains("^") &&
                    !words[wordIndex].Contains("£"))
                {
                    //check the word
                    if (dic.Hunspell.Spell(words[wordIndex]))
                    {
                        wordIndex++;
                        CheckNextWord();
                    }
                    //let's see if this word exist in the replacements list to replace directly
                    else if (dic.ReplacementsList.Keys.Contains(words[wordIndex]))
                    {
                        subtitleTextEditor1.Replace(words[wordIndex], dic.ReplacementsList[words[wordIndex]]);
                        subtitleTextEditor1.SaveTextToSubtitle();
                        this.subtitle.Text = subtitleTextEditor1.SubtitleText;
                        wordIndex++;
                        CheckNextWord();
                    }
                    else
                    {
                        //show the word
                        textBox_word.Text = words[wordIndex];
                        subtitleTextEditor1.SelectWord(words[wordIndex]);
                        //get suggestions
                        listBox1.Items.Clear();
                        string[] suggestions = dic.Hunspell.Suggest(words[wordIndex]).ToArray();
                        listBox1.Items.AddRange(suggestions);
                        if (listBox1.Items.Count > 0)
                            listBox1.SelectedIndex = 0;
                    }
                }
                else//ignore the word
                {
                    wordIndex++;
                    CheckNextWord();
                }
            }
            else//end of the text !! abort with ok.
            {
                result |= CheckResult.Ok;
                Close();
            }
        }
        public CheckResult ResultOfCheck
        { get { return result; } }
        public enum CheckResult
        {
            Ok = 1,
            IgnoredSomeWords = 2,
            Abort = 4
        }
        //abort
        private void button1_Click(object sender, EventArgs e)
        {
            result = CheckResult.Abort;
            Close();
        }
        //ok, next word, assuming the user fixed the error automatically
        private void button8_Click(object sender, EventArgs e)
        {
            wordIndex++;
            CheckNextWord();
        }
        //Use
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                subtitleTextEditor1.Replace(textBox_word.Text, listBox1.SelectedItem.ToString());
                subtitleTextEditor1.SaveTextToSubtitle();
                this.subtitle.Text = subtitleTextEditor1.SubtitleText;
                wordIndex++;
                CheckNextWord();
            }
            else
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectWordFromsuggestions"), 
                    resources.GetString("MessageCaption_SpellCheck"));
            }
        }
        //Use always
        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                subtitleTextEditor1.Replace(textBox_word.Text, listBox1.SelectedItem.ToString());
                subtitleTextEditor1.SaveTextToSubtitle();
                this.subtitle.Text = subtitleTextEditor1.SubtitleText;
                //add to the replace list
                dic.ReplacementsList.Add(textBox_word.Text, listBox1.SelectedItem.ToString());
                wordIndex++;
                CheckNextWord();
            }
            else
            {
                MessageDialog.ShowMessage(resources.GetString("Message_PleaseSelectWordFromsuggestions"),
                    resources.GetString("MessageCaption_SpellCheck"));
            }
        }
        //ignore
        private void button2_Click(object sender, EventArgs e)
        {
            wordIndex++;
            CheckNextWord();
            result |= Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords;
        }
        //Ignore always
        private void button3_Click(object sender, EventArgs e)
        {
            //add the word to the ignore list
            dic.IgnoreList.Add(textBox_word.Text);
            //add to hunspell
            dic.Hunspell.Add(textBox_word.Text);
            wordIndex++;
            CheckNextWord();
            result |= Frm_SpellCheckSubtitle.CheckResult.IgnoredSomeWords;
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            button5_Click(this, new EventArgs());   //Use
        }
    }
}
