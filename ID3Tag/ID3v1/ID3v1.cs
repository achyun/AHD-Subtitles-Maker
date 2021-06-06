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
namespace AHD.ID3
{
    /// <summary>
    /// The class of ID3 Tag v1, handles read and write.
    /// </summary>
    public class ID3v1
    {
        private const int bufferChunk = 0x100000;// 1 MB
        private Encoding encoding = new ASCIIEncoding();
        private string songTitle = "";
        private string artist = "";
        private string album = "";
        private string year = "2013";
        private string comment = "";
        private byte track = 0;
        private byte genre = 0;
        private static string[] genres =
		{
			"Blues","Classic Rock","Country","Dance","Disco","Funk","Grunge","Hip-Hop","Jazz","Metal",
			"New Age","Oldies","Other","Pop","R&B","Rap","Reggae","Rock","Techno","Industrial",
			"Alternative","Ska","Death Metal","Pranks","Soundtrack","Euro-Techno","Ambient","Trip-Hop",
			"Vocal","Jazz+Funk","Fusion","Trance","Classical","Instrumental","Acid","House",
			"Game","Sound Clip","Gospel","Noise","Alternative Rock","Bass","Soul","Punk","Space",
			"Meditative","Instrumental Pop","Instrumental Rock","Ethnic","Gothic",
			"Darkwave","Techno-Industrial","Electronic","Pop-Folk","Eurodance","Dream",
			"Southern Rock","Comedy","Cult","Gangsta","Top 40","Christian Rap","Pop/Funk","Jungle",
			"Native American","Cabaret","New Wave","Psychadelic","Rave","Showtunes","Trailer","Lo-Fi",
			"Tribal","Acid Punk","Acid Jazz","Polka","Retro","Musical","Rock & Roll","Hard Rock","Folk",
			"Folk/Rock","National Folk","Swing","Fast-Fusion","Bebob","Latin","Revival","Celtic","Bluegrass",
			"Avantgarde","Gothic Rock","Progressive Rock","Psychedelic Rock","Symphonic Rock","Slow Rock",
			"Big Band","Chorus","Easy Listening","Acoustic","Humour","Speech","Chanson","Opera","Chamber Music",
			"Sonata","Symphony","Booty Bass","Primus","Porn Groove","Satire","Slow Jam","Club",
			"Tango","Samba","Folklore","Ballad","Power Ballad","Rhytmic Soul","Freestyle","Duet",
			"Punk Rock","Drum Solo","Acapella","Euro-House","Dance Hall","Goa","Drum & Bass","Club-House",
			"Hardcore","Terror","Indie","BritPop","Negerpunk","Polsk Punk","Beat","Christian Gangsta Rap",
			"Heavy Metal","Black Metal","Crossover","Contemporary Christian",
			"Christian Rock","Merengue","Salsa","Trash Metal","Anime","JPop","SynthPop"
		};

