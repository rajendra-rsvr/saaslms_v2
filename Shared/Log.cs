using SAASLMS.SharedSchema;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAASLMS.SharedSchema
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Action { get; set; } // Description of the action performed

        public string Details { get; set; }

        [Required]
        public int UserId { get; set; } // Foreign key

        public DateTime CreateDate { get; set; }

    }
}
