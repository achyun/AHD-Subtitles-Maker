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
namespace AHD.ID3.Types
{
    /// <summary>
    /// The ID3v2 version
    /// </summary>
    public struct ID3Version
    {
        /// <summary>
        /// The ID3v2 version
        /// </summary>
        /// <param name="major">Major</param>
        /// <param name="revision">Revision</param>
        public ID3Version(byte major, byte revision)
        {
            this.major = major;
            this.revision = revision;
        }
        
        private byte major;
        private byte revision;

        /// <summary>
        /// Get the Major
        /// </summary>
        public byte Major { get { return major; } }
        /// <summary>
        /// Get the revision
        /// </summary>
        public byte Revision { get { return revision; } }
        /// <summary>
        /// ID3Version.ToString()
        /// </summary>
        /// <returns>Major.Revision</returns>
        public override string ToString()
        {
            return major + "." + revision;
        }
    }
}
