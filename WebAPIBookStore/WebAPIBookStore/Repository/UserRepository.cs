using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using WebAPIBookStore.Data;
using WebAPIBookStore.Enum;
using WebAPIBookStore.Input;
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
            var deleteCartItemIds = _context.CartItems.Where(p => p.UserId == deleteUser.Id && p.Status == CartItemStatus.UnPaid).Select(p =>p.Id).ToList();
            foreach (var cartItemId in deleteCartItemIds)
            {
                var cartItem = _context.CartItems.FirstOrDefault(p => p.Id == cartItemId);
                if  (cartItem != null)
                {
                    _context.Remove(cartItem);
                }
            }

            deleteUser.UserName = "Deleted";
            deleteUser.Role = Role.DeletedUser;
            _context.Update(deleteUser);    
            return Save();
        }

        public User? GetUser(int userId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public ICollection<User> GetUsersByName(string name)
        {
            return _context.Users.Where(p => p.UserName.Contains(name)).ToList();
        }

        public ICollection<User> GetUsersByRole(Role role)
        {
            return _context.Users.Where(u => u.Role == role).ToList();
        }

        public bool ManageExists(int manageId)
        {
            return _context.Users.Any(p => p.Id == manageId && (p.Role == Role.Admin));
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            if (saved > 0)
                return true;
            
            return false;
        }

        public string SendEmailOtp(string address)
        {
            Random rd = new Random();
            var OTP = rd.Next(1000, 9999).ToString();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("trinhdat11371@gmail.com"));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = "Web API";
            email.Body = new TextPart(TextFormat.Text) { Text = "Mã xác thực của bạn là : " + OTP };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("trinhdat11371@gmail.com", "qehtkqrfpeuxgamz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return OTP;
        }

        public bool UpdateUser(User userUpdate, User userInput)
        {
            userUpdate.UserName = userInput.UserName;
            userUpdate.Role = userInput.Role;
            userUpdate.Address = userInput.Address;
            userUpdate.Password = userInput.Password;
            userUpdate.EmailVerify = userInput.EmailVerify;
            _context.Update(userUpdate);
            return Save();
        }

        public bool UserExists(int userId)
        {
            return _context.Users.Any(p => p.Id == userId);
        }

        public bool UserExists(LoginInput loginInput)
        {
            return _context.Users.Any(p => p.UserName == loginInput.Email && p.Password == loginInput.Password);
        }
    }
}
