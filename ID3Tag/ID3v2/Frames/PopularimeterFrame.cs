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
    /// The Popularimeter Frame
    /// </summary>
    public class PopularimeterFrame : ID3TagFrame
    {
        /// <summary>
        /// The Popularimeter Frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public PopularimeterFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Popularimeter", data, flags) { }
      
        private string email = "";
        private byte rating = 0;
        private int counter = 0;

        /// <summary>
        /// Get or set the email
        /// </summary>
        public string Email
        { get { return email; } set { email = value; } }
        /// <summary>
        /// Get or set the rating. Values 0 - 255
        /// </summary>
        public byte Rating
        { get { return rating; } set { rating = value; } }
        /// <summary>
        /// Get or set the counter
        /// </summary>
        public int Counter
        { get { return counter; } set { counter = value; } }

        /// <summary>
        /// Load the data of this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            string[] Ltext = ID3v2EncodingWrapper.ISO_8859_1.GetString(Data, 0, Data.Length).Split(new char[] { '\0' });
            email = Ltext[0];
            int index = email.Length + 1;
            rating = Data[index]; index++;
            if ((Data.Length - index) == 4)//sometimes this frame doesn't save counter at all ...
                counter = SynchsafeConvertor.FromBytes(new byte[] 
                { Data[index], Data[index+1],
                  Data[index+2], Data[index+3] });
            else
                counter = 0;

            email = email.Replace("\0", "");
        }
        /// <summary>
        /// Save the data for this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //email
            buffer.AddRange(ID3v2EncodingWrapper.ISO_8859_1.GetBytes(email));
            buffer.Add(0);
            //rating
            buffer.Add(rating);
            //counter
            if (counter > 0)
                buffer.AddRange(SynchsafeConvertor.ToInt32Bytes(counter));
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
                return ID3v2EncodingWrapper.ISO_8859_1.GetByteCount(email) + 2 + (counter == 0 ? 0 : 4);
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
        /// <summary>
        /// PopularimeterFrame.ToString()
        /// </summary>
        /// <returns>email [rating=x, counter=x]</returns>
        public override string ToString()
        {
            return email + " [rating=" + rating.ToString() +
            ", counter=" + counter + "]";
        }
    }
}
