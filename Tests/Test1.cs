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
        [TestMethod]
        public void Server_BruteForceTest()
        {

        }
    }
}