        /// <summary>
        /// Load mp3 file and read ID3 Tag version 1 if exist.
        /// </summary>
        /// <param name="fileName">The full path of the file (file must be existed)</param>
        /// <returns>Result of the read operation</returns>
        public Result Load(string fileName)
        {
            OnLoadStart();
            Stream stream = null;
            try
            {
                // 1 create the stream
                stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                // 2 read the last 128 byte
                if (stream.Length < 128)
                {
                    stream.Close();
                    stream.Dispose();
                    OnLoadFinshed();
                    return Result.Failed;
                }
                byte[] buffer = new byte[128];
                stream.Position = stream.Length - 128;
                stream.Read(buffer, 0, 128);
                stream.Close();
                stream.Dispose();
                // 3 is it for ID3 Tag version 1 ?
                if (encoding.GetString(new byte[] { buffer[0], buffer[1], buffer[2], }) != "TAG")
                {
                    OnLoadFinshed();
                    return Result.NoID3Exist;
                }
                // 4 get info
                List<byte> listedBuffer = new List<byte>(buffer);
                int index = 3;
                // title
                songTitle = encoding.GetString(listedBuffer.GetRange(index, 30).ToArray()); index += 30;
                // artist
                artist = encoding.GetString(listedBuffer.GetRange(index, 30).ToArray()); index += 30;
                // album
                album = encoding.GetString(listedBuffer.GetRange(index, 30).ToArray()); index += 30;
                // year
                year = encoding.GetString(listedBuffer.GetRange(index, 4).ToArray()); index += 4;
                // comment and track
                comment = encoding.GetString(listedBuffer.GetRange(index, 28).ToArray()); index += 28;
                if ((listedBuffer[index] == 0) && (listedBuffer[index + 1] != 0))
                {
                    track = listedBuffer[index + 1];
                }
                else
                {
                    comment += encoding.GetString(listedBuffer.GetRange(index, 2).ToArray());
                }
                index += 2;
                // genre
                genre = listedBuffer[index];
                // done !
                OnLoadFinshed();
                return Result.Success;
            }
            catch (Exception ex)
            {
                DebugLogger.WriteLine("Unable to load id3v1 for file:", DebugCode.Error);
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
        /// Save ID3v1 information to a file
        /// </summary>
        /// <param name="fileName">The full path of the file</param>
        /// <returns>Result of the save operation</returns>
        public Result Save(string fileName)
        {
            OnSaveStart();
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
                fileStream.Position = tempStream.Position = 0;
                // copy the original file data to the temp file without id3
                long max = Check(fileName) ? fileStream.Length - 128 : fileStream.Length;
                byte[] buff = new byte[bufferChunk];
                // We can't just read file as one byte buffer and write it directly into temp, what if the file is too large ?
                // so do it chunck by chunck to limit the buffer memory to bufferChunk value....
                // larger bufferChunk value make saving goes faster but need more memory.
                // however, bufferChunk = 1 MB for now.
                for (long i = 0; i < max; i += bufferChunk)
                {
                    if (max - i >= bufferChunk)
                    {
                        buff = new byte[bufferChunk];
                        fileStream.Read(buff, 0, bufferChunk);
                        tempStream.Write(buff, 0, bufferChunk);
                    }
                    else
                    {
                        buff = new byte[max - i];
                        fileStream.Read(buff, 0, (int)(max - i));
                        tempStream.Write(buff, 0, (int)(max - i));
                    }
                    OnProgress("Writing ID3v1 data ...", (int)((i * 100) / max));
                }
                // write the ID3v1 data to temp
                tempStream.Write(CreateBuffer(), 0, 128);
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete original then make temp as original 
                FileInfo inf = new FileInfo(fileName);
                inf.IsReadOnly = false;
                File.Delete(fileName);
                File.Copy(tempName, fileName);
                File.Delete(tempName);

                // done ! 
                OnSaveFinshed();
                return Result.Success;
            }
            catch
            {
                // close streams
                fileStream.Close();
                fileStream.Dispose();
                tempStream.Close();
                tempStream.Dispose();
                // delete temp if found
                if (File.Exists(tempName))
                    File.Delete(tempName);
                OnSaveFinshed();
                return Result.Failed;
            }
        }
        /// <summary>
        /// Create the ID3Tag 128 bytes buffer using the properties
        /// </summary>
        /// <returns>The ID3Tag buffer ready to write</returns>
        public byte[] CreateBuffer()
        {
            // create dynamic buffer
            List<byte> buffer = new List<byte>();
            //header
            buffer.AddRange(encoding.GetBytes("TAG"));
            //Title
            if (songTitle.Length > 30)
                songTitle = songTitle.Substring(0, 30);
            else
                for (int i = songTitle.Length; i < 30; i++)
                    songTitle += "\0";
            buffer.AddRange(encoding.GetBytes(songTitle));
            //Artist
            if (artist.Length > 30)
                artist = artist.Substring(0, 30);
            else
                for (int i = artist.Length; i < 30; i++)
                    artist += "\0";
            buffer.AddRange(encoding.GetBytes(artist));
            //Album
            if (album.Length > 30)
                album = album.Substring(0, 30);
            else
                for (int i = album.Length; i < 30; i++)
                    album += "\0";
            buffer.AddRange(encoding.GetBytes(album));
            //Year
            if (year.Length != 4)
                year = "2012";
            buffer.AddRange(encoding.GetBytes(year));
            //Comment
            if (comment.Length > 28)
                comment = comment.Substring(0, 28);
            for (int i = comment.Length; i < 29; i++)
                comment += "\0";
            buffer.AddRange(encoding.GetBytes(comment));
            //Track
            buffer.Add(track);
            //Genre
            buffer.Add(genre);

            return buffer.ToArray();
        }
        /// <summary>
        /// Check if a file has ID3v1
        /// </summary>
        /// <param name="fileName">The full mp3 file path</param>
        /// <returns>True if given file has ID3v1, otherwise false</returns>
        public static bool Check(string fileName)
        {
            try
            {
                Stream str = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (str.Length < 128)
                    return false;
                str.Position = str.Length - 128;
                byte[] header = new byte[3];
                str.Read(header, 0, 3);
                str.Dispose();
                str.Close();
                ASCIIEncoding enc = new ASCIIEncoding();
                string tag = enc.GetString(header);
                if (enc.GetString(header) == "TAG")
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                DebugLogger.WriteLine("Unable to check id3v1 for file:", DebugCode.Error);
                DebugLogger.WriteLine(fileName, DebugCode.Error);
                DebugLogger.WriteLine(ex.Message, DebugCode.Error);
            }
            return false;
        }

        /// <summary>
        /// Clear a file (erase) from ID3v1
        /// </summary>
        /// <param name="fileName">The complete file path (file MUST exist)</param>
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
                fileStream.Position = tempStream.Position = 0;
                // copy the original file data to the temp file without id3
                long max = Check(fileName) ? fileStream.Length - 128 : fileStream.Length;
                byte[] buff = new byte[bufferChunk];
                // We can't just read file as one byte buffer and write it directly into temp, what if the file is too large ?
                // so do it chunck by chunck to limit the buffer memory to bufferChunk value....
                // larger bufferChunk value make saving goes faster but need more memory.
                // however, bufferChunk = 1 MB for now.
                for (long i = 0; i < max; i += bufferChunk)
                {
                    if (max - i >= bufferChunk)
                    {
                        buff = new byte[bufferChunk];
                        fileStream.Read(buff, 0, bufferChunk);
                        tempStream.Write(buff, 0, bufferChunk);
                    }
                    else
                    {
                        buff = new byte[max - i];
                        fileStream.Read(buff, 0, (int)(max - i));
                        tempStream.Write(buff, 0, (int)(max - i));
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
                File.Copy(tempName, fileName);
                File.Delete(tempName);
            }
            catch
            {
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

        //Properties
        /// <summary>
        /// Get or set the song title
        /// </summary>
        public string Title { get { return songTitle; } set { songTitle = value; } }
        /// <summary>
        /// Get or set the artist
        /// </summary>
        public string Artist { get { return artist; } set { artist = value; } }
        /// <summary>
        /// Get or set the album
        /// </summary>
        public string Album { get { return album; } set { album = value; } }
        /// <summary>
        /// Get or set the year
        /// </summary>
        public string Year { get { return year; } set { year = value; } }
        /// <summary>
        /// Get or set the comment
        /// </summary>
        public string Comment { get { return comment; } set { comment = value; } }
        /// <summary>
        /// Get or set the track
        /// </summary>
        public byte Track { get { return track; } set { track = value; } }
        /// <summary>
        /// Get or set the genre
        /// </summary>
        public string Genre
        {
            get
            {
                if (genre < genres.Length)
                    return genres[genre];
                else
                    return "";
            }
            set
            {
                genre = 0;
                for (byte i = 0; i < genres.Length; i++)
                {
                    if (value == genres[i])
                    {
                        genre = i;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// Get the genres list
        /// </summary>
        public static string[] GenreList { get { return genres; } }

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
            OnProgress("Saving ID3v1 ...", 0);
            if (SaveStart != null)
                SaveStart(this, new EventArgs());
        }
        /// <summary>
        /// Rises the LoadStart event 
        /// </summary>
        protected void OnLoadStart()
        {
            OnProgress("Loading ID3v1 ...", 0);
            if (LoadStart != null)
                LoadStart(this, new EventArgs());
        }
        /// <summary>
        /// Rises the SaveFinished event
        /// </summary>
        protected void OnSaveFinshed()
        {
            OnProgress("Done Saving ID3v1.", 100);
            if (SaveFinished != null)
                SaveFinished(this, new EventArgs());
        }
        /// <summary>
        /// Rises the LoadFinished event
        /// </summary>
        protected void OnLoadFinshed()
        {
            OnProgress("Done Loading ID3v1.", 100);
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
