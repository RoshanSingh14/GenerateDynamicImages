using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Web;

namespace GenerateImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddTextImagesController : ControllerBase
    {
        public IWebHostEnvironment _webHostEnvironment;
        public AddTextImagesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        [Route("AddText.png")]
        public IActionResult AddText(Uri imageUrl)
        {
            try
            {
                // To Get Name and FileName from URL Paramaters.
                string? fileName = HttpUtility.ParseQueryString(imageUrl.Query)["filename"];
                string? name = HttpUtility.ParseQueryString(imageUrl.Query)["name"];

                // TO Store Image Bytes.
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
                    return NotFound("Image Not Found");
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
