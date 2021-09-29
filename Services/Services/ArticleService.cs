using Core.Entities;
using Core.Repositories;
using Core.Services;
using System.Collections.Generic;

namespace Services.Services
{
    public class ArticleService : IArticleService
    {
        IArticleRepository articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;

        }
        public bool AddNewArticle(Article article, List<Topic> topics)
        {
            bool check = articleRepository.AddNewArticle(article, topics);
            if (check)
            {
                return true;
            }
            return false;
        }
        public void AddReadCount(int articleId)
        {
            articleRepository.AddReadCount(articleId);
        }
        public List<Article> GetArticles()
        {
            return articleRepository.GetArticles();
        }
        public Article GetArticleByArticleID(int id)
        {
            return articleRepository.GetArticleByArticleID(id);
        }
        public List<Article> GetArticlesByUserID(string userId)
        {
            return articleRepository.GetArticlesByUserID(userId);
        }
        public List<Article> GetArticlesByTopicID(int topicId)
        {
            return articleRepository.GetArticlesByTopicID(topicId);
        }
        public List<Article> GetMostPopularArticles()
        {
            return articleRepository.GetMostPopularArticles();
        }
        public List<Article> GetUserStoriesByFollowedTopics(string userId)
        {
            return articleRepository.GetUserStoriesByFollowedTopics(userId);
        }
        public void RemoveArticle(Article article)
        {
            articleRepository.RemoveArticle(article);
        }

        public void UpdateArticle(Article article, List<Topic> topics)
        {
            articleRepository.UpdateArticle(article, topics);
        }
    }
}
