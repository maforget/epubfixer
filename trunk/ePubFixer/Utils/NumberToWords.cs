using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Text;

namespace ePubFixer
{
    public static class NumberToWordsConverter
    {
        #region Fields
        // Single-digit and small number names
        private static string[] _smallNumbers = new string[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

        // Tens number names from twenty upwards
        private static string[] _tens = new string[] { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        // Scale number names for use during recombination
        private static string[] _scaleNumbers = new string[] { "", "Thousand", "Million", "Billion" }; 
        #endregion

        #region Number To Roman Numerals
        public static string NumberToRoman(double number)
        {
            int num = (int)number;
            return NumberToRoman(num);
        }

        // Converts an integer value into Roman numerals
        public static string NumberToRoman(int number)
        {
            // Validate
            if (number < 0 || number > 3999)
                throw new ArgumentException("Value must be in the range 0 - 3,999.");

            if (number == 0) return "N";

            // Set up key numerals and numeral pairs
            int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
            string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

            // Initialise the string builder
            StringBuilder result = new StringBuilder();

            // Loop through each of the values to diminish the number
            for (int i = 0; i < 13; i++)
            {
                // If the number being converted is less than the test value, append
                // the corresponding numeral or numeral pair to the resultant string
                while (number >= values[i])
                {
                    number -= values[i];
                    result.Append(numerals[i]);
                }
            }

            // Done
            return result.ToString();
        } 
        #endregion

        #region Number To Words
        public static string NumberToWords(double number)
        {
            int num = (int)number;
            return NumberToWords(num);
        }

        // Converts an integer value into English words
        public static string NumberToWords(int number)
        {
            // Zero rule
            if (number == 0)
                return _smallNumbers[0];

            // Array to hold four three-digit groups
            int[] digitGroups = new int[4];

            // Ensure a positive number to extract from
            int positive = Math.Abs(number);

            // Extract the three-digit groups
            for (int i = 0; i < 4; i++)
            {
                digitGroups[i] = positive % 1000;
                positive /= 1000;
            }

            // Convert each three-digit group to words
            string[] groupText = new string[4];

            for (int i = 0; i < 4; i++)
                groupText[i] = ThreeDigitGroupToWords(digitGroups[i]);

            // Recombine the three-digit groups
            string combined = groupText[0];
            bool appendAnd;

            // Determine whether an 'and' is needed
            appendAnd = (digitGroups[0] > 0) && (digitGroups[0] < 100);

            // Process the remaining groups in turn, smallest to largest
            for (int i = 1; i < 4; i++)
            {
                // Only add non-zero items
                if (digitGroups[i] != 0)
                {
                    // Build the string to add as a prefix
                    string prefix = groupText[i] + " " + _scaleNumbers[i];

                    if (combined.Length != 0)
                        prefix += appendAnd ? " and " : ", ";

                    // Opportunity to add 'and' is ended
                    appendAnd = false;

                    // Add the three-digit group to the combined string
                    combined = prefix + combined;
                }
            }

            // Negative rule
            if (number < 0)
                combined = "Negative " + combined;

            return combined;
        }

        // Converts a three-digit group into English words
        private static string ThreeDigitGroupToWords(int threeDigits)
        {
            // Initialise the return text
            string groupText = "";

            // Determine the hundreds and the remainder
            int hundreds = threeDigits / 100;
            int tensUnits = threeDigits % 100;

            // Hundreds rules
            if (hundreds != 0)
            {
                groupText += _smallNumbers[hundreds] + " Hundred";

                if (tensUnits != 0)
                    groupText += " and ";
            }

            // Determine the tens and units
            int tens = tensUnits / 10;
            int units = tensUnits % 10;

            // Tens rules
            if (tens >= 2)
            {
                groupText += _tens[tens];
                if (units != 0)
                    //groupText += " " + _smallNumbers[units];
                    groupText += "-" + _smallNumbers[units];
            } else if (tensUnits != 0)
                groupText += _smallNumbers[tensUnits];

            return groupText;
        } 
        #endregion
    }

}