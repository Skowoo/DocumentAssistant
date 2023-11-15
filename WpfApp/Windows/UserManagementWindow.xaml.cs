using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using DocumentAssistantLibrary.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp.Resources;

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
            ResetView();
        }

        #region Controls

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                UserUpdateCommandGrid.Visibility = Visibility.Visible;
                selectedUserView = UsersDataGrid.SelectedItem as UserViewModel;

                CancelButton.Visibility = Visibility.Visible;
                AddUserButton.Visibility = Visibility.Hidden;

                if (selectedUserView.IsActive)
                    ActivateUserButton.Content = Text.DeactivateUser;
                else
                    ActivateUserButton.Content = Text.ActivateUser;
            }
        }

        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeLoginGrid.Visibility = Visibility.Visible;
            LoginChangeDescription.Text = $"{Text.ChangingUserLoginColon} \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
            NewLoginTextBox.Text = selectedUserView.Login;
        }

        private void ConfirmLoginChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewLoginTextBox.Text.Length < 2)
            {
                MessageBox.Show(Text.LoginMinLengthWarning);
                return;
            }

            try
            {
                using (MainContext context = new MainContext())
                {
                    if (context.Users.Where(x => x.Login == NewLoginTextBox.Text.Trim()).Any())
                    {
                        MessageBox.Show(Text.LoginAlreadyExistsWarning);
                        return;
                    }
                    else
                    {
                        context.Users.Where(x => x.UserID == selectedUserView.UserID).Single().Login = NewLoginTextBox.Text.Trim();
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            ResetView();
        }

        private void ChangeNamesButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeNamesGrid.Visibility = Visibility.Visible;
            NamesChangeDescription.Text = $"{Text.ChangingPersonalDataColon} \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
            NewFirstNameTextBox.Text = selectedUserView.FirstName;
            NewLastNameTextBox.Text = selectedUserView.LastName;
        }

        private void ConfirmNamesChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewFirstNameTextBox.Text.Length < 2 || NewLastNameTextBox.Text.Length < 2)
            {
                MessageBox.Show(Text.NamesMinLengthWarning);
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
            PasswordChangeDescription.Text = $"{Text.ChangingPasswordColon} \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
        }

        private void ConfirmPasswordChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show(Text.PasswordsNotSameWarning);
                return;
            }
            if (NewPasswordBox.Password.Length < 4)
            {
                MessageBox.Show(Text.PasswordTooShortWarning);
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
            RoleChangeDescription.Text = $"{Text.ChangingUserRoleColon} \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
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
                    context.SaveChanges();
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

        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserButton.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Visible;
            AddNewUserGrid.Visibility = Visibility.Visible;

            List<Role> tempRolesList;
            using (MainContext context = new MainContext())
            {
                tempRolesList = context.Roles.ToList();
            }
            ObservableCollection<RoleViewModel> RolesViewModelsList = new();
            foreach (var item in tempRolesList)
                RolesViewModelsList.Add(new RoleViewModel(item));

            RoleComboBox.ItemsSource = RolesViewModelsList;
        }

        private void ConfirmNewUserButton_Click(object sender, RoutedEventArgs e)
        {
            if (FirstPasswordBox.Password != SecondPasswordBox.Password)
            {
                MessageBox.Show(Text.PasswordsNotSameWarning);
                return;
            }

            if (FirstPasswordBox.Password.Length < 4)
            {
                MessageBox.Show(Text.PasswordTooShortWarning);
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show(Text.NoRoleChosenWarning);
                return;
            }

            if (LoginTextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show(Text.LoginMinLengthWarning);
                return;
            }

            if (FirstNameTextBox.Text.Trim().Length < 2 || LastNameTextBox.Text.Length < 2)
            {
                MessageBox.Show(Text.NamesMinLengthWarning);
                return;
            }

            var selectedRole = RoleComboBox.SelectedItem as RoleViewModel;
            string salt = PassGenerator.GenerateSalt();
            User newUser = new User
            {
                Login = LoginTextBox.Text.Trim(),
                FirstName = FirstNameTextBox.Text.Trim(),
                LastName = LastNameTextBox.Text.Trim(),
                Salt = salt,
                Password = PassGenerator.ComputeHash(FirstPasswordBox.Password, salt),
                RoleID = selectedRole.RoleID,
                IsActive = true
            };

            try
            {
                using (MainContext context = new())
                {
                    if (context.Users.Where(x => x.Login == newUser.Login).Any())
                    {
                        MessageBox.Show(Text.LoginAlreadyExistsWarning);
                        return;
                    }
                    else
                    {
                        context.Users.Add(newUser);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            ResetView();
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
            UsersDataGrid.SelectedItem = null;
            AddUserButton.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Hidden;
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeLoginGrid.Visibility = Visibility.Hidden;
            ChangeNamesGrid.Visibility = Visibility.Hidden;
            ChangePasswordGrid.Visibility = Visibility.Hidden;
            ChangeRoleGrid.Visibility = Visibility.Hidden;
            AddNewUserGrid.Visibility = Visibility.Hidden;
        }

        #endregion

    }
}
