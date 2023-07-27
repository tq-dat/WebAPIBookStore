using WebAPIBookStore.Dto;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetUsers();
        ICollection<User> GetUsersByRole(string role);
        User GetUser(int userId);
        ICollection<User> GetUsersByName(string name);
        ICollection<CartItem> GetCartItemByUserId (int userId);
        ICollection<Order> GetOrdersByUserId(int userId);
        bool UserExists (UserLogin userLogin);
        bool UserExists(int userId);
        bool ManageExists(int manageId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int id);
        bool Save();
    }
}
