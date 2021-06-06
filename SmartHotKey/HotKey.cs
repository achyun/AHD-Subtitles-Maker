//<Sreenivasan>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartHotKey.Utils;
using System.Diagnostics;

namespace SmartHotKey
{
    /// <summary>
    /// Control for Hotkey event handler
    /// </summary>
    public class HotKey : Control
    {
        private const int WM_HOTKEY = 0x312;
        //Delegates
        public delegate void HotKeyEventHandler(object sender, HotKeyEventArgs e);
        //Events
        public event HotKeyEventHandler HotKeyPressed;
        private Dictionary<int, string> dicHotKeys;

        public HotKey()
        {
            dicHotKeys = new Dictionary<int, string>();
        }

        

        /// <summary>
        /// Rregister the given hotkey
        /// </summary>
        /// <param name="strHotKey">hotkey</param>
        /// <returns>success flag of register action</returns>
        private bool Register(string strHotKey)
        {
            this.Unregister(strHotKey);
            int hotKeyId = HotKeyUtils.GlobalAddAtom("RE:" + strHotKey);
            if (hotKeyId == 0)
                throw new Exception(string.Format("Could not register atom for {0} hotkey!",strHotKey));
            
            if (HotKeyUtils.RegisterKey(this, hotKeyId, strHotKey))
            {
                dicHotKeys.Add(hotKeyId, strHotKey);
                return true;
            }
            return false;
        }
      

        /// <summary>
        /// Add the keys to for global key event watch
        /// </summary>
        /// <param name="strHotKey">hotkey</param>
        /// <returns>success flag of addkey action</returns>
        public bool AddHotKey(string hotKey)
        {
            if (!base.IsHandleCreated)
            {
                base.CreateControl();
            }
            return this.Register(hotKey);
        }
        
        /// <summary>
        /// Unregister all registered hot keys
        /// </summary>
        private void Unregister()
        {
            foreach (KeyValuePair<int,string> hotKey in dicHotKeys)
            {
                HotKeyUtils.UnregisterKey(this, hotKey.Key);
                HotKeyUtils.GlobalDeleteAtom(hotKey.Key);
            }
            dicHotKeys.Clear();
        }
        

        /// <summary>
        /// Unregister the specified registered hot keys
        /// </summary>
        /// <param name="strKey">registered hotkey</param>
        private void Unregister(string strKey)
        {
            int intKey = 0;
            foreach (KeyValuePair<int, string> hotKey in dicHotKeys)
            {
                if (hotKey.Value == strKey)
                {
                    intKey = hotKey.Key;
                    HotKeyUtils.UnregisterKey(this, hotKey.Key);
                    HotKeyUtils.GlobalDeleteAtom(hotKey.Key);
                    break;
                }
            }
            if (intKey > 0)
                dicHotKeys.Remove(intKey);
        }

      
        /// <summary>
        /// Remove the specified key from global watcher
        /// </summary>
        /// <param name="strKey"></param>
        public void RemoveKey(string strKey)
        {
            this.Unregister(strKey);
        }
       
        /// <summary>
        /// Remove all keys from global watcher
        /// </summary>
        public void RemoveAllKeys()
        {
            this.Unregister();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                if (this.HotKeyPressed != null)
                    this.HotKeyPressed(this, new HotKeyEventArgs { HotKey = this.dicHotKeys[m.WParam.ToInt32()] });
            }
            else
                base.WndProc(ref m);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // HotKey
            // 
            this.Visible = false;
            this.ResumeLayout(false);

        }
    }
    /// <summary>
    /// EventArgs class for HotKeyPressed event
    /// </summary>
    public class HotKeyEventArgs : EventArgs
    {
        public string HotKey { get; set; }
    }
}
//</Sreenivasan>