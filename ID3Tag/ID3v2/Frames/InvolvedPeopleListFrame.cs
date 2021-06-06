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
    /// Involved people list frame
    /// </summary>
    public class InvolvedPeopleListFrame : ID3TagFrame
    {      
        /// <summary>
        /// The involved people list frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public InvolvedPeopleListFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Involved people list", data, flags) { }

        private List<InvolvedPeopleItem> items = new List<InvolvedPeopleItem>();
        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;

        /// <summary>
        /// Get or set the people list of this frame
        /// </summary>
        public List<InvolvedPeopleItem> PeopleList
        { get { return items; } set { items = value;  } }
        /// <summary>
        /// Get or set the text encoding
        /// </summary>
        public Encoding Encoding 
        { get { return encoding; } set { encoding = value; } }

        /// <summary>
        /// Load the frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            items.Clear();
            string code = encoding.GetString(Data, 1, Data.Length - 1);
            string[] list = code.Split(new char[] { '\0' });
            for (int i = 0; i < list.Length; i += 2)
            {
                if (i + 1 < list.Length)
                    items.Add(new InvolvedPeopleItem(list[i], list[i + 1]));
            }
        }
        /// <summary>
        /// Save the text frame data to the frame buffer data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding
            string encodingText = "";
            foreach (InvolvedPeopleItem item in items)
                encodingText += (item.Involvement + item.Involvee);
            encoding = ID3v2EncodingWrapper.GetBestEncoding(encodingText);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            //data
            foreach (InvolvedPeopleItem item in items)
            {
                buffer.AddRange(encoding.GetBytes(item.Involvement + "\0" + item.Involvee + "\0"));
            }
            //save
            this.data = buffer.ToArray();
        }
        /// <summary>
        /// The frame size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                int size = 1;
                foreach (InvolvedPeopleItem item in items)
                {
                    size += encoding.GetByteCount(item.Involvement + "\0" + item.Involvee + "\0");
                }
                return size;
            }
        }
        /// <summary>
        /// Can save this text frame ? true if the text length is larger than 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return (items != null) ? (items.Count > 0) : false;
            }
        }
    }
}
