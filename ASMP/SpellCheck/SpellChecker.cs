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
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using SevenZip;

namespace AHD.SM.ASMP.SpellCheck
{
    /// <summary>
    /// A spell check service.
    /// </summary>
    public class SpellChecker
    {
        /// <summary>
        /// Initialize the spell checker and load existed dictionaries if available.
        /// </summary>
        /// <param name="folder">The dictionaries folder</param>
        public SpellChecker(string folder)
        {
            //Create the directory if not exists
            Directory.CreateDirectory(folder);

            //load "available to download" list
            File.WriteAllText(Path.GetTempPath() + "\\dics.xml", Properties.Resources.OpenOfficeDictionaries);

            XmlReaderSettings sett = new XmlReaderSettings();
            sett.DtdProcessing = DtdProcessing.Ignore;

            XmlReader XMLread = XmlReader.Create(Path.GetTempPath() + "\\dics.xml", sett);

            while (XMLread.Read())
            {
                //check the header
                if (XMLread.Name == "EnglishName" && XMLread.IsStartElement())
                {
                    string name = XMLread.ReadString();
                    string nativeName = "";
                    string link = "";
                    string description = "";
                    while (XMLread.Read())
                    {
                        if (XMLread.Name == "NativeName" && XMLread.IsStartElement())
                        {
                            nativeName = XMLread.ReadString();
                        }
                        if (XMLread.Name == "DownloadLink" && XMLread.IsStartElement())
                        {
                            link = XMLread.ReadString();
                        }
                        if (XMLread.Name == "Description" && XMLread.IsStartElement())
                        {
                            description = XMLread.ReadString();
                        }
                        if (XMLread.Name == "Dictionary" && !XMLread.IsStartElement())
                        {
                            availableToDownload.Add(new DictionaryLink(name, nativeName, link, description));
                            break;
                        }
                    }
                }
            }

            XMLread.Close();
            try { File.Delete(Path.GetTempPath() + "\\dics.xml"); }
            catch { }

            RefreshDictionaries(folder);
        }

        private List<SpellCheckerDictionary> dictionaries = new List<SpellCheckerDictionary>();
        private List<DictionaryLink> availableToDownload = new List<DictionaryLink>();

        public void RefreshDictionaries(string folder)
        {
            dictionaries = new List<SpellCheckerDictionary>();
            //check if the dictionaries folder include extensions !
            string[] files = Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).ToLower() == ".oxt")
                {
                    try
                    {
                        string dicName = Path.GetFileNameWithoutExtension(file);
                        for (int i = 0; i < availableToDownload.Count; i++)
                        {
                            string[] splitted = availableToDownload[i].Link.Split('/');
                            if (splitted.Length > 0)
                            {
                                if (Path.GetFileNameWithoutExtension(splitted[splitted.Length - 1]).ToLower()
                                    == Path.GetFileNameWithoutExtension(file).ToLower())
                                {
                                    dicName = availableToDownload[i].Name;
                                    break;
                                }
                            }
                        }
                        string dicPath = folder + "\\" + dicName;
                        if (!Directory.Exists(dicPath))
                        {
                            //extract
                            SevenZipExtractor extractor = new SevenZipExtractor(file);
                            Directory.CreateDirectory(dicPath);
                            extractor.ExtractArchive(dicPath);
                        }
                    }
                    catch { }
                }
            }
            //load dictionary files
            string[] dirs = Directory.GetDirectories(folder);
            if (dirs != null)
            {
                foreach (string dir in dirs)
                {
                    SpellCheckerDictionary dictionary = new SpellCheckerDictionary(dir, availableToDownload);
                    if (dictionary.IsValid)
                        dictionaries.Add(dictionary);
                }
            }
        }
        //Properties
        /// <summary>
        /// Get available dictionaries
        /// </summary>
        public List<SpellCheckerDictionary> Dictionaries
        { get { return dictionaries; } }
        /// <summary>
        /// Get available dictionaries to download
        /// </summary>
        public List<DictionaryLink> AvailableToDownload
        { get { return availableToDownload; } }
    }
}
