namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for Document
    /// </summary>
    public class DocumentViewModel
    {
        /// <summary>
        /// COnstructor for view model
        /// </summary>
        /// <param name="input"> Document on which model should be based </param>
        public DocumentViewModel(Document input)
        {
            DocumentID = input.DocumentID;

            if (input.SignsSize is not null)
                signsSize = (int)input.SignsSize;

            Name = input.Name ??= string.Empty;

            if (input.TimeAdded is not null)
                TimeAdded = (DateTime)input.TimeAdded;

            if (input.Deadline is not null)
                Deadline = (DateTime)input.Deadline;

            if (input.IsConfirmed is not null)
                IsConfirmed = (bool)input.IsConfirmed;

            if (input.TimeDone is not null)
                TimeDone = (DateTime)input.TimeDone;

            using (MainContext context = new MainContext())
            {
                if (input.UserID is not null)
                {
                    var selectedUser = context.Users.Where(x => x.UserID == input.UserID).Single();
                    UserID = selectedUser.UserID;
                    UserLogin = selectedUser.Login;
                }

                var selectedCustomer = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single();
                CustomerName = selectedCustomer.CustomerName;
                CustomerID = selectedCustomer.CustomerID;

                var selectedDocumentType = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single();
                TypeID = selectedDocumentType.TypeID;
                TypeName = selectedDocumentType.TypeName;

                if (input.OriginalLanguageID is not null)
                {
                    var selectedOriginalLanguage = context.Languages.Where(x => x.LanguageID == input.OriginalLanguageID).Single();
                    OriginalLanguageID = selectedOriginalLanguage.LanguageID;
                    OriginalLanguage = selectedOriginalLanguage.LanguageName;
                }

                if (input.TargetLanguageID is not null)
                {
                    var selectedTargetLanguage = context.Languages.Where(x => x.LanguageID == input.TargetLanguageID).Single();
                    TargetLanguageID = selectedTargetLanguage.LanguageID;
                    TargetLanguage = selectedTargetLanguage.LanguageName;
                }
            }
        }

        #region Core properties

        /// <summary>
        /// Unique ID of document
        /// </summary>
        public int DocumentID { get; init; }

        /// <summary>
        /// Document name
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Document size (number of signs)
        /// </summary>
        public int signsSize { get; init; }

        /// <summary>
        /// Nullable ID of user assigned to this document
        /// </summary>
        public int? UserID { get; init; }

        /// <summary>
        /// ID of customer ordering the translation
        /// </summary>
        public int CustomerID { get; init; }

        /// <summary>
        /// ID of document type of this document
        /// </summary>
        public int TypeID { get; init; }

        /// <summary>
        /// Bool which states if document translation is confirmed
        /// </summary>
        public bool IsConfirmed { get; init; }

        /// <summary>
        /// ID of original language of document
        /// </summary>
        public int OriginalLanguageID { get; init; }

        /// <summary>
        /// ID of target language of document
        /// </summary>
        public int TargetLanguageID { get; init; }

        /// <summary>
        /// Time when document was introduced to database
        /// </summary>
        public DateTime TimeAdded { get; init; }

        /// <summary>
        /// Deadline for translation of document
        /// </summary>
        public DateTime Deadline { get; init; }

        /// <summary>
        /// Nullable time when document was marked as done
        /// </summary>
        public DateTime? TimeDone { get; init; }

        #endregion

        #region Secondary properties

        /// <summary>
        /// Nullable login of assigned user
        /// </summary>
        public string? UserLogin { get; init; }

        /// <summary>
        /// Name of assigned document type
        /// </summary>
        public string TypeName { get; init; }

        /// <summary>
        /// Name of assigned customer
        /// </summary>
        public string CustomerName { get; init; }

        /// <summary>
        /// Name of original language of document
        /// </summary>
        public string? OriginalLanguage { get; init; }

        /// <summary>
        /// Name of target language of document
        /// </summary>
        public string? TargetLanguage { get; init; }

        /// <summary>
        /// Boolean value of document is done - based on existence of TimeDone date.
        /// </summary>
        public bool IsDone => TimeDone is not null;

        /// <summary>
        /// Boolean value of document deadline have passed - based on difference between deadline date and current time.
        /// </summary>
        public bool IsOverdue => DateTime.Now > Deadline;

        /// <summary>
        /// Boolean value of document deadline will pass in less than 7 days - based on difference between deadline date and current time.
        /// </summary>
        public bool IsCloseToDeadline => DateTime.Now.AddDays(7) > Deadline;

        #endregion

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> String based on document name </returns>
        public override string ToString() => Name;
    }
}
