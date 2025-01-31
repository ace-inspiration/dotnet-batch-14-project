using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.BookingListByUserId;

namespace TravelAgency.Domain.Features.UserLists
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserListController : ControllerBase
    {
        private readonly UserListService _service;
        public UserListController(UserListService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Userlist()
        {
            var response = await _service.GetUserList();

            return response.Success ? Ok(response) : StatusCode(500, response);
        }
    }
}
