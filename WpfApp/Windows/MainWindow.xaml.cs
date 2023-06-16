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
using System.Windows.Navigation;
using WpfApp.Models;
using WpfApp.Classes;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using WpfApp.Models.ViewModels;
using WpfApp.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Document> objectsList = new();

        ObservableCollection<DocumentViewModel> viewList = new();

        public MainWindow()
        {
            InitializeComponent();
            UpdateDocumentsList();
            DocGrid.ItemsSource = viewList;
            ResetView();
        }

        private void DocGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DocumentViewModel doc = DocGrid.SelectedItem as DocumentViewModel;
        }

        private void Menu_ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            var window = new UserManagementWindow();
            window.Show();
        }

        private void UpdateDocumentsList()
        {
            objectsList.Clear();
            using (MainContext context = new MainContext())
            {
                objectsList = context.Documents.ToList();
            }
            viewList.Clear();
            foreach (var document in objectsList)
                viewList.Add(new DocumentViewModel(document));
        }

        private void ResetView()
        {
            DocGrid.Visibility = Visibility.Visible;
            MainControlButtonsGrid.Visibility = Visibility.Visible;
            AddDocumentGrid.Visibility = Visibility.Hidden;
        }

        private void AddDocumentBtn_Click(object sender, RoutedEventArgs e)
        {
            DocGrid.Visibility = Visibility.Hidden;
            MainControlButtonsGrid.Visibility = Visibility.Hidden;
            AddDocumentGrid.Visibility = Visibility.Visible;
        }

        private void ConfirmNewDocButton_Click(object sender, RoutedEventArgs e)
        {


            ResetView();
        }
    }
}
