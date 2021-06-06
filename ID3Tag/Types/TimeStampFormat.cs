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
    /// The time stamp format
    /// </summary>
    public enum TimeStampFormat : byte
    {
        /// <summary>
        /// Absolute time, 32 bit sized, using MPEG [MPEG] frames as unit
        /// </summary>
        AbsoluteFrames = 1,
        /// <summary>
        /// Absolute time, 32 bit sized, using milliseconds as unit
        /// </summary>
        AbsoluteMilliseconds = 2
    }
}
