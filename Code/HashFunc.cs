using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    public class MultShiftHash : IHashFunction {
        private int l;
        public MultShiftHash(int int_l) {
            l = int_l;
        }

        public ulong hash_function(ulong key) {
            string rdm_binary = "1000111100001100110110111000010111110111110001110100011010010111";
            ulong a = (ulong)Convert.ToInt64(rdm_binary, 2);


            ulong hash = a*key>>(64-l);
         
            return hash;
        }
    }
    
    public class MultModPrimeHash : IHashFunction {
        private int l;
        public MultModPrimeHash(int int_l) {
            l = int_l;
        }

        public ulong hash_function(ulong key) {
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

    public class FourUniversalHash : IHashFunction {

        public ulong hash_function(ulong key) {
            int b = 89;
            List<BigInteger> paramList = new List<BigInteger>();
            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, b), 1);
            paramList.Add(BigInteger.Parse("32462347247234723"));
            paramList.Add(BigInteger.Parse("462346723"));
            paramList.Add(BigInteger.Parse("46754334325347"));
            paramList.Add(BigInteger.Parse("934587948582"));

            BigInteger y = paramList[paramList.Count-1];

            for (int i=paramList.Count-1; i >= 0; i--) {
                y += BigInteger.Add(BigInteger.Multiply(y, key), paramList[i]);
                y += (y&p)+(y>>b);
            }
            if (y >= p) {
                y -= p;
            }
            return (ulong)y;
        }
    }
}