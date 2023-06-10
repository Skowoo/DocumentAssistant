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

        private void CheckDb()
        {
            var contextOptions = new DbContextOptionsBuilder<MainContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DocumentAssistantDB;TrustServerCertificate=True;",
                options => options.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: System.TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null))
            .Options;

            try
            {
                context = new MainContext(contextOptions);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (!context.Database.CanConnect())
                MessageBox.Show("Baza danych nie istnieje!");
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
            var main = new MainWindow(/*userLevel*/);
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
    }
}
