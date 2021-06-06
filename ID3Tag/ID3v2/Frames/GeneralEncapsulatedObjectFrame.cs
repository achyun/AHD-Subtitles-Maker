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
using System.Collections.Generic;
using System.Text;
using AHD.ID3.Types;

namespace AHD.ID3.Frames
{
    /// <summary>
    /// The General encapsulated object frame
    /// </summary>
    public class GeneralEncapsulatedObjectFrame : ID3TagFrame
    {
        /// <summary>
        /// The General encapsulated object frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public GeneralEncapsulatedObjectFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "General encapsulated object", data, flags) { }

        private byte[] fileData;
        private string desc = "";
        private string filename = "";
        private string mime = "";
        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;

        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public virtual Encoding Encoding
        { get { return encoding; } set { encoding = value; } }
        /// <summary>
        /// Get or set the file mime
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
        /// Get or set the file name
        /// </summary>
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        /// <summary>
        /// Get or set the file data array
        /// </summary>
        public byte[] FileData
        { get { return fileData; } set { fileData = value; } }

        /// <summary>
        /// Load the data of this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            //mime
            string code = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, 1, Data.Length - 1);
            string[] Ltext = code.Split(new char[] { '\0' });
            int index = 1;
            mime = Ltext[0];
            index += ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(mime + "\0");
            //filename
            code = encoding.GetString(Data, index, Data.Length - index);
            Ltext = code.Split(new char[] { '\0' });
            filename = Ltext[0];
            index += encoding.GetByteCount(filename + "\0");
            //desc
            desc = Ltext[1];
            index += encoding.GetByteCount(desc + "\0");

            fileData = new byte[Data.Length - index];
            for (int i = index; i < Data.Length - index; i++)
            {
                fileData[i - index] = Data[i];
            }
        }
        /// <summary>
        /// Save the data for this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.GetBestEncoding(filename + desc);
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);

            List<byte> buffer = new List<byte>();
            //encoding
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            //mime
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(mime));
            buffer.Add(0);
            //file name
            buffer.AddRange(encoding.GetBytes(filename));
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            //desc
            buffer.AddRange(encoding.GetBytes(desc));
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            //pic data
            buffer.AddRange(fileData);
            //save
            this.data = buffer.ToArray();
        }
        /// <summary>
        /// Get the size of this frame (calculated)
        /// </summary>
        public override int Size
        {
            get
            {
                encoding = ID3v2EncodingWrapper.GetBestEncoding(filename + desc);
                int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);

                return 1 + ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(mime) + 1 +
            encoding.GetByteCount(filename) + count +
           encoding.GetByteCount(desc) + count + fileData.Length;
            }
        }
        /// <summary>
        /// Get if this frame can be saved to the tag
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return fileData.Length > 0;
            }
        }
        /// <summary>
        /// GeneralEncapsulatedObjectFrame.ToString()
        /// </summary>
        /// <returns>Description [FileName]</returns>
        public override string ToString()
        {
            return desc + " [" + filename + "]";
        }
    }
}
