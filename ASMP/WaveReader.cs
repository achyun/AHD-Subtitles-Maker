// This file is part of AHD Subtitles Maker
// A program that can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This library is free software; you can redistribute it and/or modify 
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 3 of the License, 
// or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public 
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public License 
// along with this library; if not, write to the Free Software Foundation, 
// Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.IO;
using System.Text;

namespace AHD.SM.ASMP
{
    public sealed class WaveReader
    {
        public static string FileName
        { get; private set; }
        public static Stream Stream
        { get; private set; }
        public static Stream BufferStream
        { get; private set; }
        public static long BufferLength
        { get; private set; }
        public static bool IsReady
        { get; private set; }
        public static int AudioFormat
        { get; private set; }
        public static int NumChannels
        { get; private set; }
        public static int SampleRate
        { get; private set; }
        public static int ByteRate
        { get; private set; }
        public static int BlockAlign
        { get; private set; }
        public static int BitsPerSample
        { get; private set; }
        public static int DataSize
        { get; private set; }
        public static long DataPointer
        { get; private set; }
        public static double Length { get; private set; }
        public static bool BufferPresented
        { get; private set; }
        public static int BufferNmChannel
        { get; private set; }
        public static int BufferMillisPerBlock
        { get; private set; }
        public static long BufferBytesPerBlock
        { get; private set; }
        public static string BufferFileName
        { get; private set; }
        public static bool IsGenerating { get; private set; }
        private static byte[] TheBuffer;
        private static long availableBufferSize;
        private static long chunkSize = 25600;
        private static bool abort_generating = false;
        public delegate void GenerateBuffferDelegate(string fileName, int millisBerBlock, bool closeAfterGenerate, bool deleteWaveFileAfterfinish);

