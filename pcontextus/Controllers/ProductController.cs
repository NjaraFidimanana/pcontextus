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
    public class ProductController : Controller
    {
        private readonly IRecommendationContentService _recommendationContentService;

        public ProductController(IRecommendationContentService recommendationContentService)
        {

            _recommendationContentService = recommendationContentService;

        }


        [HttpGet("context/{cxt}/identify/{id}")]
        public async Task<IActionResult> Contents(int cxt, string id)
        {
            try
            {

                var conditionFilter = new ConditionFilter
                {
                    UserId = id,
                    ContextId = cxt,
                    ContentType="Product"
                };
                var contentReturned = await _recommendationContentService.PerformContentFilteringAsync(conditionFilter);

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
