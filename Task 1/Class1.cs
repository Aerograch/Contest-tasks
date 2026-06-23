using System.Text;
using System.Text.RegularExpressions;

namespace Task_1
{
    public static class Encoding
    {
        /// <summary>
        /// Encodes an arbitrary string consisting of lowercase latin characters.
        /// Format of encoding "aabbbccd" => "a2b3c2d"
        /// </summary>
        /// <exception cref="FormatException">
        /// Thrown when string contains anything else than lowercase latin characters
        /// </exception>
        public static string Encode(string input)
        {
            // Input validation
            string requieredPattern = "^[a-z]+$";
            if (!Regex.IsMatch(input, requieredPattern))
                throw new FormatException();

            // Encoding
            char previousChar;
            int counter = 1;
            string output = "";
            for (int i = 1; i < input.Length; i++)
            {
                previousChar = input[i - 1];
                if (input[i] != previousChar)
                {
                    output += previousChar;
                    if (counter > 1)
                    {
                        output += counter;
                    }
                    counter = 0;
                }
                counter++;
            }

            output += input[input.Length - 1];
            if (counter > 1)
            {
                output += counter;
            }

            return output;
        }

        /// <summary>
        /// Decodes a string consisting of numbers and lowercase latin characters.
        /// Format of decoding "a2b3c2d" => "aabbbccd"
        /// </summary>
        /// <exception cref="FormatException">
        /// Thrown when string contains anything else than numbers and lowercase latin characters
        /// </exception>
        public static string Decode(string input)
        {
            // Input validation
            string requieredPattern = "^[a-z][a-z0-9]*$";
            if (!Regex.IsMatch(input, requieredPattern))
                throw new FormatException();

            // Decoding
            string output = "";
            int numberOrder = 0;
            int counter;
            int charAmount;

            for (int i = 0; i < input.Length; i++)
            {
                // Check if current char is not number
                if (input[i] >= '0' && input[i] <= '9') 
                    continue;

                // Get order of magnitude of amount of current char
                numberOrder = 0;
                counter = i + 1;
                while (counter < input.Length && input[counter] >= '0' && input[counter] <= '9')
                {
                    numberOrder++;
                    counter++;
                }
                
                if (numberOrder == 0)
                {
                    output += input[i];
                }
                else
                {
                    // Get char aount
                    charAmount = 0;
                    counter = 0;
                    for (int j = i + numberOrder; j > i; j--)
                    {
                        charAmount += (input[j] - '0') * ((int)Math.Pow(10, counter));
                        counter++;
                    }

                    output += new StringBuilder().Append(input[i], charAmount).ToString();
                }
            }

            return output;
        }
    }
}
