using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class ArticleDetailsAndUserDTO
    {
        public ArticleDTO ArticleDTO { get; set; }
        public UserDTO UserDTO { get; set; }
    }
}
