using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Repositories.Entity;
using Services.DTOs;

namespace Services.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<CreateCategoryRequest, Category>().ReverseMap();
			CreateMap<CreateNewArticleRequest, NewsArticle>().ReverseMap();
		}
	}
}
