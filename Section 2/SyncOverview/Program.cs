namespace SyncOverview;

public sealed class Program
{
    private static void Main(string[] args)
    {
        int counter = 0;
        var counterLock = new Lock();

        var thread1 = new Thread(IncrementCounter);
        var thread2 = new Thread(IncrementCounter);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        /// While we wouled expect the final counter value to be 200_000. This doesn't happen when both threads are running at the same time.
        /// What happens is the counter value to be incremented can be equal at a given moment when the two threads are executing the instruction
        /// This results in the counter incrementing by 1 instead of 2 (since it reads the same initial counter value before incrementing)
        /// Accessing shared resources (in this case the counter variable) when running multi threaded or parallel applications is what causes race conditions
        /// Variables declared within the thread scope are not shared and thus do not cause problems
        Console.WriteLine($"Final counter value is: {counter}");

        void IncrementCounter()
        {
            for (int i = 0; i < 100_000; i++)
            {
                /// When compiled, this translates into
                /// var temp = counter
                /// counter = temp + 1
                /// Sections accessing shared resources are called a "Critical section".
                /// Critical sections are only problematic when they include non atomic operations
                /// an "Atomic operation" is when code is indivisible. As in, it's executed as a single instruction and cannot be divided into multiple intstructions
                /// A lock can be used to wrap a set of non atomic operations and have them executed as an atomic operation
                /// With a lock in place the result is the expected 200_000
                lock (counterLock)
                {
                    counter++;
                }

                /// The same behaviour can be achieved using Monitor-based locking
                /// It's a similar thread synchronization mechanism to a lock (Locks are basically Monitors behind the scenes).
                /// When entry to the critical section is possible the Monitor generates a lock over the critical section
                /// If the there's another lock over the critical section then the Monitor waits until it can enter
                /// We must exist the lock in a finally block otherwise it will never be accessible to other threads

                //Monitor.Enter(counterLock);
                //try
                //{
                //    counter++;
                //}
                //finally
                //{
                //    Monitor.Exit(counterLock);
                //}

                // .NET 9+ introduces the System.Threading.Lock type for easier, safer and strongly typed thread manipulation
                //counterLock.Enter();
                //try
                //{
                //    counter++;
                //}
                //finally
                //{
                //    counterLock.Exit();
                //}
            }
        }
    }
}