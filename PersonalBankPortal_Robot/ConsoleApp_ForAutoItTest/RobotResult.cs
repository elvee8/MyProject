using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest
{
    public struct RobotResult
    {
        public RobotContext Context { get; set; }
        public RobotStatus Status { get; set; }
        public string Balance { get; set; }
        public string Fee { get; set; }

        public static RobotResult Default(RobotContext context)
        {
            return new RobotResult
            {
                Context = context,
                Status = RobotStatus.Default()
            };
        }

        public static RobotResult Build(RobotContext context, int code, string desc)
        {
            return new RobotResult
            {
                Context = context,
                Status = new RobotStatus(code, desc)
            };
        }

        public bool IsSuccess()
        {
            return Status.IsSuccess();
        }

    }
}
