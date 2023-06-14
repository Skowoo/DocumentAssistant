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

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainContext mainContext = new MainContext();

        List<Document> objectsList = new();

        ObservableCollection<DocumentViewModel> viewList = new();

        public MainWindow()
        {
            InitializeComponent();
            UpdateDocumentsList();
            DocGrid.ItemsSource = viewList;
        }

        private void DocGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                DocumentViewModel doc = DocGrid.SelectedItem as DocumentViewModel;
        }

        private void UpdateDocumentsList()
        {
            objectsList = mainContext.Documents.ToList();
            viewList.Clear();
            foreach (var document in objectsList)
                viewList.Add(new DocumentViewModel(document));
        }
    }
}
