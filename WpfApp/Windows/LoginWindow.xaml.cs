using Microsoft.EntityFrameworkCore;
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
using System.Windows.Shapes;
using WpfApp.Classes;
using WpfApp.Models;
using System.Windows.Annotations;

namespace WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MainContext context;

        public LoginWindow()
        {
            InitializeComponent();
            CheckDb();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User? user;

            try
            {
                user = (from c in context.Users where c.Login == LoginTextBox.Text select c).Single();
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono loginu w bazie.");
                LoginTextBox.Text = "";
                PasswordBox.Password = "";
                return;
            }
            var password = PassGenerator.ComputeHash(PasswordBox.Password, user.Salt);

            if (password == user.Password && user.IsActive == true) GrantAccess(sender, e, user.RoleID);
            else MessageBox.Show("Nieprawidłowe dane logowania. Sprawdź login i hasło");
        }

        private void GrantAccess(object sender, RoutedEventArgs e, int userLevel)
        {
            var main = new MainWindow();
            try
            {
                main.Show();
                this.Close();
            }
            catch
            {
                MessageBox.Show("Proces logowania nie powiódł się. Sprawdź czy posiadasz odpowiednie uprawnienia");
            }
        }

        private void CheckDb()
        {
            try
            {
                context = new MainContext();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            MessageBoxResult newDbCreateWindowDecision = MessageBoxResult.None;
            if (!context.Database.CanConnect())
                newDbCreateWindowDecision = MessageBox.Show("Baza danych nie odnaleziona! Jeżeli to pierwsze uruchomienie aplikacji wybierz 'tak' aby utworzyć nową bazę danych.",
                                                            "Brak połączenia z bazą danych!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (newDbCreateWindowDecision == MessageBoxResult.Yes)
                CreateNewDb();
        }

        public void CreateNewDb()
        {
            using (var db = new MainContext())
            {
                var salt = PassGenerator.GenerateSalt();
                var adminRole = new Role
                {
                    RoleName = "Admin"
                };
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Login = "Admin",
                    Password = PassGenerator.ComputeHash("Admin", salt),
                    Salt = salt,
                    IsActive = true,
                    RoleID = 1
                };

                db.Database.EnsureCreated();

                db.Roles.Add(adminRole);
                db.SaveChanges();

                db.Users.Add(adminUser);
                db.SaveChanges();

                MessageBox.Show("Utworzono nową bazę danych z kontem administratora. \nLogin: Admin \nHasło: Admin  \nMożesz się zalogować z wykorzystaniem powyższych danych.");
            }
        }
    }
}
