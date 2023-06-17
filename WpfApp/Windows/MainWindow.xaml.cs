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
        List<Document> documentsList = new();

        ObservableCollection<DocumentViewModel> documentViewsList = new();

        List<User> usersList = new();

        ObservableCollection<UserViewModel> userViewModelsList = new();

        List<Customer> customersList = new();

        ObservableCollection<CustomerViewModel> customerViewModelsList = new();

        List<DocumentType> documentTypesList = new();

        ObservableCollection<DocumentTypeViewModel> documentTypeViewModelsList = new();

        public MainWindow()
        {
            InitializeComponent();
            UpdateLists();
            DocGrid.ItemsSource = documentViewsList;
            NewDocType_ComboBox.ItemsSource = documentTypeViewModelsList;
            NewDocCustomer_ComboBox.ItemsSource = customerViewModelsList;
            NewDocUser_ComboBox.ItemsSource = userViewModelsList;

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

        private void UpdateLists()
        {
            documentsList.Clear();
            usersList.Clear();
            customersList.Clear();
            documentTypesList.Clear();

            using (MainContext context = new MainContext())
            {
                documentsList = context.Documents.ToList();
                usersList = context.Users.ToList();
                customersList = context.Customers.ToList();
                documentTypesList = context.DocumentTypes.ToList();
            }

            documentViewsList.Clear();
            userViewModelsList.Clear();
            customerViewModelsList.Clear();
            documentTypeViewModelsList.Clear();

            foreach (var document in documentsList)
                documentViewsList.Add(new DocumentViewModel(document));

            foreach (var user in usersList)
                userViewModelsList.Add(new UserViewModel(user));

            foreach (var customer in customersList)
                customerViewModelsList.Add(new CustomerViewModel(customer));

            foreach (var documentType in documentTypesList)
                documentTypeViewModelsList.Add(new DocumentTypeViewModel(documentType));
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
            if (NewDocName_TextBox.Text.Trim().Length < 3) {
                MessageBox.Show("Nazwa dokumentu musi mieć conajmniej 3 znaki!");
                return; }

            bool sizeParsed = Int32.TryParse(NewDocSize_TextBox.Text, out int docSize);
            if (!sizeParsed) {
                MessageBox.Show("Podano niepoprawny rozmiar dokumentu!");
                return; }

            if (NewDocType_ComboBox.SelectedItem is null) {
                MessageBox.Show("Nie wybrano typu dokumentu!");
                return; }

            if (NewDocCustomer_ComboBox.SelectedItem is null) {
                MessageBox.Show("Nie przypisano zleceniodawcy do dokumentu!");
                return; }

            if (DeadlineCallendar.SelectedDate is null) {
                MessageBox.Show("Nie wybrano terminu wykonania dokumentu!");
                return; }

            Document newDocument = new Document
            {
                TimeAdded = DateTime.Now,
                Name = NewDocName_TextBox.Text.Trim(),
                signsSize = docSize,
                Deadline = (DateTime)DeadlineCallendar.SelectedDate
            };

            if (NewDocUser_ComboBox.SelectedItem is not null)
            {
                var tempUser = NewDocUser_ComboBox.SelectedItem as UserViewModel;
                newDocument.UserID = tempUser.UserID;
            }
                
            using (MainContext context = new MainContext())
            {
                context.Documents.Add(newDocument);
                context.SaveChanges();
            }

            ResetView();
        }
    }
}
