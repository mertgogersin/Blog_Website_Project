using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog_Website_Project.Models.DTOs
{
    public class LoginDTO
    {
        public Guid UserID { get; set; }
        [Required(ErrorMessage ="Email section needs to be filled.")]
        [MaxLength(40, ErrorMessage = "Email address cannot be longer than 40 characters.")]
        [DataType(DataType.EmailAddress,ErrorMessage = "Please fill the section according to email format.")]
        public string Email { get; set; }
    }
}
