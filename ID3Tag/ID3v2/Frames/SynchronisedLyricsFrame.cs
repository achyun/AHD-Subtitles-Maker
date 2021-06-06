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
    /// Synchronised lyrics frame
    /// </summary>
    public class SynchronisedLyricsFrame : ID3TagFrame
    {
        /// <summary>
        /// The synchronised lyrics frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public SynchronisedLyricsFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Synchronised lyrics frame", data, flags) { }

        private string languageID = "";
        private string contentDescriptor = "";
        private string contentType = "";
        private TimeStampFormat timeStamp = TimeStampFormat.AbsoluteMilliseconds;
        private List<SynchronisedLyricsItem> items = new List<SynchronisedLyricsItem>();
        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;

        /// <summary>
        /// Get or set the encoding used in save
        /// </summary>
        public virtual Encoding Encoding
        { get { return encoding; } set { encoding = value; } }
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
        /// Get or set the content type
        /// </summary>
        public string ContentType
        {
            get
            {
                return contentType;
            }
            set
            {
                contentType = value;
            }
        }
        /// <summary>
        /// Get or set the content descriptor
        /// </summary>
        public string ContentDescriptor
        { get { return contentDescriptor; } set { contentDescriptor = value; } }
        /// <summary>
        /// Get or set the time stamp format
        /// </summary>
        public TimeStampFormat TimeStampFormat
        { get { return timeStamp; } set { timeStamp = value; } }
        /// <summary>
        /// Get or set the items collection
        /// </summary>
        public List<SynchronisedLyricsItem> Items
        { get { return items; } set { items = value; } }

        private int GetItemsSize()
        {
            int size = 0;
            foreach (SynchronisedLyricsItem item in items)
            {
                size += encoding.GetByteCount(item.Text + "\0") + 4;
            }
            return size;
        }
        private byte GetType(string type)
        {
            for (byte i = 0; i < ID3FrameConsts.SynchronisedLyricsContentTypes.Length; i++)
            {
                if (ID3FrameConsts.SynchronisedLyricsContentTypes[i] == type)
                {
                    return i;
                }
            }
            return 0;
        }

        /// <summary>
        /// Save frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            int index = 1;
            string code = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, 1, Data.Length - 1);
            languageID = code.Substring(0, 3);
            index += 3;
            timeStamp = (TimeStampFormat)Data[index];
            if (timeStamp == 0)
                timeStamp = TimeStampFormat.AbsoluteMilliseconds;
            index += 1;
            contentType = ID3FrameConsts.SynchronisedLyricsContentTypes[Data[index]];
            index += 1;

            code = encoding.GetString(Data, index, Data.Length - index);
            string[] Ltext = code.Split(new char[] { '\0' });
            contentDescriptor = Ltext[0];
            index += encoding.GetByteCount(contentDescriptor + "\0");
            //items
            int TerminationBytesCount = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            items.Clear();
            for (int i = index; i < Data.Length - index; i++)
            {
                string text = "";
                if (TerminationBytesCount == 1)
                {
                    while (encoding.GetString(new byte[] { Data[i] }) != "\0")
                    {
                        text += encoding.GetString(new byte[] { Data[i] });
                        i++;
                    }
                }
                else if (TerminationBytesCount == 2)
                {
                    while (encoding.GetString(new byte[] { Data[i], Data[i + 1] }) != "\0")
                    {
                        text += encoding.GetString(new byte[] { Data[i], Data[i + 1] });
                        i += 2;
                    }
                }
                i += TerminationBytesCount;
                int time = (Data[i] << 24) | (Data[i + 1] << 16) | (Data[i + 2] << 8) | Data[i + 3];
                i += 3;
                items.Add(new SynchronisedLyricsItem(time, text));
            }
        }
        /// <summary>
        /// Load frame data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding, detect for the items
            string text = "";
            foreach (SynchronisedLyricsItem item in items)
            {
                text+=item.Text;
            }
            encoding = ID3v2EncodingWrapper.GetBestEncoding(text);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));

            //language
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(languageID.ToUpper()));
            //timeStamp
            buffer.Add((byte)timeStamp);
            //contentType
            buffer.Add(GetType(contentType));
            //contentDescriptor
            buffer.AddRange(encoding.GetBytes(contentDescriptor + "\0"));
            //items
            foreach (SynchronisedLyricsItem item in items)
            {
                buffer.AddRange(encoding.GetBytes(item.Text + "\0"));
                //size
                buffer.Add((byte)((item.Time & 0xFF000000) >> 24));
                buffer.Add((byte)((item.Time & 0x00FF0000) >> 16));
                buffer.Add((byte)((item.Time & 0x0000FF00) >> 08));
                buffer.Add((byte)((item.Time & 0x000000FF) >> 00));
            }
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
                return 6 + encoding.GetByteCount(contentDescriptor + "\0") + GetItemsSize();
            }
        }
        /// <summary>
        /// Get if this frame can be saved
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return languageID.Length == 3 && items.Count > 0;
            }
        }
        /// <summary>
        /// SynchronisedLyricsFrame.ToString()
        /// </summary>
        /// <returns>Content Descriptor [LanguageID]</returns>
        public override string ToString()
        {
            return contentDescriptor + " [" + LanguageID + "]";
        }
    }
    public class SynchronisedLyricsItemComparer : IComparer<SynchronisedLyricsItem>
    {
        public int Compare(SynchronisedLyricsItem x, SynchronisedLyricsItem y)
        {
            return x.Time - y.Time;
        }
    }
}
