using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Repositories.Entity;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; }

    public int? AccountRole { get; set; }
    [JsonIgnore]
    public string? AccountPassword { get; set; }
    [JsonIgnore]
    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
