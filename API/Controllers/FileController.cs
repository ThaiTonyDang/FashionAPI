using API.Extensions;
using Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<IActionResult> SaveFile([FromForm]IFormFile file)
        {
            if(file == null)
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false,
                    Message = "Please Upload File"
                });
            }

            var baseUrl = HttpContext.Request.BaseUrl();
            var fullFileName = _fileService.GetFullFileName(file.FileName);

            var data = await file.GetBytes();
            await this._fileService.SaveFileAsync(fullFileName, data);
            var link = this._fileService.GetFileLink(baseUrl, HTTTP.SLUG, fullFileName);
            var dataList = new List<string> { fullFileName, link } ;
            return Ok(new
            {
                StatusCode = HttpStatusCode.Created,
                Success = true,
                Message = "Created Image Success !",
                Data = dataList
            });
        }

        [HttpGet($"/{HTTTP.SLUG}/{{fileName}}")]
        public async Task<IActionResult> GetImage(string fileName)
        {           
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest(new
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "File's Name Cannot Be Empty !"
                });
            }

            var imageBytes = await _fileService.GetFileBytesAsync(fileName);
            if (imageBytes == default(byte[]))
            {
                return NotFound(new
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Image Not Found !"
                });
            }

            string contentType = fileName.GetContentType();
            return File(imageBytes, contentType);
        }      
    }
}
