using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConsoleApp_ForAutoItTest.SendKeyMessage
{
    public class SimulateKey
    {
        private const uint _lParamKeyDown = 0x001E0001;

        private const uint _lParamChar = 0x001E0001;

        private const uint _lParamKeyUp = 0xC01E0001;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, uint wMsg, uint wParam, uint lParam);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindowNative(string className, string windowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        public static IntPtr FindWindow(string className, string windowName)
        {
            return FindWindowNative(className, windowName);
        }

        public static bool SendText(IntPtr hWnd, string text)
        {
            var isSuccess = false;
            foreach (var item in text)
            {
                Thread.Sleep(GetRandomDelay(50));
                isSuccess = SendChar(hWnd, item);
                if (!isSuccess)
                {
                    break;
                }
            }
            return isSuccess;
        }

        
        [DllImport("User32.dll",
                       EntryPoint = "mouse_event",
                       CallingConvention = CallingConvention.Winapi)]
        internal static extern void Mouse_Event(int dwFlags,
                                                    int dx,
                                                    int dy,
                                                    int dwData,
                                                    int dwExtraInfo);
        public static void MouseMove(int x, int y)
        {
            Mouse_Event(0x0001 | 0x8000, x, y, 0, 0);
        }

        public static void MouseClick(int x, int y)
        {
            Mouse_Event(0x0002, x, y, 0, 0);
            Thread.Sleep(10);
            Mouse_Event(0x0004, x, y, 0, 0);
        }

        public static void ClearText(IntPtr hw, int length = 20)
        {
            for (int i = 0; i < length; i++)
            {
                SendMessage(hw, MessageCode.WM_KEYDOWN, VirtualKeyCode.BACK, _lParamKeyDown);
                SendMessage(hw, MessageCode.WM_CHAR, VirtualKeyCode.BACK, _lParamChar);
                SendMessage(hw, MessageCode.WM_KEYUP, VirtualKeyCode.BACK, _lParamKeyUp);
            }
        }

        private static bool SendChar(IntPtr hWnd, char character)
        {
            var result = true;
            const uint deltaUppercaseAndLowercase = 32;
            var charCode = ConvertCharToInt(character);
            var isNumber = charCode >= VirtualKeyCode.VK_0 && charCode <= VirtualKeyCode.VK_9;
            var isUppercase = charCode >= VirtualKeyCode.VK_A && charCode <= VirtualKeyCode.VK_Z;
            var isLowercase = charCode >= VirtualKeyCode.VK_A + deltaUppercaseAndLowercase && charCode <= VirtualKeyCode.VK_Z + deltaUppercaseAndLowercase;
            if (isNumber || isUppercase || isLowercase)
            {
                uint wParamKey = charCode;
                uint wParamChar = isLowercase ? (charCode - deltaUppercaseAndLowercase) : charCode;
                SendMessage(hWnd, MessageCode.WM_KEYDOWN, wParamKey, _lParamKeyDown);
                SendMessage(hWnd, MessageCode.WM_CHAR, wParamChar, _lParamChar);
                SendMessage(hWnd, MessageCode.WM_KEYUP, wParamKey, _lParamKeyUp);
            }
            else
            {
                result = false;
            }
            return result;
        }

        public static uint ConvertCharToInt(char character)
        {
            return (uint)(character - '0') + VirtualKeyCode.VK_0;
        }

        private static int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(3, 7);
            return value * multiplier;
        }

    }

}
