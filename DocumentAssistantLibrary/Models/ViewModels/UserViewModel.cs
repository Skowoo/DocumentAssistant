namespace DocumentAssistantLibrary.Models.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(User input)
        {
            UserID = input.UserID;
            FirstName = input.FirstName;
            LastName = input.LastName;
            Login = input.Login;
            IsActive = input.IsActive;

            if (input.IsActive)
                Status = "Aktywny";
            else
                Status = "Nieaktywny";

            using (MainContext context = new MainContext())
            {
                var selectedRole = context.Roles.Where(x => x.RoleID == input.RoleID).Single();
                RoleID = selectedRole.RoleID;
                RoleName = selectedRole.RoleName;
            };
        }

        #region Core properties

        public int UserID { get; init; }

        public int RoleID { get; init; }

        public bool IsActive { get; init; }

        public string FirstName { get; init; }

        public string LastName { get; init; }

        public string Login { get; init; }

        #endregion

        #region Secondary properties

        public string Status { get; init; }

        public string RoleName { get; init; }

        #endregion

        public override string ToString() => $"{FirstName} {LastName}";
    }
}
