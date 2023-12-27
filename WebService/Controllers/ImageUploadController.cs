using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using System;

namespace WebService.Controllers
{
    [Route("api/ImageUpload")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        private const string UploadDirectory = "Uploads"; // Directory to store uploaded images

            [HttpPost]
        [HttpPost, Route("/api/ImageUpload/UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                if (Request.Form.Files!=null)
                {
                    var file = Request.Form.Files[0]; // Assuming only one file is uploaded

                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), UploadDirectory, file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        return Ok("Image uploaded successfully.");
                    }
                }

                return BadRequest("No image uploaded.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
