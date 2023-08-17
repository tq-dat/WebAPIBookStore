using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Usecase;

namespace WebAPIBookStore.UseCase
{
    public class UserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUseCaseOutput _useCaseOutput;

        public UserUseCase(
            IUserRepository userRepository, 
            IMapper mapper,
            IUseCaseOutput useCaseOutput)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _useCaseOutput = useCaseOutput;
        }

        public Output GetByRole(Role role)
        {
            var users = _userRepository.GetUsersByRole(role);
            if (!users.Any())
            {
                return _useCaseOutput.NotFound("Not found users");
            }

            return _useCaseOutput.Success(_mapper.Map<List<UserDto>>(users));          
        }

        public Output GetById(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user == null)
            {
                return _useCaseOutput.NotFound("Not found user");
            }

            return _useCaseOutput.Success(_mapper.Map<UserDto>(user));
        }

        public Output GetByName(string name) 
        {
            var users = _userRepository.GetUsersByName(name);
            if (!users.Any())
            {
                return _useCaseOutput.NotFound("Not found user");
            }

            return _useCaseOutput.Success(_mapper.Map<UserDto>(users));
        }

        public Output Login(UserLogin userLogin)
        {
            if (!_userRepository.UserExists(userLogin))
            {
                return _useCaseOutput.NotFound("Not found user");
            }

            return _useCaseOutput.Success(userLogin);
        }

        public Output SignUp(UserDto userDto)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(c => c.UserName.Trim().ToUpper() == userDto.UserName.Trim().ToUpper() || c.Email == userDto.Email);
            if (user != null)
            {
                return _useCaseOutput.UnprocessableEntity("User already exists");
            }

            var userMap = _mapper.Map<User>(userDto);
            if (!_userRepository.CreateUser(userMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<UserDto>(userMap));
        }

        public Output Put(UserDto userInput)
        {
            var user = _userRepository.GetUser(userInput.Id);
            if (user == null)
            {
                return _useCaseOutput.NotFound("Not found user");
            }
            var userMap = _mapper.Map<User>(userInput);
            if (!_userRepository.UpdateUser(user, userMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<UserDto>(userMap));
        }

        public Output Delete(int id)
        {
            var deleteUser = _userRepository.GetUser(id);
            if (deleteUser == null)
            {
                return _useCaseOutput.NotFound("Not found user");
            }

            if (!_userRepository.DeleteUser(deleteUser))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<UserDto>(deleteUser));
        }
    }
}
