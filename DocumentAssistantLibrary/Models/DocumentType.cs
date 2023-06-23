using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of DocumentType entity for database
    /// </summary>
    public class DocumentType
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        /// <summary>
        /// Customer's name
        /// </summary>
        [Required]
        public string TypeName { get; set; }
    }
}
