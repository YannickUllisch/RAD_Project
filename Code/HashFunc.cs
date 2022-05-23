using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    public class hash_functions {

        public ulong mult_shift_hash(ulong key) {
            int l = 26;
            string rdm_binary = "1000111100001100110110111000010111110111110001110100011010010111";
            ulong a = (ulong)Convert.ToInt64(rdm_binary, 2);
            ulong hash = (a*key) >> (64-l);
            return hash;
        }
        
        public ulong mult_modprime_hash(ulong key) {
            int l = 26;
            int q = 89;
            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, q), 1);
            BigInteger a = 2201655446200777458;
            BigInteger b = 84363649162346;
            BigInteger x = BigInteger.Add(BigInteger.Multiply(a, key), b);
            BigInteger hash = (x&p)+(x>>q);
            if (hash >= p) {
                hash -= p;
            }
            BigInteger result = hash % BigInteger.Pow(2,l);
            
            return (ulong)result;
        }
    }
}