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
using System.Windows.Shapes;
using DocumentAssistantLibrary.Models;
using DocumentAssistantLibrary.Models.ViewModels;
using DocumentAssistantLibrary.Classes;

namespace WpfApp.Windows
{
    /// <summary>
    /// Interaction logic for StatisticsWindow.xaml
    /// </summary>
    public partial class StatisticsWindow : Window
    {
        public StatisticsWindow(User inputUser)
        {
            InitializeComponent();
            ConfigurateControls();
        }

        #region Private methods

        private void ConfigurateControls()
        {
            TranslatorCustomBox.ItemsSource = MainWindow.userViewModelsList;
            CustomerCustomBox.ItemsSource = MainWindow.customerViewModelsList;
            DocTypeCustomBox.ItemsSource = MainWindow.documentTypeViewModelsList;
            OriginLangCustomBox.ItemsSource = MainWindow.languagesViewModelsList;
            TargetLangCustomBox.ItemsSource = MainWindow.languagesViewModelsList;
        }

        #endregion

        #region Controls

        private void ClearTranslatorBtn_Click(object sender, RoutedEventArgs e) => TranslatorCustomBox.SelectedItem = null;

        private void ClearCustomerBtn_Click(object sender, RoutedEventArgs e) => CustomerCustomBox.SelectedItem = null;

        private void ClearDocTypeBtn_Click(object sender, RoutedEventArgs e) => DocTypeCustomBox.SelectedItem = null;

        private void ClearOriginLangBtn_Click(object sender, RoutedEventArgs e) => OriginLangCustomBox.SelectedItem = null;

        private void ClearTargetLangBtn_Click(object sender, RoutedEventArgs e) => TargetLangCustomBox.SelectedItem = null;

        private void ConfirmQueryButton_Click(object sender, RoutedEventArgs e)
        {
            List<Document> queriedDocuments = new();

            foreach (var doc in MainWindow.documentsList)
                queriedDocuments.Add(doc);

            if (TranslatorCustomBox.SelectedItem is not null)
            {
                var selectedItem = TranslatorCustomBox.SelectedItem as UserViewModel;
                queriedDocuments = queriedDocuments.Where(x => x.UserID == selectedItem.UserID).ToList();
            }

            if (CustomerCustomBox.SelectedItem is not null)
            {
                var selectedItem = CustomerCustomBox.SelectedItem as CustomerViewModel;
                queriedDocuments = queriedDocuments.Where(x => x.CustomerID == selectedItem.CustomerID).ToList();
            }

            if (DocTypeCustomBox.SelectedItem is not null)
            {
                var selectedItem = DocTypeCustomBox.SelectedItem as DocumentTypeViewModel;
                queriedDocuments = queriedDocuments.Where(x => x.TypeID == selectedItem.TypeID).ToList();
            }

            if (OriginLangCustomBox.SelectedItem is not null)
            {
                var selectedItem = OriginLangCustomBox.SelectedItem as LanguageViewModel;
                queriedDocuments = queriedDocuments.Where(x => x.OriginalLanguageID == selectedItem.LanguageID).ToList();
            }

            if (TargetLangCustomBox.SelectedItem is not null)
            {
                var selectedItem = TargetLangCustomBox.SelectedItem as LanguageViewModel;
                queriedDocuments = queriedDocuments.Where(x => x.TargetLanguageID == selectedItem.LanguageID).ToList();
            }

            StatisticsTextBlock.Text = StatisticsCalculator.GenerateStatsString(queriedDocuments);
        }

        #endregion
    }
}
