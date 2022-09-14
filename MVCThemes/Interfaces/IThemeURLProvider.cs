/* MVCThemes
 * Copyright (C) 2011-2012 Steen F. Tøttrup
 * http://creativeminds.dk/
 */

using System;

namespace MVCThemes.Interfaces {

	public interface IThemeURLProvider {
		String GetThemeBaseURL(String theme);
	}
}