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
using System.IO;
using System.Text;
using AHD.ID3;
namespace AHD.MP3
{
    //TODO: complete this class (mp3info class)
    /// <summary>
    /// This class is for reading mp3 file to get info from it like frame info and frames count. 
    /// </summary>
    public class MP3Info
    {
        /// <summary>
        /// This class is for reading mp3 file to get info from it like frame info and frames count.
        /// </summary>
        /// <param name="fileName">The complete file path. File must be existed.</param>
        public MP3Info(string fileName)
        {
            Load(fileName);
        }
        private readonly int[][] bitrateTable =
{
        new int []{00, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, -1},//V1,L1
        new int []{00, 32, 48, 56, 64 , 80 , 96 , 112, 128, 160, 192, 224, 256, 320, 384, -1},//V1,L2
        new int []{00, 32, 40, 48, 56 , 64 , 80 , 96 , 112, 128, 160, 192, 224, 256, 320, -1},//V1,L3
        new int []{00, 32, 48, 56, 64 , 80 , 96 , 112, 128, 144, 160, 176, 192, 224, 256, -1},//V2,L1
        new int []{00, 8 , 16, 24, 32 , 40 , 48 , 56 , 64 , 80 , 96 , 112, 128, 144, 160, -1} //V2, L2 & L3
};
        private readonly int[][] SamplingRateFrequencyTable =
{
        new int []{44100, 48000, 32000, -1},//MPEG1
        new int []{22050, 24000, 16000, -1},//MPEG2
        new int []{11025, 12000, 8000 , -1},//MPEG2.5
};
        private int framesCount = 0;
        private MPEGAudioVersion _MPEGAudioVersion = MPEGAudioVersion.Reserved;
        private LayerDescription _LayerDescription = LayerDescription.Reserved;
        private bool _ProtectedByCRC = false;
        private int bitrate = 0;
        private int _SamplingRateFrequency = 0;
        private bool _Padding = false;
        private bool _Private = false;
        private ChannelMode _ChannelMode = ChannelMode.Stereo;
        private bool isVBR = false;
        private void Load(string fileName)
        {
            // get the stream
            Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // make sure
            if (stream.Length < 10)
            {
                stream.Close();
                stream.Dispose();
                return;
            }
            // read first 10 bytes to see if we have id3 tag here.
            // read header and check
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] header = new byte[10];
            stream.Read(header, 0, 10);
            // ID3 file identifier
            if (encoding.GetString(new byte[] { header[0], header[1], header[2], }) == "ID3")
            {
                // Size 11675
                int tagSize = SynchsafeConvertor.FromSynchsafe(new byte[] { header[6], header[7], header[8], header[9], });
                // skip
                stream.Position += tagSize;
            }
            else// no id3v2 exist, seek to begining
            {
                stream.Position = 0;
            }
            // reset
            framesCount = 0;
            // Load mp3 data
            int b = 0;
            while (( b = stream.ReadByte()) >= 0)
            {
                if (stream.Length - (stream.Position + 1) < 4)
                    break;
                // look for a sync byte
                if (b == 0xFF)
                {
                    stream.Position--;
                    // usally the mp3 header is 4 byte, so read 4 bytes as buffer
                    byte[] mp3Buffer = new byte[4];
                    stream.Read(mp3Buffer, 0, 4);

                    // just make sure
                    if ((mp3Buffer[1] & 0xE0) == 0xE0 )
                    {
                        if (mp3Buffer[1] == 0xFF && mp3Buffer[2] == 0xFF && mp3Buffer[3] == 0xFF) continue;
                        // this is it !
                        framesCount++;
                        // values of current frame, temp
                        MPEGAudioVersion C_MPEGAudioVersion = MPEGAudioVersion.Reserved;
                        LayerDescription C_LayerDescription = LayerDescription.Reserved;
                        bool C_ProtectedByCRC = false;
                        int C_bitrate = 0;
                        int C_SamplingRateFrequency = 0;
                        bool C_Padding = false;
                        bool C_Private = false;
                        ChannelMode C_ChannelMode = ChannelMode.Stereo;

                        // get flags and info
                        // MPEG Audio Version
                        C_MPEGAudioVersion = (MPEGAudioVersion)((mp3Buffer[1] >> 3) & 0x3);
                        // Layer Description 
                        C_LayerDescription = (LayerDescription)((mp3Buffer[1] >> 1) & 0x3);
                        // Protected by CRC
                        C_ProtectedByCRC = (mp3Buffer[1] & 1) == 1;
                        if (C_ProtectedByCRC)// 16bit crc follows header, skip 'em
                            stream.Position += 2;
                        // bitrate
                        int bindex = ((mp3Buffer[2] >> 4) & 0xF);
                        if (bindex == 0)
                            C_bitrate = 0;
                        else if (bindex == 0xFF)
                            C_bitrate = -1;
                        else
                        {
                            switch (C_MPEGAudioVersion)
                            {
                                case MPEGAudioVersion.Version1:
                                    switch (C_LayerDescription)
                                    {
                                        case LayerDescription.LayerI: C_bitrate = bitrateTable[0][bindex]; break;
                                        case LayerDescription.LayerII: C_bitrate = bitrateTable[1][bindex]; break;
                                        case LayerDescription.LayerIII: C_bitrate = bitrateTable[2][bindex]; break;
                                    }
                                    break;
                                case MPEGAudioVersion.Version2:
                                case MPEGAudioVersion.Version2_5:
                                    switch (C_LayerDescription)
                                    {
                                        case LayerDescription.LayerI: C_bitrate = bitrateTable[3][bindex]; break;
                                        case LayerDescription.LayerII:
                                        case LayerDescription.LayerIII: C_bitrate = bitrateTable[4][bindex]; break;
                                    }
                                    break;
                            }
                        }
                        // Sampling rate frequency
                        int rateIndex = ((mp3Buffer[2] >> 2) & 0x3);
                        switch (C_MPEGAudioVersion)
                        {
                            case MPEGAudioVersion.Version1: C_SamplingRateFrequency = SamplingRateFrequencyTable[0][rateIndex]; break;
                            case MPEGAudioVersion.Version2: C_SamplingRateFrequency = SamplingRateFrequencyTable[1][rateIndex]; break;
                            case MPEGAudioVersion.Version2_5: C_SamplingRateFrequency = SamplingRateFrequencyTable[2][rateIndex]; break;
                        }
                        // Padding
                        C_Padding = (mp3Buffer[2] & 2) == 2;
                        // Private ??
                        C_Private = (mp3Buffer[2] & 1) == 1;
                        // Channel Mode
                        C_ChannelMode = (ChannelMode)((mp3Buffer[3] >> 6) & 0x3);
                        // now get the frame size to skip
                        int size = 0;
                        switch (C_MPEGAudioVersion)
                        {
                            case MPEGAudioVersion.Version1:
                                switch (C_LayerDescription)
                                {
                                    case LayerDescription.LayerI: size = (12 * C_bitrate / C_SamplingRateFrequency + (C_Padding ? 1 : 0)) * 4; break;
                                    case LayerDescription.LayerII:
                                    case LayerDescription.LayerIII: size = 144 * C_bitrate / C_SamplingRateFrequency + (C_Padding ? 1 : 0); break;
                                }
                                break;
                            case MPEGAudioVersion.Version2:
                            case MPEGAudioVersion.Version2_5:
                                switch (C_LayerDescription)
                                {
                                    case LayerDescription.LayerI: size = (12 * C_bitrate / C_SamplingRateFrequency + (C_Padding ? 1 : 0)) * 4; break;
                                    case LayerDescription.LayerII: size = 144 * C_bitrate / C_SamplingRateFrequency + (C_Padding ? 1 : 0); break;
                                    case LayerDescription.LayerIII: size = 72 * C_bitrate / C_SamplingRateFrequency + (C_Padding ? 1 : 0); break;
                                }
                                break;
                        }
                        if (size > 0)
                            stream.Position += size;
                        // get info from 1st frame only
                        if (framesCount == 1)
                        {
                            this._MPEGAudioVersion = C_MPEGAudioVersion;
                            this._LayerDescription = C_LayerDescription;
                            this._ProtectedByCRC = C_ProtectedByCRC;
                            this.bitrate = C_bitrate;
                            this._SamplingRateFrequency = C_SamplingRateFrequency;
                            this._Padding = C_Padding;
                            this._Private = C_Private;
                            this._ChannelMode = C_ChannelMode;

                            break;
                        }
                    }
                }
            }
            // done !!
            stream.Close();
            stream.Dispose();
        }

