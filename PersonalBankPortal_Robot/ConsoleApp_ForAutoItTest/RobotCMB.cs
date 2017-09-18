using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using AutoIt;
using ConsoleApp_ForAutoItTest.SendKeyMessage;
using NLog;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private const int AutoItXSuccess = 1;
        private const string LoginFormTitle = "招商银行个人银行专业版";
        private const string LoginFormClass = "TWealthLoginFrm";

        private const string MainWindowTitle = "[TITLE:招商银行个人银行专业版; CLASS:TMainFrm]";
        private const string MainWindowClass = "TMainFrm";
        private const string MainWindowText = "功能";
        private const int HWND_BROADCAST = 0xffff;
        private delegate RobotResult FundOutStep(RobotContext context);

        private InputSimulator inputSimulator = new InputSimulator();

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
                string programFullPath = "D:\\CMB\\Locale.Emulator.2.3.1.1";
                Process process = new Process();
                process.StartInfo.FileName = "LEProc.exe";
                process.StartInfo.WorkingDirectory = programFullPath;
                process.StartInfo.Arguments = "C:\\Windows\\syswow64\\PersonalBankPortal.exe";
                process.Start();

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
                EnterPassword();

                inputSimulator.Mouse.MoveMouseTo(34000, 42700);
                inputSimulator.Mouse.LeftButtonClick();

                Thread.Sleep(10000);

                //WaitUtils.UntilWinActive(MainWindowTitle, MainWindowText);
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
                SimulateKey.SetForegroundWindow(SimulateKey.FindWindow(MainWindowClass, null));
                SimulateKey.BringWindowToTop(SimulateKey.FindWindow(MainWindowClass, null));
                SimulateKey.SetForegroundWindow(SimulateKey.FindWindow(MainWindowClass, null));
                SimulateKey.BringWindowToTop(SimulateKey.FindWindow(MainWindowClass, null));


                inputSimulator.Mouse.MoveMouseTo(5500, 25000); //Transfer Button
                Thread.Sleep(GetRandomNumber(100));
                inputSimulator.Mouse.LeftButtonClick();

                Thread.Sleep(GetRandomNumber(2000));

                //ClickButton(mainFormWindow, 60, 320); // click Transfer button, default 'Same-bank transfer'
                //WaitUtils.UntilControlFocus(MainWindowTitle, MainWindowText, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");



                FillSameBankTransInfo(context);
                FillTransferPassword(context);

               
                return RobotResult.Build(context, RobotStatus.SUCCESS, "");
            }
            catch (Exception e)
            {
                return RobotResult.Build(context, RobotStatus.ERROR, e.Message);
            }
        }

        private void FillSameBankTransInfo(RobotContext context)
        {
            //IntPtr textToAccountName = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:2]");
            ////EnterTextBox(mainFormWindow, textToAccountName, context.ToAccountName);

            //ClickToFocus(mainFormWindow, textToAccountName);
            //AutoItX.ClipPut(context.ToAccountName);
            Thread.Sleep(2000);


            Thread thread = new Thread(() => Clipboard.SetText(context.ToAccountName));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
            
            inputSimulator.Mouse.MoveMouseTo(14000, 25500); //Account Name
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();

            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode) VirtualKeyCode.LCONTROL);
            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode) VirtualKeyCode.LCONTROL);
            Thread.Sleep(GetRandomNumber(100));
            Thread.Sleep(5000);


            

            thread = new Thread(() => Clipboard.SetText(context.ToAccountNumber));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();

            inputSimulator.Mouse.MoveMouseTo(14000, 28200); //Account Number
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();

            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.LCONTROL);
            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.LCONTROL);
            Thread.Sleep(GetRandomNumber(100));
            Thread.Sleep(5000);





            inputSimulator.Mouse.MoveMouseTo(14000, 33500); //Amount
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();


            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_1);




            inputSimulator.Mouse.MoveMouseTo(15000, 43500); //Transfer Okay
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();

            Thread.Sleep(GetRandomNumber(1000));
            inputSimulator.Mouse.MoveMouseTo(31000, 36500); //Transfer Okay Confirm
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();


            Thread.Sleep(GetRandomNumber(2000));
            //IntPtr textToAccountNumber = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:3]");
            //EnterAccountNumber(mainFormWindow, textToAccountNumber, context.ToAccountNumber);

            ////IntPtr searchComboBoxToAccountCity = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBSearchComboBox; INSTANCE:1]");
            ////SearchAndSelectComboBox(mainFormWindow, searchComboBoxToAccountCity, context.ToAccountCity);
            //Thread.Sleep(2000);
            //IntPtr textTransferAmount = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleEdit; INSTANCE:4]");
            //EnterOtpBox(mainFormWindow, textTransferAmount, context.WithdrawAmount);

            //IntPtr textPostscript = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBStyleComboBox; INSTANCE:1]");
            ////EnterOtpBox(mainFormWindow, textPostscript, context.BoTransactionId);

            //ClickButton(mainFormWindow, 350, 640); // click 'Next' button
            //int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "选择的收款方地址与收款方账户所属开户地不符", 10);
            //if (warningHappen1 == AutoItXSuccess)
            //{
            //    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "选择的收款方地址与收款方账户所属开户地不符");
            //    ClickButton(warningPopWin1, 300, 150);
            //}
        }



        private void FillTransferPassword(RobotContext context)
        {
            //ClickButton(mainFormWindow, 550, 410); // click '获取短信验证码' button

            //int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码", 5);
            //if (warningHappen1 == AutoItXSuccess)
            //{
            //    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "选择通过短信方式获取验证码");
            //    ClickButton(warningPopWin1, 250, 170);
            //}
            //int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功", 10);
            //if (warningHappen2 == AutoItXSuccess)
            //{
            //    IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "通过短信方式获取验证码的请求提交成功");
            //    ClickButton(warningPopWin2, 300, 160);
            //}

            // wait to get OTP
            context.Otp = "452541";

            var y =  AutoItX.ControlGetHandle(SimulateKey.FindWindow(MainWindowClass, null), "[CLASS:TCMBEdit; INSTANCE:1]");


            //
            //0x00d904fc
            inputSimulator.Mouse.MoveMouseTo(18000, 34500); //Transferpassword
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();


            Thread.Sleep(GetRandomNumber(10));
            SimulateKey.SendText(y,context.Otp);

            Thread.Sleep(GetRandomNumber(10));

            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_4);
            //Thread.Sleep(GetRandomNumber(50));
            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_5);
            //Thread.Sleep(GetRandomNumber(50));
            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_2);
            //Thread.Sleep(GetRandomNumber(50));
            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_5);
            //Thread.Sleep(GetRandomNumber(50));
            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_4);
            //Thread.Sleep(GetRandomNumber(50));
            //inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_1);


            inputSimulator.Mouse.MoveMouseTo(18000, 36500); //Transfer Okay Confirm
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();


            //IntPtr textOtpBox = AutoItX.ControlGetHandle(mainFormWindow, "[CLASS:TCMBEdit; INSTANCE:1]");
            //EnterOtpBox(mainFormWindow, textOtpBox, context.Otp);
            //ClickButton(mainFormWindow, 420, 530); // click 'Next' button
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private RobotResult DoLogOut(RobotContext context)
        {
            try
            {
                //IntPtr mainFormWindow = GetMainFormWindow();
                //AutoItX.WinActivate(mainFormWindow);

                //Rectangle mainWindowPosition = AutoItX.WinGetPos(mainFormWindow);
                //ClickButton(mainFormWindow, mainWindowPosition.Width - 150, 10);

                //int warningHappen1 = AutoItX.WinWaitActive("[CLASS:TAppExitForm]", "", 5);
                //if (warningHappen1 == AutoItXSuccess)
                //{
                //    IntPtr warningPopWin1 = AutoItX.WinGetHandle("[CLASS:TAppExitForm]");
                //    ClickButton(warningPopWin1, 110, 190);
                //}
                //int warningHappen2 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑", 5);
                //if (warningHappen2 == AutoItXSuccess)
                //{
                //    IntPtr warningPopWin2 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "移动证书优KEY还插在电脑");
                //    ClickButton(warningPopWin2, 250, 170);
                //}
                //int warningHappen3 = AutoItX.WinWaitActive("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版", 5);
                //if (warningHappen3 == AutoItXSuccess)
                //{
                //    IntPtr warningPopWin3 = AutoItX.WinGetHandle("[CLASS:TPbBaseMsgForm]", "再次确认是否要不拔掉优KEY退出专业版");
                //    ClickButton(warningPopWin3, 260, 160);
                //}
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

      
        private void EnterAccountNumber(IntPtr mainWindow, IntPtr textBox, string value)
        {

            //ClickToFocus(mainWindow, textBox);
            //AutoItX.ClipPut(value);

            //var inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.LCONTROL);
            inputSimulator.Keyboard.KeyDown((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_V);
            inputSimulator.Keyboard.KeyUp((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.LCONTROL);
            Thread.Sleep(GetRandomNumber(100));


            
        }

        private void EnterPassword()
        {
            Thread.Sleep(GetRandomNumber(500));

            ////AutoItX.MouseMove(840, 480);
            inputSimulator.Mouse.LeftButtonClick();
            inputSimulator.Mouse.MoveMouseTo(34000, 42700); //Login

            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();
            inputSimulator.Mouse.MoveMouseTo(34000, 36000); // Message box
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();
            Thread.Sleep(GetRandomNumber(100));
            

            SimulateKey.SetForegroundWindow(SimulateKey.FindWindow(LoginFormClass, null));
            SimulateKey.BringWindowToTop(SimulateKey.FindWindow(LoginFormClass, null));
            SimulateKey.SetForegroundWindow(SimulateKey.FindWindow(LoginFormClass, null));
            SimulateKey.BringWindowToTop(SimulateKey.FindWindow(LoginFormClass, null));



            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.MoveMouseTo(34000,39100);

            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Mouse.LeftButtonClick();
            Thread.Sleep(GetRandomNumber(100));

            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_A);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_A);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_2);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_5);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_4);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_1);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_7);
            Thread.Sleep(GetRandomNumber(100));
            inputSimulator.Keyboard.KeyPress((WindowsInput.Native.VirtualKeyCode)VirtualKeyCode.VK_2);

        }
        

        private int GetRandomNumber(int multiplier)
        {
            var rnd = new Random();
            var value = rnd.Next(3, 7);
            return value * multiplier;
        }

        #endregion

    }

}

