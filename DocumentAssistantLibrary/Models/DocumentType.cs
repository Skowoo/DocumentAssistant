using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    public class DocumentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
