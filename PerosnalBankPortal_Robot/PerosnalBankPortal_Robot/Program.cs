using System;
namespace PerosnalBankPortal_Robot
{
    using AutoIt;
    using SendMessageKey;
    using System.Drawing;
    using System.Threading;

    internal class Program
    {
        private const string Transferpassword = "103830";
        private const string Userpass = "048627";
        private static Rectangle _windowsPosition;
        private static int _btnLogOutPossitionX;
        private static int _btnLogOutPossitionY;
        private static IntPtr _mainForm;

        private static void Main(string[] args)
        {
            if (AutoItX.WinExists("招商银行个人银行专业版_") == 1)
            {
                Console.WriteLine("LogIn Form Found");
                //AutoItX.Run("C:/Windows/SysWOW64/PersonalBankPortal.exe", "");

                LogIn();

                if (IsMainFormActivated())
                {
                    SetFormPosition();

                    Transfer();

                    LogOut();
                }
            }
            else
            {
                Console.WriteLine("Application is not Running");
            }
        }

        private static void SetPassword(IntPtr mainForm, IntPtr txtpass)
        {
            if (AutoItX.ControlFocus(mainForm, txtpass) != 1) return;
            Console.WriteLine("Focus on Password");
            AutoItX.Send(Userpass);
            Console.WriteLine($"{Userpass}: Password Set");
        }

        private static void SetBtnLogOutPossition()
        {
            _btnLogOutPossitionX = _windowsPosition.X + _windowsPosition.Width - 140;
            _btnLogOutPossitionY = _windowsPosition.Y + 17;
        }

        private static void SetMaximized()
        {
            AutoItX.WinSetState(_mainForm, AutoItX.SW_MAXIMIZE);
            Thread.Sleep(2000);
        }

        private static bool IsMainFormActivated()
        {
            var i = 0;
            while (i < 1000)
            {
                Thread.Sleep(4000);
                _mainForm = AutoItX.WinGetHandle("招商银行个人银行专业版_");
                AutoItX.WinActivate(_mainForm);
                if (AutoItX.ControlGetText(_mainForm,
                        AutoItX.ControlGetHandle(_mainForm, "[CLASS:TCMBStyleComboBox72; INSTANCE:1]")) == "功能")
                {
                    Console.WriteLine("Main Page is now Active.");
                    return true;
                }
                Console.WriteLine("Waiting for Main Page");
                i++;
            }
            return false;
        }

        private static void SetFormPosition()
        {
            SetMaximized();
            _windowsPosition = AutoItX.WinGetPos(_mainForm);
        }

        private static void LogIn()
        {
            if (AutoItX.WinActivate("招商银行个人银行专业版_") != 1) return;

            var loginForm = AutoItX.WinGetHandle("招商银行个人银行专业版_");

            var txtpass = AutoItX.ControlGetHandle(loginForm, "[CLASS:TCMBStyleEdit72]");

            SetPassword(loginForm, txtpass);

            DoLogIn(loginForm, txtpass);

            WaitForLogin(loginForm);
        }

        private static void WaitForLogin(IntPtr loginForm)
        {
            var i = 0;
            while (i < 1000)
            {
                Thread.Sleep(2000);

                if (AutoItX.ControlGetText(loginForm,
                        AutoItX.ControlGetHandle(loginForm, "[CLASS:TCMBStyleComboBox72; INSTANCE:1]")) != "功能")
                {
                    Thread.Sleep(4000);
                    Console.WriteLine("Successfully LogIn.. ");
                    Console.WriteLine("Closing Login Window.");
                    break;
                }
                Console.WriteLine("Logging In.");
                i++;
            }
        }

