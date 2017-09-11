using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;

namespace ConsoleApp_ForAutoItTest
{
    class RobotCMBTest
    {
        public static void Main1(string[] args)
        {
            Console.WriteLine("---------------------------1234---------------------------");
            int loopTimes = 1;
            for (int i = 0; i < loopTimes; i++)
            {
                RobotContext context = new RobotContext();
                context.LoginPassword = "gh202123";
                context.ToBankName = "厦门国际银行";
                context.ToAccountName = "吕文斌";
                context.ToAccountNumber = "6214837694277025";
                context.WithdrawAmount = "1001.01";
                context.WithdrawTransactionId = "123-4567-8901";
                RobotCMB robot = new RobotCMB();
                robot.Transfer(context);
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("---------------------------5678---------------------------");
            Console.ReadKey();
        }
    }
}
