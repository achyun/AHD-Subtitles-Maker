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
    /// The Play Counter Frame
    /// </summary>
    public class PlayCounterFrame : ID3TagFrame
    {
        /// <summary>
        /// The Play Counter Frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public PlayCounterFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Play counter", data, flags) { }

        private long counter = 0;

        /// <summary>
        /// Get or set the counter
        /// </summary>
        public long Counter
        { get { return counter; } set { counter = value; } }

        /// <summary>
        /// Load the data of this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            counter = 0;
            counter = BitConverter.ToInt64(Data,0);
        }
        /// <summary>
        /// Save the data for this frame
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();

            buffer.AddRange(BitConverter.GetBytes(counter));

            this.data = buffer.ToArray();
        }
        /// <summary>
        /// Get the size of this frame (calculated)
        /// </summary>
        public override int Size
        {
            get
            {
                return BitConverter.GetBytes(counter).Length;
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
        /// PlayCounterFrame.ToString()
        /// </summary>
        /// <returns>Play counter [counter value]</returns>
        public override string ToString()
        {
            return "Play counter  [" + this.counter.ToString() + "]";
        }
    }
}
