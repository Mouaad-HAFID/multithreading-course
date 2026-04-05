namespace ReaderWriterLock;

/// In a web server example. Reauests can arrive at the same time
/// Meaning the different threads can either read or write from our cache at the same time
/// The Add and Get methods are not atomic and the Dictionary is not thread safe
/// This can produce unexpected and inconsistent results
/// A lock will not work in this case because it will lock the dictionnary regardless of operation
/// In order to make our Cache thread safe. We need to use ReaderWriter locks
/// These locks allow for many non destructive concurrent threads to access the critical section in case we're performing a Read operation
/// that doesn't alter state but only allows a single writer thread at a time.
/// If a writer thread acquires the lock then its access is exclusive
/// Somply put, multiple readers but a single writer
public sealed class GlobalConfigurationCache
{
    private ReaderWriterLockSlim _lock = new();
    private Dictionary<int, string> _cache = [];

    public void Add(int key, string value)
    {
        // This flag is a simple yet effective mechanism to safeguard against exceptions when exiting the lock in case it wasn't entered in the first place
        bool lockAcquired = false;
        try
        {
            _lock.EnterWriteLock();
            lockAcquired = true;
            _cache[key] = value;
        }
        finally
        {
            if (lockAcquired)
            {
                _lock.ExitWriteLock();
            }
        }
    }

    public string? Get(int key)
    {
        bool lockAcquired = false;

        try
        {
            _lock.EnterReadLock();
            lockAcquired = true;
            return _cache.TryGetValue(key, out var value) ? value : null;
        }
        finally
        {
            if (lockAcquired)
            {
                _lock.ExitReadLock();
            }
        }
    }
}