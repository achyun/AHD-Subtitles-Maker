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
    /// The Synchronised Lyrics Item
    /// </summary>
    public class SynchronisedLyricsItem
    {
        /// <summary>
        /// Synchronised Lyrics Item
        /// </summary>
        /// <param name="time">The time</param>
        /// <param name="text">The text</param>
        public SynchronisedLyricsItem(int time, string text)
        {
            this.time = time;
            this.text = text;
        }

        private int time;
        private string text;

        /// <summary>
        /// Get or set the time
        /// </summary>
        public int Time
        { get { return time; } set { time = value; } }
        /// <summary>
        /// Get or set the text
        /// </summary>
        public string Text
        { get { return text; } set { text = value; } }

        /// <summary>
        /// SynchronisedLyricsItem.ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ((double)Time / 1000).ToString("F3");
        }

        /// <summary>
        /// Get an empty Synchronised Lyrics Item
        /// </summary>
        public static SynchronisedLyricsItem Empty
        { get { return new SynchronisedLyricsItem(0, ""); } }
    }
}
