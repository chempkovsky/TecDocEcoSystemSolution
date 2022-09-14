using System;
using System.Text;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Extensions {

	/// <summary>
	/// Extension methods for the String object.
	/// </summary>
	public static class StringExtensions {

		/// <summary>
		/// Extension method for creating a 'slug' from a string.
		/// A slug will contain a lower-cased string, containing letters, digits or the hyphen.
		/// </summary>
		/// <param name="input">String to convert into a slug.</param>
		/// <returns>A lower-cased 'slug' string</returns>
		public static String ToSlug(this String input) {
			StringBuilder output = new StringBuilder();
			String lower = input.ToLower();
			foreach (Char c in lower) {
				if (c == ' ' || c == '.' || c == '=' || c == '-') {
					output.Append('-');
				}
				else if (Char.IsLetterOrDigit(c)) {
					//else if ((c <= 'z' && c >= 'a') || (c <= '9' && c >= '0')) {
					output.Append(c);
				}
			}

			return output.ToString().Replace("--", "-").Trim('-');
		}

		/// <summary>
		/// Extension method for creating a 'slug' from a string.
		/// A slug will contain a lower-cased string, containing english letters, digits or the hyphen.
		/// </summary>
		/// <param name="input">String to convert into a slug.</param>
		/// <returns>A lower-cased 'slug' string</returns>
		public static String ToEnglishSlug(this String input) {
			StringBuilder output = new StringBuilder();
			String lower = input.ToLower();
			foreach (Char c in lower) {
				if (c == ' ' || c == '.' || c == '=' || c == '-') {
					output.Append('-');
				}
				else if ((c <= 'z' && c >= 'a') || (c <= '9' && c >= '0')) {
					output.Append(c);
				}
			}

			return output.ToString().Replace("--", "-").Trim('-');
		}

		/// <summary>
		/// The first (maxLength) part of the input string is returned. The text is split on
		/// white-space, punctuation and separators.
		/// </summary>
		/// <param name="input">The string to extract the digest from.</param>
		/// <param name="maxLength">The maximum number of character in the digest.</param>
		/// <returns></returns>
		public static String ToDigest(this String input, Int32 maxLength) {
			// Is this string actually longer than the allow max. chars?
			if (input.Length <= maxLength) {
				// No, let's just return it then!
				return input;
			}
			// Let's cut the string!
			String output = input.Substring(0, maxLength);
			// Let's start from the end!
			Int32 index = maxLength;
			while (index > 0) {
				Char indexChar = output[index - 1];
				// Is this a good place to end the digest?
				if (Char.IsWhiteSpace(indexChar) || Char.IsPunctuation(indexChar) || Char.IsSeparator(indexChar)) {
					break;
				}
				index--;
			}
			// Did we break before first char?
			if (index <= 0) {
				index = maxLength + 1;
			}
			return output.Substring(0, index - 1);
		}

		/// <summary>
		/// The method takes the input string, splits it at any newline, and wraps the part in the paragraph-tag
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static MvcHtmlString ToParagraphs(this String input) {
			String output = String.Empty;
			if (!String.IsNullOrWhiteSpace(input)) {
				// Let's split the string on newline chars!
				String[] parts = input.Split(new Char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
				// Iterate the parts!
				foreach (String part in parts) {
					// Let's create paragraphs!
					TagBuilder paragraph = new TagBuilder("p");
					paragraph.SetInnerText(part);
					// And join then!
					output += paragraph.ToString(TagRenderMode.Normal);
				}
			}
			// Time to output the result!
			return new MvcHtmlString(output);
		}
	}
}