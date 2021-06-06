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
using System.IO;
using System.Collections.Generic;
using AHD.ID3.Frames;
using AHD.ID3.Types;
namespace AHD.ID3
{
    /// <summary>
    /// The class of ID3v2, handles save and load for ID3 Tag version 2.x
    /// </summary>
    public class ID3v2
    {
        /// <summary>
        /// The class of ID3v2, handles save and load for ID3 Tag version 2.x
        /// </summary>
        public ID3v2()
        {
        }
        private const int bufferChunk = 0x100000;// 1 MB
        private string fileName = "";
        private int tagSize = 0;
        private byte tagFlags = 0;
        private int paddingSize = 0;
        private ExtendedHeader extendedHeader = new ExtendedHeader();
        /*Flags*/
        private bool extenderHeaderPresented = false;// version 3
        private bool unsynchronisation = false;// version 2, 3 and 4
        private bool experimental = false;// version 3
        private bool compression = false;// version 2 only
        private bool footer = false;// version 4 only
        private bool savePadding = false;

        private ID3Version version;
        private ASCIIEncoding encoding = new ASCIIEncoding();
        private List<ID3TagFrame> frames = new List<ID3TagFrame>();

        // Methods
        /// <summary>
        /// Get a frame that may loaded
        /// </summary>
        /// <param name="id">The id of the frame. Note that there's a difference between version 2.2 frames and version 2.3/2.4</param>
        /// <returns>The frame if found otherwise null</returns>
        public ID3TagFrame GetFrameLoaded(string id)
        {
            foreach (ID3TagFrame frame in frames)
            {
                if (frame.ID.ToLower() == id.ToLower())
                {
                    return frame;
                }
            }
            return null;
        }
        /// <summary>
        /// Remove a frame from the frames collection. (one frame will be removed, if this frame id can be exist more than once,
        ///  Only the first one with that id will be removed.
        /// </summary>
        /// <param name="id">The frame id.</param>
        public void RemoveFrame(string id)
        {
            foreach (ID3TagFrame frame in frames)
            {
                if (frame.ID.ToLower() == id.ToLower())
                {
                    frames.Remove(frame);
                    break;
                }
            }
        }
        /// <summary>
        /// Remove a frame from the frames collection. (all frames will be removed, if this frame id can be exist more than once,
        ///  all the frames of this id will be removed.
        /// </summary>
        /// <param name="id">The frame id.</param>
        public void RemoveFrameAll(string id)
        {
            for (int i = 0; i < frames.Count; i++)
            {
                if (frames[i].ID.ToLower() == id.ToLower())
                {
                    frames.RemoveAt(i);
                    i--;
                }
            }
        }
        /// <summary>
        /// Load ID3 Tag information from file.
        /// </summary>
        /// <param name="fileName">The complete file path. Must be exist.</param>
        /// <returns>The result of the load operation.</returns>
        public Result Load(string fileName)
        {
            OnLoadStart();
            Stream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (stream.Length < 10)
                {
                    stream.Dispose();
                    stream.Close();
                    return Result.NoID3Exist;
                }
                // read header and check
                byte[] header = new byte[10];
                stream.Read(header, 0, 10);
                // ID3 file identifier
                if (encoding.GetString(new byte[] { header[0], header[1], header[2], }) != "ID3")
                {
                    stream.Dispose();
                    stream.Close();
                    OnLoadFinshed();
                    return Result.NoID3Exist;
                }
                // Version
                version = new ID3Version(header[3], header[4]);
                // Flags
                tagFlags = header[5];
                // Size
                // first, make sure size is acceptable
                int size = SynchsafeConvertor.FromBytes(new byte[] { header[6], header[7], header[8], header[9], });
                if (size == 0 || (header[6] >= 0x80 || header[7] >= 0x80 || header[8] >= 0x80 || header[9] >= 0x80))
                {
                    stream.Dispose();
                    stream.Close();
                    OnLoadFinshed();
                    return Result.Failed;
                }
                // now read the actual size
                tagSize = SynchsafeConvertor.FromSynchsafe(new byte[] { header[6], header[7], header[8], header[9], });
                // get data and frames depending on version supported...
                switch (header[3])
                {
                    case 2: this.fileName = fileName; return LoadDataVersion2(stream);
                    case 3: this.fileName = fileName; return LoadDataVersion3(stream);
                    case 4: this.fileName = fileName; return LoadDataVersion4(stream);
                    default:
                        stream.Dispose();
                        stream.Close();
                        OnLoadFinshed();
                        this.fileName = "";
                        return Result.NotCompatibleVersion;
                }

            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine("Unable to load id3v2 for file:", DebugCode.Error);
                DebugLogger.WriteLine(fileName, DebugCode.Error);
                DebugLogger.WriteLine(ex.Message, DebugCode.Error);
                if (stream != null)
                {
                    stream.Dispose();
                    stream.Close();
                }
                OnLoadFinshed();
                return Result.Failed;
            }
        }
        /// <summary>
        /// Save the current frames collection into file.
        /// </summary>
        /// <param name="fileName">The complete file path</param>
        /// <returns>The result of the save operation</returns>
        public Result Save(string fileName)
        {
            OnSaveStart();
            // 1 the original file stream
            Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            bool tempSaved = false;
            // create the temp file
            int j = 0;
            string tempFolder = Path.GetDirectoryName(fileName);
            if (tempFolder.Length == 0)
                tempFolder = Path.GetPathRoot(fileName);
            string tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            while (File.Exists(tempName))
            {
                j++;
                tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            }
            Stream tempStream = new FileStream(tempName, FileMode.Create, FileAccess.Write);
            try
            {
                // write the ID3v2 buffer to the temp file
                byte[] data = new byte[0];
                switch (version.Major)
                {
                    case 2: data = GetVersion2Buffer(); break;
                    case 3: data = GetVersion3Buffer(); break;
                    case 4: data = GetVersion4Buffer(); break;
                }
                tempStream.Write(data, 0, data.Length);
                // check if the original file has ID3 ...
                byte[] orHeadr = new byte[10];
                fileStream.Read(orHeadr, 0, 10);
                if (encoding.GetString(new byte[] { orHeadr[0], orHeadr[1], orHeadr[2], }) == "ID3")
                {
                    int size = SynchsafeConvertor.FromSynchsafe(new byte[] { orHeadr[6], orHeadr[7], orHeadr[8], orHeadr[9], });
                    // skip
                    fileStream.Position = 10 + size;
                }
                // copy the original file data to the temp file without id3
                byte[] buff = new byte[bufferChunk];
                // We can't just read file as one byte buffer and write it directly into temp, what if the file is too large ?
                // so do it chunck by chunck to limit the buffer memory to bufferChunk value....
                // larger bufferChunk value make saving goes faster but need more memory.
                // however, bufferChunk = 1 MB for now.
                while (fileStream.Position <= fileStream.Length)
                {
                    long siz = fileStream.Length - fileStream.Position;
                    if (siz >= bufferChunk)
                    {
                        buff = new byte[bufferChunk];
                        fileStream.Read(buff, 0, bufferChunk);// ->|
                        tempStream.Write(buff, 0, bufferChunk);//<-|
                    }
                    else
                    {
                        buff = new byte[siz];
                        fileStream.Read(buff, 0, (int)(siz));// ->|
                        tempStream.Write(buff, 0, (int)(siz));//<-|
                        break;
                    }
                    OnProgress("Writing ID3v2 data ...", (int)((fileStream.Position * 100) / fileStream.Length));
                }
                tempSaved = true;// backup plan ..later
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete original then make temp as original 
                FileInfo inf = new FileInfo(fileName);
                inf.IsReadOnly = false;
                File.Delete(fileName);
                // this is stupid, but fixes the issue when a media player is playing
                // this file and attempting to save.....
                bool ok = false;
                while (!ok)
                {
                    try
                    {
                        File.Copy(tempName, fileName);
                        ok = true;
                    }
                    catch { }
                }
                File.Delete(tempName);
                // done ! 
                OnSaveFinshed();
                this.fileName = fileName;
                return Result.Success;
            }
            catch (Exception ex)
            {
                // use the backup plan
                if (tempSaved)
                {
                    if (File.Exists(tempName))
                    {
                        try
                        {
                            DebugLogger.WriteLine("Trying to save the file backup to another path", DebugCode.Warning);
                            Directory.CreateDirectory(Path.GetTempPath() + "\\AITE\\");
                            File.Copy(tempName, Path.GetTempPath() + "\\AITE\\" + Path.GetFileName(fileName));
                            DebugLogger.WriteLine("Done ! file saved to: " + Path.GetTempPath() + "\\AITE\\" +
                                Path.GetFileName(fileName), DebugCode.Good);
                        }
                        catch (Exception ex1)
                        {
                            DebugLogger.WriteLine("Unable to save backup file: " + ex1.Message, DebugCode.Error);
                        }
                    }
                }
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete temp if found
                if (File.Exists(tempName))
                    File.Delete(tempName);
                OnSaveFinshed();
                this.fileName = "";
                return Result.Failed;
            }
        }
        /// <summary>
        /// Save the current frames collection and id3 tag version 1 into file.
        /// </summary>
        /// <param name="fileName">The complete file path</param>
        /// <param name="id3v1">The id3v1 object to use to save id3v1 data</param>
        /// <returns>The result of the save operation</returns>
        public Result Save(string fileName, ID3v1 id3v1)
        {
            OnSaveStart();
            bool tempSaved = false;
            // 1 the original file stream
            Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            // create the temp file
            int j = 0;
            string tempFolder = Path.GetDirectoryName(fileName);
            if (tempFolder.Length == 0)
                tempFolder = Path.GetPathRoot(fileName);
            string tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            while (File.Exists(tempName))
            {
                j++;
                tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            }
            Stream tempStream = new FileStream(tempName, FileMode.Create, FileAccess.Write);
            try
            {
                // write the ID3v2 buffer to the temp file
                byte[] data = new byte[0];
                switch (version.Major)
                {
                    case 2: data = GetVersion2Buffer(); break;
                    case 3: data = GetVersion3Buffer(); break;
                    case 4: data = GetVersion4Buffer(); break;
                }
                tempStream.Write(data, 0, data.Length);
                // check if the original file has ID3 ...
                byte[] orHeadr = new byte[10];
                fileStream.Read(orHeadr, 0, 10);
                if (encoding.GetString(new byte[] { orHeadr[0], orHeadr[1], orHeadr[2], }) == "ID3")
                {
                    int size = SynchsafeConvertor.FromSynchsafe(new byte[] { orHeadr[6], orHeadr[7], orHeadr[8], orHeadr[9], });
                    // skip
                    fileStream.Position = 10 + size;
                }
                // id3v1 ?
                long max = fileStream.Length - 128;
                // copy the original file data to the temp file without id3
                byte[] buff = new byte[bufferChunk];
                // We can't just read file as one byte buffer and write it directly into temp, what if the file is too large ?
                // so do it chunck by chunck to limit the buffer memory to bufferChunk value....
                // larger bufferChunk value make saving goes faster but need more memory.
                // however, bufferChunk = 1 MB for now.
                while (fileStream.Position <= max)
                {
                    long siz = max - fileStream.Position;
                    if (siz >= bufferChunk)
                    {
                        buff = new byte[bufferChunk];
                        fileStream.Read(buff, 0, bufferChunk);// ->|
                        tempStream.Write(buff, 0, bufferChunk);//<-|
                    }
                    else
                    {
                        buff = new byte[siz];
                        fileStream.Read(buff, 0, (int)(siz));// ->|
                        tempStream.Write(buff, 0, (int)(siz));//<-|
                        break;
                    }
                    OnProgress("Writing ID3 Tag data ...", (int)((fileStream.Position * 100) / max));
                }
                // now write id3v1 data
                tempStream.Write(id3v1.CreateBuffer(), 0, 128);
                tempSaved = true;// backup plan ..later
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete original then make temp as original 
                FileInfo inf = new FileInfo(fileName);
                inf.IsReadOnly = false;
                File.Delete(fileName);
                // this is stupid, but fixes the issue when a media player is playing
                // this file and attempting to save.....
                bool ok = false;
                while (!ok)
                {
                    try
                    {
                        File.Copy(tempName, fileName);
                        ok = true;
                    }
                    catch { }
                }
                File.Delete(tempName);
                // done ! 
                OnSaveFinshed(); this.fileName = fileName;
                return Result.Success;
            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine("Unable to save file: " + ex.Message, DebugCode.Error);
                // use the backup plan
                if (tempSaved)
                {
                    if (File.Exists(tempName))
                    {
                        try
                        {
                            DebugLogger.WriteLine("Trying to save the file backup to another path", DebugCode.Warning);
                            Directory.CreateDirectory(Path.GetTempPath() + "\\AITE\\");
                            File.Copy(tempName, Path.GetTempPath() + "\\AITE\\" + Path.GetFileName(fileName));
                            DebugLogger.WriteLine("Done ! file saved to: " + Path.GetTempPath() + "\\AITE\\" +
                                Path.GetFileName(fileName), DebugCode.Good);
                        }
                        catch (Exception ex1)
                        {
                            DebugLogger.WriteLine("Unable to save backup file: " + ex1.Message, DebugCode.Error);
                        }
                    }
                }
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete temp if found
                if (File.Exists(tempName))
                    File.Delete(tempName);
                OnSaveFinshed(); this.fileName = "";
                return Result.Failed;
            }
        }

