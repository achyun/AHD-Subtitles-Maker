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
using System.Reflection;
using System.IO;
namespace AHD.SM.Formats
{
    /// <summary>
    /// Class of subtitle formats
    /// </summary>
    public class SubtitleFormats
    {
        static SubtitlesFormat[] formats;
        /// <summary>
        /// Get supported formats, DetectSupportedFormats must be called before using this
        /// </summary>
        public static SubtitlesFormat[] Formats
        { get { return formats; } }
        /// <summary>
        /// Get supported enabled formats, DetectSupportedFormats must be called before using this
        /// </summary>
        public static SubtitlesFormat[] EnabledFormats
        {
            get
            {
                List<SubtitlesFormat> list = new List<SubtitlesFormat>();
                int i = 0;
                foreach (SubtitlesFormat format in formats)
                {
                    if (format.Enabled)
                        list.Add(format);
                    i++;
                }
                return list.ToArray();
            }
        }
        /// <summary>
        /// Detect available formats
        /// </summary>
        public static void DetectSupportedFormats()
        {
            DetectSupportedFormats(false, false);
        }
        /// <summary>
        /// Detect available formats
        /// </summary>
        /// <param name="sort">Sort formats in the list?</param>
        /// <param name="AtoZ">Sort A to Z ?</param>
        public static void DetectSupportedFormats(bool sort, bool AtoZ)
        {
            List<SubtitlesFormat> availableFormats = new List<SubtitlesFormat>();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type tp in types)
            {
                if (tp.IsSubclassOf(typeof(SubtitlesFormat)))
                {
                    SubtitlesFormat format = Activator.CreateInstance(tp) as SubtitlesFormat;
                    availableFormats.Add(format);
                }
            }
            if (sort)
                availableFormats.Sort(new SubtitleFormatComparer(AtoZ));
            formats = availableFormats.ToArray();
        }
        /// <summary>
        /// Check the file to see what format it is
        /// </summary>
        /// <param name="FilePath">The subtitles format file</param>
        /// <param name="DeepSearch">If true, the program will check every format even if the extension is not the same, otherwise it will check depending on extension.</param>
        /// <param name="encoding">The encoding that will be used to open the file</param>
        /// <returns>Array of formats which could read this file</returns>
        public static SubtitlesFormat[] CheckFile(string FilePath, bool DeepSearch, Encoding encoding)
        {
            List<SubtitlesFormat> found = new List<SubtitlesFormat>();
            foreach (SubtitlesFormat format in formats)
            {
                bool yes = false;
                if (!DeepSearch)
                {
                    foreach (string ex in format.Extensions)
                    {
                        if (Path.GetExtension(FilePath).ToLower() == ex.ToLower())
                        { yes = true; break; }
                    }
                    if (yes)
                    {
                        if (format.CheckFile(FilePath, encoding))
                        {
                            found.Add(format);
                        }
                    }
                }
                else
                {
                    if (format.CheckFile(FilePath, encoding))
                    {
                        found.Add(format);
                    }
                }
            }
            return found.ToArray();
        }
        /// <summary>
        /// Get the filter that used in open/save file browser of supported ENABLED format 'disabled formats will NOT included'
        /// </summary>
        /// <returns>Filter includes all supported ENABLED formats</returns>
        public static string GetFilter()
        {
            string _Filter = "Supported subtitle formats|";
            foreach (SubtitlesFormat formm in EnabledFormats)
            {
                foreach (string ex in formm.Extensions)
                {
                    _Filter += "*" + ex + ";";
                }
            }
            _Filter = _Filter.Substring(0, _Filter.Length - 1) + "|";
            foreach (SubtitlesFormat formm in EnabledFormats)
            {
                _Filter += formm.Name + "|";
                foreach (string ex in formm.Extensions)
                {
                    if (ex == formm.Extensions[formm.Extensions.Length - 1])
                    { _Filter += "*" + ex; }
                    else { _Filter += "*" + ex + ";"; }
                }
                if (formm != EnabledFormats[EnabledFormats.Length - 1])
                { _Filter += "|"; }
            }
            return _Filter;
        }
    }
    public class SubtitleFormatComparer : IComparer<SubtitlesFormat>
    {
        bool AtoZ;
        public SubtitleFormatComparer(bool AtoZ)
        { this.AtoZ = AtoZ; }
        public int Compare(SubtitlesFormat x, SubtitlesFormat y)
        {
            if (AtoZ)
                return (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name);
            else
                return (-1 * (StringComparer.Create(System.Threading.Thread.CurrentThread.CurrentCulture, false)).Compare(x.Name, y.Name));
        }
    }
}
