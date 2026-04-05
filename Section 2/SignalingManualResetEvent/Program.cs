namespace SignalingManualResetEvent;

public sealed class Program
{
    private static void Main(string[] args)
    {
        /// A ManualResetEvent does the exact same thing as an AutoResetEvent in terms of signaling waiting threads to proceed
        /// It however does not reset automatically as soon as a waiting thread is signaled, so the signal remains available for
        /// all waiting threads until it is manually reset
        /// This allows for multiple threads to be signaled at the same time
        using var manualResetEvent = new ManualResetEventSlim(false);
        Console.WriteLine("Press enter to release all threads...");

        for (int i = 0; i < 3; i++)
        {
            var thread = new Thread(DoWork)
            {
                Name = $"Worker-{i + 1}"
            };
            thread.Start();
        }

        Console.ReadLine();
        manualResetEvent.Set();
        Console.ReadLine();

        void DoWork()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} is waiting for signal...");
            manualResetEvent.Wait();
            Thread.Sleep(1000);
            Console.WriteLine($"{Thread.CurrentThread.Name} has been released.");
        }
    }
}