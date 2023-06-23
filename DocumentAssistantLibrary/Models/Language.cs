using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentAssistantLibrary.Models
{
    /// <summary>
    /// Model of Language entity for database
    /// </summary>
    public class Language
    {
        /// <summary>
        /// Unique ID - primary key
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageID { get; set; }

        /// <summary>
        /// Language name
        /// </summary>
        [Required]
        public string LanguageName { get; set; }

        /// <summary>
        /// Collection of Documents on which language was marked as original
        /// </summary>
        //Inversed properties
        [InverseProperty("OriginalLanguage")]
        public ICollection<Document> OriginalDocuments { get; set; }

        /// <summary>
        /// Collection of Documents on which language was marked as target
        /// </summary>
        [InverseProperty("TargetLanguage")]
        public ICollection<Document> TargetDocuments { get; set; }
    }
}
