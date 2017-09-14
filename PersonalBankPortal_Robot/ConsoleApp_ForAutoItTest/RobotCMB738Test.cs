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
    class RobotCMB738Test
    {
        public static void Main1(string[] args)
        {
            // 詹德宏 6214832004002676 181081 广州分行越秀支行 aa254172 452541   13168135761
            Console.WriteLine("---------------------------1234---------------------------");
            string[] bankNames = new[] { "交通银行", "招商银行", "中国工商银行" };
            string[] acctNames = new[] { "阿拉木斯", "吕文斌", "李忠谦" };
            string[] acctNums = new[] { "6222620910034272595", "6214837694277025", "6212262102018676967" };

            int loopTimes = 1;
            for (int i = 0; i < loopTimes; i++)
            {
                RobotContext context = new RobotContext();
                context.MidasTransactionId = "100100" + i;
                context.LoginPassword = "aa254172";
                context.ToBankName = bankNames[i];
                context.ToAccountName = acctNames[i];
                context.ToAccountNumber = acctNums[i];
                context.WithdrawAmount = "1.00";
                context.BoTransactionId = "123-4567-890" + i;
                Robot738CMB robot = new Robot738CMB();
                robot.Transfer(context);
                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
            Console.WriteLine("---------------------------5678---------------------------");
            Console.ReadKey();
        }
    }
}
