using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Controllers {

	public class AlgebraCaptchaController : Controller {

		public ActionResult Read(String prefix) {
			Random rand = new Random();

			// Generate the numbers for the calculation!
			Int32 firstNumber = rand.Next(10, 99);
			Int32 secondNumber = rand.Next(0, 9);
			String captcha = String.Format("{0} + {1} = ?", firstNumber, secondNumber);

			// Let's store the answer
			new CalculatedCaptcha(prefix).StoreResult(firstNumber + secondNumber);

			// Create the output image stream
			FileContentResult image = null;

			using (MemoryStream memory = new MemoryStream()) {
				using (Bitmap bitmap = new Bitmap(130, 30)) {
					using (Graphics gfx = Graphics.FromImage((Image)bitmap)) {
						gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						gfx.SmoothingMode = SmoothingMode.AntiAlias;
						gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

						Int32 r, x, y;
						Pen pen = new Pen(Color.Yellow);
						// Let's add noise
						for (Int32 index = 1; index < 10; index++) {
							pen.Color = Color.FromArgb(
														(rand.Next(0, 255)),
														(rand.Next(0, 255)),
														(rand.Next(0, 255))
													);

							r = rand.Next(0, (130 / 3));
							x = rand.Next(0, 130);
							y = rand.Next(0, 30);

							gfx.DrawEllipse(pen, x - r, y - r, r, r);
						}

						// Add question/calculation
						gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

						// Render as jpeg
						bitmap.Save(memory, ImageFormat.Jpeg);
						image = this.File(memory.GetBuffer(), "image/jpeg");
					}
				}
			}
			return image;
		}
	}
}