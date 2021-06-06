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
namespace AHD.SM.ASMP.SpellCheck
{
    public struct DictionaryLink
    {
        public DictionaryLink(string name, string nativeName, string link, string description)
        {
            this.name = name;
            this.nativeName = nativeName;
            this.link = link;
            this.description = description;
        }
        private string link;
        private string name;
        private string nativeName;
        private string description;

        public string Name
        { get { return name; } }
        public string Description
        { get { return description; } }
        public string NativeName
        { get { return nativeName; } }
        public string Link
        { get { return link; } }

        public override string ToString()
        {
            return name + " - " + nativeName;
        }
    }
}
