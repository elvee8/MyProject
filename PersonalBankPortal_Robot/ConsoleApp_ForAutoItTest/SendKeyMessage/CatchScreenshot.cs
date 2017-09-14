using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest.SendKeyMessage
{
    class CatchScreenshot
    {

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt
        (
            IntPtr hdcDest,    //目标DC的句柄  
            int nXDest,        //目标DC的矩形区域的左上角的x坐标  
            int nYDest,        //目标DC的矩形区域的左上角的y坐标  
            int nWidth,        //目标DC的句型区域的宽度值  
            int nHeight,       //目标DC的句型区域的高度值  
            IntPtr hdcSrc,     //源DC的句柄  
            int nXSrc,         //源DC的矩形区域的左上角的x坐标  
            int nYSrc,         //源DC的矩形区域的左上角的y坐标  
            System.Int32 dwRo  //光栅的处理数值  
        );

        //[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        //public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        public static void SaveWinShot(IntPtr hwnd1, string filename)
        {
            if (!hwnd1.Equals(IntPtr.Zero))
            {
                Rectangle rect;
                GetWindowRect(hwnd1, out rect);  //获得目标窗体的大小  
                Bitmap QQPic = new Bitmap(rect.Width, rect.Height);
                Graphics g1 = Graphics.FromImage(QQPic);
                IntPtr hdc1 = GetDC(hwnd1);
                IntPtr hdc2 = g1.GetHdc();  //得到Bitmap的DC  
                BitBlt(hdc2, 0, 0, rect.Width, rect.Height, hdc1, 0, 0, 13369376);
                g1.ReleaseHdc(hdc2);  //释放掉Bitmap的DC  
                QQPic.Save("ErrorShot_" + filename + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                //以JPG文件格式保存  
            }

        }

    }
}
