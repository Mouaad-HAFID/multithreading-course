using System.Diagnostics;

namespace DivideAndConquer;

public sealed class Program
{
    private static void Main(string[] args)
    {
        int[] array = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        //int sum = 0;
        int sum1 = 0, sum2 = 0, sum3 = 0, sum4 = 0;

        int numOfThreads = 4;
        int segmentLength = array.Length / numOfThreads;

        Thread[] threads = new Thread[numOfThreads];

        threads[0] = new Thread(() => sum1 = Sum(0, segmentLength));
        threads[1] = new Thread(() => sum2 = Sum(segmentLength, 2 * segmentLength));
        threads[2] = new Thread(() => sum3 = Sum(2 * segmentLength, 3 * segmentLength));
        threads[3] = new Thread(() => sum4 = Sum(3 * segmentLength, array.Length));

        Stopwatch sw = Stopwatch.StartNew();

        // Multi threaded demo - 445 ms
        foreach (var thread in threads)
        {
            thread.Start();
        }

        // Blocks the calling thread execution until it terminates / finishes execution
        // The goal is to wait for all threads to finish execution before stopping the stopwatch
        // otherwise we would end up with inaccurate values due to the threads running asynchronously
        // compared to the main thread
        foreach (var thread in threads)
        {
            thread.Join();
        }

        // Single thread sum example - 1060 ms.
        //foreach (int i in array)
        //{
        //    Thread.Sleep(100);
        //    sum += i;
        //}

        sw.Stop();

        Console.WriteLine($"Sum = {sum1 + sum2 + sum3 + sum4}");
        Console.WriteLine($"Elapsed time in ms = {sw.ElapsedMilliseconds}");

        // In real life scenarios we would usually have to run multiple cpu intensive tasks
        // Dividing those tasks into multiple sub tasks and having each run on a separate thread
        // significantly cuts down on execution time
        int Sum(int start, int end)
        {
            var sum = 0;

            for (int i = start; i < end; i++)
            {
                Thread.Sleep(100);
                sum += array[i];
            }

            return sum;
        }
    }
}