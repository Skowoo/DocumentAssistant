using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models.ViewModels
{
    public class DocumentTypeViewModel
    {
        public DocumentTypeViewModel(DocumentType input) 
        {
            this.TypeID = input.TypeID;
            this.TypeName = input.TypeName;
        }

        public string TypeName { get; init; }

        public int TypeID { get; init; }

        public override string ToString() => TypeName;
    }
}
