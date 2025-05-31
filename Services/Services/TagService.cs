using Repositories.Entity;
using Repositories.Interface;
using Services.Interface;

namespace Services.Services
{
	public class TagService : ITagService
	{
		private readonly IUnitOfWork _unitOfWork;

		public TagService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IEnumerable<Tag> GetListTags()
		{
			try
			{
				return _unitOfWork.GenericRepository<Tag>().GetAll();
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while retrieving news articles.", ex);
			}
		}
	}
}
