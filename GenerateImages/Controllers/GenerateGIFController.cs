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
            MagickImageCollection collection = new MagickImageCollection();

            for (int i = 1; i <= 59; i++)
            {
                MagickImage image = new MagickImage(MagickColors.White, 200, 200);

                using (MagickImage caption = new MagickImage($"label:{i}", new MagickReadSettings
                {

                    Width = 200,
                    Height = 50,
                    FillColor = MagickColors.Black,
                    TextGravity = Gravity.Center
                }))
                {
                    image.Composite(caption, Gravity.Center);
                }

                collection.Add(image);
            }

            foreach (var frame in collection)
            {
                // Set the 1 Second Delay.
                frame.AnimationDelay = 100; 
            }

            collection.Quantize(new QuantizeSettings { Colors = 256 }); 

            var stream = new MemoryStream();
            collection.Write(stream, MagickFormat.Gif);
            stream.Position = 0;

            return File(stream, "image/gif");
        }
    }
}
