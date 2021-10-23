using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Entities
{
    [Table("Articles")]
    public class Article
    {
        [Key]
        public int ArticleID { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public int ReadCount { get; set; }
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        public string Subtitle { get; set; }
        [Required]
        public string Content { get; set; }
        public int ReadTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public User User { get; set; }

        public ICollection<ArticleTopic> ArticleTopics { get; set; }

    }
}
