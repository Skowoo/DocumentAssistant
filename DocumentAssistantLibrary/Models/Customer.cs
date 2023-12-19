using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of Customer entity for database
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerID { get; set; }

        /// <summary>
        /// Customer's name
        /// </summary>
        [Required]
        public string CustomerName { get; set; } = default!;
    }
}
