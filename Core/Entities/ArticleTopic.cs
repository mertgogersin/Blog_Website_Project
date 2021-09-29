using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("ArticleTopics")]
    public class ArticleTopic
    {

        public int ArticleID { get; set; }

        public int TopicID { get; set; }

        public Article Article { get; set; }
        public Topic Topic { get; set; }
    }
}
