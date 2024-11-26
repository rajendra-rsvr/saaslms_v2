using SAASLMS.SharedSchema;
using System;
using System.ComponentModel.DataAnnotations;

namespace SAASLMS.SharedSchema
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime CreateDate { get; set; }

    }
}
