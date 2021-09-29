using Blog_Website_Project.Models;
using Blog_Website_Project.Models.DTOs;
using Blog_Website_Project.Models.Repositories;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog_Website_Project.Controllers
{
    public class UserController : Controller
    {
        IUserService userService;
        ITopicService topicService;
        IArticleService articleService;
        ConvertToDTORepository convertToDTORepository;
        public UserController(IUserService userService, ITopicService topicService, IArticleService articleService)
        {
            this.userService = userService;
            this.topicService = topicService;
            this.articleService = articleService;
            convertToDTORepository = new ConvertToDTORepository(userService);
        }
        public IActionResult UserHomePage()
        {

            AllAndFollowedTopicDTOs allAndFollowedTopicDTOs = new AllAndFollowedTopicDTOs();

            string userID = HttpContext.Session.GetObject<User>("Login").UserID.ToString();

            allAndFollowedTopicDTOs.TopicDTOs = convertToDTORepository.GetTopicDTOList(topicService.GetTopics());

            allAndFollowedTopicDTOs.FollowedTopicDTOs = convertToDTORepository.GetTopicDTOList(topicService.GetFavouriteTopicsByUserID(userID));

            return View(allAndFollowedTopicDTOs);
        }

        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            string token = userService.Login(loginDTO.Email);
            if (token != null)
            {
                var confirmationLink = "<a href='"
                        + Url.Action("Login", "User", new { token = token }, Request.Scheme)
                        + "'>Click here</a>";
                userService.SendActivationEmailToUser(loginDTO.Email, confirmationLink);
                TempData["message"] = "Your login link has been sent. Please check your inbox.";

            }
            else
            {
                bool check = userService.DeleteUser(loginDTO.Email);
                if (check)
                {
                    TempData["message"] = "Your account is not active. Please sign up again.";
                }
                else
                {
                    TempData["message"] = "Account not found.";
                }

            }

            return RedirectToAction("GeneralHomePage", "Home");
        }

        public IActionResult Login(string token)
        {
            if (token == null || !ModelState.IsValid)
            {
                return View();
            }
            User user = userService.ValidateUser(token, true);
            if (user != null)
            {
                HttpContext.Session.SetObject("Login", user);
                HttpContext.Session.SetString("FullName", user.FullName);
                HttpContext.Session.SetString("url", user.Url);

                List<Topic> topics = topicService.GetFavouriteTopicsByUserID(user.UserID.ToString());

                if (topics.Count > 0)
                {
                    return RedirectToAction(nameof(UserHomePage));
                }
                //to topic list page
                return RedirectToAction("Index", "Topic");
            }
            else
            {
                TempData["message"] = "Your login link is expired. Please sign in again.";
            }
            return RedirectToAction("GeneralHomePage", "Home");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("FullName");
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("url");
            return RedirectToAction("GeneralHomePage", "Home");
        }

    }
}

