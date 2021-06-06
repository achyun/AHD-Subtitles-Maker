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

namespace AHD.ID3.MIME
{
    /// <summary>
    /// The mime item
    /// </summary>
    public struct MimeItem
    {
        /// <summary>
        /// The mime item
        /// </summary>
        /// <param name="extension">The file extension</param>
        /// <param name="contentType">The content type</param>
        /// <param name="progID">The program id</param>
        /// <param name="perceivedType">The perceived type</param>
        public MimeItem(string extension, string contentType, string progID, string perceivedType)
        {
            this.extension = extension;
            this.contentType = contentType;
            this.progID = progID;
            this.perceivedType = perceivedType;
        }
        private string extension;
        private string contentType;
        private string progID;
        private string perceivedType;
        /// <summary>
        /// Get or set the extension of this file
        /// </summary>
        public string Extension
        { get { return extension; } set { extension = value; } }
        /// <summary>
        /// Get or set the MIME of this file
        /// </summary>
        public string ContentType
        { get { return contentType; } set { contentType = value; } }
        /// <summary>
        /// Get or set the program id
        /// </summary>
        public string ProgID
        { get { return progID; } set { progID = value; } }
        /// <summary>
        /// Get or set the Perceived Type
        /// </summary>
        public string PerceivedType
        { get { return perceivedType; } set { perceivedType = value; } }
        /// <summary>
        /// MimeItem.ToString()
        /// </summary>
        /// <returns>extension</returns>
        public override string ToString()
        {
            return extension;
        }
    }
}
