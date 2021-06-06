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
using System.Text;
using System.Collections.Generic;
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    /// <summary>
    /// The text frame
    /// </summary>
    [ID3TagFrameAttribute("Text Frame", "Text Frame", "")]
    public class TextFrame : ID3TagFrame
    {
        /// <summary>
        /// The text frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public TextFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Text frame", data, flags) { }

        private string text = "";
        private Encoding encoding;

        /// <summary>
        /// Get or set the text of this frame
        /// </summary>
        public string Text { get { return text; } set { text = value; encoding = ID3v2EncodingWrapper.GetBestEncoding(value); } }
        /// <summary>
        /// Get or set the text encoding
        /// </summary>
        public Encoding Encoding { get { return encoding; } set { encoding = value; } }

        /// <summary>
        /// Load the text frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            text = encoding.GetString(this.data, 1, this.data.Length - 1);
            text = text.Replace("\0", "");
        }
        /// <summary>
        /// Save the text frame data to the frame buffer data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            buffer.AddRange(encoding.GetBytes(text));
            this.data = buffer.ToArray();
        }
        /// <summary>
        /// The frame size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                return 1 + encoding.GetByteCount(text);
            }
        }
        /// <summary>
        /// Can save this text frame ? true if the text length is larger than 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return text.Length > 0;
            }
        }
    }
}
