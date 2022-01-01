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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace AHD.SM
{
    [Serializable()]
    public class LayoutPresent
    {
        /*Default layout should take these values*/
        public int SplitterDistance1 = 756;
        public int SplitterDistance2 = 296;
        public int SplitterDistance3 = 296;
        public int SplitterDistance4 = 420;
        public int SplitterDistance5 = 420;

        public TabProperties Tab_Media = new TabProperties(TabParent.TopLeft, true);
        public TabProperties Tab_SubtitlesData = new TabProperties(TabParent.Top, true);
        public TabProperties Tab_MultipleSubtitleTracksViewer = new TabProperties(TabParent.DownRight, true);
        public TabProperties Tab_SubtitleTracks = new TabProperties(TabParent.TopRight, true);
        public TabProperties Tab_ProjectDescription = new TabProperties(TabParent.TopRight, true);
        public TabProperties Tab_History = new TabProperties(TabParent.TopRight, true);
        public TabProperties Tab_TimeLine = new TabProperties(TabParent.DownLeft, true);
        public TabProperties Tab_Errors = new TabProperties(TabParent.DownLeft, true);
        public TabProperties Tab_Log = new TabProperties(TabParent.DownLeft, true);
        public TabProperties Tab_Properties = new TabProperties(TabParent.DownRight, true);
        public TabProperties Tab_PreparedText = new TabProperties(TabParent.DownRight, true);

        public ToolbarProperties Bar_Main = new ToolbarProperties(ToolbarParent.Top, new Point(0, 0), true);
        public ToolbarProperties Bar_Edit = new ToolbarProperties(ToolbarParent.Top, new Point(0, 25), true);
        public ToolbarProperties Bar_Marks = new ToolbarProperties(ToolbarParent.Top, new Point(226, 25), true);
    }
    public enum TabParent
    {
        None,
        TopLeft, Top, TopRight,
        DownLeft, Down, DownRight
    }
}
