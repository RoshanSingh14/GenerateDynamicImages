using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace GenerateImages.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetImagesController : ControllerBase
    {
        public IWebHostEnvironment _webHostEnvironment;
        public GetImagesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        [Route("DownloadImage.png")]
        public IActionResult GetApiData(Uri imageUrl)
        {
            try
            {
                // To Get FileName from URL Paramaters.
                string? fileName = HttpUtility.ParseQueryString(imageUrl.Query)["filename"];

                // TO Store Image Bytes.
                byte[] imageData;

                if (string.IsNullOrEmpty(fileName))
                {
                    return BadRequest("Image Name Not Specified");
                }
                // Store the Project Path.
                string imagePath = _webHostEnvironment.ContentRootPath + "\\Images\\"+ fileName +".gif";

                if (System.IO.File.Exists(imagePath))
                {
                    imageData= System.IO.File.ReadAllBytes(imagePath);
                }
                else
                {
                    return BadRequest("Invalid URL");
                }
                return File(imageData, "image/png");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}

