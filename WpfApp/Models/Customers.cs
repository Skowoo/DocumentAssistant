using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WpfApp.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        public string CustomerName { get; set; }
    }
}