        /// <summary>
        /// Check if a file has ID3v2. This will not check for compatible version or size.
        /// </summary>
        /// <param name="fileName">The full mp3 file path</param>
        /// <returns>True if given file has ID3v2, otherwise false</returns>
        public static bool Check(string fileName)
        {
            try
            {
                Stream str = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (str.Length < 10)
                    return false;
                byte[] header = new byte[10];
                str.Read(header, 0, 10);
                str.Dispose();
                str.Close();
                ASCIIEncoding enc = new ASCIIEncoding();
                string tag = enc.GetString(new byte[] { header[0], header[1], header[2], });
                if (tag == "ID3")
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine("Unable to check id3v2 for file:", DebugCode.Error);
                DebugLogger.WriteLine(fileName, DebugCode.Error);
                DebugLogger.WriteLine(ex.Message, DebugCode.Error);
            }
            return false;
        }
        /// <summary>
        /// Clear (erase) id3v2 from file
        /// </summary>
        /// <param name="fileName">Complete file path</param>
        public static void Clear(string fileName)
        {
            // 1 the original file stream
            Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            // create the temp file
            int j = 0;
            string tempFolder = Path.GetDirectoryName(fileName);
            if (tempFolder.Length == 0)
                tempFolder = Path.GetPathRoot(fileName);
            string tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            while (File.Exists(tempName))
            {
                j++;
                tempName = tempFolder + Path.GetFileNameWithoutExtension(fileName) + "_temp" + j;
            }
            Stream tempStream = new FileStream(tempName, FileMode.Create, FileAccess.Write);
            try
            {
                // check if the original file has ID3 ...
                byte[] orHeadr = new byte[10];
                fileStream.Read(orHeadr, 0, 10);
                ASCIIEncoding encoding = new ASCIIEncoding();
                if (encoding.GetString(new byte[] { orHeadr[0], orHeadr[1], orHeadr[2], }) == "ID3")
                {
                    int size = SynchsafeConvertor.FromSynchsafe(new byte[] { orHeadr[6], orHeadr[7], orHeadr[8], orHeadr[9], });
                    // skip
                    fileStream.Position = 10 + size;
                }
                // copy the original file data to the temp file without id3
                byte[] buff = new byte[bufferChunk];
                // We can't just read file as one byte buffer and write it directly into temp, what if the file is too large ?
                // so do it chunck by chunck to limit the buffer memory to bufferChunk value....
                // larger bufferChunk value make saving goes faster but need more memory.
                // however, bufferChunk = 1 MB for now.
                while (fileStream.Position <= fileStream.Length)
                {
                    long siz = fileStream.Length - fileStream.Position;
                    if (siz >= bufferChunk)
                    {
                        buff = new byte[bufferChunk];
                        fileStream.Read(buff, 0, bufferChunk);// ->|
                        tempStream.Write(buff, 0, bufferChunk);//<-|
                    }
                    else
                    {
                        buff = new byte[siz];
                        fileStream.Read(buff, 0, (int)(siz));// ->|
                        tempStream.Write(buff, 0, (int)(siz));//<-|
                        break;
                    }

                }

                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete original then make temp as original 
                FileInfo inf = new FileInfo(fileName);
                inf.IsReadOnly = false;
                File.Delete(fileName);
                // this is stupid, but fixes the issue when a media player is playing
                // this file and attempting to save.....
                bool ok = false;
                while (!ok)
                {
                    try
                    {
                        File.Copy(tempName, fileName);
                        ok = true;
                    }
                    catch { }
                }
                File.Delete(tempName);
            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine("Unable to clear file: " + ex.Message, DebugCode.Error);
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete temp if found
                if (File.Exists(tempName))
                    File.Delete(tempName);
            }
        }

