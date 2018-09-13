using Contextus.Core.Domain;
using MongoDB.Driver;
using PContextus.Core.Domain;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Domain.Models;
using PContextus.Core.Helpers;
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

            var result = new List<ArticleContent>();

            switch (status) {

                case Constants.Decision.PROACTIF:
                    var temp =await GetContentsBySegment(userProfile.Segments, businessRule);
                    result = temp.ToList();
                    break;

                case Constants.Decision.PREDICTIF:
                    var temp2= await GetContentsPredictive(userProfile, businessRule);
                    result = temp2.ToList();
                    break;

                case Constants.Decision.COGNITIF:
                    var temp3 = await GetRelevantContents(userProfile, businessRule);
                    result = temp3.ToList();
                    break;

                default:
                    return await GetDefaultRelevantContents(businessRule);
   
            }

            var filledContents = await FilledContents(result.Count(), businessRule);
            var response = result.ToList();
           response.AddRange(filledContents);

            return response;

        }

        /// <summary>
        /// Find
        /// </summary>
        /// <param name="requestFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleContent>> Find(ConditionFilter requestFilter)
        {
            var skip = 0;
            var take = 10;
            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();

            var filter = filterDefinition.Eq(x => x.ContentType, requestFilter.ContentType);

            if (!requestFilter.Brand.IsNullOrEmpty()) {               
                filter =filter & filterDefinition.Where(x => x.Brand.Contains(requestFilter.Brand));
            }
            if (!requestFilter.Category.IsNullOrEmpty()) {

                filter = filter & filterDefinition.Eq(x => x.Category.Parent, requestFilter.Category);
            }
            if (!requestFilter.Language.IsNullOrEmpty()) {

                filter = filter & filterDefinition.Eq(x => x.Market, requestFilter.Language);
            }

            skip = take * (requestFilter.Page - 1);


            return await _repository.FindAsync<ArticleContent>(filter, orderBy: x => x.GetOrderFor(requestFilter.Ordering),skip:skip,take:take);

        }

            /// <summary>
            /// Get Contents By Segment
            /// </summary>
            /// <param name="segmentedCode"></param>
            /// <returns></returns>
            public async Task<IEnumerable<ArticleContent>> GetContentsBySegment(IEnumerable<Segmentation> segments, BusinessRule businessRule, IEnumerable<string> exclude =null) {

            //Todo get all segmentCodes
            var segmentedCodes = segments.Select(x => x.SegmentedCode).ToList();
            var contentSegments = await _repository.GetAllAsync<SegmentedContent>(x=>x.SegmentedCode.Equals(segmentedCodes[0]));
            var contentIds= contentSegments.Select(x => x.ContentId);

            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();

            var filter=filterDefinition.In(x => x.ContentId, contentIds);

            if (businessRule != null) {

                filter = filter & filterDefinition.Eq(x => x.ContentType, businessRule.ContentType);
            }
            

            if (exclude != null) {

                filter = filter & filterDefinition.Nin(x => x.ContentId, exclude);

            }

            return await _repository.FindAsync<ArticleContent>(filter, orderBy: x => x.OrderByDescending(d => d.Title));
      
        }

        /// <summary>
        /// Contents Predictive
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        protected async Task<IEnumerable<ArticleContent>> GetContentsPredictive(UserProfile userProfile, BusinessRule businessRule) {

            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();
            FilterDefinition<ArticleContent> filter = null;
            if (!userProfile.FavoriteSection.IsNullOrEmpty()) {
                var categories = userProfile.FavoriteSection.Split("|").Select(x => x.ToLower());
                filter = filterDefinition.In(x => x.Category.Parent, categories);
                filter = filter | filterDefinition.In(x => x.Category.SubCategory, categories);
            }
            if (businessRule != null)
            {

                filter = filter & filterDefinition.Eq(x => x.ContentType, businessRule.ContentType);
            }
            if (!userProfile.TriedBrand.IsNullOrEmpty()) {
                var brands = userProfile.TriedBrand.Split("|").Select(x => x.ToLower());
                if (filter != null)
                {

                    filter = filter | filterDefinition.In(x => x.Brand, brands);
                }
                else {

                    filter =  filterDefinition.In(x => x.Brand, brands);
                }
                
            }
            
            return await _repository.FindAsync(filter, orderBy: x => x.GetOrderFor(businessRule.Default), skip: 0, take: businessRule.NumberOfContents);

         
        }
        /// <summary>
        /// Default relevamtContents
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<ArticleContent>> GetDefaultRelevantContents(BusinessRule businessRule) {

            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();

           var filter = filterDefinition.Eq(x => x.ContentType, businessRule.ContentType);

            return await _repository.FindAsync<ArticleContent>(filter,
                orderBy: x => x.GetOrderFor(businessRule.Default),
                skip:0,take:businessRule.NumberOfContents);

        }

        /// <summary>
        /// Get Relevant Content
        /// </summary>
        /// <returns></returns>
        protected async Task<IEnumerable<ArticleContent>> GetRelevantContents(UserProfile userProfile, BusinessRule businessRule) {

            var response = new List<ArticleContent>();

            var filterDefinition = new FilterDefinitionBuilder<ArticleContent>();
            FilterDefinition<ArticleContent> filter = null;
            if (businessRule != null)
            {

                filter = filter & filterDefinition.Eq(x => x.ContentType, businessRule.ContentType);
            }

            var relevantresult1= await _repository.FindAsync<ArticleContent>(filter,
                orderBy: x => x.GetOrderFor("Relevant"),
                skip: 0, take: businessRule.NumberOfContents-2);
            var tempNumber = businessRule.NumberOfContents;
            var br = businessRule;
            br.NumberOfContents = 1;
            var relevantSegment =await  GetContentsBySegment(userProfile.Segments , br);

            var relevantPredictive = await GetContentsPredictive(userProfile,br);

            response.AddRange(relevantresult1);
            response.AddRange(relevantSegment);
            response.AddRange(relevantPredictive);

            return response;
        }

        protected async Task<IEnumerable<ArticleContent>> FilledContents(int countResult, BusinessRule businessRule) {
            if (countResult < businessRule.NumberOfContents)
            {
                businessRule.NumberOfContents = businessRule.NumberOfContents - countResult;
                var contentsToFill = await GetDefaultRelevantContents(businessRule);
                return contentsToFill;
            }
            return new List<ArticleContent>();
        }
    }
}
