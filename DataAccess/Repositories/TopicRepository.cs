using Core.Entities;
using DataAccess.BlogDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        BlogContext context;
        public TopicRepository(BlogContext context)
        {
            this.context = context;
        }
        public bool AddFavouriteTopicsToUser(string userId, List<Topic> topics)
        {
            try
            {
                DeleteUserTopicsByUserID(userId);
                foreach (Topic item in topics)
                {
                    context.UserTopics.Add(new UserTopic()
                    {
                        TopicID = item.TopicID,
                        UserID = Guid.Parse(userId)
                    });
                }
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private void DeleteUserTopicsByUserID(string userId)
        {
            List<UserTopic> userTopics = new List<UserTopic>();
            userTopics = context.UserTopics.Where(m => m.UserID == Guid.Parse(userId)).ToList();
            if (userTopics.Count > 0)
            {
                foreach (UserTopic item in userTopics.ToList())
                {
                    context.UserTopics.Remove(item);
                }

            }
        }
        private void DeleteArticleTopicsByArticleID(int articleId)
        {
            List<ArticleTopic> articleTopics = new List<ArticleTopic>();
            articleTopics = context.ArticleTopics.Where(m => m.ArticleID == articleId).ToList();
            if (articleTopics.Count > 0)
            {
                foreach (ArticleTopic item in articleTopics.ToList())
                {
                    context.ArticleTopics.Remove(item);
                }
                context.SaveChanges();
            }
        }
        public List<Topic> GetTopics()
        {
            return context.Topics.ToList();
        }
        public Topic GetTopicByTopicID(int topicID)
        {
            return context.Topics.Where(m => m.TopicID == topicID).FirstOrDefault();
        }
        public List<Topic> GetArticleTopics(int articleId)
        {
            List<int> topicIDs = context.ArticleTopics.Where(m => m.ArticleID == articleId).Select(m => m.TopicID).ToList();
            List<Topic> topics = new List<Topic>();
            foreach (int item in topicIDs)
            {
                topics.Add(GetTopicByTopicID(item));
            }
            return topics;
        }
        public bool AddTopicsToArticle(int articleId, List<Topic> topics)
        {
            try
            {
                DeleteArticleTopicsByArticleID(articleId);
                foreach (Topic item in topics)
                {
                    context.ArticleTopics.Add(new ArticleTopic()
                    {
                        TopicID = item.TopicID,
                        ArticleID = articleId
                    });
                }
                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<Topic> GetFavouriteTopicsByUserID(string userId)
        {
            List<UserTopic> userTopics = context.UserTopics.Where(x => x.UserID == Guid.Parse(userId)).ToList();
            List<Topic> topics = new List<Topic>();
            foreach (UserTopic item in userTopics)
            {
                Topic topic = context.Topics.Find(item.TopicID);
                if (!topics.Contains(topic))
                {
                    topics.Add(topic);
                }
            }
            return topics;
        }
    }
}
