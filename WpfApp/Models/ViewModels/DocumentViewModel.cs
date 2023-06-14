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
            TimeAdded = input.TimeAdded.ToString("dd.MM.yyyy");
            Deadline = input.Deadline.ToString("dd.MM.yyyy");
            TimeDone = input.TimeDone.ToString("dd.MM.yyyy");
            Name = input.Name;
            signsSize = input.signsSize.ToString();

            using (MainContext context = new MainContext())
            {
                UserID = context.Users.Where(x => x.UserID == input.UserID).Single().UserID;
                UserLogin = context.Users.Where(x => x.UserID == input.UserID).Single().Login;

                CustomerName = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single().CustomerName;
                CustomerID = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single().CustomerID;

                TypeID = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single().TypeID;
                TypeName = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single().TypeName;
            }
        }

        public int DocumentID { get; init; }

        public int UserID { get; init; }

        public int CustomerID { get; init; }

        public int TypeID { get; init; }

        public string TimeAdded { get; init; }

        public string Deadline { get; init; }

        public string TimeDone { get; init; }

        public string Name { get; init; }

        public string signsSize { get; init; }
        
        public string UserLogin { get; init; }

        public string TypeName { get; init; }

        public string CustomerName { get; init; }
    }
}
