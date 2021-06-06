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
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    /// <summary>
    /// Music CD identifier frame. Use frame data as cd bin data.
    /// </summary>
    public class MusicCDIdentifierFrame : ID3TagFrame
    {
        /// <summary>
        /// Music CD identifier frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">Data used in creation</param>
        /// <param name="flags">Flags</param>
        public MusicCDIdentifierFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Music CD identifier", data, flags) { }
        /*No need to do anything since we can use the data property as the cd data.*/

        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
        }

        public override bool CanSave
        {
            get
            {
                if (data != null)
                    return data.Length > 0;
                return false;
            }
        }
    }
}
