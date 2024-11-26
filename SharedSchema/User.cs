using System;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace SAASLMS.SharedSchema
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; } // Foreign key

        //[Required]
        //public int MaxUserLimit { get; set; } = 0;

        public DateTime CreateDate { get; set; }

    }
}
