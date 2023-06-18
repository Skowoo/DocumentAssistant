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

        DocumentViewModel? selectedDocument;

        public MainWindow()
        {
            InitializeComponent();
            UpdateLists();
            AssignItemSources();
            ResetView();
        }

        private void AssignItemSources()
        {
            //Main view
            DocGrid.ItemsSource = documentViewsList;
            AssignUserMainMenu_ComboBox.ItemsSource = userViewModelsList;

            //New document grid
            NewDocType_ComboBox.ItemsSource = documentTypeViewModelsList;
            NewDocCustomer_ComboBox.ItemsSource = customerViewModelsList;
            NewDocUser_ComboBox.ItemsSource = userViewModelsList;

            //Edit document grid
            EditDocType_ComboBox.ItemsSource = documentTypeViewModelsList;
            EditDocCustomer_ComboBox.ItemsSource = customerViewModelsList;
            EditDocUser_ComboBox.ItemsSource = userViewModelsList;
        }

        private void DocGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDocument = DocGrid.SelectedItem as DocumentViewModel;
            if (selectedDocument is not null) 
            {
                if (selectedDocument.IsConfirmed)
                    ConfirmDoneBtn.Content = "Anuluj zatwierdzenie";
                else 
                    ConfirmDoneBtn.Content = "Zatwierdź dokument";
            }
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

            AssignUserMainMenu_ComboBox.Visibility = Visibility.Collapsed;
            AssignUserMainMenuConfirm_Button.Visibility = Visibility.Collapsed;

            AddDocumentGrid.Visibility = Visibility.Hidden;
            NewTypeGrid.Visibility = Visibility.Hidden;
            NewCustomerGrid.Visibility = Visibility.Hidden;
            EditDocGrid.Visibility = Visibility.Hidden;
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
                Deadline = (DateTime)DeadlineCallendar.SelectedDate,
                IsConfirmed = false
            };

            var tempCustomer = NewDocCustomer_ComboBox.SelectedItem as CustomerViewModel;
            newDocument.CustomerID = tempCustomer.CustomerID;

            var tempType = NewDocType_ComboBox.SelectedItem as DocumentTypeViewModel;
            newDocument.TypeID = tempType.TypeID;

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

            UpdateLists();
            ResetView();
        }

        private void NewTypeDocGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            NewCustomerGrid.Visibility = Visibility.Hidden;
            NewTypeGrid.Visibility = Visibility.Visible;
        }

        private void NewCustomerDocGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            NewTypeGrid.Visibility = Visibility.Hidden;
            NewCustomerGrid.Visibility = Visibility.Visible;
        }

        private void ConfirmNewType_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewType_TextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show("Minimalne długośc typu dokumentu to dwa znaki!");
                return;
            }

            DocumentType newType = new DocumentType
            {
                TypeName = NewType_TextBox.Text.Trim()
            };

            using (MainContext context = new MainContext())
            {
                context.DocumentTypes.Add(newType);
                context.SaveChanges();
            }

            UpdateLists();
            NewDocType_ComboBox.SelectedItem = documentTypeViewModelsList.Where(x => x.TypeName == newType.TypeName).Single();
            NewTypeGrid.Visibility = Visibility.Hidden;
        }

        private void ConfirmNewCustomer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewCustomer_TextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show("Minimalne długośc nazwy klienta to dwa znaki!");
                return;
            }

            Customer newCustomer = new Customer
            {
                CustomerName = NewCustomer_TextBox.Text.Trim()
            };

            using (MainContext context = new MainContext())
            {
                context.Customers.Add(newCustomer);
                context.SaveChanges();
            }

            UpdateLists();
            NewDocCustomer_ComboBox.SelectedItem = customerViewModelsList.Where(x => x.CustomerName == newCustomer.CustomerName).Single();
            NewCustomerGrid.Visibility = Visibility.Hidden;
        }

        private void EditDocBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            DocGrid.Visibility = Visibility.Hidden;
            MainControlButtonsGrid.Visibility = Visibility.Hidden;
            EditDocGrid.Visibility = Visibility.Visible;

            EditDocTitleLabel.Content = $"ID: {selectedDocument.DocumentID}, {selectedDocument.Name}";
            EditDocName_TextBox.Text = selectedDocument.Name;
            EditDocSize_TextBox.Text = selectedDocument.signsSize.ToString();
            EditDocType_ComboBox.SelectedItem = documentTypeViewModelsList.Where(x => x.TypeID == selectedDocument.TypeID).Single();
            EditDocCustomer_ComboBox.SelectedItem = customerViewModelsList.Where(x => x.CustomerID == selectedDocument.CustomerID).Single();
            EditDocDeadlineCallendar.SelectedDate = DateTime.Parse(selectedDocument.Deadline);

            if (selectedDocument.UserID is not null)
                EditDocUser_ComboBox.SelectedItem = userViewModelsList.Where(x => x.UserID == selectedDocument.UserID).Single();

            if (selectedDocument.TimeDone is not null)
                EditDocTimeDoneCallendar.SelectedDate = DateTime.Parse(selectedDocument.TimeDone);

            EditDocGridConfirmed_CheckBox.IsChecked = selectedDocument.IsConfirmed;
        }

        private void ConfirmEditDocButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            var tempSelectedDocType = EditDocType_ComboBox.SelectedItem as DocumentTypeViewModel;
            var tempSelectedDocCustomer = EditDocCustomer_ComboBox.SelectedItem as CustomerViewModel;
            UserViewModel? tempSelectedUser = null;

            if (EditDocUser_ComboBox.SelectedItem is not null)
                tempSelectedUser = EditDocUser_ComboBox.SelectedItem as UserViewModel;

            using (MainContext context = new MainContext())
            {
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();
                editedDocument.Name = EditDocName_TextBox.Text.Trim();
                editedDocument.signsSize = Int32.Parse(EditDocSize_TextBox.Text.Trim());
                editedDocument.CustomerID = tempSelectedDocCustomer.CustomerID;
                editedDocument.TypeID = tempSelectedDocType.TypeID;
                editedDocument.IsConfirmed = (bool)EditDocGridConfirmed_CheckBox.IsChecked;

                if (EditDocDeadlineCallendar.SelectedDate is not null)
                    editedDocument.Deadline = (DateTime)EditDocDeadlineCallendar.SelectedDate;

                if (tempSelectedUser is not null)
                    editedDocument.UserID = tempSelectedUser.UserID;

                if (EditDocTimeDoneCallendar.SelectedDate is not null)
                    editedDocument.TimeDone = EditDocTimeDoneCallendar.SelectedDate;

                context.SaveChanges();
            }

            UpdateLists();
            ResetView();
        }

        private void DeleteDocBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            using (MainContext context = new MainContext())
            {
                context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).ExecuteDelete();
                context.SaveChanges();
            }

            UpdateLists();
        }

        private void MarkAsDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            using (MainContext context = new MainContext())
            {
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();

                if (editedDocument.TimeDone is not null)
                    return;

                editedDocument.TimeDone = DateTime.Now;
                context.SaveChanges();
            }

            UpdateLists();
        }

        private void AssignDocumentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            AssignUserMainMenu_ComboBox.Visibility = Visibility.Visible;
            AssignUserMainMenuConfirm_Button.Visibility = Visibility.Visible;
        }

        private void AssignUserMainMenuConfirm_Button_Click(object sender, RoutedEventArgs e)
        {
            AssignUserMainMenu_ComboBox.Visibility = Visibility.Collapsed;
            AssignUserMainMenuConfirm_Button.Visibility = Visibility.Collapsed;

            if (selectedDocument is null || AssignUserMainMenu_ComboBox.SelectedItem is null) 
                return;

            using (MainContext context = new MainContext())
            {
                var selectedUser = AssignUserMainMenu_ComboBox.SelectedItem as UserViewModel;
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();
                editedDocument.UserID = selectedUser.UserID;
                context.SaveChanges();
            }

            UpdateLists();
        }

        private void ConfirmDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            using(MainContext context = new MainContext())
            {
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();

                if (editedDocument.IsConfirmed)
                    editedDocument.IsConfirmed = false;
                else
                    editedDocument.IsConfirmed = true;

                context.SaveChanges();
            }

            UpdateLists();
        }
    }
}
