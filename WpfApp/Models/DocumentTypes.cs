using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Models
{
    public class DocumentTypes
    {
        [Key]
        public int TypeID { get; set; }

        [Required]
        public string TypeName { get; set; }
    }
}
