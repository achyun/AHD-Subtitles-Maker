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
using System.Reflection;
using System.Collections.Generic;
using AHD.ID3.Frames;
using AHD.ID3.Types;
namespace AHD.ID3
{
    /// <summary>
    /// The class that used to manage frames
    /// </summary>
    public class FramesManager
    {
        private static Dictionary<string, ID3TagFrame> version2Frames = new Dictionary<string, ID3TagFrame>();
        private static Dictionary<string, ID3TagFrame> version3Frames = new Dictionary<string, ID3TagFrame>();
        private static Dictionary<string, ID3TagFrame> version4Frames = new Dictionary<string, ID3TagFrame>();

        /// <summary>
        /// Install frames into this manager to help detect them later.
        /// </summary>
        public static void InstallFrames()
        {
            #region Version 2
            version2Frames = new Dictionary<string, ID3TagFrame>();
            #region Text Frames
            version2Frames.Add("TT1", new TextFrame("TT1", "Content group description", null, 0));
            version2Frames.Add("TT2", new TextFrame("TT2", "Title/Songname/Content description", null, 0));
            version2Frames.Add("TT3", new TextFrame("TT3", "Subtitle/Description refinement", null, 0));
            version2Frames.Add("TP1", new TextFrame("TP1", "Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group", null, 0));
            version2Frames.Add("TP2", new TextFrame("TP2", "Band/Orchestra/Accompaniment", null, 0));
            version2Frames.Add("TP3", new TextFrame("TP3", "Conductor", null, 0));
            version2Frames.Add("TP4", new TextFrame("TP4", "Interpreted, remixed, or otherwise modified by", null, 0));
            version2Frames.Add("TCM", new TextFrame("TCM", "Composer(s)", null, 0));
            version2Frames.Add("TXT", new TextFrame("TXT", "Lyricist(s)/text writer(s)", null, 0));
            version2Frames.Add("TLA", new TextFrame("TLA", "Language(s)", null, 0));
            version2Frames.Add("TCO", new TextFrame("TCO", "Content type", null, 0));
            version2Frames.Add("TAL", new TextFrame("TAL", "Album/Movie/Show title", null, 0));
            version2Frames.Add("TPA", new TextFrame("TPA", "Part of a set", null, 0));
            version2Frames.Add("TRK", new TextFrame("TRK", "Track number/Position in set", null, 0));
            version2Frames.Add("TRC", new TextFrame("TRC", "International Standard Recording Code [ISRC].", null, 0));
            version2Frames.Add("TYE", new TextFrame("TYE", "Year", null, 0));
            version2Frames.Add("TDA", new TextFrame("TDA", "Date", null, 0));
            version2Frames.Add("TIM", new TextFrame("TIM", "Time", null, 0));
            version2Frames.Add("TRD", new TextFrame("TRD", "Recording dates", null, 0));
            version2Frames.Add("TMT", new TextFrame("TMT", "Media type", null, 0));
            version2Frames.Add("TFT", new TextFrame("TFT", "File type", null, 0));
            version2Frames.Add("TBP", new TextFrame("TBP", "Beats per minute", null, 0));
            version2Frames.Add("TCR", new TextFrame("TCR", "Copyright message", null, 0));
            version2Frames.Add("TPB", new TextFrame("TPB", "Publisher", null, 0));
            version2Frames.Add("TEN", new TextFrame("TEN", "Encoded by", null, 0));
            version2Frames.Add("TSS", new TextFrame("TSS", "Software/hardware and settings used for encoding", null, 0));
            version2Frames.Add("TOF", new TextFrame("TOF", "Original filename", null, 0));
            version2Frames.Add("TLE", new TextFrame("TLE", "Length", null, 0));
            version2Frames.Add("TDY", new TextFrame("TDY", "Playlist delay", null, 0));
            version2Frames.Add("TKE", new TextFrame("TKE", "Initial key", null, 0));
            version2Frames.Add("TOT", new TextFrame("TOT", "Original album/Movie/Show title", null, 0));
            version2Frames.Add("TOA", new TextFrame("TOA", "Original artist(s)/performer(s)", null, 0));
            version2Frames.Add("TOL", new TextFrame("TOL", "Original Lyricist(s)/text writer(s)", null, 0));
            version2Frames.Add("TOR", new TextFrame("TOR", "Original release year", null, 0));
            #endregion
            #region URL link frames
            version2Frames.Add("WCM", new URLLinkFrame("WCM", "Commercial information", null, 0));
            version2Frames.Add("WCP", new URLLinkFrame("WCP", "Copyright/Legal information", null, 0));
            version2Frames.Add("WAF", new URLLinkFrame("WAF", "Official audio file webpage", null, 0));
            version2Frames.Add("WAR", new URLLinkFrame("WAR", "Official artist/performer webpage", null, 0));
            version2Frames.Add("WAS", new URLLinkFrame("WAS", "Official audio source webpage", null, 0));
            version2Frames.Add("WPB", new URLLinkFrame("WPB", "Publishers official webpage", null, 0));
            #endregion
            version2Frames.Add("UFI", new UniqueFileIdentifierFrame("UFI", "Unique file identifier", null, 0));
            version2Frames.Add("TXX", new UserDefinedTextInformationFrame("TXX", "User defined text information", null, 0));
            version2Frames.Add("WXX", new UserDefinedURLLinkFrame("WXX", "User defined URL link", null, 0));
            version2Frames.Add("IPL", new InvolvedPeopleListFrame("IPL", "Involved people list", null, 0));
            version2Frames.Add("MCI", new MusicCDIdentifierFrame("MCI", "Music CD Identifier", null, 0));
            version2Frames.Add("ETC", new EventTimingCodesFrame("ETC", "Event timing codes", null, 0));
            version2Frames.Add("ULT", new UnsychronisedLyricsFrame("ULT", "Unsychronised lyrics/text transcription", null, 0));
            version2Frames.Add("SLT", new SynchronisedLyricsFrame("SLT", "Sychronised lyrics/text", null, 0));
            version2Frames.Add("COM", new CommentsFrame("COM", "Comments", null, 0));
            version2Frames.Add("PIC", new AttachedPictureFrame("PIC", "Attached picture frame", null, 0));
            version2Frames.Add("GEO", new GeneralEncapsulatedObjectFrame("GEO", "General encapsulated object", null, 0));
            version2Frames.Add("CNT", new PlayCounterFrame("CNT", "Play counter", null, 0));
            version2Frames.Add("POP", new PopularimeterFrame("POP", "Popularimeter", null, 0));
            #endregion
            #region Version 3
            version3Frames = new Dictionary<string, ID3TagFrame>();
            #region Text Frames
            version3Frames.Add("TALB", new TextFrame("TALB", "Album/Movie/Show title", null, 0));
            version3Frames.Add("TBPM", new TextFrame("TBPM", "Beats per minute", null, 0));
            version3Frames.Add("TCOM", new TextFrame("TCOM", "Composer(s)", null, 0));
            version3Frames.Add("TCON", new TextFrame("TCON", "Content type", null, 0));
            version3Frames.Add("TCOP", new TextFrame("TCOP", "Copyright message", null, 0));
            version3Frames.Add("TDAT", new TextFrame("TDAT", "Date", null, 0));
            version3Frames.Add("TDLY", new TextFrame("TDLY", "Playlist delay", null, 0));
            version3Frames.Add("TENC", new TextFrame("TENC", "Encoded by", null, 0));
            version3Frames.Add("TEXT", new TextFrame("TEXT", "Lyricist(s)/Text writer(s)", null, 0));
            version3Frames.Add("TFLT", new TextFrame("TFLT", "File type", null, 0));
            version3Frames.Add("TIME", new TextFrame("TIME", "Time", null, 0));
            version3Frames.Add("TIT1", new TextFrame("TIT1", "Content group description", null, 0));
            version3Frames.Add("TIT2", new TextFrame("TIT2", "Title/Songname/Content description", null, 0));
            version3Frames.Add("TIT3", new TextFrame("TIT3", "Subtitle/Description refinement", null, 0));
            version3Frames.Add("TKEY", new TextFrame("TKEY", "Initial key", null, 0));
            version3Frames.Add("TLAN", new TextFrame("TLAN", "Language(s)", null, 0));
            version3Frames.Add("TLEN", new TextFrame("TLEN", "Length", null, 0));
            version3Frames.Add("TMED", new TextFrame("TMED", "Media type", null, 0));
            version3Frames.Add("TOAL", new TextFrame("TOAL", "Original album/movie/show title", null, 0));
            version3Frames.Add("TOFN", new TextFrame("TOFN", "Original filename", null, 0));
            version3Frames.Add("TOLY", new TextFrame("TOLY", "Original lyricist(s)/text writer(s)", null, 0));
            version3Frames.Add("TOPE", new TextFrame("TOPE", "Original artist(s)/performer(s)", null, 0));
            version3Frames.Add("TORY", new TextFrame("TORY", "Original release year", null, 0));
            version3Frames.Add("TOWN", new TextFrame("TOWN", "File owner/licensee", null, 0));
            version3Frames.Add("TPE1", new TextFrame("TPE1", "Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group", null, 0));
            version3Frames.Add("TPE2", new TextFrame("TPE2", "Band/Orchestra/Accompaniment", null, 0));
            version3Frames.Add("TPE3", new TextFrame("TPE3", "BConductor", null, 0));
            version3Frames.Add("TPE4", new TextFrame("TPE4", "Interpreted, remixed, or otherwise modified by", null, 0));
            version3Frames.Add("TPOS", new TextFrame("TPOS", "Part of a set", null, 0));
            version3Frames.Add("TPUB", new TextFrame("TPUB", "Publisher", null, 0));
            version3Frames.Add("TRCK", new TextFrame("TRCK", "Track number/Position in set", null, 0));
            version3Frames.Add("TRDA", new TextFrame("TRDA", "Recording dates", null, 0));
            version3Frames.Add("TRSN", new TextFrame("TRSN", "Internet radio station name", null, 0));
            version3Frames.Add("TRSO", new TextFrame("TRSO", "Internet radio station owner", null, 0));
            version3Frames.Add("TSIZ", new TextFrame("TSIZ", "Size", null, 0));
            version3Frames.Add("TSRC", new TextFrame("TSRC", "International Standard Recording Code (ISRC)", null, 0));
            version3Frames.Add("TSSE", new TextFrame("TSSE", "Software/Hardware and settings used for encoding", null, 0));
            version3Frames.Add("TYER", new TextFrame("TYER", "Year", null, 0));
            #endregion
            #region URL link frames
            version3Frames.Add("WCOM", new URLLinkFrame("WCOM", "Commercial information", null, 0));
            version3Frames.Add("WCOP", new URLLinkFrame("WCOP", "Copyright/Legal information", null, 0));
            version3Frames.Add("WOAF", new URLLinkFrame("WOAF", "Official audio file webpage", null, 0));
            version3Frames.Add("WOAR", new URLLinkFrame("WOAR", "Official artist/performer webpage", null, 0));
            version3Frames.Add("WOAS", new URLLinkFrame("WOAS", "Official audio source webpage", null, 0));
            version3Frames.Add("WORS", new URLLinkFrame("WORS", "Official internet radio station homepage", null, 0));
            version3Frames.Add("WPAY", new URLLinkFrame("WPAY", "Payment", null, 0));
            version3Frames.Add("WPUB", new URLLinkFrame("WPUB", "Publishers official webpage", null, 0));
            #endregion
            version3Frames.Add("UFID", new UniqueFileIdentifierFrame("UFID", "Unique file identifier", null, 0));
            version3Frames.Add("TXXX", new UserDefinedTextInformationFrame("TXXX", "User defined text information", null, 0));
            version3Frames.Add("WXXX", new UserDefinedURLLinkFrame("WXXX", "User defined URL link", null, 0));
            version3Frames.Add("IPLS", new InvolvedPeopleListFrame("IPLS", "Involved people list", null, 0));
            version3Frames.Add("MCDI", new MusicCDIdentifierFrame("MCDI", "Music CD Identifier", null, 0));
            version3Frames.Add("ETCO", new EventTimingCodesFrame("ETCO", "Event timing codes", null, 0));
            version3Frames.Add("USLT", new UnsychronisedLyricsFrame("USLT", "Unsychronised lyrics/text transcription", null, 0));
            version3Frames.Add("SYLT", new SynchronisedLyricsFrame("SYLT", "Sychronised lyrics/text", null, 0));
            version3Frames.Add("COMM", new CommentsFrame("COMM", "Comments", null, 0));
            version3Frames.Add("APIC", new AttachedPictureFrame("APIC", "Attached picture frame", null, 0));
            version3Frames.Add("GEOB", new GeneralEncapsulatedObjectFrame("GEOB", "General encapsulated object", null, 0));
            version3Frames.Add("PCNT", new PlayCounterFrame("PCNT", "Play counter", null, 0));
            version3Frames.Add("POPM", new PopularimeterFrame("POPM", "Popularimeter", null, 0));
            version3Frames.Add("USER", new TermsOfUseFrame("USER", "Terms of use frame", null, 0));
            version3Frames.Add("COMR", new CommercialFrame("COMR", "Commercial", null, 0));
            #endregion
            #region Version 4
            version4Frames = new Dictionary<string, ID3TagFrame>();
            #region Text Frames
            version4Frames.Add("TALB", new TextFrame("TALB", "Album/Movie/Show title", null, 0));
            version4Frames.Add("TBPM", new TextFrame("TBPM", "Beats per minute", null, 0));
            version4Frames.Add("TCOM", new TextFrame("TCOM", "Composer(s)", null, 0));
            version4Frames.Add("TCON", new TextFrame("TCON", "Content type", null, 0));
            version4Frames.Add("TCOP", new TextFrame("TCOP", "Copyright message", null, 0));
            version4Frames.Add("TDLY", new TextFrame("TDLY", "Playlist delay", null, 0));
            version4Frames.Add("TDEN", new TextFrame("TDEN", "Encoding time", null, 0));
            version4Frames.Add("TDOR", new TextFrame("TDOR", "Original release time", null, 0));
            version4Frames.Add("TDRC", new TextFrame("TDRC", "Recording time", null, 0));
            version4Frames.Add("TDRL", new TextFrame("TDRL", "Release time", null, 0));
            version4Frames.Add("TDTG", new TextFrame("TDTG", "Tagging time", null, 0));
            version4Frames.Add("TENC", new TextFrame("TENC", "Encoded by", null, 0));
            version4Frames.Add("TEXT", new TextFrame("TEXT", "Lyricist(s)/Text writer(s)", null, 0));
            version4Frames.Add("TFLT", new TextFrame("TFLT", "File type", null, 0));
            version4Frames.Add("TIT1", new TextFrame("TIT1", "Content group description", null, 0));
            version4Frames.Add("TIT2", new TextFrame("TIT2", "Title/Songname/Content description", null, 0));
            version4Frames.Add("TIT3", new TextFrame("TIT3", "Subtitle/Description refinement", null, 0));
            version4Frames.Add("TKEY", new TextFrame("TKEY", "Initial key", null, 0));
            version4Frames.Add("TLAN", new TextFrame("TLAN", "Language(s)", null, 0));
            version4Frames.Add("TLEN", new TextFrame("TLEN", "Length", null, 0));
            version4Frames.Add("TMED", new TextFrame("TMED", "Media type", null, 0));
            version4Frames.Add("TMCL", new TextFrame("TMCL", "Musician credits list", null, 0));
            version4Frames.Add("TMOO", new TextFrame("TMOO", "Mood", null, 0));
            version4Frames.Add("TOAL", new TextFrame("TOAL", "Original album/movie/show title", null, 0));
            version4Frames.Add("TOFN", new TextFrame("TOFN", "Original filename", null, 0));
            version4Frames.Add("TOLY", new TextFrame("TOLY", "Original lyricist(s)/text writer(s)", null, 0));
            version4Frames.Add("TOPE", new TextFrame("TOPE", "Original artist(s)/performer(s)", null, 0));
            version4Frames.Add("TOWN", new TextFrame("TOWN", "File owner/licensee", null, 0));
            version4Frames.Add("TPE1", new TextFrame("TPE1", "Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group", null, 0));
            version4Frames.Add("TPE2", new TextFrame("TPE2", "Band/Orchestra/Accompaniment", null, 0));
            version4Frames.Add("TPE3", new TextFrame("TPE3", "Conductor", null, 0));
            version4Frames.Add("TPE4", new TextFrame("TPE4", "Interpreted, remixed, or otherwise modified by", null, 0));
            version4Frames.Add("TPOS", new TextFrame("TPOS", "Part of a set", null, 0));
            version4Frames.Add("TPRO", new TextFrame("TPRO", "Produced notice", null, 0));
            version4Frames.Add("TPUB", new TextFrame("TPUB", "Publisher", null, 0));
            version4Frames.Add("TRCK", new TextFrame("TRCK", "Track number/Position in set", null, 0));
            version4Frames.Add("TRSN", new TextFrame("TRSN", "Internet radio station name", null, 0));
            version4Frames.Add("TRSO", new TextFrame("TRSO", "Internet radio station owner", null, 0));
            version4Frames.Add("TSRC", new TextFrame("TSRC", "International Standard Recording Code (ISRC)", null, 0));
            version4Frames.Add("TSSE", new TextFrame("TSSE", "Software/Hardware and settings used for encoding", null, 0));
            version4Frames.Add("TSOA", new TextFrame("TSOA", "Album sort order", null, 0));
            version4Frames.Add("TSOP", new TextFrame("TSOP", "Performer sort order", null, 0));
            version4Frames.Add("TSOT", new TextFrame("TSOT", "Title sort order", null, 0));
            version4Frames.Add("TSST", new TextFrame("TSST", "Set subtitle", null, 0));
            version4Frames.Add("TIPL", new TextFrame("TIPL", "Involved people list", null, 0));
            #endregion
            #region URL link frames
            version4Frames.Add("WCOM", new URLLinkFrame("WCOM", "Commercial information", null, 0));
            version4Frames.Add("WCOP", new URLLinkFrame("WCOP", "Copyright/Legal information", null, 0));
            version4Frames.Add("WOAF", new URLLinkFrame("WOAF", "Official audio file webpage", null, 0));
            version4Frames.Add("WOAR", new URLLinkFrame("WOAR", "Official artist/performer webpage", null, 0));
            version4Frames.Add("WOAS", new URLLinkFrame("WOAS", "Official audio source webpage", null, 0));
            version4Frames.Add("WORS", new URLLinkFrame("WORS", "Official internet radio station homepage", null, 0));
            version4Frames.Add("WPAY", new URLLinkFrame("WPAY", "Payment", null, 0));
            version4Frames.Add("WPUB", new URLLinkFrame("WPUB", "Publishers official webpage", null, 0));
            #endregion
            version4Frames.Add("UFID", new UniqueFileIdentifierFrame("UFID", "Unique file identifier", null, 0));
            version4Frames.Add("TXXX", new UserDefinedTextInformationFrame("TXXX", "User defined text information", null, 0));
            version4Frames.Add("WXXX", new UserDefinedURLLinkFrame("WXXX", "User defined URL link", null, 0));
            version4Frames.Add("MCDI", new MusicCDIdentifierFrame("MCDI", "Music CD Identifier", null, 0));
            version4Frames.Add("ETCO", new EventTimingCodesFrame("ETCO", "Event timing codes", null, 0));
            version4Frames.Add("USLT", new UnsychronisedLyricsFrame("USLT", "Unsychronised lyrics/text transcription", null, 0));
            version4Frames.Add("SYLT", new SynchronisedLyricsFrame("SYLT", "Sychronised lyrics/text", null, 0));
            version4Frames.Add("COMM", new CommentsFrame("COMM", "Comments", null, 0));
            version4Frames.Add("APIC", new AttachedPictureFrame("APIC", "Attached picture frame", null, 0));
            version4Frames.Add("GEOB", new GeneralEncapsulatedObjectFrame("GEOB", "General encapsulated object", null, 0));
            version4Frames.Add("PCNT", new PlayCounterFrame("PCNT", "Play counter", null, 0));
            version4Frames.Add("POPM", new PopularimeterFrame("POPM", "Popularimeter", null, 0));
            version4Frames.Add("USER", new TermsOfUseFrame("USER", "Terms of use frame", null, 0));
            version4Frames.Add("COMR", new CommercialFrame("COMR", "Commercial", null, 0));
            #endregion
        }
        /// <summary>
        /// Detect a frame using id, returns UnknownFrame if the frame not recognized
        /// </summary>
        /// <param name="version">The tag version</param>
        /// <param name="id">The frame id</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrame(ID3Version version, string id, byte[] data, int flags)
        {
            switch (version.Major)
            {
                case 2: return GetFrameVersion2(id, data);
                case 3: return GetFrameVersion3(id, data, flags);
                case 4: return GetFrameVersion4(id, data, flags);
                default: return new UnknownFrame("Unknown", id, "Unknown or not supported", data, flags);
            }
        }
        /// <summary>
        /// Detect a frame using id, returns UnknownFrame if the frame not recognized
        /// </summary>
        /// <param name="version">The tag version</param>
        /// <param name="id">The frame id</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrame(ID3Version version, string id)
        {
            return GetFrame(version, id, null, 0);
        }
        /// <summary>
        /// Detect a frame using class type, returns UnknownFrame if the frame not recognized. 
        /// (Warning: use this only for frames that appear once in the tag like involved people list frame or 
        /// if you want to create new instance of a frame)
        /// </summary>
        /// <param name="version">The tag version</param>
        /// <param name="frameType">The type of the frame.</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrame(ID3Version version, Type frameType, byte[] data, int flags)
        {
            switch (version.Major)
            {
                case 2: return GetFrameVersion2(frameType, data);
                case 3: return GetFrameVersion3(frameType, data, flags);
                case 4: return GetFrameVersion4(frameType, data, flags);
                default: return new UnknownFrame("Unknown", "XXXX", "Unknown or not supported", data, flags);
            }
        }
        /// <summary>
        /// Detect a frame using class type, returns UnknownFrame if the frame not recognized. 
        /// (Warning: use this only for frames that appear once in the tag like involved people list frame or 
        /// if you want to create new instance of a frame)
        /// </summary>
        /// <param name="version">The tag version</param>
        /// <param name="frameType">The type of the frame.</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrame(ID3Version version, Type frameType)
        {
            return GetFrame(version, frameType, null, 0);
        }
        /// <summary>
        /// Detect a frame for version 2, returns UnknownFrame if the frame not recognized
        /// </summary>
        /// <param name="id">The id of the frame</param>
        /// <param name="data">The frame data</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion2(string id, byte[] data)
        {
            if (version2Frames.ContainsKey(id))
            {
                string name = version2Frames[id].Name;
                string ID = version2Frames[id].ID;
                return Activator.CreateInstance(version2Frames[id].GetType(),
                    new object[] { ID, name, data, 0 }) as ID3TagFrame;
            }
            return new UnknownFrame(id, "Unknown", "Unknown or not supported", data, 0);
        }
        /// <summary>
        /// Detect a version 2 frame using class type, returns UnknownFrame if the frame not recognized. 
        /// (Warning: use this only for frames that appear once in the tag like involved people list frame or 
        /// if you want to create new instance of a frame)
        /// </summary>
        /// <param name="frameType">The frame type</param>
        /// <param name="data">The data that will be used in creation</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion2(Type frameType, byte[] data)
        {
            foreach (ID3TagFrame frame in version2Frames.Values)
            {
                if (frame.GetType() == frameType)
                {
                    return Activator.CreateInstance(version2Frames[frame.ID].GetType(),
                     new object[] { frame.ID, frame.Name, data, 0 }) as ID3TagFrame;
                }
            }
            return new UnknownFrame("XXX", "Unknown", "Unknown or not supported", data, 0);
        }
        /// <summary>
        /// Detect a frame for version 3, returns UnknownFrame if the frame not recognized
        /// </summary>
        /// <param name="id">The id of the frame</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion3(string id, byte[] data, int flags)
        {
            if (version3Frames.ContainsKey(id))
            {
                string name = version3Frames[id].Name;
                string ID = version3Frames[id].ID;
                return Activator.CreateInstance(version3Frames[id].GetType(),
                    new object[] { ID, name, data, flags }) as ID3TagFrame;
            }
            return new UnknownFrame(id, "Unknown", "Unknown or not supported", data, flags);
        }
        /// <summary>
        /// Detect a version 3 frame using class type, returns UnknownFrame if the frame not recognized. 
        /// (Warning: use this only for frames that appear once in the tag like involved people list frame or 
        /// if you want to create new instance of a frame)
        /// </summary>
        /// <param name="frameType">The frame type</param>
        /// <param name="data">The data that will be used in creation</param>
        /// <param name="flags">The frame flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion3(Type frameType, byte[] data, int flags)
        {
            foreach (ID3TagFrame frame in version3Frames.Values)
            {
                if (frame.GetType() == frameType)
                {
                    return Activator.CreateInstance(version3Frames[frame.ID].GetType(),
                     new object[] { frame.ID, frame.Name, data, flags }) as ID3TagFrame;
                }
            }
            return new UnknownFrame("XXXX", "Unknown", "Unknown or not supported", data, flags);
        }
        /// <summary>
        /// Detect a frame for version 4, returns UnknownFrame if the frame not recognized
        /// </summary>
        /// <param name="id">The id of the frame</param>
        /// <param name="data">The frame data</param>
        /// <param name="flags">The frame flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion4(string id, byte[] data, int flags)
        {
            if (version4Frames.ContainsKey(id))
            {
                string name = version4Frames[id].Name;
                string ID = version4Frames[id].ID;
                return Activator.CreateInstance(version4Frames[id].GetType(),
                    new object[] { ID, name, data, flags }) as ID3TagFrame;
            }
            return new UnknownFrame(id, "Unknown", "Unknown or not supported", data, flags);
        }
        /// <summary>
        /// Detect a version 4 frame using class type, returns UnknownFrame if the frame not recognized. 
        /// (Warning: use this only for frames that appear once in the tag like involved people list frame or 
        /// if you want to create new instance of a frame)
        /// </summary>
        /// <param name="frameType">The frame type</param>
        /// <param name="data">The data that will be used in creation</param>
        /// <param name="flags">The frame flags</param>
        /// <returns>The frame if found otherwise returns the UnknownFrame.</returns>
        public static ID3TagFrame GetFrameVersion4(Type frameType, byte[] data, int flags)
        {
            foreach (ID3TagFrame frame in version4Frames.Values)
            {
                if (frame.GetType() == frameType)
                {
                    return Activator.CreateInstance(version4Frames[frame.ID].GetType(),
                     new object[] { frame.ID, frame.Name, data, flags }) as ID3TagFrame;
                }
            }
            return new UnknownFrame("XXXX", "Unknown", "Unknown or not supported", data, flags);
        }

        /// <summary>
        /// Get the supported frames collection for ID3 Tag version 2.
        /// </summary>
        public static Dictionary<string, ID3TagFrame> FramesV2
        { get { return version2Frames; } }
        /// <summary>
        /// Get the supported frames collection for ID3 Tag version 3.
        /// </summary>
        public static Dictionary<string, ID3TagFrame> FramesV3
        { get { return version3Frames; } }
        /// <summary>
        /// Get the supported frames collection for ID3 Tag version 4.
        /// </summary>
        public static Dictionary<string, ID3TagFrame> FramesV4
        { get { return version4Frames; } }
    }
}
