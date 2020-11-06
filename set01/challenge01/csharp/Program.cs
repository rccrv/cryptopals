using System;

namespace challenge01
{
    class Program
    {
        static void Main(string[] args)
        {
            string d = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
            string s = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            string r = "";
            byte n1, n2, n3;
            int d1, d2, d3, d4;

            for (int i = 0; i < s.Length; i += 6)
            {
                n1 = Convert.ToByte(s.Substring(i, 2), 16);
                n2 = Convert.ToByte(s.Substring(i + 2, 2), 16);
                n3 = Convert.ToByte(s.Substring(i + 4, 2), 16);
                d1 = n1 & 0b11111100 >> 2;
                d2 = (n1 & 0b00000011) << 4 | (n2 & 0b11110000) >> 4;
                d3 = (n2 & 0b00001111) << 2 | (n3 & 0b11000000) >> 6;
                d4 = n3 & 0b00111111;
                r += d[d1].ToString();
                r += d[d2].ToString();
                r += d[d3].ToString();
                r += d[d4].ToString();
            }

            Console.WriteLine(r);
        }
    }
}
