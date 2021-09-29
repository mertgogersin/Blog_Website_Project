using DataAccess.BlogDataAccess;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        BlogContext context;
        IArticleRepository articleRepository;



        public UserRepository(BlogContext context, IArticleRepository articleRepository)
        {
            this.context = context;
            this.articleRepository = articleRepository;
        }
        public bool AddUser(User user)
        {
            try
            {
                context.Users.Add(user);
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void UpdateUser(User user)
        {
            User newUser = context.Users.Where(m => m.UserID == user.UserID).FirstOrDefault();
            newUser.FullName = user.FullName;
            newUser.Description = user.Description;
            if (user.Picture != @"Images\user1-64x64.png")//if user didn't choose a new photo, this helps not to use default icon
            {
                newUser.Picture = user.Picture;
            }
            newUser.UserName = user.UserName;
            newUser.Url = user.Url;
            newUser.Email = user.Email;
            newUser.ModifiedDate = DateTime.Now;

            context.SaveChanges();
        }
        public void DeleteUserTopics(string userId)
        {
            List<UserTopic> userTopics = new List<UserTopic>();
            userTopics = context.UserTopics.Where(m => m.UserID == Guid.Parse(userId)).ToList();
            foreach (UserTopic item in userTopics)
            {
                context.UserTopics.Remove(item);
            }
            context.SaveChanges();
        }
        public bool DeleteUser(User user)
        {
            try
            {
                User selectedUser = context.Users.Where(m => m.UserID == user.UserID && m.IsActive).FirstOrDefault();
                DeletedUser deletedUser = new DeletedUser()
                {
                    UserID = selectedUser.UserID,
                    UserName = selectedUser.UserName,
                    Email = selectedUser.Email,
                    FullName = selectedUser.FullName,
                    Url = selectedUser.Url,
                    Description = selectedUser.Description,
                    Picture = selectedUser.Picture,
                    IsActive = selectedUser.IsActive,
                    DeletedDate = DateTime.Now,
                    ActivationDate = selectedUser.ActivationDate
                };
                DeleteUserTopics(user.UserID.ToString());
                articleRepository.RemoveArticles(articleRepository.GetArticlesByUserID(user.UserID.ToString()));
                context.Users.Remove(selectedUser);
                context.DeletedUsers.Add(deletedUser);
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }

        }
        private List<User> GetExpiredUsers()
        {
            List<User> users = context.Users.Where(m => m.ActivationDate == null && EF.Functions.DateDiffMinute(m.CreatedDate, DateTime.Now) > 5).ToList();
            return users;
        }
        public void DeleteExpiredUsers()
        {
            foreach (User item in GetExpiredUsers())
            {
                DeletedUser deletedUser = new DeletedUser()
                {
                    UserID = item.UserID,
                    UserName = item.UserName,
                    Email = item.Email,
                    FullName = item.FullName,
                    Url = item.Url,
                    Description = item.Description,
                    Picture = item.Picture,
                    IsActive = item.IsActive,
                    DeletedDate = DateTime.Now,
                    ActivationDate = item.ActivationDate
                };
                context.DeletedUsers.Add(deletedUser);
                context.Remove(item);
            }
            context.SaveChanges();
        }
        public void ActivateUser(string userId)
        {

            User user = context.Users.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
            user.IsActive = true;
            user.ModifiedDate = DateTime.Now;
            user.ActivationDate = DateTime.Now;
            context.SaveChanges();

        }
        public List<User> GetUsers()
        {
            return context.Users.ToList();
        }
        public User GetUserByUserId(string userId)
        {
            return context.Users.Where(m => m.UserID == Guid.Parse(userId)).FirstOrDefault();
        }
        public User GetUserByEmail(string email)
        {
            return context.Users.Where(m => m.Email == email).FirstOrDefault();
        }
        public User GetUserByUrl(string url)
        {
            return context.Users.Where(m => m.Url == url).FirstOrDefault();
        }

        public void ChangeFullName(string userId, string fullName)
        {
            User user = GetUserByUserId(userId);
            user.FullName = fullName;
            user.ModifiedDate = DateTime.Now;
            context.SaveChanges();
        }







    }
}
