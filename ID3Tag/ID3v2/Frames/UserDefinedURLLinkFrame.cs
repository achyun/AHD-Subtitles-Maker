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
    /// The user defined URL link frame
    /// </summary>
    public class UserDefinedURLLinkFrame : ID3TagFrame
    {
        /// <summary>
        /// The user defined URL link frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public UserDefinedURLLinkFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "User defined URL link frame", data, flags) { }

        private string description = "";
        private string url = "";
        private Encoding encoding;
        private ASCIIEncoding asciiEncoding = new ASCIIEncoding();

        /// <summary>
        /// Get or set the url link
        /// </summary>
        public string URL
        {
            get { return url; }
            set { url = value; }
        }
        /// <summary>
        /// Get or set the description of this frame
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; encoding = ID3v2EncodingWrapper.GetBestEncoding(description); }
        }
        /// <summary>
        /// Get or set the text encoding
        /// </summary>
        public Encoding Encoding 
        { get { return encoding; } set { encoding = value; } }
        /// <summary>
        /// Load and apply data to this frame
        /// </summary>
        /// <param name="tagVersion">The id3 tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == 0)
                {
                    i += count - 1;
                    description = encoding.GetString(this.data, 1, i - count);
                    i++;
                    url = asciiEncoding.GetString(this.data, i, this.data.Length - i);
                    // fix some stupid programs saving
                    description = description.Replace("\0", "");
                    url = url.Replace("\0", "");
                    break;
                }
            }
        }
        /// <summary>
        /// Save the frame
        /// </summary>
        /// <param name="tagVersion">The id3 tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            buffer.AddRange(encoding.GetBytes(description));
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            buffer.AddRange(asciiEncoding.GetBytes(url));
            this.data = buffer.ToArray();
        }
        /// <summary>
        /// Can save ? true if both description and url link are larger than 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return description.Length > 0 && url.Length > 0;
            }
        }
        /// <summary>
        /// Get the size of this frame (calculated, not the data size)
        /// </summary>
        public override int Size
        {
            get
            {
                return 1 + encoding.GetByteCount(description) + ID3v2EncodingWrapper.GetTerminationBytesCount(encoding) +
                    asciiEncoding.GetByteCount(url);
            }
        }
    }
}
