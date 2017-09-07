using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;

namespace ConsoleApp_ForAutoItTest
{
    class TestCmb
    {
        private const string LoginFormTitle = "招商银行个人银行专业版";

        public static void Main1(string[] args)
        {
            Console.WriteLine("1234");
            int loopTimes = 2;
            for (int i = 0; i < loopTimes; i++)
            {
                Run();
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("5678");
            Console.ReadKey();
        }

        private static void Run()
        {
            if (AutoItX.WinExists(LoginFormTitle) == 1)
            {
                Console.WriteLine("LogIn Form Found");
                LogIn();

            }
            else
            {
                Console.WriteLine("Application is not Running");
            }
        }

        private static void LogIn()
        {
            string loginPassword = "ah202123";

            IntPtr loginForm = AutoItX.WinGetHandle(LoginFormTitle);
            IntPtr textPass = AutoItX.ControlGetHandle(loginForm, "[CLASS:TCMBStyleEdit72]");

            ClearTextBox(loginForm, textPass);
            EnterTextBox(loginForm, textPass, loginPassword);
            ClickLoginButton(loginForm, textPass);

            /*int errorHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 2); //client password validate
            if (errorHappen1 == 1)
            {
                string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                Console.WriteLine("Login Failed1, Error<{0}>", errorText.Trim());
                AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                return;
            }*/
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int errorHappen2 = AutoItX.WinWaitActive("[CLASS:TErrorWithHelpForm]", "", 5); //token key not plugin
            if (errorHappen2 == 1)
            {
                Console.WriteLine("Login Failed2, Error<{0}>", "Authentication Key Missing");
                AutoItX.WinClose("[CLASS:TErrorWithHelpForm]");
                sw.Stop();
                Console.WriteLine("spend timeA: " + sw.ElapsedMilliseconds);
                return;
            }
            int errorHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //server password validate
            if (errorHappen3 == 1)
            {
                string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                Console.WriteLine("Login Failed3, Error<{0}>", errorText.Trim());
                AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                sw.Stop();
                Console.WriteLine("spend timeB: " + sw.ElapsedMilliseconds);
                return;
            }

        }


        private static void ClickLoginButton(IntPtr loginForm, IntPtr textPass)
        {
            Rectangle loginFormPosition = AutoItX.WinGetPos(loginForm);
            Rectangle textPassPosition = AutoItX.ControlGetPos(loginForm, textPass);
            var btnLogInPossitionX = loginFormPosition.X + textPassPosition.X + 50;
            var btnLogInPossitionY = loginFormPosition.Y + textPassPosition.Y + 60;

            Console.WriteLine("Click Log In");
            //AutoItX.MouseMove(btnLogInPossitionX, btnLogInPossitionY);
            AutoItX.MouseClick("LEFT", btnLogInPossitionX, btnLogInPossitionY);
        }

        private static void EnterTextBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == 1)
            {
                AutoItX.Send(value);
            }
        }

        private static void ClearTextBox(IntPtr mainWindow, IntPtr textBox)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == 1)
            {
                do
                {
                    AutoItX.Send("{BACKSPACE}");
                } while (!string.IsNullOrEmpty(AutoItX.ControlGetText(mainWindow, textBox)));
            }
        }
        
    }
}
