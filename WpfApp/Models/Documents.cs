using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Models
{
    public class Documents
    {
        [Key]
        public int DocumentID { get; set; }

        [Required]
        public DateTime TimeAdded { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        public DateTime TimeDone { get; set; }

        [Required]
        public string Name { get; set; }

        //Foreign keys
        [Display(Name = "Users")]
        public virtual int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual Users User { get; set; }


        [Display(Name = "DocumentTypes")]
        public virtual int TypeID { get; set; }

        [ForeignKey("TypeID")]
        public virtual DocumentTypes DocumentType { get; set; }

        [Display(Name = "Customers")]
        public virtual int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customers Customer { get; set; }
    }
}
