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
            UserID = input.UserID;
            FirstName = input.FirstName;
            LastName = input.LastName;
            Login = input.Login;
            if (input.IsActive)
                IsActive = "Aktywny";
            else IsActive = "Nieaktywny";

            using MainContext context = new MainContext();
            RoleID = context.Roles.Where(x => x.RoleID == input.RoleID).Single().RoleID;
            RoleName = context.Roles.Where(x => x.RoleID == input.RoleID).Single().RoleName;
        }

        public int UserID { get; init; }

        public int RoleID { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Login { get; init; }

        public string IsActive { get; init; }

        public string RoleName { get; init; }
    }
}
