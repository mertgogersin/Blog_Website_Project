using Core.Entities;
using Core.Repositories;
using Core.Services;
using System.Collections.Generic;

namespace Services.Services
{
    public class TopicService : ITopicService
    {
        ITopicRepository topicRepository;
        public TopicService(ITopicRepository topicRepository)
        {
            this.topicRepository = topicRepository;
        }
        public bool AddFavouriteTopicsToUser(string userId, List<Topic> topics)
        {
            bool check = topicRepository.AddFavouriteTopicsToUser(userId, topics);
            if (check)
            {
                return true;
            }
            return false;
        }
        public List<Topic> GetTopics()
        {
            return topicRepository.GetTopics();
        }
        public bool AddTopicsToArticle(int articleId, List<Topic> topics)
        {
            bool check = topicRepository.AddTopicsToArticle(articleId, topics);
            if (check)
            {
                return true;
            }
            return false;
        }
        public List<Topic> GetFavouriteTopicsByUserID(string userId)
        {
            return topicRepository.GetFavouriteTopicsByUserID(userId);
        }
        public List<Topic> GetArticleTopics(int articleId)
        {
            return topicRepository.GetArticleTopics(articleId);
        }
    }
}
