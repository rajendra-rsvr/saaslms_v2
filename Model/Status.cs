using System.ComponentModel.DataAnnotations;

namespace SAASLMS.Model
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string StatusName { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
