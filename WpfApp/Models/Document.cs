using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentID { get; set; }

        [Required]
        public DateTime TimeAdded { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public DateTime TimeDone { get; set; }

        [Required]
        public string Name { get; set; }

        //Foreign keys
        [Display(Name = "User")]
        public virtual int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User Users { get; set; }


        [Display(Name = "DocumentType")]
        public virtual int TypeID { get; set; }

        [ForeignKey("TypeID")]
        public virtual DocumentType DocumentTypes { get; set; }

        [Display(Name = "Customer")]
        public virtual int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customers { get; set; }
    }
}
