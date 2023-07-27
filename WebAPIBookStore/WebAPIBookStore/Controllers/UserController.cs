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
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsers);
            if (users.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(users) : BadRequest(ModelState);
        }

        [HttpGet("{role}")]
        public IActionResult GetUsersByRole(string role)
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsersByRole(role));
            if (users.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(users) : BadRequest(ModelState);
        }

        [HttpGet("{userId:int}")]
        public IActionResult GetUserByUserId(int userId) 
        {
            if (!_userRepository.UserExists(userId)) 
                return NotFound();

            var user = _mapper.Map<UserDto>(_userRepository.GetUser(userId));
            return ModelState.IsValid ? Ok(user) : BadRequest(ModelState);
        }
        [HttpGet("seachUser/{name}")]
        public IActionResult GetUsersByName(string name)
        {
            var users = _mapper.Map<List<UserDto>>(_userRepository.GetUsersByName(name));
            if (users.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(users) : BadRequest(ModelState);
        }

        [HttpGet("order/{userId}")]
        public IActionResult GetOrderByUserId(int userId)
        {
            if (!_userRepository.UserExists(userId))
                return NotFound();

            var orders = _mapper.Map<List<OrderDto>>(_userRepository.GetOrdersByUserId(userId));
            if (orders.Count() <= 0)
                return NotFound();

            return ModelState.IsValid ? Ok(orders) : BadRequest(ModelState);
        }

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
            if (!_userRepository.UserExists(id))
                return NotFound();

            if (!_userRepository.DeleteUser(id))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
