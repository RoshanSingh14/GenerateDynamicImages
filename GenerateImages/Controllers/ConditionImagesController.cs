using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Web;

namespace GenerateImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenderImageController : ControllerBase
    {
        public IWebHostEnvironment _webHostEnvironment;
        public GenderImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("Gender.png")]
        public IActionResult GetImage(Uri imageUrl)
        {
            try
            {
                // To Get Gender Parameter from URL.
                string? gender = HttpUtility.ParseQueryString(imageUrl.Query)["gender"];

                // Store the Project Path.
                string imagePath = _webHostEnvironment.ContentRootPath + "\\Images\\"+ gender +".jpg";

                byte[] imageData;

                if (string.IsNullOrEmpty(gender))
                {
                    return BadRequest("Invalid Parameter");
                }
                // Returns the image based on Gender.
                else if (System.IO.File.Exists(imagePath))
                {
                    imageData= System.IO.File.ReadAllBytes(imagePath);
                }
                else
                {
                    return BadRequest("Invalid Gender");
                }
                return File(imageData, "image/png");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GenderandText.png")]

        public IActionResult GetConditionImage(Uri imageUrl)
        {
            try
            {
                // To Get Gender Parameter from URL.
                string? gender = HttpUtility.ParseQueryString(imageUrl.Query)["gender"];
                string? name = HttpUtility.ParseQueryString(imageUrl.Query)["name"];
                string? fileName = HttpUtility.ParseQueryString(imageUrl.Query)["filename"];

                // Store the Project Path.
                string imagePath;

                byte[] imageData;

                if (!string.IsNullOrEmpty(gender))
                {
                    imagePath =_webHostEnvironment.ContentRootPath + "\\Images\\"+ gender +".jpg";
                    // Returns the image based on Gender.
                    if (System.IO.File.Exists(imagePath))
                    {
                        imageData= System.IO.File.ReadAllBytes(imagePath);
                    }
                    else
                    {
                        return BadRequest("Invalid Gender");
                    }
                    return File(imageData, "image/png");
                }
                else if (!string.IsNullOrEmpty(name))
                {
                    imagePath = _webHostEnvironment.ContentRootPath + "\\Images\\"+ fileName +".jpg";
                    // Returns the image based on Gender.
                    if (System.IO.File.Exists(imagePath))
                    {
                        imageData= System.IO.File.ReadAllBytes(imagePath);
                    }
                    else
                    {
                        return BadRequest("Invalid Gender");
                    }
                    // Using SkiaSharp Library to createt image Instance and Draw Text On Image.
                    using (SKBitmap bitmap = SKBitmap.Decode(imageData))
                    {
                        SKCanvas canvas = new SKCanvas(bitmap);

                        SKPaint paint = new SKPaint();

                        paint.TextSize = 32;
                        paint.Color = SKColors.White;
                        paint.Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Bold);

                        //if (string.IsNullOrEmpty(name))
                        //{
                        //    throw new Exception("Invalid Parameter");
                        //}

                        if (!string.IsNullOrEmpty(name))
                        {
                            float x = (bitmap.Width - paint.MeasureText(name)) / 2;
                            float y = (bitmap.Height - paint.TextSize) / 2;

                            canvas.DrawText(name, x, y, paint);
                        }

                        MemoryStream memoryStream = new MemoryStream();

                        SKImage image = SKImage.FromBitmap(bitmap);

                        SKData data = image.Encode(SKEncodedImageFormat.Png, 100);

                        data.SaveTo(memoryStream);

                        memoryStream.Position = 0;

                        return File(memoryStream, "image/png");
                    }

                }
                else
                {
                    imagePath = _webHostEnvironment.ContentRootPath + "\\Images\\"+ fileName +".jpg";
                    // Returns the image based on Gender.
                    if (System.IO.File.Exists(imagePath))
                    {
                        imageData= System.IO.File.ReadAllBytes(imagePath);
                    }
                    else
                    {
                        return BadRequest("Invalid Gender");
                    }
                    return File(imageData, "image/png");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
