﻿using System;
using System.Windows.Forms;

namespace AutoClicker
{
    internal class Clicker : IDisposable
    {
        private readonly uint buttonDownCode;
        private readonly uint buttonUpCode;
        private readonly IntPtr minecraftHandle;
        private readonly Timer timer;

        private bool hold = false;

        public Clicker(uint buttonDownCode, uint buttonUpCode, IntPtr minecraftHandle)
        {
            this.buttonDownCode = buttonDownCode;
            this.buttonUpCode = buttonUpCode;
            this.minecraftHandle = minecraftHandle;

            timer = new Timer();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Click();
        }

        public void Start(int delay)
        {
            Stop();
            hold = (delay == 0);

            if (hold)
            {
                //Select the minecraft handle with Alt+Tab to not stop holding (when using the program)
                Win32Api.PostMessage(minecraftHandle, buttonDownCode, (IntPtr)0x0001, IntPtr.Zero);
            }
            else
            {
                Click();
                timer.Interval = delay;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (!hold)
            {
                timer.Stop();
            }

            Click();
        }

        public void Dispose()
        {
            Stop();
            timer.Dispose();
        }

        private void Click()
        {
            Win32Api.PostMessage(minecraftHandle, buttonDownCode, IntPtr.Zero, IntPtr.Zero);
            Win32Api.PostMessage(minecraftHandle, buttonUpCode, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
