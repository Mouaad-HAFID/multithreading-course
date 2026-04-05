namespace AirplaneBooking;

public sealed class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine(@"Welcome to Skyscape Airlines.
        If you wish to book a ticket please type 'b'.
        If you wish to cancel a ticket, please type 'c'.
        Type 'exit' to leave.");

        int seats = 20;
        int availableTickets = 20;
        var processingLock = new Lock();

        var bookingQueue = new Queue<string>();

        Thread monitoringThread = new Thread(MonitorQueue);
        monitoringThread.Start();

        while (true)
        {
            string? input = Console.ReadLine();

            if (input == "exit")
            {
                break;
            }

            bookingQueue.Enqueue(input);
        }

        void MonitorQueue()
        {
            while (true)
            {
                if (bookingQueue.Count > 0)
                {
                    var input = bookingQueue.Dequeue();
                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }
            }
        }

        void ProcessInput(string? input)
        {
            /// While the basic lock works for our basic example. It wouldn't cut in a real life scenario
            /// Assuming that multiple users are trying to access the same resource at the same time
            /// there is a small chance that locks would be released within an acceptable time frame
            /// In this case we need a mechanism to notify users and ask them to try again laters
            /// Monitors can be used to achieve the desired behaviour

            // 1. Lock based implementation
            //lock (processingLock)
            //{
            //    if (input == "b")
            //    {
            //        if (availableTickets == 0)
            //        {
            //            Console.WriteLine("No tickets available. Please try again later.");
            //        }
            //        else
            //        {
            //            Thread.Sleep(1000);
            //            availableTickets--;
            //            Console.WriteLine("Ticket booked successfully");
            //        }
            //    }
            //    else if (input == "c")
            //    {
            //        if (availableTickets == seats)
            //        {
            //            Console.WriteLine("No ticket to cancel.");
            //        }
            //        else
            //        {
            //            Thread.Sleep(1000);
            //            availableTickets++;
            //            Console.WriteLine("Your booking has been cancelled");
            //        }
            //    }
            //}

            // 2. Monitor based implementation with timeout (.NET 9+)
            //if (processingLock.TryEnter())
            //{
            //    try
            //    {
            //        Thread.Sleep(2000);

            //        if (input == "b")
            //        {
            //            if (availableTickets == 0)
            //            {
            //                Console.WriteLine("No tickets available. Please try again later.");
            //            }
            //            else
            //            {
            //                availableTickets--;
            //                Console.WriteLine("Ticket booked successfully");
            //            }
            //        }
            //        else if (input == "c")
            //        {
            //            if (availableTickets == seats)
            //            {
            //                Console.WriteLine("No ticket to cancel.");
            //            }
            //            else
            //            {
            //                availableTickets++;
            //                Console.WriteLine("Your booking has been cancelled");
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        processingLock.Exit();
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("System is currently busy. Please try again later.");
            //}

            // 2. Monitor based implementation with timeout (.NET 8 and pre)
            //if (Monitor.TryEnter(processingLock))
            //{
            //    try
            //    {
            //        Thread.Sleep(2000);
            //
            //        if (input == "b")
            //        {
            //            if (availableTickets == 0)
            //            {
            //                Console.WriteLine("No tickets available. Please try again later.");
            //            }
            //            else
            //            {
            //                availableTickets--;
            //                Console.WriteLine("Ticket booked successfully");
            //            }
            //        }
            //        else if (input == "c")
            //        {
            //            if (availableTickets == seats)
            //            {
            //                Console.WriteLine("No ticket to cancel.");
            //            }
            //            else
            //            {
            //                availableTickets++;
            //                Console.WriteLine("Your booking has been cancelled");
            //            }
            //        }
            //    }
            //    finally
            //    {
            //        Monitor.Exit(processingLock);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("System is currently busy. Please try again later.");
            //}
        }
    }
}