
using Domain.Dtos;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using Utilities.GlobalHelpers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categotyService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categotyService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var categories = await _categotyService.GetListCategoryAsync();
            return Ok(new
            {
                StatusCode = (int)HttpStatusCode.OK,
                IsSuccess = true,
                Message = "Get list success",
                Data = categories,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = "Categoy Cannot Be Empty !"
                });
            }
            var result = await _categotyService.CreateCategoryAsync(categoryDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
                return Ok(new
                {
                    StatusCode = HttpStatusCode.Created,
                    IsSuccess = true,
                    Messenger = $"{message}"
                });
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Messenger = $"{message}"
            });
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            var result = await _categotyService.UpdateCategoryAsync(categoryDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = $"{message}",
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> Delete(string categoryId)
        {
            var isConverToGuid = categoryId.IsConvertToGuid();
            if (!isConverToGuid)
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Messenger = "Category Id Is Invalid ! Delete Failed !"
                });

            var result = await _categotyService.DeleteCategoryAsync(new Guid(categoryId));
            var isSuccess = result.Item1;
            var message = result.Item2;

            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = $"{message}",
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> GetCategory(string categoryId)
        {
            var isConverToGuid = categoryId.IsConvertToGuid();
            if (!isConverToGuid)
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Messenger = "Category Id Is Invalid !"
                });

            var result = await _categotyService.GetCategoryById(new Guid(categoryId));
            var categoryDto = result.Item1;
            var message = result.Item2;

            if (categoryDto != null)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = categoryDto
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }
    }
}
