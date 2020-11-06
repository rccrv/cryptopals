using System;
using System.Collections.Generic;

namespace base64
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Pad the string if it isn't a multiple of 3
            // TODO: Add pads
            string dict = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            string s = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            List<byte> v = new List<byte>();

            for (int i = 0; i < s.Length; i += 2)
            {
                string sub = s.Substring(i, 2);
                byte b = 0;

                b += sub[0] >= '0' && sub[0] <= '9' ? (byte)(sub[0] - '0') : (byte)(sub[0] - 'a' + 10);
                b <<= 4;
                b += sub[1] >= '0' && sub[1] <= '9' ? (byte)(sub[1] - '0') : (byte)(sub[1] - 'a' + 10);

                v.Add(b);
            }

            //Console.WriteLine(string.Join(", ", v));

            while (v.Count > 2)
            {
                uint n = 0;
                byte d1, d2, d3, d4;
                n += (uint)(v[0] << 16) + (uint)(v[1] << 8) + (uint)(v[2]);
                v.RemoveRange(0, 3);
                d1 = (byte)((n >> 18) & 63);
                d2 = (byte)((n >> 12) & 63);
                d3 = (byte)((n >> 6) & 63);
                d4 = (byte)(n & 63);

                Console.Write($"{dict[d1]}{dict[d2]}{dict[d3]}{dict[d4]}");
            }
            Console.WriteLine("");
        }
    }
}
