using Core.Entities;
using Core.Repositories;
using Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Services
{
    public class UserService : IUserService
    {
        IUserRepository userRepository;
        IEmailService emailService;
        private readonly AppSettings appSettings;

        public UserService(IUserRepository userRepository, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            this.userRepository = userRepository;
            this.emailService = emailService;
            this.appSettings = appSettings.Value;

        }

        public string AddUser(User user)
        {

            user.UserID = Guid.NewGuid();
            string fullname = user.Email.Split('@')[0];
            user.UserName = user.Email.Split('@')[0];
            user.CreatedDate = DateTime.Now;
            user.FullName = Regex.Replace(fullname, "[^a-zA-Z]", "");
            user.Url = Regex.Replace(user.UserName, "[^a-zA-Z]", "");
            foreach (User item in userRepository.GetUsers())
            {
                if (item.Url == user.Url)
                {
                    user.Url = user.Url + "x";
                }
            }

            bool check = userRepository.AddUser(user);
            if (check)
            {
                string jwtToken = generateJwtToken(user);
                return jwtToken;
            }
            else
            {
                return null;
            }
        }
        public List<string> UpdateUser(User user)
        {
            user.FullName = Regex.Replace(user.FullName, "[^a-zA-Z]", "");
            user.Url = Regex.Replace(user.Url, "[^a-zA-Z]", "");
            List<User> users = userRepository.GetUsers();
            List<string> errors = new List<string>();
            //unique validation
            bool checkUserName = users.Where(m => m.UserID != user.UserID).All(m => m.UserName != user.UserName);
            if (!checkUserName) { errors.Add("Please use different username."); }

            bool checkEmail = users.Where(m => m.UserID != user.UserID).All(m => m.Email != user.Email);
            if (!checkEmail || !user.Email.EndsWith("gmail.com")) { errors.Add("Please use different Email."); }

            bool checkUrl = users.Where(m => m.UserID != user.UserID).All(m => m.Url != user.Url);
            if (!checkUrl) { errors.Add("Please use different Url."); }

            if (errors.Count == 0)
            {
                userRepository.UpdateUser(user);
                return null;
            }
            return errors;
        }
        public string DeleteUser(User user)
        {
            bool check = userRepository.DeleteUser(user);
            if (check)
            {
                return "Kullanıcı başarıyla silinmiştir.";
            }
            else
            {
                return "Kullanıcı silinemedi.";
            }
        }
        public bool DeleteUser(string email)
        {
            User user = userRepository.GetUserByEmail(email);
            bool check = userRepository.DeleteUser(user);
            if (check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public User ValidateUser(string token, bool isLogin)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenHandler = new JwtSecurityTokenHandler();
            string userID = null;
            SecurityToken securityToken = null;
            try
            {
                //Check if token is valid(expire time etc.)
                tokenHandler.ValidateToken(token, new TokenValidationParameters { ValidateLifetime = true, IssuerSigningKey = new SymmetricSecurityKey(key), ValidateAudience = false, ValidateIssuer = false }, out securityToken);
                if (securityToken != null)
                {
                    userID = ((JwtSecurityToken)securityToken).Claims.Where(x => x.Type == "id").First().Value;
                    if (!isLogin) userRepository.ActivateUser(userID);

                }
                return userRepository.GetUserByUserId(userID);
            }
            catch (Exception)
            {
                if (!isLogin) userRepository.DeleteExpiredUsers();
                return null;
            }
        }
        public async void SendActivationEmailToUser(string email, string activationLink)
        {
            User loggedInUser = userRepository.GetUserByEmail(email);
            EmailRequest emailRequest = new EmailRequest();
            emailRequest.ToEmail = loggedInUser.Email;
            string type = "Account activation link: ";
            if (loggedInUser.IsActive)
            {
                emailRequest.Subject = "Login Link";
                type = "Your login link: ";
            }
            else
            {
                emailRequest.Subject = "Account Activation";
            }
            emailRequest.Body = "<!DOCTYPE html>" +
                                "<html> " +
                                    "<body style=\"background -color:#ff7f26;text-align:center;\"> " +
                                    "<h4>" + type + activationLink + "</h4>" +
                                    "<label style=\"color:black;font-size:75px;border:3px solid;border-radius:50px;padding: 5px\">My Blog Site</label> " +
                                    "</body> " +
                                "</html>";
            await emailService.SendEmailAsync(emailRequest);
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserID.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public void ChangeFullName(string userId, string fullName)
        {
            string newFullName = Regex.Replace(fullName, "[^a-zA-Z]", "");

            userRepository.ChangeFullName(userId, newFullName);
        }

        public string Login(string email)
        {
            foreach (User item in userRepository.GetUsers())
            {
                if (item.Email == email && item.IsActive)
                {

                    return generateJwtToken(item);
                }
            }
            return null;
        }

        public User GetUserByUserId(string userId)
        {
            return userRepository.GetUserByUserId(userId);
        }
        public User GetUserByUrl(string url)
        {
            return userRepository.GetUserByUrl(url);
        }
    }
}
