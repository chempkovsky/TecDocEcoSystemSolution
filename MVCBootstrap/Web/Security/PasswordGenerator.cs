using System;
using System.Text;

namespace MVCBootstrap.Web.Security {

	public static class PasswordGenerator {

		public static String Generate(Int32 length) {
			return PasswordGenerator.Generate(length, PasswordStrength.AlphaNumeric);
		}

		public static String Generate(Int32 length, PasswordStrength strength) {
			String output = String.Empty;

			Random rnd = new Random();
			Int32 max = 2 + (Int32)strength;

			for (Int32 index = 0; index < length; index++) {
				Int32 number = rnd.Next(1, max + 1);

				switch (number) {
					case 1:
						// uppercase english alphabet
						Byte uppercased = (Byte)rnd.Next(65, 91);
						output += Encoding.ASCII.GetString(new Byte[] { uppercased });
						break;
					case 2:
						// lowercase english alphabet
						Byte lowercased = (Byte)rnd.Next(97, 123);
						output += Encoding.ASCII.GetString(new Byte[] { lowercased });
						break;
					case 3:
						// the numbers, 0 to 9
						output += rnd.Next(0, 10).ToString();
						break;
					case 4:
						// the symbols, 33 to 47
						Byte symbol = (Byte)rnd.Next(33, 47);
						output += Encoding.ASCII.GetString(new Byte[] { symbol });
						break;
					default:
						throw new ApplicationException("Ups");
				}
			}

			return output;
		}
	}
}