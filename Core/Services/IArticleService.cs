using Core.Entities;
using System.Collections.Generic;

namespace Core.Services
{
    public interface IArticleService
    {
        bool AddNewArticle(Article article, List<Topic> topics);
        void UpdateArticle(Article article, List<Topic> topics);
        void AddReadCount(int articleId);
        List<Article> GetArticles();
        Article GetArticleByArticleID(int id);
        List<Article> GetArticlesByUserID(string userId);
        List<Article> GetArticlesByTopicID(int topicId);
        List<Article> GetMostPopularArticles();
        List<Article> GetUserStoriesByFollowedTopics(string userId);
        void RemoveArticle(Article article);
    }
}
