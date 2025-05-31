using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
	public class CreateNewArticleRequest
	{
		public string? NewsTitle { get; set; }
		public string Headline { get; set; } = null!;
		public string? NewsContent { get; set; }
		public string? NewsSource { get; set; }
		public short? CategoryId { get; set; }
		public List<int> TagIds { get; set; } = new List<int>();
	}
}
