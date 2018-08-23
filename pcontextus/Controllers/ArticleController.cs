using Microsoft.AspNetCore.Mvc;
using PContextus.Core.Domain;
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

        // GET api/article/language/context/tag
        [HttpGet("{language}/identify/{id}/context/{cxt}/{tag}")]
        public async Task<IActionResult> Contents(string language, string id,int cxt,int tag)
        {
            try {
               var contentReturned= await _recommendationContentService.PerformContentFilteringAsync(null);

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
