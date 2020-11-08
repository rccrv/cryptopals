using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace challenge04
{
    class Program
    {
        private static (char, double)[] ALPHABET = {
            ('a', 0.08167),
            ('b', 0.01492),
            ('c', 0.02782),
            ('d', 0.04253),
            ('e', 0.12702),
            ('f', 0.02228),
            ('g', 0.02015),
            ('h', 0.06094),
            ('i', 0.06966),
            ('j', 0.00153),
            ('k', 0.00772),
            ('l', 0.04025),
            ('m', 0.02406),
            ('n', 0.06749),
            ('o', 0.07507),
            ('p', 0.01929),
            ('q', 0.00095),
            ('r', 0.05987),
            ('s', 0.06327),
            ('t', 0.09056),
            ('u', 0.02758),
            ('v', 0.00978),
            ('w', 0.02360),
            ('x', 0.00150),
            ('y', 0.01974),
            ('z', 0.00074),
        };
        static double chi_square(string xored)
        {
            double sum = 0.0;
            ushort[] count = new ushort[26];
            Array.Clear(count, 0, count.Length);

            foreach (char c in xored.ToCharArray())
            {
                if (c < 128 && Char.IsLower(c))
                {
                    count[c - 'a'] += 1;
                }
                else if (c < 128 && Char.IsNumber(c))
                {
                    sum += 5.0;
                }
                else if (c < 128 && Char.IsWhiteSpace(c))
                {
                    sum += 0.1;
                }
                else if (c < 128 && Char.IsPunctuation(c))
                {
                    sum += 5.0;
                }
                else
                {
                    return Double.PositiveInfinity;
                }
            }

            foreach (var i in Enumerable.Range(0, 26))
            {
                var o = count[i];
                var e = ALPHABET[i].Item2 * xored.Length;
                sum += (o - e) * (o - e) / e;
            }

            return sum;
        }

        static (char, double, string) analyzestring(string s) {
            List<byte> bytes = new List<byte>();
            (char, double, string) bestfit = ('\0', Double.PositiveInfinity, "");
            var l = Enumerable.Range(48, 10).ToList();
            l.AddRange(Enumerable.Range(65, 26).ToList());
            l.AddRange(Enumerable.Range(97, 26).ToList());

            for (int i = 0; i < s.Length; i += 2)
            {
                bytes.Add(Convert.ToByte(s.Substring(i, 2), 16));
            }

            foreach (var k in l)
            {
                string xored = String.Join("", bytes.Select(i => (char)(i ^ k)));
                var n = chi_square(xored.ToLower());
                if (n < bestfit.Item2)
                {
                    bestfit = ((char)k, n, xored);
                }
            }

            return bestfit;
        }
        static void Main(string[] args)
        {
            (char, double, string) bestfit = ('\0', Double.PositiveInfinity, "");
            if (args.Length >= 1)
            {
                foreach (var line in File.ReadAllLines($"{args[0]}"))
                {
                    (char, double, string) r = analyzestring(line);
                    if (r.Item2 < bestfit.Item2)
                    {
                        bestfit = r;
                    }
                }
            }

            Console.WriteLine($"Decoded by using {bestfit.Item1}\nResult: {bestfit.Item3}");
        }
    }
}
