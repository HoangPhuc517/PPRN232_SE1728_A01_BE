using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.Entity;
using Services.DTOs;
using Services.Interface;

namespace PPRN232_SE1728_A01_BE.Controllers
{

	public class NewsArticlesController : ODataController
	{
		private readonly INewsArticleService _newsArticleService;

		public NewsArticlesController(INewsArticleService newsArticleService)
		{
			_newsArticleService = newsArticleService;
		}

		[EnableQuery]
		public IActionResult Get()
		{
			try
			{
				var articles = _newsArticleService.GetAllNewsArticles();
				return Ok(articles);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Post([FromBody] CreateNewArticleRequest newsArticle)
		{
			if (newsArticle == null)
			{
				return BadRequest("News article cannot be null.");
			}

			try
			{
				var createdArticle = await _newsArticleService.CreateNewArticleAsync(newsArticle);
				return Created(createdArticle);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Patch(int key, [FromBody] Delta<NewsArticle> delta)
		{
			if (delta == null)
			{
				return BadRequest("Delta cannot be null.");
			}

			try
			{
				var updatedArticle = await _newsArticleService.UpdateNewsArticleAsync(key, delta);
				return Updated(updatedArticle);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
		}

		[EnableQuery]
		public async Task<IActionResult> Delete(int key)
		{
			try
			{
				await _newsArticleService.DeleteNewsArticleAsync(key);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
			}
		}
	}
}
