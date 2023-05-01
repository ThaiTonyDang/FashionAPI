using API.Extensions;
using Domain.Services;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> SaveFile([FromForm]IFormFile file, [FromForm]string folder)
        {
            var fileName = file.FileName;
            var fullPath = await _fileService.GetImagePath(fileName);

            if(file != null)
            {
                var data = await file.GetBytes();
                await this._fileService.SaveFile(fullPath, data);
            }

            return Ok(new {
                statusCode = 200,
                path = imagePath
            });
        }
    }
}
