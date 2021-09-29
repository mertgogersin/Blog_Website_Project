using Blog_Website_Project.Models;
using Blog_Website_Project.Models.Abstracts;
using Blog_Website_Project.Models.DTOs;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Website_Project.Controllers
{
    public class RegisterController : Controller
    {
        IUserService userService;
        IUploadPictureRepository uploadPictureRepository;
        public RegisterController(IUserService userService, IUploadPictureRepository uploadPictureRepository)
        {
            this.userService = userService;
            this.uploadPictureRepository = uploadPictureRepository;
        }
        public IActionResult Index()
        {
            return RedirectToAction("GeneralHomePage", "Home");
        }
        public IActionResult GetRegisterModalPartial()
        {
            return PartialView("_RegisterModal");
        }
        [HttpPost]
        public IActionResult AddUser(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {

                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        TempData["error"] = modelError.ErrorMessage;
                    }
                }
                return RedirectToAction("Index");
            }

            User user = new User()
            {
                Email = registerDTO.Email,
                Picture = uploadPictureRepository.GetPhotoFile(null)
            };
            string token = userService.AddUser(user);

            if (token != null)
            {
                var confirmationLink = "<a href='"
                        + Url.Action("ActivateUser", "Register", new { token = token }, Request.Scheme)
                        + "'>Click here</a>";
                userService.SendActivationEmailToUser(registerDTO.Email, confirmationLink);
                TempData["message"] = "Your activation link has been sent. Please check your inbox.";
            }
            else
            {
                TempData["error"] = "There is already such an account.";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ActivateUser(string token)
        {
            User user = userService.ValidateUser(token, false);
            if (user != null)
            {

                UserDTO userDTO = new UserDTO()
                {
                    UserID = user.UserID,
                    UserName = user.UserName,
                    Email = user.Email,
                    FullName = user.FullName,
                    Picture = null,
                    Url = user.Url
                };
                User checkedUser = uploadPictureRepository.ConvertUserDTO(userDTO, user.UserID.ToString());

                HttpContext.Session.SetString("url", checkedUser.Url);
                HttpContext.Session.SetString("FullName", checkedUser.FullName);
                HttpContext.Session.SetObject("Login", checkedUser);
                return View(userDTO);
            }
            else
            {

                return View();
            }
        }


    }
}
