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
namespace AHD.ID3
{
    /// <summary>
    /// The result of read/write operation for ID3 Tag
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// The operation completed without problems.
        /// </summary>
        Success,
        /// <summary>
        /// The ID3 Tag not existed on this file on read operation.
        /// </summary>
        NoID3Exist,
        /// <summary>
        /// The operation failed.
        /// </summary>
        Failed,
        /// <summary>
        /// This version of ID3 Tag is not compatible with current version of this library.
        /// </summary>
        NotCompatibleVersion
    }
}
