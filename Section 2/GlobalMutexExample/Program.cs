namespace GlobalMutexExample;

public sealed class Program
{
    private static void Main(string[] args)
    {
        string filePath = "counter.txt";
        //for (int i = 0; i < 1000; i++)
        //{
        //    // In this example this is our critical section
        //    /// Using a lock here wouldn't fix the race condition. Because locks are exclusive within a single process
        //    /// and they do not work across processes.
        //    int counter = ReadCounter(filePath);
        //    counter++;
        //    WriteCounter(filePath, counter);
        //}

        /// In order to synchronize threads across multilple processes we need to use a mutex
        /// The mutex is built on top of an OS level construct
        /// Mutexes must be acquired before they come into action and must be released at the end
        /// Without the mutex the counter value at the end of execution is never 20_000
        /// However, when using a mutex we notice that although execution is slower the result is correct

        /// Locks are better when locking resources within a the same process over Mutexes.
        /// While both can get the job done, Mutexes come with a performance overhead given they're kernel bound resources
        /// Cross process sync however cannot be performed using locks and Mutexes should be used instead
        using (var mutex = new Mutex(false, $"MutexId-{Guid.NewGuid}"))
        {
            for (int i = 0; i < 10_000; i++)
            {
                try
                {
                    mutex.WaitOne();
                    int counter = ReadCounter(filePath);
                    counter++;
                    Console.WriteLine($"Counter incremented - {counter}");
                    WriteCounter(filePath, counter);
                }
                finally
                {
                    mutex?.ReleaseMutex();
                }
            }
        }

        Console.WriteLine($"Counting Process finished. Final counter value: {ReadCounter(filePath)}");
        Console.ReadKey();

        int ReadCounter(string filePath)
        {
            using FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader sr = new(fs);
            string content = sr.ReadToEnd();
            return int.TryParse(content, out var counter) ? counter : 0;
        }

        void WriteCounter(string filePath, int counter)
        {
            using FileStream fs = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            using StreamWriter sw = new(fs);
            sw.Write(counter);
        }
    }
}