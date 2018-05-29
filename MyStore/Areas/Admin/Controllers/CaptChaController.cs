using FX.Core.Utils;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;

namespace MyStore.Areas.Admin.Controllers
{
    public class CaptChaController : Controller
    {
        private const int height = 30;
        private const int width = 80;
        private const int length = 4;

        public PartialViewResult GetCaptcha()
        {
            var randomText = TextHelper.GenerateRandomText(4);

            var rnd = new Random();
            var fonts = new[] { "Verdana", "Times New Roman" };
            float orientationAngle = rnd.Next(0, 359);

            var index0 = rnd.Next(0, fonts.Length);
            var familyName = fonts[index0];

            using (var bmpOut = new Bitmap(width, height))
            {
                var g = Graphics.FromImage(bmpOut);
                var gradientBrush = new LinearGradientBrush(new Rectangle(0, 0, width, height),
                                                            Color.White, Color.DarkGray,
                                                            orientationAngle);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
                DrawRandomLines(ref g, width, height);
                g.DrawString(randomText, new Font(familyName, 18), new SolidBrush(Color.LightSlateGray), 0, 2);
                var ms = new MemoryStream();
                bmpOut.Save(ms, ImageFormat.Png);
                var bmpBytes = ms.GetBuffer();
                bmpOut.Dispose();
                ms.Close();
                return PartialView(bmpBytes);
            }
        }

        private static void DrawRandomLines(ref Graphics g, int width, int height)
        {
            var rnd = new Random();
            var pen = new Pen(Color.Gray);
            for (var i = 0; i < 10; i++)
            {
                g.DrawLine(pen, rnd.Next(0, width), rnd.Next(0, height),
                                rnd.Next(0, width), rnd.Next(0, height));
            }
        }

        private static string GetSalt()
        {
            return typeof(CaptChaController).Assembly.FullName;
        }
    }
}