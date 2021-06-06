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
using AHD.ID3.Types;
namespace AHD.ID3.Frames
{
    [ID3TagFrameAttribute("Unknown", "Unknown or not supported", "")]
    class UnknownFrame : ID3TagFrame
    {
        public UnknownFrame(byte[] data, int flags)
            : base(data, flags) { }
        public UnknownFrame(string id, string name, string type, byte[] data, int flags)
            : base(id, name, type, data, flags) { }

        public override void Load(ID3Version tagVersion)
        {
        }
        public override void Save(ID3Version tagVersion)
        {
        }
        public override bool CanSave
        {
            get
            {
                return data.Length > 0;
            }
        }
    }
}
