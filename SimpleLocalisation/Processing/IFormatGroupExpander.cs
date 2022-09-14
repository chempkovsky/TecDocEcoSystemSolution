using System;

namespace SimpleLocalisation.Processing {

	public interface IFormatGroupExpander {
		String Expand(String pattern, String content);
	}
}