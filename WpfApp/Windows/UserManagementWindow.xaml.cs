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
        private List<User> usersList = new ();

        private ObservableCollection<UserViewModel> usersViews = new();

        private int SelectedUserID;

        public UserManagementWindow()
        {
            InitializeComponent();
            UpdateUsersList();
            UsersDataGrid.ItemsSource = usersViews;
            EditUserButton.IsEnabled = false;
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

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem != null)
            {
                EditUserButton.IsEnabled = true;
                UserViewModel temp = UsersDataGrid.SelectedItem as UserViewModel;
                SelectedUserID = temp.UserID;
            }
        }
    }
}
