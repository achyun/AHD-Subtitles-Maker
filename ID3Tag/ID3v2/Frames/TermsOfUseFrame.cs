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
    /// The Terms Of Use Frame
    /// </summary>
    public class TermsOfUseFrame : ID3TagFrame
    {
        /// <summary>
        /// The Terms of use frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public TermsOfUseFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Terms Of Use", data, flags) { }

        private string languageID = "";
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
        /// Load the data of this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            string code = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, 1, Data.Length - 1);
            languageID = code.Substring(0, 3);

            text = encoding.GetString(Data, 4, Data.Length - 4);
        }
        /// <summary>
        /// Save the data for this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding
            encoding = ID3v2EncodingWrapper.GetBestEncoding(text);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            if (languageID.Length != 3 || languageID.Contains("\0"))
                languageID = "ENG";
            //language "id"
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(languageID.ToUpper()));
            //text
            buffer.AddRange(encoding.GetBytes(text));
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
                return 4 + encoding.GetByteCount(text);
            }
        }
        /// <summary>
        /// Get if this frame can be saved to the tag
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return text.Length > 0;
            }
        }
        /// <summary>
        /// TermsOfUseFrame.ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
