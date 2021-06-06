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
using System.IO;
using System.Collections.Generic;
using AHD.ID3.Types;
namespace AHD.ID3
{
    /// <summary>
    /// The extended header class. Extended header is optional and supported only for version 3 and 4
    /// </summary>
    public class ExtendedHeader
    {
        private int size = 0;
        private byte[] buffer;
        /// <summary>
        /// Read the extended header
        /// </summary>
        /// <param name="stream">The opened file stream that will be used to read this header, the stream MUST be seeked to 10 (after ID3 header)</param>
        /// <param name="version">The id3 version used</param>
        public void Read(Stream stream, ID3Version version)
        {
            byte[] header = new byte[4];//read size
            stream.Read(header, 0, 4);
            switch (version.Major)
            {
                default: size = SynchsafeConvertor.FromBytes(header); break;
                case 4: size = SynchsafeConvertor.FromSynchsafe(header); break;
            }

            //now we know the size, read and store this header into buffer, since it's optional
            buffer = new byte[size];
            stream.Read(buffer, 0, size);
        }
        /// <summary>
        /// Save the extended header
        /// </summary>
        /// <param name="stream">The opened file stream that will be used to save this header, the stream MUST be seeked to 10 (after ID3 header)</param>
        /// <param name="version">The id3 version used</param>
        public void Save(Stream stream, ID3Version version)
        {
            if (size == 0) return;
            //size
            switch (version.Major)
            {
                default: stream.Write(SynchsafeConvertor.ToInt32Bytes(size), 0, 4); break;
                case 4: stream.Write(SynchsafeConvertor.ToSynchsafeBytes(size), 0, 4); break;
            }
            //data
            stream.Write(buffer, 0, buffer.Length);
        }
        /// <summary>
        /// Save the extended header
        /// </summary>
        /// <param name="buffer">Dynamic buffer used to save the ID3 Tag, this buffer should be filled with ID3v2 Tag header</param>
        /// <param name="version">The id3 version used</param>
        public void Save(List<byte> buffer, ID3Version version)
        {
            if (size == 0) return;
            //size
            switch (version.Major)
            {
                default: buffer.AddRange(SynchsafeConvertor.ToInt32Bytes(size)); break;
                case 4: buffer.AddRange(SynchsafeConvertor.ToSynchsafeBytes(size)); break;
            }
            //data
            buffer.AddRange(buffer);
        }
        /// <summary>
        /// Get or set the size of extended header
        /// </summary>
        public int Size
        { get { return size; } set { size = value; } }
    }
}
