using System;
using System.Text;

namespace SimpleLocalisation.Processing.ValueFormatters {

	/// <summary>
	/// Thanks http://www.blackwasp.co.uk/RomanToNumber.aspx
	/// </summary>
	class NumberToRomanConvertor {
		// Converts an integer value into Roman numerals
		public string NumberToRoman(Int32 number) {
			// Validate
			if (number < 0 || number > 3999)
				throw new ArgumentOutOfRangeException("number");

			if (number == 0) return "N";

			// Set up key numerals and numeral pairs
			int[] values = new int[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };
			string[] numerals = new string[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };

			// Initialise the string builder
			StringBuilder result = new StringBuilder();

			// Loop through each of the values to diminish the number
			for (int i = 0; i < 13; i++) {
				// If the number being converted is less than the test value, append
				// the corresponding numeral or numeral pair to the resultant string
				while (number >= values[i]) {
					number -= values[i];
					result.Append(numerals[i]);
				}
			}

			// Done
			return result.ToString();
		}
	}
}