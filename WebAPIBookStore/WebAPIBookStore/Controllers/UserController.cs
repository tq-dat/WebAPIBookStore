using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Repository;

namespace WebAPIBookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers;
            if (users == null)
                return NotFound();

            var userDtos = _mapper.Map<List<UserDto>>(users);
            return ModelState.IsValid ? Ok(users) : BadRequest(ModelState);
        }

        [HttpGet("{role}")]
        public IActionResult GetUsersByRole([FromQuery] string role)
        {
            var users = _userRepository.GetUsersByRole(role);
            if (users == null)
                return NotFound();

            var userMaps = _mapper.Map<List<UserDto>>(users);
            if (userMaps.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(userMaps) : BadRequest(ModelState);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUserByUserId([FromQuery] int id) 
        {
            var user = _userRepository.GetUser(id);
            if (user == null) 
                return NotFound();

            var userMap = _mapper.Map<UserDto>(user);
            return ModelState.IsValid ? Ok(userMap) : BadRequest(ModelState);
        }
        [HttpGet("seachUser/{name}")]
        public IActionResult GetUsersByName([FromQuery] string name)
        {
            var users = _userRepository.GetUsersByName(name);
            if (users.Count() <= 0)
                return NotFound();

            var userMaps = _mapper.Map<List<UserDto>>(users);
            return ModelState.IsValid ? Ok(userMaps) : BadRequest(ModelState);
        }
        // dang fix (chuyen sang order va tra ve dung Order
        //[HttpGet("order/{userId}")]
        //public IActionResult GetOrderByUserId(int userId)
        //{
        //    if (!_userRepository.UserExists(userId))
        //        return NotFound();

        //    var orders = _mapper.Map<List<OrderDto>>(_userRepository.GetOrdersByUserId(userId));
        //    if (orders.Count() <= 0)
        //        return NotFound();

        //    return ModelState.IsValid ? Ok(orders) : BadRequest(ModelState);
        //}

        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return !_userRepository.UserExists(userLogin) ? Ok("Login success") : NotFound(ModelState);      
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp(UserDto userCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = _userRepository.GetUsers().FirstOrDefault(c => c.UserName.Trim().ToUpper() == userCreate.UserName.Trim().ToUpper());
            if (user != null)
            {
                ModelState.AddModelError("", "User already exists");
                return StatusCode(422, ModelState);
            }

            var userMap = _mapper.Map<User>(userCreate);
            if (!_userRepository.CreateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut]
        public IActionResult updateUser(UserDto userUpdate) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMap = _mapper.Map<User>(userUpdate);
            if (!_userRepository.UpdateUser(userMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var deleteUser = _userRepository.GetUser(id);
            if (deleteUser == null)
                return NotFound();

            if (!_userRepository.DeleteUser(deleteUser))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
