using API.Dtos;
using API.Extensions;
using Domain.Dtos;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private string _fileFolder;
        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [Authorize]
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> SaveFileAsync([FromForm] IFormFile file)
        {
            if(file == null || file.Length <= 0)
            {
                return BadRequest(new Error(
                    (int)HttpStatusCode.BadRequest,
                    "Upload file failed",
                    new[] { "Please Upload File" }));
            }

            var fileDto = new FileDto
            {
                ContentLength = file.Length,
                ContentType = file.ContentType,
                Data = await file.GetBytes(),
                FileName = file.FileName,
                Stream = file.OpenReadStream()
            };

            var fileName = await _fileService.SaveAsync(fileDto);
            var sublink = HTTTP.SLUG;
            var baseUrl = HttpContext.Request.BaseUrl();
            var linkFileName = _fileService.GetFileLink(baseUrl, sublink, fileName);
            
            return Ok(new SuccessData<List<string>>(
                (int)HttpStatusCode.OK,
                "Save File Successfully",
                new List<string>
                {
                    fileName,
                    linkFileName
                }
            ));

        }

        [HttpGet]
        [Route($"/{HTTTP.SLUG}/{{fileName}}")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {    
            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrEmpty(extension))
            {
                return BadRequest(new Error(
                    (int)HttpStatusCode.BadRequest,
                    "Get file name error",
                    new[] { "File's name cannot be empty !" }
                ));
            }

            string contentType = fileName.GetContentType();
            var fileDto = new FileDto
            {
                FileName = fileName,
                ContentType = contentType
            };

            var data = await this._fileService.GetAsync(fileDto);
            if (data == default(byte[]))
            {
                return NotFound(new Error
                (
                    (int)HttpStatusCode.NotFound,
                    "Get file name error",
                    new[] { "File is not found !" }
                ));
            }

            return File(data, contentType);
        }     
        
    }
}
