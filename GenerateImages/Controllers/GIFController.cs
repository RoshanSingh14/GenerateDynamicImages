using ImageMagick;
using Microsoft.AspNetCore.Mvc;

namespace GenerateImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GIFController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GIFController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GIFImages")]
        public IActionResult GetGIFImage()
        {
            string image1 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_01.png";
            string image2 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_02.png";
            string image3 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_03.png";
            string image4 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_04.png";
            string image5 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_05.png";
            string image6 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_06.png";
            string image7 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_07.png";
            string image8 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_08.png";
            string image9 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_09.png";
            string image10 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_10.png";
            string image11 = _webHostEnvironment.ContentRootPath + "\\GIF\\" + "frame_11.png";



            string[] imagePaths = { image1, image2, image3, image4, image5, image6, image7, image8, image9, image10, image11 };
            string outputPath = _webHostEnvironment.ContentRootPath + "\\Images\\" +"output.gif";
            int delayBetweenFrames = 10;

            CreateGifImage(imagePaths, outputPath, delayBetweenFrames);

            return PhysicalFile(outputPath, "image/gif");
        }

        [NonAction]
        public void CreateGifImage(string[] imagePaths, string outputPath, int delayBetweenFrames)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {

                foreach (string imagePath in imagePaths)
                {
                    collection.Add(imagePath);

                    collection[collection.Count - 1].AnimationDelay = delayBetweenFrames;
                }

                collection.Write(outputPath);
            }
        }

    }
}
