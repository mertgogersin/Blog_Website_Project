using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class DeletedArticle
    {
        [Key]
        public int ID { get; set; }
        public int ArticleID { get; set; }
        public Guid UserID { get; set; }
        public int? ReadCount { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Content { get; set; }
        public double ReadTime { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
