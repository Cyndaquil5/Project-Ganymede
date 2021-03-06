﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace Project_Ganymede
{   
    public partial class Form1 : Form
    {

        List<Image> images = new List<Image>();
        private void wallpaperchange()
        {
            //Fill with more images
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Make a Random-object
            Random rand = new Random();         
  

            //Pick a random image from the list
            this.BackgroundImage = images[rand.Next(0, images.Count - 1)];
            

        }
        
        public Form1()
        {
            InitializeComponent();
        }
        KeyboardHook hook = new KeyboardHook();

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hook.KeyPressed +=  new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            hook.RegisterHotKey(Project_Ganymede.ModifierKeys.Shift, Keys.F1);

            images.Add(Image.FromFile(@"C:\Users\Kronos\Desktop\Io\Switching Wallpaper\Abstract Wallpaper.jpg"));
            images.Add(Image.FromFile(@"C:\Users\Kronos\Desktop\Io\Switching Wallpaper\Helix Nebula.jpg"));
            images.Add(Image.FromFile(@"C:\Users\Kronos\Desktop\Io\Switching Wallpaper\Sculptor Galaxy.jpg"));
            images.Add(Image.FromFile(@"C:\Users\Kronos\Desktop\Io\Switching Wallpaper\Crab Nebula.jpg"));
        }
        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            Process.Start("Firefox.exe");
            playSimpleSound(@"C:\Users\Kronos\Desktop\Io\Audio\heavengate.wav");
            Process.Start("cmd.exe");
            playSimpleSound(@"C:\Users\Kronos\Desktop\Io\Audio\heavengate.wav");
            // show the keys pressed in a label.
            //label1.Text = e.Modifier.ToString() + " + " + e.Key.ToString();
        }
        private void playSimpleSound(string path)
        {
            SoundPlayer simpleSound = new SoundPlayer(path);
            simpleSound.Play();
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            playSimpleSound(@"C:\Users\Kronos\Desktop\Io\Audio\kaching.wav");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            wallpaperchange();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("taskmgr.exe");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("Firefox.exe");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Process.Start("cmd.exe");
        }
    } 
    
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    // get the keys.
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKeys modifier = (ModifierKeys)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            _window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        public void RegisterHotKey(ModifierKeys modifier, Keys key)
        {
            // increment the counter.
            _currentId = _currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }

            // dispose the inner native window.
            _window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum ModifierKeys : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

}
