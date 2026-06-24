using System.Text.RegularExpressions;

namespace Task_3
{
    internal class Program
    {
        enum LoggingFormat
        {
            Format1,
            Format2,
            InvalidFormat
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }

        public static class Standartization
        {
            public static void WriteLog(string inputPath, string outputPath, string errorPath)
            {
                string firstFormatPattern = "^[0-3][0-9]\\.[0-1][0-9]\\.[0-9]{4} [0-2][0-9]:[0-6][0-9]:[0-6][0-9]\\.[0-9]{1,4} ((INFORMATION)|(WARNING)|(ERROR)|(DEBUG)) .+$";
                string secondFormatPattern = "[0-9]{4}-[0-1][0-9]-[0-3][0-9] [0-2][0-9]:[0-6][0-9]:[0-6][0-9].[0-9]{1,4}\\| ((INFO)|(WARN)|(ERROR)|(DEBUG))\\|[^|]*\\|[^|]+\\| .+$";

                string[] inputLogs = File.ReadAllLines(inputPath);
                string stdOutput = "";
                string errOutput = "";
                Dictionary<string, string> data;
                LoggingFormat format;

                foreach (string log in inputLogs)
                {
                    if (Regex.IsMatch(log, firstFormatPattern))
                        format = LoggingFormat.Format1;
                    else if (Regex.IsMatch(log, secondFormatPattern))
                        format = LoggingFormat.Format2;
                    else
                        format = LoggingFormat.InvalidFormat;

                    data = new Dictionary<string, string>();

                    
                }
            }
        }
    }
}
