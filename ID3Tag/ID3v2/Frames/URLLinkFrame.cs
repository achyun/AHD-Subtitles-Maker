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
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    /// <summary>
    /// The text frame
    /// </summary>
    public class URLLinkFrame : ID3TagFrame
    {
        /// <summary>
        /// The URL Link Frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public URLLinkFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "URL link frame", data, flags) { }

        private string url = "";
        private ASCIIEncoding encoding = new ASCIIEncoding();

        /// <summary>
        /// Get or set the url of this frame
        /// </summary>
        public string URL { get { return url; } set { url = value; } }

        /// <summary>
        /// Load the url link frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            url = encoding.GetString(this.data);
        }
        /// <summary>
        /// Save the url link frame data to the frame buffer data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            this.data = encoding.GetBytes(url);
        }
        /// <summary>
        /// The frame size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                return encoding.GetByteCount(url);
            }
        }
        /// <summary>
        /// Can save this text frame ? true if the url length is larger than 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return url.Length > 0;
            }
        }
    }
}
