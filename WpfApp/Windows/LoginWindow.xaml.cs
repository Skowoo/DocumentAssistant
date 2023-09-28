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

        CultureInfo culture;

        public LoginWindow() : this(null) { }
        public LoginWindow(CultureInfo? culture = null)
        {
            if (culture != null)
            {
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
                this.culture = culture;
            }
            else
                this.culture = CultureInfo.CurrentCulture;
                
            InitializeComponent();
            CheckDb();
        }

        #region Controls

        private void SelectLanguageEng_Click(object sender, RoutedEventArgs e)
        {
            culture = new CultureInfo("en-EN");
            var newWindow = new LoginWindow(culture);
            newWindow.Show();
            this.Close();
        }

        private void SelectLanguagePol_Click(object sender, RoutedEventArgs e)
        {
            culture = new CultureInfo("pl-PL");
            var newWindow = new LoginWindow(culture);
            newWindow.Show();
            this.Close();
        }

        private void SelectLanguageJap_Click(object sender, RoutedEventArgs e)
        {
            culture = new CultureInfo("jp-JP");
            var newWindow = new LoginWindow(culture);
            newWindow.Show();
            this.Close();
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
                MessageBox.Show(Strings.LoginNotFound);
                LoginTextBox.Text = "";
                PasswordBox.Password = "";
                return;
            }
            var password = PassGenerator.ComputeHash(PasswordBox.Password, foundUser.Salt);

            if (password == foundUser.Password && foundUser.IsActive == true) GrantAccess(foundUser);
            else MessageBox.Show(Strings.LoginDataNotCorrect);
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
                MessageBox.Show(Strings.LoginFailed);
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
                newDbCreateWindowDecision = MessageBox.Show(Strings.NoDbBoxText, Strings.NoDbBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);
            bool dbCreated = false;

            if (newDbCreateWindowDecision == MessageBoxResult.Yes)
            {
                dbCreated = DbCreator.CreateNewDb();

                if (dbCreated)
                    MessageBox.Show(Strings.DbCreatedBoxText);
                else
                    MessageBox.Show(Strings.DbNotCreatedBoxText);
            }
        }

        #endregion
    }
}
