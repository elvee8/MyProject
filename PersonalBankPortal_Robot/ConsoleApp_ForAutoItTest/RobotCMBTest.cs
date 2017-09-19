using System;
using System.Threading;

namespace ConsoleApp_ForAutoItTest
{
    class RobotCMBTest
    {
        public static void Main1(string[] args)
        {
            Console.WriteLine("---------------------------1234---------------------------");
            string[] bankNames = { "", "交通银行", "" };
            string[] acctNames = { "吕文斌", "阿拉木斯", "" };
            string[] acctNums = { "6214837694277025", "6222620910034272595", "" };
            int loopTimes = 1;
            for (int i = 0; i < loopTimes; i++)
            {
                RobotContext context = new RobotContext();
                context.MidasTransactionId = "100100" + i;
                context.LoginPassword = "aa254172";
                context.TokenWithdrawPin = "452541";
                context.ToBankName = bankNames[i];
                context.ToAccountCity = "苏州";
                context.ToAccountName = acctNames[i];
                context.ToAccountNumber = acctNums[i];
                context.WithdrawAmount = "1.00";
                context.BoTransactionId = "123-4567-890" + i;
                RobotCMB robot = new RobotCMB();
                robot.Transfer(context);
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            Console.WriteLine("---------------------------5678---------------------------");
            Console.ReadKey();
        }
    }
}
