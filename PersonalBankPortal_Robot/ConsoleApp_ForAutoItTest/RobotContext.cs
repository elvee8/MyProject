using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotContext
    {
        public string MidasTransactionId { get; set; }
        public string FromAccountNumber { get; set; }
        public string FromAccountName { get; set; }
        public string LoginPassword { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToAccountName { get; set; }
        public string ToAccountCity { get; set; }
        public string ToBankName { get; set; }
        public string WithdrawAmount { get; set; }
        public string BoTransactionId { get; set; }
        public string Otp { get; set; }
        public string TokenWithdrawPin { get; set; }
    }

}