        // properties
        /// <summary>
        /// Get the frames count
        /// </summary>
        public int FramesCount
        { get { return framesCount; }  }
        /// <summary>
        /// Get the <see cref="MPEGAudioVersion"/>
        /// </summary>
        public MPEGAudioVersion MPEGAudioVersion
        { get { return _MPEGAudioVersion; } }
        /// <summary>
        /// Get the <see cref="MPEGAudioVersion"/>
        /// </summary>
        public LayerDescription LayerDescription
        { get { return _LayerDescription; } }
        /// <summary>
        /// Get if protected by CRC (16bit crc follows header) bit is set
        /// </summary>
        public bool ProtectedByCRC
        { get { return _ProtectedByCRC; } }
        /// <summary>
        /// Get the bitrate
        /// </summary>
        public int Bitrate
        { get { return bitrate; } }
        /// <summary>
        /// Get the Sampling Rate Frequency
        /// </summary>
        public int SamplingRateFrequency
        { get { return _SamplingRateFrequency; } }
        /// <summary>
        /// Get if the padding bit is set
        /// </summary>
        public bool Padding
        { get { return _Padding; } }
        /// <summary>
        /// Get if the private bit is set
        /// </summary>
        public bool Private
        { get { return _Private; } }
        /// <summary>
        /// Get the <see cref="ChannelMode"/>
        /// </summary>
        public ChannelMode ChannelMode
        { get { return _ChannelMode; } }
        /// <summary>
        /// Is VBR ?
        /// </summary>
        public bool IsVBR
        { get { return isVBR; } }
    }
}
