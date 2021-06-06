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
using System.Collections.Generic;
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    /// <summary>
    /// Event timing codes frame
    /// </summary>
    public class EventTimingCodesFrame : ID3TagFrame
    {
        /// <summary>
        /// Event timing codes frame
        /// </summary>
        /// <param name="id">The frame id</param>
        /// <param name="name">The frame name</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        public EventTimingCodesFrame(string id, string name, byte[] data, int flags)
            : base(id, name, "Event timing codes", data, flags) { }

        private TimeStampFormat timeStamp = TimeStampFormat.AbsoluteMilliseconds;
        private List<EventTimingItem> items = new List<EventTimingItem>();

        /// <summary>
        /// The items list
        /// </summary>
        public List<EventTimingItem> Items
        { get { return items; } set { items = value; } }
        /// <summary>
        /// The time stamp format to use in save
        /// </summary>
        public TimeStampFormat TimeStamp
        { get { return timeStamp; } set { timeStamp = value; } }

        /// <summary>
        /// Load data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Load(ID3Version tagVersion)
        {
            items = new List<EventTimingItem>();
            timeStamp = (TimeStampFormat)data[0];
            for (int i = 1; i < Data.Length - 1; i++)
            {
                byte type = data[i];
                i++;
                int time = SynchsafeConvertor.FromBytes(new byte[] { data[i], data[i + 1], data[i + 2], data[i + 3] });
                i += 3;
                items.Add(new EventTimingItem(time, type));
            }
        }
        /// <summary>
        /// Save data
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public override void Save(ID3Version tagVersion)
        {
            List<byte> buffer = new List<byte>();
            //timeStamp
            buffer.Add((byte)timeStamp);
            //items
            foreach (EventTimingItem item in items)
            {
                buffer.Add(item.EventType);
                //time
                buffer.AddRange(SynchsafeConvertor.ToInt32Bytes(item.Time));
            }
            //save
            this.data = buffer.ToArray();
        }

        /// <summary>
        /// Get the size calculated
        /// </summary>
        public override int Size
        {
            get
            {
                return 1 + (items.Count * 5);
            }
        }
        /// <summary>
        /// Get if can save the frame
        /// </summary>
        public override bool CanSave
        {
            get
            {
                return items != null ? (items.Count > 0) : false;
            }
        }
    }
    public class EventTimingItemComparer : IComparer<EventTimingItem>
    {
        public int Compare(EventTimingItem x, EventTimingItem y)
        {
            return x.Time - y.Time;
        }
    }
}
