using System;
using System.Threading;
using WindowsInput;
using ConsoleApp_ForAutoItTest.SendKeyMessage;
using System.Diagnostics;

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
                    _inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)wParamChar);
                    _inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
                }
                else
                {
                    _inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)wParamChar);
                }
            }
        }

        private static int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(3, 7);
            return value * multiplier;
        }

        public static string EnterKeysByVirtualKeyboard(string text, bool delayBetweenCharacters = false)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cscript";
            var args = string.Format(@"d:\dsf\Antelope.VirtualKeyboard.wsf ""{0}""", text);
            Console.WriteLine(args);
            if (delayBetweenCharacters)
                args = string.Format(@"{0} """"", args);
            p.StartInfo.Arguments = args;
            ////p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.Start();
            p.WaitForExit();

            string output = p.StandardOutput.ReadToEnd();
            string err = p.StandardError.ReadToEnd();
            return output + err;
        }

        }

    }