        public static void AbortGenerateProcess()
        {
            abort_generating = true;
        }
        public static void LoadFile(string fileName)
        {
            Dispose();
            if (!File.Exists(fileName))
                return;
            Stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            if (!Stream.CanRead)
            {
                Console.WriteLine("ERROR: cannot read wave file !");
                Dispose();
                return;
            }
            if (Stream.Length < 50)
            {
                Console.WriteLine("ERROR: cannot read wave file, file is too small to be an wave file.");
                Dispose();
                return;
            }
            // Start reading !!
            // 1 Read RIFF
            byte[] readData = new byte[4];
            Stream.Read(readData, 0, 4);
            if (ASCIIEncoding.ASCII.GetString(readData) != "RIFF")
            {
                Console.WriteLine("ERROR: cannot read wave file, RIFF check faild.");
                Dispose();
                return;
            }
            // 2 Read ChunkSize
            readData = new byte[4];
            Stream.Read(readData, 0, 4);
            int ChunkSize = IntFromBytes(readData);

            // 3 Read Format
            readData = new byte[4];
            Stream.Read(readData, 0, 4);
            if (ASCIIEncoding.ASCII.GetString(readData) != "WAVE")
            {
                Console.WriteLine("ERROR: cannot read wave file, WAVE check faild.");
                Dispose();
                return;
            }

            // Read the rest of the chuncks.
            while (Stream.Position < Stream.Length)
            {
                // Read SubchunkID
                readData = new byte[4];
                Stream.Read(readData, 0, 4);
                string id = ASCIIEncoding.ASCII.GetString(readData);
                if (id == "fmt ")
                {
                    // This is it, the fmt chunk
                    // Read Subchunk1Size
                    readData = new byte[4];
                    Stream.Read(readData, 0, 4);
                    int Subchunk1Size = IntFromBytes(readData);

                    // Read AudioFormat
                    readData = new byte[2];
                    Stream.Read(readData, 0, 2);
                    AudioFormat = IntFromBytes(readData);
                    Subchunk1Size -= 2;
                    // Read NumChannels
                    readData = new byte[2];
                    Stream.Read(readData, 0, 2);
                    NumChannels = IntFromBytes(readData);
                    Subchunk1Size -= 2;
                    // Read SampleRate
                    readData = new byte[4];
                    Stream.Read(readData, 0, 4);
                    SampleRate = IntFromBytes(readData);
                    Subchunk1Size -= 4;
                    // Read ByteRate
                    readData = new byte[4];
                    Stream.Read(readData, 0, 4);
                    ByteRate = IntFromBytes(readData);
                    Subchunk1Size -= 4;
                    // Read BlockAlign
                    readData = new byte[2];
                    Stream.Read(readData, 0, 2);
                    BlockAlign = IntFromBytes(readData);
                    Subchunk1Size -= 2;
                    // Read BitsPerSample
                    readData = new byte[2];
                    Stream.Read(readData, 0, 2);
                    BitsPerSample = IntFromBytes(readData);
                    Subchunk1Size -= 2;

                    // Read dummy
                    readData = new byte[Subchunk1Size];
                    Stream.Read(readData, 0, Subchunk1Size);
                }
                else if (id == "data")
                {
                    // 13 Read Subchunk2Size
                    readData = new byte[4];
                    Stream.Read(readData, 0, 4);
                    DataSize = IntFromBytes(readData);
                    DataPointer = Stream.Position;
                    FileName = fileName;
                    IsReady = true;
                    Length = (double)DataSize / (double)ByteRate;

                    break;
                }
                else // Unkown
                {
                    // Read and skip this chunck
                    readData = new byte[4];
                    Stream.Read(readData, 0, 4);
                    int size = IntFromBytes(readData);

                    Stream.Seek(Stream.Position + size, SeekOrigin.Begin);
                }
            }
        }
        /// <summary>
        /// Generate wave buffer (WaveForm Pixel Data "WFPD")
        /// </summary>
        /// <param name="fileName">The complete path where to save the buffer</param>
        /// <param name="millisBerBlock">How many millisecond each block presents. Can be 1 - 1000, smaller values make heigher quality but take longer time.</param>
        /// <param name="closeAfterGenerate">Indicates if the reader stream should be closed after completation.</param>
        /// <param name="deleteWaveFileAfterfinish">Indicates if the wave file should be deleted after done (BE CAREFULL !!)</param>
        /// <param name="quality">The WFPD quality, can be 1-10, 1 is the best quality but take longer time.</param>
        public static void GenerateBuffer(string fileName, int millisBerBlock, bool closeAfterGenerate, bool deleteWaveFileAfterfinish, int quality)
        {
            if (IsGenerating)
                return;
            abort_generating = false;
            IsGenerating = true;
            ProgressStarted?.Invoke(null, new EventArgs());
            // Start the progress
            // 1 seek the stream we have
            Stream.Seek(DataPointer, SeekOrigin.Begin);
            // 2 greate the writter stream
            Stream ws = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            // 3 write the header
            // First 4 bytes should be WFPD (WaveForm Pixel Data)
            ws.Write(ASCIIEncoding.ASCII.GetBytes("WFPD"), 0, 4);
            // Second 2 bytes should be the number of channels
            ws.Write(BytesFromInt16((short)NumChannels), 0, 2);
            // Second 2 bytes should be the bits per sample
            ws.Write(BytesFromInt16((short)BitsPerSample), 0, 2);
            // Now the milli block value
            ws.Write(BytesFromInt32(millisBerBlock), 0, 4);

            // Now the actual data ...
            BufferMillisPerBlock = millisBerBlock;
            long bytesPerBlock = ((long)millisBerBlock * ByteRate) / 1000;
            double secondsPerBlock = (double)millisBerBlock / 1000;
            BufferBytesPerBlock = bytesPerBlock;
            int leftChannelPOS = 0;
            int leftChannelNEG = 0;
            int rightChannelPOS = 0;
            int rightChannelNEG = 0;
            int leftPOSAv = 0;
            int rightPOSAv = 0;
            int leftNEGAv = 0;
            int rightNEGAv = 0;
            long pointA = 0;
            long pointB = 0;
            int bytesBySample = BitsPerSample / 8;
            if (quality <= 0 || quality > 10)
                quality = 1;
            double trackLengthOccurdingToData = (double)DataSize / (double)ByteRate;
            Progress?.Invoke(null, new ProgressArgs(0, "Generating WaveForm Pixel Data file ..."));

            long dataWrittenIndex = 0;
            long dataWrittenLength = 0;
            availableBufferSize = 0;
            TheBuffer = new byte[chunkSize];
            BufferNmChannel = NumChannels;

            for (double sec = 0; sec <= trackLengthOccurdingToData; sec += secondsPerBlock)
            {
                if (abort_generating)
                    break;

                Progress?.Invoke(null, new ProgressArgs((int)((sec * 100) / trackLengthOccurdingToData), "Generating WaveForm Pixel Data file ..."));
                pointA = GetSamplePoint(sec);
                pointA -= (pointA - DataPointer) % bytesPerBlock;
                Stream.Seek(pointA, SeekOrigin.Begin);

                //pointA = Stream.Position;
                pointB = pointA + bytesPerBlock;

                if (pointB >= Stream.Length)
                {
                    ws.Flush();
                    ws.Close();
                    ws = null;
                    BufferFileName = fileName;
                    BufferPresented = true;

                    OpenBuffer(fileName);
                    break;
                }
                leftChannelPOS = 0;
                leftChannelNEG = 0;
                rightChannelPOS = 0;
                rightChannelNEG = 0;
                leftPOSAv = 0;
                rightPOSAv = 0;
                leftNEGAv = 0;
                rightNEGAv = 0;
                byte[] sampleBlock = new byte[BlockAlign];
                for (long point = pointA; point < pointB; point += BlockAlign * quality)
                {
                    sampleBlock = new byte[BlockAlign];
                    if (point + (BlockAlign - 1) < Stream.Length)
                    {
                        Stream.Read(sampleBlock, 0, BlockAlign);
                    }
                    // Channels processing
                    switch (NumChannels)
                    {
                        case 1: // MONO
                            {
                                switch (bytesBySample)
                                {
                                    case 1:
                                        {
                                            leftPOSAv = sampleBlock[0];
                                            leftNEGAv = 0;
                                            break;
                                        }
                                    case 2:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });

                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;

                                            break;
                                        }
                                    case 3:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;
                                            break;
                                        }
                                    case 4:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;
                                            break;
                                        }
                                }
                                if (leftPOSAv > leftChannelPOS)
                                    leftChannelPOS = leftPOSAv;
                                if (leftNEGAv < leftChannelNEG)
                                    leftChannelNEG = leftNEGAv;
                                break;
                            }
                        case 2: // STEREO
                            {
                                switch (bytesBySample)
                                {
                                    case 1:
                                        {
                                            leftPOSAv = sampleBlock[0];
                                            rightPOSAv = sampleBlock[1];
                                            break;
                                        }
                                    case 2:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });
                                            int right = IntFromBytes(new byte[] { sampleBlock[2], sampleBlock[3] });

                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;
                                            rightPOSAv = right > 0 ? right : 0;
                                            rightNEGAv = right < 0 ? right : 0;

