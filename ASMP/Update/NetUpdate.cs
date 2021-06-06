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
using System.Net;
using System.IO;
using System.Xml.Serialization;

namespace AHD.SM.ASMP
{
    [Serializable()]
    public class NetUpdate
    {
        public List<ASMUpdate> Updates = new List<ASMUpdate>();
    }
    public struct UpdateCheckResult
    {
        public NetUpdate Updates { get; set; }
        /// <summary>
        /// Set to false when at least one update is detected (update version is newer than current version of asm)
        /// </summary>
        public bool IsUpToDate { get; set; }
        public ASMUpdate LatestUpdateFound { get; set; }
        public static UpdateCheckResult Empty
        {
            get
            {
                UpdateCheckResult result = new UpdateCheckResult();
                result.IsUpToDate = true;
                return result;
            }
        }
    }
    public class UpdateChecker
    {
        const string downloadLink = "";
        public static UpdateCheckResult CheckForUpdate(Version asmVersion)
        {
            return CheckForUpdate(asmVersion, false, "", 0);
        }
        public static UpdateCheckResult CheckForUpdate(Version asmVersion, bool useproxy, string proxyhost, int proxyport)
        {
            try
            {
                UpdateCheckResult result = new UpdateCheckResult();

                // Download the file
                WebClient webClient = new WebClient();
                if (useproxy)
                {
                    webClient.Proxy = new WebProxy(proxyhost, proxyport);
                }
                webClient.DownloadFile(downloadLink, Path.GetTempPath() + "\\update.xml");
                webClient.Dispose();
                // Deserialize the xml data
                XmlSerializer ser = new XmlSerializer(typeof(NetUpdate));
                FileStream str = new FileStream(Path.GetTempPath() + "\\update.xml", FileMode.Open, FileAccess.Read);

                result.IsUpToDate = true;// Set to true for now
                result.Updates = (NetUpdate)ser.Deserialize(str);
                str.Close();

                // See if there is an update
                foreach (ASMUpdate update in result.Updates.Updates)
                {
                    if (update.IsForASM)
                    {
                        Version newVersion = new Version(update.Version);
                        if (newVersion > asmVersion)
                        {
                            result.LatestUpdateFound = update;
                            result.IsUpToDate = false;
                            break;
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                // TODO: exception handling when checking for updates online
                // MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_CantConnectToServer") + "\n\n" + ex.ToString(),
                // Program.ResourceManager.GetString("Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                // richTextBox1.Text = Program.ResourceManager.GetString("Message_ConnectionError");
            }
            return UpdateCheckResult.Empty;
        }
    }
}
