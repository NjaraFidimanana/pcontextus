using Contextus.Core.Domain;
using MongoDB.Driver;
using PContextus.Core.Domain;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Domain.Models;
using PContextus.Core.Interfaces;
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

        private readonly IUserService _userService;

        private readonly IBusinessRule _businessRules;

        public RecommendationContentService(IRepository repository, IUserService userService, IBusinessRule businessRules)
        {
            _repository = repository;

            _userService = userService;

            _businessRules = businessRules;
        }

        public async Task<IEnumerable<ArticleContent>> PerformContentFilteringAsync(ConditionFilter requestFilter)
        {

            var status = "Default";

            UserProfile userProfile = null;
            if (requestFilter != null) {

                 userProfile = await _userService.GetCurrentUserAsync(requestFilter.UserId);
                if (userProfile != null)
                {
                    status = userProfile.Status;
                }
            }
            BusinessRule businessRule = await _businessRules.GetDefinedRule(requestFilter.ContextId);

            switch (status) {

                case Constants.Decision.PROACTIF:
                    return await GetContentsBySegment(userProfile.Segments);

                case Constants.Decision.PREDICTIF:
                    break;

                case Constants.Decision.COGNITIF:
                    break;

                default:
                    return await GetDefaultRelevantContents(businessRule);
   
            }

            return new List<ArticleContent>();

        }

        /// <summary>
        /// Get Contents By Segment
        /// </summary>
        /// <param name="segmentedCode"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<ArticleContent>> GetContentsBySegment(IEnumerable<Segmentation> segments) {

            //Todo get all segmentCodes
            var segmentedCodes = segments.Select(x => x.SegmentedCode).ToList();
            var contentSegments = await _repository.GetAllAsync<SegmentedContent>(x=>x.SegmentedCode.Equals(segmentedCodes[0]));
            var contentIds= contentSegments.Select(x => x.ContentId);

            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();

            var filter=filterDefinition.In(x => x.ContentId, contentIds);

            return await _repository.FindAsync<ArticleContent>(filter, orderBy: x => x.OrderByDescending(d => d.Title));

        }

        /// <summary>
        /// Default relevamtContents
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<ArticleContent>> GetDefaultRelevantContents(BusinessRule businessRule) {

            return await _repository.GetAllAsync<ArticleContent>(
                orderBy: x => x.OrderByDescending(d => d.Title),
                skip:0,take:businessRule.NumberOfContents);
        }
    }
}
