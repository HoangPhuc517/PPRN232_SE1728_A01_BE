using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services.Interface;
using Services.Services;

namespace PPRN232_SE1728_A01_BE.Controllers
{
	public class TagsController : ODataController
	{
		private readonly ITagService _tagService;

		public TagsController(ITagService tagService)
		{
			_tagService = tagService;
		}

		[EnableQuery]
		public IActionResult Get()
		{
			try
			{
				var tags = _tagService.GetListTags();
				return Ok(tags);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}
	}
}
