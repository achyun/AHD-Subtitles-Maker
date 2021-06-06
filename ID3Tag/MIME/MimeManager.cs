/* This file is part of AHD ID3 Tag Editor (AITE)
 * A program that edit and create ID3 Tag.
 *
 * Copyright © Alaa Ibrahim Hadid 2012 - 2021
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AHD.ID3.MIME
{
    /// <summary>
    /// class for file extensions information
    /// </summary>
    public class MimeManager
    {
        private static List<MimeItem> items = new List<MimeItem>();
        /// <summary>
        /// Detect all available extensions from registry and fill the Items list
        /// </summary>
        public static void Refresh()
        {
            RegistryKey root = Registry.ClassesRoot;
            items.Clear();
            string[] subKeys = root.GetSubKeyNames();

            foreach (string subKey in subKeys)
            {
                if (subKey.StartsWith("."))
                {
                    string extension = subKey;
                    string contentType = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + subKey, "Content Type", "");
                    string progID = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + subKey, "", "");
                    string perceivedType = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + subKey, "PerceivedType", "");

                    items.Add(new MimeItem(extension, contentType, progID, perceivedType));
                }
            }
        }
        /// <summary>
        /// Get MIME (or content type of extension) of given file extension
        /// </summary>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The MIME (or content type of extension)</returns>
        public static string GetMime(string extension)
        {
            if (items.Count == 0)
                Refresh();
            foreach (MimeItem item in items)
            {
                if (extension.ToLower() == item.Extension.ToLower())
                    return item.ContentType;
            }
            return "";
        }
        /// <summary>
        /// Get ProgID of given file extension
        /// </summary>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The ProgID</returns>
        public static string GetProgID(string extension)
        {
            if (items.Count == 0)
                Refresh();
            foreach (MimeItem item in items)
            {
                if (extension.ToLower() == item.Extension.ToLower())
                    return item.ProgID;
            }
            return "";
        }
        /// <summary>
        /// Get PerceivedType of given file extension
        /// </summary>
        /// <param name="extension">The extension of the file</param>
        /// <returns>The PerceivedType</returns>
        public static string GetPerceivedType(string extension)
        {
            if (items.Count == 0)
                Refresh();
            foreach (MimeItem item in items)
            {
                if (extension.ToLower() == item.Extension.ToLower())
                    return item.PerceivedType;
            }
            return "";
        }
        /// <summary>
        /// Get extensions found for given content type (MIME)
        /// </summary>
        /// <param name="contentType">The content type (MIME)</param>
        /// <returns>List of available extensions</returns>
        public static string[] GetExtensions(string contentType)
        {
            if (items.Count == 0)
                Refresh();
            List<string> extensions = new List<string>();
            if (contentType == "")
                return extensions.ToArray();
            foreach (MimeItem item in items)
            {
                if (contentType.ToLower() == item.ContentType.ToLower())
                    extensions.Add(item.Extension);
            }
            return extensions.ToArray();
        }
        /// <summary>
        /// Get extensions found for given content type (MIME)
        /// </summary>
        /// <param name="contentType">The content type (MIME)</param>
        /// <returns>Available extensions as open/save file filter</returns>
        public static string GetExtensionsFilter(string contentType)
        {
            if (items.Count == 0)
                Refresh();
            string extensions = "";
            foreach (MimeItem item in items)
            {
                if (contentType.ToLower() == item.ContentType.ToLower())
                {
                    extensions += GetDescription(item.ProgID) + " (*" + item.Extension + ")|*" + item.Extension + "|";
                }
            }
            if (contentType == "")
                return "";
            if (extensions.Length > 1)
                return extensions.Substring(0, extensions.Length - 1);
            return "";
        }
        /// <summary>
        /// Get the description of program
        /// </summary>
        /// <param name="progID">The program id</param>
        /// <returns>The program description</returns>
        public static string GetDescription(string progID)
        {
            string description = (string)Registry.GetValue("HKEY_CLASSES_ROOT\\" + progID, "", "");
            return description;
        }
    }
}
