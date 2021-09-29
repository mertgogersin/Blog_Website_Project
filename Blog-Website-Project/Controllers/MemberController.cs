using Blog_Website_Project.Models;
using Blog_Website_Project.Models.Abstracts;
using Blog_Website_Project.Models.DTOs;
using Blog_Website_Project.Models.Repositories;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog_Website_Project.Controllers
{
    public class MemberController : Controller
    {
        IUserService userService;
        IArticleService articleService;
        IUploadPictureRepository uploadPictureRepository;
        ConvertToDTORepository convertToDTORepository;

        public MemberController(IUserService userService, IArticleService articleService, IUploadPictureRepository uploadPictureRepository)
        {
            this.userService = userService;
            this.articleService = articleService;
            this.uploadPictureRepository = uploadPictureRepository;
            convertToDTORepository = new ConvertToDTORepository(userService);

        }
        //Member profile page
        [Route("[controller]/{url}")]
        public IActionResult Index(string url)
        {

            User loggedInUser = HttpContext.Session.GetObject<User>("Login");
            ViewBag.IsLoggedInUser = true;
            UserWithArticles userWithArticles = new UserWithArticles();
            User userFromArticle = HttpContext.Session.GetObject<User>("UserByArticle");
            User userByUrl = userService.GetUserByUrl(url);

            if (userFromArticle != null)
            {
                HttpContext.Session.Remove("UserByArticle");
                userWithArticles.UserDTO = convertToDTORepository.GetUserDTO(userFromArticle);
                userWithArticles.ArticleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetArticlesByUserID(userFromArticle.UserID.ToString()));

                if (loggedInUser != null)
                {
                    if (userFromArticle.UserID != loggedInUser.UserID) { ViewBag.IsLoggedInUser = false; }
                }
                else
                {
                    ViewBag.IsLoggedInUser = false;
                }
                return View(userWithArticles);
            }

            if (userByUrl != null) { userWithArticles.UserDTO = convertToDTORepository.GetUserDTO(userByUrl); }

            if (userWithArticles.UserDTO != null)
            {
                if (loggedInUser == null) { ViewBag.IsLoggedInUser = false; }
                else if (loggedInUser.UserID != userByUrl.UserID) { ViewBag.IsLoggedInUser = false; }
                userWithArticles.ArticleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetArticlesByUserID(userWithArticles.UserDTO.UserID.ToString()));
            }
            else
            {
                if (loggedInUser != null) { return RedirectToAction("UserHomePage", "User"); }
                else { return RedirectToAction("GeneralHomePage", "Home"); }
            }
            return View(userWithArticles);
        }

        public IActionResult GetUser(string id)
        {
            User user = userService.GetUserByUserId(id);
            HttpContext.Session.SetObject("UserByArticle", user);
            return Json("ok");
        }


        [HttpPost]
        public IActionResult ChangeFullNameFirstTime(UserDTO userDTO)
        {
            if (userDTO.FullName == null)
            {
                TempData["error"] = "Fullname section must be filled";
                return RedirectToAction("GeneralHomePage", "Home");
            }


            userService.ChangeFullName(userDTO.UserID.ToString(), userDTO.FullName);
            User loggedInUser = userService.GetUserByUserId(userDTO.UserID.ToString());

            HttpContext.Session.SetObject("Login", loggedInUser);
            HttpContext.Session.SetString("FullName", userDTO.FullName);

            return Json("ok");
        }
        public IActionResult UpdateUser(string id)
        {
            UserDTO userDTO = convertToDTORepository.GetUserDTO(userService.GetUserByUserId(id));
            return View(userDTO);
        }
        [HttpPost]
        public IActionResult UpdateUser(UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(userDTO);
            }

            string userId = HttpContext.Session.GetObject<User>("Login").UserID.ToString();
            User user = uploadPictureRepository.ConvertUserDTO(userDTO, userId);

            List<string> errors = userService.UpdateUser(user);
            if (errors != null)
            {
                UserDTO checkedUserDTO1 = convertToDTORepository.GetUserDTO(user);
                ViewBag.Errors = errors;
                return View(checkedUserDTO1);
            }
            ViewBag.Success = "User information has been updated.";
            HttpContext.Session.SetObject("Login", user);
            HttpContext.Session.SetString("url", user.Url);
            UserDTO checkedUserDTO = convertToDTORepository.GetUserDTO(user);
            return View(checkedUserDTO);
        }
        public IActionResult DeleteUser(string id)
        {
            User user = userService.GetUserByUserId(id);
            userService.DeleteUser(user);
            return RedirectToAction("LogOut", "User");
        }
    }
}
