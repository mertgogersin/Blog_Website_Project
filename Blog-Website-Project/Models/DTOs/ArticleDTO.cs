using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class ArticleDTO
    {
        public int ArticleID { get; set; }
        public Guid UserID { get; set; }    
        public string FullName { get; set; }
        public string Url { get; set; }
        [Required(ErrorMessage ="Title section must be filled")]
        [MaxLength(100,ErrorMessage = "Title can not be more than 100 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Subtitle section must be filled")]
        [MaxLength(100,ErrorMessage ="Subtitle can not be more than 100 characters.")]
        public string Subtitle { get; set; }
        [Required(ErrorMessage = "Content section must be filled")]
        public string Content { get; set; }

        public string PhotoPath { get; set; }
        public double ReadTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
