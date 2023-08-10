using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Models;
using WebAPIBookStore.UseCase;

namespace WebAPIBookStore.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserUseCase _userUseCase;
        public UserController(UserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet("Role")]
        public IActionResult GetUsersByRole([FromQuery] string role)
        {
            var output = _userUseCase.GetByRole(role);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByUserId([FromRoute] int id)
        {

            var output = _userUseCase.GetById(id);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpGet("SearchUser")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            var output = _userUseCase.GetByName(name);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _userUseCase.Login(userLogin);
            return output.Error != StatusCodeAPI.NotFound ? Ok(output) : NotFound(output);
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] UserDto userCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _userUseCase.SignUp(userCreate);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.UnprocessableEntity:
                        return StatusCode(422, output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserDto userUpdate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var output = _userUseCase.Put(userUpdate);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var output = _userUseCase.Delete(id);
            if (!output.Success)
            {
                switch (output.Error)
                {
                    case StatusCodeAPI.NotFound:
                        return NotFound(output);

                    case StatusCodeAPI.InternalServer:
                        return BadRequest(output);
                }
            }

            return Ok(output);
        }
    }
}
