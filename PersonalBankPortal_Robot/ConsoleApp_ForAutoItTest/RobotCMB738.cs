using AutoIt;
using System;
using System.Drawing;
using System.Threading;
using ConsoleApp_ForAutoItTest.SendKeyMessage;
using NLog;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB738
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private const string ProcessName = "PersonalBankPortal.exe";
        private const string ProgramFullPath = "D:\\MIDAS\\CMB\\Locale.Emulator.2.3.1.1\\LEProc.exe -run \"C:\\Windows\\SysWOW64\\PersonalBankPortal.exe\"";
        private const int AutoItXSuccess = WaitUtils.AutoItXSuccess;
        private const string LoginFormTitle = "招商银行个人银行专业版";
        private const string MainWindowTitle = "[TITLE:招商银行个人银行专业版; CLASS:TMainFrm]";
        private const string MainWindowText = "功能";

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
                    LOG.Log(LogLevel.Debug, "TransactionId<{0}>, Step <{1}|{2}> Started With <{3}>", context.MidasTransactionId, stepNo, step.Method.Name, context);
                    // UpdateStatus
                    transferResult = step.Invoke(context);
                    if (transferResult.IsSuccess())
                    {
                        LOG.Log(LogLevel.Debug, "TransactionId<{0}>, Step <{1}|{2}> PassBy <{3}|{4}>", context.MidasTransactionId, stepNo, step.Method.Name, transferResult.Status.Code, transferResult.Status.Description);
                    }
                    else
                    {
                        LOG.Log(LogLevel.Debug, "TransactionId<{0}>, Step <{1}|{2}> FailIn <{3}|{4}>", context.MidasTransactionId, stepNo, step.Method.Name, transferResult.Status.Code, transferResult.Status.Description);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                LOG.Error(e, "TransactionId<{0}>, Error<{1}>", context.MidasTransactionId, e.Message);
            }
            finally
            {
                // UpdateStatus
                Console.WriteLine("TransferResult is [{0}|{1}]", transferResult.Status.Code, transferResult.Status.Description);
            }

        }

        #region Step Methods
        private RobotResult DoOpenClientApp(RobotContext context)
        {
            try
            {
                int processExists = AutoItX.ProcessExists(ProcessName);
                if (processExists != 0)
                {
                    int processClose = AutoItX.ProcessClose(ProcessName);
                    if (processClose == AutoItXSuccess)
                    {
                        LOG.Log(LogLevel.Debug, "TransactionId<{0}>, Kill old process<{1}> done", context.MidasTransactionId, processExists);
                    }
                }
                if (AutoItX.WinExists(LoginFormTitle) != AutoItXSuccess)
                {
                    AutoItX.Run(ProgramFullPath, "");
                    int errorHappen1 = AutoItX.WinWaitActive(LoginFormTitle, "", 5);
                    if (errorHappen1 == AutoItXSuccess)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                    }
                    else
                    {
                        LOG.Error("TransactionId<{0}>, App<{1}> not found", context.MidasTransactionId, ProgramFullPath);
                        throw new Exception("Open App Failed, Error<App Location Not Found>");
                    }
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
            try
            {
                IntPtr loginFormWindow = AutoItX.WinGetHandle(LoginFormTitle);
                IntPtr textPassBox = AutoItX.ControlGetHandle(loginFormWindow, "[CLASS:TCMBStyleEdit72]");
                EnterPinBox(loginFormWindow, textPassBox, context.LoginPassword);
                ClickButton(loginFormWindow, 200, 400);

                int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //login password validate
                if (warningHappen1 == AutoItXSuccess)
                {
                    string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                    RobotCMB738Utils.SaveErrorShot(context.MidasTransactionId);
                    AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, $"Login Failed, Error<{errorText.Trim()}>");
                }

                RobotCMB738Utils.UntilWinActive(context.MidasTransactionId, MainWindowTitle, MainWindowText);
                Thread.Sleep(TimeSpan.FromSeconds(2)); // sleep wait for [CLASS:Internet Explorer_Server] load done
                return RobotResult.Build(context, RobotStatus.SUCCESS, "Login Success, Awesome!");
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
                IntPtr mainFormWindow = GetMainFormWindow();
                AutoItX.WinActivate(mainFormWindow);

                ClickButton(mainFormWindow, 60, 320); // click 'Transfer' button
                RobotCMB738Utils.UntilControlFocus(context.MidasTransactionId, MainWindowTitle, MainWindowText, "[CLASS:TCMBStyleEdit72; INSTANCE:4]");

                FillBankTransInfo(mainFormWindow, context);
                ClickButton(mainFormWindow, 180, 650); // click 'Next' button
                //RobotCMB738Utils.UntilControlFocus(MainWindowTitle, MainWindowText, "[CLASS:TCMBStyleEdit72; INSTANCE:4]");

                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void FillBankTransInfo(IntPtr mainFormWindow, RobotContext context)
        {
            IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:Edit; INSTANCE:1]");
            EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit72; INSTANCE:5]");
            EnterTextBox(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            IntPtr searchComboBoxToAccountBank = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox72; INSTANCE:5]");
            AutoItX.ControlClick(mainFormWindow, searchComboBoxToAccountBank);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit72; INSTANCE:4]");
            EnterTextBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox72; INSTANCE:2]");
            EnterPinBox(mainFormWindow, textPostscript, context.BoTransactionId);
        }

        private void FillOtp(IntPtr mainFormWindow, RobotContext context)
        {
            ClickButton(mainFormWindow, 550, 410); // click '获取短信验证码' button

            int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码", 5);
            if (warningHappen1 == AutoItXSuccess)
            {
                IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码");
                ClickButton(warningPopWin1, 250, 170);
            }
            int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功", 10);
            if (warningHappen2 == AutoItXSuccess)
            {
                IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功");
                ClickButton(warningPopWin2, 300, 160);
            }

            // wait to get OTP
            context.Otp = "6543210";

            IntPtr textOtpBox = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBEdit; INSTANCE:1]");
            EnterOtpBox(mainFormWindow, textOtpBox, context.Otp);
            ClickButton(mainFormWindow, 420, 610); // click 'Next' button
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private RobotResult DoLogOut(RobotContext context)
        {
            try
            {
                IntPtr mainFormWindow = GetMainFormWindow();
                AutoItX.WinActivate(mainFormWindow);

                Rectangle mainWindowPosition = AutoItX.WinGetPos(mainFormWindow);
                ClickButton(mainFormWindow, mainWindowPosition.Width - 150, 10);

                int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TAppExitForm]", "", 5);
                if (warningHappen1 == AutoItXSuccess)
                {
                    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TAppExitForm]");
                    ClickButton(warningPopWin1, 110, 190);
                }
                int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑", 5);
                if (warningHappen2 == AutoItXSuccess)
                {
                    IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑");
                    ClickButton(warningPopWin2, 250, 210);
                }
                int warningHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版", 5);
                if (warningHappen3 == AutoItXSuccess)
                {
                    IntPtr warningPopWin3 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版");
                    ClickButton(warningPopWin3, 250, 180);
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }
        #endregion

        #region Helper Methods
        private IntPtr GetMainFormWindow()
        {
            return AutoItX.WinGetHandle(MainWindowTitle, MainWindowText);
        }

        private void EnterOtpBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                ClickToFocus(mainWindow, textBox);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                SimulateKey.SetForegroundWindow(textBox);
                SimulateKey.ClearText(textBox);
                SimulateKey.SendText(textBox, value);
                AutoItX.Sleep(GetRandomDelay(1000));
            }
        }

        private void EnterPinBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                ClickToFocus(mainWindow, textBox);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.Send(value);
                AutoItX.Sleep(GetRandomDelay(1000));
            }
        }

        private void EnterTextBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            ClickToFocus(mainWindow, textBox);
            AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
            AutoItX.ControlSetText(mainWindow, textBox, value);
            AutoItX.Sleep(GetRandomDelay(1000));
        }

        private void ClickToFocus(IntPtr mainWindow, IntPtr refElement)
        {
            ClearTextBox(mainWindow, refElement);
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            Rectangle refElementPosition = AutoItX.ControlGetPos(mainWindow, refElement);
            int startX = mainWindowPosition.X + refElementPosition.X;
            int startY = mainWindowPosition.Y + refElementPosition.Y;
            AutoItX.WinActivate(mainWindow);
            ClickElement(startX, startY, 10, 10);
        }

        private void ClickButton(IntPtr mainWindow, int offsetX, int offsetY)
        {
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            AutoItX.WinActivate(mainWindow);
            ClickElement(mainWindowPosition.X, mainWindowPosition.Y, offsetX, offsetY);
        }

        private void ClickElement(int startX, int startY, int offsetX, int offsetY)
        {
            int elementPossitionX = startX + offsetX;
            int elementPossitionY = startY + offsetY;
            AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
            AutoItX.MouseClick("LEFT", elementPossitionX, elementPossitionY);
            AutoItX.Sleep(GetRandomDelay(1000));
        }

        private int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(1, 3);
            return value * multiplier;
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
        #endregion

    }

}

