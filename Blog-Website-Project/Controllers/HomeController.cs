using Blog_Website_Project.Models.DTOs;
using Blog_Website_Project.Models.Repositories;
using Core.Services;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Controllers
{

    public class HomeController : Controller
    {  
        ITopicService topicService;
        IArticleService articleService;
        IUserService userService;
        ConvertToDTORepository convertToDTORepository;
        public HomeController(ITopicService topicService, IArticleService articleService, IUserService userService)
        {
            this.topicService = topicService;
            this.articleService = articleService;
            this.userService = userService;
            convertToDTORepository = new ConvertToDTORepository(userService);
        }
        //General home page
        public IActionResult GeneralHomePage()
        {
            TopicsAndPopularArticles topicsAndPopularArticles = new TopicsAndPopularArticles();
            topicsAndPopularArticles.TopicDTOs = convertToDTORepository.GetTopicDTOList(topicService.GetTopics());
            topicsAndPopularArticles.ArticleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetMostPopularArticles());          
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }
            if (TempData.ContainsKey("error"))
            {
                ViewBag.Message = TempData["error"].ToString();
            }
            return View(topicsAndPopularArticles);
        }
        
        

    }
}
