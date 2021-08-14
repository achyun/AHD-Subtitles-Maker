// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
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
using System.Windows.Forms;
using System.Reflection;
using System.Resources;

namespace AHD.Forms
{
    public class MessageDialog
    {
        static ResourceManager resources = new ResourceManager("AHD.Forms.LanguageResources.Resource",
              Assembly.GetExecutingAssembly());
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <param name="NoButtonText">Set the no button text, no button MUSt be enabled via MessageButtons parameter</param>
        /// <param name="CancelButtonText">Set the cancel button text, cancel button MUSt be enabled via MessageButtons parameter</param>
        /// <param name="CheckBoxText">Set the check box text, check box MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption,
            MessageDialogButtons MessageButtons, MessageDialogIcon MessageIcon, bool CheckBoxChecked,
            string OkButtonText, string NoButtonText, string CancelButtonText, string CheckBoxText)
        {
            Frm_MessageDialog frmMessage = new Frm_MessageDialog();
            //texts
            frmMessage.Text = WindowCaption;
            frmMessage.label1.Text = MessageText;
            frmMessage.button_cancel.Text = CancelButtonText;
            frmMessage.button_no.Text = NoButtonText;
            frmMessage.button_ok.Text = OkButtonText;
            frmMessage.checkBox1.Text = CheckBoxText;
            //enables
            frmMessage.button_no.Visible = (MessageButtons & MessageDialogButtons.OkNo) == MessageDialogButtons.OkNo;
            frmMessage.button_cancel.Visible = (MessageButtons & MessageDialogButtons.OkNoCancel) == MessageDialogButtons.OkNoCancel;
            frmMessage.checkBox1.Visible = (MessageButtons & MessageDialogButtons.Checked) == MessageDialogButtons.Checked;
            frmMessage.checkBox1.Checked = CheckBoxChecked;
            //Icon
            switch (MessageIcon)
            {
                case MessageDialogIcon.Error: frmMessage.label_icon.Image = Properties.Resources.Error; break;
                case MessageDialogIcon.Info: frmMessage.label_icon.Image = Properties.Resources.Info; break;
                case MessageDialogIcon.Question: frmMessage.label_icon.Image = Properties.Resources.Question; break;
                case MessageDialogIcon.Save: frmMessage.label_icon.Image = Properties.Resources.Save; break;
                case MessageDialogIcon.Warning: frmMessage.label_icon.Image = Properties.Resources.Warning; break;
            }
            //Result
            DialogResult windowResult;
            if (ParentWindow == null)
                windowResult = frmMessage.ShowDialog();
            else
                windowResult = frmMessage.ShowDialog(ParentWindow);

            MessageDialogResult result = MessageDialogResult.None;
            switch (windowResult)
            {
                case DialogResult.OK: result |= MessageDialogResult.Ok; break;
                case DialogResult.No: result |= MessageDialogResult.No; break;
                case DialogResult.Cancel: result |= MessageDialogResult.Cancel; break;
            }
            if (frmMessage.checkBox1.Checked)
                result |= MessageDialogResult.Checked;
            return result;
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageDialogButtons.Ok, MessageDialogIcon.Info, false,
                resources.GetString("Button_Ok"), "", "", "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption)
        {
            return ShowMessage(ParentWindow, MessageText, WindowCaption, MessageDialogButtons.Ok,
                MessageDialogIcon.Info, false, resources.GetString("Button_Ok"), "", "", "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageDialogIcon.Info, false,
                resources.GetString("Button_Ok"), resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons)
        {
            return ShowMessage(ParentWindow, MessageText, WindowCaption, MessageButtons, MessageDialogIcon.Info, false,
                resources.GetString("Button_Ok"), resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, false, resources.GetString("Button_Ok"),
               resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, false, resources.GetString("Button_Ok"),
             resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                resources.GetString("Button_Ok"), resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                resources.GetString("Button_Ok"), resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked, OkButtonText,
                      resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked, OkButtonText,
                resources.GetString("Button_No"), resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <param name="NoButtonText">Set the no button text, no button MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText, string NoButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                OkButtonText, NoButtonText, resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <param name="NoButtonText">Set the no button text, no button MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText, string NoButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                OkButtonText, NoButtonText, resources.GetString("Button_Cancel"), "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <param name="NoButtonText">Set the no button text, no button MUSt be enabled via MessageButtons parameter</param>
        /// <param name="CancelButtonText">Set the cancel button text, cancel button MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText, string NoButtonText, string CancelButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                OkButtonText, NoButtonText, CancelButtonText, "");
        }
        /// <summary>
        /// Show message box
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The message text</param>
        /// <param name="WindowCaption">The dialog caption</param>
        /// <param name="MessageButtons">The buttons you want to enable</param>
        /// <param name="MessageIcon">The icon refers to the message</param>
        /// <param name="CheckBoxChecked">Indecate if the check box is checked, check box MUSt be enabled via MessageButtons parameter</param>
        /// <param name="OkButtonText">Set the ok button text</param>
        /// <param name="NoButtonText">Set the no button text, no button MUSt be enabled via MessageButtons parameter</param>
        /// <param name="CancelButtonText">Set the cancel button text, cancel button MUSt be enabled via MessageButtons parameter</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowMessage(IWin32Window ParentWindow, string MessageText, string WindowCaption, MessageDialogButtons MessageButtons,
            MessageDialogIcon MessageIcon, bool CheckBoxChecked, string OkButtonText, string NoButtonText, string CancelButtonText)
        {
            return ShowMessage(null, MessageText, WindowCaption, MessageButtons, MessageIcon, CheckBoxChecked,
                OkButtonText, NoButtonText, CancelButtonText, "");
        }

        #region Question Message
        /// <summary>
        /// Show a message that asks the user a question. Only two buttons available, yes and no.
        /// </summary>
        /// <param name="MessageText">The question</param>
        /// <param name="WindowCaption">The message caption</param>
        /// <param name="YesButtonText">The text that represents "yes"</param>
        /// <param name="NoButtonText">The text that represents "no"</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowQuestionMessage(string MessageText,
          string WindowCaption, string YesButtonText, string NoButtonText)
        { return ShowQuestionMessage(null, MessageText, WindowCaption, YesButtonText, NoButtonText); }
        /// <summary>
        /// Show a message that asks the user a question. Only two buttons available, yes and no.
        /// </summary>
        /// <param name="ParentWindow">The IWin32Window parent window</param>
        /// <param name="MessageText">The question</param>
        /// <param name="WindowCaption">The message caption</param>
        /// <param name="YesButtonText">The text that represents "yes"</param>
        /// <param name="NoButtonText">The text that represents "no"</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowQuestionMessage(IWin32Window ParentWindow, string MessageText,
            string WindowCaption, string YesButtonText, string NoButtonText)
        {
            return ShowMessage(ParentWindow, MessageText, WindowCaption, MessageDialogButtons.OkNo,
                MessageDialogIcon.Question, false, YesButtonText, NoButtonText, "", "");
        }
        #endregion
        /// <summary>
        /// Show an error message
        /// </summary>
        /// <param name="messageText">The message text</param>
        /// <param name="caption">The message caption</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowErrorMessage(string messageText, string caption)
        {
            return ShowErrorMessage(null, messageText, caption);
        }
        /// <summary>
        /// Show an error message
        /// </summary>
        /// <param name="ParentWindow">The message text</param>
        /// <param name="messageText">The message caption</param>
        /// <param name="caption">MessageDialogResult</param>
        /// <returns>Result of message after user choose option</returns>
        public static MessageDialogResult ShowErrorMessage(IWin32Window ParentWindow, string messageText, string caption)
        {
            return ShowMessage(ParentWindow, messageText, caption, MessageDialogButtons.Ok, MessageDialogIcon.Error);
        }
    }

    public enum MessageDialogButtons
    {
        /// <summary>
        /// Show ok button only
        /// </summary>
        Ok = 0x1,
        /// <summary>
        /// Show ok and no buttons
        /// </summary>
        OkNo = 0x3,
        /// <summary>
        /// Show all buttons (ok, no and cancel)
        /// </summary>
        OkNoCancel = 0x7,
        /// <summary>
        /// Show check box
        /// </summary>
        Checked = 0x8
    }
    public enum MessageDialogIcon
    {
        Info,
        Question,
        Save,
        Error,
        Warning
    }
    public enum MessageDialogResult : int
    {
        None = 0,
        /// <summary>
        /// Ok
        /// </summary>
        Ok = 0x1,
        /// <summary>
        /// No
        /// </summary>
        No = 0x2,
        /// <summary>
        /// Cancel
        /// </summary>
        Cancel = 0x4,
        /// <summary>
        /// Box Checked
        /// </summary>
        Checked = 0x8
    }
}
