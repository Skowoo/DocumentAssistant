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
        MainContext? context;

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

            CultureInfo.CurrentCulture = this.culture;
            CultureInfo.CurrentUICulture = this.culture;

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
            culture = new CultureInfo("ja-JP");
            var newWindow = new LoginWindow(culture);
            newWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User? foundUser;

            try
            {
                foundUser = (from c in context!.Users where c.Login == LoginTextBox.Text select c).Single();
            }
            catch
            {
                MessageBox.Show(Text.LoginNotFound);
                LoginTextBox.Text = "";
                PasswordBox.Password = "";
                return;
            }
            var password = PassGenerator.ComputeHash(PasswordBox.Password, foundUser.Salt);

            if (password == foundUser.Password && foundUser.IsActive == true) GrantAccess(foundUser);
            else MessageBox.Show(Text.LoginDataNotCorrect);
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
                MessageBox.Show(Text.LoginFailed);
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
                this.Close();
            }

            MessageBoxResult newDbCreateWindowDecision = MessageBoxResult.None;

            if (!context!.Database.CanConnect())
                newDbCreateWindowDecision = MessageBox.Show(Text.NoDbBoxText, Text.NoDbBoxTitle,
                                                            MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (newDbCreateWindowDecision == MessageBoxResult.Yes)
            {
                if (DbCreator.CreateNewDb())
                    MessageBox.Show(Text.DbCreatedBoxText);
                else
                {
                    MessageBox.Show(Text.DbNotCreatedBoxText);
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }
        #endregion
    }
}
