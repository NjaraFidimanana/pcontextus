using Contextus.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using PContextus.Core.Domain;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Helpers;
using PContextus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace pcontextus.Controllers
{

    [Route("api/[controller]/[action]")]
    public class SegmentController : Controller
    {
        private readonly ISegmentService _segmentService;

        private readonly IRepository _repository;

        private readonly IRecommendationContentService _recommendationContentService;

        public SegmentController(IRepository repository, IRecommendationContentService recommendationContentService)
        {
            _repository = repository;
            _recommendationContentService = recommendationContentService;
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> SaveSegment([FromBody] Segmentation segmentation)
        {
            try
            {
                if (!segmentation.SegmentedCode.IsNullOrEmpty()) {

                    if (segmentation == null) return Json(new { error = "Segmentation cannot be empty" });

                    await _repository.InsertAsync(segmentation);

                    return Json(new { status = "Inserted", statusCode = (int)HttpStatusCode.Created });
                }

                return Json(new { status = "Revert", statusCode = (int)HttpStatusCode.ExpectationFailed });

            }
            catch (Exception)
            {
                return Json(new { error = "An error occured" });
            }
        }

       

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> AssociatedContentSegment([FromBody] SegmentedContent segmentedContent) {

            try
            {
                if (!segmentedContent.ContentId.IsNullOrEmpty() && !segmentedContent.SegmentedCode.IsNullOrEmpty()) {
                    var contentsSegments = new List<SegmentedContent>();
                    var listOfSegments = segmentedContent.SegmentedCode.Split("|");

                    foreach (var segment in listOfSegments) {

                        var segmentContent = new SegmentedContent
                        {
                            ContentId = segmentedContent.ContentId,
                            SegmentedCode = segment
                        };

                        var filter = Builders<SegmentedContent>.Filter.Eq("ContentId", segmentContent.ContentId);
                        filter = filter & Builders<SegmentedContent>.Filter.Eq("SegmentedCode", segmentContent.SegmentedCode);
                        var updateDef = Builders<SegmentedContent>.Update.Set("SegmentedCode", segmentContent.SegmentedCode);
                        await _repository.UpdateOneAsync(segmentContent, updateDef, filter);


                    }
                    return Json(new { status = "Contents are segmented", statusCode = (int)HttpStatusCode.Created });
                }
            }
            catch (Exception ex) {
                return Json(new { error = "An error occured" });
            }

            return Json(new { status = "Revert", statusCode = (int)HttpStatusCode.ExpectationFailed });
        }

        [HttpGet]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> Find()
        {
            try
            {
               var data= await _repository.GetAllAsync<Segmentation>();

                return Json(new { status = "Find",
                    statusCode = (int)HttpStatusCode.Accepted,
                    data
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occured" });

            }
        }

        [HttpGet("{code}")]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> Delete(string code)
        {
            try
            {
                await _repository.DeleteManyAsync<Segmentation>(x => x.SegmentedCode.Equals(code));

                return Json(new
                {
                    status = "Deleted",
                    statusCode = (int)HttpStatusCode.OK
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occured" });

            }
        }

        [HttpGet("{code}")]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> ContentsBySegment(string code) {

            try
            {
                var segments = new List<Segmentation> {
                    new Segmentation{
                        SegmentedCode=code
                    }
                };
                var contents = await  _recommendationContentService.GetContentsBySegment(segments, null);
              
                return Json(new
                {
                    status = "Find",
                    statusCode = (int)HttpStatusCode.OK,
                    contents
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occured" });

            }
        }
    }
}
