using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Models
{
    public class Users
    {
        [Key]
        public int userID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required] 
        public string LastName { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserSalt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        //Foreign keys
        [Display(Name = "Roles")]
        public virtual int RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual Roles Role { get; set; }
    }
}
