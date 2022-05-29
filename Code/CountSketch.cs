using System;
using System.Collections.Generic;
using System.Numerics;
using System.Diagnostics;

namespace RAD_Project {

    public class CountSketch {
        CountSketchHash hash_function;
        IHashFunction four_universal_hash;
        long[] CS_array;
        int t;
        ulong m;

        public CountSketch(CountSketchHash hash) {
            hash_function = hash;
            four_universal_hash = new FourUniversalHash();
        }

        public void BCS_Init(int val_t) {
            t = val_t;
            m = (ulong)Math.Pow(2,t);
            CS_array = new long[m];
        }

        public void BCS_Process(Tuple<ulong, int> keys) {
            Tuple<ulong, int> hashes = hash_function.Compute_Hashfunctions(keys.Item1, t, four_universal_hash);
            CS_array[hashes.Item1] = CS_array[hashes.Item1] + hashes.Item2 * keys.Item2;
        }
        
        public ulong BCS_Estimator() {
            ulong sum = 0;
            for (ulong i = 0; i < m; i++) {
                sum += (ulong)Math.Pow(CS_array[i],2);
            }
            return sum;
        } 
        
    }
}