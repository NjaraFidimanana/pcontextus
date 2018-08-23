using Contextus.Core.Domain;
using PContextus.Core.Domain;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PContextus.Core.Services
{
    public class RecommendationContentService : IRecommendationContentService
    {

        private readonly IRepository _repository;


        public RecommendationContentService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ArticleContent>> PerformContentFilteringAsync(ConditionFilter requestFilter)
        {
            if (requestFilter == null) {
                
                return await _repository.GetAllAsync<ArticleContent>(orderBy: x => x.OrderByDescending(d => d.Title));
            }

            return null;
           
            
        }
    }
}
