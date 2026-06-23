using System.Text;
using Task_1;
using Task_2;

namespace Tests
{
    [TestClass]
    public class EncodingTests
    {
        [TestMethod]
        public void Encode_Simple()
        {
            string testInput = "abbcccdddd";
            string expectedOutput = "ab2c3d4";

            string actualOutput = Task_1.Encoding.Encode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Encode_EndingIsSingle()
        {
            string testInput = "kkkljjjjeac";
            string expectedOutput = "k3lj4eac";

            string actualOutput = Task_1.Encoding.Encode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Encode_OnlySingleCharacters()
        {
            string testInput = "heloworld";
            string expectedOutput = "heloworld";

            string actualOutput = Task_1.Encoding.Encode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Encode_MultiDigitCompression()
        {
            string testInput = "aaaaaaaaaaaaaaaaaaccccccdgggggggggggggggggggggg";
            string expectedOutput = "a18c6dg22";

            string actualOutput = Task_1.Encoding.Encode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Encode_InvalidFormat()
        {
            string testInput = ";23";
            Assert.Throws<FormatException>(() => Task_1.Encoding.Encode(testInput));
        }

        [TestMethod]
        public void Decode_Simple()
        {
            string testInput = "b2c3d4";
            string expectedOutput = "bbcccdddd";

            string actualOutput = Task_1.Encoding.Decode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Decode_EndingIsSingle()
        {
            string testInput = "k3lj4eac";
            string expectedOutput = "kkkljjjjeac";

            string actualOutput = Task_1.Encoding.Decode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Decode_StartIsSimple()
        {
            string testInput = "ab2c3d4";
            string expectedOutput = "abbcccdddd";

            string actualOutput = Task_1.Encoding.Decode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Decode_OnlySingleCharacters()
        {
            string testInput = "heloworld";
            string expectedOutput = "heloworld";

            string actualOutput = Task_1.Encoding.Decode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Decode_MultiDigitCompression()
        {
            string testInput = "a18c6dg22";
            string expectedOutput = "aaaaaaaaaaaaaaaaaaccccccdgggggggggggggggggggggg";

            string actualOutput = Task_1.Encoding.Decode(testInput);

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        [TestMethod]
        public void Decode_InvalidFormat()
        {
            string testInput = "1a1a4";
            Assert.Throws<FormatException>(() => Task_1.Encoding.Decode(testInput));
        }
    }

    [TestClass]
    public class ServerTests
    {
        private static SemaphoreSlim semaphore;
        private static DateTime startTime;
        [TestMethod]
        public void Server_BruteForceTest()
        {
            startTime = DateTime.Now;
            semaphore = new SemaphoreSlim(0);
            int readerCounter = 0;
            int writerCounter = 0;
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                if (rand.Next(0, 2) == 1)
                {
                    readerCounter++;
                    Thread t = new Thread(new ParameterizedThreadStart(Reader));
                    t.Start(readerCounter);
                }
                else
                {
                    writerCounter++;
                    Thread t = new Thread(new ParameterizedThreadStart(Writer));
                    t.Start(writerCounter);
                }
            }

            Thread.Sleep(500);
            semaphore.Release(100);

            Thread.Sleep(200);
            Assert.AreEqual(writerCounter, Server.GetCount());
        }

        private static void Reader(object id)
        {
            semaphore.Wait();
            int count = Server.GetCount();
            TimeSpan currentTime = DateTime.Now - startTime;
            Console.WriteLine("[{0}.{1}] Reader {2} read number, current count: {3}", currentTime.Seconds, currentTime.Milliseconds, id, count);
        }

        private static void Writer(object id)
        {
            semaphore.Wait();
            Server.AddToCount(1);
            TimeSpan currentTime = DateTime.Now - startTime;
            Console.WriteLine("[{0}.{1}] Writer {2} added 1 to number", currentTime.Seconds, currentTime.Milliseconds, id);
        }
    }
}
