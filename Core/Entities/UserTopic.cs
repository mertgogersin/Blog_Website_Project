using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("UserTopics")]
    public class UserTopic
    {

        public Guid UserID { get; set; }

        public int TopicID { get; set; }

        public User User { get; set; }
        public Topic Topic { get; set; }
    }
}
