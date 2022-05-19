
class MainProgram
{
    static void Main(string[] args)
    {
        // Display the number of command line arguments.
        Console.WriteLine(args.Length);
        mult_shift_hash(1,2);
    }

    private static mult_shift_hash(int x, int l ) {
        // Generate random a
        string rdm = "10001111 00001100 11011011 10000101 11110111 11000111 01000110 10010110";
        int a = Convert.ToInt32(rdm, 2);
        Console.WriteLine(a);
        hash = (a*x)>>(64-l);
        return 5;
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

