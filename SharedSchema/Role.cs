using SAASLMS.SharedSchema;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SAASLMS.SharedSchema
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        [StringLength(500)]
        public string RoleDescription { get; set; }
    }
}
