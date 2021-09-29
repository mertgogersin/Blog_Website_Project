using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class AllAndFollowedTopicDTOs
    {
        public AllAndFollowedTopicDTOs()
        {
            TopicDTOs = new List<TopicDTO>();
            FollowedTopicDTOs = new List<TopicDTO>();            
        }
        public List<TopicDTO> TopicDTOs { get; set; }      
        public List<TopicDTO> FollowedTopicDTOs { get; set; }      
       
    }
}
