using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Core.Entities
{
    [Table("Topics")]
    public class Topic
    {
        [Key]
        public int TopicID { get; set; }
        [Required]
        public string TopicName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserTopic> UserTopics { get; set; }
        public virtual ICollection<ArticleTopic> ArticleTopics { get; set; }
    }
}
