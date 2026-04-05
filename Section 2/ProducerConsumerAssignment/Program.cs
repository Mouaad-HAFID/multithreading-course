namespace ProducerConsumerAssignment;

public sealed class Program
{
    private static void Main(string[] args)
    {
        var producerQueue = new Queue<int>();
        // A manual reset event used to signal the consumer threads to stop waiting and consume
        var consumeEvent = new ManualResetEventSlim(false);
        // A manual reset event used to signal the producer thread that consumption is finished and to produce again
        // Initiated as signaled so that it doesn't block at the first execution
        var produceEvent = new ManualResetEventSlim(true);
        string? input;
        int consumerCounter = 0;
        Lock counterLock = new();

        for (int i = 0; i < 3; i++)
        {
            var worker = new Thread(DoWork);
            worker.Start();
        }

        while (true)
        {
            // This doesn't block on first execution. The Reset however causes it to always block on the second run
            /// The goal of these two lines is to prevent over production. So we only allow the user to produce once we're certain that
            /// all items in queue have already been consumed
            produceEvent.Wait();
            produceEvent.Reset();

            Console.WriteLine("Production Queue Empty. Press P to generate more work:");
            input = Console.ReadLine();
            if (input.Equals("p", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Starting Production.");
                for (int i = 0; i < 10; i++)
                {
                    producerQueue.Enqueue(i + 1);
                }
                // Signals all worker threads to begin consumption because production is finished
                consumeEvent.Set();
                Console.WriteLine("Production Finished");
            }
        }

        void DoWork()
        {
            while (true)
            {
                // When the thread start running they stop and wait for production before proceeding
                // Line 41 signals them to start consuming the queue
                consumeEvent.Wait();
                while (producerQueue.TryDequeue(out int item))
                {
                    Console.WriteLine($"Worker Thread {Environment.CurrentManagedThreadId} consumed {item}");
                    Thread.Sleep(1000);
                }

                lock (counterLock)
                {
                    // This counter ensure that all threads have finished running the loop above
                    // and that we can go ahead with production again if the user wants to
                    consumerCounter++;

                    if (consumerCounter == 3)
                    {
                        // Resetting the consumer event so that next time we wait until we have production
                        consumeEvent.Reset();
                        // Setting the produce event in order to unblock the producer thread (main thread)
                        // and avoid overproduction
                        produceEvent.Set();
                        consumerCounter = 0;
                        Console.WriteLine("READY FOR MORE");
                    }
                }
            }
        }
    }
}