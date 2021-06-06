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
using System.Collections.Generic;
namespace AHD.ID3
{
    /// <summary>
    /// Used to convert bytes and integers to Synchsafe format.
    /// </summary>
    public sealed class SynchsafeConvertor
    {
        /// <summary>
        /// Convert integer to "Synchsafe" integer
        /// </summary>
        /// <param name="val">A Int32 value to convert</param>
        /// <returns>Int32 as "Synchsafe" integer</returns>
        public static int ToSynchsafe(int val)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)((val & 0x0FE00000) >> 21));
            buffer.Add((byte)((val & 0x001FC000) >> 14));
            buffer.Add((byte)((val & 0x00003F80) >> 07));
            buffer.Add((byte)((val & 0x0000007F)));
            return ((buffer[0] << 24) | (buffer[1] << 16) | (buffer[2] << 8) | buffer[3]);
        }
        /// <summary>
        /// Convert integer to "Synchsafe" integer (4 bytes buffer, MSB first)
        /// </summary>
        /// <param name="val">A 32Int value to convert</param>
        /// <returns>32Int as "Synchsafe" integer (4 bytes buffer, MSB first)</returns>
        public static byte[] ToSynchsafeBytes(int val)
        {
            List<byte> buffer = new List<byte>();
            buffer.Add((byte)((val & 0x0FE00000) >> 21));
            buffer.Add((byte)((val & 0x001FC000) >> 14));
            buffer.Add((byte)((val & 0x00003F80) >> 07));
            buffer.Add((byte)((val & 0x0000007F)));
            return buffer.ToArray();
        }
        /// <summary>
        /// Convert "Synchsafe" integer bytes to normal integer
        /// </summary>
        /// <param name="bytes">A bytes array represents "Synchsafe" integer bytes (should be 4 bytes, MSB first)</param>
        /// <returns>Int32 normal integer</returns>
        public static int FromSynchsafe(byte[] bytes)
        {
            return ((bytes[0] & 0x7F) << 21) |
                   ((bytes[1] & 0x7F) << 14) |
                   ((bytes[2] & 0x7F) << 7) |
                    (bytes[3] & 0x7F);
        }
        /// <summary>
        /// Convert normal integer bytes to normal integer
        /// </summary>
        /// <param name="bytes">A bytes array represents Int32 bytes (should be 2, 3 or 4 bytes, MSB first)</param>
        /// <returns>Int32 normal integer</returns>
        public static int FromBytes(byte[] bytes)
        {
            switch (bytes.Length)
            {
                case 2: return (bytes[0] << 8) | bytes[1];
                case 3: return (bytes[0] << 16) | (bytes[1] << 8) | bytes[2];
                case 4: return (bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3];
            }
            return 0;
        }
        /// <summary>
        /// Convert normal integer to bytes array
        /// </summary>
        /// <param name="val">Int32 normal integer</param>
        /// <returns>byte[] normal integer bytes (4 bytes, MSB first)</returns>
        public static byte[] ToInt32Bytes(int val)
        {
            return ToInt32Bytes(val, 4);
        }
        /// <summary>
        /// Convert normal integer to bytes array
        /// </summary>
        /// <param name="val">Int32 normal integer</param>
        /// <param name="size">How many bytes ? 2, 3 or 4</param>
        /// <returns>byte[] normal integer bytes ('size' bytes, MSB first)</returns>
        public static byte[] ToInt32Bytes(int val, int size)
        {
            List<byte> buffer = new List<byte>();
            switch (size)
            {
                case 4: 
                    buffer.Add((byte)((val & 0xFF000000) >> 24));
                    buffer.Add((byte)((val & 0x00FF0000) >> 16));
                    buffer.Add((byte)((val & 0x0000FF00) >> 8));
                    buffer.Add((byte)((val & 0x000000FF)));
                    break;
                case 3:
                    buffer.Add((byte)((val & 0x00FF0000) >> 16));
                    buffer.Add((byte)((val & 0x0000FF00) >> 8));
                    buffer.Add((byte)((val & 0x000000FF)));
                    break;
                case 2:
                    buffer.Add((byte)((val & 0x0000FF00) >> 8));
                    buffer.Add((byte)((val & 0x000000FF)));
                    break;
            }
        
            return buffer.ToArray();
        }
    }
}
