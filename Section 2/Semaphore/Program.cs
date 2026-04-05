namespace WebServer;

// A copy of the WebServer assignment in order to demonstrate how semaphores work

/// In our initial implementation, we were spawning as many thread as users were requesting
/// which isn't a realistic way of handling incoming requests given hardware limitations.
/// This is where Semaphores come into play. Unlike Mutexes or locks, they're not primarily used
/// to limit the number of threads that can access a block of code.
/// Similarly to a mutex, a semaphore is also an OS construct which means extra performance overhead
/// and extra resources.

public sealed class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Server started. Type 'exit' to stop.");

        var requestQueue = new Queue<string>();

        /// A SemaphoreSlim is a lightweight Semaphore implementation meant to limit the number of threads
        /// accessing a block of code within the same process. The normal Semaphore can be used to limit the number of threads
        /// across processes.
        /// The initial count indicates the amount of threads allowed immediately after instantiation
        /// The max count is a hard cap over the number of threads allowed by the semaphore
        using var semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);
        // A regular Queue is not thread safe, so we must use a lock to protect the Enqueue and Dequeue operations
        Lock queueLock = new Lock();

        Thread monitorThread = new(MonitorQueue);
        monitorThread.Start();

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == "exit")
            {
                Console.WriteLine("Exiting the web server");
                break;
            }
            if (input is not null)
            {
                lock (queueLock)
                {
                    requestQueue.Enqueue(input);
                }
            }
        }

        void MonitorQueue()
        {
            while (true)
            {
                if (requestQueue.Count > 0)
                {
                    string? input;
                    lock (queueLock)
                    {
                        input = requestQueue.Dequeue();
                    }
                    semaphore.Wait(); // This reserves a spot within the semaphore if available and waits for a sport to open up if not
                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }
                Thread.Sleep(100);
            }
        }

        void ProcessInput(string? input)
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Processed Input: {input}");
            semaphore.Release(); // This icreases the available spots within the semaphore by 1
        }
    }
}