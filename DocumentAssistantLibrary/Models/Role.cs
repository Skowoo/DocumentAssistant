using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of Role entity for database
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }

        /// <summary>
        /// Customer's name
        /// </summary>
        [Required]
        public string RoleName { get; set; }
    }
}
