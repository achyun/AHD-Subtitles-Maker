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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NHunspell;

namespace AHD.SM.ASMP.SpellCheck
{
    public class SpellCheckerDictionary
    {
        public SpellCheckerDictionary(string directory, List<DictionaryLink> availableToDownload)
        {
            this.directory = directory;
            string[] files = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
            if (files != null)
            {
                string affFile = "";
                string dicFile = "";
                foreach (string file in files)
                {
                    if (Path.GetExtension(file).ToLower() == ".aff")
                    {
                        affFile = file;
                    }
                    if (Path.GetExtension(file).ToLower() == ".dic")
                    {
                        dicFile = file;
                    }
                }
                if (affFile != "" && dicFile != "")
                {
                    hunSpell = new Hunspell(affFile, dicFile);
                    isValid = true;
                }
            }
            if (isValid)
            {
                //get information
                foreach (DictionaryLink link in availableToDownload)
                {
                    if (link.Name == Path.GetFileName(directory))
                    {
                        name = link.Name;
                        nativeName = link.NativeName;
                        description = link.Description;
                        break;
                    }
                }
                //get the ignore list
                _ignoreList = new List<string>();
                if (File.Exists(directory + "\\asm_adds.txt"))
                {
                    _ignoreList = new List<string>(File.ReadAllLines(directory + "\\asm_adds.txt"));
                }
                //add the words to the dictionary
                foreach (string word in _ignoreList)
                    hunSpell.Add(word);

                //get the replacements list
                _replacementsList = new Dictionary<string, string>();
                if (File.Exists(directory + "\\asm_rplc.txt"))
                {
                    string[] lines = File.ReadAllLines(directory + "\\asm_rplc.txt");
                    foreach (string line in lines)
                    {
                        string[] code = line.Split(new char[] { '=' });
                        _replacementsList.Add(code[0], code[1]);
                    }
                }
            }
        }

        private string name;
        private string nativeName;
        private string description;
        private bool isValid = false;
        private Hunspell hunSpell;
        private List<string> _ignoreList = new List<string>();
        private Dictionary<string, string> _replacementsList = new Dictionary<string, string>();
        private string directory;

        /// <summary>
        /// Get the name of this dictionary in English
        /// </summary>
        public string Name
        { get { return name; } }
        /// <summary>
        /// Get the original language name
        /// </summary>
        public string NativeName
        { get { return nativeName; } }
        /// <summary>
        /// Get the description of this dictionary
        /// </summary>
        public string Description
        { get { return description; } }
        /// <summary>
        /// Get if this dictionary is valid and can be used
        /// </summary>
        public bool IsValid
        { get { return isValid; } }
        /// <summary>
        /// Get SpellCheckerDictionary.ToString()
        /// </summary>
        /// <returns>string : name + " - " + nativeName</returns>
        public override string ToString()
        {
            return name + " - " + nativeName;
        }
        /// <summary>
        /// Get the Hunspell class that used to spelling
        /// </summary>
        public Hunspell Hunspell
        { get { return hunSpell; } }
        /// <summary>
        /// Get or set the ignore list
        /// </summary>
        public List<string> IgnoreList
        { get { return _ignoreList; } set { _ignoreList = value; } }
        /// <summary>
        /// Get or set the replacements list
        /// </summary>
        public Dictionary<string, string> ReplacementsList
        { get { return _replacementsList; } set { _replacementsList = value; } }
        /// <summary>
        /// Get the dictionary folder path
        /// </summary>
        public string FolderPath
        { get { return directory; } }

        /// <summary>
        /// Save the dictionary additional items like the adds list
        /// </summary>
        public void Save()
        {
            if (isValid)
            {
                //save the ignore list
                File.WriteAllLines(directory + "\\asm_adds.txt", _ignoreList.ToArray());
                //save the replacements list
                List<string> lines = new List<string>();
                foreach (string key in _replacementsList.Keys)
                {
                    lines.Add(key + "=" + _replacementsList[key]);
                }
                File.WriteAllLines(directory + "\\asm_rplc.txt", lines.ToArray());
            }
        }
    }
}
