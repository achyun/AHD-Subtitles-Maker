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

namespace AHD.SM.Formats
{
    class AHDCustomizedTimingFormatter
    {
        /* ! FORMATTING !
         * 
         * h-m-s-i-fxx-sfxx-n
         * 
         * -: a splitter. can be ':' , '-' , '.' , ';' , ','
         * 
         * h: means hour. ranged between 0-23. Can be hh or h. 
         *    h is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc)
         *    hh means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)
         * 
         * m: means minute. ranged between 0-59. Can be mm or m. 
         *    m is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc)
         *    mm means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)
         * 
         * s: means second. ranged between 0-59. Can be ss or s. 
         *    s is one digit if value < 10 (e.i 1=1, 2=2, 23=23 ...etc)
         *    ss means the entry is always 2 digits (e.i. 0=00, 1=01 ...etc)
         * 
         * i: milliseconds. ranged between 0-999. Can be i, ii or iii
         *    i is one digit (e.i. 4=400, 5=500) 
         *    ii is 2 digits (e.i. 4=040, 5=050, 45=450 ..etc) 
         *    iii always 3 digits (e.i. 1=001, 2=002, 10=010, 244=244 ...etc)
         *  
         * fxx: frame while xx is the framerate. (e.i. f25, f29_97 ...etc) each digit represents one frame.
         *      NOTE: for frame rate, use '_' instead of '.' . Example: for 29.97 FPS use 'f29_97' not 'f29.97' . 
         * 
         * sfxx: sub frame while xx is the frame rate. Each digit represents one frame range.
         *       sfxx is one digit 
         *       sffxx is 2 digits
         * 
         * n: means no decode. Return the second value as it is.
         *    n is absolute second. (i.e. 1, 2, 44, 5334 ....etc)
         *    nn second.milli one digit for milli (e.i. 1.4=1.400, 1.5=1.500) 
         *    nnn second.milli 2 digits for milli (e.i. 1.04=1.040, 1.05=050, 1.45=1.450 ..etc) 
         *    nnnn second.milli 3 digits for milli (e.i. 1.001, 1.002, 1.010, 1.244 ...etc) 
         */
        /// <summary>
        /// The splitter that can be used in the formatting (';', ':', '-', ',', '.').
        /// </summary>
        public static readonly char[] Splitters = { ';', ':', '-', ',', '.' };

        private static string ParseCode(string c, double v)
        {
            // Get value times
            if (c.StartsWith("sff"))
            {
                // SubFrame 2 digits
                double frameRate = 0;
                if (!double.TryParse(c.Substring(3, c.Length - 3).Replace('_', '.'), out frameRate))
                    throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + c + "'");
                // Get the milliseconds
                double mi = ((v * 1000) % 1000);
                double subF = (mi * frameRate) / 1000;
                return ((int)subF).ToString("D2");
            }
            else if (c.StartsWith("sf"))
            {
                // SubFrame 1 digits
                double frameRate = 0;
                if (!double.TryParse(c.Substring(2, c.Length - 2).Replace('_', '.'), out frameRate))
                    throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + c + "'");

                // Get the milliseconds
                double mi = ((v * 1000) % 1000);
                double subF = (mi * frameRate) / 1000;
                return ((int)subF).ToString("D1");
            }
            else if (c.StartsWith("f"))
            {
                // Frame
                double frameRate = 0;
                if (!double.TryParse(c.Substring(1, c.Length - 1).Replace('_', '.'), out frameRate))
                    throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + c + "'");

