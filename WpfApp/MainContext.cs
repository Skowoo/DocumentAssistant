using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Policy;
using WpfApp.Models;

namespace WpfApp
{
    public class MainContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
