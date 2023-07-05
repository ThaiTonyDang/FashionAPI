
using Domain.Dtos;
using Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            var categories = await _categotyService.GetCategoryListAsync();
            if (categories != null)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = "Get list success",
                    Data = categories,
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = true,
                Message = "Get categories fail !",
            });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = "Category Cannot Be Empty !"
                });
            }
            categoryDto.CreateDate = DateTime.Now;
            var result = await _categotyService.CreateCategoryAsync(categoryDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            if (isSuccess)
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.Created,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = categoryDto
                });
            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = "Category Cannot Be Empty !"
                });
            }
            var searchResult = await _categotyService.GetCategoryByIdAsync(categoryDto.Id);
            var categoryEntity = searchResult.Item1;
            if (categoryEntity == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = true,
                    Message = "Category needs updating cannot found !",
                });
            }
            categoryDto.ModifiedDate = DateTime.Now;
            var result = await _categotyService.UpdateCategoryAsync(categoryDto);
            var isSuccess = result.Item1;
            var message = result.Item2;
            var data = new List<CategoryDto> { categoryEntity, categoryDto };
            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = data
                });
            }

            return BadRequest(new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                IsSuccess = false,
                Message = $"{message}"
            });
        }

        [Authorize]
        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> Delete(string categoryId)
        {
            var message = "";
            var isConverToGuid = categoryId.IsConvertToGuid();
            if (!isConverToGuid)
                return BadRequest(new
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    IsSuccess = false,
                    Messenger = "Category Id Is Invalid ! Delete Failed !"
                });
            var result = await _categotyService.GetCategoryByIdAsync(new Guid(categoryId));
            var categoryObject = result.Item1;
            if (categoryObject == null)
            {
                return NotFound(new
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    IsSuccess = false,
                    Message = $"Category not found ! Delete Fail !",
                });
            }
            var result_second = await _categotyService.DeleteCategoryAsync(new Guid(categoryId));
            var isSuccess = result_second.Item1;
            message = result_second.Item2;

            if (isSuccess)
            {
                return Ok(new
                {
                    StatusCode = (int)HttpStatusCode.NoContent,
                    IsSuccess = true,
                    Message = $"{message}",
                    Data = categoryObject
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
                    Message = "Category Id Is Invalid !"
                });

            var result = await _categotyService.GetCategoryByIdAsync(new Guid(categoryId));
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

        [Authorize]
        [HttpGet]
        [Route("total")]
        public async Task<IActionResult> GetTotal()
        {
            var total = await _categotyService.GetTotalCategory();
            return Ok(total);
        }
    }
}
