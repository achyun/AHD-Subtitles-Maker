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
    /// The progress event arg
    /// </summary>
    public class ProgressArg : EventArgs
    {
        /// <summary>
        /// The progress event arg
        /// </summary>
        /// <param name="status">The status</param>
        /// <param name="progress">The complete precent</param>
        public ProgressArg(string status, int progress)
        {
            this.status = status;
            this.progress = progress;
        }
        private string status = "";
        private int progress = 0;
        /// <summary>
        /// Get the status
        /// </summary>
        public string Status { get { return status; } }
        /// <summary>
        /// Get the complete precent value
        /// </summary>
        public int Progress { get { return progress; } }
    }
}
