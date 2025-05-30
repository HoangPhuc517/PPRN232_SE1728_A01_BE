using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Repositories.Entity;

namespace Infrastructure
{
    public static class ConfigOData
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<NewsArticle>("NewsArticles")
                   .EntityType
                   .HasKey(_ => _.NewsArticleId);
            
            builder.EntitySet<Category>("Categories")
                   .EntityType
                   .HasKey(_ => _.CategoryId);

            builder.EntitySet<Tag>("Tags")
                   .EntityType
                   .HasKey(_ => _.TagId);

            builder.EntitySet<SystemAccount>("SystemAccounts")
                   .EntityType
                   .HasKey(_ => _.AccountId);
            return builder.GetEdmModel();
        }
    }
}