        private static void DoLogIn(IntPtr mainForm, IntPtr txtpass)
        {
            var windowsLogPosition = AutoItX.WinGetPos(mainForm);
            var txtPassPosition = AutoItX.ControlGetPos(mainForm, txtpass);

            var btnLogInPossitionX = windowsLogPosition.X + txtPassPosition.X + 50;
            var btnLogInPossitionY = windowsLogPosition.Y + txtPassPosition.Y + 60;

            Console.WriteLine("Click Log In");
            AutoItX.MouseClick("LEFT", btnLogInPossitionX, btnLogInPossitionY);
            if (AutoItX.WinActivate("[CLASS:TPbBaseMsgForm]") == 1)
                Console.WriteLine("Invalid Password");

            if (AutoItX.WinActivate("错误") == 1)
                Console.WriteLine("Authentication Key Is Required");
        }

        private static void LogOut()
        {
            SetBtnLogOutPossition();

            Console.WriteLine("Click Log Out");
            AutoItX.MouseClick("LEFT", _btnLogOutPossitionX, _btnLogOutPossitionY);

            if (ConfirmationMessageFindByClassName("TAppExitForm", 112, 194))
                Console.WriteLine("Are you sure?, OK...");

            if (ConfirmationMessageFindByClassName("TPbBaseMsgForm", 253, 219))
                Console.WriteLine("Remove Device?, OK...");

            if (ConfirmationMessageFindByClassName("TPbBaseMsgForm", 253, 193))
                Console.WriteLine("Last Warning Remove Device?, OK...");

            Console.WriteLine("Successfully Logout.");
        }

        private static bool ConfirmationMessageFindByClassName(string classname, int buttonLocationX,
            int buttonLocationY)
        {
            if (AutoItX.WinActivate($"[CLASS:{classname}]") != 1) return false;

            var exitForm = AutoItX.WinGetHandle($"[CLASS:{classname}]");

            var windowsExitmsgPosition = AutoItX.WinGetPos(exitForm);
            var exitPosX = windowsExitmsgPosition.X + buttonLocationX;
            var exitPosY = windowsExitmsgPosition.Y + buttonLocationY;

            AutoItX.MouseClick("LEFT", exitPosX, exitPosY);

            Thread.Sleep(500);

            return true;
        }

        private static int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(1, 3);
            return value * multiplier;
        }

        private static void FillUpTransferForm(IntPtr inputMainPanel)
        {
            var toAccountNumber = "6214837694277025";
            var toAccountName = "吕文斌";
            //var fromAccountNumber = "";
            //var fromAccountName = "";
            var transactionReferenceId = "12345678912";
            var transactionReference = "test Remarks";
            var amount = "1";


            //Click Transger

            if (AutoItX.WinActivate(_mainForm) == 1)
            {
                var i = 0;
                while (i < 1000)
                {
                    AutoItX.MouseClick("LEFT", _windowsPosition.X + 334, _windowsPosition.Y + 470);
                    Console.WriteLine("Click on Transfer Button.");
                    Thread.Sleep(2000);
                    inputMainPanel = AutoItX.ControlGetHandle(_mainForm, "InputMainPanel");
                    if (AutoItX.ControlFocus(_mainForm, inputMainPanel) == 1)
                    {
                        Console.WriteLine("Transfer page Found!");
                        break;
                    }
                    Console.WriteLine("Waiting for Transfer Page");
                    i++;
                }

                var inputMainPanelPosition = AutoItX.ControlGetPos(_mainForm, inputMainPanel);

                var elemementPositionX = _windowsPosition.X + inputMainPanelPosition.X;
                var elemementPositionY = _windowsPosition.Y + inputMainPanelPosition.Y;

                //Account Name
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                var txttoAccount = AutoItX.ControlGetHandle(_mainForm, "[CLASS:Edit; INSTANCE:1]");
                AutoItX.ControlSetText(_mainForm, txttoAccount, toAccountName);
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 35);
                Thread.Sleep(GetRandomDelay(1000));

                //Account Number
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 75);
                AutoItX.Send(toAccountNumber);
                Thread.Sleep(GetRandomDelay(1000));

