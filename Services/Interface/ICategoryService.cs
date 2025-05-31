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
	public interface ICategoryService
	{
		IEnumerable<Category> GetAllCategories();
		Task<Category> UpdateCategoryAsync(int id, Delta<Category> delta);
		Task<Category> CreateCategoryAsync(CreateCategoryRequest category);
		Task DeleteCategoryAsync(int id);
	}
}
