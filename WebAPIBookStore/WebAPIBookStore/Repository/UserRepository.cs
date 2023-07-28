using WebAPIBookStore.Data;
using WebAPIBookStore.Dto;
using WebAPIBookStore.Interfaces;
using WebAPIBookStore.Models;

namespace WebAPIBookStore.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context) 
        {
            _context = context;
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User deleteUser)
        {
            var deleteCartItemIds = _context.CartItems.Where(P => P.UserId == deleteUser.Id && P.Status == "UnPaid").Select(p =>p.Id).ToList();
            foreach (var cartItemId in deleteCartItemIds)
            {
                var cartItem = _context.CartItems.FirstOrDefault(p => p.Id == cartItemId);
                if  (cartItem != null)
                {
                    _context.Remove(cartItem);
                }
            }

            deleteUser.Role = "UserDeleted";
            _context.Update(deleteUser);    
            return Save();
        }

        public User? GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public ICollection<User> GetUsersByName(string name)
        {
            return _context.Users.Where(p => p.UserName.Contains(name)).ToList();
        }

        public ICollection<User> GetUsersByRole(string role)
        {
            return _context.Users.Where(u => u.Role == role).ToList();
        }

        public bool ManageExists(int manageId)
        {
            return _context.Users.Any(p => p.Id == manageId && (p.Role == "admin" || p.Role == "manage"));
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(p => p.Id == userId);
        }

        public bool UserExists(UserLogin userLogin)
        {
            return _context.Users.Any(p => p.UserName == userLogin.UserName && p.Password == userLogin.Password && p.Role == userLogin.Role);
        }
    }
}
