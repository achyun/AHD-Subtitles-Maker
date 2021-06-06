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
using AHD.ID3.Frames;

namespace AHD.ID3
{
    /// <summary>
    /// The ID3v2 wrapper. Useful to get useful text frames quickly.
    /// </summary>
    public class ID3v2QuickWrapper
    {
        /// <summary>
        /// The ID3v2 wrapper. Useful to get useful text frames quickly.
        /// </summary>
        /// <param name="id3v2">The ID3 Tag version 2 object</param>
        public ID3v2QuickWrapper(ID3v2 id3v2)
        {
            this.id3v2 = id3v2;
        }
        private ID3v2 id3v2;

        /// <summary>
        /// Get or set the song title
        /// </summary>
        public string Title
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TIT2");
                else
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TT2");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TIT2");
                    else
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TT2");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TT2", "Title/Songname/Content description", null, 0);
                        else
                            Tframe = new TextFrame("TIT2", "Title/Songname/Content description", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TIT2");
                    else
                        id3v2.RemoveFrame("TT2");
                }
            }
        }
        /// <summary>
        /// Get or set the artist
        /// </summary>
        public string Artist
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TPE1");
                else
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TP1");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TPE1");
                    else
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TP1");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TP1", "Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group", null, 0);
                        else
                            Tframe = new TextFrame("TPE1", "Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TPE1");
                    else
                        id3v2.RemoveFrame("TP1");
                }
            }
        }
        /// <summary>
        /// Get or set the album
        /// </summary>
        public string Album
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TALB");
                else
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TAL");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TALB");
                    else
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TAL");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TAL", "Album/Movie/Show title", null, 0);
                        else
                            Tframe = new TextFrame("TALB", "Album/Movie/Show title", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TALB");
                    else
                        id3v2.RemoveFrame("TAL");
                }
            }
        }
        /// <summary>
        /// Get or set the genre
        /// </summary>
        public string Genre
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TCON");
                else
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TCO");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TCON");
                    else
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TCO");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TCO", "Content type", null, 0);
                        else
                            Tframe = new TextFrame("TCON", "Content type", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TCON");
                    else
                        id3v2.RemoveFrame("TCO");
                }
            }
        }
        /// <summary>
        /// Get or set the track
        /// </summary>
        public string Track
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TRCK");
                else
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TRK");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TRCK");
                    else
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TRK");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TRK", "Track number/Position in set", null, 0);
                        else
                            Tframe = new TextFrame("TRCK", "Track number/Position in set", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TRCK");
                    else
                        id3v2.RemoveFrame("TRK");
                }
            }
        }
        /// <summary>
        /// Get or set the year (the value must be in yyyy format)
        /// </summary>
        public string Year
        {
            get
            {
                TextFrame Tframe = null;
                if (id3v2.TagVersion.Major == 3)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TYER");
                else if (id3v2.TagVersion.Major == 2)
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TYE");
                else if (id3v2.TagVersion.Major == 4)// use Release time for v4
                    Tframe = (TextFrame)id3v2.GetFrameLoaded("TDRL");

                if (Tframe != null)
                    return Tframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    TextFrame Tframe = null;
                    if (id3v2.TagVersion.Major ==3 )
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TYER");
                    else if (id3v2.TagVersion.Major == 2)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TYE");
                    if (id3v2.TagVersion.Major == 4)
                        Tframe = (TextFrame)id3v2.GetFrameLoaded("TDRL");

                    if (Tframe != null)
                        Tframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Tframe = new TextFrame("TYE", "Year", null, 0);
                        else if (id3v2.TagVersion.Major == 3)
                            Tframe = new TextFrame("TYER", "Year", null, 0);
                        else if (id3v2.TagVersion.Major == 4)
                            Tframe = new TextFrame("TDRL", "Release time", null, 0);
                        Tframe.Text = value;
                        id3v2.Frames.Add(Tframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("TYER");
                    else
                        id3v2.RemoveFrame("TYE");
                }
            }
        }
        /// <summary>
        /// Get or set the comment
        /// </summary>
        public string Comment
        {
            get
            {
                CommentsFrame Cframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Cframe = (CommentsFrame)id3v2.GetFrameLoaded("COMM");
                else
                    Cframe = (CommentsFrame)id3v2.GetFrameLoaded("COM");

                if (Cframe != null)
                    return Cframe.Text;
                else
                    return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    CommentsFrame Cframe = null;
                    if (id3v2.TagVersion.Major > 2)
                        Cframe = (CommentsFrame)id3v2.GetFrameLoaded("COMM");
                    else
                        Cframe = (CommentsFrame)id3v2.GetFrameLoaded("COM");

                    if (Cframe != null)
                        Cframe.Text = value;
                    else
                    {
                        if (id3v2.TagVersion.Major == 2)
                            Cframe = new CommentsFrame("COM", "Comments", null, 0);
                        else
                            Cframe = new CommentsFrame("COMM", "Comments", null, 0);
                        Cframe.Text = value;
                        Cframe.LanguageID = "ENG";
                        id3v2.Frames.Add(Cframe);
                    }
                }
                else
                {
                    if (id3v2.TagVersion.Major > 2)
                        id3v2.RemoveFrame("COMM");
                    else
                        id3v2.RemoveFrame("COM");
                }
            }
        }
        /// <summary>
        /// Get or set the rating
        /// </summary>
        public byte Rating
        {
            get
            {
                PopularimeterFrame Pframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Pframe = (PopularimeterFrame)id3v2.GetFrameLoaded("POPM");
                else
                    Pframe = (PopularimeterFrame)id3v2.GetFrameLoaded("POP");

                if (Pframe != null)
                    return Pframe.Rating;
                else
                    return 0;
            }
            set
            {
                PopularimeterFrame Pframe = null;
                if (id3v2.TagVersion.Major > 2)
                    Pframe = (PopularimeterFrame)id3v2.GetFrameLoaded("POPM");
                else
                    Pframe = (PopularimeterFrame)id3v2.GetFrameLoaded("POP");

                if (Pframe != null)
                    Pframe.Rating = value;
                else
                {
                    if (id3v2.TagVersion.Major == 2)
                        Pframe = new PopularimeterFrame("POP", "Popularimeter", null, 0);
                    else
                        Pframe = new PopularimeterFrame("POPM", "Popularimeter", null, 0);
                    Pframe.Rating = value;
                    Pframe.Counter = 0;
                    id3v2.Frames.Add(Pframe);
                }
            }
        }
    }
}
