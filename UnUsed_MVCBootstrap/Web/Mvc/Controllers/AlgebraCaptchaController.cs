// MVCBootstrap.Web.Mvc.Controllers.AlgebraCaptchaController
using MVCBootstrap.Web.Mvc;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web.Mvc;

namespace MVCBootstrap.Web.Mvc.Controllers
{

    public class AlgebraCaptchaController : Controller
    {
        public ActionResult Read(string prefix)
        {
            Random random = new Random();
            int num = random.Next(10, 99);
            int num2 = random.Next(0, 9);
            string s = $"{num} + {num2} = ?";
            new CalculatedCaptcha(prefix).StoreResult(num + num2);
            FileContentResult fileContentResult = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (Bitmap bitmap = new Bitmap(130, 30))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                        graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        graphics.FillRectangle(Brushes.White, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                        Pen pen = new Pen(Color.Yellow);
                        for (int i = 1; i < 10; i++)
                        {
                            pen.Color = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
                            int num3 = random.Next(0, 43);
                            int num4 = random.Next(0, 130);
                            int num5 = random.Next(0, 30);
                            graphics.DrawEllipse(pen, num4 - num3, num5 - num3, num3, num3);
                        }
                        graphics.DrawString(s, new Font("Tahoma", 15f), Brushes.Gray, 2f, 3f);
                        bitmap.Save(memoryStream, ImageFormat.Jpeg);
                        return File(memoryStream.GetBuffer(), "image/jpeg");
                    }
                }
            }
        }
    }

}
