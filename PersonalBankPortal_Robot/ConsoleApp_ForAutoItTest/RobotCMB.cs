using AutoIt;
using System;
using System.Drawing;
using System.Threading;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB
    {
        private const int AutoItXSuccess = 1;
        private const string LoginFormTitle = "招商银行个人银行专业版";

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

        #region Step Methods
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
                IntPtr loginFormWindow = AutoItX.WinGetHandle(LoginFormTitle);
                IntPtr textPassBox = AutoItX.ControlGetHandle(loginFormWindow, "[CLASS:TCMBStyleEdit72]");
                EnterPinBox(loginFormWindow, textPassBox, loginPassword);
                ClickButton(loginFormWindow, 200, 400);

                int errorHappen1 = AutoItX.WinWaitActive("[TITLE:错误; CLASS:TErrorWithHelpForm]", "", 5); //token key not plugin
                if (errorHappen1 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed1, Error<Authentication Key Missing>");
                }
                int errorHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "", 10); //login password validate
                if (errorHappen2 == AutoItXSuccess)
                {
                    string errorText = AutoItX.WinGetText("[CLASS:TPbBaseMsgForm]");
                    AutoItX.WinClose("[CLASS:TPbBaseMsgForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, $"Login Failed2, Error<{errorText.Trim()}>");
                }
                int errorHappen3 = AutoItX.WinWaitActive("[TITLE:招商银行个人银行专业版; CLASS:TMainFrm]", "功能", 60); //main portal window
                if (errorHappen3 == AutoItXSuccess)
                {
                    //Thread.Sleep(TimeSpan.FromSeconds(3)); // sleep wait for [CLASS:Internet Explorer_Server] load done
                    return RobotResult.Build(context, RobotStatus.SUCCESS, "Login Success, Awesome!");
                }
                int errorHappen4 = AutoItX.WinWaitActive("[TITLE:错误;CLASS: TErrorWithHelpForm]", "", 5); //main portal window
                if (errorHappen4 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误; CLASS:TErrorWithHelpForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, "Login Failed3, Error<Handshake Fault>");
                }
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
                IntPtr mainFormWindow = GetMainFormWindow();
                AutoItX.WinActivate(mainFormWindow);

                ClickButton(mainFormWindow, 50, 80); // click HomePage button
                ClickButton(mainFormWindow, 60, 330); // click Transfer button, default 'Same-bank transfer'

                if (string.IsNullOrEmpty(context.ToBankName))
                {
                    FillSameBankTransInfo(mainFormWindow, context);
                    FillOtp(mainFormWindow, context);
                }
                else
                {
                    FillInterBankTransInfo(mainFormWindow, context);
                    FillOtp(mainFormWindow, context);
                }

                int errorHappen1 = AutoItX.WinWaitActive("[TITLE:错误; CLASS:TErrorWithHelpForm]", "", 5); //transfer pre-check failed
                if (errorHappen1 == AutoItXSuccess)
                {
                    AutoItX.WinClose("[TITLE:错误;CLASS:TErrorWithHelpForm]");
                    return RobotResult.Build(context, RobotStatus.ERROR, "Transfer Failed1, Error<Withdraw Validate Failed>");
                }

                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void FillSameBankTransInfo(IntPtr mainFormWindow, RobotContext context)
        {
            ClickButton(mainFormWindow, 100, 210); // click 'Same-bank transfer' button

            IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:2]");
            EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:3]");
            EnterTextBox(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            IntPtr searchComboBoxToAccountCity = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");
            SearchAndSelectComboBox(mainFormWindow, searchComboBoxToAccountCity, context.ToAccountCity);

            IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:4]");
            EnterTextBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox; INSTANCE:1]");
            EnterPinBox(mainFormWindow, textPostscript, context.WithdrawTransactionId);

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
            ClickButton(mainFormWindow, 210, 210); // click 'Inter-bank transfer' button

            IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:1]");
            EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:2]");
            EnterTextBox(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            IntPtr transferTypeImmediate = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleRadioButton; INSTANCE:1]"); //immediate
            AutoItX.ControlClick(mainFormWindow, transferTypeImmediate);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            IntPtr searchComboBoxToBankName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");
            SearchAndSelectComboBox(mainFormWindow, searchComboBoxToBankName, context.ToBankName);

            IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:4]");
            EnterTextBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox; INSTANCE:2]");
            EnterPinBox(mainFormWindow, textPostscript, context.WithdrawTransactionId);

            ClickButton(mainFormWindow, 350, 660); // click 'Next' button
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private void FillOtp(IntPtr mainFormWindow, RobotContext context)
        {
            ClickButton(mainFormWindow, 550, 412); // click '获取短信验证码' button
            int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码", 5);
            if (warningHappen1 == AutoItXSuccess)
            {
                IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码");
                ClickButton(warningPopWin1, 250, 170);
            }
            int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功", 5);
            if (warningHappen2 == AutoItXSuccess)
            {
                IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功");
                ClickButton(warningPopWin2, 300, 160);
            }

            // wait to get OTP
            context.Otp = "123456";

            IntPtr textOtpBox = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBEdit; INSTANCE:1]");
            EnterPinBox(mainFormWindow, textOtpBox, context.Otp);
            ClickButton(mainFormWindow, 420, 612); // click 'Next' button
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private RobotResult DoLogOut(RobotContext context)
        {
            try
            {
                IntPtr mainFormWindow = GetMainFormWindow();
                AutoItX.WinActivate(mainFormWindow);
                Rectangle mainWindowPosition = AutoItX.WinGetPos(mainFormWindow);
                ClickButton(mainFormWindow, mainWindowPosition.Width - 140, 17);

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
            return AutoItX.WinGetHandle("[TITLE:招商银行个人银行专业版; CLASS:TMainFrm]", "功能");
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

            Thread.Sleep(TimeSpan.FromSeconds(1)); // stop to make sure the dropdown selected done
        }

        private void EnterPinBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            if (AutoItX.ControlFocus(mainWindow, textBox) == AutoItXSuccess)
            {
                ClickToFocus(mainWindow, textBox);
                AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
                AutoItX.Send(value);
                Thread.Sleep(GetRandomDelay(1000));
            }
        }

        private void EnterTextBox(IntPtr mainWindow, IntPtr textBox, string value)
        {
            ClickToFocus(mainWindow, textBox);
            AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
            AutoItX.ControlSetText(mainWindow, textBox, value);
            Thread.Sleep(GetRandomDelay(1000));
        }

        private void ClickToFocus(IntPtr mainWindow, IntPtr refElement)
        {
            ClearTextBox(mainWindow, refElement);
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            Rectangle refElementPosition = AutoItX.ControlGetPos(mainWindow, refElement);
            int startX = mainWindowPosition.X + refElementPosition.X;
            int startY = mainWindowPosition.Y + refElementPosition.Y;
            ClickElement(startX, startY, 10, 10);
        }

        private void ClickButton(IntPtr mainWindow, int offsetX, int offsetY)
        {
            Rectangle mainWindowPosition = AutoItX.WinGetPos(mainWindow);
            ClickElement(mainWindowPosition.X, mainWindowPosition.Y, offsetX, offsetY);
        }

        private void ClickElement(int startX, int startY, int offsetX, int offsetY)
        {
            int elementPossitionX = startX + offsetX;
            int elementPossitionY = startY + offsetY;
            AutoItX.AutoItSetOption("SendKeyDelay", GetRandomDelay(100));
            AutoItX.MouseClick("LEFT", elementPossitionX, elementPossitionY);
            Thread.Sleep(GetRandomDelay(1000));
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

