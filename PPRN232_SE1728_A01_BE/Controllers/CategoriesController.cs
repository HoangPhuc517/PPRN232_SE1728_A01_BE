using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Entity;
using Services.DTOs;
using Services.Interface;

namespace PPRN232_SE1728_A01_BE.Controllers
{

	public class CategoriesController : ODataController
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		[EnableQuery]
		public IActionResult Get()
		{
			try
			{
				var categories = _categoryService.GetAllCategories();
				return Ok(categories);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Patch(int key, [FromBody] Delta<Category> delta)
		{
			try
			{
				var updatedCategory = await _categoryService.UpdateCategoryAsync(key, delta);
				return Ok(updatedCategory);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Post([FromBody] CreateCategoryRequest category)
		{
			if (category == null)
			{
				return BadRequest("Category cannot be null.");
			}

			try
			{
				return Created(await _categoryService.CreateCategoryAsync(category));
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Delete(int key)
		{
			try
			{
				await _categoryService.DeleteCategoryAsync(key);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
