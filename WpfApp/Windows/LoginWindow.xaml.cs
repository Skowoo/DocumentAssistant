using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Classes;
using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using DocumentAssistantLibrary.Models.ViewModels;

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

            if (password == user.Password && user.IsActive == true) GrantAccess(user.RoleID);
            else MessageBox.Show("Nieprawidłowe dane logowania. Sprawdź login i hasło");
        }

        private void GrantAccess(int userLevel)
        {
            var main = new MainWindow(userLevel);
            try
            {
                this.Close();
                main.Show();
            }
            catch
            {
                MessageBox.Show("Proces logowania nie powiódł się. Sprawdź czy posiadasz odpowiednie uprawnienia");
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(sender, e);
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
                db.Database.EnsureCreated();

                var salt = PassGenerator.GenerateSalt();
                var adminRole = new Role
                {
                    RoleName = "Admin"
                };
                db.Roles.Add(adminRole);
                db.SaveChanges();

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
                db.Users.Add(adminUser);
                db.SaveChanges();

                db.Roles.Add(new Role { RoleName = "Kierownik" });
                db.Roles.Add(new Role { RoleName = "Koordynator" });
                db.Roles.Add(new Role { RoleName = "Użytkownik" });
                db.Roles.Add(new Role { RoleName = "Obserwator" });

                db.Languages.Add(new Language { LanguageName = "Polski" });
                db.Languages.Add(new Language { LanguageName = "Angielski" });
                db.Languages.Add(new Language { LanguageName = "Japoński" });
                db.Languages.Add(new Language { LanguageName = "Wietnamski" });
                db.Languages.Add(new Language { LanguageName = "Chiński" });

                db.SaveChanges();

                MessageBox.Show("Utworzono nową bazę danych z kontem administratora. \nLogin: Admin \nHasło: Admin  \nMożesz się zalogować z wykorzystaniem powyższych danych.");
            }
        }
    }
}