        private Result LoadDataVersion2(Stream stream)
        {
            extendedHeader = new ExtendedHeader();
            byte[] tagBuffer = new byte[tagSize];
            stream.Position = 10;
            long index = 10;
            // compression.
            compression = ((tagFlags & 0x40) == 0x40);
            // Now get the buffer ...
            stream.Position = index;
            stream.Read(tagBuffer, 0, tagSize);
            List<byte> buffer = new List<byte>(tagBuffer);
            stream.Close();
            stream.Dispose();
            // "Unsynchronisation" used ?
            if (unsynchronisation = ((tagFlags & 0x80) == 0x80))
            {
                // search for a false synchronisation
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !, we expect the next byte must be zero so remove it
                        i++;
                        buffer.RemoveAt(i);
                        i--;
                    }
                    OnProgress("Decoding unsynchronisation ...", (i * 100) / buffer.Count);
                }
            }
            // get the frames
            frames = new List<ID3TagFrame>();
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer.Count - i < 6) break;// this can't be anything but padding or currepted frame.
                //get header
                byte[] header = buffer.GetRange(i, 6).ToArray();
                i += 6;
                if (header[0] == 0)
                {
                    // this is padding, get the size
                    paddingSize = (int)(6 + (buffer.Count - i));
                    break;
                }
                else
                {
                    // get id, size and flags
                    string frameId = encoding.GetString(new byte[] { header[0], header[1], header[2] });
                    int frameSize = SynchsafeConvertor.FromBytes(new byte[] { header[3], header[4], header[5] });
                    // get data
                    if (frameSize == 0)
                    {
                        // currepted frame or padding ? break anyway
                        break;
                    }
                    byte[] data = buffer.GetRange(i, frameSize).ToArray();
                    i += frameSize;
                    i--;// we need this since we are in a 'for' loop
                    // detect frame and add to the list
                    ID3TagFrame frame = FramesManager.GetFrameVersion2(frameId, data);
                    frame.Load(version);
                    frames.Add(frame);
                }
                OnProgress("Reading ID3 Tag data ...", (i * 100) / buffer.Count);
            }
            OnLoadFinshed();
            return Result.Success;
        }
        private Result LoadDataVersion3(Stream stream)
        {
            extendedHeader = new ExtendedHeader();
            byte[] tagBuffer = new byte[tagSize];
            stream.Position = 10;
            long index = 10;
            // extended header
            if (extenderHeaderPresented = ((tagFlags & 0x40) == 0x40))
            {
                extendedHeader.Read(stream, version);
                index += extendedHeader.Size;// skip header
            }
            // Experimental.
            experimental = ((tagFlags & 0x20) == 0x20);
            // Now get the buffer ...
            stream.Position = index;
            stream.Read(tagBuffer, 0, tagSize);
            List<byte> buffer = new List<byte>(tagBuffer);
            stream.Close();
            stream.Dispose();
            // "Unsynchronisation" used ?
            if (unsynchronisation = ((tagFlags & 0x80) == 0x80))
            {
                // search for a false synchronisation
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !, we expect the next byte must be zero so remove it
                        i++;
                        buffer.RemoveAt(i);
                        i--;
                    }
                    OnProgress("Decoding unsynchronisation ...", (i * 100) / buffer.Count);
                }
            }
            // get the frames
            frames = new List<ID3TagFrame>();
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer.Count - i < 10) break;// this can't be anything but padding or currepted frame.
                //get header
                byte[] header = buffer.GetRange(i, 10).ToArray();
                i += 10;
                if (header[0] == 0)
                {
                    // this is padding, get the size
                    paddingSize = (int)(10 + (buffer.Count - i));
                    break;
                }
                else
                {
                    // get id, size and flags
                    string frameId = encoding.GetString(new byte[] { header[0], header[1], header[2], header[3], });
                    int frameSize = SynchsafeConvertor.FromBytes(new byte[] { header[4], header[5], header[6], header[7], });
                    int frameFlags = (header[8] << 8) | header[9];
                    // get data
                    if (frameSize == 0 || i + frameSize > buffer.Count)
                    {
                        // currepted frame or padding ? break anyway
                        break;
                    }
                    byte[] data = buffer.GetRange(i, frameSize).ToArray();
                    i += frameSize;
                    i--;// we need this since we are in a 'for' loop
                    // detect frame and add to the list
                    // TODO: Read flags and use them for compression and encryption
                    ID3TagFrame frame = FramesManager.GetFrameVersion3(frameId, data, frameFlags);
                    frame.Load(version);
                    frames.Add(frame);
                }
                OnProgress("Reading ID3 Tag data ...", (i * 100) / buffer.Count);
            }
            OnLoadFinshed();
            return Result.Success;
        }
        private Result LoadDataVersion4(Stream stream)
        {
            extendedHeader = new ExtendedHeader();
            byte[] tagBuffer = new byte[tagSize];
            stream.Position = 10;
            long index = 10;
            // extended header
            if (extenderHeaderPresented = ((tagFlags & 0x40) == 0x40))
            {
                extendedHeader.Read(stream, version);
                index += extendedHeader.Size;// skip header
            }
            // Experimental and Footer.
            experimental = ((tagFlags & 0x20) == 0x20);
            footer = ((tagFlags & 0x10) == 0x10);
            // Now get the buffer ...
            stream.Position = index;
            stream.Read(tagBuffer, 0, tagSize);
            List<byte> buffer = new List<byte>(tagBuffer);
            stream.Close();
            stream.Dispose();
            // "Unsynchronisation" used ?
            if (unsynchronisation = ((tagFlags & 0x80) == 0x80))
            {
                // search for a false synchronisation
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !, we expect the next byte must be zero so remove it
                        i++;
                        buffer.RemoveAt(i);
                        i--;
                    }
                    OnProgress("Decoding unsynchronisation ...", (i * 100) / buffer.Count);
                }
            }
            // get the frames
            frames = new List<ID3TagFrame>();
            for (int i = 0; i < buffer.Count; i++)
            {
                if (buffer.Count - i < 10) break;// this can't be anything but padding or currepted frame.

                //get header
                byte[] header = buffer.GetRange(i, 10).ToArray();
                i += 10;
                if (header[0] == 0)
                {
                    // this is padding, get the size
                    paddingSize = (int)(10 + (buffer.Count - i));
                    break;
                }
                else if (footer) // let's check if we reach the footer ...
                {
                    if (encoding.GetString(new byte[] { header[0], header[1], header[2], }) == "3DI")
                    {
                        // we got a footer !! just break lol
                        break;
                    }
                }
                else// normal frame
                {
                    // get id, size and flags
                    string frameId = encoding.GetString(new byte[] { header[0], header[1], header[2], header[3], });
                    int frameSize = SynchsafeConvertor.FromSynchsafe(new byte[] { header[4], header[5], header[6], header[7], });
                    int frameFlags = (header[8] << 8) | header[9];
                    // get data
                    if (frameSize == 0)
                    {
                        // currepted frame or padding ? break anyway
                        break;
                    }
                    byte[] data = buffer.GetRange(i, frameSize).ToArray();
                    i += frameSize;
                    i--;// we need this since we are in a 'for' loop
                    // detect frame and add to the list
                    ID3TagFrame frame = FramesManager.GetFrameVersion4(frameId, data, frameFlags);
                    frame.Load(version);
                    frames.Add(frame);
                }
                OnProgress("Reading ID3 Tag data ...", (i * 100) / buffer.Count);
            }
            OnLoadFinshed();
            return Result.Success;
        }

        /// <summary>
        /// Get the data buffer for ID3v2 version 2 including header and frames.
        /// </summary>
        /// <returns>The data buffer ready to write into file</returns>
        public byte[] GetVersion2Buffer()
        {
            List<byte> buffer = new List<byte>();
            //ID3
            buffer.AddRange(encoding.GetBytes("ID3"));
            //Version
            buffer.Add(version.Major);
            buffer.Add(version.Revision);
            //Flags
            tagFlags = 0;
            tagFlags |= (byte)(unsynchronisation ? 0x80 : 0);
            tagFlags |= (byte)(compression ? 0x40 : 0);
            buffer.Add(tagFlags);
            //Size, set 0 for now
            int size = 0;
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            //Frames
            foreach (ID3TagFrame frame in frames)
            {
                if (frame.CanSave)
                {
                    frame.Save(version);
                    size += 6 + frame.Size;

                    //ID
                    buffer.AddRange(encoding.GetBytes(frame.ID));
                    //Size
                    buffer.AddRange(SynchsafeConvertor.ToInt32Bytes(frame.Size, 3));
                    //Data
                    buffer.AddRange(frame.Data);
                }
            }
            //padding ? include if presented ...
            if (savePadding)
            {
                if (paddingSize > 0)
                    buffer.AddRange(new byte[paddingSize]);
                size += paddingSize;
            }
            //unsynchronisation ? TODO: detect the nesseccary usage of unsynchronisation
            if (unsynchronisation)
            {
                // search for a false synchronisation, insert 0 after the byte ..
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !!
                        i++;
                        buffer.Insert(i, 0);
                        size++;
                    }
                }
            }

            // now the size
            byte[] sizeBuffer = SynchsafeConvertor.ToSynchsafeBytes(size);
            buffer[6] = sizeBuffer[0];
            buffer[7] = sizeBuffer[1];
            buffer[8] = sizeBuffer[2];
            buffer[9] = sizeBuffer[3];

            return buffer.ToArray();
        }
        /// <summary>
        /// Get the data buffer for ID3v2 version 3 including header and frames.
        /// </summary>
        /// <returns>The data buffer ready to write into file</returns>
        public byte[] GetVersion3Buffer()
        {
            List<byte> buffer = new List<byte>();
            //ID3
            buffer.AddRange(encoding.GetBytes("ID3"));
            //Version
            buffer.Add(version.Major);
            buffer.Add(version.Revision);
            //Flags
            tagFlags = 0;
            tagFlags |= (byte)(extenderHeaderPresented ? 0x40 : 0);
            tagFlags |= (byte)(unsynchronisation ? 0x80 : 0);
            tagFlags |= (byte)(experimental ? 0x20 : 0);
            buffer.Add(tagFlags);
            //Size, set 0 for now
            int size = 0;
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            // Extended header ?
            if (extenderHeaderPresented)
            {
                extendedHeader.Save(buffer, version);
                size += extendedHeader.Size;
            }
            //Frames
            foreach (ID3TagFrame frame in frames)
            {
                if (frame.CanSave)
                {
                    frame.Save(version);
                    size += 10 + frame.Size;

                    //ID
                    buffer.AddRange(encoding.GetBytes(frame.ID));
                    //Size
                    buffer.AddRange(SynchsafeConvertor.ToInt32Bytes(frame.Size));
                    //Flags
                    buffer.Add((byte)((frame.Flags & 0xFF00) >> 8));
                    buffer.Add((byte)((frame.Flags & 0x00FF) >> 0));
                    //Data
                    buffer.AddRange(frame.Data);
                }
            }
            //padding ? include if presented ...
            if (savePadding)
            {
                if (paddingSize > 0)
                    buffer.AddRange(new byte[paddingSize]);
                size += paddingSize;
            }
            //unsynchronisation ? TODO: detect the nesseccary usage of unsynchronisation
            if (unsynchronisation)
            {
                // search for a false synchronisation, insert 0 after the byte ..
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !!
                        i++;
                        buffer.Insert(i, 0);
                        size++;
                    }
                }
            }

            // now the size
            byte[] sizeBuffer = SynchsafeConvertor.ToSynchsafeBytes(size);
            buffer[6] = sizeBuffer[0];
            buffer[7] = sizeBuffer[1];
            buffer[8] = sizeBuffer[2];
            buffer[9] = sizeBuffer[3];

            return buffer.ToArray();
        }
        /// <summary>
        /// Get the data buffer for ID3v2 version 4 including header and frames.
        /// </summary>
        /// <returns>The data buffer ready to write into file</returns>
        public byte[] GetVersion4Buffer()
        {
            List<byte> buffer = new List<byte>();
            //ID3
            buffer.AddRange(encoding.GetBytes("ID3"));
            //Version
            buffer.Add(version.Major);
            buffer.Add(version.Revision);
            //Flags
            tagFlags = 0;
            tagFlags |= (byte)(extenderHeaderPresented ? 0x40 : 0);
            tagFlags |= (byte)(unsynchronisation ? 0x80 : 0);
            tagFlags |= (byte)(experimental ? 0x20 : 0);
            tagFlags |= (byte)(footer ? 0x10 : 0);
            buffer.Add(tagFlags);
            //Size, set 0 for now
            int size = 0;
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            buffer.Add(0);
            // Extended header ?
            if (extenderHeaderPresented)
            {
                extendedHeader.Save(buffer, version);
                size += extendedHeader.Size;
            }
            //Frames
            foreach (ID3TagFrame frame in frames)
            {
                if (frame.CanSave)
                {
                    frame.Save(version);
                    size += 10 + frame.Size;

                    //ID
                    buffer.AddRange(encoding.GetBytes(frame.ID));
                    //Size
                    buffer.AddRange(SynchsafeConvertor.ToSynchsafeBytes(frame.Size));
                    //Flags
                    buffer.Add((byte)((frame.Flags & 0xFF00) >> 8));
                    buffer.Add((byte)((frame.Flags & 0x00FF) >> 0));
                    //Data
                    buffer.AddRange(frame.Data);
                }
            }
            //padding ? include if presented ...
            if (savePadding)
            {
                if (paddingSize > 0)
                    buffer.AddRange(new byte[paddingSize]);
                size += paddingSize;
            }
            // footer ?
            if (footer)
            {
                buffer.AddRange(encoding.GetBytes("3DI"));
                //Version
                buffer.Add(version.Major);
                buffer.Add(version.Revision);
                //Flags
                buffer.Add(tagFlags);
                //size... set 0 for now
                buffer.Add(0);
                buffer.Add(0);
                buffer.Add(0);
                buffer.Add(0);
            }
            //unsynchronisation ? TODO: detect the nesseccary usage of unsynchronisation
            if (unsynchronisation)
            {
                // search for a false synchronisation, insert 0 after the byte ..
                for (int i = 0; i < buffer.Count; i++)
                {
                    if (buffer[i] == 0xFF)
                    {
                        // this is it !!
                        i++;
                        buffer.Insert(i, 0);
                        size++;
                    }
                }
            }

            // now the size
            byte[] sizeBuffer = SynchsafeConvertor.ToSynchsafeBytes(size);
            buffer[6] = sizeBuffer[0];
            buffer[7] = sizeBuffer[1];
            buffer[8] = sizeBuffer[2];
            buffer[9] = sizeBuffer[3];
            if (footer)
            {
                buffer[buffer.Count - 3] = sizeBuffer[0];
                buffer[buffer.Count - 2] = sizeBuffer[1];
                buffer[buffer.Count - 1] = sizeBuffer[2];
                buffer[buffer.Count - 0] = sizeBuffer[3];
            }

            return buffer.ToArray();
        }

        //Properties
        /// <summary>
        /// Get or set the ID3 Tag frames collection.
        /// </summary>
        public List<ID3TagFrame> Frames
        { get { return frames; } set { frames = value; } }
        /// <summary>
        /// Get the tag size (after loading a file)
        /// </summary>
        public int TagSize
        { get { return tagSize; } }
        /// <summary>
        /// Get or set the padding size
        /// </summary>
        public int PaddingSize
        { get { return paddingSize; } set { paddingSize = value; } }
        /// <summary>
        /// Get or set if unsynchronisation is used. Version 2, 3 and 4
        /// </summary>
        public bool Unsynchronisation
        { get { return unsynchronisation; } set { unsynchronisation = value; } }
        /// <summary>
        /// Get or set if extended header is presented. Version 3 and 4
        /// </summary>
        public bool ExtendedHeader
        { get { return extenderHeaderPresented; } set { extenderHeaderPresented = value; } }
        /// <summary>
        /// Get or set the Experimental flag. Version 3 and 4
        /// </summary>
        public bool Experimental
        { get { return experimental; } set { experimental = value; } }
        /// <summary>
        /// Get or set if compression is used. Version 2 only.
        /// </summary>
        public bool Compression
        { get { return compression; } set { compression = value; } }
        /// <summary>
        /// Get or set if footer is presented. Version 4 only.
        /// </summary>
        public bool Footer
        { get { return footer; } set { footer = value; } }
        /// <summary>
        /// Get or set if should save the padding if presented on load.
        /// </summary>
        public bool SavePadding
        { get { return savePadding; } set { savePadding = value; } }
        /// <summary>
        /// Get or set the tag version
        /// </summary>
        public ID3Version TagVersion
        { get { return version; } set { version = value; } }
        /// <summary>
        /// Get last used file path (last file that loaded/save successfully)
        /// </summary>
        public string FileName
        {
            get { return fileName; }
        }

        //Events
        /// <summary>
        /// Rised when this class make a progress ..
        /// </summary>
        public event EventHandler<ProgressArg> Progress;
        /// <summary>
        /// Rised when the save operation started
        /// </summary>
        public event EventHandler SaveStart;
        /// <summary>
        /// Rised when the save operation fisnished
        /// </summary>
        public event EventHandler SaveFinished;
        /// <summary>
        /// Rised when the load operation started
        /// </summary>
        public event EventHandler LoadStart;
        /// <summary>
        /// Rised when the load operation started
        /// </summary>
        public event EventHandler LoadFinished;

        /// <summary>
        /// Rises the SaveStart event
        /// </summary>
        protected void OnSaveStart()
        {
            OnProgress("Saving ID3v2 ...", 0);
            if (SaveStart != null)
                SaveStart(this, new EventArgs());
        }
        /// <summary>
        /// Rises the LoadStart event 
        /// </summary>
        protected void OnLoadStart()
        {
            OnProgress("Loading ID3v2 ...", 0);
            if (LoadStart != null)
                LoadStart(this, new EventArgs());
        }
        /// <summary>
        /// Rises the SaveFinished event
        /// </summary>
        protected void OnSaveFinshed()
        {
            OnProgress("Done Saving ID3v2.", 100);
            if (SaveFinished != null)
                SaveFinished(this, new EventArgs());
        }
        /// <summary>
        /// Rises the LoadFinished event
        /// </summary>
        protected void OnLoadFinshed()
        {
            OnProgress("Done Loading ID3v2.", 100);
            if (LoadFinished != null)
                LoadFinished(this, new EventArgs());
        }
        /// <summary>
        /// Rises the Progress event.
        /// </summary>
        /// <param name="status">The status</param>
        /// <param name="progress">The progress value (in precdent)</param>
        protected void OnProgress(string status, int progress)
        {
            if (Progress != null)
                Progress(this, new ProgressArg(status, progress));
        }
    }
}
