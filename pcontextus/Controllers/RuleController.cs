using Contextus.Core.Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PContextus.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace pcontextus.Controllers
{

    [Route("api/[controller]/[action]")]
    public class RuleController : Controller
    {

        private readonly IRepository _repository;

        public RuleController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> SaveRule([FromBody] BusinessRule businessRule)
        {
            try
            {
                if (businessRule.ContextId!=0)
                {

                    if (businessRule == null) return Json(new { error = "Business Rule cannot be empty" });

                    await _repository.InsertAsync(businessRule);

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
        [EnableCors("AllowSpecificOrigin")]
        public async Task<IActionResult> FindAll() {
            try
            {
                var data = await _repository.GetAllAsync<BusinessRule>();

                return Json(new
                {
                    status = "OK",
                    statusCode = (int)HttpStatusCode.Accepted,
                    rules=data
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occured" });

            }
        }
    }
}