                                            break;
                                        }
                                    case 3:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                                            int right = IntFromBytes(new byte[] { sampleBlock[3], sampleBlock[4], sampleBlock[5] });

                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;
                                            rightPOSAv = right > 0 ? right : 0;
                                            rightNEGAv = right < 0 ? right : 0;
                                            break;
                                        }
                                    case 4:
                                        {
                                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                                            int right = IntFromBytes(new byte[] { sampleBlock[4], sampleBlock[5], sampleBlock[6], sampleBlock[7] });
                                            leftPOSAv = left > 0 ? left : 0;
                                            leftNEGAv = left < 0 ? left : 0;
                                            rightPOSAv = right > 0 ? right : 0;
                                            rightNEGAv = right < 0 ? right : 0;
                                            break;
                                        }
                                }
                                if (leftPOSAv > leftChannelPOS)
                                    leftChannelPOS = leftPOSAv;
                                if (leftNEGAv < leftChannelNEG)
                                    leftChannelNEG = leftNEGAv;
                                if (rightPOSAv > rightChannelPOS)
                                    rightChannelPOS = rightPOSAv;
                                if (rightNEGAv < rightChannelNEG)
                                    rightChannelNEG = rightNEGAv;
                                break;
                            }
                    }
                }
                // Write down data
                switch (NumChannels)
                {
                    case 1: // MONO
                        {
                            // Now write the samples data
                            leftChannelPOS = ToPixels(leftChannelPOS);
                            if (leftChannelPOS < 0)
                                leftChannelPOS *= -1;

                            leftChannelNEG = ToPixels(leftChannelNEG);
                            if (leftChannelNEG < 0)
                                leftChannelNEG *= -1;

                            ws.Write(new byte[] {
                                               (byte)leftChannelPOS,
                                               (byte)leftChannelNEG,
                                                    }, 0, 2);
                            if (abort_generating)
                                break;
                            // UPDATE BUFFER
                            TheBuffer[dataWrittenIndex + 0] = (byte)leftChannelPOS;
                            TheBuffer[dataWrittenIndex + 1] = (byte)leftChannelNEG;
                            dataWrittenIndex += 2;

                            if (dataWrittenIndex - dataWrittenLength >= chunkSize)
                            {
                                dataWrittenLength = dataWrittenIndex;
                                availableBufferSize = dataWrittenIndex;
                                BufferPresented = true;
                                WFPDChunckGenerated?.Invoke(null, new EventArgs());
                                Array.Resize(ref TheBuffer, (int)(TheBuffer.Length + chunkSize));
                            }

                            break;
                        }
                    case 2: // STEREO
                        {
                            // Now write the samples data
                            leftChannelPOS = ToPixels(leftChannelPOS);
                            if (leftChannelPOS < 0)
                                leftChannelPOS *= -1;
                            leftChannelNEG = ToPixels(leftChannelNEG);
                            if (leftChannelNEG < 0)
                                leftChannelNEG *= -1;
                            rightChannelPOS = ToPixels(rightChannelPOS);
                            if (rightChannelPOS < 0)
                                rightChannelPOS *= -1;
                            rightChannelNEG = ToPixels(rightChannelNEG);
                            if (rightChannelNEG < 0)
                                rightChannelNEG *= -1;

                            ws.Write(new byte[] {
                                               (byte)leftChannelPOS,
                                               (byte)leftChannelNEG,
                                               (byte)rightChannelPOS,
                                               (byte)rightChannelNEG },
                                            0, 4);
                            if (abort_generating)
                                break;
                            // UPDATE BUFFER
                            TheBuffer[dataWrittenIndex + 0] = (byte)leftChannelPOS;
                            TheBuffer[dataWrittenIndex + 1] = (byte)leftChannelNEG;
                            TheBuffer[dataWrittenIndex + 2] = (byte)rightChannelPOS;
                            TheBuffer[dataWrittenIndex + 3] = (byte)rightChannelNEG;
                            dataWrittenIndex += 4;
                            if (dataWrittenIndex - dataWrittenLength >= chunkSize)
                            {
                                dataWrittenLength = dataWrittenIndex;
                                availableBufferSize = dataWrittenIndex;
                                BufferPresented = true;
                                WFPDChunckGenerated?.Invoke(null, new EventArgs());
                                Array.Resize(ref TheBuffer, (int)(TheBuffer.Length + chunkSize));
                            }
                            break;
                        }
                }

                if (abort_generating)
                    break;
            }
            // Finished
            if (ws != null)
            {
                ws.Flush();
                ws.Dispose();
                ws.Close();
            }
            if (abort_generating)
            {
                Progress?.Invoke(null, new ProgressArgs(100, "Generating process aborted."));
            }
            else
                Progress?.Invoke(null, new ProgressArgs(100, "Done , opening buffer ..."));

            if (closeAfterGenerate)
            {
                IsReady = false;
                Stream.Dispose();
                Stream.Close();
            }
            if (!abort_generating)
                OpenBuffer(fileName);
            else
            {
                // Delete corrupted file
                if (File.Exists(fileName))
                    try
                    {
                        File.Delete(fileName);
                    }
                    catch
                    {

                    }
            }
            if (deleteWaveFileAfterfinish)
                if (File.Exists(FileName))
                    try
                    {
                        File.Delete(FileName);
                    }
                    catch
                    {

                    }
            ProgressFinished?.Invoke(null, new EventArgs());
            IsGenerating = false;
        }
        public static bool OpenBuffer(string fileName)
        {
            BufferStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BufferPresented = false;
            // 1 Read WFPD
            byte[] readData = new byte[4];
            BufferStream.Read(readData, 0, 4);
            if (ASCIIEncoding.ASCII.GetString(readData) != "WFPD")
            {
                Console.WriteLine("ERROR: cannot read wfpd file, WFPD check faild.");
                BufferStream.Dispose();
                BufferStream.Close();
                return false;
            }
            // 2 Read NumChannels
            readData = new byte[2];
            BufferStream.Read(readData, 0, 2);
            BufferNmChannel = IntFromBytes(readData);
            // 3 read bits per sample
            readData = new byte[2];
            BufferStream.Read(readData, 0, 2);
            BitsPerSample = IntFromBytes(readData);
            // 4 Read MilliBlock
            readData = new byte[4];
            BufferStream.Read(readData, 0, 4);
            BufferMillisPerBlock = IntFromBytes(readData);

            BufferFileName = fileName;
            BufferPresented = true;
            BufferBytesPerBlock = ((long)BufferMillisPerBlock * ByteRate) / 1000;

            TheBuffer = new byte[BufferStream.Length - 12];
            BufferLength = BufferStream.Length - 12;

            Length = (double)BufferLength / (double)(BufferNmChannel * 2);
            availableBufferSize = TheBuffer.Length;
            BufferStream.Read(TheBuffer, 0, TheBuffer.Length);
            BufferStream.Dispose();
            BufferStream.Close();

            WFPDLoaded?.Invoke(null, new EventArgs());
            return true;
        }
        public static byte[] GetSampleBlockAt(double seconds)
        {
            return GetSampleBlock(GetSamplePoint(seconds));
        }
        public static byte[] GetSampleBlock(long point)
        {
            byte[] sampleBlock = new byte[BlockAlign];
            if (point + BlockAlign < Stream.Length)
            {
                Stream.Seek(point, SeekOrigin.Begin);
                Stream.Read(sampleBlock, 0, BlockAlign);
            }
            return sampleBlock;
        }
        public static long GetSamplePoint(double seconds)
        {
            seconds *= 1000;
            long index = (long)((seconds * DataSize) / (Length * 1000));

            // Finally the point within the file

            return DataPointer + index;
        }
        public static void GetMonoSample(double seconds, out int sample)
        {
            byte[] sampleBlock = GetSampleBlockAt(seconds);

            int bytesBySample = BitsPerSample / 8;
            switch (bytesBySample)
            {
                default:
                    {
                        sample = 0;
                        break;
                    }
                case 1:
                    {
                        sample = sampleBlock[0];
                        break;
                    }

                case 2:
                    {
                        sample = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });
                        break;
                    }
                case 3:
                    {
                        sample = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                        break;
                    }
                case 4:
                    {
                        sample = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                        break;
                    }
            }
        }
        public static void GetMonoSampleAV(double seconds_start, double seconds_end, out int sample)
        {
            long pointA = GetSamplePoint(seconds_start);
            long pointB = GetSamplePoint(seconds_end);
            long diff = pointB - pointA;
            int bytesBySample = BitsPerSample / 8;
            if (diff < 0)
            {
                sample = 0;
                return;
            }
            int sampAv = 0;
            int samplesPassed = 0;
            for (long point = pointA; point <= pointB; point += diff / BlockAlign)
            {
                byte[] sampleBlock = GetSampleBlock(point);
                switch (bytesBySample)
                {
                    case 1:
                        {
                            sampAv += sampleBlock[0];
                            break;
                        }

                    case 2:
                        {
                            sampAv += IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });
                            break;
                        }
                    case 3:
                        {
                            sampAv += IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                            break;
                        }
                    case 4:
                        {
                            sampAv += IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                            break;
                        }
                }

                samplesPassed++;
            }
            if (samplesPassed > 0)
            {
                sample = sampAv / samplesPassed;
            }
            else
                sample = 0;
        }
        public static void GetStereoSample(double seconds, out int leftChannel, out int rightChannel)
        {
            byte[] sampleBlock = GetSampleBlockAt(seconds);

            int bytesBySample = BitsPerSample / 8;
            switch (bytesBySample)
            {
                default:
                    {
                        rightChannel = 0;
                        leftChannel = 0;
                        break;
                    }
                case 1:
                    {
                        leftChannel = sampleBlock[0];
                        rightChannel = sampleBlock[1];
                        break;
                    }

                case 2:
                    {
                        leftChannel = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });
                        rightChannel = IntFromBytes(new byte[] { sampleBlock[2], sampleBlock[3] });
                        break;
                    }
                case 3:
                    {
                        leftChannel = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                        rightChannel = IntFromBytes(new byte[] { sampleBlock[3], sampleBlock[4], sampleBlock[5] });
                        break;
                    }
                case 4:
                    {
                        leftChannel = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                        rightChannel = IntFromBytes(new byte[] { sampleBlock[4], sampleBlock[5], sampleBlock[6], sampleBlock[7] });
                        break;
                    }
            }
        }
        public static void GetStereoSampleAV(double seconds_start, double seconds_end, out int leftChannelPOS, out int leftChannelNEG, out int rightChannelPOS, out int rightChannelNEG)
        {
            long pointA = GetSamplePoint(seconds_start);
            long pointB = GetSamplePoint(seconds_end);
            long diff = pointB - pointA;
            int bytesBySample = BitsPerSample / 8;
            if (diff < 0)
            {
                leftChannelPOS = 0;
                leftChannelNEG = 0;
                rightChannelPOS = 0;
                rightChannelNEG = 0;
                return;
            }
            int leftPOSAv = 0;
            int rightPOSAv = 0;

            int leftNEGAv = 0;
            int rightNEGAv = 0;

            int samplesPassed = 0;
            for (long point = pointA; point < pointB; point += diff / BlockAlign)
            {
                byte[] sampleBlock = GetSampleBlock(point);
                switch (bytesBySample)
                {
                    case 1:
                        {
                            leftPOSAv += sampleBlock[0];

                            rightPOSAv += sampleBlock[1];
                            break;
                        }

                    case 2:
                        {
                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1] });
                            int right = IntFromBytes(new byte[] { sampleBlock[2], sampleBlock[3] });

                            leftPOSAv += left > 0 ? left : 0;
                            leftNEGAv += left < 0 ? left : 0;
                            rightPOSAv += right > 0 ? right : 0;
                            rightNEGAv += right < 0 ? right : 0;

                            break;
                        }
                    case 3:
                        {
                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2] });
                            int right = IntFromBytes(new byte[] { sampleBlock[3], sampleBlock[4], sampleBlock[5] });

                            leftPOSAv += left > 0 ? left : 0;
                            leftNEGAv += left < 0 ? left : 0;
                            rightPOSAv += right > 0 ? right : 0;
                            rightNEGAv += right < 0 ? right : 0;
                            break;
                        }
                    case 4:
                        {
                            int left = IntFromBytes(new byte[] { sampleBlock[0], sampleBlock[1], sampleBlock[2], sampleBlock[3] });
                            int right = IntFromBytes(new byte[] { sampleBlock[4], sampleBlock[5], sampleBlock[6], sampleBlock[7] });
                            leftPOSAv += left > 0 ? left : 0;
                            leftNEGAv += left < 0 ? left : 0;
                            rightPOSAv += right > 0 ? right : 0;
                            rightNEGAv += right < 0 ? right : 0;
                            break;
                        }
                }

                samplesPassed++;
            }
            if (samplesPassed > 0)
            {
                leftChannelPOS = leftPOSAv / samplesPassed;
                leftChannelNEG = leftNEGAv / samplesPassed;
                rightChannelPOS = rightPOSAv / samplesPassed;
                rightChannelNEG = rightNEGAv / samplesPassed;
            }
            else
            {
                leftChannelPOS = 0;
                leftChannelNEG = 0;
                rightChannelPOS = 0;
                rightChannelNEG = 0;
            }
        }
        public static void GetPixelValuesFromBufferSTEREO(double seconds, int millipixel, out int leftChannelPOS, out int leftChannelNEG, out int rightChannelPOS, out int rightChannelNEG)
        {
            if (TheBuffer == null)
            {
                leftChannelPOS = 0;
                leftChannelNEG = 0;
                rightChannelPOS = 0;
                rightChannelNEG = 0;
                return;
            }
            long block1 = (long)(seconds * 1000);
            //block1 += 1;
            block1 /= BufferMillisPerBlock;
            block1 *= 4;

            if (block1 >= availableBufferSize)
            {
                leftChannelPOS = 0;
                leftChannelNEG = 0;
                rightChannelPOS = 0;
                rightChannelNEG = 0;
                return;
            }

            double seconds1 = seconds + ((double)millipixel / 1000);
            long block2 = (long)(seconds1 * 1000);
            block2 /= BufferMillisPerBlock;
            block2 *= 4;

            leftChannelPOS = 0;
            leftChannelNEG = 0;
            rightChannelPOS = 0;
            rightChannelNEG = 0;

            for (long blk = block1; blk <= block2; blk += 4)
            {
                // Read it ! 
                if (blk + 3 < TheBuffer.Length)
                {
                    if (TheBuffer[blk + 0] > leftChannelPOS)
                        leftChannelPOS = TheBuffer[blk + 0];
                    if (TheBuffer[blk + 1] > leftChannelNEG)
                        leftChannelNEG = TheBuffer[blk + 1];
                    if (TheBuffer[blk + 2] > rightChannelPOS)
                        rightChannelPOS = TheBuffer[blk + 2];
                    if (TheBuffer[blk + 3] > rightChannelNEG)
                        rightChannelNEG = TheBuffer[blk + 3];
                }
            }
        }
        public static void GetPixelValuesFromBufferMONO(double seconds, int millipixel, out int sampleChannelPOS, out int sampleChannelNEG)
        {
            if (TheBuffer == null)
            {
                sampleChannelPOS = 0;
                sampleChannelNEG = 0;
                return;
            }
            long block1 = (long)(seconds * 1000);
            block1 /= BufferMillisPerBlock;
            block1 *= 2;

            if (block1 >= availableBufferSize)
            {
                sampleChannelPOS = 0;
                sampleChannelNEG = 0;
                return;
            }

            double seconds1 = seconds + ((double)millipixel / 1000);
            long block2 = (long)(seconds1 * 1000);
            block2 /= BufferMillisPerBlock;
            block2 *= 2;

            sampleChannelPOS = 0;
            sampleChannelNEG = 0;

            for (long blk = block1; blk <= block2; blk += 2)
            {
                // Read it ! 
                if (blk + 1 < TheBuffer.Length)
                {
                    if (TheBuffer[blk + 0] > sampleChannelPOS)
                        sampleChannelPOS = TheBuffer[blk + 0];
                    if (TheBuffer[blk + 1] > sampleChannelNEG)
                        sampleChannelNEG = TheBuffer[blk + 1];
                }
            }
        }
        public static void Dispose()
        {
            IsReady = false;
            FileName = "";
            if (Stream != null)
            {
                Stream.Dispose();
                Stream.Close();
            }
            if (BufferStream != null)
            {
                BufferStream.Dispose();
                BufferStream.Close();
            }
        }
        public static void ClearBuffer()
        {
            if (BufferStream != null)
            {
                BufferStream.Dispose();
                BufferStream.Close();
            }
            BufferPresented = false;
            BufferNmChannel = BufferMillisPerBlock = 0;
            BufferFileName = "";
            TheBuffer = null;
            BufferLength = 0;
        }

        public static double ToDB(int sample)
        {
            double dB = 0;

            /*
amplitude = 14731 / 32767 = 0.44
dB = 20 * log10(0.44) = -7.13

 */
            double max = 0;
            switch (BitsPerSample)
            {
                case 8: max = byte.MaxValue; break;
                case 16: max = ushort.MaxValue; break;
                case 24: max = 16777215; break;
            }
            double amp = (double)sample / max;
            if (amp != 0)
                dB = 20 * Math.Log10(amp);

            return dB;
        }
        public static int ToPixels(int sample)
        {
            int maxPos = 0;
            int maxNeg = 0;
            switch (BitsPerSample)
            {
                case 8:
                    {
                        maxPos = byte.MaxValue;
                        maxNeg = byte.MinValue;
                        break;
                    }
                case 16:
                    {
                        maxPos = short.MaxValue;
                        maxNeg = short.MinValue;
                        break;
                    }
                case 24:
                    {
                        maxPos = 8388607;
                        maxNeg = -8388607;
                        break;
                    }
                case 32:
                    {
                        maxPos = int.MaxValue;
                        maxNeg = int.MinValue;
                        break;
                    }
            }
            if (maxPos > 0)
                return (sample * byte.MaxValue) / maxPos;

            return 0;
        }
        public static int ScaleBufferPixel(int pixel, int height)
        {
            return (pixel * height) / byte.MaxValue;
        }
        public static double ScaleBufferPixel(double pixel, double height)
        {
            return (pixel * height) / (double)byte.MaxValue;
        }

        private static int IntFromBytes(byte[] data)
        {
            int val = 0;
            if (data.Length == 1)
                val = data[0];
            else if (data.Length == 2)
                val = (short)((data[0] << 0) | (data[1] << 8));
            else if (data.Length == 3)
            {
                val = (data[0] << 0) | (data[1] << 8) | (data[2] << 16);

                if ((val & 0x800000) != 0)
                    val |= ~0xffffff;
            }
            else if (data.Length == 4)
                val = (data[0] << 0) | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);
            return val;
        }
        private static byte[] BytesFromInt16(short val)
        {
            byte[] data = new byte[2];
            data[0] = (byte)(val & 0xFF);
            data[1] = (byte)((val & 0xFF00) >> 8);
            return data;
        }
        private static byte[] BytesFromInt32(int val)
        {
            byte[] data = new byte[4];
            data[0] = (byte)(val & 0xFF);
            data[1] = (byte)((val & 0xFF00) >> 8);
            data[2] = (byte)((val & 0xFF0000) >> 16);
            data[3] = (byte)((val & 0xFF000000) >> 24);

            return data;
        }

        public static event EventHandler WFPDLoaded;
        public static event EventHandler WFPDChunckGenerated;
        public static event EventHandler ProgressStarted;
        public static event EventHandler<ProgressArgs> Progress;
        public static event EventHandler ProgressFinished;
    }
}
