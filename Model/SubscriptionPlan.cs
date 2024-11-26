using System.ComponentModel.DataAnnotations;

namespace SAASLMS.Model
{
    public class SubscriptionPlan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Plan { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int MaximumUsersUpperLimit { get; set; }
    }
}
