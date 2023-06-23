namespace DocumentAssistantLibrary.Models.ViewModels
{
    /// <summary>
    /// View model for Role entity in app's database
    /// </summary>
    public class RoleViewModel
    {
        /// <summary>
        /// Constructor for view model
        /// </summary>
        /// <param name="input"> Role on which model should be based </param>
        public RoleViewModel(Role input)
        {
            this.RoleID = input.RoleID;
            this.RoleName = input.RoleName;
        }

        /// <summary>
        /// Role unique ID
        /// </summary>
        public int RoleID { get; init; }

        /// <summary>
        /// Role name
        /// </summary>
        public string RoleName { get; init; }

        /// <summary>
        /// ToString method
        /// </summary>
        /// <returns> string consisting of Role name </returns>
        public override string ToString() => RoleName;
    }
}
