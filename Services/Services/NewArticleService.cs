using AutoMapper;
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
										   .Select(n => n.NewsArticleId)
										   .OrderByDescending(id => id)
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
				var newsArticle = await _unitOfWork.GenericRepository<NewsArticle>().GetByIdAsync(id.ToString());

				if (newsArticle == null)
				{
					throw new Exception($"News article with ID {id} not found.");
				}

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
				var newsArticle = await _unitOfWork.GenericRepository<NewsArticle>().GetByIdAsync(id.ToString());

				if (newsArticle == null)
				{
					throw new KeyNotFoundException($"News article with ID {id} not found.");
				}

				delta.Patch(newsArticle);
				_unitOfWork.GenericRepository<NewsArticle>().Update(newsArticle);
				await _unitOfWork.SaveChangeAsync();

				return newsArticle;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while updating the news article: {ex.Message}");
			}
		}
	}
}
