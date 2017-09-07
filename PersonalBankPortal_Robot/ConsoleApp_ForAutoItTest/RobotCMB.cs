using AutoIt;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB
    {
        private const int WIN_STATE_MINIMIZED = 5;
        private const string LoginFormTitle = "招商银行个人银行专业版";
        private static IntPtr _mainForm;
        private static Rectangle _mainFormPosition;

        private delegate RobotResult FundOutStep(RobotContext context);

        private FundOutStep[] AllSteps()
        {
            return new FundOutStep[]
            {
                OpenDesktopApplication,
                DoLogIn,
                DoTransfer,
                DoLogOut
            };
        }

        public void Transfer(RobotContext context)
        {
            RobotResult transferResult = RobotResult.Default(context);
            var steps = AllSteps();
            try
            {
                for (int i = 0; i < steps.Length; i++)
                {
                    FundOutStep step = steps[i];
                    int stepNo = i + 1;
                    transferResult = step.Invoke(context);
                    if (transferResult.IsSuccess())
                    {
                        Console.WriteLine("Step<{0}> Pass By <{1}|{2}>", stepNo, transferResult.Status.Code, transferResult.Status.Description);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: <{0}>", ex);
            }
            finally
            {
                Console.WriteLine("TransferResult is [{0}|{1}]", transferResult.Status.Code, transferResult.Status.Description);
            }

        }

        private RobotResult OpenDesktopApplication(RobotContext context)
        {
            try
            {
                if (AutoItX.WinExists(LoginFormTitle) == 1)
                {
                    int winGetState = AutoItX.WinGetState(LoginFormTitle);
                    if (winGetState == WIN_STATE_MINIMIZED)
                    {
                        AutoItX.WinSetState(LoginFormTitle, "", AutoItX.SW_RESTORE);
                    }
                }
                else
                {
                    AutoItX.Run("D:\\MIDAS\\CMB\\Locale.Emulator.2.3.1.1\\LEProc.exe -run \"C:\\Windows\\SysWOW64\\PersonalBankPortal.exe\"", "");
                    AutoItX.WinWait(LoginFormTitle, "", 5);
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
                return RobotResult.Build(context, RobotStatus.SUCCESS, "Open Client App Success!");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private RobotResult DoLogIn(RobotContext context)
        {
            string loginPassword = context.LoginPassword;
            try
            {
                IntPtr loginForm = AutoItX.WinGetHandle(LoginFormTitle);
                IntPtr textPass = AutoItX.ControlGetHandle(loginForm, "[CLASS:TCMBStyleEdit72]");

                ClearTextBox(loginForm, textPass);
                EnterTextBox(loginForm, textPass, loginPassword);
                ClickLoginButton(loginForm, textPass);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                int errorHappen1 = AutoItX.WinWaitActive("[TITLE:错误;CLASS:TErrorWithHelpForm]", "", 5); //token key not plugin
                if (errorHappen1 == 1)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    sw.Stop();
                    Console.WriteLine("spend timeA: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed1, Error<Authentication Key Missing>");
                }
                int errorHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //login password validate
                if (errorHappen2 == 1)
                {
                    string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                    AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                    sw.Stop();
                    Console.WriteLine("spend timeB: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, $"Login Failed2, Error<{errorText.Trim()}>");
                }
                int errorHappen3 = AutoItX.WinWaitActive("[TITLE:招商银行个人银行专业版;CLASS:TMainFrm]", "功能", 20); //main portal window
                if (errorHappen3 == 1)
                {
                    _mainForm = AutoItX.WinGetHandle("[TITLE:招商银行个人银行专业版;CLASS:TMainFrm]", "功能");
                    _mainFormPosition = AutoItX.WinGetPos(_mainForm);
                    sw.Stop();
                    Console.WriteLine("spend timeC: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Success, Awesome!");
                }
                sw.Stop();
                Console.WriteLine("spend timeZ: " + sw.ElapsedMilliseconds);
                return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed3, Unknown Error<Main Portal Window Not Active>");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private RobotResult DoTransfer(RobotContext context)
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));

                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private RobotResult DoLogOut(RobotContext context)
        {
            try
            {
                int btnLogOutPossitionX = _mainFormPosition.X + _mainFormPosition.Width - 140;
                int btnLogOutPossitionY = _mainFormPosition.Y + 17;
                Console.WriteLine("Click Log Out");
                AutoItX.MouseClick("LEFT", btnLogOutPossitionX, btnLogOutPossitionY);

                int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TAppExitForm]", "", 5);
                if (warningHappen1 == 1)
                {
                    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TAppExitForm]");
                    Rectangle warningPopWinPossition1 = AutoItX.WinGetPos(warningPopWin1);
                    int btnYesPossitionX1 = warningPopWinPossition1.X + 112;
                    int btnYesPossitionY1 = warningPopWinPossition1.Y + 194;
                    Console.WriteLine("Click Yes1 Button");
                    AutoItX.MouseClick("LEFT", btnYesPossitionX1, btnYesPossitionY1);
                }
                int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑", 5);
                if (warningHappen2 == 1)
                {
                    IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑");
                    Rectangle warningPopWinPossition2 = AutoItX.WinGetPos(warningPopWin2);
                    int btnYesPossitionX2 = warningPopWinPossition2.X + 253;
                    int btnYesPossitionY2 = warningPopWinPossition2.Y + 219;
                    Console.WriteLine("Click Yes2 Button");
                    AutoItX.MouseMove(btnYesPossitionX2, btnYesPossitionY2);
                    //AutoItX.MouseClick("LEFT", btnYesPossitionX2, btnYesPossitionY2);
                }
                int warningHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版", 5);
                if (warningHappen3 == 1)
                {
                    IntPtr warningPopWin3 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版");
                    Rectangle warningPopWinPossition3 = AutoItX.WinGetPos(warningPopWin3);
                    int btnYesPossitionX3 = warningPopWinPossition3.X + 253;
                    int btnYesPossitionY3 = warningPopWinPossition3.Y + 193;
                    Console.WriteLine("Click Yes3 Button");
                    AutoItX.MouseClick("LEFT", btnYesPossitionX3, btnYesPossitionY3);
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private static void ClickLoginButton(IntPtr loginForm, IntPtr textPass)
        {
            Rectangle loginFormPosition = AutoItX.WinGetPos(loginForm);
            Rectangle textPassPosition = AutoItX.ControlGetPos(loginForm, textPass);
            int btnLogInPossitionX = loginFormPosition.X + textPassPosition.X + 50;
            int btnLogInPossitionY = loginFormPosition.Y + textPassPosition.Y + 60;
            Console.WriteLine("Click Log In");
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
                string textBoxContent = AutoItX.ControlGetText(mainWindow, textBox);
                while (!string.IsNullOrEmpty(textBoxContent))
                {
                    AutoItX.Send("{BACKSPACE}");
                    textBoxContent = AutoItX.ControlGetText(mainWindow, textBox);
                }
            }
        }

    }
}
