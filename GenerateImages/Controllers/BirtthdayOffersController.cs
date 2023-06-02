using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Globalization;
using System.Web;

namespace GenerateImages.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BirthdayOffersController : ControllerBase
    {
        public IWebHostEnvironment _webHostEnvironment;
        public BirthdayOffersController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("BirthdayOffers.png")]
        public IActionResult GenerateCountdownImage(Uri imageUrl)
        {
            try
            {
                // To Get DateofBirth and FileName from URL Paramaters.
                string? fileName = HttpUtility.ParseQueryString(imageUrl.Query)["filename"];
                string? DateOfBirth = HttpUtility.ParseQueryString(imageUrl.Query)["dateofbirth"];

                byte[] imageData;
                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("Invalid URL or filename");
                }
                // Store the Project Path.
                string imagePath = _webHostEnvironment.ContentRootPath + "\\Images\\"+ fileName +".jpg";

                if (System.IO.File.Exists(imagePath))
                {
                    imageData= System.IO.File.ReadAllBytes(imagePath);
                }
                else
                {
                    return NotFound("File Not Found");
                }

                // Using SkiaSharp Library to createt image Instance and Draw Timer On Image.
                using (SKBitmap bitmap = SKBitmap.Decode(imageData))
                {
                    SKCanvas canvas = new SKCanvas(bitmap);

                    SKPaint paint = new SKPaint();

                    paint.TextSize = 32;
                    paint.Color = SKColors.White;
                    paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);

                    if (!string.IsNullOrEmpty(DateOfBirth))
                    {
                        // Convert string dateofbirth to datetime.
                        DateTime dateOfBirth = DateTime.ParseExact(DateOfBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                        DateTime currentDate = DateTime.Now;

                        // Add 7 Days to Birthdate For Valid Offer.
                        DateTime validDate = dateOfBirth.AddDays(7);

                        dateOfBirth = dateOfBirth.AddHours(currentDate.Hour)
                                                 .AddMinutes(currentDate.Minute)
                                                 .AddSeconds(currentDate.Second);

                        // TO Set the Timer that how many TimeSpan is remaning for Offer.
                        TimeSpan remainingTime = validDate - currentDate;

                        // To Check Valid Offer and Set Timer To 00:00.
                        if (remainingTime.TotalSeconds <= 0)
                        {
                            remainingTime = TimeSpan.Zero;
                        }

                        float x = (bitmap.Width - paint.MeasureText(remainingTime.ToString(@"dd\.hh\:mm\:ss"))) / 2;
                        float y = (bitmap.Height - paint.TextSize) / 2;

                        canvas.DrawText("Offer Valid For", x - 25, y - 45, paint);
                        canvas.DrawText(remainingTime.ToString(@"dd\:hh\:mm\:ss"), x, y, paint);

                    }

                    MemoryStream memoryStream = new MemoryStream();

                    SKImage image = SKImage.FromBitmap(bitmap);

                    SKData data = image.Encode(SKEncodedImageFormat.Png, 100);

                    data.SaveTo(memoryStream);

                    memoryStream.Position = 0;

                    return File(memoryStream, "image/png");

                }
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
