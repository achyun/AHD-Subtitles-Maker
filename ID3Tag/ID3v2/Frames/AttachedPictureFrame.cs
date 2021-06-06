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
    /// Attached picture frame
    /// </summary>
    public class AttachedPictureFrame : ID3TagFrame
    {
        /// <summary>
        /// The Attached picture frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public AttachedPictureFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Attached picture", data, flags) { }

        private string mime = "";
        private int picType = 0;
        private string desc = "";
        private byte[] picData = new byte[0];
        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;

        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public virtual Encoding Encoding
        { get { return encoding; } set { encoding = value; } }
        /// <summary>
        /// Get or set the mime
        /// </summary>
        public string MIME
        {
            get { return mime; }
            set { mime = value; }
        }
        /// <summary>
        /// Get or set the description
        /// </summary>
        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }
        /// <summary>
        /// Get or set the picture type
        /// </summary>
        public string PictureType
        {
            get { return ID3FrameConsts.PictureTypes[picType]; }
            set
            {
                for (int i = 0; i < ID3FrameConsts.PictureTypes.Length; i++)
                {
                    if (ID3FrameConsts.PictureTypes[i] == value)
                    {
                        picType = i; break;
                    }
                }
            }
        }
        /// <summary>
        /// Picture data
        /// </summary>
        public byte[] PictureData
        { get { return picData; } set { picData = value; } }

        /// <summary>
        /// Save frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(data[0]);
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            //read mime
            string code = ID3v2EncodingWrapper.ISO_8859_1.GetString(data, 1, data.Length - 1);
            string[] Ltext = code.Split(new char[] { '\0' });
            int index = 1;
            mime = Ltext[0]; index += mime.Length + 1;
            //type
            picType = data[index]; index++;
            //description (according to encoding)
            Ltext = encoding.GetString(data, index, data.Length - index).Split(new char[] { '\0' });

            desc = Ltext[0]; index += encoding.GetByteCount(desc) + count;
            //picture data
            picData = new byte[data.Length - index];
            for (int i = index; i < Data.Length - index; i++)
            {
                picData[i - index] = data[i];
            }
        }
        /// <summary>
        /// Load frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding
            encoding = ID3v2EncodingWrapper.GetBestEncoding(desc);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            //mime
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(mime));
            buffer.Add(0);
            //type
            buffer.Add((byte)picType);
            //desc
            buffer.AddRange(encoding.GetBytes(desc));
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            //pic data
            buffer.AddRange(picData);
            //save
            this.data = buffer.ToArray();
        }

        /// <summary>
        /// Get the frame size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
                return 1 + ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(mime + 1) + 1 + encoding.GetByteCount(desc) + count
              + picData.Length;
            }
        }
        /// <summary>
        /// Get if this frame can be saved
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return picData.Length > 0;
            }
        }
        /// <summary>
        /// AttachedPictureFrame.ToString()
        /// </summary>
        /// <returns>Description [PictureType]</returns>
        public override string ToString()
        {
            return desc + " [" + PictureType + "]";
        }
        /// <summary>
        /// Get if this frame equals another one
        /// </summary>
        /// <param name="frame">The frame to compare</param>
        /// <returns>True if equals otherwise false</returns>
        public bool Equals(AttachedPictureFrame frame)
        {
            if (mime == frame.mime &&
                picType == frame.picType &&
                picData.Length == frame.picData.Length)
            {
                for (int i = 0; i < picData.Length; i++)
                {
                    if (picData[i] != frame.picData[i])
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}
