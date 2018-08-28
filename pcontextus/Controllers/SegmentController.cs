using Contextus.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public SegmentController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
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

        [HttpGet]
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
    }
}
