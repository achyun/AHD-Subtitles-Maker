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
    /// The ID3 Tag version 3 frames flags
    /// </summary>
    public struct Version3FrameFlags
    {
        /// <summary>
        /// The ID3 Tag version 3 frames flags
        /// </summary>
        /// <param name="flags">The flags value</param>
        public Version3FrameFlags(int flags)
        {
            this.flags = flags;
        }

        private int flags;

        /// <summary>
        /// This flag tells the software what to do with this frame if it is 
        /// unknown and the tag is altered in any way. This applies to all
        /// kinds of alterations, including adding more padding and reordering
        /// the frames.
        /// false: Frame should be preserved.
        /// true: Frame should be discarded.
        /// </summary>
        public bool TagAlterPreservation
        { get { return (flags & 0x8000) == 0x8000; } set { flags = (flags & 0x7FFF) | (value ? 0x8000 : 0x0000); } }
        /// <summary>
        /// This flag tells the software what to do with this frame if it is
        /// unknown and the file, excluding the tag, is altered. This does not
        /// apply when the audio is completely replaced with other audio data.
        /// false: Frame should be preserved.
        /// true: Frame should be discarded.
        /// </summary>
        public bool FileAlterPreservation
        { get { return (flags & 0x4000) == 0x4000; } set { flags = (flags & 0xBFFF) | (value ? 0x4000 : 0x0000); } }
        /// <summary>
        /// This flag, if set, tells the software that the contents of this
        /// frame is intended to be read only. Changing the contents might
        /// break something, e.g. a signature. If the contents are changed,
        /// without knowledge in why the frame was flagged read only and
        /// without taking the proper means to compensate, e.g. recalculating
        /// the signature, the bit should be cleared.
        /// </summary>
        public bool ReadOnly
        { get { return (flags & 0x2000) == 0x2000; } set { flags = (flags & 0xDFFF) | (value ? 0x2000 : 0x0000); } }
        /// <summary>
        /// This flag indicates whether or not the frame is compressed.
        /// false: Frame is not compressed.
        /// true: Frame is compressed using zlib [zlib] with 4 bytes for
        /// 'decompressed size' appended to the frame header.
        /// </summary>
        public bool Compression
        { get { return (flags & 0x80) == 0x80; } set { flags = (flags & 0xFF7F) | (value ? 0x80 : 0x00); } }
        /// <summary>
        /// This flag indicates wether or not the frame is enrypted. If set 
        /// one byte indicating with which method it was encrypted will be 
        /// appended to the frame header. See section 4.26. for more
        /// information about encryption method registration.
        /// false: Frame is not encrypted.
        /// true: Frame is encrypted.
        /// </summary>
        public bool Encryption
        { get { return (flags & 0x40) == 0x40; } set { flags = (flags & 0xFFBF) | (value ? 0x40 : 0x00); } }
        /// <summary>
        /// This flag indicates whether or not this frame belongs in a group 
        /// with other frames. If set a group identifier byte is added to the 
        /// frame header. Every frame with the same group identifier belongs 
        /// to the same group.
        /// </summary>
        public bool GroupingIdentity
        { get { return (flags & 0x20) == 0x20; } set { flags = (flags & 0xFFDF) | (value ? 0x20 : 0x00); } }
        /// <summary>
        /// Get the flags as integer
        /// </summary>
        public int Flags
        { get { return flags; } }
    }
}
