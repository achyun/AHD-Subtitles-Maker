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
    /// The Involved People Item for involved people list frame
    /// </summary>
    public struct InvolvedPeopleItem
    {
        /// <summary>
        /// The Involved People Item for involved people list frame
        /// </summary>
        /// <param name="involvement">The involvement</param>
        /// <param name="involvee">The involvee</param>
        public InvolvedPeopleItem(string involvement, string involvee)
        {
            this.involvement = involvement;
            this.involvee = involvee;
        }

        private string involvement;
        private string involvee;

        /// <summary>
        /// Get or set the involvement
        /// </summary>
        public string Involvement
        { get { return involvement; } set { involvement = value; } }
        /// <summary>
        /// Get or set the involvee
        /// </summary>
        public string Involvee
        { get { return involvee; } set { involvee = value; } }

        /// <summary>
        /// Empty involved people item
        /// </summary>
        public InvolvedPeopleItem Empty { get { return new InvolvedPeopleItem("", ""); } }
    }
}
