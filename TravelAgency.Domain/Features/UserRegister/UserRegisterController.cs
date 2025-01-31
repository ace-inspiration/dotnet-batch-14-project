using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.UserRegister
{
    [Route("api/user")]
    [ApiController]
    public class UserRegisterController : ControllerBase
    {
        private  UserRegisterService _userRegisterService;

        public UserRegisterController(UserRegisterService userRegisterService)
        {
            _userRegisterService = userRegisterService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterRequestModel requestModel)
        {
           
            try
            {
                var model = await _userRegisterService.UserRegister(requestModel);
                if (!model.IsSuccess)
                {
                    return BadRequest(model);
                }
                return Ok(model);

            }
            catch (Exception ex)
            {

                return BadRequest(new UserRegisterResponseModel()
                {
                    //Message = ex.ToString()
                    Message = ex.Message

                });
            }
        }
    }
}
