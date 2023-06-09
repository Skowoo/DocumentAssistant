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
            using (var db = new MainContext())
            {
                var salt = PassGenerator.GenerateSalt();
                var adminRole = new Role { 
                    RoleID = 1,
                    RoleName = "Admin" };
                var adminUser = new User {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "Admin",
                    Password = PassGenerator.ComputeHash("Admin", salt),
                    Salt = salt,
                    IsActive = true,
                    RoleID = 1 };
                
                db.Roles.Add(adminRole);
                db.Users.Add(adminUser);
                db.SaveChanges();
            }
        }
    }
}
