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
    /// CommentsFrame
    /// </summary>
    public class CommentsFrame : ID3TagFrame
    {
        /// <summary>
        /// The Comments frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public CommentsFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Comments", data, flags) { }

        private string languageID = "";
        private string contentDescriptor = "";
        private string text = "";
        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;

        /// <summary>
        /// Get or set the language id
        /// </summary>
        public string LanguageID
        {
            get
            {
                return languageID;
            }
            set
            {
                languageID = value;
            }
        }
        /// <summary>
        /// Get or set the content descriptor
        /// </summary>
        public string ContentDescriptor
        {
            get { return contentDescriptor; }
            set { contentDescriptor = value; }
        }
        /// <summary>
        /// Get or set the text
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public virtual Encoding Encoding
        { get { return encoding; } set { encoding = value; } }

        /// <summary>
        /// Save frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            languageID = ID3v2EncodingWrapper.ISO_8859_1.GetString(new byte[] { Data[1], Data[2], Data[3] });

            string code = encoding.GetString(Data, 4, Data.Length - 4);

            string[] Ltext = code.Split(new char[] { '\0' });

            if (Ltext.Length > 0)
                contentDescriptor = Ltext[0].Replace("\0", "");

            if (Ltext.Length > 1)
                text = Ltext[1].Replace("\0", "");
        }
        /// <summary>
        /// Load frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding
            encoding = ID3v2EncodingWrapper.GetBestEncoding(contentDescriptor + text);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            //language "id"
            // fix
            if (languageID.Length != 3 || languageID.Contains("\0"))
                languageID = "\0\0\0";//000
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(languageID.ToUpper()));
            //content descriptor
            buffer.AddRange(encoding.GetBytes(contentDescriptor));

            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            for (int i = 0; i < count; i++)
                buffer.Add(0);

            //text
            buffer.AddRange(encoding.GetBytes(text));
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
                return 4 + encoding.GetByteCount(contentDescriptor + text) + count;
            }
        }
        /// <summary>
        /// Get if this frame can be saved
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return text.Length > 0;
            }
        }
        /// <summary>
        /// CommentsFrame.ToString()
        /// </summary>
        /// <returns>Content Descriptor [LanguageID]</returns>
        public override string ToString()
        {
            if (!LanguageID.Contains("\0"))
                return contentDescriptor + " [" + LanguageID + "]";
            else
                return contentDescriptor;
        }
    }
}
