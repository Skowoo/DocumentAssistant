using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of User entity for database
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// User's last name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// User's login
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Salt for User's password
        /// </summary>
        [Required]
        public string Salt { get; set; }

        /// <summary>
        /// Boolean IsActive - true if user is activated and can log in
        /// </summary>
        [Required]
        public bool IsActive { get; set; }

        /// <summary>
        /// Assigned Role ID
        /// </summary>
        //Foreign keys
        [Display(Name = "Role")]
        public virtual int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role Roles { get; set; }
    }
}
