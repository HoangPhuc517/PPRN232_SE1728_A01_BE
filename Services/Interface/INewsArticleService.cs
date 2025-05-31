using Microsoft.AspNetCore.OData.Deltas;
using Repositories.Entity;
using Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
	public interface INewsArticleService
	{
		IEnumerable<NewsArticle> GetAllNewsArticles();
		Task<NewsArticle> CreateNewArticleAsync(CreateNewArticleRequest createNewArticle);
		Task DeleteNewsArticleAsync(int id);
		Task<NewsArticle> UpdateNewsArticleAsync(int id, Delta<NewsArticle> delta);
	}
}
