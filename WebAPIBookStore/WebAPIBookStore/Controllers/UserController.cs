using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(
            IUserRepository userRepository, 
            IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("Role")]
        public IActionResult GetUsersByRole([FromQuery] Role role)
        {
            var users = _userRepository.GetUsersByRole(role);
            if (users.Count <= 0)
                return NotFound();

            var userMaps = _mapper.Map<List<UserDto>>(users);
            if (userMaps.Count <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(userMaps) : BadRequest(ModelState);
        }

        [HttpGet("{id}")]
        public IActionResult GetUserByUserId([FromRoute] int id) 
        {
            var user = _userRepository.GetUser(id);
            if (user == null) 
                return NotFound();

            var userMap = _mapper.Map<UserDto>(user);
            return ModelState.IsValid ? Ok(userMap) : BadRequest(ModelState);
        }

        [HttpGet("SearchUser")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            var users = _userRepository.GetUsersByName(name);
            if (users.Count <= 0)
                return NotFound();

            var userMaps = _mapper.Map<List<UserDto>>(users);
            return ModelState.IsValid ? Ok(userMaps) : BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return _userRepository.UserExists(userLogin) ? Ok(userLogin) : NotFound(ModelState);      
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] UserDto userCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers().FirstOrDefault(c => c.UserName.Trim().ToUpper() == userCreate.UserName.Trim().ToUpper() || c.Email == userCreate.Email);
            if (user != null)
            {
                ModelState.AddModelError("", "Username or email already exists");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(userCreate);
            return _userRepository.CreateUser(userMap) ? Ok(userMap) : BadRequest(ModelState);
        }

        [HttpPut]
        public IActionResult UpdateUser([FromBody] UserDto userUpdate) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_userRepository.UserExists(userUpdate.Id))
                return NotFound();

            var userMap = _mapper.Map<User>(userUpdate);
            return _userRepository.UpdateUser(userMap) ? Ok(userMap) : BadRequest(ModelState);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser([FromRoute] int id)
        {
            var deleteUser = _userRepository.GetUser(id);
            if (deleteUser == null)
                return NotFound();

            return _userRepository.DeleteUser(deleteUser) ? Ok(deleteUser) : BadRequest(ModelState);
        }
    }
}
