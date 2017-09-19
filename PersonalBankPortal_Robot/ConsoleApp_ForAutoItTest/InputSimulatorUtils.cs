using System;
using System.Threading;
using WindowsInput;
using ConsoleApp_ForAutoItTest.SendKeyMessage;

namespace ConsoleApp_ForAutoItTest
{
    public class InputSimulatorUtils
    {
        private static InputSimulator _inputSimulator = new InputSimulator();

        public static void KeyIn(string value)
        {
            foreach (char item in value)
            {
                Thread.Sleep(GetRandomDelay(50));

                const uint deltaUppercaseAndLowercase = 32;
                var charCode = SimulateKey.ConvertCharToInt(item);
                var isNumber = charCode >= VirtualKeyCode.VK_0 && charCode <= VirtualKeyCode.VK_9;
                var isUppercase = charCode >= VirtualKeyCode.VK_A && charCode <= VirtualKeyCode.VK_Z;
                var isLowercase = charCode >= VirtualKeyCode.VK_A + deltaUppercaseAndLowercase && charCode <= VirtualKeyCode.VK_Z + deltaUppercaseAndLowercase;

                uint wParamChar = charCode;
                if (isNumber || isUppercase || isLowercase)
                {
                    wParamChar = isLowercase ? (charCode - deltaUppercaseAndLowercase) : charCode;
                }

                if (isUppercase)
                {
                    _inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                }
                _inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)wParamChar);
                if (isUppercase)
                {
                    _inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
                }
            }
        }

        private static int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(3, 7);
            return value * multiplier;
        }

    }

}
