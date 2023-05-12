using API.Extensions;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> SaveFile([FromForm] IFormFile file)
        {
            if(file == null)
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "PLease upload file"
                });
            }

            var baseUrl = HttpContext.Request.BaseUrl();
            var fullFileName = _fileService.GetFullFileName(file.FileName);

            var data = await file.GetBytes();
            await this._fileService.SaveFileAsync(fullFileName, data);
            var link = this._fileService.GetFileLink(baseUrl, HTTTP.SLUG, fullFileName);
            
            return Ok(new
            {
                StatusCode = 200,
                Message = "Created Image successfully",
                IsSucces = true,
                fileName = fullFileName,
                Link = link
            });;
        }

        [HttpGet($"/{HTTTP.SLUG}/{{fileName}}")]
        public async Task<IActionResult> GetImage(string fileName)
        {           
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Image not found"
                });
            }

            var imageBytes = await _fileService.GetFileBytesAsync(fileName);
            if (imageBytes == default(byte[]))
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    Message = "Image not found"
                });
            }

            string contentType = fileName.GetContentType();
            return File(imageBytes, contentType);
        }      
    }
}
