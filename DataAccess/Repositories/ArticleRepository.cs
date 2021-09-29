using Core.Entities;
using DataAccess.BlogDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        BlogContext context;
        ITopicRepository topicRepository;
        public ArticleRepository(BlogContext context, ITopicRepository topicRepository)
        {
            this.context = context;
            this.topicRepository = topicRepository;
        }
        public bool AddNewArticle(Article article, List<Topic> topics)
        {
            try
            {
                article.CreatedDate = DateTime.Now;
                article.ReadTime = CalculateReadTime(article.Content);
                context.Articles.Add(article);
                context.SaveChanges();
                Article newArticle = context.Articles.OrderByDescending(m => m.CreatedDate).Take(1).FirstOrDefault();
                if (topics.Count > 0) { topicRepository.AddTopicsToArticle(newArticle.ArticleID, topics); }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void AddReadCount(int articleId)
        {
            Article article = context.Articles.Where(m => m.ArticleID == articleId).FirstOrDefault();
            article.ReadCount++;
            context.SaveChanges();
        }
        private int CalculateReadTime(string content)
        {
            //1 minute every 1000 chars
            int minutes = 0;
            long letters = content.ToArray().Length;
            while (letters >= 1000)
            {
                if (letters % 1000 < 1000)
                {
                    letters -= 1000;
                    minutes++;
                }
            }
            return minutes;
        }
        public List<Article> GetArticles()
        {
            return context.Articles.OrderByDescending(m => m.CreatedDate).ToList();
        }
        public Article GetArticleByArticleID(int id)
        {
            return context.Articles.Where(m => m.ArticleID == id).FirstOrDefault();
        }
        public List<Article> GetArticlesByUserID(string userId)
        {
            List<Article> articles = context.Articles.Where(x => x.UserID == Guid.Parse(userId)).ToList();
            return articles;
        }

        public List<Article> GetArticlesByTopicID(int topicId)
        {
            List<int> articleIds = context.ArticleTopics.Where(x => x.TopicID == topicId).Select(x => x.ArticleID).ToList();
            List<Article> articles = new List<Article>();
            foreach (int item in articleIds)
            {
                Article article = context.Articles.Where(m => m.ArticleID == item).OrderByDescending(m => m.CreatedDate).FirstOrDefault();
                articles.Add(article);
            }

            return articles;
        }

        public List<Article> GetMostPopularArticles()
        {
            List<Article> articles = context.Articles.OrderByDescending(x => x.ReadCount).Take(10).ToList();
            return articles;
        }

        public List<Article> GetUserStoriesByFollowedTopics(string userId)
        {


            List<Topic> topics = topicRepository.GetFavouriteTopicsByUserID(userId);
            List<Article> articles = new List<Article>();
            foreach (Topic topic in topics)
            {
                foreach (ArticleTopic articleTopic in context.ArticleTopics.ToList())
                {
                    Article article = context.Articles.Where(m => m.ArticleID == articleTopic.ArticleID).FirstOrDefault();
                    if (topic.TopicID == articleTopic.TopicID && !articles.Contains(article))
                    {
                        articles.Add(article);
                    }
                }
            }

            return articles;
        }
        public void RemoveArticleTopics(int articleId)
        {
            List<ArticleTopic> articleTopics = new List<ArticleTopic>();
            articleTopics = context.ArticleTopics.Where(m => m.ArticleID == articleId).ToList();
            foreach (ArticleTopic item in articleTopics)
            {
                context.ArticleTopics.Remove(item);
            }
            context.SaveChanges();
        }
        public void RemoveArticle(Article article)
        {
            DeletedArticle deletedArticle = new DeletedArticle()
            {
                ArticleID = article.ArticleID,
                UserID = article.UserID,
                ReadCount = article.ReadCount,
                Title = article.Title,
                Subtitle = article.Subtitle,
                Content = article.Content,
                ReadTime = article.ReadTime,
                DeletedDate = DateTime.Now
            };
            RemoveArticleTopics(article.ArticleID);
            context.DeletedArticles.Add(deletedArticle);
            context.Articles.Remove(article);
            context.SaveChanges();
        }
        public void RemoveArticles(List<Article> articles)
        {
            foreach (Article item in articles)
            {
                RemoveArticleTopics(item.ArticleID);
                context.Articles.Remove(item);
            }
            context.SaveChanges();
        }


        public void UpdateArticle(Article article, List<Topic> topics)
        {

            Article newArticle = context.Articles.Find(article.ArticleID);
            newArticle.Title = article.Title;
            newArticle.Subtitle = article.Subtitle;
            newArticle.Content = article.Content;
            newArticle.ReadTime = CalculateReadTime(article.Content);
            newArticle.ModifiedDate = DateTime.Now;
            newArticle.UserID = article.UserID;
            if (topics.Count > 0) { topicRepository.AddTopicsToArticle(newArticle.ArticleID, topics); }
            context.SaveChanges();


        }
    }
}
