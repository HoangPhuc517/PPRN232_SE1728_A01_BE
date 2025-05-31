using Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
	public interface INewsArticleRepository
	{
		IEnumerable<NewsArticle> GetAllNewsArticles();

		Task<NewsArticle> CreateNewArticle();
	}
}
