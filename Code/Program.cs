using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    class RAD_Main {
        static void Main(string[] args) {

            ulong f = mult_shift_hash(50);
            Console.WriteLine(f);
            ulong t = mult_modprime_hash(1513);
            System.Console.WriteLine(t);


            ulong hash_sum = 0;
            IEnumerable<Tuple<ulong, int>> data_stream = CreateStream(10000, 500000);
            var watch = Stopwatch.StartNew();
            foreach(Tuple<ulong, int> key in data_stream) {
                hash_sum += mult_shift_hash(key.Item1);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            System.Console.WriteLine(hash_sum);
            System.Console.WriteLine(elapsedMs + " ms");

            hash_sum = 0;
            watch.Restart();
            foreach(Tuple<ulong, int> key in data_stream) {
                hash_sum += mult_modprime_hash(key.Item1);
            }
            elapsedMs = watch.ElapsedMilliseconds;
            System.Console.WriteLine(elapsedMs + " ms");
            System.Console.WriteLine(hash_sum);
            
        }

        public static ulong mult_shift_hash(ulong key) {
            int l = 44;
            string rdm_binary = "1000111100001100110110111000010111110111110001110100011010010111";
            ulong a = (ulong)Convert.ToInt64(rdm_binary, 2);

            ulong hash = (a*key) >> (64-l);
            return hash;
        }
        
        public static ulong mult_modprime_hash(ulong key) {
            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, 89), 1);
            BigInteger a = 2201655446200458;
            BigInteger b = 84363649162346;
            int l = 26;
            BigInteger x = BigInteger.Add(BigInteger.Multiply(a, key), b);
            BigInteger hash = (x&p)+(x>>89);
            if (hash >= p) {
                hash -= p;
            }
            ulong result = (ulong)hash % (ulong)Math.Pow(2,l);
            
            return result;
        }

        public static IEnumerable<Tuple<ulong, int>> CreateStream( int n, int l) {
            // We generate a random uint64 number.
            Random rnd = new System.Random(); 
            ulong a = 0UL;
            Byte [] b = new Byte [8]; 
            rnd.NextBytes(b);
            
            for(int i = 0; i < 8; ++i) {
                a = (a << 8) + (ulong)b[i];
            }
            // We demand that our random number has 30 zeros on the least
            // significant bits and then a one.

            a = (a | ((1UL << 31) - 1UL)) ^ ((1UL << 30) - 1UL);
            ulong x = 0UL; 
            for(int i = 0; i < n/3; ++i) {
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), 1);
            }

            for(int i = 0; i < (n + 1)/3; ++i) { 
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), -1);
            }
            for(int i = 0; i < (n + 2)/3; ++i) { 
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), 1); 
            }
        }
    }
}
