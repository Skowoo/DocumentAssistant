using DocumentAssistantLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAssistantLibrary
{
    /// <summary>
    /// Main context to connect and operate on DocumentAssistantDB
    /// </summary>
    public class MainContext : DbContext
    {
        private static DbContextOptionsBuilder<MainContext> defaultOptions => new DbContextOptionsBuilder<MainContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DocumentAssistantDB;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                    );

        /// <summary>
        /// Parameterless constructor - uses defaultOptions specified inside MainContext class
        /// </summary>
        public MainContext() : base(defaultOptions.Options) { }

        /// <summary>
        /// List of Customers stored in DB
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// List of Documents stored in DB
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// List of Document types stored in DB
        /// </summary>
        public DbSet<DocumentType> DocumentTypes { get; set; }

        /// <summary>
        /// List of Roles stored in DB
        /// </summary>
        public DbSet<Role> Roles { get; set; }

        /// <summary>
        /// List of Users stored in DB
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// List of Languages stored in DB
        /// </summary>
        public DbSet<Language> Languages { get; set; }
    }
}
