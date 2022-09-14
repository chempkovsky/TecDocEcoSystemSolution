using System;

namespace SimpleLocalisation.Parsing {

	public interface IPatternTransformer {
		String Encode(String pattern);
		String Decode(String encodedPattern);
	}
}
