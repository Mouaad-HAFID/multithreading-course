namespace SignalingAutoResetEvent;

public sealed class Program
{
    private static void Main(string[] args)
    {
        /// In scenarios where we want threads to signal each other to perform specific operations
        /// we use the AutoResetEvent. The goal of AutoResetEvent is to alert all waiting threads
        /// when the AutoResetEvent is in the signaled state. Simply put, it lets waiting threads know
        /// when they can proceed.
        /// This can be easier represented in a Producer/Consumer pattern
        using var autoResetEvent = new AutoResetEvent(false);

        for (int i = 0; i < 3; i++)
        {
            var consumer = new Thread(DoWork)
            {
                Name = $"Thread-{i}"
            };
            consumer.Start();
        }

        string? input;
        Console.WriteLine("Server is running. Type 'go' to proceed");
        while (true)
        {
            input = Console.ReadLine();
            if (input == "go")
            {
                /// The Set method sets the event's state to signaled, meaning waiting threads can proceed
                /// Think of it as a traffic light for threads. Everyone is waiting for the green light before
                /// moving ahead.
                autoResetEvent.Set();
            }
        }

        void DoWork()
        {
            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} Waiting for signal");
                /// WaitOne blocks the calling thread until it receives a signal (through the Set() method)
                /// to proceed. Once it's called this "turns off" the signal that triggered it. (Hence the Auto-)
                autoResetEvent.WaitOne();
                Console.WriteLine("Doing work after receiving signal");
                Thread.Sleep(1000);
            }
        }
    }
}