using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Policy;
using WpfApp.Models;

namespace WpfApp
{
    public class MainContext : DbContext
    {
        private static DbContextOptionsBuilder<MainContext> defaultOptions => new DbContextOptionsBuilder<MainContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DocumentAssistantDB;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                    );

        public MainContext() : base(defaultOptions.Options) { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Language> Languages { get; set; }
    }
}
