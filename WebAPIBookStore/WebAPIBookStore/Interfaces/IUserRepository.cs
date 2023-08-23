using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IUserRepository
    {
        public ICollection<User> GetUsers();
        public ICollection<User> GetUsersByRole(Role role);
        public User? GetUser(int userId);
        public ICollection<User> GetUsersByName(string name);
        public User? GetUserByEmail(string email);
        public bool UserExists (LoginInput loginInput);
        public bool UserExists(int userId);
        public bool ManageExists(int manageId);
        public bool CreateUser(User user);
        public bool UpdateUser(User userUpdate, User userInput);
        public bool DeleteUser(User deleteUser);
        public string SendEmailOtp (string address);
        public bool Save();
    }
}
