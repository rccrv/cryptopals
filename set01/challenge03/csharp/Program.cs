using System;
using System.Linq;
using System.Collections.Generic;

namespace set01_challenge03
{
    class Program
    {
        // TODO: ETAOIN SHRDLU
        static void Main(string[] args)
        {
            string s1 = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";
            Dictionary<char, string> d = new Dictionary<char, string>();

            var l3 = Enumerable.Range(97, 26).ToList();
            var l2 = Enumerable.Range(65, 26).ToList();
            l2.AddRange(l3);
            var l = Enumerable.Range(48, 10).ToList();
            l.AddRange(l2);

            List<byte> v1 = new List<byte>();
            for (int i = 0; i < s1.Length; i += 2)
            {
                string sub = s1.Substring(i, 2);
                byte b = 0;

                b += sub[0] >= '0' && sub[0] <= '9' ? (byte)(sub[0] - '0') : (byte)(sub[0] - 'a' + 10);
                b <<= 4;
                b += sub[1] >= '0' && sub[1] <= '9' ? (byte)(sub[1] - '0') : (byte)(sub[1] - 'a' + 10);

                v1.Add(b);
            }

            l.ForEach(k => d.Add((char)k, String.Join("", v1.Select(i => (char)(i ^ k)))));
            foreach (var v in d)
            {
                Console.WriteLine(v);
            }
        }
    }
}
