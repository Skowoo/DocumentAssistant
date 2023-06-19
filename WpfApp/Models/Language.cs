using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models
{
    public class Language
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LanguageID { get; set; }

        [Required]
        public string LanguageName { get; set; }

        //Inversed properties

        [InverseProperty("OriginalLanguage")]
        public ICollection<Document> OriginalDocuments { get; set; }

        [InverseProperty("TargetLanguage")]
        public ICollection<Document> TargetDocuments { get; set; }
    }
}
