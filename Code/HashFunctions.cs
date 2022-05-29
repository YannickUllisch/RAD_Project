using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace RAD_Project {
    public class MultShiftHash : IHashFunction {
        private int const_l;
        public MultShiftHash(int val_l) {
            const_l = val_l;
        }

        public ulong hash_function(ulong key) {
            string rdm_binary = "1000111100001100110110111000010111110111110001110100011010010111";
            ulong a = (ulong)Convert.ToInt64(rdm_binary, 2);
            ulong hash = a*key>>(64-const_l);
            return hash;
        }
    }
    
    public class MultModPrimeHash : IHashFunction {
        private int const_l;
        public MultModPrimeHash(int val_l) {
            const_l = val_l;
        }

        public ulong hash_function(ulong key) {
            int q = 89;
            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, q), 1);
            BigInteger a = BigInteger.Parse("10013252864223968");
            BigInteger b = BigInteger.Parse("1252223875434578182");
            BigInteger x = BigInteger.Add(BigInteger.Multiply(a, key), b);
            BigInteger hash = (x&p)+(x>>q);
            if (hash >= p) {
                hash -= p;
            }
            BigInteger result = hash % BigInteger.Pow(2,const_l);
            
            return (ulong)result;
        }
    }

    public class FourUniversalHash : IHashFunction {

        public ulong hash_function(ulong key) {
            int b = 89;
            // Generating random Parameters for our 4-Universal Hashfunction. 
            List<BigInteger> paramList = new List<BigInteger>();
            for (int i=0; i<4; i++) {
                Random random = new Random();
                byte[] rdm_int = new byte[10];
                random.NextBytes(rdm_int);
                paramList.Add(new BigInteger(rdm_int));
            }

            BigInteger p = BigInteger.Subtract(BigInteger.Pow(2, b), 1);
            BigInteger y = paramList[paramList.Count-1];

            for (int i=paramList.Count-1; i >= 0; i--) {
                y = BigInteger.Add(BigInteger.Multiply(y, key), paramList[i]);
                y = (y&p)+(y>>b);
            }
            if (y>=p) {y-=p;}

            // Possible since we are only looking for 64 bits in y. So we mod with 2^64-1 prime
            BigInteger xm = BigInteger.Subtract(BigInteger.Pow(2, 64), 1);
            y = (y&xm)+(xm>>64);
            if (y>=xm) {y-=xm;}
            return (ulong)y;
        }
    }
    public class CountSketchHash {
        public Tuple<ulong, int> Compute_Hashfunctions(ulong key, int t, IHashFunction hash) {
            ulong m = (ulong)Math.Pow(2,t);
            ulong gx = hash.hash_function(key);
            ulong hx = gx&(m-1);
            ulong bx = (gx>>t)&1;
            int sx = 1-(2*(int)bx);
            return Tuple.Create(hx, sx);
        }
    }
}