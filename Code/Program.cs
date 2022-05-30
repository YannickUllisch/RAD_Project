using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Linq;
using CsvHelper;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RAD_Project {
    public static class project_main {

        static void Main(string[] args) {
            // Task 1(c) - Testing runtime of Hash functions. 
            // Initializing data stream and hash functions.
            int l = 23;
            IEnumerable<Tuple<ulong, int>> data_stream = CreateStream(999999, l);
            IHashFunction shift_hash = new MultShiftHash(l);
            IHashFunction modprime_hash = new MultModPrimeHash(l);
            //hashfunc_time(shift_hash, modprime_hash, data_stream);

            // Task 3 - Calculating sum of squares for different streams with HashTable.
            int[] lval_arr = new int[] {16, 20, 22, 24, 26, 28};
            //hashtable_time_complexity(lval_arr, 268435456);
            
            // Task 7 and 8 - Experiments 
            int[] tval_arr = new int[] {26, 18, 22};
            for (int i = 0; i < tval_arr.Length; i++) {
                IEnumerable<Tuple<ulong, int>> stream = CreateStream(268435, tval_arr[i]);
                Tuple<List<ulong>, ulong> estimates = CS_Experiments(stream, tval_arr[i]);
                sorted_experiment_tocsv(estimates.Item1, estimates.Item2, i);
                unsorted_experiment_tocsv(estimates.Item1, estimates.Item2, i);
            }
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

        /// <summary>
        /// Calculates time it takes to run our hash functions on some stream of data, aswell as calculating 
        /// the hash sum. Function used for Task 1(c).
        /// </summary>
        /// <param name="shift">  Expected to be our Multiply-Shift Hash function </param>
        /// <param name="prime"> Expected to be our Multiply-ModPrime Hash function </param>
        public static void hashfunc_time(IHashFunction shift, IHashFunction prime, IEnumerable<Tuple<ulong, int>> stream) {
            // Testing Multiply-Shift Hash function for stream
            ulong hash_sum = 0;
            var watch = Stopwatch.StartNew();
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum += shift.hash_function(key.Item1);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Shift Time: " + elapsedMs + "ms");
            Console.WriteLine("Sum of Hashvalues: " + hash_sum);

            // Testing Multiply-ModPrime Hash function for stream
            hash_sum = 0;
            watch.Restart();
            foreach(Tuple<ulong, int> key in stream) {
                hash_sum += prime.hash_function(key.Item1);
            }
            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Multiply-Mod-Prime Time: " + elapsedMs + "ms");
            Console.WriteLine("Sum of Hashvalues: " + hash_sum);
        }   

        /// <summary> Calculates the sum of squares for some stream of data. Used as base for Task 3. </summary>
        /// <param name="hash"> Some hash function to be used by our hash table for calculations </param>
        /// <param name="l"> Integer deciding range of hash values and size of hash table </param>
        /// <returns> Returns a tuple with sum of squares as Item 1 and time taken for calculation as Item2.  </returns>
        public static Tuple<ulong, long> sum_of_squares(IEnumerable<Tuple<ulong, int>> stream, IHashFunction hash, int l) {
            var watch = Stopwatch.StartNew();
            hash_table hashTable = new hash_table(hash, l);
            double sum = 0;
            foreach (Tuple<ulong, int> key in stream) {
                // Use nested forloops for later exercises so values of IEnumerable dont get randomized again
                hashTable.insert(key);
                sum += Math.Pow(hashTable.get(key.Item1), 2.0); 
            }
            watch.Stop();
            return Tuple.Create((ulong)sum, watch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Calculating sum of squares for different values of l inside of the array "l" and a number of elements
        // used to create a stream. Additionally calculates this using both the mutiply-shift and the 
        // multiply-modprime hash function. Used for Task 3. 
        /// </summary>
        /// <param name="l"> An array with different values of 'l' to calculate the hash table time complexity for. </param>
        /// <param name="num_elements">
        /// The number of elements in our data stream. Important note that num_elements >= 2^l for all l. 
        /// </param>
        public static void hashtable_time_complexity(int[] l, int num_elements) {
            for (int i = 0; i < l.Length; i++) {
                IHashFunction iter_shifthash = new MultShiftHash(l[i]);
                IHashFunction iter_modprimehash = new MultModPrimeHash(l[i]);
                IEnumerable<Tuple<ulong, int>> iter_stream = CreateStream(num_elements, l[i]);
                Tuple<ulong, long> shift_sum = sum_of_squares(iter_stream, iter_shifthash, l[i]);
                System.Console.WriteLine("");
                Console.WriteLine("Sum of Squares with l=" + l[i]+ " and ShiftHash: " + shift_sum.Item1);
                System.Console.WriteLine("Elapsed time: " + shift_sum.Item2 + "ms");
                Tuple<ulong, long> prime_sum = sum_of_squares(iter_stream, iter_modprimehash, l[i]);
                System.Console.WriteLine("");
                Console.WriteLine("Sum of Squares with l=" + l[i]+ " and ModPrimeHash: " + prime_sum.Item1);
                System.Console.WriteLine("Elapsed time: " + prime_sum.Item2 + "ms");
            }
        }

        /// <summary>
        /// Uses a data stream to calculate S via count sketch 100 times aswell as our 'true' estimate of S 
        /// via Hash Table implementation. Used for Task 7.
        /// </summary>
        /// <param name="t"> Integer used as size of Count Sketch array and value range in our Hash function </param>
        /// <returns>
        /// A tuple consisting of a List of Estimates via count sketch as Item1 and the true estimate as Item2. 
        /// </returns>
        public static Tuple<List<ulong>, ulong> CS_Experiments(IEnumerable<Tuple<ulong, int>> stream, int t) {
            ulong trueEstimate = 0;
            List<ulong> estimators = new List<ulong>();
            IHashFunction shift_hash = new MultShiftHash(t);
            int num_iterations = 16;

            for (int i = 0; i < num_iterations; i++) {
                if (trueEstimate == 0) {
                    trueEstimate = (sum_of_squares(stream, shift_hash, t)).Item1;
                }
                System.Console.WriteLine(i);
                IHashFunction hash = new FourUniversalHash();
                CountSketchHash sketch_hash = new CountSketchHash();
                CountSketch CS = new CountSketch(sketch_hash);
                CS.BCS_Init(t);
                foreach (Tuple<ulong, int> key in stream) {
                    CS.BCS_Process(key);
                }

                estimators.Add(CS.BCS_Estimator());
            }
            return Tuple.Create(estimators, trueEstimate);
        }

        /// <summary> Plots the sorted list of estimates aswell as the true estimate </summary>
        /// <param name="estimates"> A list of estimates for S </param>
        /// <param name="trueEstimate"> The true estimate for S </param>
        /// <param name="iteration"> Keeping track of iteratio number for naming of csvs </param>
        public static void sorted_experiment_tocsv(List<ulong> estimates, ulong trueEstimate, int iteration) {
            List<ulong> sortedList = estimates.OrderBy(i => i).ToList();
            List<Tuple<ulong, int>> numberedList = new List<Tuple<ulong, int>>();
            for (int i = 1; i<=sortedList.Count; i++) {
                numberedList.Add(Tuple.Create(sortedList[i-1],i));
            }
            
            // Printing MSE value
            System.Console.WriteLine("MSE for experiment "+(iteration+1)+" is "+ MSE(sortedList, trueEstimate));

            // Writing into csv file
            using (TextWriter sw = new StreamWriter((iteration+1)+"sorted.csv")) {
                foreach (Tuple<ulong, int> key in numberedList) {
                    sw.WriteLine("{0},{1},{2}", key.Item1, key.Item2, trueEstimate);
                }
            }
        }

        /// <summary>
        /// Using the true estimate for S aswell as the estimates made by Count Sketch to partition and plot medians 
        /// from these partitions
        /// </summary>
        /// <param name="estimates"> A list of estimates for S </param>
        /// <param name="trueEstimate"> The true estimate for S </param>
        /// <param name="iteration"> Keeping track of iteratio number for naming of csvs </param>
        public static void unsorted_experiment_tocsv(List<ulong> estimates, ulong trueEstimate, int iteration) {
            int partitionSize = 11;
            List<List<ulong>> partitions = new List<List<ulong>>();
            // Partitioning into smaller sized lists
            for (int i=0; i<estimates.Count; i+=partitionSize) {
                partitions.Add(estimates.GetRange(i, Math.Min(partitionSize, estimates.Count - i)));
            }   

            // Sorting all partitions
            for (int i = 0; i<partitions.Count; i++) {
                partitions[i] = partitions[i].OrderBy(x => x).ToList();
            }
            // Extracting all median values and sorting them
            List<ulong> medianValues = new List<ulong>();
            foreach(List<ulong> subList in partitions) {
                medianValues.Add(subList[(subList.Count)/2]);
            }
            medianValues = medianValues.OrderBy(x => x).ToList();

            // Giving each median value an ascending order integer for plotting
            List<Tuple<ulong, int>> numberedMedianList = new List<Tuple<ulong, int>>();
            for (int i = 1; i<=medianValues.Count; i++) {
                numberedMedianList.Add(Tuple.Create(medianValues[i-1],i));
            }

            // Writing into csv file
            using (TextWriter sw = new StreamWriter((iteration+1)+"medians.csv")) {
                foreach (Tuple<ulong, int> key in numberedMedianList) {
                    sw.WriteLine("{0},{1},{2}", key.Item1, key.Item2, trueEstimate);
                }
            }
        }

        /// <summary>
        /// Calculating the Mean Square Error between list of estimates and the real estimate
        /// </summary>
        /// <param name="estimates"> A list of estimates for S </param>
        /// <param name="trueEstimate"> The true estimate for S </param>
        /// <returns> The Mean Square error value </returns>
        public static ulong MSE(List<ulong> estimates, ulong trueEstimate) {
            ulong MSE = 0;

            for(int i = 0; i < estimates.Count; i++) {
                MSE += (ulong)Math.Pow((estimates[i] - trueEstimate), 2);
            }
            MSE = MSE / (ulong)estimates.Count;
            return MSE;
        }
    }
}
