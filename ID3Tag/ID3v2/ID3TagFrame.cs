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
using AHD.ID3.Types;
namespace AHD.ID3
{
    /// <summary>
    /// Represents ID3v2 frame.
    /// </summary>
    public abstract class ID3TagFrame
    {
        /// <summary>
        /// Initialize ID3v2 frame using the information from the attributes.
        /// </summary>
        /// <param name="data">The data without the header</param>
        /// <param name="flags">The frame flags. Used only for version 3 and 4</param>
        public ID3TagFrame(byte[] data, int flags)
        {
            LoadAttributes();
            this.data = data;
            this.flags = flags;
        }
        /// <summary>
        /// Initialize ID3v2 frame.
        /// </summary>
        /// <param name="name">The name of the frame</param>
        /// <param name="id">The id of this frame</param>
        /// <param name="type">The type of this frame</param>
        /// <param name="data">The data without the header</param>
        /// <param name="flags">The flags</param>
        public ID3TagFrame(string id, string name, string type, byte[] data, int flags)
        {
            this.name = name;
            this.id = id;
            this.type = type;
            this.data = data;
            this.flags = flags;
        }

        /// <summary>
        /// The frame name
        /// </summary>
        protected string name = "Unknown";
        /// <summary>
        /// The frame id
        /// </summary>
        protected string id = "";
        /// <summary>
        /// The frame type
        /// </summary>
        protected string type = "";
        /// <summary>
        /// The frame flags
        /// </summary>
        protected int flags = 0;
        /// <summary>
        /// The frame data
        /// </summary>
        protected byte[] data;
        /// <summary>
        /// Get a value indecate if this frame can be saved (due to values set by user)
        /// </summary>
        protected bool canSave = false;

        private void LoadAttributes()
        {
            foreach (Attribute attr in Attribute.GetCustomAttributes(this.GetType()))
            {
                if (attr.GetType() == typeof(ID3TagFrameAttribute))
                {
                    this.name = ((ID3TagFrameAttribute)attr).Name;
                    this.id = ((ID3TagFrameAttribute)attr).ID;
                    this.type = ((ID3TagFrameAttribute)attr).Type;
                    break;
                }
            }
        }
        /// <summary>
        /// Load the data and apply information
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public abstract void Load(ID3Version tagVersion);
        /// <summary>
        /// Save and apply the frame to the data buffer
        /// </summary>
        /// <param name="tagVersion">The tag version</param>
        public abstract void Save(ID3Version tagVersion);

        /// <summary>
        /// Get the name of this frame
        /// </summary>
        public virtual string Name
        { get { return name; } }
        /// <summary>
        /// Get or set the id of this frame
        /// </summary>
        public virtual string ID
        { get { return id; } set { id = value; } }
        /// <summary>
        /// Get the size of this frame (without header)
        /// </summary>
        public virtual int Size
        { get { return (data != null) ? data.Length : 0; } }
        /// <summary>
        /// Get the data of this frame (without header)
        /// </summary>
        public virtual byte[] Data
        { get { return data; } }
        /// <summary>
        /// Get the frame type
        /// </summary>
        public virtual string Type
        { get { return type; } }
        /// <summary>
        /// Get the flags of this frame
        /// </summary>
        public virtual int Flags
        { get { return flags; } }
        /// <summary>
        /// Get a value indecate if this frame can be saved (due to values set by user)
        /// </summary>
        public virtual bool CanSave
        { get { return canSave; } }

        /// <summary>
        /// Frame.ToString()
        /// </summary>
        /// <returns>Frame name</returns>
        public override string ToString()
        {
            return name;
        }
    }
}
