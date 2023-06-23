using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of Document entity for database
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        /// <summary>
        /// Document name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Document size (number of signs)
        /// </summary>
        [Required]
        public int SignsSize { get; set; }

        /// <summary>
        /// Time when document was introduced to database
        /// </summary>
        [Required]
        public DateTime TimeAdded { get; set; }

        /// <summary>
        /// Deadline for translation of document
        /// </summary>
        [Required]
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Nullable time when document was marked as done
        /// </summary>
        public DateTime? TimeDone { get; set; }

        /// <summary>
        /// Bool which states if document translation is confirmed
        /// </summary>
        [Required]
        public bool IsConfirmed { get; set; }

        //Foreign keys

        /// <summary>
        /// Nullable ID of user assigned to this document
        /// </summary>
        [Display(Name = "User")]
        public virtual int? UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User? Users { get; set; }

        /// <summary>
        /// ID of document type of this document
        /// </summary>
        [Display(Name = "DocumentType")]
        public virtual int TypeID { get; set; }
        [ForeignKey("TypeID")]
        public virtual DocumentType DocumentTypes { get; set; }

        /// <summary>
        /// ID of customer ordering the translation
        /// </summary>
        [Display(Name = "Customer")]
        public virtual int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customers { get; set; }

        //Languages

        /// <summary>
        /// ID of original language of document
        /// </summary>
        [ForeignKey("OriginalLanguageID")]
        public int? OriginalLanguageID { get; set; }
        public Language OriginalLanguage { get; set; }

        /// <summary>
        /// ID of target language of document
        /// </summary>
        [ForeignKey("TargetLanguageID")]
        public int? TargetLanguageID { get; set; }
        public Language TargetLanguage { get; set; }
    }
}
