using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Resources;

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

        private void SelectLanguageEng_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-EN");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-EN");
        }

        private void SelectLanguagePol_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pl-PL");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pl-PL");
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User? foundUser;

            try
            {
                foundUser = (from c in context.Users where c.Login == LoginTextBox.Text select c).Single();
            }
            catch
            {
                MessageBox.Show(strings.LoginNotFound);
                LoginTextBox.Text = "";
                PasswordBox.Password = "";
                return;
            }
            var password = PassGenerator.ComputeHash(PasswordBox.Password, foundUser.Salt);

            if (password == foundUser.Password && foundUser.IsActive == true) GrantAccess(foundUser);
            else MessageBox.Show(strings.LoginDataNotCorrect);
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
                MessageBox.Show(strings.LoginFailed);
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
                newDbCreateWindowDecision = MessageBox.Show(strings.NoDbBoxText, strings.NoDbBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            bool dbCreated = false;

            if (newDbCreateWindowDecision == MessageBoxResult.Yes)
            {
                dbCreated = DbCreator.CreateNewDb();

                if (dbCreated)
                    MessageBox.Show(strings.DbCreatedBoxText);
                else
                    MessageBox.Show(strings.DbNotCreatedBoxText);
            }
        }

        #endregion
    }
}
