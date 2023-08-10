using WebAPIBookStore.Dto;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IUserRepository
    {
        public ICollection<User> GetUsers();
        public ICollection<User> GetUsersByRole(Role role);
        public User? GetUser(int userId);
        public ICollection<User> GetUsersByName(string name);
        public bool UserExists (UserLogin userLogin);
        public bool UserExists(int userId);
        public bool ManageExists(int manageId);
        public bool CreateUser(User user);
        public bool UpdateUser(User user);
        public bool DeleteUser(User deleteUser);
        public bool Save();
    }
}
