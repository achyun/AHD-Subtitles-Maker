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
    /// The Commercial Frame
    /// </summary>
    public class CommercialFrame : ID3TagFrame
    {
        /// <summary>
        /// The Commercial Frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public CommercialFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Commercial", data, flags) { }

        private Encoding encoding = ID3v2EncodingWrapper.ISO_8859_1;
        private string price = "";
        private string validUntil = "";
        private string contactURL = "";
        private byte receivedAs = 0;
        private string nameOfSeller = "";
        private string description = "";
        private string pictureMIMEtype = "";
        private byte[] sellerLogoData;
        private string currency = "usd";

        /// <summary>
        /// Get or set the price
        /// </summary>
        public string Price
        { get { return price; } set { price = value; } }
        /// <summary>
        /// Get or set the currency
        /// </summary>
        public string Currency
        {
            get
            {
                foreach (string cur in ID3FrameConsts.Currency)
                {
                    if (cur.Substring(cur.Length - 5, 3).ToLower() == currency.ToLower())
                        return cur;
                }
                return currency;
            }
            set
            {
                currency = value.Substring(value.Length - 5, 3).ToUpper();
            }
        }
        /// <summary>
        /// Get or set the valid until
        /// </summary>
        public string ValidUntil
        { get { return validUntil; } set { validUntil = value; } }
        /// <summary>
        /// Get or set the contact URL
        /// </summary>
        public string ContactURL
        { get { return contactURL; } set { contactURL = value; } }
        /// <summary>
        /// Get or set the name of the seller
        /// </summary>
        public string NameOfSeller
        { get { return nameOfSeller; } set { nameOfSeller = value; } }
        /// <summary>
        /// Get or set the description
        /// </summary>
        public string Description
        { get { return description; } set { description = value; } }
        /// <summary>
        /// Get or set the picture mime type
        /// </summary>
        public string PictureMIMEType
        { get { return pictureMIMEtype; } set { pictureMIMEtype = value; } }
        /// <summary>
        /// Get or set the seller logo data
        /// </summary>
        public byte[] SellerLogoData
        { get { return sellerLogoData; } set { sellerLogoData = value; } }
        /// <summary>
        /// Get or set the received as
        /// </summary>
        public string ReceivedAs
        {
            get
            {
                return ID3FrameConsts.CommercialReceivedAs[receivedAs];
            }
            set
            {
                for (byte i = 0; i < ID3FrameConsts.CommercialReceivedAs.Length; i++)
                {
                    if (ID3FrameConsts.CommercialReceivedAs[i] == value)
                    { receivedAs = i; break; }
                }
            }
        }
        /// <summary>
        /// Get or set the encoding
        /// </summary>
        public Encoding Encoding
        { get { return encoding; } set { encoding = value; } }

        /// <summary>
        /// Load the data of this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            encoding = ID3v2EncodingWrapper.FromByte(Data[0]);
            string code = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, 1, Data.Length - 1);
            int index = 1;
            string[] list = code.Split(new char[] { '\0' });
            if (list[0].Length >= 3)
            {
                currency = list[0].Substring(0, 3);
                price = list[0].Substring(3, list[0].Length - 3);
            }
            index += ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(list[0]);
            validUntil = code.Substring(index, 8);
            index += 8;
            list = code.Substring(index).Split(new char[] { '\0' });
            contactURL = list[0];
            index += contactURL.Length + 2;
            receivedAs = Data[index]; index++;

            code = encoding.GetString(Data, index, Data.Length - index);
            list = code.Split(new char[] { '\0' });
            nameOfSeller = list[0];
            index += encoding.GetByteCount(nameOfSeller + "\0");

            description = list[1];
            index += encoding.GetByteCount(description + "\0");

            code = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, index, Data.Length - index);
            list = code.Split(new char[] { '\0' });
            pictureMIMEtype = list[0];
            index += ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(pictureMIMEtype + "\0");

            if (Data.Length - index > 0)    // just in case the logo isn't exist
            {
                sellerLogoData = new byte[Data.Length - index];
                for (int i = index; i < Data.Length - index; i++)
                {
                    sellerLogoData[i - index] = Data[i];
                }
            }
        }
        /// <summary>
        /// Save the data for this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //encoding
            encoding = ID3v2EncodingWrapper.GetBestEncoding(nameOfSeller + description);
            int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
            buffer.Add(ID3v2EncodingWrapper.ToByte(encoding));
            //price
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(currency + price));
            buffer.Add(0);
            //valid until
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(validUntil));
            //url
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(contactURL));
            buffer.Add(0);
            //Received as
            buffer.Add(receivedAs);
            //Name Of Seller
            buffer.AddRange(encoding.GetBytes(nameOfSeller));
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            //Description
            buffer.AddRange(encoding.GetBytes(description));
            for (int i = 0; i < count; i++)
                buffer.Add(0);
            //Picture MIME type 
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(pictureMIMEtype));
            buffer.Add(0);
            //picture data
            if (sellerLogoData != null)
                if (sellerLogoData.Length > 0)
                    buffer.AddRange(sellerLogoData);
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
                encoding = ID3v2EncodingWrapper.GetBestEncoding(nameOfSeller + description);
                int count = ID3v2EncodingWrapper.GetTerminationBytesCount(encoding);
                return 1 + ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(currency + price) + 1 +
ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(validUntil) + ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(contactURL) + 1 +
1 + encoding.GetByteCount(nameOfSeller) + count + encoding.GetByteCount(description) +
count + ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(pictureMIMEtype) + 1 +
+((sellerLogoData != null) ? sellerLogoData.Length : 0);
            }
        }
        /// <summary>
        /// Get if this frame can be saved to the tag
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return true;
            }
        }
    }
}
