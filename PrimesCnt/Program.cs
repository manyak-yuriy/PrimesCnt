using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimesCnt
{
    
    class Program
    {
        private static readonly int upperBound = 1000000;

        static void Main(string[] args)
        {
            // The majority of computations are done to initialize this list of potential circular primes
            List<int> listOfPrimes = getAllPrimes(upperBound);
            SortedSet<int> primePool = new SortedSet<int>(listOfPrimes);
            //Console.Write(primePool.Count);
            DateTime startTime = DateTime.Now;

            int circPrimesCnt = 1;

            while (primePool.Count > 0)
                filterPool(primePool, ref circPrimesCnt);

            Console.WriteLine("Result: {0}", circPrimesCnt);

            DateTime stopTime = DateTime.Now;

            TimeSpan totalTime = stopTime - startTime;
            Console.WriteLine("Execution time: {0} ms", totalTime.TotalMilliseconds);

            Console.ReadKey();
        }

        static int getDigCnt(int num, out bool discarded)
        {
            int res = 0;
            while (num > 0)
            {
                int digit = num % 10;
                
                if (digit % 2 == 0)
                {
                    discarded = true;
                    // it doesn't matter what is returned
                    return 0;
                }
                
                num /= 10;
                res++;
            }
            discarded = false;
            return res;
        }

        static void filterPool(SortedSet<int> primePool, ref int circPrimesCnt)
        {
            int numStart = primePool.First();
            bool discarded = false;
            int digCnt = getDigCnt(numStart, out discarded);

            if (discarded)
            {
                primePool.Remove(numStart);
                return;
            }

            int order = 1;
            for (int i = 1; i < digCnt; i++)
                order *= 10;

            SortedSet<int> primesInCycle = new SortedSet<int>();

            bool broken = false;

            for (int shift = 0; shift < digCnt; shift++)
            {
                if (primePool.Contains(numStart))
                    // numbers are unique since a Set type is used
                    primesInCycle.Add(numStart);
                else
                {
                    broken = true;
                    break;
                };

                // get next number in cycle
                int digit = numStart % 10;
                numStart = numStart / 10 + digit * order;
            }

            foreach (int num in primesInCycle)
               primePool.Remove(num);

            if (!broken)
                circPrimesCnt += primesInCycle.Count;
        }



        // Naive method is usually faster than Sieve of Eratosthenes due to cache misses in large arrays
        // If perfomance were critical I would implement sieve of Atkin or any other efficient algorithm
        public static List<int> getAllPrimes(int upperBound)
        {
            List<int> primePool = new List<int>();

            primePool.Add(2);

            for (int num = 3; num <= upperBound; num += 2)
            {
                bool isPrime = true;
                int root = (int)Math.Sqrt(num) + 1;
                foreach (int divisor in primePool)
                {
                    if (divisor > root)
                        break;
                    if (num % divisor == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }

                if (isPrime)
                    primePool.Add(num);
            }

            primePool.Remove(2);

            return primePool;
        }

    }


}
