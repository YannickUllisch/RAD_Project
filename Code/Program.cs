using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    public class project_main {

        // ASK ABOUT:
        // Da x findes flere gange i strømmen, har de så altid samme data værdi?
        // Ellers er det jo rimelig svært at finde data værdien for at incrementere
        // Spørg hvorfor dataen hele tiden ændrer sig uden at vi laver noget ift den
        static void Main(string[] args) {
            int l = 26;
            IHashFunction shift_hash = new MultShiftHash(l);
            IHashFunction modprime_hash = new MultModPrimeHash(l);
            // Creating stream of data tuples
            IEnumerable<Tuple<ulong, int>> data_stream = CreateStream(6000000, 20);

            // Problem 1(c) - Testing runtime of Hash functions
            //test_hashfunc_time(shift_hash, modprime_hash, data_stream);

            // Initializing Hash Table of size 2^l
            int test = sum_of_squares(data_stream, shift_hash, l);
            System.Console.WriteLine(test);
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

        public static void test_hashfunc_time(IHashFunction mult_hash, IHashFunction prime_hash, IEnumerable<Tuple<ulong, int>> stream) {
            // Testing Multiply-Shift Hash function for stream
            ulong hash_sum = 0;
            var watch = Stopwatch.StartNew();
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum += mult_hash.hash_function(key.Item1);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Shift Time: " + elapsedMs + "ms");
            Console.WriteLine(hash_sum);

            // Testing Multiply-ModPrime Hash function for stream
            hash_sum = 0;
            watch.Restart();
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum += prime_hash.hash_function(key.Item1);
            }
            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Mod-Prime Time: " + elapsedMs + "ms");
            Console.WriteLine(hash_sum);
        }

        public static int sum_of_squares(IEnumerable<Tuple<ulong, int>> stream, IHashFunction hash, int l) {
            hash_table hashTable = new hash_table(hash, l);
            double sum = 0;
            foreach (Tuple<ulong, int> key in stream) {
                hashTable.insert(key);
                sum += Math.Pow(hashTable.get(key.Item1), 2.0);
                //sum += hashTable.get(key.Item1);  
            }
            System.Console.WriteLine("test: " + hashTable.test);
            double testsum = 0;
            foreach (Tuple<ulong, int> key in stream) {

                testsum += Math.Pow(hashTable.get(key.Item1), 2.0);
                //sum += hashTable.get(key.Item1);  
            }
            System.Console.WriteLine(testsum);
            return (int)sum;
        }
    }
}
