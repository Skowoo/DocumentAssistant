using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private List<User> usersList = new();

        private ObservableCollection<UserViewModel> usersViews = new();

        public UserManagementWindow()
        {
            InitializeComponent();
            UpdateUsersList();
            UsersDataGrid.ItemsSource = usersViews;
            UserUpdateCommandGrid.IsEnabled = false;
            ResetView();
        }

        #region Controls

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                UserUpdateCommandGrid.IsEnabled = true;
                selectedUserView = UsersDataGrid.SelectedItem as UserViewModel;

                CancelButton.Visibility = Visibility.Visible;

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
            if (NewLoginTextBox.Text.Length < 2)
            {
                MessageBox.Show("Login musi składać się z conajmniej dwóch znaków!");
                return;
            }

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

        private void ChangeNamesButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeNamesGrid.Visibility = Visibility.Visible;
            NamesChangeDescription.Text = $"Zmiana danych osobowych: \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
            NewFirstNameTextBox.Text = selectedUserView.FirstName;
            NewLastNameTextBox.Text = selectedUserView.LastName;
        }

        private void ConfirmNamesChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewFirstNameTextBox.Text.Length < 2 || NewLastNameTextBox.Text.Length < 2)
            {
                MessageBox.Show("Imię i Nazwisko muszą składać się z conajmniej dwóch znaków!");
                return;
            }

            try
            {
                using (MainContext context = new MainContext())
                {
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().FirstName = NewFirstNameTextBox.Text;
                    context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().LastName = NewLastNameTextBox.Text;
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
            foreach (var item in tempRolesList)
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

        private void CancelButton_Click(object sender, RoutedEventArgs e) => ResetView();

        #endregion

        #region Private methods
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
            CancelButton.Visibility = Visibility.Hidden;
            UserUpdateCommandGrid.Visibility = Visibility.Visible;
            ChangeLoginGrid.Visibility = Visibility.Hidden;
            ChangeNamesGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ChangeRoleGrid.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}
