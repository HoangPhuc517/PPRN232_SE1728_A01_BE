using Repositories.Entity;

namespace Services.Interface
{
	public interface ITagService
	{
		IEnumerable<Tag> GetListTags();
	}
}
