using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    public class project_main {

        static void Main(string[] args) {
            int l = 26;
            hash_functions functions = new hash_functions();
            // Creating stream of data tuples
            IEnumerable<Tuple<ulong, int>> data_stream = CreateStream(1000, (int)Math.Pow(2, l));

            ulong shift_test = functions.mult_shift_hash(5780);
            Console.WriteLine(shift_test);
            ulong prime_test = functions.mult_modprime_hash(1513);
            System.Console.WriteLine(prime_test);

            // Problem 1(c) - Testing runtime of Hash functions
            test_hashfunc_time(functions, data_stream);

            // Initializing Hash Table of size 2^l
            hash_table table = new hash_table();
            table.init_table((int)Math.Pow(2, l));
        }

        public static IEnumerable<Tuple<ulong, int>> CreateStream(int n, int l) {
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

        public static void test_hashfunc_time(hash_functions functions, IEnumerable<Tuple<ulong, int>> stream) {
            ulong hash_sum = 0;
            var watch = Stopwatch.StartNew();
            System.Console.WriteLine(stream);
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum += functions.mult_shift_hash(key.Item1);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Shift Time: " + elapsedMs + " ms");
            Console.WriteLine(hash_sum);

            ulong hash_sum2 = 0;
            watch.Restart();
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum2 += functions.mult_modprime_hash(key.Item1);
            }
            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Mod-Prime Time: " + elapsedMs + " ms");
            Console.WriteLine(hash_sum2);
        }
    }
}
