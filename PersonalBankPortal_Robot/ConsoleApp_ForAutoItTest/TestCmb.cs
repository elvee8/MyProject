using System;
using System.Collections.Generic;
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
            Run();
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
            string loginPassword = "gh202123X";

            IntPtr loginForm = AutoItX.WinGetHandle(LoginFormTitle);
            IntPtr textPass = AutoItX.ControlGetHandle(loginForm, "[CLASS:TCMBStyleEdit72]");

            ClearTextBox(loginForm, textPass);
            EnterTextBox(loginForm, textPass, loginPassword);
            ClickLoginButton(loginForm, textPass);

            int errorHappen1 = AutoItX.WinActive("", "错误"); //weird
            int errorHappen = AutoItX.WinActive("[CLASS:TPbBaseMsgForm]");
            if (errorHappen == 1)
            {
                string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                Console.WriteLine("Login Failed, Error<{0}>", errorText.Trim());
                AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
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
