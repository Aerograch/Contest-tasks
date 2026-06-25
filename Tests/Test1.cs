using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Text;
using Task_1;
using Task_2;
using Task_3;

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

    [TestClass]
    public class StandartizationTests
    {
        private static string inputPath;
        private static string outputPath;
        private static string errorPath;

        [TestMethod]
        [DoNotParallelize]
        public void WriteLog_SimpleTest()
        {
            PrepareEmptyFiles();

            File.WriteAllLines(inputPath, new string[]{ "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'", 
                 "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'" });

            Task_3.Program.Standartization.WriteLog(inputPath, outputPath, errorPath);

            string expectedStdOutput = "10-03-2025\t15:14:49.523\tINFO\tDEFAULT\tВерсия программы: '3.4.0.48729'\r\n" +
                "10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'\r\n";
            string actualStdOutput = File.ReadAllText(outputPath);

            string expectedErrOutput = "";
            string actualErrOutput = File.ReadAllText(errorPath);

            Assert.AreEqual(expectedStdOutput, actualStdOutput);
            Assert.AreEqual(expectedErrOutput, actualErrOutput);
        }

        [TestMethod]
        [DoNotParallelize]
        public void WriteLog_ErrorTest()
        {
            PrepareEmptyFiles();

            File.WriteAllLines(inputPath, new string[]{ "10.03.2025 15:14:49.523 INFOR231ATION Версия программы: '3.4.0.48729'",
                 "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'" });

            Task_3.Program.Standartization.WriteLog(inputPath, outputPath, errorPath);

            string expectedStdOutput = "10-03-2025\t15:14:51.5882\tINFO\tMobileComputer.GetDeviceId\tКод устройства: '@MINDEO-M40-D-410244015546'\r\n";
            string actualStdOutput = File.ReadAllText(outputPath);

            string expectedErrOutput = "10.03.2025 15:14:49.523 INFOR231ATION Версия программы: '3.4.0.48729'\r\n";
            string actualErrOutput = File.ReadAllText(errorPath);

            Assert.AreEqual(expectedStdOutput, actualStdOutput);
            Assert.AreEqual(expectedErrOutput, actualErrOutput);
        }

        private static void PrepareEmptyFiles()
        {
            Directory.CreateDirectory(Path.GetFullPath("StandartizationTests"));

            File.Create(Path.GetFullPath("StandartizationTests\\input.txt")).Dispose();
            inputPath = Path.GetFullPath("StandartizationTests\\input.txt");

            File.Create(Path.GetFullPath("StandartizationTests\\output.txt")).Dispose();
            outputPath = Path.GetFullPath("StandartizationTests\\output.txt");

            File.Create(Path.GetFullPath("StandartizationTests\\error.txt")).Dispose();
            errorPath = Path.GetFullPath("StandartizationTests\\error.txt");
        }
    }
}
