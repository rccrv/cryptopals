using System;
using System.Linq;
using System.Numerics;

namespace challenge06
{
    class Program
    {
        static uint hd(string s1, string s2)
        {
            uint sum = 0;
            foreach (var c in s1.Zip(s2))
            {
                sum += (uint)BitOperations.PopCount((uint)(c.Item1 ^ c.Item2));
            }
            return sum;
        }
        static void Main(string[] args)
        {
            string s1 = "this is a test";
            string s2 = "wokka wokka!!!";
            var r = hd(s1, s2);
            Console.WriteLine(r);
        }
    }
}
