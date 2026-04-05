namespace WebServer;

public sealed class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Server started. Type 'exit' to stop.");

        var requestQueue = new Queue<string>();

        // Worker thread monitoring incoming requests
        Thread monitorThread = new Thread(MonitorQueue);
        monitorThread.Start();

        // Main thread handling incoming requests
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
                // Basic Queue is not thread safe
                // If Enqueue and Dequeue are run in parallel then we'll run into a race condition
                requestQueue.Enqueue(input);
            }
        }

        void MonitorQueue()
        {
            while (true)
            {
                // Second worker thread Processing the inputs
                if (requestQueue.Count > 0)
                {
                    string? input = requestQueue.Dequeue();
                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }
                Thread.Sleep(100);
            }
        }

        static void ProcessInput(string? input)
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Processed Input: {input}");
        }
    }
}