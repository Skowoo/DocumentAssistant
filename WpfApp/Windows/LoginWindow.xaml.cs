using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        #region Controls

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User? foundUser;

            try
            {
                foundUser = (from c in context.Users where c.Login == LoginTextBox.Text select c).Single();
            }
            catch
            {
                MessageBox.Show("Nie odnaleziono loginu w bazie.");
                LoginTextBox.Text = "";
                PasswordBox.Password = "";
                return;
            }
            var password = PassGenerator.ComputeHash(PasswordBox.Password, foundUser.Salt);

            if (password == foundUser.Password && foundUser.IsActive == true) GrantAccess(foundUser);
            else MessageBox.Show("Nieprawidłowe dane logowania. Sprawdź login i hasło");
        }

        private void GrantAccess(User loggedUser)
        {
            var main = new MainWindow(loggedUser);
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

        #endregion

        #region Methods

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
            bool dbCreated = false;

            if (newDbCreateWindowDecision == MessageBoxResult.Yes)
            {
                dbCreated = DbCreator.CreateNewDb();

                if (dbCreated)
                    MessageBox.Show("Utworzono nową bazę danych z kontem administratora. \nLogin: Admin \nHasło: Admin  \nMożesz się zalogować z wykorzystaniem powyższych danych.");
                else
                    MessageBox.Show("Nie udało się utworzyć bazy danych. Sprawdź czy na urządzeniu jest zainstalowane wymagane środowisko!");
            }
        }

        #endregion
    }
}
