using AutoMapper;
using Core.Enum;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.EntityFrameworkCore;
using Repositories.Entity;
using Repositories.Interface;
using Services.DTOs;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
	public class NewArticleService : INewsArticleService
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public NewArticleService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<NewsArticle> CreateNewArticleAsync(CreateNewArticleRequest createNewArticle)
		{
			try
			{
				var maxId = await _unitOfWork.GenericRepository<NewsArticle>()
							  .GetAll()
							  .OrderByDescending(n => n.CreatedDate)
							  .Select(n => n.NewsArticleId)
							  .FirstOrDefaultAsync();


				int nextId = string.IsNullOrEmpty(maxId)
					? 1
					: int.Parse(maxId) + 1;

				var newsArticle = _mapper.Map<NewsArticle>(createNewArticle);

				newsArticle.Tags = await _unitOfWork.GenericRepository<Tag>()
						  .GetAll()
						  .Where(t => createNewArticle.TagIds.Contains(t.TagId))
						  .ToListAsync();

				newsArticle.NewsArticleId = nextId.ToString();
				newsArticle.CreatedDate = DateTime.Now;
				newsArticle.ModifiedDate = DateTime.Now;
				newsArticle.UpdatedById = newsArticle.CreatedById;
				newsArticle.NewsStatus = true;
				
				 await _unitOfWork.GenericRepository<NewsArticle>().InsertAsync(newsArticle);
				 await _unitOfWork.SaveChangeAsync();

				return newsArticle;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while creating the news article.", ex);
			}
		}

		public IEnumerable<NewsArticle> GetAllNewsArticles()
		{
			try
			{
				return _unitOfWork.GenericRepository<NewsArticle>().GetAll();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving news articles.", ex);
			}
		}


		public async Task DeleteNewsArticleAsync(int id)
		{
			try
			{
				var newsArticle = await _unitOfWork.GenericRepository<NewsArticle>()
								.GetAll()
								.Include(n => n.Tags)
								.FirstOrDefaultAsync(n => n.NewsArticleId == id.ToString());

				if (newsArticle == null)
				{
					throw new Exception($"News article with ID {id} not found.");
				}
				newsArticle.Tags.Clear();

				_unitOfWork.GenericRepository<NewsArticle>().Delete(newsArticle);
				var res = await _unitOfWork.SaveChangeAsync();
				if (res < 0)
				{
					throw new Exception("Can't delete the news article");
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while deleting the news article: {ex.Message}");
			}
		}

		public async Task<NewsArticle> UpdateNewsArticleAsync(int id, Delta<NewsArticle> delta)
		{
			try
			{
				var newsArticle = _unitOfWork.GenericRepository<NewsArticle>()
					.GetAll()
					.Include(s => s.Tags)
					.FirstOrDefault(s => s.NewsArticleId == id.ToString());

				if (newsArticle == null)
				{
					throw new KeyNotFoundException($"News article with ID {id} not found.");
				}

				var oldTagIds = newsArticle.Tags.Select(t => t.TagId).ToList();

				delta.Patch(newsArticle);

				if (newsArticle.Tags != null)
				{
					var newTagIds = newsArticle.Tags
						.Where(t => t.TagId != 0)
						.Select(t => t.TagId)
						.Distinct()
						.ToList();

					var validTags = await _unitOfWork.GenericRepository<Tag>()
						.GetAll()
						.Where(t => newTagIds.Contains(t.TagId))
						.ToListAsync();

					newsArticle.Tags.Clear();

					foreach (var tag in validTags)
					{
						newsArticle.Tags.Add(tag);
					}
				}
				else
				{
					newsArticle.Tags.Clear();
				}

				newsArticle.ModifiedDate = DateTime.Now;

				_unitOfWork.GenericRepository<NewsArticle>().Update(newsArticle);
				await _unitOfWork.SaveChangeAsync();

				return newsArticle;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating the news article: {ex.Message}", ex);
			}
		}
	}
}
