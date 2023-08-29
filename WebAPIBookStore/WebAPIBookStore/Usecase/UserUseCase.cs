using AutoMapper;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;
using WebAPIBookStore.Result;
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

            return _useCaseOutput.Success(_mapper.Map<List<UserDto>>(users));
        }

        public Output Login(LoginInput loginInput)
        {
            var user = _userRepository.GetUserByEmail(loginInput.Email);
            if (user == null)
                return _useCaseOutput.NotFound("Email not true");

            if (loginInput.Password != user.Password)
                return _useCaseOutput.NotFound("Password not true");

            return _useCaseOutput.Success(_mapper.Map<UserOutput>(user));
        }

        public Output SignUp(SignUpInput signUpInput)
        {
            var user = _userRepository.GetUsers().FirstOrDefault(c => c.UserName.Trim().ToUpper() == signUpInput.UserName.Trim().ToUpper() || c.Email == signUpInput.Email);
            if (user != null)
                return _useCaseOutput.UnprocessableEntity("User already exists");

            var userMap = _mapper.Map<User>(signUpInput);
            userMap.Role = Role.User;
            userMap.EmailVerify = false;
            if (!_userRepository.CreateUser(userMap))
                return _useCaseOutput.InternalServer("Something went wrong while saving");

            return _useCaseOutput.Success(_mapper.Map<UserOutput>(userMap));
        }

        public Output SendOTP(string email)
        {
            var otp = _userRepository.SendEmailOtp(email);
            if (otp == null)
                return _useCaseOutput.InternalServer("Error");

            return _useCaseOutput.Success(otp);
        }

        public Output Put(UpdateUserInput updateUserInput)
        {
            var user = _userRepository.GetUser(updateUserInput.Id);
            if (user == null)
            {
                return _useCaseOutput.NotFound("Not found user");
            }
            var userMap = _mapper.Map<User>(updateUserInput);
            if (!_userRepository.UpdateUser(user, userMap))
            {
                return _useCaseOutput.InternalServer("Something went wrong while saving");
            }

            return _useCaseOutput.Success(_mapper.Map<UserOutput>(userMap));
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
