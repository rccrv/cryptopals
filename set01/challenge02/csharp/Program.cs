using System;

namespace challenge02
{
    class Program
    {
        static void Main(string[] args)
        {
            string s1 = "1c0111001f010100061a024b53535009181c";
            string s2 = "686974207468652062756c6c277320657965";
            string r = "";
            byte n1, n2;

            for (int i = 0; i < s1.Length; i += 2)
            {
                n1 = Convert.ToByte(s1.Substring(i, 2), 16);
                n2 = Convert.ToByte(s2.Substring(i, 2), 16);
                r += (n1 ^ n2).ToString("x");
            }
            Console.WriteLine(r);
        }
    }
}
