using AutoIt;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB
    {
        private const int AutoItXSuccess = 1;
        private const string LoginFormTitle = "招商银行个人银行专业版";
        private IntPtr _mainForm;
        private Rectangle _mainFormPosition;

        private delegate RobotResult FundOutStep(RobotContext context);

        private FundOutStep[] AllSteps()
        {
            return new FundOutStep[]
            {
                DoOpenClientApp,
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

        private RobotResult DoOpenClientApp(RobotContext context)
        {
            try
            {
                int processExists = AutoItX.ProcessExists("PersonalBankPortal.exe");
                if (processExists != 0)
                {
                    int processClose = AutoItX.ProcessClose("PersonalBankPortal.exe");
                    if (processClose == AutoItXSuccess)
                    {
                        Console.WriteLine("Kill old process done");
                    }
                }
                if (AutoItX.WinExists(LoginFormTitle) != AutoItXSuccess)
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
                if (errorHappen1 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    sw.Stop();
                    Console.WriteLine("spend timeA: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed1, Error<Authentication Key Missing>");
                }
                int errorHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //login password validate
                if (errorHappen2 == AutoItXSuccess)
                {
                    string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                    AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                    sw.Stop();
                    Console.WriteLine("spend timeB: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, $"Login Failed2, Error<{errorText.Trim()}>");
                }
                int errorHappen3 = AutoItX.WinWaitActive("[TITLE:招商银行个人银行专业版;CLASS:TMainFrm]", "功能", 60); //main portal window
                if (errorHappen3 == AutoItXSuccess)
                {
                    _mainForm = AutoItX.WinGetHandle("[TITLE:招商银行个人银行专业版;CLASS:TMainFrm]", "功能");
                    _mainFormPosition = AutoItX.WinGetPos(_mainForm);
                    sw.Stop();
                    Console.WriteLine("spend timeC: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.SUCCESS, "Login Success, Awesome!");
                }
                int errorHappen4 = AutoItX.WinWaitActive("[TITLE:错误;CLASS:TErrorWithHelpForm]", "", 5); //main portal window
                if (errorHappen4 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    sw.Stop();
                    Console.WriteLine("spend timeC: " + sw.ElapsedMilliseconds);
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed3, Error<Handshake Fault>");
                }
                sw.Stop();
                Console.WriteLine("spend timeZ: " + sw.ElapsedMilliseconds);
                return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed4, Unknown Error<Main Portal Window Not Active>");
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
                for (int i = 0; i < 7; i++)
                {
                    Console.WriteLine("Do Transfer Out" + i);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
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
                ClickButton(_mainFormPosition.X, _mainFormPosition.Y, _mainFormPosition.Width - 140, 17);

                int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TAppExitForm]", "", 5);
                if (warningHappen1 == AutoItXSuccess)
                {
                    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TAppExitForm]");
                    Rectangle warningPopWinPossition1 = AutoItX.WinGetPos(warningPopWin1);
                    ClickButton(warningPopWinPossition1.X, warningPopWinPossition1.Y, 110, 190);
                }
                int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑", 5);
                if (warningHappen2 == AutoItXSuccess)
                {
                    IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑");
                    Rectangle warningPopWinPossition2 = AutoItX.WinGetPos(warningPopWin2);
                    ClickButton(warningPopWinPossition2.X, warningPopWinPossition2.Y, 250, 170);
                }
                int warningHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版", 5);
                if (warningHappen3 == AutoItXSuccess)
                {
                    IntPtr warningPopWin3 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版");
                    Rectangle warningPopWinPossition3 = AutoItX.WinGetPos(warningPopWin3);
                    ClickButton(warningPopWinPossition3.X, warningPopWinPossition3.Y, 260, 160);
                }
                Thread.Sleep(TimeSpan.FromSeconds(3));
                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void ClickLoginButton(IntPtr loginForm, IntPtr textPass)
        {
            Rectangle loginFormPosition = AutoItX.WinGetPos(loginForm);
            Rectangle textPassPosition = AutoItX.ControlGetPos(loginForm, textPass);
            int startX = loginFormPosition.X + textPassPosition.X;
            int startY = loginFormPosition.Y + textPassPosition.Y;
            ClickButton(startX, startY, 50, 60);
        }

        private void ClickButton(int startX, int startY, int offsetX, int offsetY)
        {
            AutoItX.MouseMove(startX, startY);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            int btnPossitionX = startX + offsetX;
            int btnPossitionY = startY + offsetY;
            AutoItX.MouseMove(btnPossitionX, btnPossitionY);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            AutoItX.MouseDown();
            AutoItX.MouseUp();
        }


        private void EnterTextBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                AutoItX.Send(value);
            }
        }

        private void ClearTextBox(IntPtr mainWindow, IntPtr textBox)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
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
