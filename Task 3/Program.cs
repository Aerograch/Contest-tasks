using System.Text.RegularExpressions;

namespace Task_3
{
    public class Program
    {
        /// <summary>
        /// Simple enum defining format
        /// </summary>
        enum LoggingFormat
        {
            Format1,
            Format2,
            InvalidFormat
        }


        static void Main(string[] args)
        {
            
        }


        public static class Standartization
        {
            /// <summary>
            /// Reads logs from imput file and writes them to output file in unified format if able, else writes them to error file
            /// </summary>
            /// <returns>False if input file does not exist, or output file does not exist and createNew == false</returns>
            public static bool WriteLog(string inputPath, string outputPath, string errorPath, bool createNew = false)
            {
                string firstFormatPattern = "^[0-3][0-9]\\.[0-1][0-9]\\.[0-9]{4} [0-2][0-9]:[0-6][0-9]:[0-6][0-9]\\.[0-9]{1,4} ((INFORMATION)|(WARNING)|(ERROR)|(DEBUG)) .+$";
                string secondFormatPattern = "^[0-9]{4}-[0-1][0-9]-[0-3][0-9] [0-2][0-9]:[0-6][0-9]:[0-6][0-9].[0-9]{1,4}\\| ((INFO)|(WARN)|(ERROR)|(DEBUG))\\|[^|]*\\|[^|]+\\| .+$";

                if (!File.Exists(inputPath) || ((!File.Exists(outputPath) || !File.Exists(errorPath) && !createNew))) 
                    return false;

                string[] inputLogs = File.ReadAllLines(inputPath);
                List<string> stdOutput = new List<string>();
                List<string> errOutput = new List<string>();
                Dictionary<string, string> data;
                LoggingFormat format;

                foreach (string log in inputLogs)
                {
                    // Figure out input format
                    if (Regex.IsMatch(log, firstFormatPattern))
                        format = LoggingFormat.Format1;
                    else if (Regex.IsMatch(log, secondFormatPattern))
                        format = LoggingFormat.Format2;
                    else
                        format = LoggingFormat.InvalidFormat;

                    data = new Dictionary<string, string>();
                    data["method"] = "DEFAULT";

                    // Dump invalid log into err
                    if (format == LoggingFormat.InvalidFormat)
                    {
                        errOutput.Add(log.Trim('\n'));
                        continue;
                    }


                    // Prepare log data in easy to use format
                    if (format == LoggingFormat.Format1)
                    {
                        string[] logSplit = log.Split(' ');

                        // Bring date to correct format
                        string[] dateRaw = logSplit[0].Split('.');
                        data["date"] = dateRaw[0] + "-" + dateRaw[1] + "-" + dateRaw[2];

                        data["time"] = logSplit[1];
                        data["logLevel"] = logSplit[2];
                        data["message"] = log.Substring(logSplit.Take(3).Sum(x => x.Length) + 3).Trim('\n');

                        // Trim INFORMATION and WARNING to INFO and WARN
                        if (data["logLevel"].Length > 5)
                            data["logLevel"] = data["logLevel"].Substring(0, 4);
                    }
                    else if (format == LoggingFormat.Format2)
                    {
                        string[] logSplit = log.Split('|');

                        // Bring date to correct format
                        string[] dateRaw = logSplit[0].Split(' ')[0].Split('-');
                        data["date"] = dateRaw[2] + "-" + dateRaw[1] + "-" + dateRaw[0];

                        data["time"] = logSplit[0].Split(' ')[1];
                        data["logLevel"] = logSplit[1].Trim();
                        data["method"] = logSplit[3];

                        // Here we add + 5 because there are 4 delimiters and one space char
                        data["message"] = log.Substring(logSplit.Take(4).Sum(x => x.Length) + 5).Trim('\n');
                    }

                    stdOutput.Add(data["date"] + '\t' + data["time"] + '\t' + data["logLevel"] + '\t' + data["method"] + '\t' + data["message"]);
                }

                if (createNew)
                {
                    File.WriteAllLines(outputPath, stdOutput);
                    File.WriteAllLines(errorPath, errOutput);
                }
                else
                {
                    File.AppendAllLines(outputPath, stdOutput);
                    File.AppendAllLines(errorPath, errOutput);
                }
                return true;
            }
        }
    }
}
