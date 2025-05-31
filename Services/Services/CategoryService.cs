using Microsoft.AspNetCore.OData.Deltas;
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
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;

		public CategoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<Category> GetAllCategories()
		{
			return _unitOfWork.GenericRepository<Category>().GetAll();
		}

		public async Task DeleteCategoryAsync(int id)
		{
			try
			{
				var category = await _unitOfWork.GenericRepository<Category>().GetByIdAsync((short)id);

				if (category == null)
				{
					throw new Exception($"Category with ID {id} not found.");
				}

				 _unitOfWork.GenericRepository<Category>().Delete(category);
				var res = await _unitOfWork.SaveChangeAsync();
				if (res < 0)
				{
					throw new Exception("Can't delete the category");
				}
			}
			catch(Exception ex)
			{
				throw new Exception($"An error occurred while retrieving the category: {ex.Message}");
			}
        }

		public async Task<Category> UpdateCategoryAsync(int id, Delta<Category> delta)
		{

			try
			{
				var category = await _unitOfWork.GenericRepository<Category>().GetByIdAsync((short)id);

				if (category == null)
				{
					throw new KeyNotFoundException($"Category with ID {id} not found.");
				}

				//if (delta.GetChangedPropertyNames().Contains("Id"))
				//{
				//	throw new Exception("Cannot update the Id property.");
				//}

				var changedProperties = delta.GetChangedPropertyNames();
				if (changedProperties.Contains("Id"))
				{
					throw new InvalidOperationException("Updating the Id property is not allowed.");
				}


				delta.Patch(category);

				await _unitOfWork.SaveChangeAsync();

				return category;

			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while update the category: {ex.Message}");
			}
		}

		public async Task<Category> CreateCategoryAsync(CreateCategoryRequest category)
		{
			try
			{
				var newCategory = new Category
				{
					CategoryName = category.CategoryName,
					CategoryDesciption = category.CategoryDesciption,
					ParentCategoryId = category.ParentCategoryId ?? null,
				};

				await _unitOfWork.GenericRepository<Category>().InsertAsync(newCategory);
				  await _unitOfWork.SaveChangeAsync();
               
                return newCategory;
			}
			catch (Exception ex)
			{
				throw new Exception($"An error occurred while creating the category: {ex.Message}");
			}
		}
	}
}
