using System.Security.Principal;
using TestCase.Models;

namespace TestCase.Interface
{
    public interface IUser
    {
        ICollection<User> GetAllUsers();
        User GetUserByName(string name);
        User GetAccountByEmail(string email);
        User GetAccountByPhoneNumber(string phoneNumber);
        User GetAccountByStatus(string status);
        User GetAccountByCreatedAt(DateTime createdAt);
        User GetAccountByUpdatedAt(DateTime updatedAt);
        void Register(User user);

    }
}
