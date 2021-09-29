using Blog_Website_Project.Models.DTOs;
using Core.Entities;
using Core.Services;
using System.Collections.Generic;

namespace Blog_Website_Project.Models.Repositories
{
    public class ConvertToDTORepository
    {
        IUserService userService;
        public ConvertToDTORepository(IUserService userService)
        {
            this.userService = userService;

        }
        public ConvertToDTORepository()
        {

        }
        public List<ArticleDTO> GetArticleDTOList(List<Article> articleList)
        {
            List<ArticleDTO> articleDTOList = new List<ArticleDTO>();
            foreach (Article item in articleList)
            {
                ArticleDTO articleDTO = new ArticleDTO()
                {
                    UserID = item.UserID,
                    FullName = userService.GetUserByUserId(item.UserID.ToString()).FullName,
                    Url = userService.GetUserByUserId(item.UserID.ToString()).Url,
                    ArticleID = item.ArticleID,
                    Title = item.Title,
                    Subtitle = item.Subtitle,
                    Content = item.Content,
                    ReadTime = item.ReadTime,
                    PhotoPath = userService.GetUserByUserId(item.UserID.ToString()).Picture,
                    CreatedDate = item.CreatedDate
                };
                articleDTOList.Add(articleDTO);
            }
            return articleDTOList;
        }
        public ArticleDTO GetArticleDTO(Article article)
        {
            ArticleDTO articleDTO = new ArticleDTO()
            {
                UserID = article.UserID,
                FullName = userService.GetUserByUserId(article.UserID.ToString()).FullName,
                Url = userService.GetUserByUserId(article.UserID.ToString()).Url,
                ArticleID = article.ArticleID,
                Title = article.Title,
                Subtitle = article.Subtitle,
                Content = article.Content,
                ReadTime = article.ReadTime,
                PhotoPath = userService.GetUserByUserId(article.UserID.ToString()).Picture,
                CreatedDate = article.CreatedDate
            };
            return articleDTO;
        }
        public List<TopicDTO> GetTopicDTOList(List<Topic> topicList)
        {
            List<TopicDTO> topicDTOList = new List<TopicDTO>();
            TopicDTO topicDTO;
            foreach (Topic item in topicList)
            {
                topicDTO = new TopicDTO()
                {
                    TopicID = item.TopicID,
                    TopicName = item.TopicName,
                    Description = item.Description
                };
                topicDTOList.Add(topicDTO);
            }
            return topicDTOList;
        }
        public UserDTO GetUserDTO(User user)
        {
            UserDTO userDTO = new UserDTO()
            {
                UserID = user.UserID,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Url = user.Url,
                PhotoPath = user.Picture,
                Description = user.Description
            };
            return userDTO;

        }
    }
}
