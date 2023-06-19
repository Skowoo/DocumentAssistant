using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Windows;

namespace WpfApp.Models.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentViewModel(Document input)
        {
            DocumentID = input.DocumentID;
            TimeAdded = input.TimeAdded;
            Deadline = input.Deadline;           
            Name = input.Name;
            signsSize = input.signsSize.ToString();
            IsConfirmed = input.IsConfirmed;

            if (input.TimeDone is not null)
            {
                TimeDone = (DateTime)input.TimeDone;
            }

            using (MainContext context = new MainContext())
            {
                if (input.UserID is not null)
                {
                    UserID = context.Users.Where(x => x.UserID == input.UserID).Single().UserID;
                    UserLogin = context.Users.Where(x => x.UserID == input.UserID).Single().Login;
                }

                CustomerName = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single().CustomerName;
                CustomerID = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single().CustomerID;

                TypeID = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single().TypeID;
                TypeName = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single().TypeName;
            }
        }

        public int DocumentID { get; init; }

        public int? UserID { get; init; }

        public int CustomerID { get; init; }

        public int TypeID { get; init; }

        public bool IsConfirmed { get; init; }

        public DateTime TimeAdded { get; init; }

        public DateTime Deadline { get; init; }

        public DateTime? TimeDone { get; init; }

        public string Name { get; init; }

        public string signsSize { get; init; }
        
        public string UserLogin { get; init; }

        public string TypeName { get; init; }

        public string CustomerName { get; init; }

        public bool IsDone => TimeDone is not null;

        public bool IsOverdue => DateTime.Now > Deadline;

        public bool IsCloseToDeadline => DateTime.Now.AddDays(7) > Deadline;

        public override string ToString() => Name;
    }
}
