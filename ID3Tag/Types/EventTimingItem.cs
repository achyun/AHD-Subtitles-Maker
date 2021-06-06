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
namespace AHD.ID3.Types
{
    /// <summary>
    /// The event timing item used in event timing codes Frame
    /// </summary>
    public class EventTimingItem
    {
        /// <summary>
        /// The event timing item used in event timing codes Frame
        /// </summary>
        /// <param name="time">The time if this item</param>
        /// <param name="eventType">The type of the event</param>
        public EventTimingItem(int time, byte eventType)
        {
            this.time = time;
            this.eventType = eventType;
        }

        private byte eventType;
        private int time;

        /// <summary>
        /// The event type
        /// </summary>
        public byte EventType
        { get { return eventType; } set { eventType = value; } }
        /// <summary>
        /// The event time
        /// </summary>
        public int Time
        { get { return time; } set { time = value; } }

        /// <summary>
        /// EventTimingItem.ToString()
        /// </summary>
        /// <returns>Time in seconds</returns>
        public override string ToString()
        {
            return ((double)Time / 1000).ToString("F3");
        }

        /// <summary>
        /// Get an empty item
        /// </summary>
        public static EventTimingItem Empty
        { get { return new EventTimingItem(0, 0); } }
    }
}
