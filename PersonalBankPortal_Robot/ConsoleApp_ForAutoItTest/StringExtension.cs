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
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageX2.html", x.Compress());

            string y = File.ReadAllText(@"D:\MIDAS\Base64ImagePageX2.html");
            File.WriteAllText(@"D:\MIDAS\Base64ImagePageX3.html", y.Decompress());

            Console.WriteLine("-------------2--------------");
            Console.ReadKey();
        }

        public static string Compress(this string uncompressedString)
        {
            var compressedStream = new MemoryStream();
            var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString));

            using (var compressorStream = new DeflateStream(compressedStream, CompressionMode.Compress, true))
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
