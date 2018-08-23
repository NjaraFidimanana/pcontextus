using PContextus.Core.Domain.Entities;
using PContextus.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Domain
{
    public interface IRecommendationContentService
    {
        Task<IEnumerable<ArticleContent>> PerformContentFilteringAsync(ConditionFilter requestFilter);
    }
}
