using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;
using NLog;

namespace ConsoleApp_ForAutoItTest
{
    public class WaitUtils
    {
        private static readonly Logger LOG = LogManager.GetCurrentClassLogger();
        private const string ErrorWindowTitle = "[TITLE:错误; CLASS:TErrorWithHelpForm]";
        public static int TimeOut = 60;
        public static int SleepInterval = 2;

        public static void UntilWinActive(string winTitle, string winText)
        {
            DateTime otherDateTime = DateTime.Now.AddSeconds(TimeOut);
            while (true)
            {
                int winActive = AutoItX.WinActive(winTitle, winText);
                if (winActive == 1)
                {
                    break;
                }
                LOG.Log(LogLevel.Debug, "WinActive Check By<{0}|{1}>", winTitle, winText);
                int errorWinActive = AutoItX.WinActive(ErrorWindowTitle);
                if (errorWinActive == 1)
                {
                    LOG.Error("CheckWinActive Error By<{0}|{1}>", winTitle, winText);
                    AutoItX.WinClose(ErrorWindowTitle);
                    throw new Exception("Check Win Active Error");
                }
                if (DateTime.Compare(DateTime.Now, otherDateTime) > 0)
                {
                    LOG.Warn("CheckWinActive TimeOut By<{0}|{1}>", winTitle, winText);
                    throw new Exception("Check Win Active Timeout");
                }
                Thread.Sleep(TimeSpan.FromSeconds(SleepInterval));
            }
        }

        public static void UntilControlFocus(string winTitle, string winText, string control)
        {
            DateTime otherDateTime = DateTime.Now.AddSeconds(TimeOut);
            while (true)
            {
                int controlFocus = AutoItX.ControlFocus(winTitle, winText, control);
                if (controlFocus == 1)
                {
                    break;
                }
                LOG.Log(LogLevel.Debug, "ControlFocus Check By<{0}|{1}|{2}>", winTitle, winText, control);
                int errorWinActive = AutoItX.WinActive(ErrorWindowTitle);
                if (errorWinActive == 1)
                {
                    LOG.Error("ControlFocus Error By<{0}|{1}|{2}>", winTitle, winText, control);
                    AutoItX.WinClose(ErrorWindowTitle);
                    throw new Exception("Check Control Focus Error");
                }
                if (DateTime.Compare(DateTime.Now, otherDateTime) > 0)
                {
                    LOG.Warn("CheckControlFocus TimeOut By<{0}|{1}|{2}>", winTitle, winText, control);
                    throw new Exception("Check Control Focus Timeout");
                }
                Thread.Sleep(TimeSpan.FromSeconds(SleepInterval));
            }

        }
    }

}
