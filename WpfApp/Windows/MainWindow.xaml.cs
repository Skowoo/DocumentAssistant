﻿using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using DocumentAssistantLibrary.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Resources;
using WpfApp.Windows;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties

        private int currentPage = 1;

        private int pageSize = 50;

        private int totalPages;

        private ObservableCollection<DocumentViewModel> documentViewModelsPage = new();

        private DataGridSortingEventArgs? lastSorting;

        DocumentViewModel? selectedDocument;


        List<User> usersList = new();
        public static ObservableCollection<UserViewModel> userViewModelsList = new();

        List<Customer> customersList = new();
        public static ObservableCollection<CustomerViewModel> customerViewModelsList = new();

        List<DocumentType> documentTypesList = new();
        public static ObservableCollection<DocumentTypeViewModel> documentTypeViewModelsList = new();

        List<Language> languagesList = new();
        public static ObservableCollection<LanguageViewModel> languagesViewModelsList = new();

        User loggedUser;

        #endregion

        public MainWindow(User inputUser)
        {
            InitializeComponent();
            loggedUser = inputUser;
            ManageAccessLevel(loggedUser.RoleID);
            UpdateAllLists();
            ConfigurateControls();
            ResetView();
        }

        #region Private methods

        private void ManageAccessLevel(int userLevel)
        {
            switch (userLevel)
            {
                case 1:
                    AddDocumentBtn.IsEnabled = true;
                    EditDocBtn.IsEnabled = true;
                    DeleteDocBtn.IsEnabled = true;
                    MarkAsDoneBtn.IsEnabled = true;
                    ConfirmDoneBtn.IsEnabled = true;
                    AssignDocumentBtn.IsEnabled = true;
                    Menu_ManageUsers.IsEnabled = true;
                    GenerateRandomDocs.Visibility = Visibility.Visible;
                    Menu_ShowStatistics.IsEnabled = true;
                    return;
                case 2:
                    goto case 1;
                case 3:
                    AddDocumentBtn.IsEnabled = true;
                    MarkAsDoneBtn.IsEnabled = true;
                    ConfirmDoneBtn.IsEnabled = true;
                    AssignDocumentBtn.IsEnabled = true;
                    Menu_ShowStatistics.IsEnabled = true;
                    return;
                case 4:
                    AddDocumentBtn.IsEnabled = true;
                    MarkAsDoneBtn.IsEnabled = true;
                    Menu_ShowStatistics.IsEnabled = true;
                    return;
                case 5:
                    return;
                default:
                    MessageBox.Show(Text.UserLevelWarning);
                    this.Close();
                    return;
            }
        }

        private void ConfigurateControls()
        {
            //Assign item sources
            //Main view
            DocGrid.ItemsSource = documentViewModelsPage;
            AssignUserMainMenu_ComboBox.ItemsSource = userViewModelsList;

            //New document grid
            NewDocType_ComboBox.ItemsSource = documentTypeViewModelsList;
            NewDocCustomer_ComboBox.ItemsSource = customerViewModelsList;
            NewDocUser_ComboBox.ItemsSource = userViewModelsList;
            NewDocTargetLang_ComboBox.ItemsSource = languagesViewModelsList;
            NewDocOriginalLang_ComboBox.ItemsSource = languagesViewModelsList;

            //Edit document grid
            EditDocType_ComboBox.ItemsSource = documentTypeViewModelsList;
            EditDocCustomer_ComboBox.ItemsSource = customerViewModelsList;
            EditDocUser_ComboBox.ItemsSource = userViewModelsList;
            EditDocOriginalLang_ComboBox.ItemsSource = languagesViewModelsList;
            EditDocTargetLang_ComboBox.ItemsSource = languagesViewModelsList;

            //Parametrize Calendars
            DeadlineCallendarBlackout.End = DateTime.Now.AddDays(-1);
        }

        private void ResetView()
        {
            DocumentFullGrid.Visibility = Visibility.Visible;

            AssignUserMainMenu_ComboBox.Visibility = Visibility.Collapsed;
            AssignUserMainMenuConfirm_Button.Visibility = Visibility.Collapsed;

            AddDocumentGrid.Visibility = Visibility.Hidden;
            HideNewTypeGrid();
            HideNewCustomerGrid();
            EditDocGrid.Visibility = Visibility.Hidden;
            HideNewLanguageGrid();
        }

        private void HideNewTypeGrid()
        {
            NewDocTypeLabel.Visibility = Visibility.Collapsed;
            NewType_TextBox.Visibility = Visibility.Collapsed;
            ConfirmNewType_Button.Visibility = Visibility.Collapsed;
        }

        private void ShowNewTypeGrid()
        {
            NewDocTypeLabel.Visibility = Visibility.Visible;
            NewType_TextBox.Visibility = Visibility.Visible;
            ConfirmNewType_Button.Visibility = Visibility.Visible;
        }

        private void HideNewCustomerGrid()
        {
            NewCustomerLabel.Visibility = Visibility.Collapsed;
            NewCustomer_TextBox.Visibility = Visibility.Collapsed;
            ConfirmNewCustomer_Button.Visibility = Visibility.Collapsed;
        }

        private void ShowNewCustomerGrid()
        {
            NewCustomerLabel.Visibility = Visibility.Visible;
            NewCustomer_TextBox.Visibility = Visibility.Visible;
            ConfirmNewCustomer_Button.Visibility = Visibility.Visible;
        }

        private void HideNewLanguageGrid()
        {
            NewLanguageLabel.Visibility = Visibility.Collapsed;
            NewLanguage_TextBox.Visibility = Visibility.Collapsed;
            ConfirmNewLanguage_Button.Visibility = Visibility.Collapsed;
        }

        private void ShowNewLanguageGrid()
        {
            NewLanguageLabel.Visibility = Visibility.Visible;
            NewLanguage_TextBox.Visibility = Visibility.Visible;
            ConfirmNewLanguage_Button.Visibility = Visibility.Visible;
        }

        private void UpdateDocumentsListPageNumberText() => DocumentsListPageNumber.Content = $"{Text.Page} {currentPage} {Text.Of} {totalPages}";

        #region Updating of elements lists

        private void UpdateAllLists()
        {
            DocGridSortingChanged();
            UpdateUsersList();
            UpdateCustomersList();
            UpdateDocTypesList();
            UpdateLanguagesList();
        }

        private void UpdateUsersList()
        {
            usersList.Clear();

            using (MainContext context = new MainContext())
                usersList = context.Users.ToList();

            userViewModelsList.Clear();

            foreach (var user in usersList)
                userViewModelsList.Add(new UserViewModel(user));
        }

        private void UpdateCustomersList()
        {
            customersList.Clear();

            using (MainContext context = new MainContext())
                customersList = context.Customers.ToList();

            customerViewModelsList.Clear();

            foreach (var customer in customersList)
                customerViewModelsList.Add(new CustomerViewModel(customer));
        }

        private void UpdateLanguagesList()
        {
            languagesList.Clear();

            using (MainContext context = new MainContext())
                languagesList = context.Languages.ToList();

            languagesViewModelsList.Clear();

            foreach (var language in languagesList)
                languagesViewModelsList.Add(new LanguageViewModel(language));
        }

        private void UpdateDocTypesList()
        {
            documentTypesList.Clear();

            using (MainContext context = new MainContext())
                documentTypesList = context.DocumentTypes.ToList();

            documentTypeViewModelsList.Clear();

            foreach (var documentType in documentTypesList)
                documentTypeViewModelsList.Add(new DocumentTypeViewModel(documentType));
        }

        #endregion

        #endregion

        #region Controls

        private void DocGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDocument = DocGrid.SelectedItem as DocumentViewModel;
            if (selectedDocument is not null)
            {
                if (selectedDocument.IsConfirmed)
                    ConfirmDoneBtn.Content = Text.CancelConfirm;
                else
                    ConfirmDoneBtn.Content = Text.ConfirmDocument;
            }
        }

        private void DocGridSortingChanged(object? sender = null, DataGridSortingEventArgs? e = null)
        {
            ListSortDirection? lastSortingDirection = ListSortDirection.Ascending;

            if (lastSorting is not null)
                lastSortingDirection = lastSorting.Column.SortDirection;

            if (e is null)
                e = lastSorting;
            else
            {
                lastSorting = e;
                currentPage = 1;

                if (e.Column.SortDirection == ListSortDirection.Ascending)
                    lastSortingDirection = ListSortDirection.Descending;
                else if (e.Column.SortDirection == ListSortDirection.Descending)
                    lastSortingDirection = ListSortDirection.Ascending;
            }

            documentViewModelsPage.Clear();

            List<Document> downloadedDocuments = new();

            using (MainContext context = new MainContext())
            {
                totalPages = (int)Math.Ceiling(context.Documents.Count() / (double)pageSize);

                var sortedQuery = context.Documents.OrderBy(s => s.DocumentID);

                if (lastSorting is not null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference - null can be sorted.
                    switch (lastSorting.Column.DisplayIndex)
                    {
                        case 0:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.Name);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.Name);
                            break;
                        case 1:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.OriginalLanguage.LanguageName);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.OriginalLanguage.LanguageName);
                            break;
                        case 2:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.TargetLanguage.LanguageName);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.TargetLanguage.LanguageName);
                            break;
                        case 3:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.DocumentTypes.TypeName);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.DocumentTypes.TypeName);
                            break;
                        case 4:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.SignsSize);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.SignsSize);
                            break;
                        case 5:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.Customers.CustomerName);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.Customers.CustomerName);
                            break;
                        case 6:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.TimeAdded);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.TimeAdded);
                            break;
                        case 7:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.Deadline);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.Deadline);
                            break;
                        case 8:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.Users.Login);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.Users.Login);
                            break;
                        case 9:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.TimeDone);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.TimeDone);
                            break;
                        case 10:
                            if (lastSortingDirection == ListSortDirection.Descending)
                                sortedQuery = context.Documents.OrderByDescending(s => s.IsConfirmed);
                            else
                                sortedQuery = context.Documents.OrderBy(s => s.IsConfirmed);
                            break;
                        default:
                            break;
                    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                downloadedDocuments = sortedQuery
                    .Skip((currentPage - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }

            foreach (Document item in downloadedDocuments)
                documentViewModelsPage.Add(new DocumentViewModel(item));

            UpdateDocumentsListPageNumberText();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage >= totalPages) return;
            currentPage++;
            DocGridSortingChanged();
            UpdateDocumentsListPageNumberText();
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage <= 1) return;
            currentPage--;
            DocGridSortingChanged();
            UpdateDocumentsListPageNumberText();
        }

        #region Menu Bar controls

        private void Menu_ManageUsers_Click(object sender, RoutedEventArgs e)
        {
            var window = new UserManagementWindow();
            window.Show();
        }

        private void Menu_Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void GenerateRandomDocs_Click(object sender, RoutedEventArgs e)
        {
            int numberOfDocs = 1000;

            MessageBoxResult createExampleDocs = MessageBox.Show($"{Text.GenerateRandomDocs} {numberOfDocs}\n{Text.ContinueQuestion}",
                                                                    Text.GenerateRandomDocsBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (createExampleDocs == MessageBoxResult.Yes)
                RandomDataGenerator.GenerateExampleDocuments(numberOfDocs);

            UpdateAllLists();
        }

        private void Menu_ShowStatistics_Click(object sender, RoutedEventArgs e)
        {
            var statisticsWindow = new StatisticsWindow(loggedUser);
            statisticsWindow.Show();
        }

        #endregion

        #region Add New Document

        private void AddDocumentBtn_Click(object sender, RoutedEventArgs e)
        {
            DocumentFullGrid.Visibility = Visibility.Hidden;
            AddDocumentGrid.Visibility = Visibility.Visible;
        }

        private void ConfirmNewDocButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewDocName_TextBox.Text.Trim().Length < 3)
            {
                MessageBox.Show(Text.DocNameLengthWarning);
                return;
            }

            bool sizeParsed = Int32.TryParse(NewDocSize_TextBox.Text, out int docSize);
            if (!sizeParsed)
            {
                MessageBox.Show(Text.DocSizeWarning);
                return;
            }

            if (NewDocType_ComboBox.SelectedItem is null)
            {
                MessageBox.Show(Text.NoDocTypeWarning);
                return;
            }

            if (NewDocCustomer_ComboBox.SelectedItem is null)
            {
                MessageBox.Show(Text.NoClientWarning);
                return;
            }

            if (DeadlineCallendar.SelectedDate is null)
            {
                MessageBox.Show(Text.NoDeadlineWarning);
                return;
            }

            if (NewDocOriginalLang_ComboBox.SelectedItem is null)
            {
                MessageBox.Show(Text.NoSourceLangWarning);
                return;
            }

            if (NewDocTargetLang_ComboBox.SelectedItem is null)
            {
                MessageBox.Show(Text.NoTargetLangWarning);
                return;
            }

            if (NewDocTargetLang_ComboBox.SelectedItem == NewDocOriginalLang_ComboBox.SelectedItem)
            {
                MessageBox.Show(Text.DocSameLanguagesWarning);
                return;
            }

            Document newDocument = new Document
            {
                TimeAdded = DateTime.Now,
                Name = NewDocName_TextBox.Text.Trim(),
                SignsSize = docSize,
                Deadline = (DateTime)DeadlineCallendar.SelectedDate,
                IsConfirmed = false
            };

