using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class ArticleWithTopics
    {
        public ArticleWithTopics()
        {
            
        }

        public ArticleDTO ArticleDTO { get; set; }
        public TopicDTO[] TopicDTOs { get; set; }   
    }
}
