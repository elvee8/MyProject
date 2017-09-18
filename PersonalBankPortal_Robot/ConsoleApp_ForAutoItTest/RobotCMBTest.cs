﻿using System;
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
            string[] bankNames = new[] { "", "招商银行", "" };
            int loopTimes = 1;
            for (int i = 0; i < loopTimes; i++)
            {
                RobotContext context = new RobotContext();
                context.MidasTransactionId = "100100" + i;
                context.LoginPassword = "aa254172";
                context.ToBankName = bankNames[i];
                context.ToAccountCity = "苏州";
                context.ToAccountName = "吕文斌";
                context.ToAccountNumber = "6214837694277025";
                context.WithdrawAmount = "1.00";
                context.BoTransactionId = "123-4567-8901";
                RobotCMB robot = new RobotCMB();
                robot.Transfer(context);
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("---------------------------5678---------------------------");
            Console.ReadKey();
        }
    }
}
