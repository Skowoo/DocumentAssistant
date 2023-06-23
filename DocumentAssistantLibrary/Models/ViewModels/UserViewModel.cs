namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for User entity in app's database
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Constructor for view model
        /// </summary>
        /// <param name="input"> User on which model should be based </param>
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

        /// <summary>
        /// User unique ID
        /// </summary>
        public int UserID { get; init; }

        /// <summary>
        /// Assigned Role ID
        /// </summary>
        public int RoleID { get; init; }

        /// <summary>
        /// Boolean IsActive - true if user is activated and can log in
        /// </summary>
        public bool IsActive { get; init; }

        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// User's second name
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// User's login
        /// </summary>
        public string Login { get; init; }

        #endregion

        #region Secondary properties

        /// <summary>
        /// string value which represents if User IsActive or not
        /// </summary>
        public string Status { get; init; }

        /// <summary>
        /// Name of assigned Role
        /// </summary>
        public string RoleName { get; init; }

        #endregion

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> string consisting of Users first name and second name </returns>
        public override string ToString() => $"{FirstName} {LastName}";
    }
}
