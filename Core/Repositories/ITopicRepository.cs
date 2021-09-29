using Core.Entities;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface ITopicRepository
    {
        bool AddFavouriteTopicsToUser(string userId, List<Topic> topics);
        List<Topic> GetTopics();
        Topic GetTopicByTopicID(int topicID);
        bool AddTopicsToArticle(int articleId, List<Topic> topics);
        List<Topic> GetArticleTopics(int articleId);
        List<Topic> GetFavouriteTopicsByUserID(string userId);
    }
}