                double Frames = (v * frameRate);
                return Math.Round(Frames).ToString("F0");
            }
            TimeSpan t = TimeSpan.FromSeconds(v);
            // Reached here means not a frame mode.
            switch (c)
            {
                case "n":
                    {
                        return v.ToString("F0");
                    }
                case "nn":
                    {
                        return v.ToString("F1");
                    }
                case "nnn":
                    {
                        return v.ToString("F2");
                    }
                case "nnnn":
                    {
                        return v.ToString("F3");
                    }
                case "h":
                    {
                        // Just add hours 1 digit mode
                        return t.Hours.ToString("D1");
                    }
                case "hh":
                    {
                        // Just add hours 2 digits mode
                        return t.Hours.ToString("D2");
                    }
                case "s":
                    {
                        // Just add seconds 1 digit mode
                        return t.Seconds.ToString("D1");
                    }
                case "ss":
                    {
                        // Just add seconds 2 digits mode
                        return t.Seconds.ToString("D2");
                    }
                case "m":
                    {
                        // Just add minutes 1 digit mode
                        return t.Minutes.ToString("D1");
                    }
                case "mm":
                    {
                        // Just add minutes 2 digits mode
                        return t.Minutes.ToString("D2");
                    }
                case "i":
                    {
                        // Just add milliseconds 1 digit mode
                        return t.Milliseconds.ToString("D3").Substring(0, 1);
                    }
                case "ii":
                    {
                        // Just add milliseconds 2 digits mode
                        return t.Milliseconds.ToString("D3").Substring(0, 2);
                    }
                case "iii":
                    {
                        // Just add milliseconds 3 digits mode
                        return t.Milliseconds.ToString("D3").Substring(0, 3);
                    }
                default: throw new Exception(Properties.Resources.Status_UnableToParseTimingFormatInvalidParameter);
            }
        }

        /// <summary>
        /// Convert seconds to timing format
        /// </summary>
        /// <param name="value">The value to convert (in seconds.milliseconds)</param>
        /// <param name="format">The format to use</param>
        /// <returns>Timing string</returns>
        public static string SecondsToTime(double value, string format)
        {
            // Check
            if (format.Length == 0)
                throw new Exception(Properties.Resources.Status_FormatCantBeEmpty);

            string time = "";

            // Parse
            List<char> splitters = new List<char>(Splitters);// using list help for search
            string code = "";
            for (int i = 0; i < format.Length; i++)
            {
                if (splitters.Contains(format[i]))
                {
                    // We reach one of the splitters; update timeing
                    time += ParseCode(code, value);
                    // Add the splitter
                    time += format[i];
                    // Reset code
                    code = "";
                }
                else
                {
                    code += format[i];
                }
            }
            // Reached here and the code is not empty
            if (code.Length > 0)
            {
                time += ParseCode(code, value);
            }
            return time;
        }
        /// <summary>
        /// Convert timing value to seconds
        /// </summary>
        /// <param name="value">The value to convert</param>
        /// <param name="format">The format to use.</param>
        /// <returns>Time in seconds.milliseconds form</returns>
        public static double TimeToSeconds(string value, string format)
        {
            double time = 0;
            if (format == "n" || format == "nn" || format == "nnn" || format == "nnnn")
            {
                double.TryParse(value, out time);
                return time;
            }
            // Get codes
            string[] formatSplit = format.Split(Splitters);
            string[] valueSplit = value.Split(Splitters);
            // Check
            if (formatSplit.Length != valueSplit.Length)
                throw new Exception(Properties.Resources.Status_GivenValueDoesntMatchTheGivenFormat);
            for (int i = 0; i < formatSplit.Length; i++)
            {
                int v = 0;
                if (!int.TryParse(valueSplit[i], out v))
                    throw new Exception(Properties.Resources.Status_ErrorInGivenValueUnableToCastIntegerFrom + " '" + valueSplit[i] + "'");
                if (formatSplit[i].ToLower().StartsWith("sff"))
                {
                    // SubFrame 2 digits
                    double frameRate = 0;
                    if (!double.TryParse(formatSplit[i].Substring(3, formatSplit[i].Length - 3).Replace('_', '.'), out frameRate))
                        throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + formatSplit[i] + "'");

                    double mili = (int)((1000 * v) / frameRate);
                    mili /= 1000;
                    time += mili;
                }
                else if (formatSplit[i].ToLower().StartsWith("sf"))
                {
                    // SubFrame 2 digits
                    double frameRate = 0;
                    if (!double.TryParse(formatSplit[i].Substring(2, formatSplit[i].Length - 2).Replace('_', '.'), out frameRate))
                        throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + formatSplit[i] + "'");

                    double mili = (int)((1000 * v) / frameRate);
                    mili /= 1000;
                    time += mili;
                }
                else if (formatSplit[i].ToLower().StartsWith("f"))
                {
                    // Frame
                    double frameRate = 0;
                    if (!double.TryParse(formatSplit[i].Substring(1, formatSplit[i].Length - 1).Replace('_', '.'), out frameRate))
                        throw new Exception(Properties.Resources.Status_ErrorInGivenFormatNotValidFramerate + " '" + formatSplit[i] + "'");

                    time += ((v / frameRate));
                }
                else
                {
                    switch (formatSplit[i].ToLower())
                    {
                        case "h":
                        case "hh":
                        case "hhh":
                            {
                                // Add hours
                                time += v * 3600;
                                break;
                            }
                        case "m":
                        case "mm":
                        case "mmm":
                            {
                                // Add minutes
                                time += v * 60;
                                break;
                            }
                        case "s":
                        case "ss":
                        case "sss":
                            {
                                // Add seconds
                                time += v;
                                break;
                            }
                        case "i":
                            {
                                if (!int.TryParse(valueSplit[i] + "00", out v))
                                    throw new Exception(Properties.Resources.Status_ErrorInGivenValueUnableToCastIntegerFrom + " '" + valueSplit[i] + "'");
                                // Add milliseconds
                                time += (double)v / 1000;
                                break;
                            }
                        case "ii":
                            {
                                if (!int.TryParse(valueSplit[i] + "0", out v))
                                    throw new Exception(Properties.Resources.Status_ErrorInGivenValueUnableToCastIntegerFrom + " '" + valueSplit[i] + "'");
                                // Add milliseconds
                                time += (double)v / 1000;
                                break;
                            }
                        case "iii":
                            {
                                // Add milliseconds
                                time += (double)v / 1000;
                                break;
                            }
                    }
                }
            }
            return time;
        }
    }
}
