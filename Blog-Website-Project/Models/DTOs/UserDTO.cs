using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
 
    public class UserDTO
    {
        public Guid UserID { get; set; } 
        [Required(ErrorMessage = "You must fill username section.")]
        [MaxLength(20,ErrorMessage = "Username length can not be more than 20 chars.")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="You must fill email section")]
        [MaxLength(40,ErrorMessage = "Email length can not be more than 40 chars.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "You must fill full name section")]
        [MaxLength(40,ErrorMessage ="Fullname can not be more than 40 chars.")]
        public string FullName { get; set; }
        public string PhotoPath { get; set; }
        public IFormFile Picture { get; set; }
        [Required(ErrorMessage = "You must fill url section")]
        [MaxLength(20, ErrorMessage = "Url length can not be more than 20 chars.")]
        public string Url { get; set; }
        public string Description { get; set; }

    }
}
