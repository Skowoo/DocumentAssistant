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
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                UserUpdateCommandGrid.IsEnabled = true;
                selectedUserView = UsersDataGrid.SelectedItem as UserViewModel;
            }
        }

        private void ChangeLoginButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
            ChangeLoginGrid.Visibility = Visibility.Visible;
            LoginChangeDescription.Text = $"Zmiana loginu: \n{selectedUserView.Login} - {selectedUserView.FirstName} {selectedUserView.LastName} (ID: {selectedUserView.UserID})";
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
        }

        private void ChangeRankButton_Click(object sender, RoutedEventArgs e)
        {
            UserUpdateCommandGrid.Visibility = Visibility.Hidden;
        }

        private void ActivateUserButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
