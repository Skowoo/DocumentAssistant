using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models.ViewModels
{
    public class DocumentViewModel
    {
        public DocumentViewModel(Document input)
        {
            DocumentID = input.DocumentID.ToString();
            TimeAdded = input.TimeAdded.ToString("dd.MM.yyyy");
            Deadline = input.Deadline.ToString("dd.MM.yyyy");
            TimeDone = input.TimeDone.ToString("dd.MM.yyyy");
            Name = input.Name;
            signsSize = input.signsSize.ToString();

            using (MainContext context = new MainContext())
            {
                UserID = context.Users.Where(x => x.userID == input.UserID).Single().Login;
                CustomerID = context.Customers.Where(x => x.CustomerID == input.CustomerID).Single().CustomerName;
                TypeID = context.DocumentTypes.Where(x => x.TypeID == input.TypeID).Single().TypeName;
            }
        }

        public string DocumentID { get; set; }

        public string TimeAdded { get; set; }

        public string Deadline { get; set; }

        public string TimeDone { get; set; }

        public string Name { get; set; }

        public string signsSize { get; set; }

        public string UserID { get; set; }

        public string TypeID { get; set; }

        public string CustomerID { get; set; }
    }
}
