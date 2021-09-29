using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class TopicDTO
    {
        public int TopicID { get; set; }
        [Required]
        public string TopicName { get; set; }
        public string Description { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
