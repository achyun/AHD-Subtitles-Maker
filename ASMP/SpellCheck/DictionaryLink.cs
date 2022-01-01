// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
