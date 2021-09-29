using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class TopicsAndPopularArticles
    {
        public TopicsAndPopularArticles()
        {
            TopicDTOs = new List<TopicDTO>();
            ArticleDTOs = new List<ArticleDTO>();
        }
        public List<TopicDTO> TopicDTOs { get; set; }
        public List<ArticleDTO> ArticleDTOs { get; set; }
 
    }
}
