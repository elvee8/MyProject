using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoIt;

namespace ConsoleApp_ForAutoItTest
{
    class TestAutoItX
    {
        public static void Main1(string[] args)
        {
            Console.WriteLine("123");

            TestNotepad();

            Console.WriteLine("456");
            Console.ReadKey();
        }

        public static void Main2(string[] args)
        {
            string x = "2,169,600.00";
            string y = x.Replace(",", "").Replace(".00", "").Trim();

            string a = "2.169.600";
            string b = a.Replace(".", "").Trim();

            Console.WriteLine(y);
            Console.WriteLine(b);
            Console.ReadKey();
        }

        static void TestNotepad()
        {
            AutoItX.Run("notepad.exe", "D:/MIDAS/CMB2");
            AutoItX.WinWaitActive("Untitled - Notepad");
            AutoItX.Send("This is some text.");
            AutoItX.WinClose("Untitled - Notepad");
            AutoItX.WinWaitActive("Notepad", "Cancel");
            //AutoItX.Send("!n");
        }

    }
}