                //Bank
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 120);
                Thread.Sleep(GetRandomDelay(1000));

                //Click Balance
                //AutoItX.WinActivate(_mainForm);
                //AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                //AutoItX.MouseClick("LEFT", elemementPositionX + 150, elemementPositionY + 195);
                //Thread.Sleep(GetRandomDelay(1000));

                //Amount
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 230);
                AutoItX.Send(amount);
                Thread.Sleep(GetRandomDelay(1000));

                //transaction Remarks
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 285);
                AutoItX.Send(transactionReference);
                Thread.Sleep(GetRandomDelay(1000));

                //transaction ID
                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 335);
                AutoItX.Send(transactionReferenceId);
                Thread.Sleep(GetRandomDelay(1000));

                AutoItX.WinActivate(_mainForm);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.MouseClick("LEFT", elemementPositionX + 200, elemementPositionY + 380);
                Thread.Sleep(GetRandomDelay(1000));

                if (ConfirmationMessageFindByClassName("TPbBaseMsgForm", 285, 180))
                    Console.WriteLine("Error FillUpTransferForm");
            }
        }

        private static bool IsValidTransferForm()
        {
            //WAIT FOR TRANSFER PAGE
            var i = 0;
            while (i < 1000)
            {
                Thread.Sleep(2000);

                if (ConfirmationMessageFindByClassName("TErrorWithHelpForm", 200, 200))
                {
                    Console.WriteLine("ERROR ON TRANSFER PAGE VALUES");
                    break;
                }

                //正常或校验码确认
                //短信校验码确认
                var inputMainPanel = AutoItX.ControlGetHandle(_mainForm, "正常或校验码确认");
                if (AutoItX.ControlFocus(_mainForm, inputMainPanel) == 1)
                {
                    Console.WriteLine("Transfer Confirmation page Found!");
                    return true;
                }

                inputMainPanel = AutoItX.ControlGetHandle(_mainForm, "短信校验码确认");
                if (AutoItX.ControlFocus(_mainForm, inputMainPanel) == 1)
                {
                    Console.WriteLine("Transfer Confirmation page Found!");
                    return true;
                }
                Console.WriteLine("Waiting for Transfer Confirmation Page");
                i++;
            }

            Console.WriteLine("Error Transfer Confirmation Not Found");

            return false;
        }

        private static void Transfer()
        {
            var inputMainPanel = new IntPtr();

            FillUpTransferForm(inputMainPanel);

            if (IsValidTransferForm())
            {
                //SET TRANSFER PASSWORD
                var txtTransferpass = AutoItX.ControlGetHandle(_mainForm, "[CLASS:TCMBEdit]");
                var txtTransferpassPosition = AutoItX.ControlGetPos(_mainForm, txtTransferpass);

                var elemementPositionX = _windowsPosition.X + txtTransferpassPosition.X;
                var elemementPositionY = _windowsPosition.Y + txtTransferpassPosition.Y;

                if (AutoItX.ControlFocus(_mainForm, txtTransferpass) != 1) return;

                Console.WriteLine("Focus on Transfer Password");

                AutoItX.WinActivate(_mainForm);
                SimulateKey.SetForegroundWindow(txtTransferpass);
                Thread.Sleep(100);
                SimulateKey.ClearText(txtTransferpass);
                SimulateKey.SendText(txtTransferpass, Transferpassword);
                

                Console.WriteLine($"{Transferpassword}: Transfer Password Set");

                Thread.Sleep(1000);

                Console.WriteLine("Confirm Transfer");
                AutoItX.MouseClick("LEFT", elemementPositionX + 80, elemementPositionY + 80);
                Thread.Sleep(2000);

                if (ConfirmationMessageFindByClassName("TPbBaseMsgForm", 200, 180))
                    Console.WriteLine("Confirmation Error On Transfer Transaction!");
                if (ConfirmationMessageFindByClassName("TErrorWithHelpForm", 200, 190))
                    Console.WriteLine("Confirmation Error On Transfer Transaction!");
            }
        }
    }
}