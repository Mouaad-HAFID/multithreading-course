namespace BasicSyntax;

public sealed class Program
{
    private static void Main(string[] args)
    {
        //WriteThreadId(); //Running on main thread - Blocks execution

        // All Thread constructors require at least a delegate
        // A threads purpose for existing is performing an action so it makes sense
        // for constructors to reauire delegates
        // Running on secondary threads and do not block main thread or other thread execution
        //
        var thread = new Thread(WriteThreadId);
        var loopedThread = new Thread(WriteThreadId);

        // If we remove the Thread.Sleep we'll notice that threads are executed from highest to lowest priority
        // If we keep the Thread.Sleep then we'll notice that the Thread Scheduler switches between threads regadless of priority
        // and based off execution time. If the execution takes too long the task is kicked out to allow for other tasks to run
        thread.Priority = ThreadPriority.Highest;
        loopedThread.Priority = ThreadPriority.Lowest;
        Thread.CurrentThread.Priority = ThreadPriority.Normal;

        // We can rename our thread to have a meaningful output instead of having to remember thread IDs
        thread.Name = "First thread";
        loopedThread.Name = "Second thread";

        Thread.CurrentThread.Name = "Main thread"; // Naming main thread is also possible

        thread.Start();
        loopedThread.Start();
        //WriteThreadId(); //Running on main thread - Blocks execution when called before other threads

        // With loops and sleeps involed we can see the ThreadScheduler at work
        // alternating between the two threads

        Console.ReadLine();
    }

    private static void WriteThreadId()
    {
        for (int i = 0; i < 100; i++)
        {
            // Environment.CurrentManagedThreadId is the recommended approach to accessing the current thread
            // compared to Thread.CurrentThread.ManagedThreadId (static access to variable vs accessing object property)
            Console.WriteLine(Environment.CurrentManagedThreadId);
            Console.WriteLine(Thread.CurrentThread.Name);

            Thread.Sleep(50);
        }
    }
}