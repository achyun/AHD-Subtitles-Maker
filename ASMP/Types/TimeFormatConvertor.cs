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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHD.SM.ASMP
{
    /// <summary>
    /// Class for converting time format
    /// </summary>
    public class TimeFormatConvertor
    {
        /// <summary>
        /// Convert time from double (sec.milli) to timespan format (00:00:00)
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <returns>timespan format (00:00:00)</returns>
        public static string To_TimeSpan(double time)
        { return TimeSpan.FromSeconds(time).ToString().Substring(0, 8); }
        /// <summary>
        /// Convert timespan format (00:00:00) to double (sec.milli) format
        /// </summary>
        /// <param name="time">Time in timespan format (00:00:00)</param>
        /// <returns>double (sec.milli) time format</returns>
        public static double From_TimeSpan(string time)
        {
            string[] ent = time.Split(new char[] { ':', ';', '.' }, StringSplitOptions.RemoveEmptyEntries);
            int H = Convert.ToInt32(ent[0].Replace(" ", ""));
            int M = Convert.ToInt32(ent[1].Replace(" ", ""));
            int S = Convert.ToInt32(ent[2].Replace(" ", ""));
            return (H * 3600) + (M * 60) + S ;
        }

        /// <summary>
        /// Convert time from double (sec.milli) to timespan.milli format (00:00:00.000)
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <returns>timespan.milli format (00:00:00.000)</returns>
        public static string To_TimeSpan_Milli(double time)
        { return To_TimeSpan_Milli(time, ".", MillisecondLength.N3); }
        /// <summary>
        /// Convert time from double (sec.milli) to timespan.milli format (00:00:00.000)
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <param name="spliter">The spliter char that placed between timepans and milliseconds (Timespam'spliter'milli)</param>
        /// <param name="N">The millisecond length that should be taken</param>
        /// <returns>timespan'spliter'milli format (00:00:00'spliter'000)</returns>
        public static string To_TimeSpan_Milli(double time, string spliter, MillisecondLength N)
        {
            string returnValue = TimeSpan.FromSeconds(time).ToString().Substring(0,8);
            int milli = TimeSpan.FromSeconds(time).Milliseconds;
            switch (N)
            {
                case MillisecondLength.N1: returnValue += spliter + milli.ToString("D3").Substring(0, 1); break;
                case MillisecondLength.N2: returnValue += spliter + milli.ToString("D3").Substring(0, 2); break;
                case MillisecondLength.N3: returnValue += spliter + milli.ToString("D3").Substring(0, 3); break;
            }
            return returnValue;
        }
        /// <summary>
        /// Convert timespan.milli format (00:00:00.000) to double (sec.milli) format
        /// </summary>
        /// <param name="time">Time in timespan.milli format (00:00:00.000)</param>
        /// <returns>double (sec.milli) time format</returns>
        public static double From_TimeSpan_Milli(string time)
        { return From_TimeSpan_Milli(time, MillisecondLength.N3); }
        /// <summary>
        /// Convert timespan.milli format (00:00:00.000) to double (sec.milli) format
        /// </summary>
        /// <param name="time">Time in timespan.milli format (00:00:00.000)</param>
        /// <param name="N">The millisecond length expected</param>
        /// <returns>double (sec.milli) time format</returns>
        public static double From_TimeSpan_Milli(string time, MillisecondLength N)
        {
            string[] ent = time.Split(new char[] { ':', ';', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            int H = Convert.ToInt32(ent[0].Replace(" ", ""));
            int M = Convert.ToInt32(ent[1].Replace(" ", ""));
            int S = Convert.ToInt32(ent[2].Replace(" ", ""));
            string milliText = "000";
            if (ent.Length == 4)
                milliText = ent[3].Replace(" ", "");
            if (milliText.Length > 3)
                milliText = milliText.Substring(0, 3);
            switch (N)
            {
                case MillisecondLength.N1: milliText += "00"; break;
                case MillisecondLength.N2: milliText += "0"; break;
                case MillisecondLength.N3: break;
            }
            double mili = Convert.ToDouble(milliText);
            mili /= 1000;
            return (H * 3600) + (M * 60) + S + mili;
        }

        /// <summary>
        /// Convert time from double (sec.milli) to timespan:frame format (00:00:00:00)
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <param name="frameRate">The none-zero framerate</param>
        /// <param name="spliter">: or ; or .</param>
        /// <returns>timespan:fame format (00:00:00:00)</returns>
        public static string To_TimeSpan_Frame(double time, double frameRate, string spliter)
        {
            string Tim = TimeSpan.FromSeconds(time).ToString();
            int MilliSec = TimeSpan.FromSeconds(time).Milliseconds;
            string Frame = ((int)(MilliSec * frameRate / 1000)).ToString();
            if (Frame.Length == 1)
            { Frame = "0" + Frame; }
            string HH = Tim.Substring(0, 2);
            string MM = Tim.Substring(3, 2);
            string SS = Tim.Substring(6, 2);
            return (HH + spliter + MM + spliter + SS
                + spliter + Frame);
        }
        /// <summary>
        /// Convert timespan:frame (00:00:00:00) to double (sec.milli) format
        /// </summary>
        /// <param name="time">Time in timespan:frame (00:00:00:00)</param>
        /// <param name="frameRate">The none-zero framerate</param>
        /// <returns>double (sec.milli) time format</returns>
        public static double From_TimeSpan_Frame(string time, double frameRate)
        {
            string[] ent = time.Split(new char[] { ':', ';', '.' }, StringSplitOptions.RemoveEmptyEntries);
            int H = Convert.ToInt32(ent[0].Replace(" ", ""));
            int M = Convert.ToInt32(ent[1].Replace(" ", ""));
            int S = Convert.ToInt32(ent[2].Replace(" ", ""));
            double mili = (int)((1000 * Convert.ToInt32(ent[3].Replace(" ", ""))) / frameRate);
            mili /= 1000;
            return (H * 3600) + (M * 60) + S + mili;
        }

        /// <summary>
        /// Convert time from double (sec.milli) to frame form
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <param name="frameRate">The none-zero framerate</param>
        /// <returns>frame form</returns>
        public static string To_Frame(double time, double frameRate)
        {
            return To_Frame(time, frameRate, "");
        }
        /// <summary>
        /// Convert time from double (sec.milli) to frame form
        /// </summary>
        /// <param name="time">double (sec.milli) time format</param>
        /// <param name="frameRate">The none-zero framerate</param>
        /// <param name="stringFormat">The string format that will be used in double.ToString(stringFormat) method</param>
        /// <returns>frame form</returns>
        public static string To_Frame(double time, double frameRate, string stringFormat)
        {
            double frames = (time * frameRate);
            return ((int)frames).ToString(stringFormat);
        }
        /// <summary>
        /// Convert frame form to double (sec.milli) format
        /// </summary>
        /// <param name="time">Time in frame form</param>
        /// <param name="frameRate">The none-zero framerate</param>
        /// <returns>double (sec.milli) time format</returns>
        public static double From_Frame(string time, double frameRate)
        {
            double theTime = double.Parse(time);
            return (theTime / frameRate);
        }
    }
    /// <summary>
    /// The milli length
    /// </summary>
    public enum MillisecondLength
    {
        /// <summary>
        /// 1 number length, 0.1
        /// </summary>
        N1,
        /// <summary>
        /// 2 numbers length, 0.11
        /// </summary>
        N2,
        /// <summary>
        /// 3 numbers length, 0.111
        /// </summary>
        N3
    }
}
