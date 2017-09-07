using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest
{
    public class RobotStatus
    {
        public const int IN_PROCESS = 100;
        public const int SUCCESS = 200;

        public const int UNKNOWN = 0;
        public const int ERROR = 501;

        public int Code { get; set; }
        public string Description { get; set; }

        public static RobotStatus Default()
        {
            return new RobotStatus(){Code = UNKNOWN, Description = "Unknown"};
        }

        public RobotStatus()
        {
        }

        public RobotStatus(int code, string desc)
        {
            Code = code;
            Description = desc;
        }

        public bool IsSuccess()
        {
            return Code == IN_PROCESS || Code == SUCCESS;
        }

    }
}
