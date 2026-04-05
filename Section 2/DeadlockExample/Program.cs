/// Let's consider an ecommerce scenario
/// 1. Managing users
/// 2. Managing orders
/// Thread 1 wants to lock users first then lock orders
/// Thread 2 wants to lock orders first then lock users

namespace DeadlockExample;

public sealed class Program
{
    private static void Main(string[] args)
    {
        /// Running the code below will output the first console writeline inside each manage method
        /// It will however directly move on to Finished instead of displaying the second writeline
        /// Reason being each method acquires a first lock and when moving on to the second it finds that
        /// it's already blocked by another thread. The second thread however is also waiting for the first
        /// thread to realse a lock for it to proceed. This is called a deadlock situation and is often caused by
        /// nested locks.

        Lock userLock = new();
        Lock orderLock = new();

        Console.WriteLine("Hello, World!");

        Thread thread = new(ManageOrder);
        thread.Start();

        ManageUser();

        thread.Join();

        Console.ReadLine();
        Console.WriteLine("Finished");

        void ManageUser()
        {
            lock (userLock)
            {
                Console.WriteLine("User Management acquired user lock");
                Thread.Sleep(2000);

                lock (orderLock)
                {
                    Console.WriteLine("User Management acquired order lock");
                }
            }
        }

        void ManageOrder()
        {
            lock (orderLock)
            {
                Console.WriteLine("Order Management acquired order lock");
                Thread.Sleep(1000);

                lock (userLock)
                {
                    Console.WriteLine("Order Management acquired user lock");
                }
            }
        }
    }
}