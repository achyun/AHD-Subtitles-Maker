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
namespace AHD.MP3
{
    /// <summary>
    /// MPEG Audio version ID
    /// </summary>
    public enum MPEGAudioVersion : int
    {
        /// <summary>
        /// MPEG Version 2.5 (unofficial)
        /// </summary>
        Version2_5 = 0,
        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 1,
        /// <summary>
        /// MPEG Version 2 (ISO/IEC 13818-3)
        /// </summary>
        Version2 = 2,
        /// <summary>
        /// MPEG Version 1 (ISO/IEC 11172-3)
        /// </summary>
        Version1 = 3
    }
    /// <summary>
    /// Layer description
    /// </summary>
    public enum LayerDescription : int
    {
        /// <summary>
        /// Reserved
        /// </summary>
        Reserved = 0,
        /// <summary>
        /// Layer III
        /// </summary>
        LayerIII = 1,
        /// <summary>
        /// Layer II
        /// </summary>
        LayerII = 2,
        /// <summary>
        /// Layer I
        /// </summary>
        LayerI = 3
    }
    /// <summary>
    /// The channel mode
    /// </summary>
    public enum ChannelMode : int
    {
        /// <summary>
        /// Stereo
        /// </summary>
        Stereo = 0,
        /// <summary>
        /// Stereo
        /// </summary>
        JointStereo = 1,
        /// <summary>
        /// 2 mono channels
        /// </summary>
        DualChannel = 2,
        /// <summary>
        /// Mono
        /// </summary>
        SingleChannel = 3
    }
}
