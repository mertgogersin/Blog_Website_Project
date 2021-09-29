using Core.Entities;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IUserRepository
    {
        bool AddUser(User user);
        void ActivateUser(string userId);
        bool DeleteUser(User user);
        void UpdateUser(User user);
        void DeleteExpiredUsers();
        List<User> GetUsers();
        User GetUserByUserId(string userId);
        User GetUserByEmail(string email);
        User GetUserByUrl(string url);
        void ChangeFullName(string userId, string fullName);

    }
}
