using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class DeletedUser
    {
        [Key]
        public int ID { get; set; }
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Url { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime DeletedDate { get; set; }
        public DateTime? ActivationDate { get; set; }
    }
}
