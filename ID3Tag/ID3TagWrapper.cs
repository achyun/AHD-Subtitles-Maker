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

namespace AHD.ID3
{
    /// <summary>
    /// A class used to get mp3 id3 tag useful information. Also can be used to save id3 both versions quickly.
    /// </summary>
    public class ID3TagWrapper
    {
        /// <summary>
        /// Create new instance for the mp3 id3 tag wrapper class.
        /// </summary>
        /// <param name="filePath">The complete file path. Must be existed mp3 file.</param>
        public ID3TagWrapper(string filePath)
        {
            this.filePath = filePath;
            this.v1 = new ID3v1();
            this.v2 = new ID3v2();
            this.hasV1 = ID3v1.Check(filePath);
            this.hasV2 = ID3v2.Check(filePath);
        }
        /// <summary>
        /// Create new instance for the mp3 id3 tag wrapper class.
        /// </summary>
        /// <param name="filePath">The complete file path. Must be existed mp3 file.</param>
        /// <param name="load">Load id3 tag data ?</param>
        public ID3TagWrapper(string filePath, bool load)
        {
            this.filePath = filePath;
            this.v1 = new ID3v1();
            this.v2 = new ID3v2();
            this.hasV1 = ID3v1.Check(filePath);
            this.hasV2 = ID3v2.Check(filePath);
            if (load)
            {
                this.v2.Load(filePath);
                this.v2Wrapper = new ID3v2QuickWrapper(this.v2);
                this.v1.Load(filePath);
            }
        }

        private ID3v1 v1;
        private ID3v2 v2;
        private ID3v2QuickWrapper v2Wrapper;
        private string filePath;
        private bool hasV1 = false;
        private bool hasV2 = false;

        /// <summary>
        /// Get the <see cref="ID3v1"/>
        /// </summary>
        public ID3v1 ID3v1
        { get { return v1; } }
        /// <summary>
        /// Get the <see cref="ID3v2"/>
        /// </summary>
        public ID3v2 ID3v2
        { get { return v2; } }
        /// <summary>
        /// Get the <see cref="ID3v2QuickWrapper"/>
        /// </summary>
        public ID3v2QuickWrapper ID3v2QuickWrapper
        { get { return v2Wrapper; } }
        /// <summary>
        /// Get a value indecate whether this file has id3 tag version 1
        /// </summary>
        public bool HasID3V1
        { get { return hasV1; } }
        /// <summary>
        /// Get a value indecate whether this file has id3 tag version 2
        /// </summary>
        public bool HasID3V2
        { get { return hasV2; } }

        /// <summary>
        /// Save id3 tag data
        /// </summary>
        /// <param name="filePath">The complete mp3 file path</param>
        /// <returns>The result of the save operation</returns>
        public Result Save(string filePath)
        {
            return v2.Save(filePath, v1);
        }
    }
}
