using System;
using System.Linq;
using System.Collections.Generic;

namespace set01_challenge02
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = "1c0111001f010100061a024b53535009181c";
            string s2 = "686974207468652062756c6c277320657965";
            List<byte> v1 = new List<byte>();
            List<byte> v2 = new List<byte>();

            for (int i = 0; i < s1.Length; i += 2)
            {
                string sub = s1.Substring(i, 2);
                byte b = 0;

                b += sub[0] >= '0' && sub[0] <= '9' ? (byte)(sub[0] - '0') : (byte)(sub[0] - 'a' + 10);
                b <<= 4;
                b += sub[1] >= '0' && sub[1] <= '9' ? (byte)(sub[1] - '0') : (byte)(sub[1] - 'a' + 10);

                v1.Add(b);
            }

            for (int i = 0; i < s2.Length; i += 2)
            {
                string sub = s2.Substring(i, 2);
                byte b = 0;

                b += sub[0] >= '0' && sub[0] <= '9' ? (byte)(sub[0] - '0') : (byte)(sub[0] - 'a' + 10);
                b <<= 4;
                b += sub[1] >= '0' && sub[1] <= '9' ? (byte)(sub[1] - '0') : (byte)(sub[1] - 'a' + 10);

                v2.Add(b);
            }

            var v1V2 = v1.Zip(v2);

            foreach (var i in v1V2) {
                byte r = (byte)(i.Item1 ^ i.Item2);
                Console.Write(r.ToString("x"));
            }
            Console.WriteLine("");
        }
    }
}
