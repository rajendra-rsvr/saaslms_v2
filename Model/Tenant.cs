using System.ComponentModel.DataAnnotations;

namespace SAASLMS.Model
{
    public class Tenant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenantName { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Domain { get; set; }

        [Required]
        public int StatusId { get; set; }  = 0;

        [Required]
        public int SubscriptionPlanId { get; set; } = 0;

        [StringLength(200)]
        public string SiteUrl { get; set; }

        public DateTime CreateDate { get; set; }

        
    }
}
