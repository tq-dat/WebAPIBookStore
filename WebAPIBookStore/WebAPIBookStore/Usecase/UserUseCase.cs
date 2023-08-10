using AutoMapper;
using WebAPIBookStore.Consts;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.UseCase
{
    public class UserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserUseCase(
            IUserRepository userRepository, 
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Output GetByRole(string role)
        {
            var output = new Output();
            var users = _userRepository.GetUsersByRole(role);
            if (!users.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found users";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(users);
            return output;            
        }

        public Output GetById(int id)
        {
            var output = new Output();
            var user = _userRepository.GetUser(id);
            if (user == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found user";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(user);
            return output;
        }

        public Output GetByName(string name) 
        {
            var output = new Output();
            var users = _userRepository.GetUsersByName(name);
            if (!users.Any())
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found users";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(users);
            return output;
        }

        public Output Login(UserLogin userLogin)
        {
            var output = new Output();
            if (!_userRepository.UserExists(userLogin))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found user";
                return output;
            }

            output.Success = true;
            output.Data = userLogin;
            return output;
        }

        public Output SignUp(UserDto userDto)
        {
            var output = new Output();
            var user = _userRepository.GetUsers().FirstOrDefault(c => c.UserName.Trim().ToUpper() == userDto.UserName.Trim().ToUpper() || c.Email == userDto.Email);
            if (user != null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.UnprocessableEntity;
                output.Message = "User already exists";
                return output;
            }

            var userMap = _mapper.Map<User>(userDto);
            if (!_userRepository.CreateUser(userMap))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(userMap);
            return output;
        }

        public Output Put(UserDto userUpdate)
        {
            var output = new Output();
            var user = _userRepository.GetUser(userUpdate.Id);
            if (user == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found user";
                return output;
            }
            var userMap = _mapper.Map<User>(userUpdate);
            if (!_userRepository.UpdateUser(userMap))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(userMap);
            return output;
        }

        public Output Delete(int id)
        {
            var output = new Output();
            var deleteUser = _userRepository.GetUser(id);
            if (deleteUser == null)
            {
                output.Success = false;
                output.Error = StatusCodeAPI.NotFound;
                output.Message = "Not found user";
                return output;
            }

            if (!_userRepository.DeleteUser(deleteUser))
            {
                output.Success = false;
                output.Error = StatusCodeAPI.InternalServer;
                output.Message = "Something went wrong while saving";
                return output;
            }

            output.Success = true;
            output.Data = _mapper.Map<UserDto>(deleteUser);
            return output;
        }
    }
}
