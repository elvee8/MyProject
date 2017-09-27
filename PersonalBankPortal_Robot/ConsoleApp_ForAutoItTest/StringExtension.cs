using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_ForAutoItTest
{
    public static class StringExtension
    {

        public static void Main1(string[] args)
        {
            Console.WriteLine("-------------1--------------");

            string x = File.ReadAllText(@"D:\MIDAS\Base64ImagePageX.html");
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageXx.html", x.Compress());

            string y = File.ReadAllText(@"D:\MIDAS\Base64ImagePageXx.html");
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageXy.html", y.Decompress());

            string a = File.ReadAllText(@"D:\MIDAS\Base64ImagePageX.html");
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageXa.html", a.Zip());

            string b = File.ReadAllText(@"D:\MIDAS\Base64ImagePageXa.html");
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageXb.html", b.Unzip());

            Console.WriteLine("-------------2--------------");
            Console.ReadKey();
        }

        private static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string Zip(this string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string Unzip(this string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }
        


        public static string Compress(this string uncompressedString)
        {
            var compressedStream = new MemoryStream();
            var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString));
            
            using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
            {
                uncompressedStream.CopyTo(compressorStream);
            }

            return Convert.ToBase64String(compressedStream.ToArray());
        }

        public static string Decompress(this string compressedString)
        {
            var decompressedStream = new MemoryStream();
            var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

            using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
            {
                decompressorStream.CopyTo(decompressedStream);
            }

            return Encoding.UTF8.GetString(decompressedStream.ToArray());
        }

    }
}
