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
            //ResetView();
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
            ExpandDocGrid();
            MainControlButtonsGrid.Visibility = Visibility.Visible;
        }

        private void ShrinkDocGrid()
        {
            DocGrid.Margin = new Thickness(0, 20, 900, 0);
            docUserColumn.Visibility = Visibility.Collapsed;
            docTimeDoneColumn.Visibility = Visibility.Collapsed;
            docDeadlineColumn.Visibility = Visibility.Collapsed;
            docTimeAddedColumn.Visibility = Visibility.Collapsed;
            docCustomerIDColumn.Visibility = Visibility.Collapsed;
        }

        private void ExpandDocGrid()
        {
            DocGrid.Margin = new Thickness(0, 20, 300, 0);
            docUserColumn.Visibility = Visibility.Visible;
            docTimeDoneColumn.Visibility = Visibility.Visible;
            docDeadlineColumn.Visibility = Visibility.Visible;
            docTimeAddedColumn.Visibility = Visibility.Visible;
            docCustomerIDColumn.Visibility = Visibility.Visible;
        }
    }
}