#pragma warning disable CS8602 // Dereference of a possibly null reference - object existence checked.

            var tempCustomer = NewDocCustomer_ComboBox.SelectedItem as CustomerViewModel;
            newDocument.CustomerID = tempCustomer.CustomerID;

            var tempType = NewDocType_ComboBox.SelectedItem as DocumentTypeViewModel;
            newDocument.TypeID = tempType.TypeID;

            var selectedLanguage = NewDocOriginalLang_ComboBox.SelectedItem as LanguageViewModel;
            newDocument.OriginalLanguageID = selectedLanguage.LanguageID;

            selectedLanguage = NewDocTargetLang_ComboBox.SelectedItem as LanguageViewModel;
            newDocument.TargetLanguageID = selectedLanguage.LanguageID;

            if (NewDocUser_ComboBox.SelectedItem is not null)
            {
                var tempUser = NewDocUser_ComboBox.SelectedItem as UserViewModel;
                newDocument.UserID = tempUser.UserID;
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.

            using (MainContext context = new MainContext())
            {
                context.Documents.Add(newDocument);
                context.SaveChanges();
            }

            DocGridSortingChanged();
            ResetView();
        }

        #region Forms for adding new Language, Customer and DocumentType

        private void NewTypeDocGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            HideNewCustomerGrid();
            ShowNewTypeGrid();
            HideNewLanguageGrid();
        }

        private void ConfirmNewType_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewType_TextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show(Text.DocTypeLengthWarning);
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

            UpdateDocTypesList();
            NewDocType_ComboBox.SelectedItem = documentTypeViewModelsList.Where(x => x.TypeName == newType.TypeName).Single();
            HideNewTypeGrid();
        }

        private void NewCustomerDocGrid_Button_Click(object sender, RoutedEventArgs e)
        {
            HideNewTypeGrid();
            ShowNewCustomerGrid();
            HideNewLanguageGrid();
        }

        private void ConfirmNewCustomer_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewCustomer_TextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show(Text.ClientNameLengthWarning);
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

            UpdateCustomersList();
            NewDocCustomer_ComboBox.SelectedItem = customerViewModelsList.Where(x => x.CustomerName == newCustomer.CustomerName).Single();
            HideNewCustomerGrid();
        }

        private void NewDocAddLanguage_Button_Click(object sender, RoutedEventArgs e)
        {
            HideNewTypeGrid();
            HideNewCustomerGrid();
            ShowNewLanguageGrid();
        }

        private void ConfirmNewLanguage_Button_Click(object sender, RoutedEventArgs e)
        {
            if (NewLanguage_TextBox.Text.Trim().Length < 2)
            {
                MessageBox.Show(Text.LanguageNameLengthWarning);
                return;
            }

            Language newLanguage = new Language
            {
                LanguageName = NewLanguage_TextBox.Text.Trim()
            };

            using (MainContext context = new MainContext())
            {
                context.Languages.Add(newLanguage);
                context.SaveChanges();
            }

            UpdateLanguagesList();
            NewDocOriginalLang_ComboBox.SelectedItem = languagesViewModelsList.Where(x => x.LanguageName == newLanguage.LanguageName).Single();
            HideNewLanguageGrid();
        }
        #endregion

        #endregion

        #region Edit existing document

        private void EditDocBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            DocumentFullGrid.Visibility = Visibility.Hidden;
            EditDocGrid.Visibility = Visibility.Visible;

            EditDocTitleLabel.Content = $"ID: {selectedDocument.DocumentID}, {selectedDocument.Name}";
            EditDocName_TextBox.Text = selectedDocument.Name;
            EditDocSize_TextBox.Text = selectedDocument.signsSize.ToString();
            EditDocType_ComboBox.SelectedItem = documentTypeViewModelsList.Where(x => x.TypeID == selectedDocument.TypeID).Single();
            EditDocCustomer_ComboBox.SelectedItem = customerViewModelsList.Where(x => x.CustomerID == selectedDocument.CustomerID).Single();
            EditDocOriginalLang_ComboBox.SelectedItem = languagesViewModelsList.Where(x => x.LanguageID == selectedDocument.OriginalLanguageID).Single();
            EditDocTargetLang_ComboBox.SelectedItem = languagesViewModelsList.Where(x => x.LanguageID == selectedDocument.TargetLanguageID).Single();
            EditDocDeadlineCallendar.SelectedDate = selectedDocument.Deadline;

            if (selectedDocument.UserID is not null)
                EditDocUser_ComboBox.SelectedItem = userViewModelsList.Where(x => x.UserID == selectedDocument.UserID).Single();

            if (selectedDocument.TimeDone is not null)
                EditDocTimeDoneCallendar.SelectedDate = selectedDocument.TimeDone;

            EditDocGridConfirmed_CheckBox.IsChecked = selectedDocument.IsConfirmed;
        }

        private void ConfirmEditDocButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            var tempSelectedDocType = EditDocType_ComboBox.SelectedItem as DocumentTypeViewModel;
            var tempSelectedDocCustomer = EditDocCustomer_ComboBox.SelectedItem as CustomerViewModel;
            var tempSelectedOriginalLang = EditDocOriginalLang_ComboBox.SelectedItem as LanguageViewModel;
            var tempSelectedTargetLang = EditDocTargetLang_ComboBox.SelectedItem as LanguageViewModel;
            UserViewModel? tempSelectedUser = null;

            if (EditDocUser_ComboBox.SelectedItem is not null)
                tempSelectedUser = EditDocUser_ComboBox.SelectedItem as UserViewModel;

            using (MainContext context = new MainContext())
            {
                bool sizeParsed = Int32.TryParse(EditDocSize_TextBox.Text.Trim(), out int documentSize);
#pragma warning disable CS8602 // Dereference of a possibly null reference - variables can be null
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();
                editedDocument.Name = EditDocName_TextBox.Text.Trim();
                if (sizeParsed)
                    editedDocument.SignsSize = documentSize;
                editedDocument.CustomerID = tempSelectedDocCustomer.CustomerID;
                editedDocument.TypeID = tempSelectedDocType.TypeID;
                editedDocument.IsConfirmed = EditDocGridConfirmed_CheckBox.IsChecked;
                editedDocument.OriginalLanguageID = tempSelectedOriginalLang.LanguageID;
                editedDocument.TargetLanguageID = tempSelectedTargetLang.LanguageID;
#pragma warning restore CS8602 // Dereference of a possibly null reference.


                if (EditDocDeadlineCallendar.SelectedDate is not null)
                    editedDocument.Deadline = (DateTime)EditDocDeadlineCallendar.SelectedDate;

                if (tempSelectedUser is not null)
                    editedDocument.UserID = tempSelectedUser.UserID;

                if (EditDocTimeDoneCallendar.SelectedDate is not null)
                    editedDocument.TimeDone = EditDocTimeDoneCallendar.SelectedDate;

                context.SaveChanges();
            }

            DocGridSortingChanged();
            ResetView();
        }

        #endregion

        #region Other controls from MainMenu

        private void DeleteDocBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            MessageBoxResult deleteDocument = MessageBox.Show(Text.DeleteWarning, Text.DeleteBoxTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (deleteDocument == MessageBoxResult.Yes)
            {
                using (MainContext context = new MainContext())
                {
                    context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).ExecuteDelete();
                    context.SaveChanges();
                }

                DocGridSortingChanged();
            }
            else return;
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

            DocGridSortingChanged();
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
                editedDocument.UserID = selectedUser!.UserID;
                context.SaveChanges();
            }

            DocGridSortingChanged();
        }

        private void ConfirmDoneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDocument is null) return;

            using (MainContext context = new MainContext())
            {
                var editedDocument = context.Documents.Where(x => x.DocumentID == selectedDocument.DocumentID).Single();

                if (editedDocument.IsConfirmed == false || editedDocument.IsConfirmed is null)
                    editedDocument.IsConfirmed = true;
                else
                    editedDocument.IsConfirmed = false;

                context.SaveChanges();
            }

            DocGridSortingChanged();
        }

        #endregion

        private void Callendars_SelectedDatesChanged(object sender, SelectionChangedEventArgs e) => Mouse.Capture(null);

        private void ResetViewButton_Click(object sender, RoutedEventArgs e) => ResetView();

        #endregion
    }
}
