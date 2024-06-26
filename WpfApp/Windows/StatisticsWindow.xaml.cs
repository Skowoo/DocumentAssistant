﻿using DocumentAssistantLibrary;
using DocumentAssistantLibrary.Classes;
using DocumentAssistantLibrary.Models;
using DocumentAssistantLibrary.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using WpfApp.Resources;

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

            using (MainContext context = new MainContext())
                queriedDocuments = context.Documents.ToList();

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

            var stats = new Statistics(queriedDocuments);

            if (stats.IsValid)
            {
                string? averageDocTranslationTime = stats.AverageTimeToCompleteDoc is null ?
                    Text.NotEnoughTranslatedDocuments :
                    $"{stats.AverageTimeToCompleteDoc} ({Text.CalculationBasedOn} {stats.TranslatedDocumentCount} {Text.Documents})";

                StatisticsTextBlock.Text = $"{Text.NumberOfDocsColon} {stats.DocumentCount}\n" +
                    $"{Text.AverageDocSizeColon} {stats.AverageDocSize}\n" +
                    $"{Text.AverageTranslationTimeColon} {averageDocTranslationTime}\n";
            }
            else
                StatisticsTextBlock.Text = Text.NoDocumentsInQuery;
        }

        #endregion
    }
}
