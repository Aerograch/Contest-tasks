using System.Threading;

namespace Task_2
{
    /// <summary>
    /// Class that allows thread-safe IO to a single integer. Writers write exclusevly and have priority over readers. Readers read in parralel.
    /// </summary>
    public static class Server
    {
        private static ReaderWriterLockSlim countLock = new ReaderWriterLockSlim();
        private static int count = 0;

        /// <summary>
        /// Thread-safe reading of an internal count
        /// </summary>
        public static int GetCount()
        {
            countLock.EnterReadLock();
            try
            {
                return count;
            }
            finally
            { 
                countLock.ExitReadLock(); 
            }
        }

        /// <summary>
        /// Thread-safe adding to an internal count
        /// </summary>
        public static void AddToCount(int amount)
        {
            countLock.EnterWriteLock();
            try
            {
                count += amount;
            }
            finally
            {
                countLock.ExitWriteLock();
            }
        }
    }
}
