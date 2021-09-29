using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid UserID { get; set; }
        [MaxLength(20)]
        [Required]
        public string UserName { get; set; }
        [MaxLength(40)]
        [Required]
        public string Email { get; set; }
        [MaxLength(40)]
        [Required]
        public string FullName { get; set; }
        [MaxLength(20)]
        [Required]
        public string Url { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public virtual ICollection<UserTopic> UserTopics { get; set; }
    }
}
