using Core.Entities;
using System.Collections.Generic;

namespace Core.Services
{
    public interface IUserService
    {
        string AddUser(User user);
        List<string> UpdateUser(User user);
        void SendActivationEmailToUser(string email, string activationLink);
        User ValidateUser(string token, bool isLogin);
        User GetUserByUserId(string userId);
        User GetUserByUrl(string url);
        string DeleteUser(User user);
        bool DeleteUser(string email);
        void ChangeFullName(string userId, string fullName);
        string Login(string email);

    }
}
