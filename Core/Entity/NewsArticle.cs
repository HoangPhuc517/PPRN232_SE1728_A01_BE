﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Repositories.Entity;

public partial class NewsArticle
{
    public string NewsArticleId { get; set; } = null!;

    public string? NewsTitle { get; set; }

    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

	public string? NewsContent { get; set; }

    public string? NewsSource { get; set; }

    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; } = true;

    public short? CreatedById { get; set; } 

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    [JsonIgnore]
    public virtual Category? Category { get; set; }

	[JsonIgnore]
	public virtual SystemAccount? CreatedBy { get; set; }

	public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
