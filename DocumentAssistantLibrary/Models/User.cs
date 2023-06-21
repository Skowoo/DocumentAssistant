using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        //Foreign keys
        [Display(Name = "Role")]
        public virtual int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Roles { get; set; }
    }
}
