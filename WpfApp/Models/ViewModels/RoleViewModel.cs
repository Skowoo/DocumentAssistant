using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Models.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel(Role input) 
        { 
            this.RoleID = input.RoleID;
            this.RoleName = input.RoleName;
        }

        public int RoleID { get; init; }

        public string RoleName { get; init; }

        public override string ToString() => RoleName;
    }
}
