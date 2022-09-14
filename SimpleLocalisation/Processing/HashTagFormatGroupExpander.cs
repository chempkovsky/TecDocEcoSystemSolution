using System;

namespace SimpleLocalisation.Processing {

	public class HashTagFormatGroupExpander : IFormatGroupExpander {
		public String Expand(String pattern, String content) {
			return pattern.Replace("{#}", content);
		}
	}
}