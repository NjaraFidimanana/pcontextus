using Microsoft.AspNetCore.Mvc;
using PContextus.Core.Domain;
using PContextus.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace pcontextus.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ArticleController : Controller
    {

        private readonly IRecommendationContentService _recommendationContentService;

        public ArticleController(IRecommendationContentService recommendationContentService)
        {

            _recommendationContentService = recommendationContentService;

        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value3", "value4" };
        }

        [HttpGet("{language}")]
        public IEnumerable<string> GetContents(string language)
        {
            return new string[] { language, "value2" };
        }

        // GET api/article/context
        [HttpGet("context/{cxt}/identify/{id}")]
        public async Task<IActionResult> Contents(int cxt ,string id)
        {
            try {

                var conditionFilter = new ConditionFilter
                {
                    UserId=id,
                    ContextId=cxt
                };
               var contentReturned= await _recommendationContentService.PerformContentFilteringAsync(conditionFilter);

                return Json(new
                {
                    data = contentReturned,
                    totalResult = contentReturned.Count()
                });

            }
            catch (Exception ex)
            {

                return Json(new { error = "An error occured" });
            }
        }   
    }
}
