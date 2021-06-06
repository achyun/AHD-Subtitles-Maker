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
namespace AHD.ID3
{
    /// <summary>
    /// The ID3v2 frame attribute.
    /// </summary>
    public class ID3TagFrameAttribute : Attribute
    {
        /// <summary>
        /// The ID3v2 frame attribute.
        /// </summary>
        /// <param name="frameName">The frame name</param>
        /// <param name="frameType">The frame type</param>
        /// <param name="frameId">The frame id</param>
        public ID3TagFrameAttribute(string frameName, string frameType, string frameId)
        {
            this.frameName = frameName;
            this.frameType = frameType;
            this.frameId = frameId;
        }
        string frameName, frameType, frameId = "";
        /// <summary>
        /// Get the frame name
        /// </summary>
        public string Name { get { return frameName; } }   
        /// <summary>
        /// Get the frame id
        /// </summary>
        public string ID { get { return frameId; } }
        /// <summary>
        /// Get the frame type
        /// </summary>
        public string Type { get { return frameType; } }
    }
}
