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
using System.Text;
using System.Collections.Generic;
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    /// <summary>
    /// The unique file identifier frame
    /// </summary>
    public class UniqueFileIdentifierFrame : ID3TagFrame
    {
        /// <summary>
        /// The unique file identifier frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public UniqueFileIdentifierFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Unique file identifier frame", data, flags) { }

        private string ownerIdentifier = "";
        private byte[] binData = new byte[0];
        private ASCIIEncoding encoding = new ASCIIEncoding();

        /// <summary>
        /// Get or set the owner identifier
        /// </summary>
        public string OwnerIdentifier { get { return ownerIdentifier; } set { ownerIdentifier = value; } }
        /// <summary>
        /// Get or set the identifier bin data
        /// </summary>
        public byte[] Identifier { get { return binData; } set { binData = value; } }

        /// <summary>
        /// Load data from data buffer
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>(data);// lists is the best for spliting, get range ...etc

            // search for the first 00
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer[i] == 0)
                {
                    // this is it !
                    ownerIdentifier = encoding.GetString(buffer.GetRange(0, i).ToArray());
                    i++;
                    binData = buffer.GetRange(i, buffer.Count - i).ToArray();
                    break;
                }
            }
        }
        /// <summary>
        /// Save values to data buffer and make it ready for saving
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(encoding.GetBytes(ownerIdentifier));
            buffer.Add(0);
            buffer.AddRange(binData);
            data = buffer.ToArray();
        }
        /// <summary>
        /// Get the size of this frame
        /// </summary>
        public override int Size
        {
            get
            {
                return encoding.GetByteCount(ownerIdentifier) + 1 + binData.Length;
            }
        }
        /// <summary>
        /// Can save this frame ? true if both owner identifier and identifier data are larger than 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return (ownerIdentifier.Length > 0) && (binData.Length > 0);
            }
        }

        /// <summary>
        /// Owner Identifier
        /// </summary>
        /// <returns>Owner Identifier name</returns>
        public override string ToString()
        {
            return ownerIdentifier;
        }
    }
}
