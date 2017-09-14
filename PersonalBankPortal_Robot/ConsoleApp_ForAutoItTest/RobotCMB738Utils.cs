using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;
using ConsoleApp_ForAutoItTest.SendKeyMessage;
using NLog;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotCMB738Utils
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        public const int AutoItXSuccess = 1;
        private const string ErrorWindowTitle = "[TITLE:错误; CLASS:TErrorWithHelpForm]";
        public static int TimeOut = 60;
        public static int SleepInterval = 2;

        public static void UntilWinActive(string midasTxnId, string winTitle, string winText)
        {
            DateTime otherDateTime = DateTime.Now.AddSeconds(TimeOut);
            LOG.Log(LogLevel.Debug, "CheckWinActive Start By<{0}|{1}>", winTitle, winText);
            while (true)
            {
                int winActive = AutoItX.WinActive(winTitle, winText);
                if (winActive == AutoItXSuccess)
                {
                    break;
                }
                LOG.Log(LogLevel.Debug, "CheckWinActive Loop By<{0}|{1}>", winTitle, winText);
                int errorWinActive = AutoItX.WinActive(ErrorWindowTitle);
                if (errorWinActive == AutoItXSuccess)
                {
                    LOG.Error("CheckWinActive Error By<{0}|{1}>", winTitle, winText);
                    SaveErrorShot(midasTxnId);
                    AutoItX.WinClose(ErrorWindowTitle);
                    throw new Exception("Check Win Active Error");
                }
                if (DateTime.Compare(DateTime.Now, otherDateTime) > 0)
                {
                    LOG.Warn("CheckWinActive TimeOut By<{0}|{1}>", winTitle, winText);
                    SaveErrorShot(midasTxnId);
                    throw new Exception("Check Win Active Timeout");
                }
                Thread.Sleep(TimeSpan.FromSeconds(SleepInterval));
            }
        }

        public static void UntilControlFocus(string midasTxnId, string winTitle, string winText, string control)
        {
            DateTime otherDateTime = DateTime.Now.AddSeconds(TimeOut);
            LOG.Log(LogLevel.Debug, "CheckControlFocus Start By<{0}|{1}>", winTitle, winText);
            while (true)
            {
                int controlFocus = AutoItX.ControlFocus(winTitle, winText, control);
                if (controlFocus == AutoItXSuccess)
                {
                    break;
                }
                LOG.Log(LogLevel.Debug, "CheckControlFocus Loop By<{0}|{1}|{2}>", winTitle, winText, control);
                int errorWinActive = AutoItX.WinActive(ErrorWindowTitle);
                if (errorWinActive == AutoItXSuccess)
                {
                    LOG.Error("ControlFocus Error By<{0}|{1}|{2}>", winTitle, winText, control);
                    SaveErrorShot(midasTxnId);
                    AutoItX.WinClose(ErrorWindowTitle);
                    throw new Exception("Check Control Focus Error");
                }
                if (DateTime.Compare(DateTime.Now, otherDateTime) > 0)
                {
                    LOG.Warn("CheckControlFocus TimeOut By<{0}|{1}|{2}>", winTitle, winText, control);
                    SaveErrorShot(midasTxnId);
                    throw new Exception("Check Control Focus Timeout");
                }
                Thread.Sleep(TimeSpan.FromSeconds(SleepInterval));
            }

        }

        public static void SaveErrorShot(string midasTransactionId)
        {
            CatchScreenshot.SaveWinShot(AutoItX.WinGetHandle("[ACTIVE]"), midasTransactionId);
        }

    }

}
