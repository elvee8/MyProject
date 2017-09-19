using AutoIt;
using ConsoleApp_ForAutoItTest.SendKeyMessage;
using NLog;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using WindowsInput;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private const string LocaleEmulatorWorkingDirectory = @"D:\CMB\Locale.Emulator.2.3.1.1";
        private const string LocaleEmulatorFileName = "LEProc.exe";
        private const string PersonalBankPortalPath = @"C:\Windows\syswow64\PersonalBankPortal.exe";
        private const string ProcessName = "PersonalBankPortal.exe";
        private const int AutoItXSuccess = 1;
        private const string LoginFormTitle = "[TITLE:招商银行个人银行专业版; CLASS:TWealthLoginFrm]";
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
                TryToKillOldProcess(context.MidasTransactionId);

                Process process = new Process();
                process.StartInfo.FileName = LocaleEmulatorFileName;
                process.StartInfo.WorkingDirectory = LocaleEmulatorWorkingDirectory;
                process.StartInfo.Arguments = PersonalBankPortalPath;
                process.Start();

                Thread.Sleep(TimeSpan.FromSeconds(2));
                return RobotResult.Build(context, RobotStatus.SUCCESS, "Open Client App Success!");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void TryToKillOldProcess(string midasTxnId)
        {
            int processExists = AutoItX.ProcessExists(ProcessName);
            if (processExists != 0)
            {
                int processClose = AutoItX.ProcessClose(ProcessName);
                if (processClose == AutoItXSuccess)
                {
                    LOG.Log(LogLevel.Debug, "TransactionId<{0}>, Kill old process<{1}> done", midasTxnId, processExists);
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                }
            }
        }

        private RobotResult DoLogIn(RobotContext context)
        {
            try
            {
                IntPtr loginFormWindow = AutoItX.WinGetHandle(LoginFormTitle);
                IntPtr textPassBox = AutoItX.ControlGetHandle(loginFormWindow, "[CLASS:TCMBStyleEdit72]");
                EnterLoginBox(loginFormWindow, textPassBox, context.LoginPassword);
                ClickButton(loginFormWindow, textPassBox, 70, 70);

                int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //login password validate
                if (warningHappen1 == AutoItXSuccess)
                {
                    string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                    AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, $"Login Failed, Error<{errorText.Trim()}>");
                }

                WaitUtils.UntilWinActive(MainWindowTitle, MainWindowText);
                Thread.Sleep(TimeSpan.FromSeconds(3)); // sleep wait for [CLASS:Internet Explorer_Server] load done
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

                ClickButton(mainFormWindow, 60, 320); // click Transfer button, default 'Same-bank transfer'
                WaitUtils.UntilControlFocus(MainWindowTitle, MainWindowText, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");

                if (string.IsNullOrEmpty(context.ToBankName))
                {
                    FillSameBankTransInfo(mainFormWindow, context);
                }
                else
                {
                    ClickButton(mainFormWindow, 250, 210); // click 'Inter-bank transfer' button
                    WaitUtils.UntilControlFocus(MainWindowTitle, MainWindowText, "[CLASS:TCMBStyleRadioButton; INSTANCE:1]");
                    FillInterBankTransInfo(mainFormWindow, context);
                }
                FillWithdrawPin(mainFormWindow, context);

                int errorHappen1 = AutoItX.WinWaitActive("[TITLE:错误; CLASS:TErrorWithHelpForm]", "", 5); //transfer pre-check failed
                if (errorHappen1 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, "Transfer Failed1, Error<Withdraw Validate Failed>");
                }

                WaitUtils.UntilWinActive("[CLASS:TTransferSuccessFrm]", context.ToAccountName);

                Thread.Sleep(GetRandomDelay(100));
                AutoItX.WinClose("[CLASS:TTransferSuccessFrm]");
                Thread.Sleep(GetRandomDelay(100));

                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void FillSameBankTransInfo(IntPtr mainFormWindow, RobotContext context)
        {
            IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:2]");
            EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:3]");
            EnterTextBox(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            IntPtr searchComboBoxToAccountCity = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");
            SearchAndSelectComboBox(mainFormWindow, searchComboBoxToAccountCity, context.ToAccountCity);

            IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:4]");
            EnterTextBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox; INSTANCE:1]");
            EnterComboBoxBox(mainFormWindow, textPostscript, context.BoTransactionId);

            ClickButton(mainFormWindow, 350, 640); // click 'Next' button
            int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "选择的收款方地址与收款方账户所属开户地不符", 10);
            if (warningHappen1 == AutoItXSuccess)
            {
                IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "选择的收款方地址与收款方账户所属开户地不符");
                ClickButton(warningPopWin1, 300, 150);
            }
        }

        private void FillInterBankTransInfo(IntPtr mainFormWindow, RobotContext context)
        {
            IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:1]");
            EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:2]");
            EnterTextBox(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            IntPtr transferTypeImmediate = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleRadioButton; INSTANCE:1]"); //immediate
            AutoItX.ControlClick(mainFormWindow, transferTypeImmediate);
            Thread.Sleep(GetRandomDelay(100));

            IntPtr searchComboBoxToBankName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");
            SearchAndSelectComboBox(mainFormWindow, searchComboBoxToBankName, context.ToBankName);

            IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:4]");
            EnterTextBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox; INSTANCE:2]");
            EnterComboBoxBox(mainFormWindow, textPostscript, context.BoTransactionId);

            ClickButton(mainFormWindow, 350, 660); // click 'Next' button
            Thread.Sleep(TimeSpan.FromSeconds(2));
        }

        private void FillWithdrawPin(IntPtr mainFormWindow, RobotContext context)
        {
            IntPtr textPinBox = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBEdit; INSTANCE:1]");
            EnterWithdrawPinBox(mainFormWindow, textPinBox, context.TokenWithdrawPin);
            ClickButton(mainFormWindow, textPinBox, 70, 50); // click 'Next' button
            Thread.Sleep(GetRandomDelay(1000));
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
                    ClickButton(warningPopWin2, 250, 170);
                }
                int warningHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版", 5);
                if (warningHappen3 == AutoItXSuccess)
                {
                    IntPtr warningPopWin3 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版");
                    ClickButton(warningPopWin3, 260, 160);
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

        private void SearchAndSelectComboBox(IntPtr mainWindow, IntPtr searchComboBox, string value)
        {
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            Rectangle refElementPosition = AutoItX.ControlGetPos(mainWindow, searchComboBox);
            int startX = mainWindowPosition.X + refElementPosition.X;
            int startY = mainWindowPosition.Y + refElementPosition.Y;
            int elementPossitionX = startX + 10;
            int elementPossitionY = startY + 10;
            AutoItX.MouseMove(elementPossitionX, elementPossitionY);

            AutoItX.MouseClickDrag("LEFT", elementPossitionX, elementPossitionY, elementPossitionX + 100, elementPossitionY + 30, 100);
            AutoItX.MouseDown();
            AutoItX.MouseUp();

            AutoItX.ClipPut(value);
            AutoItX.Send("^v");

            AutoItX.MouseMove(elementPossitionX + 100, elementPossitionY + 70);
            AutoItX.MouseDown();
            AutoItX.MouseUp();

            Thread.Sleep(GetRandomDelay(100)); // stop to make sure the dropdown selected done
        }

        private void EnterWithdrawPinBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                Thread.Sleep(GetRandomDelay(100));
                SimulateKey.SendText(textBox, value);
                Thread.Sleep(GetRandomDelay(100));
            }
        }

        private void EnterComboBoxBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                ClickToFocus(mainWindow, textBox);
                Thread.Sleep(GetRandomDelay(100));
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(50));
                AutoItX.Send(value);
                Thread.Sleep(GetRandomDelay(100));
            }
        }

        private void EnterLoginBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                InputSimulator inputSimulator = new InputSimulator();
                foreach (char item in value)
                {
                    Thread.Sleep(GetRandomDelay(50));
                    inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)SimulateKey.GetKeyCode(item));
                }
            }
        }

        private void EnterTextBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                Thread.Sleep(GetRandomDelay(100));
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.ControlSetText(mainWindow, textBox, value);
                Thread.Sleep(GetRandomDelay(100));
            }
        }

        private void ClickButton(IntPtr mainWindow, int offsetX, int offsetY)
        {
            AutoItX.WinActivate(mainWindow);
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            ClickElement(mainWindowPosition.X, mainWindowPosition.Y, offsetX, offsetY);
        }

        private void ClickButton(IntPtr mainWindow, IntPtr refElement, int offsetX, int offsetY)
        {
            AutoItX.WinActivate(mainWindow);
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            Rectangle refElementPosition = AutoItX.ControlGetPos(mainWindow, refElement);
            int startX = mainWindowPosition.X + refElementPosition.X;
            int startY = mainWindowPosition.Y + refElementPosition.Y;
            ClickElement(startX, startY, offsetX, offsetY);
        }

        private void ClickToFocus(IntPtr mainWindow, IntPtr refElement)
        {
            ClickButton(mainWindow, refElement, 10, 10);
        }

        private void ClickElement(int startX, int startY, int offsetX, int offsetY)
        {
            int elementPossitionX = startX + offsetX;
            int elementPossitionY = startY + offsetY;
            AutoItX.MouseClick("LEFT", elementPossitionX, elementPossitionY);
            Thread.Sleep(GetRandomDelay(100));
        }

        private int GetRandomDelay(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(3, 7);
            return value * multiplier;
        }
        #endregion

    }

}

