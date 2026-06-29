using System;
using System.Runtime.InteropServices;

namespace glc_cs.Core.RemoteDesktop
{
    internal static class InputSimulator
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);

        private const uint INPUT_MOUSE = 0;
        private const uint INPUT_KEYBOARD = 1;

        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;
        private const uint MOUSEEVENTF_ABSOLUTE = 0x8000;

        private const uint KEYEVENTF_KEYDOWN = 0x0000;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_UNICODE = 0x0004;

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct INPUTUNION
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public static void MoveMouse(int screenX, int screenY)
        {
            SetCursorPos(screenX, screenY);
        }

        public static void MouseClick(int screenX, int screenY, string button = "left")
        {
            SetCursorPos(screenX, screenY);
            uint down, up;
            switch (button)
            {
                case "right":
                    down = MOUSEEVENTF_RIGHTDOWN;
                    up = MOUSEEVENTF_RIGHTUP;
                    break;
                case "middle":
                    down = MOUSEEVENTF_MIDDLEDOWN;
                    up = MOUSEEVENTF_MIDDLEUP;
                    break;
                default:
                    down = MOUSEEVENTF_LEFTDOWN;
                    up = MOUSEEVENTF_LEFTUP;
                    break;
            }
            mouse_event(down, 0, 0, 0, IntPtr.Zero);
            mouse_event(up, 0, 0, 0, IntPtr.Zero);
        }

        public static void MouseDown(int screenX, int screenY, string button = "left")
        {
            SetCursorPos(screenX, screenY);
            uint flag;
            switch (button)
            {
                case "right": flag = MOUSEEVENTF_RIGHTDOWN; break;
                case "middle": flag = MOUSEEVENTF_MIDDLEDOWN; break;
                default: flag = MOUSEEVENTF_LEFTDOWN; break;
            }
            mouse_event(flag, 0, 0, 0, IntPtr.Zero);
        }

        public static void MouseUp(int screenX, int screenY, string button = "left")
        {
            SetCursorPos(screenX, screenY);
            uint flag;
            switch (button)
            {
                case "right": flag = MOUSEEVENTF_RIGHTUP; break;
                case "middle": flag = MOUSEEVENTF_MIDDLEUP; break;
                default: flag = MOUSEEVENTF_LEFTUP; break;
            }
            mouse_event(flag, 0, 0, 0, IntPtr.Zero);
        }

        public static void MouseDoubleClick(int screenX, int screenY, string button = "left")
        {
            MouseClick(screenX, screenY, button);
            System.Threading.Thread.Sleep(50);
            MouseClick(screenX, screenY, button);
        }

        public static void MouseScroll(int screenX, int screenY, int delta)
        {
            SetCursorPos(screenX, screenY);
            mouse_event(MOUSEEVENTF_WHEEL, 0, 0, delta, IntPtr.Zero);
        }

        public static void KeyDown(ushort vkCode)
        {
            var input = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = vkCode,
                        dwFlags = KEYEVENTF_KEYDOWN | (IsExtendedKey(vkCode) ? KEYEVENTF_EXTENDEDKEY : 0)
                    }
                }
            };
            SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void KeyUp(ushort vkCode)
        {
            var input = new INPUT
            {
                type = INPUT_KEYBOARD,
                u = new INPUTUNION
                {
                    ki = new KEYBDINPUT
                    {
                        wVk = vkCode,
                        dwFlags = KEYEVENTF_KEYUP | (IsExtendedKey(vkCode) ? KEYEVENTF_EXTENDEDKEY : 0)
                    }
                }
            };
            SendInput(1, new[] { input }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void KeyPress(ushort vkCode)
        {
            KeyDown(vkCode);
            KeyUp(vkCode);
        }

        public static void KeyCombo(ushort[] vkCodes)
        {
            foreach (var vk in vkCodes)
                KeyDown(vk);
            System.Threading.Thread.Sleep(50);
            for (int i = vkCodes.Length - 1; i >= 0; i--)
                KeyUp(vkCodes[i]);
        }

        public static string GetClipboardText()
        {
            string result = "";
            var thread = new System.Threading.Thread(() =>
            {
                try
                {
                    if (System.Windows.Forms.Clipboard.ContainsText())
                        result = System.Windows.Forms.Clipboard.GetText();
                }
                catch { }
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join(2000);
            return result;
        }

        public static void SetClipboardText(string text)
        {
            var thread = new System.Threading.Thread(() =>
            {
                try { System.Windows.Forms.Clipboard.SetText(text); }
                catch { }
            });
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
            thread.Join(2000);
        }

        public static void TypeText(string text)
        {
            foreach (char c in text)
            {
                var inputs = new INPUT[2];
                inputs[0] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new INPUTUNION
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)c,
                            dwFlags = KEYEVENTF_UNICODE
                        }
                    }
                };
                inputs[1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    u = new INPUTUNION
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = (ushort)c,
                            dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP
                        }
                    }
                };
                SendInput(2, inputs, Marshal.SizeOf(typeof(INPUT)));
            }
        }

        private static bool IsExtendedKey(ushort vk)
        {
            return vk == 0x21 || vk == 0x22 || vk == 0x23 || vk == 0x24 || // PgUp/Dn, End, Home
                   vk == 0x25 || vk == 0x26 || vk == 0x27 || vk == 0x28 || // Arrows
                   vk == 0x2D || vk == 0x2E || // Insert, Delete
                   vk == 0x5B || vk == 0x5C || // Win keys
                   vk == 0x6F; // Numpad Divide (only numpad key that's extended)
        }
    }
}
