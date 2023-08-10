using System.Security.Principal;
using TestCase.Data;
using TestCase.Interface;
using TestCase.Models;

namespace TestCase.Service
{
    public class UserService : IUser
    {
        private readonly TesCaseContext _context;

        public UserService(TesCaseContext context)
        {
            _context = context;
        }

        public DateTime UpdatedAt { get; private set; }

        public User GetAccountByCreatedAt(DateTime createdAt)
        {
            return _context.Users.Where(a => a.CreatedAt == createdAt).FirstOrDefault()!;

        }

        public User GetAccountByEmail(string email)
        {
            return _context.Users.Where(a => a.Email == email).FirstOrDefault()!;

        }

        public User GetAccountByPhoneNumber(string phoneNumber)
        {
            return _context.Users.Where(a => a.PhoneNumber == phoneNumber).FirstOrDefault()!;

        }

        public User GetAccountByStatus(string status)
        {
            return _context.Users.Where(a => a.Status == status).FirstOrDefault()!;
        }

        public User GetAccountByUpdatedAt(DateTime updatedAt)
        {
            return _context.Users.Where(a => a.UpdatedAt == updatedAt).FirstOrDefault()!;
        }

        public ICollection<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserByName(string name)
        {
            return _context.Users.Where(a => a.Name == name).FirstOrDefault()!;
        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}
