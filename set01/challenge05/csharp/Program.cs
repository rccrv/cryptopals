using System;
using System.Collections.Generic;

namespace challenge05
{
    class Cycle
    {
        List<char> v;
        int counter;
        public Cycle(string s) {
            v = new List<char>();
            counter = 0;
            foreach (char c in s) {
                v.Add(c);
            }
        }

        public char next()
        {
            char r;

            r = this.v[this.counter];
            counter = counter == 2 ? 0 : counter + 1;

            return r;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string s = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
            string r = "";
            Cycle iter = new Cycle("ICE");
            List<byte> bytes = new List<byte>();
            foreach (var c in s)
            {
                bytes.Add((byte)c);
            }

            foreach (var b in bytes)
            {
                r += ((byte)b ^ (byte)iter.next()).ToString("x2");
            }

            Console.WriteLine(r);
        }
    }
}
