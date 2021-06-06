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
using System.Text;

namespace AHD.ID3.Types
{
    /// <summary>
    /// A class for ID3 Tag encodings
    /// </summary>
    public class ID3v2EncodingWrapper
    {
        /*
$00   ISO-8859-1 [ISO-8859-1]. Terminated with $00. cp=1252
$01   UTF-16 [UTF-16] encoded Unicode [UNICODE] with BOM. All
      strings in the same frame SHALL have the same byteorder.
      Terminated with $00 00. cp= 1200
$02   UTF-16BE [UTF-16] encoded Unicode [UNICODE] without BOM.
      Terminated with $00 00. cp= 1201
$03   UTF-8 [UTF-8] encoded Unicode [UNICODE]. Terminated with $00. cp=65001
         */
        static int ISO_8859_1_CODEPAGE = 1252;
        static int UTF_16_WithBOM_CODEPAGE = 1200;
        static int UTF_16BE_WithoutBOM_CODEPAGE = 1201;
        static int UTF_8_CODEPAGE = 65001;
        /// <summary>
        /// Get encoding from ID3v2 encoding index
        /// </summary>
        /// <param name="index">The ID3v2 encoding index</param>
        /// <returns>Encoding</returns>
        public static Encoding FromByte(byte index)
        {
            switch (index)
            {
                default:
                case 0x00: return ISO_8859_1;
                case 0x01: return UTF_16_WithBOM;
                case 0x02: return UTF_16BE_WithoutBOM;
                case 0x03: return UTF_8;
            }
        }
        /// <summary>
        /// Convert given encoding to ID3v2 encoding index
        /// </summary>
        /// <param name="encoding">The encoding to convert</param>
        /// <returns>ID3v2 encoding index</returns>
        public static byte ToByte(Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                default:
                case 1252: return 0x00; //ISO-8859-1 [ISO-8859-1].
                case 1200: return 0x01; //UTF-16 [UTF-16] encoded Unicode [UNICODE] with BOM.
                case 1201: return 0x02; //UTF-16BE [UTF-16] encoded Unicode [UNICODE] without BOM.
                case 65001: return 0x03;//UTF-8 [UTF-8] encoded Unicode [UNICODE].
            }
        }
        /// <summary>
        /// Test and try to detect the best encoding for a text
        /// </summary>
        /// <param name="text">The text to test</param>
        /// <returns>The best encoding</returns>
        public static Encoding GetBestEncoding(string text)
        {
            foreach (char c in text)
            {
                if ((int)c > 127)
                {
                    //TODO: indecate other encodings
                    return UTF_16_WithBOM;
                }
            }
            return ISO_8859_1;
        }
        /// <summary>
        /// Check if given encoding is one of ID3 Tag version 2 encodings
        /// </summary>
        /// <param name="encoding">The encoding to check</param>
        /// <returns>True if given encoding is one of ID3 Tag version 2 encodings, otherwise false</returns>
        public static bool IsID3Encoding(Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                case 1252:
                case 1200:
                case 1201:
                case 65001:
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Get the char byte count for encoding
        /// </summary>
        /// <param name="encoding">The encoding</param>
        /// <returns>The char count (1 or 2)</returns>
        public static int GetTerminationBytesCount(Encoding encoding)
        {
            switch (encoding.CodePage)
            {
                default:
                case 1252: return 1; //ISO-8859-1 [ISO-8859-1].
                case 1200: return 2; //UTF-16 [UTF-16] encoded Unicode [UNICODE] with BOM.
                case 1201: return 2; //UTF-16BE [UTF-16] encoded Unicode [UNICODE] without BOM.
                case 65001: return 1;//UTF-8 [UTF-8] encoded Unicode [UNICODE].
            }
        }

        /// <summary>
        /// ISO-8859-1 [ISO-8859-1], Codepage = 1252
        /// </summary>
        public static Encoding ISO_8859_1
        { get { return Encoding.GetEncoding(ISO_8859_1_CODEPAGE); } }
        /// <summary>
        /// UTF-16 [UTF-16] encoded Unicode [UNICODE] with BOM, Codepage = 1200
        /// </summary>
        public static Encoding UTF_16_WithBOM
        { get { return Encoding.GetEncoding(UTF_16_WithBOM_CODEPAGE); } }
        /// <summary>
        /// UTF-16BE [UTF-16] encoded Unicode [UNICODE] without BOM, Codepage = 1201 
        /// </summary>
        public static Encoding UTF_16BE_WithoutBOM
        { get { return Encoding.GetEncoding(UTF_16BE_WithoutBOM_CODEPAGE); } }
        /// <summary>
        /// UTF-8 [UTF-8] encoded Unicode [UNICODE], Codepage = 65001 
        /// </summary>
        public static Encoding UTF_8
        { get { return Encoding.GetEncoding(UTF_8_CODEPAGE); } }
    }
}
