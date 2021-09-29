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
    public class ArticleController : Controller
    {
        ITopicService topicService;
        IArticleService articleService;
        IUserService userService;
        ConvertToDTORepository convertToDTORepository;

        public ArticleController(ITopicService topicService, IArticleService articleService, IUserService userService)
        {
            this.topicService = topicService;
            this.articleService = articleService;
            this.userService = userService;
            convertToDTORepository = new ConvertToDTORepository(userService);
        }

        public IActionResult Index(int? id = null)
        {
            User user = HttpContext.Session.GetObject<User>("Login");
            ArticleWithTopics articleWithTopics = new ArticleWithTopics();
            if (user == null)
            {
                return RedirectToAction("Login", "User");
            }
            if (id == null)
            {
                ViewBag.Errors = TempData["error"];
                ViewBag.Success = TempData["Success"];
                TempData["id"] = null;
                ViewBag.isUpdate = false;
                articleWithTopics.TopicDTOs = convertToDTORepository.GetTopicDTOList(topicService.GetTopics()).ToArray();
                return View(articleWithTopics);
            }
            else
            {
                List<TopicDTO> topicDTOs = new List<TopicDTO>();
                foreach (TopicDTO all in convertToDTORepository.GetTopicDTOList(topicService.GetTopics()))
                {
                    foreach (Topic selected in topicService.GetArticleTopics(id.Value))
                    {
                        if (selected.TopicName == all.TopicName)
                        {
                            all.IsSelected = true;
                        }
                    }
                    topicDTOs.Add(all);
                }

                ArticleDTO articleDTO = convertToDTORepository.GetArticleDTO(articleService.GetArticleByArticleID(id.Value));
                TempData["id"] = id;
                ViewBag.isUpdate = true;
                articleWithTopics.TopicDTOs = topicDTOs.ToArray();
                articleWithTopics.ArticleDTO = articleDTO;
                return View(articleWithTopics);
            }
        }

        public IActionResult GetArticlesPartial(int? id = null)
        {
            List<ArticleDTO> articleDTOs;
            if (id == null)
            {
                articleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetArticles());
            }
            else
            {
                articleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetArticlesByTopicID(id.Value));
            }
            return PartialView("_ListArticles", articleDTOs);
        }
        public IActionResult GetFollowedArticlesPartial()
        {
            User user = HttpContext.Session.GetObject<User>("Login");

            string userID = user.UserID.ToString();
            List<ArticleDTO> articleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetUserStoriesByFollowedTopics(userID));
            return PartialView("_ListArticles", articleDTOs);

        }
        public IActionResult GetPopularArticlesPartial()
        {
            List<ArticleDTO> articleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetMostPopularArticles());
            return PartialView("_ListArticles", articleDTOs);
        }

        [HttpPost]
        public JsonResult AddArticle(ArticleWithTopics articleWithTopics, int? id)
        {
            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        errors.Add(modelError.ErrorMessage);
                    }
                }
                TempData["error"] = errors;
                return Json("ok");
            }
            User user = HttpContext.Session.GetObject<User>("Login");
            List<Topic> topics = new List<Topic>();

            if (articleWithTopics.TopicDTOs != null)
            {

                foreach (TopicDTO item in articleWithTopics.TopicDTOs)
                {
                    if (item.IsSelected)
                    {
                        Topic topic = new Topic()
                        {
                            TopicID = item.TopicID,
                            TopicName = item.TopicName
                        };
                        topics.Add(topic);
                    }
                }
            }
            if (id == null)
            {
                if (articleWithTopics.ArticleDTO != null)
                {
                    Article article = new Article()
                    {
                        UserID = user.UserID,
                        Title = articleWithTopics.ArticleDTO.Title,
                        Content = articleWithTopics.ArticleDTO.Content,
                        Subtitle = articleWithTopics.ArticleDTO.Subtitle
                    };



                    articleService.AddNewArticle(article, topics);
                    TempData["Success"] = "Article successfully saved.";
                }
                return Json("ok");
            }
            else
            {
                Article article = new Article()
                {
                    ArticleID = articleWithTopics.ArticleDTO.ArticleID,
                    UserID = user.UserID,
                    Title = articleWithTopics.ArticleDTO.Title,
                    Content = articleWithTopics.ArticleDTO.Content,
                    Subtitle = articleWithTopics.ArticleDTO.Subtitle
                };

                articleService.UpdateArticle(article, topics);
                TempData["Success"] = "Article successfully updated.";
                return Json("ok");
            }

        }

        public IActionResult ArticleDetailsPage(int id)
        {

            ArticleDetailsAndUserDTO articleDetailsAndPhoto = new ArticleDetailsAndUserDTO();

            articleDetailsAndPhoto.ArticleDTO = convertToDTORepository.GetArticleDTO(articleService.GetArticleByArticleID(id));
            articleDetailsAndPhoto.UserDTO = convertToDTORepository.GetUserDTO(userService.GetUserByUserId(articleDetailsAndPhoto.ArticleDTO.UserID.ToString()));

            // I prevented the number of reads from increasing when clicking on my article from my profile page
            User user = HttpContext.Session.GetObject<User>("Login");
            if (user != null)
            {
                if (articleDetailsAndPhoto.ArticleDTO.UserID != user.UserID) { articleService.AddReadCount(id); }
            }
            else
            {
                articleService.AddReadCount(id);
            }

            return View(articleDetailsAndPhoto);
        }
        [Route("[controller]/[action]/{url}")]
        public IActionResult UserArticlesPage(string url)
        {
            string userId = userService.GetUserByUrl(url).UserID.ToString();
            List<ArticleDTO> articleDTOs = convertToDTORepository.GetArticleDTOList(articleService.GetArticlesByUserID(userId));
            return View(articleDTOs);
        }
        public IActionResult DeleteArticle(int id)
        {

            articleService.RemoveArticle(articleService.GetArticleByArticleID(id));
            return RedirectToAction(HttpContext.Session.GetString("url"), "Member");
        }
    }

}
