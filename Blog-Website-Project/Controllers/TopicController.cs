using Blog_Website_Project.Models;
using Blog_Website_Project.Models.DTOs;
using Blog_Website_Project.Models.Repositories;
using Core.Entities;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Blog_Website_Project.Controllers
{
    public class TopicController : Controller
    {
        ITopicService topicService;
        IArticleService articleService;
        ConvertToDTORepository convertToDTORepository;
        public TopicController(ITopicService topicService, IArticleService articleService)
        {
            this.topicService = topicService;
            this.articleService = articleService;
            convertToDTORepository = new ConvertToDTORepository();
        }
        public IActionResult Index()
        {
            //Check if user has already has followed topics and if he has, checkboxes at specific topic will be checked
            List<TopicDTO> topics = new List<TopicDTO>();
            foreach (TopicDTO allTopic in convertToDTORepository.GetTopicDTOList(topicService.GetTopics()))
            {
                foreach (Topic favTopic in topicService.GetFavouriteTopicsByUserID(HttpContext.Session.GetObject<User>("Login").UserID.ToString()))
                {
                    if (favTopic.TopicName == allTopic.TopicName)
                    {
                        allTopic.IsSelected = true;
                    }

                }
                topics.Add(allTopic);
            }
            return View(topics);
        }

        public IActionResult AddTopicsToUser(TopicDTO[] topicDTOs)
        {
            List<Topic> topics = new List<Topic>();
            foreach (TopicDTO item in topicDTOs)
            {
                if (item.IsSelected)
                {
                    Topic topic = new Topic()
                    {
                        TopicID = item.TopicID,
                        TopicName = item.TopicName,
                        Description = item.Description
                    };
                    topics.Add(topic);
                }
            }
            User user = HttpContext.Session.GetObject<User>("Login");
            topicService.AddFavouriteTopicsToUser(user.UserID.ToString(), topics);
            return Json("ok");
        }


    }
}
