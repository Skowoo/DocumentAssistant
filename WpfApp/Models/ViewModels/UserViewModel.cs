using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(User input) 
        { 
            userID = input.userID.ToString();
            FirstName = input.FirstName;
            LastName = input.LastName;
            Login = input.Login;
            if (input.IsActive)
                IsActive = "Aktywny";
            else IsActive = "Nieaktywny";

            using MainContext context = new MainContext();
            RoleID = context.Roles.Where(x => x.RoleID == input.RoleID).Single().RoleName;
        }

        public string userID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public string IsActive { get; set; }

        public string RoleID { get; set; }
    }
}
