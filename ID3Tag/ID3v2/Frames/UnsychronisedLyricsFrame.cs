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
    /// Unsychronised lyrics frame
    /// </summary>
    public class UnsychronisedLyricsFrame : ID3TagFrame
    {
        /// <summary>
        /// The unsychronised lyrics frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public UnsychronisedLyricsFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Unsychronised lyrics", data, flags) { }

        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;
        private Encoding asci = ID3v2EncodingWrapper.ISO_8859_1;
        private string languageID = "ENG";
        private string contentDescriptor = "";
        private string lyricsText = "";

        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public Encoding Encoding
        { get { return encoding; } set { encoding = value; } }
        /// <summary>
        /// Get or set the language id. The language id must be 3 chars in upper case
        /// </summary>
        public string LanguageID
        { get { return languageID; } set { languageID = value; } }
        /// <summary>
        /// Get or set the content descriptor
        /// </summary>
        public string ContentDescriptor
        { get { return contentDescriptor; } set { contentDescriptor = value; } }
        /// <summary>
        /// Get or set the lyrics text
        /// </summary>
        public string LyricsText
        { get { return lyricsText; } set { lyricsText = value; } }

        /// <summary>
        /// Load frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(data[0]);

            languageID = asci.GetString(new byte[] { data[1], data[2], data[3] });
            if (languageID.Contains("\0"))
            { 
            // we have problem with language id, the file previously saved without language or with error
                languageID = "";// We've better set it empty to avoid errors
            }

            string[] code = encoding.GetString(this.data, 4, data.Length - 4).Split(new char[] { '\0' });

            if (code.Length > 1)
            {
                contentDescriptor = code[0];
                contentDescriptor = contentDescriptor.Replace("\0","");// just in case
                lyricsText = code[1];
                lyricsText = lyricsText.Replace("\0", "");// just in case  
            }
        }
        /// <summary>
        /// Save frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            encoding = ID3v2EncodingWrapper.GetBestEncoding(contentDescriptor + lyricsText);

            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));

            //fix language if there's error(s)
            if (languageID.Length != 3 || languageID.Contains("\0"))
            {
                languageID = "\0\0\0";// this will reset language
            }
            buffer.AddRange(asci.GetBytes(languageID));

            buffer.AddRange(encoding.GetBytes(contentDescriptor));

            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            for (int i = 0; i < count; i++)
                buffer.Add(0);

            buffer.AddRange(encoding.GetBytes(lyricsText));

            data = buffer.ToArray();
        }
        /// <summary>
        /// Get the size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
                return 4 + encoding.GetByteCount(contentDescriptor + lyricsText) + count;
            }
        }
        /// <summary>
        /// Get if can save this frame. True if languageID.Length == 3 and lyricsText.Length > 0
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return lyricsText.Length > 0;
            }
        }
        /// <summary>
        /// UnsychronisedLyricsFrame.ToString()
        /// </summary>
        /// <returns>Content descriptor + [language id]</returns>
        public override string ToString()
        {
            return contentDescriptor + " [" + languageID + "]";
        }
    }
}
