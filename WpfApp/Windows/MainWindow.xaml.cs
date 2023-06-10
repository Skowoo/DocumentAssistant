using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WpfApp.Models;
using WpfApp.Classes;
using Microsoft.EntityFrameworkCore;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeDb();
        }

        public void InitializeDb()
        {
            var contextOptions = new DbContextOptionsBuilder<MainContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DocumentAssistantDB;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: System.TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null)
                    )
                .Options;

            using (var db = new MainContext(contextOptions))
            {
                var salt = PassGenerator.GenerateSalt();
                var adminRole = new Role { 
                    RoleName = "Admin" 
                };
                var adminUser = new User {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "Admin",
                    Password = PassGenerator.ComputeHash("Admin", salt),
                    Salt = salt,
                    IsActive = true
                };

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Roles.Add(adminRole);
                db.SaveChanges();

                adminUser.RoleID = db.Roles.Single().RoleID;
                db.Users.Add(adminUser);   
                db.SaveChanges();
            }
        }
    }
}
