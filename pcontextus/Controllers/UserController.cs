using Contextus.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using PContextus.Core.Domain.Entities;
using PContextus.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace pcontextus.Controllers
{

    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {

        private readonly IUserService _userService;

        private readonly IRepository _repository;
        public UserController(IUserService userService, IRepository repository) {

            _userService = userService;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser(string urn)
        {
            var currentUser =await  _userService.GetCurrentUserAsync(urn);

            return Json(new
            {
                data = currentUser,
            });
 
        }

        [HttpGet]
        public async Task<IActionResult> FindAll() {

            var users = await _userService.GetUsers();
            var count = users?.ToList().Count;
            return Json(new
            {
                data = users,
                count
            });
        }


        [HttpPost]
        public async Task<IActionResult> SaveCustomer([FromBody] UserProfile userProfile) {

            try
            {
                if (userProfile == null) return Json(new { error = "userProfile cannot be empty" });

                await _repository.InsertAsync(userProfile);

                return Json(new { status = "Inserted", statusCode = (int)HttpStatusCode.Created });
            }
            catch(Exception ex) {
                return Json(new { error = "An error occured" });
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateUserSegment(string urn, string segmentedValue ) {

            try
            {
                if (segmentedValue == null) return Json(new { error = "Cannot update user segment" });

                await _userService.UpdateCustomerSegment(urn, segmentedValue);

                return Json(new { status = "Updated", statusCode = (int)HttpStatusCode.Accepted });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occured" });
            }
        }

    }
}
