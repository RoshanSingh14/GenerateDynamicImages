using ImageMagick;
using Microsoft.AspNetCore.Mvc;

namespace ImageGenerationAPI.Controllers
{
    public class GenerateGIFController : ControllerBase
    {
        [HttpGet]
        [Route("GetNumbers")]
        public IActionResult GenerateImage()
        {
            // Convert string dateofbirth to datetime.
            DateTime dateOfBirth = new DateTime(2023, 06, 01);

            DateTime currentDate = DateTime.Now;

            // Add 7 Days to Birthdate For Valid Offer.
            DateTime validDate = dateOfBirth.AddDays(7);

            // TO Set the Timer that how many TimeSpan is remaning for Offer.
            TimeSpan remainingTime = validDate - currentDate;

            // TO Store GIF
            MagickImageCollection imageCollection = new MagickImageCollection();

            for (int seconds = 59; seconds >= 0; seconds--)
            {
                MagickImage image = new MagickImage(MagickColors.White, 200, 200);

                using (MagickImage caption = new MagickImage($"label:{remainingTime.ToString("dd\\:hh\\:mm")}:{seconds:00}", new MagickReadSettings
                // using (MagickImage caption = new MagickImage($"label:days\nminutes\nseconds\n{remainingTime.Days:00}:{remainingTime.Hours:00}:{remainingTime.Minutes:00}:{seconds:00}", new MagickReadSettings
                {
                    Width = 200,
                    Height = 50,
                    FillColor = MagickColors.Black,
                    TextGravity = Gravity.Center
                }))
                {
                    image.Composite(caption, Gravity.Center);
                }
                imageCollection.Add(image);
            }

            foreach (var frame in imageCollection)
            {
                // Set the 1 second Delay in FrameDelay
                frame.AnimationDelay = 100;
            }

            imageCollection.Quantize(new QuantizeSettings { Colors = 256 });

            var memoryStream = new MemoryStream();
            imageCollection.Write(memoryStream, MagickFormat.Gif);
            memoryStream.Position = 0;

            return File(memoryStream, "image/gif");
        }

    }
}
