using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WpfApp.Models.ViewModels;

namespace WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        private UserViewModel? selectedUserView;

        private List<User> usersList = new ();

        private ObservableCollection<UserViewModel> usersViews = new();

        public UserManagementWindow()
        {
            InitializeComponent();
            UpdateUsersList();
            UsersDataGrid.ItemsSource = usersViews;
            UserUpdateCommandGrid.IsEnabled = false;
            ResetView();
        }

        private void UpdateUsersList()
        {
            usersList.Clear();
            using (MainContext context = new MainContext())
            {
                usersList = context.Users.ToList();
            }
            usersViews.Clear();
            foreach (var user in usersList)
                usersViews.Add(new UserViewModel(user));
        }

        private void ResetView()
        {
            selectedUserView = null;
            UpdateUsersList();
            UserUpdateCommandGrid.IsEnabled = false;
            UsersDataGrid.SelectedItem = null;
            UserUpdateCommandGrid.Visibility = Visibility.Visible;
            ChangeLoginGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ChangeRoleGrid.Visibility = Visibility.Hidden;
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                UserUpdateCommandGrid.IsEnabled = true;
                selectedUserView = UsersDataGrid.SelectedItem as UserViewModel;

                if (selectedUserView.IsActive)
                    ActivateUserButton.Content = "Zdezaktywuj użytkownika";
                else
                    ActivateUserButton.Content = "Aktywuj użytkownika";
            }
        }

        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeLoginGrid.Visibility = Visibility.Visible;
            LoginChangeDescription.Text = $"Zmiana loginu użytkownika: \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
            NewLoginTextBox.Text = selectedUserView.Login;
        }

        private void ConfirmLoginChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MainContext context = new MainContext())
                {
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().Login = NewLoginTextBox.Text;
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            ResetView();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Visible;
            PasswordChangeDescription.Text = $"Zmiana hasła użytkownika: \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
        }

        private void ConfirmPasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Hasła nie są jednakowe!");
                return;
            }
            if (NewPasswordBox.Password.Length < 4)
            {
                MessageBox.Show("Hasło za krótkie! minimalny rozmiar to 4 znaki");
                return;
            }

            string salt = PassGenerator.GenerateSalt();

            try
            {
                using (MainContext context = new MainContext())
                {
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().Salt = salt;
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().Password = PassGenerator.ComputeHash(NewPasswordBox.Password, salt);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            ResetView();
        }

        private void ChangeRankButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeRoleGrid.Visibility = Visibility.Visible;
            RoleChangeDescription.Text = $"Zmiana roli użytkownika: \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
            List<Role> tempRolesList;
            using (MainContext context = new MainContext())
            {                
                tempRolesList = context.Roles.ToList();
            }
            ObservableCollection<RoleViewModel> RolesViewModelsList = new();
            foreach (var item in  tempRolesList)
                RolesViewModelsList.Add(new RoleViewModel(item));

            RoleSelectionComboBox.ItemsSource = RolesViewModelsList;
            RoleSelectionComboBox.SelectedItem = RolesViewModelsList.Where(x => x.RoleID == selectedUserView.RoleID).Single();
        }

        private void ConfirmRoleChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRole = RoleSelectionComboBox.SelectedItem as RoleViewModel;
            try
            {
                using (MainContext context = new MainContext())
                {
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().RoleID = selectedRole.RoleID;
                }
            } 
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            ResetView();
        }

        private void ActivateUserButton_Click(object sender, RoutedEventArgs e)
        {
            using (MainContext context = new MainContext())
            {
                try
                {
                    if (selectedUserView.IsActive)
                        context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().IsActive = false;
                    else
                        context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().IsActive = true;

                    context.SaveChanges();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                ResetView();
            }
        }
    }
}
