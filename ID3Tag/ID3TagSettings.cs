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
    /// The class of settings. This class include static fields which should loaded at application start, use 
    /// the feilds when using ID3v2 class to keep the settings during application run.
    /// </summary>
    public class ID3TagSettings
    {
        /// <summary>
        /// The ID3v2 version (major, e.g. 2.x)
        /// </summary>
        public static int ID3V2Version = 3;
        /// <summary>
        /// Use the unsynchronisation when saving.
        /// </summary>
        public static bool UseUnsynchronisation = false;
        /// <summary>
        /// If padding is presented at load progress, keep it at save.
        /// </summary>
        public static bool KeepPadding = true;
        /// <summary>
        /// If extended header is presented at load, drop it at save.
        /// </summary>
        public static bool DropExtendedHeader = false;
        /// <summary>
        /// Write footer for version 4.
        /// </summary>
        public static bool WriteFooter = false;
    }
}
