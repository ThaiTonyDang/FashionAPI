using API.Extensions;
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

        [HttpPost("upload")]
        public async Task<IActionResult> SaveFile([FromForm] IFormFile file)
        {
            var dataList = new List<string> { };
            if(file == null)
            {
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Message = "Please Upload File"
                });
            }
            
            var baseUrl = HttpContext.Request.BaseUrl();
            var fileName = _fileService.GetFullFileName(file.FileName);

            if (file.ContentType.Contains("image"))
            {
                fileName = _fileService.GetFullImageName(file.FileName, SIZE.Width, SIZE.Height);
                _fileFolder = GetFolderNameByDateCreate(fileName);
                var stream = file.OpenReadStream();
                _fileService.ResizeImage(stream, fileName, _fileFolder);
            }
            if (!file.ContentType.Contains("image"))
            {
                var data = await file.GetBytes();
                await this._fileService.SaveFileAsync(fileName, data, _fileFolder);
            }

            var sublink = HTTTP.SLUG;
            var link = this._fileService.GetFileLink(baseUrl, sublink, fileName);
            dataList.Add(fileName);
            dataList.Add(link);             
            
            return Ok(new
            {
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
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = "File's Name Cannot Be Empty !"
                });
            }

            _fileFolder = GetFolderNameByDateCreate(fileName);
            var imageBytes = await _fileService.GetFileBytesAsync(fileName, _fileFolder);
            if (imageBytes == default(byte[]))
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Message = "Image Not Found !"
                });
            }

            string contentType = fileName.GetContentType();
            return File(imageBytes, contentType);
        }     
        
        private string GetFolderNameByDateCreate(string fileName)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var resultstring = new string(fileNameWithoutExtension.ToCharArray().Reverse().ToArray());
            var folderName = new string(resultstring.Substring(6, 8).ToCharArray().Reverse().ToArray());
            return folderName;
        }
    }
}
