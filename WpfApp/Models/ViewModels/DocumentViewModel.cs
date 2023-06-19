using System;
using System.Linq;

namespace WpfApp.Models.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentViewModel(Document input)
        {
            DocumentID = input.DocumentID;
            signsSize = input.signsSize;
            Name = input.Name;
            TimeAdded = input.TimeAdded;
            Deadline = input.Deadline;                        
            IsConfirmed = input.IsConfirmed;

            if (input.TimeDone is not null)
            {
                TimeDone = (DateTime)input.TimeDone;
            }

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

        public int DocumentID { get; init; }

        public string Name { get; init; }

        public int signsSize { get; init; }

        public int? UserID { get; init; }

        public int CustomerID { get; init; }

        public int TypeID { get; init; }

        public bool IsConfirmed { get; init; }

        public int OriginalLanguageID { get; init; }

        public int TargetLanguageID { get; init; }

        public DateTime TimeAdded { get; init; }

        public DateTime Deadline { get; init; }

        public DateTime? TimeDone { get; init; }

        #endregion

        #region Secondary properties

        public string? UserLogin { get; init; }

        public string TypeName { get; init; }

        public string CustomerName { get; init; }

        public string OriginalLanguage { get; init; }

        public string TargetLanguage { get; init; }


        public bool IsDone => TimeDone is not null;

        public bool IsOverdue => DateTime.Now > Deadline;

        public bool IsCloseToDeadline => DateTime.Now.AddDays(7) > Deadline;

        #endregion

        public override string ToString() => Name;
    }
}
